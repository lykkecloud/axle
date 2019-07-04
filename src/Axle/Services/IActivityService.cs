// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

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
