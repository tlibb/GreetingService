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

namespace GreetingService.Infrastructure.GreetingRepository
{
    public class BlobGreetingRepository : IGreetingRepository
    {

        public BlobContainerClient _container;


        
        public BlobGreetingRepository(IConfiguration config)
        {
            _container = new BlobContainerClient(config["LoggingStorageAccount"], "greetingcontainer");
            _container.CreateIfNotExists();
            
         
        }

        public async Task CreateAsync(Greeting greeting)
        {
            
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
