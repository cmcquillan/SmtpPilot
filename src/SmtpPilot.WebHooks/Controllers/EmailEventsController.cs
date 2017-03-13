using Microsoft.AspNetCore.Mvc;
using SmtpPilot.WebHooks.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpPilot.WebHooks.Controllers
{
    [Route("event")]
    public class EmailEventsController : ControllerBase
    {
        [Route("status")]
        [HttpGet]
        public async virtual Task<IActionResult> GetStatus()
        {
            return await Task.FromResult(Ok("Service Online"));
        }

        [Route("email")]
        [HttpPost]
        public async virtual Task<IActionResult> PostEmailEvent([FromBody] EmailProcessedServerEvent emailEvent)
        {
            return await Task.FromResult(Ok());
        }
    }
}