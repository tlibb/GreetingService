using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core.Entities
{
    public interface IUserService
    { 
        public Task<bool> IsValidUserAsync(string username, string password);

        public Task<User> GetUserAsync(string email);

        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(string email);

        public Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
