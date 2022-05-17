using commercetools.Sdk.Api.Models.Products;
using commercetools.Sdk.Api.Models.ProductSelections;

namespace Noodle_Assignment_API.Services
{
    public class ProductSelectionService : IProductSelectionService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public ProductSelectionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProductKey");
        }
        public async Task<string> ExecuteAsync()
        {
            //Create Product Selection
            var productSelectionDraft = new ProductSelectionDraft()
            {
                Name = new LocalizedString { { "product", "product" } }
            };
            var productSelection = await _client.WithApi()
                      .WithProjectKey(_projectKey)
                      .ProductSelections()
                      .Post(productSelectionDraft)
                      .ExecuteAsync();
            //Add product to the selection
            var addedProduct = new ProductSelectionAddProductAction()
            {
                Product = new ProductResourceIdentifier() { Id = " 661a95b5-fe1a-4a53-bd0e-f6de70951c50" }
            };
            var updateProductSelection = new ProductSelectionUpdate()
            {
                Actions=new List<IProductSelectionUpdateAction> { addedProduct }
            };
            var updatedProductSelection = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .ProductSelections()
                .WithId(productSelection.Id)
                .Post(updateProductSelection)
                .ExecuteAsync();

            var store = await _client.WithApi()
               .WithProjectKey(_projectKey)
               .Stores()
               .WithId("43903c40-6bc2-4c9f-9be7-5457e169dbe8")
               .Get()
               .ExecuteAsync();
            //new AddProductSelection (beta)
             //var ProductSelectionSettingDraft=  new ProductSelectionSettingDraft()
             //      { }



            throw new NotImplementedException();
        }
    }
}
