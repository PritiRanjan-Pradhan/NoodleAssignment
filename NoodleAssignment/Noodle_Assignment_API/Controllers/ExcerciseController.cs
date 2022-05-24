


namespace Noodle_Assignment_API.Controllers
{
    [Route("excercise")]
    [ApiController]
    public class ExcerciseController : ControllerBase
    {
        private readonly IDummyExcercise _dummyExcercise;
        private readonly ICreateService _createService;
        private readonly IUpdateGroupService _updateGroupService;
        private readonly IImportApiService _importApiService;
        private readonly IStateMachineService _stateMachineService;
        private readonly ICheckoutService _checkoutService;
        private readonly IMeService _meService;
        private readonly ICartMerging _cartMerging;
        private readonly IInStore _inStore;
        private readonly ISerchService _serchService;
        private readonly IPagedQuery _pagedQuery;
        private readonly IGraphQLService _graphQLService;
        private readonly ICustomType _customType;
        private readonly ICustomObjectService _customObjectService;
        private readonly IApiExtensionService _apiExtensionService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly IProductSelectionService _productSelectionService;
        public ExcerciseController(IDummyExcercise dummyExcercise,
            ICreateService createService,
            IUpdateGroupService updateGroupService,
            IImportApiService importApiService,
            IStateMachineService stateMachineService,
            ICheckoutService checkoutService,
            IMeService meService,
            ICartMerging cartMerging
            , IInStore inStore
            , ISerchService serchService,
            IPagedQuery pagedQuery,
            IGraphQLService graphQLService,
            ICustomType customType,
            ICustomObjectService customObjectService,
            ISubscriptionService subscriptionService,
            IApiExtensionService apiExtensionService,
            IErrorHandlingService errorHandlingService,
            IProductSelectionService productSelectionService)
        {
            _dummyExcercise = dummyExcercise;
            _createService = createService;
            _updateGroupService = updateGroupService;
            _importApiService = importApiService;
            _stateMachineService = stateMachineService;
            _checkoutService = checkoutService;
            _meService = meService;
            _cartMerging = cartMerging;
            _inStore = inStore;
            _serchService = serchService;
            _pagedQuery = pagedQuery;
            _graphQLService = graphQLService;
            _customType = customType;
            _customObjectService = customObjectService;
            _subscriptionService = subscriptionService;
            _apiExtensionService = apiExtensionService;
            _errorHandlingService = errorHandlingService;
            _productSelectionService = productSelectionService;
        }
        [HttpGet("dummy-execute")]
        public Task<string> DummyExcercise()
        {
            return _dummyExcercise.ExecuteAsync();
        }

        [HttpPost("create-customer")]
        public Task<string> CreateCustomer([FromBody] CreateCustomer customer)
        {
            return _createService.ExecuteAsync(customer);
        }

        [HttpPost("set-customer-group")]
        public Task<string> SetCustomerGroup(UpdateServiceModel updateServiceModel)
        {
            return _updateGroupService.ExecuteAsync(updateServiceModel);
        }

        [HttpPost("import-api")]
        public Task<string> ImportAPI()
        {
            return _importApiService.ExecuteAsync();
        }

        [HttpPost("create-update-state-transitions")]
        public Task<string> CreateUpdateStateTransitions(StateMachineModel stateMachineModel)
        {
            return _stateMachineService.ExecuteAsync(stateMachineModel);

        }
        [HttpPost("checkout")]
        public async Task<string> Chekout()
        {
            return await _checkoutService.ExecuteAsync();
        }

        [HttpPost("myProfile")]
        public async Task<string> MyProfile([FromBody] MeClientModel meClient)
        {
            return await _meService.ExecuteAsync(meClient);
        }

        [HttpPost("product-selection")]
        public async Task ProductSelection(ProductSelectionModel productSelectionModel)
        {
             await _productSelectionService.ExecuteAsync(productSelectionModel);
        }

        [HttpPost("mergecart")]
        public Task MergeCart(CartMergeModel cartMergeModel)
        {
            return _cartMerging.ExecuteAsync(cartMergeModel);
        }
        [HttpPost("in-store-service")]
        public Task<string> CreateCartInStore([FromBody] InStoreModel inStoreModel)
        {
            return _inStore.ExecuteAsync(inStoreModel);
        }
        [HttpPost("search/{productTypeKey}")]
        public Task Search(string productTypeKey)
        {
            return _serchService.ExecuteAsync(productTypeKey);
        }

        [HttpPost("getProductsSortById")]
        public Task<string> GetProductsSortById()
        {
            return _pagedQuery.ExecuteAsync();
        }
        [HttpPost("graphql")]
        public Task GraphQL()
        {
            return _graphQLService.ExecuteAsync();
        }

        [HttpPost("add-customfield-to-customer")]
        public Task AddCustomFieldToCustomer()
        {
            return _customType.ExecuteAsync();
        }
        [HttpPost("create-custom-object")]
        public Task CreateCustomObjectService()
        {
            return _customObjectService.ExecuteAsync();
        }

        [HttpPost("create-api-extension-service")]
        public Task CreateAPIExtension()
        {
            return _apiExtensionService.ExcuteAsync();
        }

        [HttpPost("create-subscription")]
        public Task CreateSubscription()
        {
            return _subscriptionService.ExecuteAsync();
        }

        [HttpPost("error-handling")]
        public Task ErrorHandler()
        {
            return _errorHandlingService.ExecuteAsync();
        }
    }
}
