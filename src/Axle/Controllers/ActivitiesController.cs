// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

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
        private readonly ISessionLifecycleService sessionLifecycleService;

        public ActivitiesController(IActivityService activityService, ISessionLifecycleService sessionLifecycleService)
        {
            this.activityService = activityService;
            this.sessionLifecycleService = sessionLifecycleService;
        }

        [HttpPost("login")]
        [SwaggerResponse(HttpStatusCode.OK, null)]
        public async Task<IActionResult> Login(string accountId)
        {
            var userName = this.User.GetUsername();
            var sessionId = this.sessionLifecycleService.GenerateSessionId();

            var activity = new SessionActivity(SessionActivityType.Login, sessionId, userName, accountId);

            await this.activityService.PublishActivity(activity);

            return this.Ok();
        }
    }
}
