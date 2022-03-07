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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Function
{
    public class DeleteUserAsync
    {
        private readonly ILogger<DeleteUserAsync> _logger;
        private readonly IUserService _userservice;

        public DeleteUserAsync(ILogger<DeleteUserAsync> log, IUserService userservice)
        {
            _logger = log;
            _userservice = userservice;
        }

        [FunctionName("DeleteUserAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "user/{email}")] HttpRequest req, string email)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            
            try
            {
                await _userservice.DeleteUserAsync(email);
                return new OkObjectResult("Deleted user");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Error. This user might have greetings in the table. {ex.Message}");
            }

            
        }
    }
}

