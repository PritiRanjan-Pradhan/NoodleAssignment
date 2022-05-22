namespace Noodle_Assignment_API.Model
{
    public class CartModel
    {
       
        public string Currency { get; set; }

    }
    public class LineItemModel
    {
        public string ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
    public class AddedLineItemModel : LineItemModel
    { }
    public class DiscountCode
    {
        public string DiscoutCodeId { get; set; }
    }
}
