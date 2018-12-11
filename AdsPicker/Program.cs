using System;
using System.Net.Http;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace AdsPicker
{
    public class RootObject
    {
        public string ImageData { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    HttpClient client = new HttpClient
                    {
                        BaseAddress = new Uri("http://adservice.ws.dm.sof60.dk/"),
                    };
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var result = client.GetStringAsync("api/ad").GetAwaiter().GetResult();
                    var root = JsonConvert.DeserializeObject<RootObject>(result);
                    using (var ms = new MemoryStream(Convert.FromBase64String(root.ImageData)))
                    {
                        using (var bitmap = new Bitmap(ms))
                        {
                            Console.WriteLine("Success");
                            //send string to server
                        }
                    }
                }
                catch (Exception e)
                {
                    //not a picture
                    Console.WriteLine("Exception");
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(1000 * 60 * 2);
            }
        }
    }
}