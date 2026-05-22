using Domain.Entities;

namespace Application.Features.Categories.Common;

public record CategoryResult(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    List<string> Keywords,
    DateTime CreatedAt
);

public static class CategoryExtension
{
    public static CategoryResult ToDto(this Category category)
        => new(
            category.Id,
            category.UserId,
            category.Name,
            category.IsDefault,
            category.Keywords.Select(k => k.Value).ToList(),
            category.CreatedAt
        );
}
