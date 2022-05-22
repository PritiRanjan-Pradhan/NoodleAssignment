using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Orders;
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
           
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task<string> ExecuteAsync()

        {
            //Get The Customer
            var customer = await _client.WithApi()
                                 .WithProjectKey(projectKey)
                                 .Customers()
                                 .WithId("28bf29de-5a8e-4c9b-9057-f6ae881d7767")
                                 .Get()
                                 .ExecuteAsync();

            //Create Cart
            var lineItemDraft = new LineItemDraft()
            {
                ProductId = "4755917e-55fa-4c01-8d93-e1fbd1396636",

                VariantId = 1,
                Quantity = 1,
                ExternalPrice = Money.FromDecimal("INR", 599M),

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var cartDraft = new CartDraft()
            {
                Country = "IN",
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

            Console.WriteLine($"Cart {cart.Id} for customer: {cart.CustomerId}");

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
           
            var updatedCart = await _client.WithApi()
                 .WithProjectKey(projectKey)
                 .Carts()
                 .WithId(cart?.Id)
                 .Post(cartUpdate)
                 .ExecuteAsync();
            var customerResource = new CustomerResourceIdentifier() { Id = customer.Id };
            var payementDraft = new PaymentDraft()
            {
                Customer = customerResource,
                AmountPlanned = Money.FromDecimal(updatedCart?.TotalPrice.CurrencyCode, Convert.ToDecimal(updatedCart?.TotalPrice.CentAmount))
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
                Amount = Money.FromDecimal(updatedCart?.TotalPrice.CurrencyCode, Convert.ToDecimal(updatedCart?.TotalPrice.CentAmount))
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
            var paymentAddedToCart = new CartUpdate()
            {
                Actions = new List<ICartUpdateAction> { cartPayment },
                Version = updatedCart?.Version ?? 0,

            };
            var cartUpdateWithPayment = await _client.WithApi()
                  .WithProjectKey(projectKey)
                  .Carts()
                  .WithId(updatedCart?.Id)
                  .Post(paymentAddedToCart)
                  .ExecuteAsync();

            //Create order 

            var cartResource = new CartResourceIdentifier() { Id = cartUpdateWithPayment?.Id, };

            var orderDraft = new OrderFromCartDraft()
            {
                Cart = cartResource,
                Version = cartUpdateWithPayment?.Version ?? 0,
            };

            var order = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Orders()
                .Post(orderDraft)
                .ExecuteAsync();

            Console.WriteLine($"Order Created with order number: {order.OrderNumber}");

          
            var changeOrderState = new OrderChangeOrderStateAction()
            {
                OrderState = IOrderState.Confirmed,

            };
            var orderUpdate = new OrderUpdate()
            {
                Actions = new List<IOrderUpdateAction> { changeOrderState },
                Version = order?.Version ?? 0,

            };

            var orderConfirmed = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Orders()
                .WithId(order?.Id)
                .Post(orderUpdate)
                .ExecuteAsync();

            Console.WriteLine($"Order state changed to: {orderConfirmed?.OrderState.Value}");

            //GET custom workflow state for Order
            var orderPackedState = await _client.WithApi()
              .WithProjectKey(projectKey)
              .States()
              .WithId("e7008107-2a72-4ffd-b358-e0e4b6bcfd76")
              .Get()
              .ExecuteAsync();
            ///   var orderStateChangeToPacked =new OrderTra
            var orderShippedState = await _client.WithApi()
              .WithProjectKey(projectKey)
              .States()
              .WithId("f289835b-ad6a-4d47-b7ca-8715307ede9e")
              .Get()
              .ExecuteAsync();
            //Packed


            //var stateResource = new StateResourceIdentifier() { Id = orderShippedState?.Id,Key=orderShippedState?.Key };

            //var action = new StateSetTransitionsAction()
            //{
            //    Transitions = new List<IStateResourceIdentifier>() { stateResource }
            //};

            //var stateUpdate = new StateUpdate()
            //{
            //    Actions = new List<IStateUpdateAction>() { action },
            //    Version = orderPackedState?.Version ?? 0
            //};

            //var orderStateUpdate = await _client.WithApi().WithProjectKey(projectKey)
            //    .States()
            //    .WithId(orderPackedState?.Id)
            //    .Post(stateUpdate)
            //    .ExecuteAsync();
            var stateResource2 = new StateReference() { Obj = orderPackedState, Id = orderPackedState.Id };
            var s = new StateResourceIdentifier() { Id = stateResource2.Id };
            var v = new OrderTransitionStateAction()
            {
                State = s,

            };

            //  //orderConfirmed.
            //  var r =new StateResourceIdentifier() { Id = orderStateUpdate.Id };
            //var p=  new OrderTransitionStateAction()
            //  {
            //      State = r
            //  };
            //  var orderUpdate2 = new OrderUpdate()
            //  {
            //      Actions = new List<IOrderUpdateAction> { p },

            //      Version = orderConfirmed?.Version ?? 0,
            //  };
            //  var orderConfirmed2 = await _client.WithApi()
            //    .WithProjectKey(projectKey)
            //    .Orders()
            //    .WithId(orderConfirmed?.Id)
            //    .Post(orderUpdate2)
            //    .ExecuteAsync();
            var orderUpdate2 = new OrderUpdate()
            {
                Actions = new List<IOrderUpdateAction> { v },

                Version = orderConfirmed?.Version ?? 0,

            };
            var orderConfirmed2 = await _client.WithApi()
               .WithProjectKey(projectKey)
               .Orders()
               .WithId(orderConfirmed?.Id)
               .Post(orderUpdate2)
               .ExecuteAsync();

            return $"Order Workflow State changed to: {orderConfirmed2?.State?.Obj?.Name["en"]}";

        }
    }
}
