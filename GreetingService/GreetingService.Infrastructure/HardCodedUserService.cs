using GreetingService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure
{
    public class HardCodedUserService : IUserService
    {
        private Dictionary<string, string>? _userDataBase;

        public HardCodedUserService()
        {
            _userDataBase = new Dictionary<string,string>();
            _userDataBase.Add("Tine", "pw0");
            _userDataBase.Add("Koen", "pw1");
            _userDataBase.Add("June", "pw2");
            _userDataBase.Add("Ebba", "pw3");
            _userDataBase.Add("Alvin", "pw4");
        }
        public bool IsValidUser(string username, string password)
        {
            return (_userDataBase.ContainsKey(username) && _userDataBase[username] == password);
            
        }
    }
}
