using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core.Entities
{
    public interface IApprovalService
    {
        public Task BeginUserApprovalAsync(User user);
    }
}
