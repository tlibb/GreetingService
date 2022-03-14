using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function
{
    public class SbComputeInvoiceForGreeting
    {
        private readonly ILogger<SbComputeInvoiceForGreeting> _logger;

        private readonly IInvoiceService _invoiceservice;

        private readonly IUserService _userservice;

        public SbComputeInvoiceForGreeting(ILogger<SbComputeInvoiceForGreeting> log, IInvoiceService invoiceservice, IUserService userService)
        {
            _logger = log;
            _invoiceservice = invoiceservice;
            _userservice = userService;
        }

        [FunctionName("SbComputeInvoiceForGreeting")]
        public async Task RunAsync([ServiceBusTrigger("main", "greeting_compute_billing", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var myGreeting = JsonSerializer.Deserialize<Greeting>(mySbMsg);

           
            var myInvoice = await _invoiceservice.GetInvoiceAsync(DateTime.Now.Year, DateTime.Now.Month, myGreeting.From);
            

            if (myInvoice != null)
            {
                await _invoiceservice.CreateOrUpdateInvoiceAsync(myInvoice);
            }
            else
            {
                myInvoice = new Invoice();
                myInvoice.Month = DateTime.Now.Month;
                myInvoice.Year = DateTime.Now.Year;
                myInvoice.User = await _userservice.GetUserAsync(myGreeting.From);
                var myList = new List<Greeting>();
                myList.Add(myGreeting);
                myInvoice.Greetings = myList;
                await _invoiceservice.CreateOrUpdateInvoiceAsync(myInvoice);
            }    
        }
    }
}
