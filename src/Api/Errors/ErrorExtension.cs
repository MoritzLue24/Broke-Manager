using Application.Common;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Errors;

public static class ErrorExtension
{
    public static ObjectResult ToProblem(this Error error, ControllerBase controller)
        => error switch
        {
            ValidationError validationError => controller.Problem(
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: $"{validationError.Property}: '{validationError.Message}'"
            ),
            CategoryNotFoundError => controller.Problem(
                type: ErrorTypes.CategoryNotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "Category not found."
            ),
            DefaultCategoryNotFoundError => controller.Problem(
                type: ErrorTypes.DefaultCategoryNotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "Default category not found."
            ),
            _ => controller.Problem(
                type: ErrorTypes.Internal,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An internal server error occured."
            )
        };

    public static ObjectResult ToProblem(this IEnumerable<Error> errors, ControllerBase controller)
    {
        if (errors.All(e => e is ValidationError))
        {
            foreach (ValidationError error in errors.Cast<ValidationError>())
                controller.ModelState.AddModelError(error.Property, error.Message);
            return (ObjectResult)controller.ValidationProblem(
                modelStateDictionary: controller.ModelState,
                type: ErrorTypes.Validation,
                title: "One or more validation errors occurred."
            );
        }
        if (!errors.Any())
            throw new InvalidOperationException("Cannot convert errors to ObjectResult if there are no errors.");
        return errors.First().ToProblem(controller);
    }
}