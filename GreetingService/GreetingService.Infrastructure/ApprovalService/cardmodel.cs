using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.ApprovalService
{
    public class Fact
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class PotentialAction
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        public string name { get; set; }
        public string target { get; set; }
    }

    public class Section
    {
        public string title { get; set; }
        public string activityImage { get; set; }
        public string activityTitle { get; set; }
        public string activitySubtitle { get; set; }
        public List<Fact> facts { get; set; }
        public List<PotentialAction> potentialAction { get; set; }
    }

    public class cardmodel
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@context")]
        public string Context { get; set; }
        public List<Section> sections { get; set; }
    }
   
    
}
