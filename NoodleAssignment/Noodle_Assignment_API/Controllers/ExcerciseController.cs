
namespace Noodle_Assignment_API.Controllers
{
    [Route("excercise")]
    [ApiController]
    public class ExcerciseController : ControllerBase
    {
        private readonly IDummyExcercise _dummyExcercise;
        private readonly ICreateService _createService;
        public ExcerciseController(IDummyExcercise dummyExcercise,ICreateService createService)
        {
            _dummyExcercise = dummyExcercise;
            _createService = createService;
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
    }
}
