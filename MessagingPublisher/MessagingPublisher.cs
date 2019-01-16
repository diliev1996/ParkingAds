using RabbitMQ.Client;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MessagingPublisher
{
    public class MessagingPublisher
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

        public static void SendMessage(
                    string emailBody = "this is the body", 
                    string address = "diliev1996@gmail.com",
                    string subject= "RabbitMQ")
        {
            var factory = new ConnectionFactory() { 
                HostName = "amqp://sgxsvkdw:zmeCytKeq8wlXGgkn44Z23XU0cC1_aY1@bee.rmq.cloudamqp.com/sgxsvkdw"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = "email-out";
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var email = new Email
                {
                    Body = emailBody,
                    Address = address,
                    Subject = subject
                };
                var body = Serialize(email);

                channel.BasicPublish(exchange: "X",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

            }

        }
    }
}
