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
    public class UpdateUserAsync
    {
        private readonly ILogger<UpdateUserAsync> _logger;
        private readonly IUserService _userservice;
        private readonly IMessagingService _messagingservice;

        public UpdateUserAsync(ILogger<UpdateUserAsync> log, IUserService userservice, IMessagingService messagingservice)
        {
            _logger = log;
            _userservice = userservice;
            _messagingservice = messagingservice;
        }

        [FunctionName("UpdateUserAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            

            try
            {
                var myUser = JsonConvert.DeserializeObject<User>(content);

                //await _userservice.UpdateUserAsync(myUser);
                await _messagingservice.SendAsync(myUser, MessageSubject.PutUser);
                return new OkObjectResult("Sent to update user");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            
        }
    }
}

