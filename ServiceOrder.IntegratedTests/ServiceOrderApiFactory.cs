using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ServiceOrder.IntegratedTests;

public class ServiceOrderApiFactory : WebApplicationFactory<Program>
{
    static ServiceOrderApiFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }
} 