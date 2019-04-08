// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
        private readonly IAccountsService accountsService;
        private readonly ISessionLifecycleService sessionLifecycleService;

        public SessionsController(
            ISessionRepository sessionRepository,
            IAccountsService accountsService,
            ISessionLifecycleService sessionLifecycleService)
        {
            this.sessionRepository = sessionRepository;
            this.accountsService = accountsService;
            this.sessionLifecycleService = sessionLifecycleService;
        }

        [Authorize(AuthorizationPolicies.System)]
        [HttpGet("{userName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserSessionResponse))]
        public IActionResult Get(string userName)
        {
            var sessionState = this.sessionRepository.GetByUser(userName);

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
            // TODO: remove this call when sessions are stored by accountId
            var userName = await this.accountsService.GetAccountOwnerUserName(accountId);

            if (string.IsNullOrEmpty(userName))
            {
                return this.NotFound(new TerminateSessionResponse
                {
                    Status = TerminateSessionStatus.BadRequest,
                    ErrorMessage = "Couldn't find account owner user id"
                });
            }

            var result = await this.sessionLifecycleService.TerminateSession(userName);

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
