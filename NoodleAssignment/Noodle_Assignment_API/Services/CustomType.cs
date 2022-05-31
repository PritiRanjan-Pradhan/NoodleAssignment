using commercetools.Sdk.Api.Models.Types;

namespace Noodle_Assignment_API.Services
{
    public class CustomType : ICustomType
    {
        private readonly IClient _client;
        private readonly string _projectKey;

        public CustomType(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(r => r.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync(CustomTypeModel customTypeModel)

        {
            var customField = new FieldDefinition()
            {
                Name = customTypeModel.CustomField.CustomFieldName,
                Required = customTypeModel.CustomField.Required,
                Label = new LocalizedString { { "en", customTypeModel.CustomField.Label } },
                Type = new CustomFieldStringType() { Name = "String" }
            };
            var customFieldDefination = new List<IFieldDefinition>() { customField };
            var typeDraft = new TypeDraft()
            {


                Name = new LocalizedString { { "en", customTypeModel.TypeName } },
                Key = customTypeModel.Key,
                ResourceTypeIds = new List<IResourceTypeId> { IResourceTypeId.Customer },
                Description = new LocalizedString { { "desc", customTypeModel.Description } },
                FieldDefinitions = customFieldDefination,


            };
            var customtype = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .Types()
                .Post(typeDraft)
                .ExecuteAsync();
            Console.WriteLine($"New custom type has been created with Id: {customtype.Id}");
            var customers = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .Customers()
                .Get()
                .ExecuteAsync();
                
            foreach (var customer in customers.Results)
            {
                var customeField = new CustomerSetCustomTypeAction()
                {
                    Type = new TypeResourceIdentifier() { Id = customtype.Id },

                };
                var customerUpdateAction = new CustomerUpdate()
                {
                    Actions = new List<ICustomerUpdateAction>() { customeField },
                    Version = customer.Version,

                };

                var updatedCustomer = await _client.WithApi()
                    .WithProjectKey(_projectKey)
                    .Customers()
                    .WithId(customer.Id)
                    .Post(customerUpdateAction)
                    .ExecuteAsync();
            }



        }
    }
}
