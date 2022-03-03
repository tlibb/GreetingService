using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GreetingService.API.Function.Authentication
{

    internal class BasicAuthHandler : IAuthHandler
    {
        private readonly IUserService _userService;
        private readonly ILogger<BasicAuthHandler> _logger;

        public BasicAuthHandler(IUserService userService, ILogger<BasicAuthHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<bool> IsAuthorizedAsync(HttpRequest req)
        {
            try
            {
                string authHeader = req.Headers["Authorization"];

                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var myusername = usernamePassword.Substring(0, seperatorIndex);
                var mypassword = usernamePassword.Substring(seperatorIndex + 1);

                var succeeded = await _userService.IsValidUserAsync(myusername, mypassword);
                if (succeeded)
                {
                    return true;
                }
                _logger.LogWarning("Error: user could not be authenticated");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"It went here");
                throw new UnauthorizedAccessException();
            }

            
            
        }

       
    }
}
