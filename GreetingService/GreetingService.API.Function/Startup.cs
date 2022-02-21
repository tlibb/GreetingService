using GreetingService.Core.Entities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GreetingService.API.Function.Authentication;
using Serilog;
using GreetingService.Infrastructure.UserService;
using GreetingService.Infrastructure.GreetingRepository;

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

            builder.Services.AddSingleton<IGreetingRepository, MemoryGreetingRepository>();

            //builder.Services.AddScoped<IUserService, AppSettingsUserService>();

            

            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();

            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            IConfiguration config = builder.GetContext().Configuration;

            builder.Services.AddScoped<IUserService, BlobUserService>(c =>
            {
                return new BlobUserService(config);
            });
           

            var connectionString = config["LoggingStorageAccount"];


            var logger = new LoggerConfiguration()
               .WriteTo.AzureBlobStorage(connectionString)
               .CreateLogger();

            Log.Logger = logger;

            //builder.Services.AddScoped<IGreetingRepository, BlobGreetingRepository>(c =>
            //{
            //    var config = c.GetService<IConfiguration>();
            //    return new BlobGreetingRepository(config);
            //});

            //builder.Services.AddSingleton<ILoggerProvider, MyLoggerProvider>();
        }
    }
}