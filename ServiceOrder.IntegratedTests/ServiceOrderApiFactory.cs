using Microsoft.AspNetCore.Mvc.Testing;

namespace ServiceOrder.IntegratedTests;

public class ServiceOrderApiFactory : WebApplicationFactory<ServiceOrder.Program>
{
    static ServiceOrderApiFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }
} 