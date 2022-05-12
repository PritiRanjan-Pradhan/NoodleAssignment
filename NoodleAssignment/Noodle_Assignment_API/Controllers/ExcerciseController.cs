


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
        public ExcerciseController(IDummyExcercise dummyExcercise,
            ICreateService createService,
            IUpdateGroupService updateGroupService,
            IImportApiService importApiService,
            IStateMachineService stateMachineService,
            ICheckoutService checkoutService)
        {
            _dummyExcercise = dummyExcercise;
            _createService = createService;
            _updateGroupService = updateGroupService;
            _importApiService = importApiService;
            _stateMachineService = stateMachineService;
           _checkoutService = checkoutService;
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
    }
}
