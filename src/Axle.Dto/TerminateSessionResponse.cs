// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Dto
{
    public class TerminateSessionResponse
    {
        public TerminateSessionStatus Status { get; set; }

        public int UserSessionId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
