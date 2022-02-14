using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

SenderEntry.Run();

internal class SenderEntry
{
    // connection string to your Service Bus namespace
    static string connectionString = "Endpoint=sb://richards-servicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SfYcU3lcVSRftV0NXeJPYl1HhR3fsWp7FrPsdISflIg=";

    // name of your Service Bus queue
    static string queueName = "MyQueue";

    // the client that owns the connection and can be used to create senders and receivers
    static ServiceBusClient client;

    // the sender used to publish messages to the queue
    static ServiceBusSender sender;

    // number of messages to be sent to the queue
    private const int numOfMessages = 3;

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public static void Run()
    {
        // The Service Bus client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when messages are being published or read
        // regularly.
        //
        // Create the clients that we'll use for sending and processing messages.
        client = new ServiceBusClient(connectionString);
        sender = client.CreateSender(queueName);

        // This is our list of people
        var people = new List<Person>();

        people.Add(new Person()
        {
            Name = "King Richard",
            Age = 21,
        });
        people.Add(new Person()
        {
            Name = "Quenn Savi",
            Age = 21,
        });
        people.Add(new Person()
        {
            Name = "Gypsy Filip",
            Age = 21,
        });


        // create a batch 
        using ServiceBusMessageBatch messageBatch = sender.CreateMessageBatchAsync().GetAwaiter().GetResult();

        foreach (var person in people)
        {
            sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(person)));
        }

        sender.DisposeAsync();
        client.DisposeAsync();


        Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");

        Console.WriteLine("Press any key to end the application");
        Console.ReadKey();
    }
}













