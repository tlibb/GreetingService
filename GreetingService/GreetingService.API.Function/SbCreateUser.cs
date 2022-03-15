using System;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function
{
    public class SbCreateUser
    {
        private readonly ILogger<SbCreateUser> _logger;

        private readonly IUserService _userservice;

        private readonly IApprovalService _approval;

        public SbCreateUser(ILogger<SbCreateUser> log, IUserService userservice, IApprovalService approval)
        {
            _logger = log;
            _userservice = userservice;
            _approval = approval;
        }

        [FunctionName("SbCreateUser")]
        public async Task RunAsync([ServiceBusTrigger("main", "user_create", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myUser = JsonSerializer.Deserialize<User>(mySbMsg);

            try
            {
                await _userservice.CreateUserAsync(myUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            
            await _approval.BeginUserApprovalAsync(myUser);

           

        }
    }
}
