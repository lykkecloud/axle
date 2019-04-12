// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Dto
{
    public class OnBehalfChangeResponse
    {
        public bool Successful { get; set; }

        public string ErrorMessage { get; set; }

        public static OnBehalfChangeResponse Success()
        {
            return new OnBehalfChangeResponse { Successful = true };
        }

        public static OnBehalfChangeResponse Fail(string errorMessage)
        {
            return new OnBehalfChangeResponse { Successful = false, ErrorMessage = errorMessage };
        }
    }
}
