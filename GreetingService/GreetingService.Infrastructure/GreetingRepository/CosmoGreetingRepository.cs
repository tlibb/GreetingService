using GreetingService.Core.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GreetingService.Infrastructure.GreetingRepository
{
    public class CosmoGreetingRepository : IGreetingRepository
    {

        private readonly CosmosClient _client;
        private Database _database;
        private Container _container;

        public CosmoGreetingRepository(IConfiguration config)
        {
            _client = new CosmosClient(config["CosmoEndPointUri"], config["CosmoPrimaryKey"],  new CosmosClientOptions() { ApplicationName = "ANewGreetingService" });
            _database = _client.GetDatabase("GreetingRepository");
            _container = _client.GetContainer("GreetingRepository", "Greetings");



        }
        public async Task CreateAsync(Greeting greeting)
        {
            try
            {
                ItemResponse<Greeting> myGreetingResponse = await _container.CreateItemAsync<Greeting>(greeting, new PartitionKey(greeting.From));
                //ItemResponse<Greeting> myGreetingResponse = await _container.CreateItemAsync<Greeting>(greeting, new PartitionKey(greeting.From));
                //ItemResponse<Greeting> myGreetingResponse = await _container.CreateItemAsync<Greeting>(greeting);
                Console.WriteLine(myGreetingResponse.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id.ToString()}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            FeedResponse<Greeting> myGreetings = await queryResultSetIterator.ReadNextAsync();
            ItemResponse<Greeting> myResponse = await _container.DeleteItemAsync<Greeting>(id.ToString(), new PartitionKey(myGreetings.FirstOrDefault().From));
            
        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            //await _container.ReadItemAsync<Greeting>(id.ToString(), new PartitionKey());
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id.ToString()}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            FeedResponse<Greeting> myGreetings = await queryResultSetIterator.ReadNextAsync();
            return myGreetings.FirstOrDefault();
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
            //await _container.ReadItemAsync<Greeting>(id.ToString(), new PartitionKey());
            var sqlQueryText = $"SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            var myListOfGreetings = new List<Greeting>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Greeting> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Greeting greeting in currentResultSet)
                {
                    myListOfGreetings.Add(greeting);
                }
            }

            return myListOfGreetings;
        }

        public async Task<IEnumerable<Greeting>> GetAsync(string from, string to)
        {
            if (from == null && to == null)
            {
                return await this.GetAsync();
            }
            var sqlQueryText = $"SELECT * FROM c ";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            var myListOfGreetings = new List<Greeting>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Greeting> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Greeting greeting in currentResultSet)
                {
                    if ( from != null && greeting.From == from && to != null && greeting.To == to)
                    {
                        myListOfGreetings.Add(greeting);
                    }
                    else if (from == null && to != null && greeting.To == to)
                    {
                        myListOfGreetings.Add(greeting);
                    }
                    else if (to == null && from != null && greeting.From == from)
                    {
                        myListOfGreetings.Add(greeting);
                    }
                }
            }

            return myListOfGreetings;

        }

        public async Task<IEnumerable<Greeting>> GetAsync(string from, int year, int month)
        {
            
            var sqlQueryText = $"SELECT * FROM c ";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            var myListOfGreetings = new List<Greeting>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Greeting> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Greeting greeting in currentResultSet)
                {
                    if (greeting.From == from && greeting.TimeStamp.Year == year && greeting.TimeStamp.Month == month)
                    {
                        myListOfGreetings.Add(greeting);
                    }
                    
                }
            }

            return myListOfGreetings;

        }

        public async Task UpdateAsync(Greeting greeting)
        {
            //var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id.ToString()}'";
            //QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            //FeedIterator<Greeting> queryResultSetIterator = _container.GetItemQueryIterator<Greeting>(queryDefinition);
            //FeedResponse<Greeting> myGreetings = await queryResultSetIterator.ReadNextAsync();
            //Greeting myGreeting = myGreetings.FirstOrDefault();
            //myGreeting.From = greeting.From;
            //myGreeting.To = greeting.To;
            //myGreeting.Message = greeting.Message;
            await _container.ReplaceItemAsync(greeting, greeting.id.ToString());

        }
    }
}
