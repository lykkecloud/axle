// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Axle.Constants;
    using Axle.Dto;
    using Axle.Persistence;
    using Axle.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using NSwag.Annotations;
    using PermissionsManagement.Client.Attribute;

    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : Controller
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ISessionService sessionService;

        public SessionsController(
            ISessionRepository sessionRepository,
            ISessionService sessionService)
        {
            this.sessionRepository = sessionRepository;
            this.sessionService = sessionService;
        }

        [Authorize(AuthorizationPolicies.System)]
        [HttpGet("{userName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserSessionResponse))]
        public async Task<IActionResult> Get(string userName)
        {
            var sessionState = await this.sessionRepository.GetByUser(userName);

            if (sessionState == null)
            {
                return this.NotFound();
            }

            return this.Ok(new UserSessionResponse { UserSessionId = sessionState.SessionId });
        }

        [AuthorizeUser(Permissions.CancelSession)]
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(TerminateSessionResponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(TerminateSessionResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(TerminateSessionResponse))]
        public async Task<IActionResult> TerminateSession([BindRequired] [FromQuery] string accountId)
        {
            var result = await this.sessionService.TerminateSession(null, accountId, false);

            if (result.Status == TerminateSessionStatus.NotFound)
            {
                return this.NotFound(result);
            }

            if (result.Status == TerminateSessionStatus.Failed)
            {
                return this.StatusCode((int)HttpStatusCode.InternalServerError, result);
            }

            return this.Ok(result);
        }
    }
}
