namespace Noodle_Assignment_API.Services
{
    public class SearchService : ISerchService
    {
        private readonly IClient _client;
        private readonly string _projectkey;

        public SearchService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            _projectkey = configuration.GetValue<string>("Client:ProjectKey");
        }   

        public async Task<string> ExecuteAsync()
        {
            var product = await _client.WithApi()
                .WithProjectKey(_projectkey)
                .Products()
                .WithKey("amanda")
                .Get()
                .ExecuteAsync();
            
            var filterQuery = $"id:\"{product.Id}\"";
            
            var facet = "variants.attributes.color";
            
            var formParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("filter",filterQuery),new KeyValuePair<string,string>("facet",facet) };
           
            var productProjection = await _client.WithApi()
                .WithProjectKey(_projectkey)
                .ProductProjections()
                .Search()
                .Post(formParams)
                .ExecuteAsync();

            Console.WriteLine($"No. of products: {productProjection.Count}");
            Console.WriteLine("products in search result: ");
            productProjection.Results.ForEach(p => Console.WriteLine(p.Name["en"]));

            //Console.WriteLine($"Number of Facets: {productProjection.Facets.Count}");

            //var colorFacetResult = searchResponse.Facets["color"] as TermFacetResult;
            //foreach (var term in colorFacetResult.Terms)
            //{
            //    Console.WriteLine($"Term : {term.Term}, Count: {term.Count}");
            //}
            return null;

        }
    }
}
