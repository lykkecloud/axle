// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Settings
{
    using Lykke.SettingsReader.Attributes;

    public class ConnectionStrings
    {
        [AmqpCheck]
        public string RabbitMq { get; set; }
    }
}
