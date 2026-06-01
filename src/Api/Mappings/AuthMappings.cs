using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries.Login;
using Contracts.Features.Auth;
using Mapster;

namespace Api.Mappings;

public class AuthMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<LoginRequest, LoginQuery>()
            .Map(dest => dest, src => src);
    }
}
