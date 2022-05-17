using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Stores;
using Noodle_Assignment_API.Interfaces;

namespace Noodle_Assignment_API.Services
{
    public class InStoreService : IInStore
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public InStoreService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task<string> ExecuteAsync()
        {
            var customer = await _client.WithApi()
                                .WithProjectKey(_projectKey)
                                .Customers()
                                .WithId("8f9072e8-38fd-47a1-8b7d-976d2a9a207d")
                                .Get()
                                .ExecuteAsync();

            var store = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .Stores()
                .WithId("43903c40-6bc2-4c9f-9be7-5457e169dbe8")
                .Get()
                .ExecuteAsync();

            var lineItemDraft = new LineItemDraft()
            {
                
                ProductId = "661a95b5-fe1a-4a53-bd0e-f6de70951c50",
                VariantId = 1,
                Quantity = 1,
                ExternalPrice=Money.FromDecimal("INR",199M)

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var storeResource = new StoreResourceIdentifier()
            {
                Id = store.Id
            };
            var cartDraft = new CartDraft()
            {
                Currency = "INR",
                CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                LineItems = lineItemDrafts,
                BillingAddress = customer.Addresses[0],
                Store = storeResource

            };
            var storeCart = await _client.WithApi()
                .WithProjectKey(_projectKey)

                .InStoreKeyWithStoreKeyValue(store.Key)
                .Carts()
                .Post(cartDraft)
                .ExecuteAsync();

            return $"Cart {storeCart.Id} created in store {store.Key} for customer {customer.FirstName}";
        }
    }
}
