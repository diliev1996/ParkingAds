using RabbitMQ.Client;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ConsoleApp1
{
    public class Program
    {
        [Serializable]
        public class Email
        {
            public string Address { get; set; }
            public string Body { get; set; }
            public string Subject { get; set; }
        }

        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = "email";
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var email = new Email { Body = "this is the body", Address = "diliev1996@gmail.com", Subject = "RabbitMQ" };
                var body = Serialize(email);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine("Message sent");
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}