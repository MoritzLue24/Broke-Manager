using Contracts.Features.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    [HttpPost("")]
    public ActionResult<TransactionDetailResponse> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        return Ok(new TransactionDetailResponse(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "MANUAL",
            createRequest.Amount,
            createRequest.Type,
            createRequest.Date,
            createRequest.Title,
            createRequest.Description,
            createRequest.CounterParty,
            DateTime.UtcNow
        ));
    }
}