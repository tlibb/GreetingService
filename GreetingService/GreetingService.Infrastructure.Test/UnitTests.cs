using Xunit;
using GreetingService;
using System.Linq;

namespace GreetingService.Infrastructure.Test
{
    public class UnitTests
    {
        [Fact]
        public void test_memorygreetingrepo_create()
        {
            var myRepo = new MemoryGreetingRepository();
            var myGreeting = new Core.Entities.Greeting();

            var ListRepo = myRepo.Get();
            Assert.Empty(ListRepo);

            myRepo.Create(myGreeting);
            ListRepo = myRepo.Get();

            Assert.NotNull(myGreeting);
            Assert.NotEmpty(ListRepo);
            Assert.Equal(1, ListRepo.ToList().Count);

        }

        [Fact]
        public void test_memorygreetingrepo_getid()
        {
            var myRepo = new MemoryGreetingRepository();
            var myGreeting = new Core.Entities.Greeting();
            var myID = myGreeting.id;

            myRepo.Create(myGreeting);
            var newGreeting = myRepo.Get(myID);

            Assert.Equal(myGreeting, newGreeting);


        }


    }
}