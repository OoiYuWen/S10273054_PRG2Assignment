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
using System.Runtime.CompilerServices;
// Actual Program Starts Here
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");

// Collections to hold data
Dictionary<string, Customer> custDict = new Dictionary<string, Customer>();
Dictionary<int, Order> orderDict = new Dictionary<int, Order>();

Dictionary<string, Menu> menuDict = new Dictionary<string, Menu>();
List<FoodItem> fooditemlist = new List<FoodItem>();
Dictionary<string, Restaurant> RestaurantDict = new Dictionary<string, Restaurant>();

List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();  // Might not need this since adding to Order's OrderedFoodItem list directly. Might remove this and the code below in feature 5

// Load Data from CSV files
LoadRestaurant(RestaurantDict);
LoadFoodItem(fooditemlist, menuDict, RestaurantDict);
LoadCustomers(custDict);
LoadOrders(orderDict, RestaurantDict, custDict);


int option = -1;

// Main menu    
while (option != 0)
{
    DisplayMainMenu();
    Console.Write("Enter your choice: ");
    option = Convert.ToInt32(Console.ReadLine());

    if (option == 1)
    {
        // 3) List all restaurants and menu items
        Displayrestaurants_MenuItems(RestaurantDict);
    }

    else if (option == 2)
    {
        // List all orders
        /*Console.WriteLine("All Orders");
        Console.WriteLine("==========");
        Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-10} {"Status",-10}");
        foreach (Order order in orderDict.Values)
        {
            Console.WriteLine($"{order.OrderId,-10} {order.Customer.CustomerName,-15} {order.Restaurant.RestaurantName,-15} {order.DeliveryDateTime,-20} {order.OrderTotal,-10:F2} {order.OrderStatus,-10}");

        }*/
        DisplayAllOrders(orderDict);
    }

    else if (option == 3)
    {
        CreateNewOrder(RestaurantDict, custDict);
    }
       
}

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
            custDict.Add(customer.EmailAddress, customer);
            counter++;
        }
    }
    Console.WriteLine($"{counter} customers loaded!");
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
            string createdDateTime = (parts[6]);
            DateTime CreatedDateTime = Convert.ToDateTime(createdDateTime);
            double totalAmount = Convert.ToDouble(parts[7]);
            string status = parts[8];
            string items = parts[9];

            string deliveryDT = parts[3] + " " + parts[4];
            DateTime deliveryDateTime = Convert.ToDateTime(deliveryDT);

            // Find restaurant
            Restaurant r = SearchRestaurant(RestaurantDict, restuarantId);
            // Find Customer
            Customer c = SearchCustomer(custDict, email);

            Order order = new Order(orderId, CreatedDateTime, totalAmount, status, deliveryDateTime, deliveryAddress, "Unknown", false, r, c);
            counter++;

            // Add to restaurant's order queue
            r.OrderQueue.Enqueue(order);
            // Add to customer's order list
            c.AddOrder(order);

        }
    }
    Console.WriteLine($"{counter} orders loaded!");
}
    
    
// Search function for customer
Customer SearchCustomer(Dictionary<string, Customer> custDict, string emailAddress)
{
    return custDict[emailAddress];
}


// Basic feature 3 method
void Displayrestaurants_MenuItems(Dictionary<string, Restaurant> RestaurantDict)
{
    Console.WriteLine();
    Console.WriteLine("All Restaurants and Menu Items");
    Console.WriteLine("==============================");
    foreach (KeyValuePair<string, Restaurant> kvp in RestaurantDict)
    {
        Console.WriteLine($"Restaurant: {kvp.Value.RestaurantName} ({kvp.Key})");
        kvp.Value.DisplayMenu();
        Console.WriteLine();
    }
}

