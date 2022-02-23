using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.UserService
{
    public class SqlUserService : IUserService
    {
        private readonly GreetingDbContext _greetingdbcontext;

        public SqlUserService(GreetingDbContext mycontext)
        {
            _greetingdbcontext = mycontext;
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            var myUser = await _greetingdbcontext.Users.Where(u => u.FirstName == username && u.Password == password).FirstOrDefaultAsync();

            if (myUser == null)
                return false;
            else
                return true;
        }
    }
}
