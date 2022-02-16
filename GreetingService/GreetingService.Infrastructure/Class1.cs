using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs.Models;

namespace GreetingService.Infrastructure
{
    public class BlobGreetingRepository : IGreetingRepository
    {

        private readonly BlobContainerClient _container;

        private readonly string _filepPath = "blobfile.json";
        
        public BlobGreetingRepository(string connectionString)
        {
           
            BlobContainerClient _container = new BlobContainerClient(connectionString, "myfirstcontainer");
            _container.Create();
        }

        public async Task CreateAsync(Greeting greeting)
        {
            //string jsonString = JsonSerializer.Serialize(greeting);
            MemoryStream mystream = new MemoryStream();
            await JsonSerializer.SerializeAsync(mystream, greeting, greeting.GetType());

            _container.UploadBlob("first", mystream);

        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {
           
            var memorystream = new MemoryStream();
            var myClient = new BlobClient(new Uri("https://tinesblobstorage.blob.core.windows.net/myfirstcontainer/first"));
            myClient.DownloadTo(memorystream);
            IEnumerable<Greeting> myGreetings = JsonSerializer.Deserialize<IEnumerable<Greeting>>(memorystream);

            return myGreetings;
            //throw new NotImplementedException();
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }
    }
}
