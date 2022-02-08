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
            //this works with hardcoded username and password in appsettings.json
            return (_config["MyUserName"] == username && _config["MyPassWord"] == password);

            //try here if it also works automatically with azure

            //var entries = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            //if (entries.TryGetValue(username, out var storedPassword))
            //    return storedPassword == password;

            //return false;
        }
    }
}
