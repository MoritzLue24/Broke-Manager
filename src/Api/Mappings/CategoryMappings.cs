using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Common;
using Contracts.Features.Categories;
using Mapster;

namespace Api.Mappings;

public class CategoryMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CategoryResult, CategoryDetailResponse>()
            .Map(dest => dest, src => src);

        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(Guid id, UpdateCategoryRequest request), UpdateCategoryCommand>()
            .Map(dest => dest.CategoryId, src => src.id)
            .Map(dest => dest, src => src.request);
    }
}
