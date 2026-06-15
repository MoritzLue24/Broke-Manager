using Api.Errors;
using Application.Features.Categories.Commands.AddCategoryRule;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.RemoveCategoryRule;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GetCategoriesByUser;
using Application.Features.Categories.Queries.GetCategory;
using Contracts.Features.Categories.Requests;
using Contracts.Features.Categories.Responses;
using Contracts.Features.AutoAssign.Requests;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoryController(IMediator mediator, IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllByUser()
    {
        var query = new GetCategoriesByUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<List<CategoryResponse>>>(
            categoryResults => this.Ok(categoryResults.Select(categoryResult => this._mapper.Map<CategoryResponse>(categoryResult))),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryResponse>> GetById(
        [FromRoute] Guid categoryId)
    {
        var query = new GetCategoryQuery(categoryId);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<CategoryResponse>>(
            categoryResult => this.Ok(this._mapper.Map<CategoryResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(
        [FromBody] CreateCategoryRequest createRequest)
    {
        var command = this._mapper.Map<CreateCategoryCommand>(createRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<CategoryResponse>>(
            // FIXME: Change to CreatedAtAction?
            categoryResult => this.Created(string.Empty, this._mapper.Map<CategoryResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPatch("{categoryId}")]
    public async Task<ActionResult<CategoryResponse>> Update(
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryRequest updateRequest)
    {
        var command = this._mapper.Map<UpdateCategoryCommand>((categoryId, updateRequest));
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<CategoryResponse>>(
            categoryResult => this.Ok(this._mapper.Map<CategoryResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost("{categoryId}/rules")]
    public async Task<ActionResult<CategoryResponse>> AddRule(
        [FromRoute] Guid categoryId,
        [FromBody] AddRuleRequest addRuleRequest)
    {
        // Manual mapping because mapping may be different on categories & standing orders
        var command = new AddCategoryRuleCommand(categoryId, addRuleRequest.Keyword);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<CategoryResponse>>(
            categoryResult => this.Ok(this._mapper.Map<CategoryResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpDelete("{categoryId}/rules/{ruleId}")]
    public async Task<IActionResult> RemoveRule(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid ruleId)
    {
        var command = new RemoveCategoryRuleCommand(categoryId, ruleId);
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            categoryResult => this.Ok(this._mapper.Map<CategoryResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            unit => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }
}
