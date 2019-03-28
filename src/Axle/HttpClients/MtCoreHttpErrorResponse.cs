// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.HttpClients
{
    public class MtCoreHttpErrorResponse
    {
        public string ErrorMessage { get; set; }

        public override string ToString() => this.ErrorMessage;
    }
}
