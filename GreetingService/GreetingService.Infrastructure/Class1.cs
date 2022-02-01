using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using GreetingService.Core.Entities;

namespace GreetingService.Infrastructure
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

        public void Create(Greeting greeting)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            greetings.Add(greeting);

            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(greetings, _options));

        }

        public Greeting Get(Guid id)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            var greeting = from g in greetings
                           where g.id == id
                           select g;

            return greeting.FirstOrDefault();
        }

        public IEnumerable<Greeting> Get()
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            var greetings = JsonSerializer.Deserialize<IList<Greeting>>(jsonstr);
            return greetings;
        }

        public void Update(Greeting greeting)
        {
            string jsonstr = File.ReadAllText(_jsonPath);
            List<Greeting> greetings = JsonSerializer.Deserialize<List<Greeting>>(jsonstr);

            // Update the greeting

            greetings.Where(g => g.id == greeting.id).Select(g =>
            {
                g.Message = greeting.Message;
                g.To = greeting.To;
                g.From = greeting.From;
                g.TimeStamp = greeting.TimeStamp;
                return g;
            }).ToList();


            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(greetings, _options));

        }
    }
}
