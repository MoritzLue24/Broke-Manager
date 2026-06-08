using Api.Errors;
using Application.Features.Users.Commands.UpdateCurrentUser;
using Application.Features.Users.Queries.GetCurrentUser;
using Contracts.Features.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDetailResponse>> GetMe()
    {
        var query = new GetCurrentUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<UserDetailResponse>>(
            userResult => this.Ok(this._mapper.Map<UserDetailResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPatch("me")]
    public async Task<ActionResult<UserDetailResponse>> UpdateMe(
        [FromBody] UpdateMeRequest updateRequest)
    {
        var command = this._mapper.Map<UpdateCurrentUserCommand>(updateRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserDetailResponse>>(
            userResult => this.Ok(this._mapper.Map<UserDetailResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

    /*
    [HttpPatch("me/change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var command = this._mapper.Map<ChangePasswordCommand>(changePasswordRequest);
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            userResult => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }


    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var command = new DeleteCurrentUserCommand();
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            unit => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }
    */
}
