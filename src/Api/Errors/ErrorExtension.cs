using Application.Common;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Errors;

public static class ErrorExtension
{
    public static ObjectResult ToProblem(this Error error, ControllerBase controller)
        => error switch
        {
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
            _ => controller.Problem(   // Type automatically set
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An internal server error occured."
            )
        };
}