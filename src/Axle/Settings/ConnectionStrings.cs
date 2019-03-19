// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Settings
{
    using Lykke.SettingsReader.Attributes;

    public class ConnectionStrings
    {
        [AmqpCheck]
        public string RabbitMq { get; set; }
    }
}
