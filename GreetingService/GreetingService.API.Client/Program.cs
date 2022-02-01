using System;
using GreetingService.API;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace GreetingService.API.Client;

public class Program
{
    private static HttpClient _httpClient = new();

    private static async Task GetGreetingsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5159/api/Greeting");
            response.EnsureSuccessStatusCode();                                                 //throws exception if HTTP response status is not a success status
            var responseBody = await response.Content.ReadAsStringAsync();

            //Do something with response
            var greetings = JsonSerializer.Deserialize<IEnumerable<Greeting>>(responseBody);

            foreach (var greeting in greetings)
            {
                Console.WriteLine($"[{greeting.id}] [{greeting.timestamp}] ({greeting.from} -> {greeting.to}) - {greeting.message}");
            }

            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Get greetings failed: {e.Message}\\n");
        }
    }

    private static async Task GetGreetingAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://localhost:5159/api/Greeting/{id}");
            response.EnsureSuccessStatusCode();                                                 //throws exception if HTTP response status is not a success status
            var responseBody = await response.Content.ReadAsStringAsync();

            //Do something with response
            var greeting = JsonSerializer.Deserialize<Greeting>(responseBody);

            Console.WriteLine($"The greeting with {id} has message {greeting.message} and was sent from {greeting.from} -> {greeting.to}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
    }

    private static async Task TransformGreetingAsync()
    {
        var response = await _httpClient.GetAsync("http://localhost:5159/api/Greeting");
        response.EnsureSuccessStatusCode();                                                 //throws exception if HTTP response status is not a success status
        var responseBody = await response.Content.ReadAsStringAsync();

        //Do something with response
        var greetings = JsonSerializer.Deserialize<IEnumerable<Greeting>>(responseBody);

        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
        };

        var xmlWriter = XmlWriter.Create("xmlGreetingFile.xml", xmlWriterSettings);
        var serializer = new XmlSerializer(typeof(List<Greeting>));                             //this xml serializer does not support serializing interfaces, need to convert to a concrete class
        serializer.Serialize(xmlWriter, greetings.ToList());


    }
    //private static async Task WriteGreetingAsync(string message)
    //private static async Task UpdateGreetingAsync(Guid id, string message)

    public static void PrintAndReadOptions()
    {
        Console.WriteLine("Which task to you want to execute?");
        Console.WriteLine("1 - Print all greetings from repo to console");
        Console.WriteLine("2 - Print a given greeting according to ID");
        Console.WriteLine("3 - Transform the json greeting repo to xml");
    }

    public static async Task Main(string[] args)
    {

	    Console.WriteLine("                                           I8                                   ");
	    Console.WriteLine("                                           I8                                   ");
	    Console.WriteLine("                                        88888888 gg                             ");
	    Console.WriteLine("                                           I8    ''                             ");
	    Console.WriteLine("   ,gggg,gg   ,gggggg,   ,ggg,    ,ggg,    I8    gg    ,ggg,,ggg,     ,gggg,gg  ");
	    Console.WriteLine("  dP'  'Y8I   dP''''8I  i8' '8i  i8' '8i   I8    88   ,8' '8P' '8,   dP'  'Y8I  ");
	    Console.WriteLine(" i8'    ,8I  ,8'    8I  I8, ,8I  I8, ,8I  ,I8,   88   I8   8I   8I  i8'    ,8I  ");
	    Console.WriteLine(",d8,   ,d8I ,dP     Y8, `YbadP'  `YbadP' ,d88b,_,88,_,dP   8I   Yb,,d8,   ,d8I  ");
	    Console.WriteLine("P'Y8888P''8888P      `Y8888P'Y888888P'Y8888P''Y88P''Y88P'   8I   `Y8P'Y8888P'888");
	    Console.WriteLine("       ,d8I'                                                              ,d8I' ");
	    Console.WriteLine("     ,dP'8I                                                             ,dP'8I  ");
	    Console.WriteLine("    ,8'  8I                                                            ,8'  8I  ");
	    Console.WriteLine("    I8   8I                                                            I8   8I  ");
	    Console.WriteLine("    `8, ,8I                                                            `8, ,8I  ");
	    Console.WriteLine("     `Y8P'                                                              `Y8P''  ");


        Console.WriteLine();
        PrintAndReadOptions();
        string res = Console.ReadLine();

        switch (res)
        {
            case "1":
                await GetGreetingsAsync();
                break;
            case "2": 
                Console.WriteLine("What is the ID of the greeting?");
                var myID = Console.ReadLine();
                await GetGreetingAsync(Guid.Parse(myID));
                break;
            case "3":
                await TransformGreetingAsync();
                break;

        }





        


    }
}