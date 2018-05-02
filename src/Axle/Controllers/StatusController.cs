namespace Axle.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult IsAlive()
        {
            return this.Ok(Process.GetCurrentProcess().Id);
        }
    }
}
