

namespace Noodle_Assignment_API.Services
{
    public class StateMachineService : IStateMachineService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public StateMachineService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task<string> ExecuteAsync()
        {
            var stateOrderPackedDraft = new StateDraft
            {
                Key = "OrderPacked4",
                Initial = true,
                Name = new LocalizedString { { "en", "Order Packed4" } },
                Type = IStateTypeEnum.OrderState
            };

            var stateOrderPackedDraftRespopnse = await _client.WithApi()
                                                .WithProjectKey(projectKey)
                                                .States()
                                                .Post(stateOrderPackedDraft)
                                                .ExecuteAsync();

            var stateOrderShippedDraft = new StateDraft
            {
                Key = "OrderShipped4",
                Initial = false,
                Name = new LocalizedString { { "en", "Order Shipped4" } },
                Type = IStateTypeEnum.OrderState
            };

            var stateOrderShippedDraftResponse = await _client.WithApi()
                                                .WithProjectKey(projectKey)
                                                .States()
                                                .Post(stateOrderShippedDraft)
                                                .ExecuteAsync();

            var stateResource = new StateResourceIdentifier() { Id = stateOrderShippedDraftResponse?.Id };

            var action = new StateSetTransitionsAction()
            {
                Transitions = new List<IStateResourceIdentifier>() { stateResource }
            };

            var stateUpdate = new StateUpdate()
            {
                Actions = new List<IStateUpdateAction>() { action },
                Version = stateOrderPackedDraftRespopnse?.Version ?? 0
            };

            var updatedOrder1response = await _client.WithApi().WithProjectKey(projectKey)
                .States()
                .WithId(stateOrderPackedDraftRespopnse?.Id)
                .Post(stateUpdate)
                .ExecuteAsync();

            ////Console.WriteLine($"stateOrderShipped Id : {stateOrderShipped.Id}, stateOrderPacked transition to:  {updatedStateOrderPacked.Transitions[0].Id}");


            return $"stateOrderShippedId :  { stateOrderShippedDraftResponse.Id} , stateOrderPacked transition to : {updatedOrder1response.Transitions[0].Id}";
            //return "hello";
        }
    }
}
