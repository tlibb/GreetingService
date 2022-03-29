using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using GreetingService.Core.Entities;

namespace GreetingService.Infrastructure.GreetingRepository
{
    public class FileGreetingRepository : IGreetingRepository
    {
        public readonly string? _jsonPath;
        
        public static readonly JsonSerializerOptions _options = new () { WriteIndented = true };

        public FileGreetingRepository(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                File.WriteAllText(pathToFile, "[ ]");
            }

            _jsonPath = pathToFile;
        }

        public async Task CreateAsync(Greeting greeting)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            greetings.Add(greeting);

            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(greetings, _options));

        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            var greeting = from g in greetings
                           where g.id == id
                           select g;

            return greeting.FirstOrDefault();
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(jsonstr);
            return greetings;
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            // Update the greeting

            var existinggreeting = greetings?.Where(g => g.id == greeting.id).FirstOrDefault();

            if (existinggreeting != null)
            {
                existinggreeting.To = greeting.To;
                existinggreeting.From = greeting.From;
                existinggreeting.Message = greeting.Message;
            }
            else throw new KeyNotFoundException("id not found");



            //greetings.Where(g => g.id == greeting.id).Select(g =>
            //{
            //    g.Message = greeting.Message;
            //    g.To = greeting.To;
            //    g.From = greeting.From;
            //    g.TimeStamp = greeting.TimeStamp;
            //    return g;
            //}).ToList();


            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(greetings, _options));

        }

        public Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Greeting>> GetAsync(string from, int year, int month)
        {
            throw new NotImplementedException();
        }
    }
}
