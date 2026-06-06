using Api.Errors;
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Queries.GetCategoriesByUser;
using Application.Features.Categories.Queries.GetCategory;
using Contracts.Features.Categories;
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
    public async Task<ActionResult<List<CategoryDetailResponse>>> GetAllByUser()
    {
        var query = new GetCategoriesByUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<List<CategoryDetailResponse>>>(
            categoryResult => this.Ok(categoryResult.Select(dto => this._mapper.Map<CategoryDetailResponse>(dto))),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDetailResponse>> GetById(
        [FromRoute] Guid categoryId)
    {
        var query = new GetCategoryQuery(categoryId);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<CategoryDetailResponse>>(
            categoryResult => this.Ok(this._mapper.Map<CategoryDetailResponse>(categoryResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDetailResponse>> Create(
        [FromBody] CreateCategoryRequest createRequest)
    {
        var command = this._mapper.Map<CreateCategoryCommand>(createRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<CategoryDetailResponse>>(
            // FIXME: Change to CreatedAtAction?
            categoryResult => this.Created(string.Empty, this._mapper.Map<CategoryDetailResponse>(categoryResult)),
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
