using System;
using System.Linq;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GreetingService.API.Function
{
    public class TimerInvoices
    {
        private readonly ILogger<TimerInvoices> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly GreetingDbContext _greetingdbcontext;

        public TimerInvoices(ILogger<TimerInvoices> log, IInvoiceService invoiceService, GreetingDbContext greetingdbcontext)
        {
            _logger = log;
            _invoiceService = invoiceService;
            _greetingdbcontext = greetingdbcontext;
        }



        [FunctionName("TimerInvoices")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task RunAsync([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var myUsers = await _greetingdbcontext.Users.ToListAsync();

            foreach (var user in myUsers)
            {
                var myInvoice = _greetingdbcontext.Invoices.Where(i => i.User == user && i.Month == DateTime.Now.Month && i.Year == DateTime.Now.Year).FirstOrDefault();
                if (myInvoice == null)
                {
                    myInvoice = new Invoice();
                    myInvoice.Month = DateTime.Now.Month;
                    myInvoice.Year = DateTime.Now.Year;
                    myInvoice.User = user;
                }

                try
                {
                    await _invoiceService.CreateOrUpdateInvoiceAsync(myInvoice);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong. {ex.Message}");
                }
            }
                
            
            

        }
    }

       
}
