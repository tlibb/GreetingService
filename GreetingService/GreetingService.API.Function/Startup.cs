using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GreetingService.API.Function.Authentication;
using Serilog;

[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            //builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(c =>
            //{
            //    var config = c.GetService<IConfiguration>();
            //    return new FileGreetingRepository(config["FileRepositoryFilePath"]);
            //});

            //builder.Services.AddSingleton<IGreetingRepository, MemoryGreetingRepository>();

            builder.Services.AddScoped<IUserService, AppSettingsUserService>();

            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();

            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            IConfiguration config = builder.GetContext().Configuration;
            
            var connectionString = config["LoggingStorageAccount"];


            var logger = new LoggerConfiguration()
               .WriteTo.AzureBlobStorage(connectionString)
               .CreateLogger();

            Log.Logger = logger;

            builder.Services.AddSingleton<IGreetingRepository, BlobGreetingRepository>(c =>
            {
                var config = c.GetService<IConfiguration>();
                return new BlobGreetingRepository(config["LoggingStorageAccount"]);
            });

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}