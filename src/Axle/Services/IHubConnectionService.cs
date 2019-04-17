// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;

    public interface IHubConnectionService
    {
        Task TerminateConnections(SessionActivityType reason, params string[] connectionIds);
    }
}
