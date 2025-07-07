using ServiceOrder.Domain.Models;

namespace ServiceOrder.Domain.Interfaces;

public interface IUserContextService
{
    UserProfile? GetCurrentUser();
    string? GetCurrentUserId();
    string? GetCurrentUserEmail();
    string? GetCurrentUserName();
    string? GetCurrentUserRole();
    bool IsAuthenticated();
    bool IsInRole(string role);
}
