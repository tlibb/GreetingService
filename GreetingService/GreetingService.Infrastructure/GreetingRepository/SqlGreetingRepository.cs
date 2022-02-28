using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.GreetingRepository
{
    public class SqlGreetingRepository : IGreetingRepository
    {
        private readonly GreetingDbContext _greetingdbcontext;

        public SqlGreetingRepository(GreetingDbContext mycontext)
        {
            _greetingdbcontext = mycontext;
        }

        public async Task CreateAsync(Greeting greeting)
        {
            await _greetingdbcontext.AddAsync(greeting);
            await _greetingdbcontext.SaveChangesAsync();
        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            var gg = await _greetingdbcontext.Greetings.Where(g => g.id == id).FirstOrDefaultAsync();
            return gg;
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            var myGreetings = await _greetingdbcontext.Greetings.ToListAsync<Greeting>();
            return myGreetings;
        }

        public async Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            

            if (String.IsNullOrWhiteSpace(from) && !String.IsNullOrWhiteSpace(to))
            {
                var mygreetings = await _greetingdbcontext.Greetings.Where(g => g.To == to).ToListAsync();
                return mygreetings;
            }

            if (!String.IsNullOrWhiteSpace(from) && String.IsNullOrWhiteSpace(to))
            {
                var mygreetings = await _greetingdbcontext.Greetings.Where(g => g.From== from).ToListAsync();
                return mygreetings;
            }

            if (!String.IsNullOrWhiteSpace(from) && !String.IsNullOrWhiteSpace(to))
            {
                var mygreetings = await _greetingdbcontext.Greetings.Where(g => g.To == to && g.From == from).ToListAsync();
                return mygreetings;
            }

            return await GetAsync();


        }

        public async Task UpdateAsync(Greeting greeting)
        {
            //Disclaimer!!! These are reference variables so changing them changes the original object!
            //Default in C#
            var myGreeting = await _greetingdbcontext.Greetings.Where(g => g.id == greeting.id).FirstOrDefaultAsync();
            
            if (myGreeting == null)
            {
                throw new Exception("Greeting ID not found");
            }
            myGreeting.Message = greeting.Message;
            myGreeting.From = greeting.From;
            myGreeting.To = greeting.To;
            myGreeting.TimeStamp = greeting.TimeStamp;
            
            await _greetingdbcontext.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {
            var gg = await _greetingdbcontext.Greetings.Where(g => g.id == id).FirstOrDefaultAsync();
            if (gg != null)
            {
                _greetingdbcontext.Remove(gg);
                await _greetingdbcontext.SaveChangesAsync();
            }
            else throw new Exception("Id not found");
            
        }
    }
}
