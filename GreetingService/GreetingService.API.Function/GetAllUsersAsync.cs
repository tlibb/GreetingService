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
    public class GetAllUsersAsync
    {
        private readonly ILogger<GetAllUsersAsync> _logger;
        private readonly IUserService _userservice;

        public GetAllUsersAsync(ILogger<GetAllUsersAsync> log, IUserService userservice)
        {
            _logger = log;
            _userservice = userservice;
        }

        [FunctionName("GetAllUsersAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var myUsers = await _userservice.GetAllUsersAsync();
                return new OkObjectResult(myUsers);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            
            
        }
    }
}

