using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class Program
{
    public static void Main(string[] args)
    {
        string _hostname = "192.168.140.137";
        string _queueName = "user-queue";

        var connectionFactory = new ConnectionFactory() {
            HostName = _hostname
        };
        var connection = connectionFactory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        Console.WriteLine(" [?] Waiting for messages.");
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] Received: " + message);
                        };
        channel.BasicConsume(
            queue: _queueName, 
            autoAck: false, 
            consumer: consumer
        );

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            Console.WriteLine(" [*] Exiting...");
            Environment.Exit(0);
        };

        while (true)
        {
            System.Threading.Thread.Sleep(1000);
        }
    }
}