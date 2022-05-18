using commercetools.Sdk.Api.Models.Subscriptions;

namespace Noodle_Assignment_API.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public SubscriptionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client=clients.FirstOrDefault(r => r.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }

        public async Task ExecuteAsync()
        {
            var destination = new GoogleCloudPubSubDestination()
            {
                Type = "GoogleCloudPubSub",
                ProjectId = "ct-support",
                Topic = "training-subscription-sample"
            };

            var subscriptionDraft = new SubscriptionDraft()
            {
                Key = "mysubscription",
                Destination = destination,
                Messages = new List<IMessageSubscription>() { new MessageSubscription() { ResourceTypeId = "order", Types = new List<string>() { "OrderCreated" } } },
                //Format=Format
            };

            var subscription = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .Subscriptions()
                .Post(subscriptionDraft)
                .ExecuteAsync();

            Console.WriteLine($"a new subscription created with Id {subscription.Id}");
        }
    }
}
