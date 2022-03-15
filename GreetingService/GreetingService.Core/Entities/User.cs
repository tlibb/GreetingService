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

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }

        public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.Pending;

        public string ApprovalStatusNote { get; set; } = "No action yet";

        public string ApprovalCode { get; set; } = CreateApprovalCode();

        public DateTime ApprovalExpiry { get; set; } = DateTime.Now.AddHours(24);

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

        public enum ApprovalStatusType
        {
            Pending = 0,
            Approved =1,
            Rejected =2,
            
        }

        public static string CreateApprovalCode()
        {
            Random rand = new Random();
            int stringlen = rand.Next(4, 10);
            int randValue;
            string str = "";
            char letter;
            for (int i = 0; i < stringlen; i++)
            {

                // Generating a random number.
                randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                str = str + letter;
            }

            return str;

        }

        
    }
}
