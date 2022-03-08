using System;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GreetingService.API.Function
{
    public class SbCreateGreeting
    {
        private readonly ILogger<SbCreateGreeting> _logger;

        private readonly IGreetingRepository _greetingrepository;

        public SbCreateGreeting(ILogger<SbCreateGreeting> log, IGreetingRepository greetingRepository)
        {
            _logger = log;
            _greetingrepository = greetingRepository;
        }

        [FunctionName("SbCreateGreeting")]
        public async Task RunAsync([ServiceBusTrigger("main", "greeting_create", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myGreeting = JsonSerializer.Deserialize<Greeting>(mySbMsg);


            await _greetingrepository.CreateAsync(myGreeting);

            //complete the message
        }
    }
}
