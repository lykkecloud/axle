// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Controllers
{
    using System.Net;
    using Axle.Constants;
    using Axle.Dto;
    using Axle.Persistence;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NSwag.Annotations;

    [Authorize(AuthorizationPolicies.System)]
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : Controller
    {
        private readonly ISessionRepository sessionRepository;

        public SessionsController(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        [HttpGet("{userId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserSessionResponse))]
        public IActionResult Get(string userId)
        {
            var sessionState = this.sessionRepository.GetByUser(userId);

            if (sessionState == null)
            {
                return this.NotFound();
            }

            return this.Ok(new UserSessionResponse { UserSessionId = sessionState.SessionId });
        }
    }
}
