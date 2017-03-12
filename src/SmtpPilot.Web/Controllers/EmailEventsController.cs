using Microsoft.AspNetCore.Mvc;
using SmtpPilot.WebHooks.Models;
using System;
using System.Linq;

namespace SmtpPilot.Web.Controllers
{
    [Route("smtppilot/event")]
    public class EmailEventsController : Controller
    {
        [Route("status")]
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok("Service Online");
        }

        [Route("email")]
        [HttpPost]
        public IActionResult PostEmailEvent([FromBody] EmailProcessedServerEvent emailEvent)
        {
            return Ok();
        }
    }
}