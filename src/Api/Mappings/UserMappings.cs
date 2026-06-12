using Application.Features.Users.Commands.ChangePassword;
using Application.Features.Users.Commands.ChangeRole;
using Application.Features.Users.Commands.UpdateCurrentUser;
using Application.Features.Users.Contracts;
using Contracts.Features.Users;
using Mapster;

namespace Api.Mappings;

public class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, UserResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<UpdateMeRequest, UpdateCurrentUserCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<ChangePasswordRequest, ChangePasswordCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(Guid userId, ChangeRoleRequest request), ChangeRoleCommand>()
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest, src => src.request);
    }
}
