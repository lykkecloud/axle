// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Constants;
    using Dto;
    using Persistence;
    using Services;
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
        [Obsolete("Use GET /for-support/{userName} and GET /for-user/{accountId}")]
        public async Task<IActionResult> Get(string userName)
        {
            var sessionId = await sessionRepository.GetSessionIdByUser(userName);

            if (sessionId == null)
            {
                return NotFound();
            }

            return Ok(new UserSessionResponse { UserSessionId = sessionId.Value });
        }

        [Authorize(AuthorizationPolicies.System)]
        [HttpGet("for-support/{userName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserSessionResponse))]
        public async Task<IActionResult> GetSessionForSupport(string userName)
        {
            var sessionId = await sessionRepository.GetSessionIdByUser(userName);

            if (sessionId == null)
            {
                return NotFound();
            }

            return Ok(new UserSessionResponse { UserSessionId = sessionId.Value });
        }

        [Authorize(AuthorizationPolicies.System)]
        [HttpGet("for-user/{accountId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserSessionResponse))]
        public async Task<IActionResult> GetSessionForUser(string accountId)
        {
            var sessionId = await sessionRepository.GetSessionIdByAccount(accountId);

            if (sessionId == null)
            {
                return NotFound();
            }

            return Ok(new UserSessionResponse { UserSessionId = sessionId.Value });
        }

        [AuthorizeUser(Permissions.CancelSession)]
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(TerminateSessionResponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(TerminateSessionResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(TerminateSessionResponse))]
        public async Task<IActionResult> TerminateSession([BindRequired] [FromQuery] string accountId)
        {
            var result = await sessionService.TerminateSession(null, accountId, false);

            if (result.Status == TerminateSessionStatus.NotFound)
            {
                return NotFound(result);
            }

            if (result.Status == TerminateSessionStatus.Failed)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }

            return Ok(result);
        }
    }
}
