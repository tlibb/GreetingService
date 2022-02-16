using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;

namespace GreetingService.Infrastructure
{
    public class MemoryGreetingRepository : IGreetingRepository
    {
        private readonly List<Greeting> _memoryRepo;
        
        public MemoryGreetingRepository()
        {
            _memoryRepo = new List<Greeting>(); 
        }

        public async Task CreateAsync(Greeting greeting)
        {
            _memoryRepo.Add(greeting);
        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            var myGreeting = from g in _memoryRepo
                             where g.id == id
                             select g;
            
            return myGreeting.FirstOrDefault();
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            return _memoryRepo;
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            var existinggreeting = _memoryRepo.Where(g => g.id == greeting.id).FirstOrDefault();

            if (existinggreeting != null)
            {
                _memoryRepo.Where(g => g.id == greeting.id).Select(g =>
                {
                    g.Message = greeting.Message;
                    g.To = greeting.To;
                    g.From = greeting.From;
                    g.TimeStamp = greeting.TimeStamp;
                    return g;
                }).ToList();
            }
            else throw new KeyNotFoundException("id not found");
        }

        
    }
}
