// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Constants
{
    using StackExchange.Redis;

    public static class RedisChannels
    {
        public static readonly RedisChannel SessionTermination = "axle:notifications:termsession";

        public static readonly RedisChannel OtherTabsTermination = "axle:notifications:othertabstermination";
    }
}
