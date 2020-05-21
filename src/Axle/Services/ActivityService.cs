// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Contracts;
    using Persistence;
    using Lykke.RabbitMqBroker.Publisher;

    public class ActivityService : IActivityService
    {
        private readonly RabbitMqPublisher<SessionActivity> publisher;

        public ActivityService(RabbitMqPublisher<SessionActivity> publisher)
        {
            this.publisher = publisher;
            publisher.Start();
        }

        public async Task PublishActivity(SessionActivity activity)
        {
            await publisher.ProduceAsync(activity);
        }

        public Task PublishActivity(Session session, SessionActivityType activityType)
        {
            var activity = new SessionActivity(activityType, session.SessionId, session.UserName, session.AccountId);

            return PublishActivity(activity);
        }
    }
}
