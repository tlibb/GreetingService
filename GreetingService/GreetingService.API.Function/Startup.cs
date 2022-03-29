using GreetingService.Core.Entities;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GreetingService.API.Function.Authentication;
using Serilog;
using GreetingService.Infrastructure.UserService;
using GreetingService.Infrastructure.GreetingRepository;
using GreetingService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using GreetingService.Infrastructure.InvoiceService;
using GreetingService.Infrastructure.MessageService;
using GreetingService.Infrastructure.ApprovalService;
using Azure.Identity;
using System;
using Microsoft.Extensions.Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Cosmos;






[assembly: FunctionsStartup(typeof(GreetingService.API.Function.Startup))]

namespace GreetingService.API.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            IConfiguration config = builder.GetContext().Configuration;

            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();

            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));


            builder.Services.AddScoped<IUserService, SqlUserService>();


            var connectionString = config["LoggingStorageAccount"];


            var logger = new LoggerConfiguration()
               .WriteTo.AzureBlobStorage(connectionString)
               .CreateLogger();

            Log.Logger = logger;

            //builder.Services.AddScoped<IGreetingRepository, SqlGreetingRepository>();

            builder.Services.AddDbContext<GreetingDbContext>(options =>
            {
                options.UseSqlServer(config["GreetingDbConnectionString"],
                sqlServerOptionsAction: SqlOptions =>
                {
                    SqlOptions.EnableRetryOnFailure();
                });
            });

            builder.Services.AddSingleton<IGreetingRepository, CosmoGreetingRepository>();


            builder.Services.AddScoped<IInvoiceService, SqlInvoiceService>();

            builder.Services.AddSingleton<IMessagingService, ServiceBusMessagingService>();

            builder.Services.AddScoped<IApprovalService, TeamsApprovalService>();


        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var s = Environment.GetEnvironmentVariable("KeyVaultUri");
            builder.ConfigurationBuilder.AddAzureKeyVault(Environment.GetEnvironmentVariable("KeyVaultUri"));

        }

    }

       



        
}