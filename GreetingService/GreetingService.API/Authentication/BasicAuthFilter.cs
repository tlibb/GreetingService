using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using GreetingService.Core.Entities;

namespace GreetingService.API.Authentication
{

    public class BasicAuthFilter : IAuthorizationFilter
    {
        private readonly string _realm;

        public BasicAuthFilter(string realm)
        {
            _realm = realm;
            if (string.IsNullOrWhiteSpace(_realm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                                            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                                            .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            if (await IsAuthorizedAsync(context, credentials[0], credentials[1]))
                            {
                                return;
                            }
                        }
                    }
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        public async Task<bool> IsAuthorizedAsync(AuthorizationFilterContext context, string username, string password)
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            return await userService.IsValidUserAsync(username, password);
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
