using GreetingService.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GreetingService.Infrastructure.ApprovalService
{
    public class TeamsApprovalService : IApprovalService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public TeamsApprovalService(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task BeginUserApprovalAsync(User user)
        {

            //string filepath = ".\\GreetingService.Infrastructure\\ApprovalService\\teamsschemacard.json";
            //string jsonfile = "bla";
            //try
            //{
            //    jsonfile = File.ReadAllText(filepath);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //cardmodel m = JsonConvert.DeserializeObject<cardmodel>(jsonfile);

            //m.sections.FirstOrDefault().activitySubtitle = $"{user.FirstName} {user.LastName}";
            //m.sections.FirstOrDefault().facts.First<Fact>().value = DateTime.Now.ToString("dd MMMM yyyy HH: mm");

            ////endpoint uri for approved
            //m.sections.LastOrDefault().potentialAction.First<PotentialAction>().target = $"http://localhost:7071/api/approve?approvalCode={user.ApprovalCode}";

            ////endpoint uri for rejected
            //m.sections.LastOrDefault().potentialAction.Last<PotentialAction>().target = $"http://localhost:7071/api/rejection?approvalCode={user.ApprovalCode}";

            //string jsonmessage = JsonConvert.SerializeObject(m);

            Card mycard = new Card(user);

            string jsonmessage = mycard.jsoncard;

            // Please note that response body needs to be extracted and read 
            // as Connectors do not throw 429s
            try
            {
                // Perform Connector POST operation     
                var httpResponseMessage = await _client.PostAsync(_configuration["WebhookUrl"], new StringContent(jsonmessage));
                // Read response content
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            
        }
    }
}
