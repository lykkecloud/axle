// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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
