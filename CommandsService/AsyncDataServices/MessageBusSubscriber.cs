using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace CommandsService.AsyncDataServices
{
    /// <summary>
    /// This will run in the background, and listen to incoming PlatformService messages (events) from the RabbitMQ MessageBus
    /// MessageBusSubscriber will be registered in Startup.cs as a HostedService, and when it starts, ExecuteAsync will run and start to listen for events.
    /// </summary>
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
        {
            _config = config;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            //Configure RabbitMQ
            var factory = new ConnectionFactory() { HostName = _config["RabbitMQHost"], Port = int.Parse(_config["RabbitMQPort"]) };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");

            Console.WriteLine("--> Listenting on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
        }      

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> RabbitMQ Connection Bus Shutdown");
        }

        public override void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
