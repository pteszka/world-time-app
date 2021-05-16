using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using world_time_app.Models;
using world_time_app.Services;

namespace world_time_app.Controllers
{
    [ApiController]
    [Route("api")]
    public class TimezonesController : ControllerBase
    {
        private readonly ITimezone _timezoneService;
        public TimezonesController(ITimezone timezoneService)
        {
            _timezoneService = timezoneService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (ModelState.IsValid)
            {
                var time = await _timezoneService.GetTimeAsync();
                if (time == null)
                {
                    return NotFound();
                }
                return Ok(time);
            }
            return BadRequest();
        }

        [HttpGet("timezones")]
        public async Task<IActionResult> GetTimezones()
        {
            if (ModelState.IsValid)
            {
                var timezones = await _timezoneService.GetTimezonesAsync();
                if (timezones == null)
                {
                    return NotFound();
                }
                return Ok(timezones);
            }
            return BadRequest();
        }
    }
}