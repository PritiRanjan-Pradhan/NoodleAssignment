namespace Noodle_Assignment_API.Services
{
    public class MeService : IMeService
    {
        private readonly IClient _client;
        private readonly string projectKey;
        public MeService(IEnumerable<IClient> clients,IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            projectKey = configuration.GetValue<string>("MeClient:ProjectKey");
        }

        public async Task<string> ExecuteAsync()
        {
            var myProfile = await _client.WithApi()
                .WithProjectKey(projectKey)
                .Customers()
                .WithId("e0cfbfb5-9f78-40aa-bdfa-5e5f49c769ea")
                .Get()
                .ExecuteAsync();
            Console.WriteLine($"My Profile, firstName:{myProfile.FirstName}, lastName:{myProfile.LastName}");

            var myOrders = await _client.WithApi()
                .WithProjectKey(projectKey)
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
