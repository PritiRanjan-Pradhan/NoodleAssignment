namespace Noodle_Assignment_API.Interfaces
{
    public interface IMeService
    {
        public Task<string> ExecuteAsync(MeClientModel meClientModel);
    }
}
