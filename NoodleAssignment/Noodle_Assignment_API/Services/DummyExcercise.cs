

namespace Noodle_Assignment_API.Services
{
    public class DummyExcercise : IDummyExcercise
    {
        public async Task<string> ExecuteAsync()
        {
           await Task.CompletedTask;
            return "DUmmy excervise";
        }
    }
}
