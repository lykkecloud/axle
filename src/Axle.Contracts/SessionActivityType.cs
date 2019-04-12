// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Contracts
{
    public enum SessionActivityType
    {
        Login,
        SignOut,
        TimeOut,
        DifferentDeviceTermination,
        ManualTermination,
        OnBehalfSupportConnected,
        OnBehalfSupportDisconnected
    }
}
