using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Contracts;
using Contracts.Features.Categories.Responses;
using Contracts.Features.Categories.Requests;
using Mapster;

namespace Api.Mappings;

public class CategoryMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CategoryResult, CategoryResponse>()
            .Map(dest => dest.MatchingRules, src => src.MatchingRules)  // Wird automatisch gemappt?
            .Map(dest => dest, src => src);

        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(Guid id, UpdateCategoryRequest request), UpdateCategoryCommand>()
            .Map(dest => dest.CategoryId, src => src.id)
            .Map(dest => dest, src => src.request);
    }
}
