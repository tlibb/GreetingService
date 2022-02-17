using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using GreetingService.Core.Entities;
using System.Text;

namespace GreetingService.API.Function
{
    public class ConvertGreetingToCsv
    {
        [FunctionName("ConvertGreetingToCsv")]
        public async Task Run([BlobTrigger("greetingcontainer/{name}", Connection = "LoggingStorageAccount")] Stream greetingsJsonBlob, string name, [Blob("greetings-csv/{name}", FileAccess.Write, Connection = "LoggingStorageAccount")] Stream greetingCsvBlob, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {greetingsJsonBlob.Length} Bytes");

            var g = JsonSerializer.Deserialize<Greeting>(greetingsJsonBlob);
            var csvstreamwriter = new StreamWriter(greetingCsvBlob);


            //StringBuilder sb = new StringBuilder();

            //sb.AppendLine($"Message;From;To;TimeStamp;Guid\n");
            //sb.AppendLine($"{g.Message};{g.From};{g.To};{g.TimeStamp};{g.id}");
           
            csvstreamwriter.WriteLine($"Message;From;To;TimeStamp;Guid");
            csvstreamwriter.WriteLine($"{g.Message};{g.From};{g.To};{g.TimeStamp};{g.id}");
            await csvstreamwriter.FlushAsync();


            //csvstreamwriter.Write($"{g.Message};{g.From};{g.To};{g.TimeStamp};{g.id}");


        

        }
    }
}
