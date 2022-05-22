namespace Noodle_Assignment_API.Interfaces
{
    public interface ICartMerging
    {
        Task ExecuteAsync(CartMergeModel cartMergeModel);
    }
}
