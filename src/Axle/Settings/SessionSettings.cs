// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Settings
{
    using System;

    public class SessionSettings
    {
        public int TimeoutInSec { get; set; } = 300;

        public TimeSpan Timeout => TimeSpan.FromSeconds(this.TimeoutInSec);
    }
}
