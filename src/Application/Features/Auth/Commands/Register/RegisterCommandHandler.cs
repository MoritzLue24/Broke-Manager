using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Common;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResult>>
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

    public async Task<Result<AuthResult>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await this._userRepo.EmailExistsAsync(request.Email, cancellationToken))
            return new UserAlreadyExistsError();

        var emailRes = Email.Create(request.Email);
        if (!emailRes.Success)
            return emailRes.Cast<AuthResult>();

        var hashRes = Hash.Create(this._hasher.Hash(request.Password));
        if (!hashRes.Success)
            return hashRes.Cast<AuthResult>();

        var domainResult = User.Create(
            emailRes.Value,
            hashRes.Value
        );

        if (!domainResult.Success)
            return domainResult.Cast<AuthResult>();

        this._userRepo.Add(domainResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);

        // TODO: Create default-category

        var token = this._tokenGenerator.GenToken(domainResult.Value.Id, domainResult.Value.Role);
        return new AuthResult(domainResult.Value.Id, token);
    }
}
