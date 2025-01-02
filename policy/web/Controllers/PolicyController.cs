using Microsoft.AspNetCore.Mvc;

namespace Nagiyu.Policy.Web.Controllers
{
    public class PolicyController : Controller
    {
        /// <summary>
        /// プライバシーポリシー
        /// </summary>
        [Route("privacy-policy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        /// <summary>
        /// 利用規約
        /// </summary>
        [Route("terms")]
        public IActionResult Terms()
        {
            return View();
        }
    }
}
