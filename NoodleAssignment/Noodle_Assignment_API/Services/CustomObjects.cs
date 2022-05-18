using commercetools.Sdk.Api.Models.CustomObjects;
using Newtonsoft.Json;

namespace Noodle_Assignment_API.Services
{
    public class CustomObjectsService : ICustomObjectService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public CustomObjectsService(IEnumerable<IClient> clients,IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(r => r.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync()
        {
            StreamReader r = new StreamReader("Resources/compatibility-info.json");
            string jsonString = r.ReadToEnd();
            var m = JsonConvert.DeserializeObject(jsonString);
            var customObjectDraft = new CustomObjectDraft()
            {
                Key = "custom_object",
                Container = "myContainer",
                Value = m
            };
            var customObject = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .CustomObjects()
                .Post(customObjectDraft)
                .ExecuteAsync();

            Console.WriteLine($"custom object created with Id {customObject.Id} with version {customObject.Version}");
        }
    }
}
