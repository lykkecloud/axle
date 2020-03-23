namespace Axle.Exceptions
{
    using System;

    public class UserClaimIsEmptyException : Exception
    {
        public string ClaimType { get; }

        public UserClaimIsEmptyException(string claimType)
            : base($"User claim [{claimType}] is empty")
        {
            this.ClaimType = claimType;
        }
    }
}