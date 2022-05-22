namespace Noodle_Assignment_API.Model
{
    public class CartMergeModel
    {
        public string ChannelId { get; set; }
        public string CustomerId { get; set; }
        public string SKU { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
    }
}
