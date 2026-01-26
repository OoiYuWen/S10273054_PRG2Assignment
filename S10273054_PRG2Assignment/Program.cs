//========================================================== 
// Student Number : S10273054
// Student Name : Ooi Yu Wen
// Partner Name : Chan Xin Chloe
//========================================================== 


using S10273054_PRG2Assignment;

// 2) Load files (customers and orders)
List<Customer> custList = new List<Customer>();

void LoadCustomers(List<Customer> customers)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // Skip header line
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(',');
            string name = parts[0];
            string email = parts[1];
            Customer customer = new Customer(email, name);
            custList.Add(customer);
        }
    }
}
// 1) Load files (restaurants and food items) 
List<Restaurant> restaurantlist = new List<Restaurant>(); 
List<FoodItem> fooditemlist = new List<FoodItem>();
void LoadFoodItem (List<FoodItem> fooditemlist, List<Restaurant> restaurantlist)
{
    Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
    using (StreamReader sr = new StreamReader("fooditem.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(",");
            string restaurantid = parts[0];
            string itemname = parts[1];
            string itemdesc = parts[2];
            double itemprice = Convert.ToDouble(parts[3]);
            string customise = "N/A";

            FoodItem fooditem = new FoodItem(itemname, itemdesc, itemprice, customise);
            fooditemlist.Add(fooditem);

            Restaurant restaurant = null;
            foreach(Restaurant r in restaurantlist)
            {
                restaurant = r;
                continue;
            }
            if (restaurant != null)
            {
                // Create a new menu for the restaurant if it doesn't exist 
                if (!menus.ContainsKey(restaurantid)) //this checks whether there is a menu already or not 
                {
                    Menu newMenu = new Menu("M" + restaurantid, restaurant.RestaurantName + " Menu"); 
                    menus[restaurantid] = newMenu;
                    restaurant.AddMenu(newMenu);
                }
                // Add the food item to the restaurant's menu
                menus[restaurantid].AddFoodItem(fooditem);
            }
        }
    }
}
void LoadRestaurant(List<Restaurant> restaurantlist)
{
    using (StreamReader sr = new StreamReader("restaurant.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(",");
            string id = parts[0];
            string name = parts[1];
            string email = parts[2];

            Restaurant restaurant = new Restaurant(id, name, email);
            restaurantlist.Add(restaurant);
        }
    }
}

// 4) List all orders with basic information
void DisplayAllOrders(List<Order> orders, List<Customer> customers, List<Restaurant> restaurantlist)
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-10} {"Status",-10}");
    Console.WriteLine($"{"--------",-10} {"----------",-15} {"-------------",-15} {"------------------",-20} {"------",-10} {"---------",-10}");
}