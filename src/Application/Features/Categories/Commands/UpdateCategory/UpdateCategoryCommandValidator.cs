using FluentValidation;

namespace Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.Name is not null);
    }
}
