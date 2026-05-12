using Domain.Entities;

namespace Application.Features.Categories;

public static class CategoryExtension
{
    public static CategoryDto ToDto(this Category category)
        => new(
            category.Id,
            category.UserId,
            category.Name,
            category.IsDefault,
            category.Keywords.Select(k => k.Value).ToList(),
            category.CreatedAt
        );
}