using Application.Common;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Errors;

public static class ErrorExtension
{
    public static ObjectResult ToProblem(this Error error, ControllerBase controller)
    {
        var env = controller.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();

        return error switch
        {
            ValidationError validationError => controller.Problem(
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: $"{validationError.Property}: '{validationError.Message}'"
            ),
            UnauthorizedError => controller.Problem(
                type: ErrorTypes.Unauthorized,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Unauthorized"
            ),
            ForbiddenError => controller.Problem(
                type: ErrorTypes.Forbidden,
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden"
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
                title: env.IsDevelopment()
                    ? error.GetType().ToString()
                    : "An internal server error occured.",
                detail: env.IsDevelopment()
                    ? $"This error was not handled, please fix."
                    : null
            )
        };
    }

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
