using System;
namespace blocket_lite.Models
{
    public class ItemModel
    {
        //miles,year,color


        public string Category { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

        public int Miles { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Gender { get; set; }

        public string Image { get; set; }
        public string Username { get; set; }

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

