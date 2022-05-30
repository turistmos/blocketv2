using System.Collections.Generic;


namespace blocket_lite.Models.ProductViewModel
{
    public class ItemViewModel
    {
        public List<ItemModel> ItemList { get; set; }
        public ItemModel Item { get; set; }
        public List<ItemModel> userItemList { get; set; }
        public List<ItemModel> likedItems { get; set; }
        public List<ItemModel> userLikedItems { get; set; }
        public List<ItemModel> addedToCartList { get; set; }
        public int cartSum { get; set; }
        public double finalPrice { get; set; }
        public double moms { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }
    }
}