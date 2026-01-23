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
    class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }
        List<Menu> Menus { get; set; } = new List<Menu>();
        List<SpecialOffer> SpecialOffers { get; set; } = new List<SpecialOffer>();
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
            foreach (SpecialOffer offer in SpecialOffers)
            {
                Console.WriteLine(offer);
            }
        }
        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }
        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu);
        }
        public override string ToString()
        {
            return $"RestaurantId: {RestaurantId}, Name: {RestaurantName}, Email: {RestaurantEmail}";
        }
    }
}
