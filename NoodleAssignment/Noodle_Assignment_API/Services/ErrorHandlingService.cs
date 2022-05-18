using commercetools.Base.Client.Error;

namespace Noodle_Assignment_API.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public ErrorHandlingService(IEnumerable<IClient> clients,  IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(r=>r.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync()
        {
            var customerMayOrMayNotExist = "customer-michele-WRONG-KEY";
            try
            {
                await _client.WithApi()
                    .WithProjectKey(_projectKey)
                    .Customers()
                    .WithKey(customerMayOrMayNotExist)
                    .Get()
                    .ExecuteAsync();
                   
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine(ex.Body);
                throw;
            }
            throw new NotImplementedException();
        }
    }
}
