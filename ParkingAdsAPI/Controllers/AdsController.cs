using Microsoft.AspNetCore.Mvc;

namespace ParkingAdsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        public static string imageStr;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Post(string base64str)
        {
            if (!string.IsNullOrEmpty(base64str))
            {
                imageStr = base64str;
            }
        }

        [HttpGet]
        public string Get()
        {
            return imageStr;
        }
    }
}