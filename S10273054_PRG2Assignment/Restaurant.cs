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
        public Restaurant() { }
        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;
        }
        
    }
}
