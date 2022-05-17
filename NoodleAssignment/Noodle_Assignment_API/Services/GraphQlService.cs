using commercetools.Base.Serialization;
using commercetools.Sdk.Api.Models.GraphQl;
using Noodle_Assignment_API.GraphQL.Training.GraphQL;
using System.Text.Json;

namespace Noodle_Assignment_API.Services
{
    public class GraphQlService : IGraphQLService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public GraphQlService(IEnumerable<IClient> clients,IConfiguration configuration)
        {
            _client=clients.FirstOrDefault(p => p.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync()
        {

            var graphRequest = new GraphQLRequest
            {
                Query = "query {customers{count,results{email}}}"
            };
            // TODO: graphQL Request
            IGraphQLResponse response = null;
            response = await _client.WithApi().WithProjectKey(_projectKey)
                .Graphql()
                .Post(graphRequest)
                .ExecuteAsync();
            //Map Response to the typed result and show it
            var typedResult = ((JsonElement)response.Data).ToObject<GraphQLResultData>(_client.SerializerService);
            var customersResult = typedResult.Customers;
            Console.WriteLine($"Customers count: {customersResult.Count}");
            Console.WriteLine("Showing Customers emails:");
            foreach (var customer in customersResult.Results)
            {
                Console.WriteLine(customer.Email);
            }
        }
    }
}
