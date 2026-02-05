using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273054_PRG2Assignment
{
    //========================================================== 
    // Student Number : S10273054(OoiYuWen)
    // Student Name : Ooi Yu Wen
    // Partner Name : Chloe Chan Xin
    //========================================================== 
    class FoodItem // Done By Ooi Yu Wen
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public string Customise { get; set; }

        public FoodItem() { }
        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise)
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = customise;
        }
        public override string ToString()
        {
            return $"ItemName:{ItemName}, ItemDesc: {ItemDesc}, ItemPrice: {ItemPrice:F2}, Customise: {Customise}";
        }
    }
}
