using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Channels;

namespace Noodle_Assignment_API.Services
{
    public class CartMerging : ICartMerging
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public CartMerging(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync()
        {
            var channel = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Channels()
                .WithId("639975c4-520a-464f-be0b-1bddba2bc941")
                .Get()
                .ExecuteAsync();

            var customer = await _client.WithApi()
                               .WithProjectKey(projectKey)
                               .Customers()
                               .WithId("e0cfbfb5-9f78-40aa-bdfa-5e5f49c769ea")
                               .Get()
                               .ExecuteAsync();

            //Create Cart for customer
            var lineItemDraft = new LineItemDraft()
            {
                Sku = "A0E2000000024BC",
                SupplyChannel = new ChannelResourceIdentifier { Id = channel.Id },

                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 399M),

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var cartDraft = new CartDraft()
            {
                Currency = "INR",
                CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                LineItems = lineItemDrafts,
                BillingAddress = customer.Addresses[0]

            };
            var cart = await _client.WithApi()
                            .WithProjectKey(projectKey)
                            .Carts()
                            .Post(cartDraft)
                            .ExecuteAsync();

            //Create cart for annonymous

            var lineItemDraftForAnnonymous = new LineItemDraft()
            {
                Sku = "A0E2000000027DV",
                SupplyChannel = new ChannelResourceIdentifier { Id = channel.Id },
                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 299M),

            };
            var lineItemDraftsForAnnonymous = new List<ILineItemDraft>() { lineItemDraftForAnnonymous };
            var annonymosCartDraft = new CartDraft()
            {
                Currency = "INR",
                AnonymousId = "1234",

                Country = "DE",
                DeleteDaysAfterLastModification = 30,
                LineItems = lineItemDraftsForAnnonymous,
                BillingAddress = customer.Addresses[0]

            };

            var anonymousCart = await _client.WithApi()
                            .WithProjectKey(projectKey)
                            .Carts()
                            .Post(annonymosCartDraft)
                            .ExecuteAsync();
            var authentiCatedCustomer = new CustomerSignin()
            {

                AnonymousCart = new CartResourceIdentifier
                {
                    Id = anonymousCart.Id,

                },
                AnonymousId = anonymousCart.AnonymousId,
                AnonymousCartSignInMode = IAnonymousCartSignInMode.MergeWithExistingCustomerCart,
                Email = customer.Email,
                Password = "pritiranjan",
                UpdateProductData = true,
                

            };
            var result = await _client.WithApi().WithProjectKey(projectKey)
              .Login()
              .Post(authentiCatedCustomer)
              .ExecuteAsync();

            //LineItems of the anonymous cart will be copied to the customer’s active cart that has been modified most recently.
            var currentCustomerCart = result?.Cart as Cart;
            if (currentCustomerCart != null)
            {
                foreach (var lineItem in currentCustomerCart.LineItems)
                {
                    Console.WriteLine($"SKU: {lineItem.Variant.Sku}, Quantity: {lineItem.Quantity}");
                }
            }
            
        }
    }
}
