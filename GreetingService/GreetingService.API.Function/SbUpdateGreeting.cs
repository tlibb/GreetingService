using System;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function
{
    public class SbUpdateGreeting
    {
        private readonly ILogger<SbUpdateGreeting> _logger;

        private readonly IGreetingRepository _greetingrepository;

        public SbUpdateGreeting(ILogger<SbUpdateGreeting> log, IGreetingRepository greetingrepository)
        {
            _logger = log;
            _greetingrepository = greetingrepository;
        }

        [FunctionName("SbUpdateGreeting")]
        public async Task RunAsync([ServiceBusTrigger("main", "greeting_update", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myGreeting = JsonSerializer.Deserialize<Greeting>(mySbMsg);


            await _greetingrepository.UpdateAsync(myGreeting);
        }
    }
}
