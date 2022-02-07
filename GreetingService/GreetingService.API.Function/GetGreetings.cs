using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using GreetingService.Core.Entities;

namespace GreetingService.API.Function
{
    public class GetGreetings
    {
        private readonly ILogger<GetGreetings> _logger;

        private readonly IGreetingRepository _greetingRepository;

        public GetGreetings(ILogger<GetGreetings> log, IGreetingRepository greetingRepository)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
        }

        [FunctionName("GetGreetings")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greetings")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var responseresult = _greetingRepository.Get();

            return new OkObjectResult(responseresult);
        }
    }
}

