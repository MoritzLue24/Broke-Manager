using System.Net.Mail;

using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        this.Value = value;
    }

    public static Result<Email> Create(string value)
    {
        MailAddress email;
        try
        {
            email = new MailAddress(value);
        }
        catch (Exception)
        {
            return new InvalidEmailFormatError();
        }
        return new Email(email.Address);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
        => [this.Value];
}
