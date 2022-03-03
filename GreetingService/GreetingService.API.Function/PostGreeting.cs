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
using System;
using GreetingService.API.Function.Authentication;

namespace GreetingService.API.Function
{
    public class PostGreeting
    {
        private readonly ILogger<PostGreeting> _logger;

        private readonly IGreetingRepository _greetingRepository;

        private readonly IAuthHandler _auth;

        public PostGreeting(ILogger<PostGreeting> log, IGreetingRepository greetingRepository, IAuthHandler auth)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
            _auth = auth;
        }

        [FunctionName("PostGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "greeting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            Boolean mybool = false;
            try
            {
                mybool = await _auth.IsAuthorizedAsync(req);
            }
            catch 
            {
                return new UnauthorizedResult();
            }

            if (mybool)            
            {

                var content = await new StreamReader(req.Body).ReadToEndAsync();

                try
                {
                    Greeting mygreeting = JsonConvert.DeserializeObject<Greeting>(content);
                    await _greetingRepository.CreateAsync(mygreeting);
                    return new OkObjectResult("Posted");
                }
                catch (Exception ex)
                {
                    return new NotFoundObjectResult($"Didn't work. {ex.InnerException.Message} ");
                }
            }
            return new NotFoundObjectResult($"Didn't work.");




        }
    }
}

