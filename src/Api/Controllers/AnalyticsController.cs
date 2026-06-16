using Api.Errors;
using Application.Features.Analytics.Queries.CategoryBreakdown;
using Application.Features.Analytics.Queries.Summary;
using Contracts.Features.Analytics.Requests;
using Contracts.Features.Analytics.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AnalyticsController(
        IMediator mediator,
        IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<SummaryResponse>> Summary(
        [FromQuery] AnalyticsPeriodRequest periodRequest)
    {
        var query = this._mapper.Map<SummaryQuery>(periodRequest);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<SummaryResponse>>(
            summaryResult => this.Ok(this._mapper.Map<SummaryResponse>(summaryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("category-breakdown")]
    public async Task<ActionResult<CategoryBreakdownResponse>> CategoryBreakdown(
        [FromQuery] AnalyticsPeriodRequest periodRequest)
    {
        var query = this._mapper.Map<CategoryBreakdownQuery>(periodRequest);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<CategoryBreakdownResponse>>(
            categoryBreakdownResults => this.Ok(categoryBreakdownResults.Select(r
                => this._mapper.Map<CategoryBreakdownResponse>(r))),
            errors => errors.ToProblem(this)
        );
    }
}
