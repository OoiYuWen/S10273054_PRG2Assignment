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
    // Partner Name : Chan Xin Chloe 
    //========================================================== 
    class OrderedFoodItem : FoodItem // Done By Chan Xin Chloe 
    {
        // Attributes
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

        // Constuctors
        public OrderedFoodItem() { }
        public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, string customise, int qtyOrdered, double subTotal) : base(itemName, itemDesc, itemPrice, customise)
        {
            QtyOrdered = qtyOrdered;
            SubTotal = subTotal;
        }

        // Methods
        public double CalculateSubTotal()
        {
            SubTotal = ItemPrice * QtyOrdered;
            return SubTotal;
        }
    }
}
