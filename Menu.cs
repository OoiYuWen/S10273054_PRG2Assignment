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
    class Menu //Done By Ooi Yu Wen 
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
        public Menu() { }
        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
        }
        public void AddFoodItem(FoodItem item)
            // Need the FoodItem as FoodItem is the data type
            // writing FoodItem shows that the parameter is inside the FoodItem class
        {
            FoodItems.Add(item);
        }
        public bool RemoveItem(FoodItem item)
        {
            return FoodItems.Remove(item);
        }
        public void DisplayFoodItem()
        {
            foreach(FoodItem item in FoodItems) // loops through each fooditem object stored in fooditem list 
            {
                Console.WriteLine($"  - {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice.ToString("0.00")}");
            }
        }
        public override string ToString()
        {
            return $"MenuId: {MenuId}, MenuName: {MenuName}";
        }
    }
}
