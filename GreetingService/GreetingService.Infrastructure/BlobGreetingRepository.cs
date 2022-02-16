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

        public BlobContainerClient _container;

        //private readonly string _filepPath = "blobfile.json";
        
        public BlobGreetingRepository(string connectionString)
        {
            //Check out this part: does not wrk when the container must be created. Right now it's hardcoded
            //BlobContainerClient _container = new BlobContainerClient(connectionString, "greetingcontainer");
            //_container.CreateIfNotExists();
            var serviceClient = new BlobServiceClient(connectionString);
            _container = serviceClient.GetBlobContainerClient("greetingcontainer");
         
        }

        public async Task CreateAsync(Greeting greeting)
        {
            //MemoryStream mystream = new MemoryStream();
            //await JsonSerializer.SerializeAsync(mystream, greeting, greeting.GetType());

            //mystream.Position = 0;

            var binarycontent = new BinaryData(greeting, new JsonSerializerOptions { WriteIndented = true }) ;
            var myTime = DateTime.Now;
            _container.UploadBlob($"{myTime.Year}/{myTime.Month}/{myTime.Day}/{greeting.id}", binarycontent);

        }

        public async Task<Greeting> GetAsync(Guid id)
        {
            var myBlobs = _container.GetBlobsAsync();
            var myGreetings = new List<Greeting>();
            await foreach (var b in myBlobs)
            {
                var blobClient = _container.GetBlobClient(b.Name);
                var mycontent = await blobClient.DownloadContentAsync();
                var myGreeting = mycontent.Value.Content.ToObjectFromJson<Greeting>();
                if (myGreeting.id == id)
                {
                    return myGreeting;
                } 
            }
            return null;

        }

        public async Task<IEnumerable<Greeting>> GetAsync()
        {

            
            var myBlobs = _container.GetBlobsAsync();
            var myGreetings = new List<Greeting>();
            await foreach (var b in myBlobs)
            {
                var blobClient = _container.GetBlobClient(b.Name);
                var mycontent = await blobClient.DownloadContentAsync();
                var myGreeting = mycontent.Value.Content.ToObjectFromJson<Greeting>();
                myGreetings.Add(myGreeting);            
            }

            return myGreetings;
        }

        public async Task UpdateAsync(Greeting greeting)
        {
            throw new NotImplementedException();
        }
    }
}
