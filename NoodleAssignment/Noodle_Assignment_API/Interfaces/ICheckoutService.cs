namespace Noodle_Assignment_API.Interfaces
{
    public interface ICheckoutService
    {
        Task ExecuteAsync(CheckoutModel checkoutModel);
    }
}
