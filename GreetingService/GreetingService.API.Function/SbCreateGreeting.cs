using System;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace GreetingService.API.Function
{
    public class SbCreateGreeting
    {
        private readonly ILogger<SbCreateGreeting> _logger;

        private readonly IGreetingRepository _greetingrepository;

        private readonly IConfiguration _config;
        public SbCreateGreeting(ILogger<SbCreateGreeting> log, IGreetingRepository greetingRepository, IConfiguration config)
        {
            _logger = log;
            _greetingrepository = greetingRepository;
            _config = config;
        }

        [FunctionName("SbCreateGreeting")]
        public async Task RunAsync([ServiceBusTrigger("main", "greeting_create", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myGreeting = JsonSerializer.Deserialize<Greeting>(mySbMsg);


            try
            {
                await _greetingrepository.CreateAsync(myGreeting);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }

            //complete the message? Not necessary I think
        }
    }
}
