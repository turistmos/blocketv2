using System;
namespace blocket_lite.Models
{
    public class ItemModel
    {
        //miles,year,color

        public string category { get; set; }
        public string title { get; set; }
        public int price { get; set; }
        public string description { get; set; }

        public int miles { get; set; }
        public int year { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string gender { get; set; }

        public string image { get; set; }
        public string username { get; set; }

        public int ProductID { get; set; }

        public int Cart { get; set; }



    }
    public enum filterVal
    {
        PriceUp,
        PriceDown,
        AÖ,
        ÖA
        
    }



}

