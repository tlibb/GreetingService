using GreetingService.Core.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GreetingService.Core.Entities
{
    public class User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public string? Email { get; set; }
        public string? Password { get; set; }

        public DateTime? Created = DateTime.Now;
        public DateTime? Modified { get; set; }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                
                if (value.IsValidEmail())
                {
                    _email = value;
                }
                else
                {
                    throw new Exception("No valid email address");
                }
                
            }
        }

        
    }
}
