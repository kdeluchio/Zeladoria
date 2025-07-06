namespace ServiceOrder.Domain.Enums;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    InternalError
}
