// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Controllers
{
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/isAlive")]
    public class IsAliveController : ControllerBase
    {
        private static readonly object Version =
            new
            {
                Title = typeof(Program).Assembly.Attribute<AssemblyTitleAttribute>(attribute => attribute.Title),
                Version = typeof(Program).Assembly.Attribute<AssemblyInformationalVersionAttribute>(attribute => attribute.InformationalVersion),
                OS = System.Runtime.InteropServices.RuntimeInformation.OSDescription.TrimEnd(),
                ProcessId = Process.GetCurrentProcess().Id,
            };

        [HttpGet]
        public IActionResult Get() => this.Ok(Version);
    }
}
