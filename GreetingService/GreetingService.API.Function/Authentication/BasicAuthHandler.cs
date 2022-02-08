using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;

namespace GreetingService.API.Function.Authentication
{

    internal class BasicAuthHandler : IAuthHandler
    {
        private readonly IUserService _userService;

        public BasicAuthHandler(IUserService userService)
        {
            _userService = userService;
        }

        public bool IsAuthorized(HttpRequest req)
        {
            string authHeader = req.Headers["Authorization"];

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            int seperatorIndex = usernamePassword.IndexOf(':');

            var myusername = usernamePassword.Substring(0, seperatorIndex);
            var mypassword = usernamePassword.Substring(seperatorIndex + 1);
            

            return _userService.IsValidUser(myusername, mypassword);
            
        }

        
    }
}
