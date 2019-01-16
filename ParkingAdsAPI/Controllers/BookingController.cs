using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ParkingAdsAPI.Controllers
{
    [Route("api/[controller]")]
    public class BookingController : Controller
    {

        [HttpPost]
        [DisableCors]
        public ActionResult Post()
        {
            Console.WriteLine(123);
            MessagingPublisher.MessagingPublisher.SendMessage();
            return Ok();
        }
    }
}
