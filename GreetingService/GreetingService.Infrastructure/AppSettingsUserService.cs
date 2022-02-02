using GreetingService.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace GreetingService.Infrastructure
{
    public class AppSettingsUserService : IUserService
    {
        private IConfiguration _config;
        public AppSettingsUserService(IConfiguration config)
        {
            _config = config;
        }

        public bool IsValidUser(string username, string password)
        {
           return (_config["MyUserName"] == username && _config["MyPassWord"] == password);
        }
    }
}
