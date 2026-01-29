//========================================================== 
// Student Number : S10273054
// Student Name : Ooi Yu Wen
// Partner Name : Chan Xin Chloe
//========================================================== 

// Chloe: 2,3,5,7
// Yu Wen: 1,4,6,8

using Microsoft.VisualBasic;
using S10273054_PRG2Assignment;
using System.Numerics;
//Actual Program Starts Here
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");

// Collections to hold data
Dictionary<string, Customer> custDict = new Dictionary<string, Customer>();
Dictionary<int, Order> orderDict = new Dictionary<int, Order>();

Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
List<FoodItem> fooditemlist = new List<FoodItem>();
Dictionary<string, Restaurant> RestaurantDict = new Dictionary<string, Restaurant>();

// Load Data from CSV files
LoadRestaurant(RestaurantDict);
LoadFoodItem(fooditemlist, menus, RestaurantDict);
LoadCustomers(custDict);
LoadOrders(orderDict, RestaurantDict, custDict);

// 2) Load files (customers and orders)
void LoadCustomers(Dictionary<string, Customer> custDict)
{
    int counter = 0;
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // Skip header line
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(',');
            string name = parts[0];
            string email = parts[1];
            Customer customer = new Customer(email, name);
            custDict.Add(customer.CustomerName, customer);
            counter++;
            Console.WriteLine($"{counter} customers loaded!");
        }
    }
}
void LoadOrders(Dictionary<int, Order> orderList, Dictionary<string, Restaurant> RestaurantDict, Dictionary<string, Customer> custDict)
{
    int counter = 0;
    // Read orders csv and create order objects
    using (StreamReader sr = new StreamReader("orders - Copy.csv"))
    {
        string? s = sr.ReadLine(); // Skip header line
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(',');
            int orderId = Convert.ToInt32(parts[0]);
            string email = parts[1];
            string restuarantId = parts[2];
            string deliveryDate = parts[3]; // put as string first to put date and time together to make them DateTime
            string deliveryTime = parts[4];
            string deliveryAddress = parts[5];
            DateTime createdDateTime = Convert.ToDateTime(parts[6]);
            double totalAmount = Convert.ToDouble(parts[7]);
            string status = parts[8];
            string items = parts[9];

            string deliveryDT = parts[3] + " " + parts[4];
            DateTime deliveryDateTime = Convert.ToDateTime(deliveryDT);

            // Find restaurant
            Restaurant r = SearchRestaurant(RestaurantDict, restuarantId);
            // Find Customer
            Customer c = SearchCustomer(custDict, email);

            Order order = new Order(orderId, createdDateTime, totalAmount, status, deliveryDateTime, deliveryAddress, "Unknown", false, r, c);
            counter++;
            Console.WriteLine($"{counter} orders loaded!");

            // Add to restaurant's order queue
            r.OrderQueue.Enqueue(order);
            // Add to customer's order list
            c.AddOrder(order);

        }
    }
}
    
// Search function for customer
Customer SearchCustomer(Dictionary<string, Customer> custDict, string emailAddress)
{
    return custDict[emailAddress];
}


// 1) Load files (restaurants and food items) 
void LoadFoodItem (List<FoodItem> fooditemlist, Dictionary<string,Menu> MenuList, Dictionary<string, Restaurant> RestaurantDict)
{
    int counter = 0;
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
            counter++;
            Console.WriteLine($"{counter} food items loaded!");

            Menu menu = new Menu("001", "Main Menu");
            menu.AddFoodItem(fooditem);

            Restaurant r = SearchRestaurant(RestaurantDict, restaurantid);
            r.AddMenu(menu);
        }
    }
}

Restaurant SearchRestaurant(Dictionary<string, Restaurant> RestaurantDict, string restaurantid)
{
    return RestaurantDict[restaurantid];
}
// Extra information: 
//create restaurant, create menu, use function to add fooditem to menu, then menu add to restaurant
           /*Restaurant restaurant = null;
            foreach(Restaurant r in restaurantlist)
            {
                restaurant = r;
                continue;
            }
            if (r != null)
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
            }  */
void LoadRestaurant(Dictionary<string,Restaurant> RestaurantDict)
{
    int counter = 0;
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
            RestaurantDict.Add(id,restaurant);
            counter++;
            Console.WriteLine($"{counter} restaurants loaded!");
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