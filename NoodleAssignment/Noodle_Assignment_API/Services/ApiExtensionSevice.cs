using commercetools.Sdk.Api.Models.Extensions;

namespace Noodle_Assignment_API.Services
{
    public class ApiExtensionSevice : IApiExtensionService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public ApiExtensionSevice(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(r => r.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExcuteAsync()
        {
            var extensionTrigger = new ExtensionTrigger()
            {
                Actions = new List<IExtensionAction>() { IExtensionAction.Create },
                ResourceTypeId = IExtensionResourceTypeId.Order
            };

            var httpDestination = new HttpDestination()
            {
                Type = "HTTP",
                Url = "https://api.australia-southeast1.gcp.commercetools.com/"
            };

          
            var extensionDraft = new ExtensionDraft()
            {
                Destination =httpDestination,
                Triggers = new List<IExtensionTrigger>() { extensionTrigger },
                Key = $"Demo_{Guid.NewGuid().ToString("n").Substring(0,4)}"
            };

                var extension = await _client.WithApi()
                 .WithProjectKey(_projectKey)
                 .Extensions()
                 .Post(extensionDraft)
                 .ExecuteAsync();

            Console.WriteLine($"extension created with Id {extension.Id}");
        }
        
    }
}
