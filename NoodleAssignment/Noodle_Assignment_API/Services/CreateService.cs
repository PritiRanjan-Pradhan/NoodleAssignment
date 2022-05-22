
namespace Noodle_Assignment_API.Services
{
    public class CreateService : ICreateService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public CreateService(IEnumerable<IClient> clients,IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task<string> ExecuteAsync(CreateCustomer customer)
        {
            // CREATE customer draft
            var customerDraft = new CustomerDraft
            {
                Email = customer.Email,
                Password = customer.Password,
                Key = Guid.NewGuid().ToString("n").Substring(0,8),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Addresses = new List<IBaseAddress>{
                        new AddressDraft {
                            Country = customer.Country,
                    }
                },
                DefaultShippingAddress = 0,
                DefaultBillingAddress = 0
            };

            // TODO: SIGNUP a customer
            var response = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .Post(customerDraft)
               .ExecuteAsync();

           

            //Console.WriteLine($"Customer Created with Id : {customer.Id} and Key : {customer.Key} and Email Verified: {customer.IsEmailVerified}");

            // TODO: CREATE a email verfification token
            var obj=new CustomerCreateEmailToken() { Id=response.Customer.Id,TtlMinutes=5,Version=response.Customer.Version};
            var emailTokenResponse = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .EmailToken()
               .Post(obj)
               .ExecuteAsync();

            // TODO: CONFIRM CustomerEmail
            var obj1 = new CustomerEmailVerify() { TokenValue = emailTokenResponse.Value, Version = response.Customer.Version };
            var confirmEmailResponse = await _client.WithApi().WithProjectKey(projectKey)
               .Customers()
               .EmailConfirm()
               .Post(obj1)
               .ExecuteAsync();


            return "IsEmailVerified "+ confirmEmailResponse.IsEmailVerified.ToString();


            //Console.WriteLine($"Is Email Verified:{retrievedCustomer.IsEmailVerified}");
        }
    }
}
