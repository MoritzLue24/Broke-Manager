using Application.Common.Interfaces.Persistence;
using Application.Features.Auth.Interfaces;
using Application.Features.Users.Events.EmailChanged;
using Application.Features.Users.Events.PasswordChanged;
using Application.Features.Users.Events.RoleChanged;
using MediatR;

namespace Application.Common.Events.Handlers;

public class UserSecurityChangedNotificationHandler
    : INotificationHandler<EmailChangedNotification>,
        INotificationHandler<PasswordChangedNotification>,
        INotificationHandler<RoleChangedNotification>
{
    private readonly IUnitOfWork _uow;
    private readonly ISessionRepository _sessionRepo;

    public UserSecurityChangedNotificationHandler(
        IUnitOfWork uow,
        ISessionRepository sessionRepo)
    {
        this._uow = uow;
        this._sessionRepo = sessionRepo;
    }

    private async Task RevokeSessionsAsync(Guid userId, CancellationToken ct)
    {
        await this._sessionRepo.DeleteAllByUserAsync(userId, ct);
        await this._uow.SaveChangesAsync(ct);
    }

    public async Task Handle(EmailChangedNotification notification, CancellationToken ct)
        => await this.RevokeSessionsAsync(notification.UserId, ct);

    public async Task Handle(PasswordChangedNotification notification, CancellationToken ct)
        => await this.RevokeSessionsAsync(notification.UserId, ct);

    public async Task Handle(RoleChangedNotification notification, CancellationToken ct)
        => await this.RevokeSessionsAsync(notification.UserId, ct);
}
