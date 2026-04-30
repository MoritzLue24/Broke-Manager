using System.Net.Mail;
using Domain.Common;

namespace Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
        => Value = value;

    public static DomainResult<Email> Create(string value)
    {
        MailAddress email;
        try
        {
            email = new MailAddress(value);
        }
        catch (Exception)
        {
            return DomainResult<Email>.Fail(DomainErrorCode.InvalidEmailFormat);
        }
        return DomainResult<Email>.Ok(new Email(email.Address));
    }
}
