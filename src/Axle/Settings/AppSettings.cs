// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Settings
{
    using Lykke.SettingsReader.Attributes;

    public class AppSettings : BaseAppSettings
    {
        [HttpCheck("api/isalive")]
        public string mtCoreAccountsMgmtServiceUrl { get; set; }
    }
}
