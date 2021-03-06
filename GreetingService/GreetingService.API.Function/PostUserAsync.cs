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
    public class PostUserAsync
    {
        private readonly ILogger<PostUserAsync> _logger;
        private readonly IUserService _userservice;
        private readonly IMessagingService _messagingservice;

        public PostUserAsync(ILogger<PostUserAsync> log, IUserService userservice, IMessagingService messagingservice)
        {
            _logger = log;
            _userservice = userservice;
            _messagingservice = messagingservice;
        }

        [FunctionName("PostUserAsync")]
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
                myUser.ApprovalStatus = User.ApprovalStatusType.Pending;
                await _messagingservice.SendAsync(myUser, MessageSubject.NewUser);
                return new OkObjectResult("User sent to be created");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.InnerException.Message);
            }

            
        }
    }
}

