using Microsoft.AspNetCore.Mvc;

namespace Nagiyu.Tools.Web.Controllers
{
    public class ToolsController : Controller
    {
        /// <summary>
        /// 乗り換え変換
        /// </summary>
        [Route("tools/convert-yahoo-transit")]
        public IActionResult ConvertYahooTransit()
        {
            return View();
        }
    }
}
