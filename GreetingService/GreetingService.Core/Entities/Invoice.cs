using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core.Entities
{
    public class Invoice
    {
        public Invoice()
        {
        }

        
        public int Id { get;}
        public User? User { get; set; }
        public IEnumerable<Greeting>? Greetings { get; set;}
        public int Year { get; set; }
        public int Month { get; set; }
        public int Cost { get; set; }
        public string? Currency { get; } = "SEK";


    }
}
