using Api.Errors;
using Application.Features.Transactions.Commands.CreateTransaction;
using Application.Features.Transactions.Queries.GetTransactionsByUser;
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

    [HttpGet]
    public async Task<ActionResult<List<TransactionDetailResponse>>> GetAllByUser()
    {
        var query = new GetTransactionsByUserQuery(Guid.NewGuid());
        var result = await _mediator.Send(query);

        return result.Match<ActionResult<List<TransactionDetailResponse>>>(
            dtos => Ok(dtos.Select(dto => _mapper.Map<TransactionDetailResponse>(dto))),
            error => error.ToProblem(this)
        );
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDetailResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var command = _mapper.Map<CreateTransactionCommand>((createRequest, Guid.NewGuid()));
        var result = await _mediator.Send(command);

        return result.Match<ActionResult<TransactionDetailResponse>>(
            dto => Ok(_mapper.Map<TransactionDetailResponse>(dto)),
            error => error.ToProblem(this)
        );
    }
}