using Domain.Events.Categories;
using MediatR;

namespace Application.Features.Categories.Events.CategoryDeleted;

public record CategoryDeletedNotification(
    Guid CategoryId
) : INotification;

public static class CategoryDeletedExtension
{
    public static CategoryDeletedNotification ToNotification(this CategoryDeletedEvent e)
        => new(e.CategoryId);
}
