using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ParkingAds.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "booking";
        }

        [DisableCors]
        [HttpPost]
        public ActionResult Post([FromBody] Object parkingInfo)
        {
            Console.WriteLine(parkingInfo);
            Console.WriteLine("booking post");

            //var process = new Process()
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        FileName = "/usr/bin/python",
            //        Arguments = "publish.py",
            //        RedirectStandardOutput = true,
            //        UseShellExecute = false,
            //        CreateNoWindow = true,
            //    }
            //};
            //process.Start();

            return Ok(parkingInfo);
        }
    }
}
