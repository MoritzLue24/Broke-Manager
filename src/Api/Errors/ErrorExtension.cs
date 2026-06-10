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
            // Common
            ValidationError validationError => controller.Problem(
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: $"{validationError.Property}: '{validationError.Message}'"
            ),
            // InvalidGuidError is considered a internal error -> handeled by default case

            // Auth
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
            UserAlreadyExistsError => controller.Problem(
                type: ErrorTypes.Duplicate,
                statusCode: StatusCodes.Status409Conflict,
                title: "Email already registered"
            ),
            InvalidCredentialsError => controller.Problem(
                type: ErrorTypes.InvalidCredentials,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Invalid email or password"
            ),
            UserNoLongerExistsError => controller.Problem(
                type: ErrorTypes.TokenInvalid,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "User no longer exists"
            ),

            // Users
            UserNotFoundError => controller.Problem(
                type: ErrorTypes.NotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "User not found"
            ),
            RoleAlreadyCurrentRoleError => controller.Problem(
                type: ErrorTypes.PropertyAlreadyAssigned,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Given role is already the current role"
            ),
            InvalidEmailFormatError => controller.Problem(
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid email format"
            ),
            // InvalidHashFormatError is considered a internal error -> handled by default case 

            // Categories
            CategoryNotFoundError => controller.Problem(
                type: ErrorTypes.NotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "Category not found."
            ),
            CategoryNameAlreadyExistsError => controller.Problem(
                type: ErrorTypes.Duplicate,
                statusCode: StatusCodes.Status409Conflict,
                title: "Category with given name already exists"
            ),
            EmptyCategoryNameError => controller.Problem(   // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Category name is empty"
            ),
            CategoryIsDefaultError => controller.Problem(
                type: ErrorTypes.CategoryIsDefault,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Cannot execute this action on the default category"
            ),

            // Rules
            // DuplicateRuleError should not happen -> internal server error 
            RuleNotFoundError => controller.Problem(    // Handeled by Validator -> should not be used
                type: ErrorTypes.NotFound,  // FIXME?: maybe change to a more specific URN like `..:rule:not-found`
                statusCode: StatusCodes.Status404NotFound,
                title: "Rule not found"
            ),
            EmptyKeywordError => controller.Problem(    // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Keyword value is empty"
            ),

            // Transactions
            DefaultCategoryNotFoundError => controller.Problem(
                type: ErrorTypes.DefaultCategoryNotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "Default category not found."
            ),
            TransactionNotFoundError => controller.Problem(
                type: ErrorTypes.NotFound,
                statusCode: StatusCodes.Status404NotFound,
                title: "Transaction not found"
            ),
            InvalidAmountError => controller.Problem(   // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Amount must be greater than 0"
            ),
            EmptyTransactionTitleError => controller.Problem(   // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Transaction title is empty"
            ),
            TransactionDescriptionNullError => controller.Problem(  // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "No description given"
            ),
            TransactionCounterPartyNullError => controller.Problem( // Handeled by Validator -> should not be used
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "No counter party given"
            ),
            InvalidCategorySourceError => controller.Problem(
                type: ErrorTypes.Validation,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Category source is not a valid value"
            ),

            // Other
            _ => controller.Problem(
                type: ErrorTypes.Internal,
                statusCode: StatusCodes.Status500InternalServerError,
                title: env.IsDevelopment()
                    ? error.ToErrorString()
                    : "An internal server error occured.",
                detail: env.IsDevelopment()
                    ? $"This error was not handled, please fix."
                    : null
            )
        };
    }

    public static ObjectResult ToProblem(this IEnumerable<Error> errors, ControllerBase controller)
    {
        // If all errors are an instance of `ValidationError`
        if (errors.All(e => e is ValidationError))
        {
            // Add all validation errors to model state
            // (model state = dictionary where all validation errors are written, is from aspdotnet)
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

        // For now, if we have at least 1 error & not all errors are ValidationErrors,
        // Map only the first error, all other are dropped for now.
        // Maybe we need to change this, but for now a multiple-error-response is only valid for validation errors
        return errors.First().ToProblem(controller);
    }
}
