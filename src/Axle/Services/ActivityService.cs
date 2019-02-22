// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Services
{
    using System.Threading.Tasks;
    using Axle.Contracts;
    using Axle.Persistence;
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
            await this.publisher.ProduceAsync(activity);
        }

        public Task PublishActivity(Session session, SessionActivityType activityType)
        {
            var activity = new SessionActivity(activityType, session.SessionId, session.UserId, session.AccountId);

            return this.PublishActivity(activity);
        }
    }
}
