using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ServiceAuth.IntegratedTests;

public class ServiceAuthApiFactory : WebApplicationFactory<Program>
{
    static ServiceAuthApiFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }
}
