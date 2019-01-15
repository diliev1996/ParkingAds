using System;
using System.Net.Http;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace AdsPicker
{
    public class RootObject
    {
        public string ImageData { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(configuration["addServiceAddress"]),
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            while (true)
            {
                try
                {
                    var resultByte = client.GetByteArrayAsync(configuration["addServiceAPIEndpoint"]).GetAwaiter().GetResult();
                    string result = System.Text.Encoding.UTF8.GetString(resultByte);
                    var root = JsonConvert.DeserializeObject<RootObject>(result);
                    using (var ms = new MemoryStream(Convert.FromBase64String(root.ImageData)))
                    {
                        using (var bitmap = new Bitmap(ms))
                        {
                            Console.WriteLine("Success");
                            client.PostAsync(configuration["parkingAdsAddress"] + configuration["parkingAdsAPIEndpoint"],
                                new ByteArrayContent(resultByte));
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