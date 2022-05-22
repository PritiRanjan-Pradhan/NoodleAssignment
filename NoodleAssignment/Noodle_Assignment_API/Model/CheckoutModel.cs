namespace Noodle_Assignment_API.Model
{
    public class CheckoutModel
    {
        
        public CustomerModel CustomerModel { get; set; }
        public LineItemModel LineItemModel { get; set; }
        public List<AddedLineItemModel> AddedLineItems { get; set; }
        public CartModel CartModel { get; set; }
        public DiscountCode DiscountCode { get; set; }
    }
}
