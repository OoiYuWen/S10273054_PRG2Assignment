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
    class Restaurant //Done By Ooi Yu Wen
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }
        public Dictionary<string,Menu> Menus { get; set; } = new Dictionary<string,Menu>();
        public List<SpecialOffer> SpecialOffers { get; set; } = new List<SpecialOffer>();
        public Queue<Order> OrderQueue { get; set; } = new Queue<Order>();
        public Restaurant() { }
        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;
        }
        public void DisplayOrders()
        {
            Console.WriteLine("Displaying Orders...");
        }
        public void DisplaySpecialOffers()
        {
            foreach(SpecialOffer offer in SpecialOffers)
            {
                Console.WriteLine(offer);
            }
        }
        public void DisplayMenu()
        {
            foreach (Menu menu in Menus.Values)
            {
                menu.DisplayFoodItem();
            }    
        }
        public void AddMenu(Menu menu)
        {
            Menus.Add(menu.MenuId,menu); // add must add menu and menuid
        }
        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu.MenuId); // remove by key only 
        }
        public override string ToString()
        {
            return $"RestaurantId: {RestaurantId}, Name: {RestaurantName}, Email: {RestaurantEmail}";
        }
    }
}
