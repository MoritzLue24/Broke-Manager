using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Users.Common;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserResult>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;

    public RegisterCommandHandler(
        IUnitOfWork uow,
        IUserRepository userRepo,
        IHasher hasher)
    {
        this._uow = uow;
        this._userRepo = userRepo;
        this._hasher = hasher;
    }

    public async Task<Result<UserResult>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await this._userRepo.EmailExistsAsync(request.Email, cancellationToken))
            return new EmailAlreadyRegisteredError();

        var emailRes = Email.Create(request.Email);
        if (!emailRes.Success)
            return emailRes.Cast<UserResult>();

        var hashRes = Hash.Create(this._hasher.Hash(request.Password));
        if (!hashRes.Success)
            return hashRes.Cast<UserResult>();

        var domainResult = User.Create(
            emailRes.Value,
            hashRes.Value
        );

        if (!domainResult.Success)
            return domainResult.Cast<UserResult>();

        var user = domainResult.Value;
        this._userRepo.Add(user);
        await this._uow.SaveChangesAsync(cancellationToken);

        return user.ToResult();
    }
}
