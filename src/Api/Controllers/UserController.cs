using Api.Errors;
using Application.Features.Users.Commands.ChangePassword;
using Application.Features.Users.Commands.ChangeRole;
using Application.Features.Users.Commands.DeleteCurrentUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateCurrentUser;
using Application.Features.Users.Queries.GetAllUsers;
using Application.Features.Users.Queries.GetCurrentUser;
using Application.Features.Users.Queries.GetUser;
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
    public async Task<ActionResult<UserResponse>> GetMe()
    {
        var query = new GetCurrentUserQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<UserResponse>>(
            userResult => this.Ok(this._mapper.Map<UserResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPatch("me")]
    public async Task<ActionResult<UserResponse>> UpdateMe(
        [FromBody] UpdateMeRequest updateRequest)
    {
        var command = this._mapper.Map<UpdateCurrentUserCommand>(updateRequest);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserResponse>>(
            userResult => this.Ok(this._mapper.Map<UserResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

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

    // Admin Endpoints

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var query = new GetAllUsersQuery();
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<List<UserResponse>>>(
            userResults => this.Ok(userResults.Select(userResult => this._mapper.Map<UserResponse>(userResult))),
            errors => errors.ToProblem(this)
        );
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserResponse>> GetById(
        [FromRoute] Guid userId)
    {
        var query = new GetUserQuery(userId);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<UserResponse>>(
            userResult => this.Ok(this._mapper.Map<UserResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPatch("{userId}/change-role")]
    public async Task<ActionResult<UserResponse>> ChangeRole(
        [FromRoute] Guid userId,
        [FromBody] ChangeRoleRequest changeRoleRequest)
    {
        var command = this._mapper.Map<ChangeRoleCommand>((userId, changeRoleRequest));
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserResponse>>(
            userResult => this.Ok(this._mapper.Map<UserResponse>(userResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteById(
        [FromRoute] Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            unit => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }
}
