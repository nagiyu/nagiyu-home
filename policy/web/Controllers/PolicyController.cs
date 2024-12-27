using Microsoft.AspNetCore.Mvc;

namespace Nagiyu.Policy.Web.Controllers
{
    public class PolicyController : Controller
    {
        /// <summary>
        /// プライバシーポリシー
        /// </summary>
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        /// <summary>
        /// 利用規約
        /// </summary>
        public IActionResult Terms()
        {
            return View();
        }
    }
}
