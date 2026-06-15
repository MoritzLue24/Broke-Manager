using Application.Features.Transactions.Contracts;
using FluentValidation;

namespace Application.Features.AutoAssign.Commands.AutoAssign;

public class AutoAssignCommandValidator : AbstractValidator<AutoAssignCommand>
{
    public AutoAssignCommandValidator()
    {
        this.RuleFor(x => x.Filter)
            .NotNull()
            .SetValidator(new TransactionFilterValidator());

        this.RuleFor(x => x.UseCategoryIds);

        this.RuleFor(x => x.OverwriteManual)
            .NotNull();
    }
}
