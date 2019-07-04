// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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
