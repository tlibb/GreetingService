namespace GreetingService.Core.Entities
{
    public interface IGreetingRepository
    {
        public Greeting Get(Guid id);
        public IEnumerable<Greeting> Get();
        public void Create(Greeting greeting);
        public void Update(Greeting greeting);
    }
}
