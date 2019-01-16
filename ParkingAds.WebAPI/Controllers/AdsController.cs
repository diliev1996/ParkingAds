using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ParkingAds.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AdsController : Controller
    {
        public static string imageStr;
      
        [HttpGet]
        public string Get()
        {
            Console.WriteLine(imageStr);
            return "asdasdas";
        }

        // POST api/ads
        [HttpPost]
        public void Post(string base64str)
        {
            if (!string.IsNullOrEmpty(base64str))
            {
                imageStr = base64str;
            }
        }
    }
}
