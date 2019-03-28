// (c) Lykke Corporation 2019 - All rights reserved. No copying, adaptation, decompiling, distribution or any other form of use permitted.

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
            var activity = new SessionActivity(activityType, session.SessionId, session.UserName, session.AccountId);

            return this.PublishActivity(activity);
        }
    }
}
