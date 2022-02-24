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

        public async Task CreateUserAsync(User user)
        {
            await _greetingdbcontext.Users.AddAsync(user);
            await _greetingdbcontext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string email)
        {
            var myUser = await _greetingdbcontext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (myUser != null)
            {
                _greetingdbcontext.Users.Remove(myUser);
                await _greetingdbcontext.SaveChangesAsync();
            }
            else throw new Exception("User Not Found");
        }

        public async Task<User> GetUserAsync(string email)
        {
            var myUser = await _greetingdbcontext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (myUser != null)
                return myUser;
            else throw new Exception("User Not Found");
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            var myUser = await _greetingdbcontext.Users.Where(u => u.FirstName == username && u.Password == password).FirstOrDefaultAsync();

            if (myUser == null)
                return false;
            else
                return true;
        }

        public async Task UpdateUserAsync(User user)
        {
            var myUser = await _greetingdbcontext.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (myUser == null)
                throw new Exception("User Not Found");
            myUser.FirstName = user.FirstName;
            myUser.LastName = user.LastName;
            myUser.Password = user.Password;
            myUser.Modified = DateTime.Now;
            await _greetingdbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var myUsers = await _greetingdbcontext.Users.ToListAsync();
            if (myUsers == null)
            {
                throw new Exception("No Users in database");
            }
            return myUsers;
        }
    }
}
