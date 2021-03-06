using GreetingService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.ApprovalService
{
    public class Card
    {
        public string jsoncard { get; set; }

        public Card(User user)
        {
            jsoncard = 
               "{\"@type\":\"MessageCard\"," +
                "\"@context\":\"https://schema.org/extensions\"," +
                "\"summary\":\"this is a summary\","+
                "\"sections\":[{\"title\":\"**Pending approval** from Tine Libbrecht\"," +
                "\"activityImage\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/1/12/User_icon_2.svg/1024px-User_icon_2.svg.png\"," +
                $"\"activityTitle\":\"Approve new user in GreetingService: {user.Email}\"," +
                $"\"activitySubtitle\":\"{user.FirstName} {user.LastName}\"," +
                "\"facts\":[{" +
                "\"name\":\"Date submitted:\"," +
                "\"value\":\"" + DateTime.Now.ToString("dd MMMM yyyy HH: mm") + "\"}," +
                "{\"name\":\"Details:\"," +
                "\"value\":\"Please approve or reject the new user for the GreetingService\"}]}," +
                "{\"potentialAction\":[{" +
                "\"@type\":\"HttpPOST\"," +
                "\"name\":\"Approve\"," +
                "\"target\":\"" + $"https://anewgreetingservice.azurewebsites.net/api/approve?approvalCode={user.ApprovalCode}" + "\"}," +
                "{\"@type\":\"HttpPOST\"," +
                "\"name\":\"Reject\"," +
                $"\"target\":\"https://anewgreetingservice.azurewebsites.net/api/rejection?approvalCode={user.ApprovalCode}" + "\"}]}]}";
        }
        
    }
}
    