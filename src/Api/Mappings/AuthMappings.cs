using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Common;
using Contracts.Features.Auth;
using Contracts.Features.Users;
using Mapster;

namespace Api.Mappings;

public class AuthMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<LoginRequest, LoginCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<AuthResult, UserResponse>()
            .Map(dest => dest, src => src.UserResult);
    }
}
