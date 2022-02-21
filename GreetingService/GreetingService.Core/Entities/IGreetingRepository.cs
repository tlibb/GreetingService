namespace GreetingService.Core.Entities
{
    public interface IGreetingRepository
    {
        public Task<Greeting> GetAsync(Guid id);
        public Task<IEnumerable<Greeting>> GetAsync();
        public Task CreateAsync(Greeting greeting);
        public Task UpdateAsync(Greeting greeting);
        public Task<IEnumerable<Greeting>> GetAsync(string from, string to);
    }
}
