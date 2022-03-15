using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Function
{
    public class RejectUserAsync
    {
        private readonly ILogger<RejectUserAsync> _logger;

        private readonly IUserService _userService;

        public RejectUserAsync(ILogger<RejectUserAsync> log, IUserService userservice)
        {
            _logger = log;
            _userService = userservice;
        }

        [FunctionName("RejectUserAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rejection")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string code = req.Query["approvalCode"];

            try
            {
                await _userService.RejectUserAsync(code);

                return new OkObjectResult("User rejection");
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult($"Something went wrong: {ex.Message}");
            }
            
        }
    }
}

