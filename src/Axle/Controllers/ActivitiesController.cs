// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Contracts;
    using Axle.Extensions;
    using Axle.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NSwag.Annotations;

    [Authorize(AuthorizationPolicies.Mobile)]
    [Route("api/accounts/{accountId}/[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly IActivityService activityService;
        private readonly ISessionService sessionService;

        public ActivitiesController(IActivityService activityService, ISessionService sessionService)
        {
            this.activityService = activityService;
            this.sessionService = sessionService;
        }

        [HttpPost("login")]
        [SwaggerResponse(HttpStatusCode.OK, null)]
        public async Task<IActionResult> Login(string accountId)
        {
            var userName = this.User.GetUsername();
            var sessionId = this.sessionService.GenerateSessionId();

            var activity = new SessionActivity(SessionActivityType.Login, sessionId, userName, accountId);

            await this.activityService.PublishActivity(activity);

            return this.Ok();
        }
    }
}
