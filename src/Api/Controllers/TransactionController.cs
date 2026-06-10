using Api.Errors;
using Application.Features.Transactions.Commands.CreateTransaction;
using Application.Features.Transactions.Commands.DeleteTransaction;
using Application.Features.Transactions.Commands.UpdateTransaction;
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
    public async Task<ActionResult<List<TransactionResponse>>> GetAllByUser()
    {
        var query = new GetTransactionsByUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<List<TransactionResponse>>>(
            results => this.Ok(results.Select(transactionResult => this._mapper.Map<TransactionResponse>(transactionResult))),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("{transactionId}")]
    public async Task<ActionResult<TransactionResponse>> GetById(
        [FromRoute] Guid transactionId)
    {
        var query = new GetTransactionQuery(transactionId);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<TransactionResponse>>(
            transactionResult => this.Ok(this._mapper.Map<TransactionResponse>(transactionResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> Create(
        [FromBody] CreateTransactionRequest createRequest)
    {
        var command = this._mapper.Map<CreateTransactionCommand>(createRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<TransactionResponse>>(
            // FIXME: Change to CreatedAtAction?
            transactionResult => this.Created(string.Empty, this._mapper.Map<TransactionResponse>(transactionResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPatch("{transactionId}")]
    public async Task<ActionResult<TransactionResponse>> Update(
        [FromRoute] Guid transactionId,
        [FromBody] UpdateTransactionRequest updateRequest)
    {
        var command = this._mapper.Map<UpdateTransactionCommand>((transactionId, updateRequest));
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<TransactionResponse>>(
            transactionResult => this.Ok(this._mapper.Map<TransactionResponse>(transactionResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid transactionId)
    {
        var command = new DeleteTransactionCommand(transactionId);
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            unit => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }
}
