using System;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function
{
    public class SbUpdateUser
    {
        private readonly ILogger<SbUpdateUser> _logger;
        private readonly IUserService _userService;

        public SbUpdateUser(ILogger<SbUpdateUser> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("SbUpdateUser")]
        public async Task RunAsync([ServiceBusTrigger("main", "user_update", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myUser = JsonSerializer.Deserialize<User>(mySbMsg);

            await _userService.UpdateUserAsync(myUser);

        }
    }
}
