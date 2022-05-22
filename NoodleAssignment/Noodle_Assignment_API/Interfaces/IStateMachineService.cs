namespace Noodle_Assignment_API.Interfaces
{
    public interface IStateMachineService
    {
        Task<string> ExecuteAsync(StateMachineModel stateMachineModel);
    }
}
