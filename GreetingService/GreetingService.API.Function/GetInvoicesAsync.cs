using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Function
{
    public class GetInvoicesAsync
    {
        private readonly ILogger<GetInvoiceAsync> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly GreetingDbContext _greetingdbcontext;
        
        public GetInvoicesAsync(ILogger<GetInvoiceAsync> log, IInvoiceService invoiceService, GreetingDbContext greetingDbContext)
        {
            _logger = log;
            _invoiceService = invoiceService;
            _greetingdbcontext = greetingDbContext;
        }

        [FunctionName("GetInvoicesAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "invoice/{year}/{month}")] HttpRequest req, string year, string month)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            int.TryParse(year, out int myYear);
            int.TryParse(month, out int myMonth);

            try
            {
                var myInvoices = await _invoiceService.GetInvoicesAsync(myYear, myMonth);
                return new OkObjectResult(myInvoices);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
                








            return new OkObjectResult("Ok");
        }
    }
}

