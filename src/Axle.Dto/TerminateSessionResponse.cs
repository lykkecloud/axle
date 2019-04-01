// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Dto
{
    public class TerminateSessionResponse
    {
        public TerminateSessionStatus Status { get; set; }

        public int UserSessionId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
