using Application.Features.MatchingRules.Contracts;
using Contracts.Features.MatchingRules;
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
