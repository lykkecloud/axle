// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Dto
{
    using Axle.Contracts;
    using MessagePack;

    [MessagePackObject]
    public class TerminateSessionNotification
    {
        [Key(0)]
        public int SessionId { get; set; }

        [Key(1)]
        public SessionActivityType Reason { get; set; }
    }
}
