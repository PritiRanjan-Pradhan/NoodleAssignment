
using commercetools.Sdk.Api.Models.CustomerGroups;
using Noodle_Assignment_API.Model;

namespace Noodle_Assignment_API.Services
{
    public class UpdateGroupService : IUpdateGroupService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public UpdateGroupService(IEnumerable<IClient> clients,IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task<string> ExecuteAsync(UpdateServiceModel updateServiceModel)
        {
         var customer= await _client.WithApi().WithProjectKey(projectKey)
                .Customers()
                .WithId(updateServiceModel.CustomerId)
                .Get()
                .ExecuteAsync();
            var action = new CustomerSetCustomerGroupAction()
            {
                CustomerGroup = new CustomerGroupResourceIdentifier() { Id = updateServiceModel.CustomerGroupId }
            };

            var customerUpdate = new CustomerUpdate()
            {
                Actions = new List<ICustomerUpdateAction>() { action },
                Version = customer?.Version ?? 0,
            };

            var updatedCustomer = await _client.WithApi().WithProjectKey(projectKey)
              .Customers()
              .WithId(updateServiceModel.CustomerId)
              .Post(customerUpdate)
              .ExecuteAsync();

            var updatedCustomerId = updatedCustomer?.Id;
            var updatedCustomerGroupId = updatedCustomer?.CustomerGroup?.Id;


            await Task.CompletedTask;
            return "customer " + updatedCustomerId + " in customer group " + updatedCustomerGroupId;

           
        }
    }
}
