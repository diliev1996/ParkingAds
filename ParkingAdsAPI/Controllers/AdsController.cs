using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace ParkingAdsAPI.Controllers
{
    public class RootObject
    {
        public (DateTime, string) ImageData { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        public (DateTime, string) ImageData { get; set; }
        public static IConfigurationRoot configuration { get; set; }
        public static HttpClient client { get; set; }

        public AdsController()
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"C:\Users\deobo\Source\repos\MailSender\ParkingAdsAPI\adsService.json", optional: true, reloadOnChange: true).Build();

            var setting = configuration["addServiceAddress"];
            client = new HttpClient
            {
                BaseAddress = new Uri(setting),
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Get();
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                var now = DateTime.UtcNow;
                if (ImageData.Item1.AddMinutes(2) > now)
                {
                    var resultByte = client.GetByteArrayAsync(configuration["addServiceAPIEndpoint"]).GetAwaiter().GetResult();
                    string result = System.Text.Encoding.UTF8.GetString(resultByte);
                    var root = JsonConvert.DeserializeObject<RootObject>(result);
                    using (var ms = new MemoryStream(Convert.FromBase64String(root.ImageData.Item2)))
                    {
                        using (var bitmap = new Bitmap(ms))
                        {
                            ImageData = (now, result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //not a picture
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message);
            }

            return ImageData.Item2;
        }
    }
}