// Basic feature 5 method
void CreateNewOrder(Dictionary<string, Restaurant> RestaurantDict, Dictionary<string, Customer> custDict)
{
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");

    Console.Write("Enter Customer Email: ");
    string CustEmail = Console.ReadLine();
    // Find customer
    Customer c = SearchCustomer(custDict, CustEmail);


    Console.Write("Enter Restaurant ID: ");
    string RestID = Console.ReadLine();
    // Find restaurant
    Restaurant r = SearchRestaurant(RestaurantDict, RestID);

    Console.Write("Enter Delivery Date (dd/mm/yyyy):");
    DateTime DeliveryD = Convert.ToDateTime(Console.ReadLine());

    Console.Write("Enter Delivery Time (hh:mm):");
    DateTime DeliveryT = Convert.ToDateTime(Console.ReadLine());

    Console.Write("Enter Delivery Address:");
    string DeliveryAddr = Console.ReadLine();

    // Showing available food items
    Console.WriteLine();
    Console.WriteLine("Available Food Items:");
    List<FoodItem> foodList = new List<FoodItem>(); // Temporary list to hold food items
    foreach (Restaurant res in RestaurantDict.Values)
    {
        if (res.RestaurantId == RestID)
        {

            foreach (Menu m in res.Menus.Values)
            {
                int count = 0;
                foreach (FoodItem fi in m.FoodItems)
                {
                    count++;
                    foodList.Add(fi);  // Store items for lookup later
                    Console.WriteLine($"{count}. {fi.ItemName} - ${fi.ItemPrice.ToString("0.00")}");
                }
            }
        }
    }

    // User inputs

    int itemNo = -1;
    double totalAmount = 0.0;
    while (itemNo != 0)
    {
        Console.Write("Enter item number (0 to finish): ");
        itemNo = Convert.ToInt32(Console.ReadLine());

        if (itemNo == 0)
            break;

        if (option < 1 || option > foodList.Count)
        {
            Console.WriteLine("Invalid option, try again.");
            continue;
        }

        Console.Write("Enter quantity: ");
        int qty = Convert.ToInt32(Console.ReadLine());

        // Find chosen item
        FoodItem selectedFoodItem = foodList[itemNo - 1];

        // Calculate subtotal
        double subtotal = qty * selectedFoodItem.ItemPrice;

        // Create OrderedFoodItem and add to orderedFoodItem List
        OrderedFoodItem ofi = new OrderedFoodItem(selectedFoodItem.ItemName, selectedFoodItem.ItemDesc, selectedFoodItem.ItemPrice, selectedFoodItem.Customise, qty, subtotal);
        orderedItems.Add(ofi);

        totalAmount += subtotal;

        // Add OrderedFoodItem to Order's OrderedFoodItem list
    }

    Console.Write("Add special request? [Y/N]: ");
    string specialReq = Console.ReadLine();
    Console.WriteLine();

    // Calculate total amount
    double delivery = 5.00;
    Console.WriteLine($"Order Total: ${totalAmount} + ${delivery} (delivery) = ${totalAmount + delivery}");
    Console.Write("Proceed to payment? [Y/N]: ");
    string proceed = Console.ReadLine();
    Console.WriteLine();

    if (proceed == "Y")
    {
        Console.WriteLine("Payment method: ");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        string paymentMethod = Console.ReadLine().ToUpper();
        Console.WriteLine();
        Console.WriteLine($"Order {1004} created successfully! Status: {"pending"}");

        // Save payment method

    }
    if (proceed == "N")
    {
        Console.WriteLine("Cancelled payment. Exiting order feature...");
        return;
    }
    else
    {
        Console.WriteLine("Invalid option, try again.");
    }
}

// 1) Load files (restaurants and food items) 
void LoadFoodItem (List<FoodItem> fooditemlist, Dictionary<string,Menu> MenuDict, Dictionary<string, Restaurant> RestaurantDict)
{
    int counter = 0;
    using (StreamReader sr = new StreamReader("fooditems - Copy.csv"))
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

            Restaurant r = SearchRestaurant(RestaurantDict, restaurantid);

            // Check if restaurant has a '001' menu
            if (!r.Menus.ContainsKey("001"))
            {
                r.AddMenu(new Menu("001", "Main Menu"));
            }
            r.Menus["001"].AddFoodItem(fooditem);

        }
    }
    Console.WriteLine($"{counter} food items loaded!");
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
    using (StreamReader sr = new StreamReader("restaurants.csv"))
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
        }
    }
    Console.WriteLine($"{counter} restaurants loaded!");
}

// 4) List all orders with basic information
void DisplayAllOrders(Dictionary<int,Order> orderDict)
    /*List<Customer> customers, List<Restaurant> restaurantlist*/
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-10} {"Status",-10}");
    Console.WriteLine($"{"--------",-10} {"----------",-15} {"-------------",-15} {"------------------",-20} {"------",-10} {"---------",-10}");
}

// Display Main menu method
void DisplayMainMenu()
{
    Console.WriteLine();
    Console.WriteLine("===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("0. Exit");
}