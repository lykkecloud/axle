// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Dto
{
    public class Error
    {
        public enum Code
        {
            WN_ATH_701,
            IF_ATH_501,
            IF_ATH_502
        }

        public static string ToErrorMessage(Code code)
        {
            switch (code)
            {
                case Code.WN_ATH_701: return "A session already existed for this user to this reference account and was terminated";
                case Code.IF_ATH_501: return "New session created";
                case Code.IF_ATH_502: return "Existing session is killed due to concurrent login";
                default: return "Unknown error";
            }
        }
    }
}
