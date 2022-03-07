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
    public class CreateUserAsync
    {
        private readonly ILogger<CreateUserAsync> _logger;
        private readonly IUserService _userservice;

        public CreateUserAsync(ILogger<CreateUserAsync> log, IUserService userservice)
        {
            _logger = log;
            _userservice = userservice;
        }

        [FunctionName("CreateUserAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var myUser = JsonConvert.DeserializeObject<User>(content);
                _userservice.CreateUserAsync(myUser);
                return new OkObjectResult("User created");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException.Message);
            }

            
        }
    }
}

