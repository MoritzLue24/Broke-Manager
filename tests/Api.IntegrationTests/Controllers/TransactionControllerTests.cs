using System.Net;
using System.Net.Http.Json;
using Api.Errors;
using Api.IntegrationTests.TestInfrastructure.Controllers;
using Contracts.Features.Transactions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.IntegrationTests.Controllers;

public class TransactionControllerTests : BaseTest
{
    public TransactionControllerTests(WebAppFactory factory) : base(factory)
    { }

    [Fact]
    public async void CreateTransaction_ShouldReturnDetail_WhenAuthenticatedAndExistingCategoryGiven()
    {
        // Setup
        var user = User.Create(
            Email.Create("some@e.mail").Value,
            Hash.Create("aspdojqwpjodq").Value
        ).Value;
        var category = Category.Create(
            user.Id,
            "essen",
            false
        ).Value;

        this.Db.Add(user);
        this.Db.Add(category);
        await this.Db.SaveChangesAsync();

        var request = new CreateTransactionRequest(
            category.Id,
            20,
            "Expense",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "title",
            "desc",
            "counterparty"
        );

        // Execute
        this.Client.DefaultRequestHeaders.Add(
            "Cookie",
            $"access_token={this.JwtTokenGenerator.GenToken(user.Id, [user.Role])}"
        );
        var response = await this.Client.PostAsJsonAsync("/transactions", request);
        var transactionDetail = await response.Content.ReadFromJsonAsync<TransactionResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(transactionDetail);
        Assert.Equal(category.Id, transactionDetail.CategoryId);
        Assert.Equal("Manual", transactionDetail.CategorySource);
        Assert.Equal("Expense", transactionDetail.Type);
        Assert.True(await this.Db.Transactions.AnyAsync(t => t.Id == transactionDetail.Id));
    }

    [Fact]
    public async void CreateTransaction_ShouldReturnUnauthorized_WhenNotAuthenticated()
    {
        // Setup
        var request = new CreateTransactionRequest(
            Guid.NewGuid(),
            20,
            "Expense",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "title",
            "desc",
            "counterparty"
        );

        // Execute
        var response = await this.Client.PostAsJsonAsync("/transactions", request);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(ErrorTypes.Unauthorized, problemDetails?.Type);
        Assert.Equal((int)HttpStatusCode.Unauthorized, problemDetails?.Status);
        Assert.False(this.Db.Transactions.Any());
    }
}
