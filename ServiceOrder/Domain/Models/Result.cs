namespace ServiceOrder.Domain.Models;

public class Result<T>
{
    public bool IsSuccess { get => !Errors.Any(); }
    public T? Value { get; }
    public IList<string> Errors { get; }

    private Result(T? value = default, IList<string>? errors = null)
    {
        Value = value;
        Errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(errors: new List<string>
        {
            error
        });
    }

    public static Result<T> Failure(IList<string> errors)
    {
        return new Result<T>(errors:  errors);
    }


    public static Result<T> NotFound(string message = "Recurso não encontrado")
    {
        return new Result<T>(errors: new List<string>
        {
            message
        });
    }

}

public class Result
{
    public bool IsSuccess { get => !Errors.Any(); }
    public IList<string> Errors { get; }

    private Result(IList<string> errors = null)
    {
        Errors = errors ?? new List<string>();
    }

    public static Result Success()
    {
        return new Result();
    }

    public static Result Failure(string error)
    {
        return new Result(errors: new List<string>
        {
            { error } 
        });
    }

    public static Result Failure(List<string> errors)
    {
        return new Result(errors: errors);
    }

    public static Result NotFound(string message = "Recurso não encontrado")
    {
        return new Result(errors: new List<string>
        {
            { message }
        });
    }

}