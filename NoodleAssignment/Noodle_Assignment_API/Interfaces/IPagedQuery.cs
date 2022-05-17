namespace Noodle_Assignment_API.Interfaces
{
    public interface IPagedQuery
    {
        Task<string> ExecuteAsync();
    }
}
