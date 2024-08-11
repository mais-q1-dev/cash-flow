using MaisQ1Dev.Libs.Domain;

namespace MaisQ1Dev.CashFlow.Reports.Api.Extensions;

public static class MinimalApiResultExtensions
{
    public static IResult ToApiResult<T>(this Result<T> result)
        => result.Code switch
        {
            200 => result.Value is null ? Results.Ok() : Results.Ok(result.Value),
            201 => Results.Created(string.Empty, result.Value),
            202 => Results.Accepted(string.Empty, result.Value),
            204 => Results.NoContent(),
            404 => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
                    title: "NotFound",
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Information not found",
                    extensions: result.Errors.Count != 0
                        ? new Dictionary<string, object?> { { "errors", result.Errors } }
                        : null),
            422 => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc4918#section-11.2",
                    title: "Unprocessable Entity",
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    detail: "One or more validation failures have occurred",
                    extensions: result.Errors.Count != 0
                        ? new Dictionary<string, object?> { { "errors", result.Errors } }
                        : null),
            _ => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                    title: "InternalServerError",
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: "An error occurred",
                    extensions: result.Errors.Count != 0
                        ? new Dictionary<string, object?> { { "errors", result.Errors } }
                        : null)
        };

    public static IResult ToApiResult(this Result result)
        => result.Code switch
        {
            200 => Results.Ok(),
            204 => Results.NoContent(),
            404 => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
                    title: "NotFound",
                    statusCode: StatusCodes.Status404NotFound,
                    detail: "Information not found",
                    extensions: result.Errors.Count != 0
                        ? new Dictionary<string, object?> { { "errors", result.Errors } }
                        : null),
            422 => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc4918#section-11.2",
                    title: "Unprocessable Entity",
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    detail: "One or more validation failures have occurred"),
            _ => Results.Problem(
                    type: "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                    title: "InternalServerError",
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: "An error occurred")
        };
}
