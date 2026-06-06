using FluentValidation;

namespace Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        this.RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        this.RuleFor(x => x.Keywords)
            .ForEach(x => x
                .NotEmpty()
                .MaximumLength(255)
            );
    }
}
