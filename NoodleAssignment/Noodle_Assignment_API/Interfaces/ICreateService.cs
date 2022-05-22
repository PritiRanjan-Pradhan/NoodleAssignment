namespace Noodle_Assignment_API.Interfaces
{
    public interface ICreateService
    {
        Task<string> ExecuteAsync(CreateCustomer customer);
    }
}
