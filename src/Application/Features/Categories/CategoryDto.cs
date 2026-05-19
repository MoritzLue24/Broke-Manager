namespace Application.Features.Categories;

public record CategoryDto(
    Guid Id,
    Guid UserId,
    string Name,
    bool IsDefault,
    List<string> Keywords,
    DateTime CreatedAt
);
