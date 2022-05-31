using commercetools.Sdk.Api.Serialization;

namespace Noodle_Assignment_API.Services
{
    public class MeService : IMeService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        private readonly IServiceProvider _serviceProvider;
        public MeService(IEnumerable<IClient> clients,IConfiguration configuration,IServiceProvider serviceProvider)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("MeClient:ProjectKey");
            _serviceProvider = serviceProvider;
        }

        public async Task<string> ExecuteAsync(MeClientModel meClientModel)
        {

            var configuration = _serviceProvider.GetService<IConfiguration>();
            var httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>();
            var serializerService = _serviceProvider.GetService<SerializerService>();

            var clientConfiguration = configuration.GetSection("MeClient").Get<ClientConfiguration>();

            //Create passwordFlow TokenProvider
            var passwordTokenProvider = TokenProviderFactory
                .CreatePasswordTokenProvider(clientConfiguration,
                    httpClientFactory,
                    new InMemoryUserCredentialsStoreManager(meClientModel.Email, meClientModel.Password));

            //Create MeClient
            var meClient = ClientFactory.Create(
                "MeClient",
                clientConfiguration,
                httpClientFactory,
                serializerService,
                passwordTokenProvider);

            var myProfile = await meClient.WithApi()
                .WithProjectKey(projectKey)
                .Me()
                .Get()
                .ExecuteAsync();
            Console.WriteLine($"My Profile, firstName:{myProfile.FirstName}, lastName:{myProfile.LastName}");
           
            var myOrders = await meClient.WithApi()
                .WithProjectKey(projectKey)
                .Me()
                .Orders()
                .Get()
                .ExecuteAsync();

            Console.WriteLine($"Orders count: {myOrders.Count}");
            foreach (var order in myOrders.Results)
            {
                Console.WriteLine($"{order.Id}");
            }
            return myOrders.Count.ToString();
        }
    }
}
