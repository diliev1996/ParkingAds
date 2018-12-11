using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SendMail
{
    public class Email
    {
        public string Address { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }

    internal class Program
    {
        private static T Deserialize<T>(byte[] param)
        {
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();
                return (T)br.Deserialize(ms);
            }
        }

        //maybe put into config
        private static readonly MailAddress fromAddress = new MailAddress("diliev1996@gmail.com", "From Delyan Iliev");

        private static readonly string fromPassword = "fromPassword";

        private static SmtpClient SetUpMailServer()
        {
            return new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
        }

        public static void Main(string[] args)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "email",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                       {
                           Console.WriteLine("Message received");
                           var body = ea.Body;
                           var emailMessage = Deserialize<Email>(body);
                           var smtp = SetUpMailServer();
                           using (var message = new MailMessage(fromAddress.Address,
                               emailMessage.Address,
                               emailMessage.Subject, emailMessage.Body))
                           {
                               using (smtp)
                               {
                                   smtp.Send(message);
                                   Console.WriteLine("Message sent");
                               }
                           }
                       };
                        channel.BasicConsume(queue: "email",
                                             autoAck: true,
                                             consumer: consumer);
                        Console.WriteLine("Listening");
                    }
                }
            }
            catch (Exception e)
            {
                //send to log queue
                Console.Write(e.Message);
            }

            //log success
            //send success event to sender
            Console.ReadLine();
        }
    }
}