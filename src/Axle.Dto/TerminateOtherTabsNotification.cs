// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.


namespace Axle.Dto
{
    using MessagePack;

    [MessagePackObject]
    public class TerminateOtherTabsNotification
    {
        [Key(0)]
        public string OriginatingServiceId { get; set; }

        [Key(1)]
        public string OriginatingConnectionId { get; set; }

        [Key(2)]
        public string AccessToken { get; set; }
    }
}
