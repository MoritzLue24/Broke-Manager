using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Common;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterCommandHandler(
        IUnitOfWork uow,
        IUserRepository userRepo,
        IHasher hasher,
        IJwtTokenGenerator tokenGenerator)
    {
        this._uow = uow;
        this._userRepo = userRepo;
        this._hasher = hasher;
        this._tokenGenerator = tokenGenerator;
    }

    public async Task<Result<RegisterResult>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await this._userRepo.EmailExistsAsync(request.Email, cancellationToken))
            return new UserAlreadyExistsError();

        var emailRes = Email.Create(request.Email);
        if (!emailRes.Success)
            return emailRes.Cast<RegisterResult>();

        var hashRes = Hash.Create(this._hasher.Hash(request.Password));
        if (!hashRes.Success)
            return hashRes.Cast<RegisterResult>();

        var domainResult = User.Create(
            emailRes.Value,
            hashRes.Value
        );

        if (!domainResult.Success)
            return domainResult.Cast<RegisterResult>();

        var user = domainResult.Value;
        this._userRepo.Add(user);
        await this._uow.SaveChangesAsync(cancellationToken);

        // TODO: Create default-category
        // TODO: Handle multiple roles
        var token = this._tokenGenerator.GenToken(domainResult.Value.Id, [domainResult.Value.Role]);

        return new RegisterResult(
            user.Id,
            user.Email.Value,
            user.Role,
            user.CreatedAt,
            token
        );
    }
}
