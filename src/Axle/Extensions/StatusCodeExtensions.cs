// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Extensions
{
    using Axle.Dto;

    public static class StatusCodeExtensions
    {
        public static string ToMessage(this StatusCode code)
        {
            switch (code)
            {
                case StatusCode.WN_ATH_701: return "A session already existed for this user to this reference account and was terminated";
                case StatusCode.IF_ATH_501: return "New session created";
                case StatusCode.IF_ATH_502: return "Existing session is killed due to concurrent login";
                default: return "Unknown error";
            }
        }
    }
}
