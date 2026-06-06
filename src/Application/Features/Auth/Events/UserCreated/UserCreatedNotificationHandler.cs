using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auth.Events.UserCreated;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public UserCreatedNotificationHandler(IUnitOfWork uow, ICategoryRepository categoryRepo)
    {
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        var defaultCategory = Category.Create(
            notification.UserId,
            "Default",   // TODO: Custom name??
            true
        ).Value;    // We assume correct parameters, because all parameters are created & verified by the program

        this._categoryRepo.Add(defaultCategory);
        await this._uow.SaveChangesAsync(cancellationToken);
    }
}
