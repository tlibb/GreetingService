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
using GreetingService.API.Function.Authentication;
using System;

namespace GreetingService.API.Function
{
    public class GetGreetings
    {
        private readonly ILogger<GetGreetings> _logger;

        private readonly IGreetingRepository _greetingRepository;

        private readonly IAuthHandler _auth;

        public GetGreetings(ILogger<GetGreetings> log, IGreetingRepository greetingRepository, IAuthHandler auth)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
            _auth = auth;
        }

        [FunctionName("GetGreetings")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greetings")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //return new OkObjectResult(_greetingRepository.Get());

            if (await _auth.IsAuthorizedAsync(req))
            {
                try
                {
                    var from = req.Query["from"];
                    var to = req.Query["to"];
                    var responseresult = await _greetingRepository.GetAsync(from, to);
                    return new OkObjectResult(responseresult);
                }
                catch (Exception ex)
                {
                    return new NotFoundObjectResult(ex.Message);
                }

                
            }
            else return new UnauthorizedResult();
        }
    }
}

