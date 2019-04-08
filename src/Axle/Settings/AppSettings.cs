// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Settings
{
    using Lykke.SettingsReader.Attributes;

    public class AppSettings
    {
        [HttpCheck("api/isalive")]
        public string ApiAuthority { get; set; }

        [HttpCheck("api/isalive")]
        public string chestUrl { get; set; }

        [HttpCheck("api/isalive")]
        public string mtCoreAccountsMgmtServiceUrl { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
