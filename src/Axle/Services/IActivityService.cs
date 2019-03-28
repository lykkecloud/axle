// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Persistence;

    public interface IActivityService
    {
        Task PublishActivity(SessionActivity activity);

        Task PublishActivity(Session session, SessionActivityType activityType);
    }
}
