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

namespace GreetingService.API.Function
{
    public class PutGreeting
    {
        private readonly ILogger<PutGreeting> _logger;

        private readonly IGreetingRepository _greetingRepository;

        private readonly IMessagingService _messagingservice;

        public PutGreeting(ILogger<PutGreeting> log, IGreetingRepository greetingRepository, IMessagingService messagingservice)
        {
            _logger = log;
            _greetingRepository = greetingRepository;
            _messagingservice = messagingservice;
        }

        [FunctionName("PutGreeting")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "greeting")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            
                

            //some comment
            try
            {
                Greeting mygreeting = JsonConvert.DeserializeObject<Greeting>(content);
                await _messagingservice.SendAsync(mygreeting, MessageSubject.PutGreeting);
                //await _greetingRepository.UpdateAsync(mygreeting);
                return new OkObjectResult("Sent to be Updated");
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult($"Didn't work: {ex.Message}");
            }


           
        }
    }
}

