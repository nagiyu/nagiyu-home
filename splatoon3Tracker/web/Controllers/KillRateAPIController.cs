﻿using Microsoft.AspNetCore.Mvc;
using Nagiyu.Splatoon3Tracker.Service.Models;
using Nagiyu.Splatoon3Tracker.Service.Services;
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
        [Route("api/splatoon3/kill-rate")]
        public async Task<IActionResult> GetKillRates()
        {
            var response = await killRateService.GetKillRates();

            return Json(response);
        }

        [HttpPost]
        [Route("api/splatoon3/kill-rate")]
        public async Task<IActionResult> AddKillRate([FromBody] KillRate killRate)
        {
            var response = await killRateService.AddKillRate(killRate);

            return Json(response);
        }

        [HttpPut]
        [Route("api/splatoon3/kill-rate/{id}")]
        public async Task<IActionResult> UpdateKillRate([FromRoute] string id, [FromBody] KillRate killRate)
        {
            await killRateService.UpdateKillRate(id, killRate);

            return Ok();
        }

        [HttpDelete]
        [Route("api/splatoon3/kill-rate/{id}")]
        public async Task<IActionResult> DeleteKillRate([FromRoute] string id)
        {
            await killRateService.DeleteKillRate(id);

            return Ok();
        }
    }
}
