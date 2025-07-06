using ServiceOrder.Domain.Enums;

namespace ServiceOrder.Domain.Models;

public class Result<T>
{
    public bool IsSuccess { get => !Errors.Any(); }
    public T? Value { get; }
    public IList<string> Errors { get; }
    public ErrorType? ErrorType { get; }

    private Result(T? value = default, IList<string>? errors = null, ErrorType? errorType = null)
    {
        Value = value;
        Errors = errors ?? new List<string>();
        ErrorType = errorType;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(string error, Enums.ErrorType errorType = Enums.ErrorType.Validation)
    {
        return new Result<T>(errors: new List<string> { error }, errorType: errorType);
    }

    public static Result<T> Failure(IList<string> errors, ErrorType errorType = Enums.ErrorType.Validation)
    {
        return new Result<T>(errors: errors, errorType: errorType);
    }

    public static Result<T> NotFound(string message = "Recurso não encontrado")
    {
        return new Result<T>(errors: new List<string> { message }, errorType: Enums.ErrorType.NotFound);
    }

    public static Result<T> Conflict(string message = "Conflito detectado")
    {
        return new Result<T>(errors: new List<string> { message }, errorType: Enums.ErrorType.Conflict);
    }

    public static Result<T> Unauthorized(string message = "Não autorizado")
    {
        return new Result<T>(errors: new List<string> { message }, errorType: Enums.ErrorType.Unauthorized);
    }

    public static Result<T> Forbidden(string message = "Acesso negado")
    {
        return new Result<T>(errors: new List<string> { message }, errorType: Enums.ErrorType.Forbidden);
    }

    public static Result<T> InternalError(string message = "Erro interno do servidor")
    {
        return new Result<T>(errors: new List<string> { message }, errorType: Enums.ErrorType.InternalError);
    }
}

public class Result
{
    public bool IsSuccess { get => !Errors.Any(); }
    public IList<string> Errors { get; }
    public ErrorType? ErrorType { get; }

    private Result(IList<string> errors = null, ErrorType? errorType = null)
    {
        Errors = errors ?? new List<string>();
        ErrorType = errorType;
    }

    public static Result Success()
    {
        return new Result();
    }

    public static Result Failure(string error, ErrorType errorType = Enums.ErrorType.Validation)
    {
        return new Result(errors: new List<string> { error }, errorType: errorType);
    }

    public static Result Failure(List<string> errors, ErrorType errorType = Enums.ErrorType.Validation)
    {
        return new Result(errors: errors, errorType: errorType);
    }

    public static Result NotFound(string message = "Recurso não encontrado")
    {
        return new Result(errors: new List<string> { message }, errorType: Enums.ErrorType.NotFound);
    }

    public static Result Conflict(string message = "Conflito detectado")
    {
        return new Result(errors: new List<string> { message }, errorType: Enums.ErrorType.Conflict);
    }

    public static Result Unauthorized(string message = "Não autorizado")
    {
        return new Result(errors: new List<string> { message }, errorType: Enums.ErrorType.Unauthorized);
    }

    public static Result Forbidden(string message = "Acesso negado")
    {
        return new Result(errors: new List<string> { message }, errorType: Enums.ErrorType.Forbidden);
    }

    public static Result InternalError(string message = "Erro interno do servidor")
    {
        return new Result(errors: new List<string> { message }, errorType: Enums.ErrorType.InternalError);
    }
}