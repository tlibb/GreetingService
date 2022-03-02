using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Core.Entities
{
    public interface IInvoiceService
    {
        public Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month);
        public Task<Invoice> GetInvoiceAsync(int year, int month, string email);
        public Task CreateOrUpdateInvoiceAsync(Invoice invoice);

    }
}
