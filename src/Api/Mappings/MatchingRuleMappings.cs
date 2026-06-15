using Application.Features.AutoAssign.Contracts;
using Contracts.Features.AutoAssign.Responses;
using Mapster;

namespace Api.Mappings;

public class MatchingRuleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MatchingRuleResult, MatchingRuleResponse>()
            .Map(dest => dest, src => src);
    }
}
