using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;





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


    public static void Run()
    {
        // The Service Bus client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when messages are being published or read
        // regularly.
        //
        // Create the clients that we'll use for sending and processing messages.
        client = new ServiceBusClient(connectionString);
        sender = client.CreateSender(queueName);

        // create a batch 
        using ServiceBusMessageBatch messageBatch = sender.CreateMessageBatchAsync().GetAwaiter().GetResult();

        for (int i = 1; i <= numOfMessages; i++)
        {
            sender.SendMessageAsync(new ServiceBusMessage($"Message {i}"));
        }

        sender.DisposeAsync();
        client.DisposeAsync();


        Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");

        Console.WriteLine("Press any key to end the application");
        Console.ReadKey();
    }
}













