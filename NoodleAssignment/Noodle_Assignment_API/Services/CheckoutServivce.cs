using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Payments;

namespace Noodle_Assignment_API.Services
{
    public class CheckoutServivce : ICheckoutService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public CheckoutServivce(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("client:ProjectKey");
        }
        public async Task<string> ExecuteAsync()
        {
            //Get The Customer
            var customer = await _client.WithApi()
                                 .WithProjectKey(projectKey)
                                 .Customers()
                                 .WithId("e0cfbfb5-9f78-40aa-bdfa-5e5f49c769ea")
                                 .Get()
                                 .ExecuteAsync();

            //Create Cart
            var lineItemDraft = new LineItemDraft()
            {
                ProductId = "75b9c337-84b6-4422-9c6b-89e72bdf3383",

                VariantId = 1,
                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 500),

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var cartDraft = new CartDraft()
            {
                Currency = "INR",
                //CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                LineItems = lineItemDrafts,
                BillingAddress = customer.Addresses[0]

            };
            var cart = await _client.WithApi()
                            .WithProjectKey(projectKey)
                            .Carts()
                            .Post(cartDraft)
                            .ExecuteAsync();

            //Add items ton the cart


            var lineItems = new CartAddLineItemAction()
            {
                Quantity = 1,
                ExternalPrice = lineItemDraft.ExternalPrice,
                ProductId = "017bbb2a-d37a-4147-ab4e-d9437184edb4",
                VariantId = 1,

            };
            var getDiscountCode = await _client.WithApi()
                .WithProjectKey(projectKey)
                .DiscountCodes()
                .WithId("0954d13e-383f-4a4b-b96c-34a1b4217212")
                .Get()
                .ExecuteAsync();

            var discousntCode = new CartAddDiscountCodeAction
            {

                Code = getDiscountCode.Code

            };
            var recalculate = new CartRecalculateAction()
            {
                UpdateProductData = true,
            };
            var addressDraft = new AddressDraft()
            {

                Country = "DE",
                FirstName = customer.FirstName,
                LastName = customer.LastName,

            };
            var shippingAddress = new CartSetShippingAddressAction()
            {
                Address = addressDraft
            };
            var cartUpdate = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction> { lineItems, discousntCode, recalculate, shippingAddress },
                Version = cart?.Version ?? 0
            };

           var updatedCart =  await _client.WithApi()
                .WithProjectKey(projectKey)
                .Carts()
                .WithId(cart?.Id)
                .Post(cartUpdate)
                .ExecuteAsync();
            var customerResource = new CustomerResourceIdentifier() { Id = customer.Id };
            var payementDraft = new PaymentDraft()
            {

                AmountPlanned = Money.FromDecimal(cart?.TotalPrice.CurrencyCode, Convert.ToDecimal(cart?.TotalPrice.CentAmount))
            };
            var payment = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Payments()
                .Post(payementDraft)
                .ExecuteAsync();
            Console.WriteLine($"Payment Created with Id: {payment.Id}");


            var transactionDraft = new TransactionDraft()
            {
                Timestamp = DateTime.UtcNow,
                Type = ITransactionType.Charge,
                Amount = Money.FromDecimal(cart?.TotalPrice.CurrencyCode, Convert.ToDecimal(cart?.TotalPrice.CentAmount))
            };

            var addTransaction = new PaymentAddTransactionAction()
            {
                Transaction = transactionDraft,
            };

            var updatedPayemnt = new PaymentUpdate()
            {
                Actions = new List<IPaymentUpdateAction> { addTransaction },
                Version = payment?.Version ?? 0,

            };

            var transaction = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Payments()
                .WithId(payment?.Id)
                .Post(updatedPayemnt)
                .ExecuteAsync();

            var paymentResource = new PaymentResourceIdentifier() { Id = payment?.Id };
            var cartPayment = new CartAddPaymentAction()
            {
                Payment = paymentResource
            };
            var cartUpdateWithPayment = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction> { cartPayment },
                Version = updatedCart?.Version ?? 0,
                
            };
         var paymentAddedTocart=   await _client.WithApi()
               .WithProjectKey(projectKey)
               .Carts()
               .WithId(updatedCart?.Id)
               .Post(cartUpdateWithPayment)
               .ExecuteAsync();

            return paymentAddedTocart.Id;
        }
    }
}
