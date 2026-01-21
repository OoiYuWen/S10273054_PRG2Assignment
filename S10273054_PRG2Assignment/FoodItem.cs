using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273054_PRG2Assignment
{
    class FoodItem
    //========================================================== 
    // Student Number : S10273054
    // Student Name : Ooi Yu Wen
    // Partner Name : Chloe Chan Xin
    //========================================================== 
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
            return "Item Name: " + ItemName + "Item Desc: " + ItemDesc + "Item Price: " + ItemPrice + "Customise: " + Customise;
        }
    }
}
