using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using GreetingService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace GreetingService.API.Function
{
    public class ApproveUserAsync
    {
        private readonly ILogger<ApproveUserAsync> _logger;

        private readonly IUserService _userService;


        public ApproveUserAsync(ILogger<ApproveUserAsync> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("ApproveUserAsync")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="approve")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string code = req.Query["approvalCode"];

            try
            {
                await _userService.ApproveUserAsync(code);

                return new OkObjectResult("User approved");
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult($"Something went wrong: {ex.Message}"); 
            }
        }
    }
}

