using Application.Features.Transactions.Commands.CreateTransaction;
using Contracts.Features.Transactions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public TransactionController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("")]
    public async Task<ActionResult<TransactionDetailResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var command = _mapper.Map<CreateTransactionCommand>(createRequest);
        var result = await _mediator.Send(command);

        return result.Match<ActionResult<TransactionDetailResponse>>(
            dto => Ok(_mapper.Map<TransactionDetailResponse>(dto)),
            error => Problem(error.GetType().FullName)  // TODO: Custom error response
        );
    }
}