// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Dto
{
    using Contracts;
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
