using Xunit;
using GreetingService;
using System.Linq;
using System.Threading.Tasks;
using GreetingService.Infrastructure.GreetingRepository;

namespace GreetingService.Infrastructure.Test
{
    public class UnitTests
    {
        [Fact]
        public async Task test_memorygreetingrepo_createAsync()
        {
            var myRepo = new MemoryGreetingRepository();
            var myGreeting = new Core.Entities.Greeting();

            var ListRepo = await myRepo.GetAsync();
            Assert.Empty((System.Collections.IEnumerable)ListRepo);

            _ = myRepo.CreateAsync(myGreeting);
            ListRepo = await myRepo.GetAsync();

            Assert.NotNull(myGreeting);
            Assert.NotEmpty(ListRepo);
            Assert.Single(ListRepo.ToList());

        }

        [Fact]
        public async Task test_memorygreetingrepo_getid()
        {
            var myRepo = new MemoryGreetingRepository();
            var myGreeting = new Core.Entities.Greeting();
            var myID = myGreeting.id;

            await myRepo.CreateAsync(myGreeting);
            var newGreeting = await myRepo.GetAsync(myID);

            Assert.Equal(myGreeting, newGreeting);


        }


    }
}