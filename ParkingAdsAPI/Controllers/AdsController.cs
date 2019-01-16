using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using Microsoft.AspNetCore.Cors;

namespace ParkingAdsAPI.Controllers
{
    public class RootObject
    {
        public string ImageData { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        public static (DateTime, string) ImageData { get; set; }
        public static IConfiguration _configuration { get; set; }
        public static HttpClient client { get; set; }

        public AdsController(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient
            {
                BaseAddress = new Uri(configuration["addServiceAddress"]),
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        [DisableCors]
        public (DateTime, string) Get()
        {
            try
            {
                var now = DateTime.UtcNow;
                if (ImageData.Item1.AddMinutes(2) < now)
                {
                    var adsApiEndpoint = _configuration["addServiceAPIEndpoint"];
                    var resultByte = client.GetByteArrayAsync(adsApiEndpoint).GetAwaiter().GetResult();
                    var resultAsString = System.Text.Encoding.UTF8.GetString(resultByte);
                    var root = JsonConvert.DeserializeObject<RootObject>(resultAsString);
                    var imageStr = root.ImageData;
                    using (var ms = new MemoryStream(Convert.FromBase64String(imageStr)))
                    {
                        using (var bitmap = new Bitmap(ms))
                        {
                            ImageData = (now, imageStr);
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

            return ImageData;
        }
    }
}