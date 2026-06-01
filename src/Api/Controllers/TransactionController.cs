using Api.Errors;
using Application.Features.Transactions.Commands.CreateTransaction;
using Application.Features.Transactions.Queries.GetTransaction;
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
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionDetailResponse>>> GetAllByUser()
    {
        var query = new GetTransactionsByUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<List<TransactionDetailResponse>>>(
            dtos => this.Ok(dtos.Select(dto => this._mapper.Map<TransactionDetailResponse>(dto))),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("{transactionId}")]
    public async Task<ActionResult<TransactionDetailResponse>> GetById(
        [FromRoute] Guid transactionId)
    {
        var query = new GetTransactionQuery(transactionId);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<TransactionDetailResponse>>(
            dto => this.Ok(this._mapper.Map<TransactionDetailResponse>(dto)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDetailResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var command = this._mapper.Map<CreateTransactionCommand>(createRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<TransactionDetailResponse>>(
            // FIXME: Change to CreatedAtAction?
            dto => this.Created(string.Empty, this._mapper.Map<TransactionDetailResponse>(dto)),
            errors => errors.ToProblem(this)
        );
    }
}
