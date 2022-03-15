using GreetingService.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace GreetingService.Infrastructure.UserService
{
    public class AppSettingsUserService : IUserService
    {
        private IConfiguration _config;
        public AppSettingsUserService(IConfiguration config)
        {
            _config = config;
        }

        public Task ApproveUserAsync(string approvalCode)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            //this works with hardcoded username and password in appsettings.json
            return (_config["MyUserName"] == username && _config["MyPassWord"] == password);

            //try here if it also works automatically with azure

            //var entries = _config.AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            //if (entries.TryGetValue(username, out var storedPassword))
            //    return storedPassword == password;

            //return false;
        }

        public Task RejectUserAsync(string approvalCode)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
