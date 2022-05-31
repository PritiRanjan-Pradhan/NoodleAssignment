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
        public async Task ExecuteAsync(CheckoutModel checkoutModel)
        {

            //Get The Customer
            var customer = await _client.WithApi()
                                         .WithProjectKey(projectKey)
                                         .Customers()
                                         .WithId(checkoutModel.CustomerModel.Id)
                                         .Get()
                                         .ExecuteAsync();

            //Create Cart
            var lineItemDraft = new LineItemDraft()
            {
                ProductId = checkoutModel.LineItemModel.ProductId,
                VariantId = checkoutModel.LineItemModel.VariantId,
                Quantity = checkoutModel.LineItemModel.Quantity,
                ExternalPrice = Money.FromDecimal(checkoutModel.LineItemModel.Currency, checkoutModel.LineItemModel.Price),

            };
            var lineItemDrafts = new List<ILineItemDraft>() { lineItemDraft };
            var cartDraft = new CartDraft()
            {
               
                Currency = checkoutModel.CartModel.Currency,
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

            ICart cartUpdateWithPayment = null;

            //Add items ton the cart
            foreach (var addedLineItems in checkoutModel.AddedLineItems)
            {

                var lineItems = new CartAddLineItemAction()
                {
                    Quantity = addedLineItems.Quantity,
                    ExternalPrice = Money.FromDecimal(addedLineItems.Currency, addedLineItems.Price),
                    ProductId = addedLineItems.ProductId,
                    VariantId = addedLineItems.VariantId,

                };
                var getDiscountCode = await _client.WithApi()
                    .WithProjectKey(projectKey)
                    .DiscountCodes()
                    .WithId(checkoutModel.DiscountCode.DiscoutCodeId)
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
                cartUpdateWithPayment = await _client.WithApi()
                      .WithProjectKey(projectKey)
                      .Carts()
                      .WithId(updatedCart?.Id)
                      .Post(paymentAddedToCart)
                      .ExecuteAsync();
            }
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




            //Console.WriteLine($"Order Created with order number: {order.OrderNumber}");


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
            
            var stateResource2 = new StateReference() { Obj = orderPackedState, Id = orderPackedState.Id };
            var s = new StateResourceIdentifier() { Id = stateResource2.Id };
            var v = new OrderTransitionStateAction()
            {
                State = s,

            };

         
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


           

        }
    }





}
