using Application.Features.Analytics.Contracts;
using Application.Features.Analytics.Queries.Summary;
using Contracts.Features.Analytics.Requests;
using Contracts.Features.Analytics.Responses;
using Mapster;

namespace Api.Mappings;

public class AnalyticsMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AnalyticsPeriodRequest, AnalyticsPeriod>()
            .Map(dest => dest, src => src);

        config.NewConfig<AnalyticsPeriodRequest, SummaryQuery>()
            .Map(dest => dest.Period, src => src);

        config.NewConfig<SummaryResult, SummaryResponse>()
            .Map(dest => dest, src => src);
    }
}
