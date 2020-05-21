// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Extensions
{
    using Contracts;
    using Dto;

    public static class EnumExtensions
    {
        public static string ToMessage(this StatusCode code)
        {
            switch (code)
            {
                case StatusCode.WN_ATH_701: return "A session already existed for this user to this reference account and was terminated";
                case StatusCode.IF_ATH_501: return "New session created";
                case StatusCode.IF_ATH_502: return "Existing session is killed due to concurrent login";
                case StatusCode.IF_ATH_503: return "A session was terminated by force";
                default: return "Unknown error";
            }
        }

        public static TerminateConnectionReason ToTerminateConnectionReason(this SessionActivityType activityType)
        {
            switch (activityType)
            {
                case SessionActivityType.DifferentDeviceTermination:
                    return TerminateConnectionReason.DifferentDevice;
                case SessionActivityType.ManualTermination:
                    return TerminateConnectionReason.ByForce;
                default:
                    return TerminateConnectionReason.Other;
            }
        }
    }
}
