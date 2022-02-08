using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using GreetingService.API.Function.Authentication;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(c => {
                var config = c.GetService<IConfiguration>();
                return new FileGreetingRepository(config["FileRepositoryFilePath"]);
            });
            builder.Services.AddScoped<IUserService, AppSettingsUserService>();

            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();

            

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}