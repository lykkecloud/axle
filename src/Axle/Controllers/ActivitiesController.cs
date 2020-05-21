// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using Swashbuckle.AspNetCore.Annotations;

namespace Axle.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Constants;
    using Contracts;
    using Extensions;
    using Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        [SwaggerResponse((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Login(string accountId)
        {
            var userName = User.GetUsername();
            var sessionId = await sessionService.GenerateSessionId();

            var activity = new SessionActivity(SessionActivityType.Login, sessionId, userName, accountId);

            await activityService.PublishActivity(activity);

            return Ok();
        }
    }
}
