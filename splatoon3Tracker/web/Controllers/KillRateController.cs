using Microsoft.AspNetCore.Mvc;

namespace Nagiyu.Splatoon3Tracker.Web.Controllers
{
    public class KillRateController : Controller
    {
        [Route("splatoon3/kill-rate")]
        public IActionResult KillRate()
        {
            return View();
        }
    }
}
