using Microsoft.AspNetCore.Mvc;
using Nagiyu.Splatoon3Tracker.Service.Exceptions;
using Nagiyu.Splatoon3Tracker.Service.Models.Requests;
using Nagiyu.Splatoon3Tracker.Service.Services;
using System;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Web.Controllers
{
    public class KillRateAPIController : Controller
    {
        private readonly KillRateService killRateService;

        public KillRateAPIController(KillRateService killRateService)
        {
            this.killRateService = killRateService;
        }

        [HttpGet]
        [Authorize(Policy = Splatoon3Consts.KILL_RATE_POLICY)]
        [Route("api/splatoon3/kill-rate")]
        public async Task<IActionResult> GetKillRates()
        {
            return await ActionResult(async () =>
            {
                var response = await killRateService.GetKillRates();
                return Json(response);
            });
        }

        [HttpPost]
        [Authorize(Policy = Splatoon3Consts.KILL_RATE_POLICY)]
        [Route("api/splatoon3/kill-rate")]
        public async Task<IActionResult> AddKillRate([FromBody] KillRateRequest killRate)
        {
            return await ActionResult(async () =>
            {
                var response = await killRateService.AddKillRate(killRate);
                return Json(response);
            });
        }

        [HttpPut]
        [Authorize(Policy = Splatoon3Consts.KILL_RATE_POLICY)]
        [Route("api/splatoon3/kill-rate/{id}")]
        public async Task<IActionResult> UpdateKillRate([FromRoute] string id, [FromBody] KillRateRequest killRate)
        {
            return await ActionResult(async () =>
            {
                await killRateService.UpdateKillRate(id, killRate);
                return Ok();
            });
        }

        [HttpDelete]
        [Authorize(Policy = Splatoon3Consts.KILL_RATE_POLICY)]
        [Route("api/splatoon3/kill-rate/{id}")]
        public async Task<IActionResult> DeleteKillRate([FromRoute] string id)
        {
            return await ActionResult(async () =>
            {
                await killRateService.DeleteKillRate(id);
                return Ok();
            });
        }

        /// <summary>
        /// ActionResult
        /// </summary>
        /// <param name="func">処理</param>
        private async Task<IActionResult> ActionResult(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func();
            }
            catch (ParameterException e)
            {
                return StatusCode(400, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
