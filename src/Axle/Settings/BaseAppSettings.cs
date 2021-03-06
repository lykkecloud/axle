﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Settings
{
    using Lykke.SettingsReader.Attributes;

    public class BaseAppSettings
    {
        [HttpCheck("api/isalive")]
        public string ApiAuthority { get; set; }

        [HttpCheck("api/isalive")]
        public string chestUrl { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
