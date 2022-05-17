
using commercetools.Sdk.ImportApi.Models.Importcontainers;

namespace Noodle_Assignment_API.Services
{
    public class ImportAPIService : IImportApiService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public ImportAPIService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p=>p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("Client:ProjectKey"); 
        }
        public async Task<string> ExecuteAsync()
        {
            var containerDraft = new ImportContainerDraft
            {
                Key = "1",
            };
           //var container = _client.WithApi()
           //     .WithProjectKey(projectKey)
           //     .

            throw new NotImplementedException();
        }
    }
}
