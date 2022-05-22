namespace Noodle_Assignment_API.Model
{
    public class InStoreModel
    {
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public LineItemModel LineItemModel { get; set; }
    }
}
