using commercetools.Sdk.Api.Models.Products;
using commercetools.Sdk.Api.Models.ProductSelections;
using commercetools.Sdk.Api.Models.Stores;

namespace Noodle_Assignment_API.Services
{
    public class ProductSelectionService : IProductSelectionService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        public ProductSelectionService(IEnumerable<IClient> clients, IConfiguration configuration)
        {
            _client = clients.FirstOrDefault(p => p.Name.Equals("Client"));
            _projectKey = configuration.GetValue<string>("Client:ProjectKey");
        }
        public async Task ExecuteAsync(ProductSelectionModel productSelectionModel)
        {
            //Create Product Selection
            var productSelectionDraft = new ProductSelectionDraft()
            {
                Key=Guid.NewGuid().ToString(),
                Name = new LocalizedString { { "de", productSelectionModel.Name } }
            };
            var productSelection = await _client.WithApi()
                      .WithProjectKey(_projectKey)
                      .ProductSelections()
                      .Post(productSelectionDraft)
                      .ExecuteAsync();
            Console.WriteLine($"Product selection: {productSelection.Id} with {productSelection.ProductCount} products");

            //Add product to the selection
            var addedProduct = new ProductSelectionAddProductAction()
            {
                Product = new ProductResourceIdentifier() { Id = productSelectionModel.ProductId }
            };
            var updateProductSelection = new ProductSelectionUpdate()
            {
                Actions=new List<IProductSelectionUpdateAction> { addedProduct },
                Version = productSelection?.Version ?? 0
            };
            var updatedProductSelection = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .ProductSelections()
                .WithId(productSelection?.Id)
                .Post(updateProductSelection)
                .ExecuteAsync();

            Console.WriteLine($"Berlin Product selection: {updatedProductSelection.Id} with {updatedProductSelection.ProductCount} products");


            var store = await _client.WithApi()
               .WithProjectKey(_projectKey)
               .Stores()
               .WithId(productSelectionModel.StoreId)
               .Get()
               .ExecuteAsync();
           
            var addProductSelection = new StoreAddProductSelectionAction()
            {
                ProductSelection = new ProductSelectionResourceIdentifier() { Id = productSelection.Id },
                Active = true,

            };
            var storeUpdate = new StoreUpdate()
            {
                Actions = new List<IStoreUpdateAction> { addProductSelection },
                Version = store.Version
            };

           var updatedStore = await _client.WithApi()
                .WithProjectKey(_projectKey)
                .Stores()
                .WithId(productSelectionModel.StoreId)
                .Post(storeUpdate)
                .ExecuteAsync();


            Console.WriteLine($"Updated store {updatedStore.Key} with selection {updatedStore.ProductSelections?.Count}");


        }
    }
}
