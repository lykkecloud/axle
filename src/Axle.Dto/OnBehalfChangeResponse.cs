// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Dto
{
    public class OnBehalfChangeResponse
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public static OnBehalfChangeResponse Success()
        {
            return new OnBehalfChangeResponse { IsSuccessful = true };
        }

        public static OnBehalfChangeResponse Fail(string errorMessage)
        {
            return new OnBehalfChangeResponse { IsSuccessful = false, ErrorMessage = errorMessage };
        }
    }
}
