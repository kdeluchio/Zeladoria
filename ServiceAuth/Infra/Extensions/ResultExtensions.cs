using ServiceAuth.Domain.Enums;
using ServiceAuth.Domain.Models;

namespace ServiceAuth.Infra.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return result.ErrorType switch
        {
            ErrorType.Validation => Results.BadRequest(new { errors = result.Errors }),
            ErrorType.NotFound => Results.NotFound(new { error = result.Errors.First() }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Errors.First() }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new { errors = result.Errors })
        };
    }

    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return result.ErrorType switch
        {
            ErrorType.Validation => Results.BadRequest(new { errors = result.Errors }),
            ErrorType.NotFound => Results.NotFound(new { error = result.Errors.First() }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Errors.First() }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new { errors = result.Errors })
        };
    }

    public static IResult ToCreatedResult<T>(this Result<T> result, string location)
    {
        if (result.IsSuccess)
            return Results.Created(location, result.Value);

        return result.ErrorType switch
        {
            ErrorType.Validation => Results.BadRequest(new { errors = result.Errors }),
            ErrorType.NotFound => Results.NotFound(new { error = result.Errors.First() }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Errors.First() }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new { errors = result.Errors })
        };
    }

    public static IResult ToNoContentResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.NoContent();

        return result.ErrorType switch
        {
            ErrorType.Validation => Results.BadRequest(new { errors = result.Errors }),
            ErrorType.NotFound => Results.NotFound(new { error = result.Errors.First() }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Errors.First() }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new { errors = result.Errors })
        };
    }

    public static IResult ToNoContentResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.NoContent();

        return result.ErrorType switch
        {
            ErrorType.Validation => Results.BadRequest(new { errors = result.Errors }),
            ErrorType.NotFound => Results.NotFound(new { error = result.Errors.First() }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Errors.First() }),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.BadRequest(new { errors = result.Errors })
        };
    }
} 