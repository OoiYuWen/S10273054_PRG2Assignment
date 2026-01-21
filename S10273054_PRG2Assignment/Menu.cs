using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273054_PRG2Assignment
{
    //========================================================== 
    // Student Number : S10273054
    // Student Name : Ooi Yu Wen
    // Partner Name : Chan Xin Chloe
    //========================================================== 
    class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public Restaurant Restaurant { get; set; }
        List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
        public Menu() { } 
        public Menu(string menuId, string menuName, Restaurant restaurant, List<FoodItem> foodItems)
        {
            MenuId = menuId;
            MenuName = menuName;
            Restaurant = restaurant;
        }
        public void AddFoodItem(FoodItem item)
        {
            FoodItems.Add(item);
        }
        public bool RemoveItem(FoodItem item)
        {
            return FoodItems.Remove(item);
        }
        public void DisplayFoodItem()
        {
            Console.WriteLine($"Menu:{MenuName}");
            foreach(FoodItem item in FoodItems)
            {
                Console.WriteLine($"");
            }
        }
        public override string ToString()
        {
            return "MenuId: " + MenuId + "MenuName" + MenuName + "Restaurant" + Restaurant;
        }
    }
}
