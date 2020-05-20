// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Settings
{
    using System;

    public class SessionSettings
    {
        public int TimeoutInSec { get; set; } = 300;

        public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutInSec);
    }
}
