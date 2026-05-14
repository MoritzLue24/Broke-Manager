using Application.Features.Transactions.Commands.CreateTransaction;
using Contracts.Features.Transactions;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("")]
    public async Task<ActionResult<TransactionDetailResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var result = await _mediator.Send(new CreateTransactionCommand(
            createRequest.UserId,
            createRequest.CategoryId,
            createRequest.Amount,
            Enum.Parse<TransactionType>(createRequest.Type),
            createRequest.Date,
            createRequest.Title,
            createRequest.Description,
            createRequest.CounterParty
        ));

        return result.Match<ActionResult<TransactionDetailResponse>>(
            dto => Ok(new TransactionDetailResponse(
                dto.Id,
                dto.UserId,
                dto.CategoryId,
                dto.CategorySource.ToString(),
                dto.Amount,
                dto.Type.ToString(),
                dto.Date,
                dto.Title,
                dto.Description,
                dto.CounterParty,
                dto.CreatedAt
            )),
            error => Problem(error.GetType().FullName)
        ); 
    }
}