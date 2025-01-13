using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nagiyu.Splatoon3Tracker.Service.Consts;

namespace Nagiyu.Splatoon3Tracker.Web.Controllers
{
    public class KillRateController : Controller
    {
        [Authorize(Policy = Splatoon3Consts.KILL_RATE_POLICY)]
        [Route("splatoon3/kill-rate")]
        public IActionResult KillRate()
        {
            return View();
        }
    }
}
