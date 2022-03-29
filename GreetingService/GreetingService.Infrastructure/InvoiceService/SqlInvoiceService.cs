using GreetingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.InvoiceService
{
    public class SqlInvoiceService : IInvoiceService
    {
        private readonly GreetingDbContext _greetingdbcontext;

        private readonly IGreetingRepository _greetingRepository;

        private int _trackid;

        public SqlInvoiceService(GreetingDbContext greetingDbContext, IGreetingRepository greetingRepository)
        {
            _greetingdbcontext = greetingDbContext;
            _greetingRepository = greetingRepository;
        }

        //input invoice should at least have a year, month, and user given
        public async Task CreateOrUpdateInvoiceAsync(Invoice invoice)
        {
            DateTime StartTime = new DateTime(invoice.Year, invoice.Month, 1);
            DateTime EndTime;
            var myGreetings = new List<Greeting>();
            var cosmoGreetings = new List<Greeting>();
            if (invoice.Month <= 11)
            {
                EndTime = new DateTime(invoice.Year, invoice.Month + 1, 1);
            }
            else if (invoice.Month == 12)
            {
                EndTime = new DateTime(invoice.Year + 1, 1, 1);
            }
            else throw new Exception("Something wrong with input month");


            try
            {
                myGreetings = await _greetingdbcontext.Greetings.Where(g => g.From == invoice.User.Email && g.TimeStamp >= StartTime && g.TimeStamp < EndTime).ToListAsync();
                //cosmoGreetings = (List<Greeting>)await _greetingRepository.GetAsync(invoice.User.Email, invoice.Year, invoice.Month);
                //myGreetings.AddRange(cosmoGreetings);
                //_greetingdbcontext.Add(cosmoGreetings);

                //await _greetingdbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }

            var myInvoice = await _greetingdbcontext.Invoices.Where(i => i.Id == invoice.Id).FirstOrDefaultAsync();


            if (myInvoice == null)
            {
                invoice.Greetings = myGreetings;
                invoice.Cost = myGreetings.Count() * 21;
                await _greetingdbcontext.Invoices.AddAsync(invoice);
            }
            else
            {
                myInvoice.Greetings = myGreetings;
                myInvoice.Cost = myGreetings.Count() * 21;
            }

            await _greetingdbcontext.SaveChangesAsync();     
            
        }

        public async Task<Invoice> GetInvoiceAsync(int year, int month, string email)
        {
            var myInvoice = await _greetingdbcontext.Invoices.Include(i => i.Greetings)
                                                             .Include(i => i.User)
                                                             .Where(i => i.Year == year && i.Month == month && i.User.Email == email).FirstOrDefaultAsync();
            return myInvoice;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int year, int month)
        {
            
            var myInvoices = await _greetingdbcontext.Invoices.Include(i => i.Greetings)
                                                              .Include(i => i.User)
                                                              .Where(i => i.Year == year && i.Month == month).ToListAsync();

            if (myInvoices == null)
            {
                throw new Exception("Nu such invoices");
            }
            return myInvoices;
        }
    }
}
