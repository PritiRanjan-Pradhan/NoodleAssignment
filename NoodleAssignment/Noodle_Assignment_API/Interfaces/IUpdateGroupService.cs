
using Noodle_Assignment_API.Model;

namespace Noodle_Assignment_API.Interfaces
{
    public interface IUpdateGroupService
    {
        Task<string> ExecuteAsync(UpdateServiceModel updateServiceModel);
    }
}
