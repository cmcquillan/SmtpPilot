using Microsoft.AspNetCore.Mvc;
using SmtpPilot.WebHooks.Models;
using System;
using System.Linq;

namespace SmtpPilot.WebHooks.Controllers
{
    [Route("event")]
    public class EmailEventsController : ControllerBase
    {
        [Route("status")]
        [HttpGet]
        public virtual IActionResult GetStatus()
        {
            return Ok("Service Online");
        }

        [Route("email")]
        [HttpPost]
        public virtual IActionResult PostEmailEvent([FromBody] EmailProcessedServerEvent emailEvent)
        {
            return Ok();
        }
    }
}