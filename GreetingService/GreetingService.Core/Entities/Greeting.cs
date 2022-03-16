using GreetingService.Core.NewFolder;
using System;

namespace GreetingService.Core.Entities
{

    public class Greeting
    {

        public string? Message { get; set; }
        //public string? From { get; set; }
        private string _from;
        public string From
        {
            get
            {
                return _from;
            }
            set
            {

                if (value.IsValidEmail())
                {
                    _from = value;
                }
                else
                {
                    throw new Exception("No valid email address");
                }

            }
        }
        //public string? To { get; set; }
        private string _to;
        public string To
        {
            get
            {
                return _to;
            }
            set
            {

                if (value.IsValidEmail())
                {
                    _to = value;
                }
                else
                {
                    throw new Exception("No valid email address");
                }

            }
        }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public Guid id { get; set; } = Guid.NewGuid();

    }
}