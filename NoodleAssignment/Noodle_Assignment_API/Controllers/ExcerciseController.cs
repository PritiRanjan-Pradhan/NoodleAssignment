


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
        public ExcerciseController(IDummyExcercise dummyExcercise,
            ICreateService createService,
            IUpdateGroupService updateGroupService,
            IImportApiService importApiService,
            IStateMachineService stateMachineService,
            ICheckoutService checkoutService,
            IMeService meService,
            ICartMerging cartMerging
            ,IInStore inStore
            ,ISerchService serchService)
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
        }
        [HttpGet("dummy-execute")]
        public Task<string> DummyExcercise()
        {
           return _dummyExcercise.ExecuteAsync();
        }

        [HttpPost("create-customer")]
        public Task<string> CreateCustomer()
        {
           return _createService.ExecuteAsync();
        }

        [HttpPost("set-customer-group")]
        public Task<string> SetCustomerGroup(UpdateServiceModel updateServiceModel)
        {
           return _updateGroupService.ExecuteAsync(updateServiceModel);
        }

        [HttpPost("import-api")]
        public Task<string> ImportAPI()
        {
           return  _importApiService.ExecuteAsync();
        }

        [HttpPost("create-update-state-transitions")]
        public Task<string> CreateUpdateStateTransitions()
        {
            return _stateMachineService.ExecuteAsync();
                
        }
        [HttpPost("checkout")]
        public async Task<string> Chekout()
        {
           return await _checkoutService.ExecuteAsync();
        }

        [HttpPost("myProfile")]
        public async Task<string> MyProfile()
        {
           return  await _meService.ExecuteAsync();
        }

        [HttpPost("mergecart")]
        public async Task<string> MergeCart()
        {
            return await _cartMerging.ExecuteAsync();
        }
        [HttpPost("")]
        public Task<string> CreateCartInStore()
        {
          return _inStore.ExecuteAsync();
        }
        [HttpPost("search")]
        public Task<string> Search()
        {
            return _serchService.ExecuteAsync();
        }
    }
}
