// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Constants
{
    using StackExchange.Redis;

    public static class RedisChannels
    {
        public static readonly RedisChannel SessionTermination = "axle:notifications:termsession";

        public static readonly RedisChannel OtherTabsTermination = "axle:notifications:othertabstermination";
    }
}
