//========================================================== 
// Student Number : S10273054
// Student Name : Ooi Yu Wen
// Partner Name : Chan Xin Chloe
//========================================================== 

// Chloe: 2,3,5,7
// Yu Wen: 1,4,6,8

using Microsoft.VisualBasic;
using S10273054_PRG2Assignment;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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

Stack<Order> refundstack = new Stack<Order>(); // basic feature 6 - refund stack 

List<SpecialOffer> specialOfferList = new List<SpecialOffer>();  // Bonus feature

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
        DisplayAllOrders(orderDict);
    }

    else if (option == 3)
    {
        CreateNewOrder(RestaurantDict, custDict);
    }
    else if (option == 4)
    {
        ProcessAnOrder(orderDict);
    }
    else if (option == 5)
    {
        ModifyExistingOrder();
    }
    else if (option == 6)
    {
        Deleteexistingorder(orderDict, refundstack);
    }
    else if (option == 7)
    {
        BulkProcessOrders(orderDict);
    }
    else if (option == 8)
    {
        DisplayTotalOrderAmt(RestaurantDict, refundstack);
    }
    else if (option == 0)
    {
        Console.WriteLine("Exiting the Gruberoo Food Delivery System. Goodbye!");
    }
    else
    {
        Console.WriteLine("Invalid option, please try again.");
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
            if (parts.Length < 10) continue;

            // First 9 fields are fixed
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

            // Rest of the line belongs to Items(join back together)
            string itemsData = string.Join(",", parts.Skip(9)).Trim('"');  // skip here means to skip the first 9 elements and join the rest back with comma which gives ["\"Chicken Katsu Bento", " 1|Vegetable Tempura Bento", " 1\""]

            string deliveryDT = parts[3] + " " + parts[4];
            DateTime deliveryDateTime = Convert.ToDateTime(deliveryDT);

            // Find restaurant
            Restaurant r = SearchRestaurant(RestaurantDict, restuarantId);
            // Find Customer
            Customer c = SearchCustomer(custDict, email);

            Order order = new Order(orderId, CreatedDateTime, totalAmount, status, deliveryDateTime, deliveryAddress, "Unknown", true, r, c);
            counter++;

            // Split items and add them to OrderedFoodItem
            string[] items = itemsData.Split('|');

            foreach(string item in items)
            {
                // split by (,) and trim spaces
                string[] part = item.Split(',');
                string itemName = part[0].Trim();
                int qtyOrdered = Convert.ToInt32(part[1].Trim());

                FoodItem foodItem = null;

                foreach (Menu m in r.Menus.Values)
                {
                    foreach (FoodItem fi in m.FoodItems)
                    {
                        if (fi.ItemName == itemName)
                        {
                            foodItem = fi;
                            break;
                        }
                    }
                }
                if (foodItem != null)
                {
                    double subtotal = qtyOrdered * foodItem.ItemPrice;
                    OrderedFoodItem ofi = new OrderedFoodItem(foodItem.ItemName, foodItem.ItemDesc, foodItem.ItemPrice, foodItem.Customise, qtyOrdered, subtotal);
                    order.AddOrderedFoodItem(ofi);
                }
            }

            
            // Add to global order dictionary
            orderDict.Add(orderId, order);

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
    try
    {
        return custDict[emailAddress];
    }
    catch (KeyNotFoundException)
    {
        return null;
    }

}


// 3) Basic feature 3 method (List all restaurants and menu items)
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

// 5) Basic feature 5 method (Create a new Order)
void CreateNewOrder(Dictionary<string, Restaurant> RestaurantDict, Dictionary<string, Customer> custDict)
{
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");

    // Checking if customer exists
    Customer c = null;
    string CustEmail = string.Empty;
    while (c == null)
    {
        Console.Write("Enter Customer Email: ");
        CustEmail = Console.ReadLine();

        // Find customer
        c = SearchCustomer(custDict, CustEmail);


        if (c == null)
        {
            Console.WriteLine("Error: Customer not found! Please try again.");
            Console.WriteLine();
        }
    }
    Console.WriteLine($"Customer found: {c.CustomerName}");
    Console.WriteLine();


    // Checking if restaurant exists
    Restaurant r = null;
    string RestID = string.Empty;
    while (r == null)
    {
        Console.Write("Enter Restaurant ID: ");
        RestID = Console.ReadLine();
        r = SearchRestaurant(RestaurantDict, RestID);


        if (r == null)
        {
            Console.WriteLine("Error: Restaurant not found! Please try again.");
            Console.WriteLine();
        }
    }
    Console.WriteLine($"Restaurant found: {r.RestaurantName}");
    Console.WriteLine();


    // Delivery Date input
    DateTime deliveryDate;
    while (true)
    {
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string DeliveryD = Console.ReadLine();

        if (DateTime.TryParse(DeliveryD, out deliveryDate))   // Faster way to check date format
        {
            break; // valid date
        }
        else
        {
            Console.WriteLine("Error: Invalid date format. Please use dd/mm/yyyy");
            Console.WriteLine();

        }
    }


    // Delivery Time input: Need to use a stricter error handling meathod since normal error handling accepts numbers like "1234". 
    TimeSpan deliveryTime;
    while (true)
    {
        Console.Write("Enter Delivery Time (hh:mm): ");
        string DeliveryT = Console.ReadLine();

        if (TimeSpan.TryParseExact(DeliveryT, "hh\\:mm", null, out deliveryTime))       // TryParseExact only accepts the exact format I specify (hh:mm) so that inputs like "1234" will be rejected
        {
            break; // valid time in hh:mm format
        }
        else
        {
            Console.WriteLine("Error: Invalid time format. Please use hh:mm");
            Console.WriteLine();
        }
    }



    string deliveryDateTime = deliveryDate.ToString("dd/MM/yyyy") + " " + deliveryTime.ToString("hh\\:mm");
    DateTime DeliveryDT = DateTime.ParseExact(deliveryDateTime, "dd/MM/yyyy HH:mm", null);


    // Checking if delivery address is empty
    string DeliveryAddr;
    while (true)
    {
        Console.Write("Enter Delivery Address: ");
        DeliveryAddr = Console.ReadLine();

        if (DeliveryAddr == null || DeliveryAddr == "")
        {
            Console.WriteLine("Error: Delivery address cannot be empty!");
            continue; // go back to the top of the loop, re‑prompt
        }

        break; // valid input, exit loop
    }

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
    Order newOrder = new Order();
    while (itemNo != 0)
    {
        Console.Write("Enter item number (0 to finish): ");
        string choice = Console.ReadLine();

        try
        {
            itemNo = Convert.ToInt32(choice);
        }
        catch
        {
            Console.WriteLine("Error: Please enter a valid number.");
            Console.WriteLine();
            continue; // go back to asking for item number
        }


        if (itemNo == 0)
            break;

        if (itemNo < 1 || itemNo > foodList.Count)
        {
            Console.WriteLine("Invalid option, try again.");
            Console.WriteLine();
            continue;
        }

        // qty input and validation
        int qty;
        while (true)
        {
            Console.Write("Enter quantity: ");
            string input = Console.ReadLine();

            try
            {
                qty = Convert.ToInt32(input);
                if (qty < 1)
                {
                    Console.WriteLine("Error: Quantity must be at least 1.");
                    Console.WriteLine();
                    continue;   // ask again
                }
                break;  // valid input, exit loop
            }
            catch
            {
                Console.WriteLine("Error: Please enter a valid number.");
                continue;
            }
        }

        // Find chosen item
        FoodItem selectedFoodItem = foodList[itemNo - 1];

        // Calculate subtotal
        double subtotal = qty * selectedFoodItem.ItemPrice;

        // Create OrderedFoodItem and add to orderedFoodItem List.
        OrderedFoodItem ofi = new OrderedFoodItem(selectedFoodItem.ItemName, selectedFoodItem.ItemDesc, selectedFoodItem.ItemPrice, selectedFoodItem.Customise, qty, subtotal);
        orderedItems.Add(ofi);  // Adding order to global list - Might not need
        newOrder.AddOrderedFoodItem(ofi); // Adding to Order's OrderedFoodItem list directly
        
    }

    Console.Write("Add special request? [Y/N]: ");
    string specialReq = Console.ReadLine().Trim().ToUpper();
    if (specialReq == "Y")
    {
        string request;
        while (true)
        {
            Console.Write("Enter special request: ");
            request = Console.ReadLine();

            // Checking if special request is empty
            if (request == null || request.Trim() == "")
            {
                Console.WriteLine("Error: Special request cannot be empty!");
                continue;   // re-prompt
            }
            break;   // valid input, exit loop
        }
        
        // Loop through the customer's ordered food items to add the special request to each item
        foreach (OrderedFoodItem ofi in newOrder.OrderedList)
        {
            ofi.Customise = request;
        }
    }
    else if (specialReq == "N")
    {
        Console.WriteLine("No special request added.");
    }
    else
    {
        Console.WriteLine("Invalid option, no special request added.");
    }
    Console.WriteLine();

    // Calculate total amount
    double totalAmount = newOrder.CalculateOrderTotal();    // After adding the OrderedFoodItems to the Order's OrderedFoodItem list, calculate the total amount
    double delivery = 5.00;
    Console.WriteLine($"Order Total: ${totalAmount.ToString("0.00")} + ${delivery.ToString("0.00")} (delivery) = ${(totalAmount + delivery).ToString("0.00")}");

    Console.Write("Proceed to payment? [Y/N]: ");
    string proceed = Console.ReadLine().ToUpper();
    Console.WriteLine();

    if (proceed == "Y")
    {
        string paymentMethod;
        while (true)
        {
            Console.WriteLine("Payment method: ");
            Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            paymentMethod = Console.ReadLine().ToUpper();

            if (paymentMethod == "CC" || paymentMethod == "PP" || paymentMethod == "CD")
            {
                break; // valid input, exit loop
            }
            else
            {
                Console.WriteLine("Error: Invalid payment method. Please enter CC, PP, or CD.");
                continue; // re-prompt
            }
        }
        Console.WriteLine();

        // Creating the New Order's ID by taking most recent orderID + 1
        int HighestOrderID = orderDict.Keys.Max();  // Keys are the order IDs in int, so Max() looks for the highest order ID
        int OrderId = HighestOrderID + 1;


        // Creating Order CreatedDateTime
        DateTime CreatedDateTime = DateTime.Now;


        // No need create another order object, just update all the values of the newOrder object created earlier
        newOrder.OrderId = OrderId;
        newOrder.OrderDateTime = CreatedDateTime;
        newOrder.OrderTotal = totalAmount + delivery;
        newOrder.OrderStatus = "Pending";
        newOrder.DeliveryDateTime = DeliveryDT;
        newOrder.DeliveryAddress = DeliveryAddr;
        newOrder.OrderPaymentMethod = paymentMethod;
        newOrder.OrderPaid = true;
        newOrder.Customer = c;
        newOrder.Restaurant = r;


        // Creating new order object to be added to customer, restaurant, and global order dictionary
        // Add to customer OrderList
        c.AddOrder(newOrder);
        // Add to restaurant's order queue
        r.OrderQueue.Enqueue(newOrder);
        // Add to global order dictionary
        orderDict.Add(OrderId, newOrder);

        // Formatting the items for CSV
        string orderedItemsString = "";

        foreach (OrderedFoodItem item in newOrder.OrderedList)
        {
            // Add "ItemName, Quantity"
            orderedItemsString += item.ItemName + ", " + item.QtyOrdered;

            // Add separator if not the last item
            if (item != newOrder.OrderedList.Last())
            {
                orderedItemsString += "|";
            }
        }

        // Appending new order to orders - Copy.csv
        using (StreamWriter sw = new StreamWriter("orders - Copy.csv", true))
        {
            sw.WriteLine($"{newOrder.OrderId}," + $"{CustEmail}," + $"{RestID}," + $"{DeliveryDT:dd/MM/yyyy}," + $"{DeliveryDT:HH:mm}," + $"{DeliveryAddr}," + $"{CreatedDateTime:dd/MM/yyyy HH:mm}," + $"{totalAmount + delivery:F2}," + $"{newOrder.OrderStatus}," + $"\"{orderedItemsString}\"");
        }

        Console.WriteLine($"Order {OrderId} created successfully! Status: {newOrder.OrderStatus}");

    }
    else if (proceed == "N")
    {
        Console.WriteLine("Cancelled payment. Exiting order feature...");
        return;
    }
    else
    {
        Console.WriteLine("Invalid option, try again.");
    }

}

// 7) Basic feature 7 meathod (Modify an existing Order)
void ModifyExistingOrder()
{
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");


    Customer MC = null;
    string CustEmail = string.Empty;
    while (MC == null)
    {
        Console.Write("Enter Customer Email: ");
        CustEmail = Console.ReadLine();

        // Find customer
        MC = SearchCustomer(custDict, CustEmail);


        if (MC == null)
        {
            Console.WriteLine("Error: Customer not found! Please try again.");
            Console.WriteLine();
        }
    }

    // MC is guaranteed to be valid at this point
    Console.WriteLine("Pending Orders:");
    bool foundPending = false;              // Set false first

    foreach (Order o in MC.OrderList.Values)
    {
        if (o.OrderStatus == "Pending")
        {
            Console.WriteLine(o.OrderId);
            foundPending = true;            // Set to true if at least one pending order found
        }
    }

    if (!foundPending)          // If foundPending = false
    {
        Console.WriteLine("No pending orders found for this customer.");
        return;
    }

    // Error handling for Order ID input
    int OrderID;
    Order selectedOrder = null;
    string input;

    while (selectedOrder == null)
    {
        Console.Write("Enter Order ID: ");
        input = Console.ReadLine();

        try
        {
            OrderID = Convert.ToInt32(input);

            bool foundOrder = false;
            foreach (Order o in MC.OrderList.Values)
            {
                if (o.OrderId == OrderID)
                {
                    selectedOrder = o; // found the order
                    Console.WriteLine("Order found.");
                    foundOrder = true;
                    Console.WriteLine();

                    // Proceed to modify order
                    Console.WriteLine("Order Items:");
                    for (int i = 0; i < o.OrderedList.Count; i++)
                    {
                        OrderedFoodItem ofi = o.OrderedList[i];
                        Console.WriteLine($"{i + 1}. {ofi.ItemName} - {ofi.QtyOrdered}");
                    }
                    Console.WriteLine("Address:");
                    Console.WriteLine(o.DeliveryAddress);
                    Console.WriteLine("Delivery Date/Time:");
                    // Splitting the Date and Time for display
                    DateTime deliveryDate = o.DeliveryDateTime.Date; // Eg.(15/2/2026 00:00:00) Time component set to 00:00:00. Hence must format later to "dd/MM/yyyy"
                    TimeSpan deliveryTime = o.DeliveryDateTime.TimeOfDay; // Eg.(12:30:00)
                    Console.WriteLine($"{deliveryDate:dd/MM/yyyy}, {deliveryTime:hh\\:mm}");
                    Console.WriteLine();

                    // Error handling for modify options
                    int modifyOption;
                    while (true)
                    {
                        Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
                        input = Console.ReadLine();

                        try
                        {
                            modifyOption = Convert.ToInt32(input);

                            if (modifyOption >= 1 && modifyOption <= 3)
                            {
                                break; // valid option, exit loop
                            }
                            else
                            {
                                Console.WriteLine("Error: Please enter 1, 2, or 3.");
                                continue; // re‑prompt
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error: Please enter a number (1, 2, or 3).");
                            continue; // re‑prompt
                        }
                    }

                    if (modifyOption == 1)
                    {
                        // Show current ordered items
                        for (int i = 0; i < o.OrderedList.Count; i++)
                        {
                            OrderedFoodItem ofi = o.OrderedList[i];
                            Console.WriteLine($"{i + 1}. {ofi.ItemName} - {ofi.QtyOrdered}");
                        }

                        // ItemNo input and validation
                        int itemNo = 0;
                        bool valid = false;

                        while (!valid)
                        {
                            Console.Write("Enter item number to modify: ");
                            try
                            {
                                itemNo = Convert.ToInt32(Console.ReadLine());

                                if (itemNo >= 1 && itemNo <= o.OrderedList.Count)
                                {
                                    valid = true; // success, exit loop
                                }
                                else
                                {
                                    Console.WriteLine("Invalid item number. Please try again.");
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("That wasn't a number. Please try again.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Number too large. Please try again.");
                            }
                        }

                        if (itemNo >= 1 && itemNo <= o.OrderedList.Count)   // Not needed but just in case
                        {
                            double oldTotal = o.OrderTotal;
                            OrderedFoodItem selectedItem = o.OrderedList[itemNo - 1];

                            // Store old values before modifying
                            int OldQty = selectedItem.QtyOrdered;
                            double OldSubtotal = selectedItem.SubTotal;

                            // New quantity input and validation
                            int newQty;
                            while (true)
                            {
                                Console.Write($"Enter new quantity for {selectedItem.ItemName}: ");
                                input = Console.ReadLine();

                                try
                                {
                                    newQty = Convert.ToInt32(input);
                                    // Allows 0 but rejects negative values
                                    if (newQty < 0)
                                    {
                                        Console.WriteLine("Error: Quantity cannot be negative.");
                                        continue;   // ask again
                                    }
                                    break;  // valid input, exit loop
                                }
                                catch
                                {
                                    Console.WriteLine("Error: Please enter a valid number.");
                                    continue;
                                }
                            }

                            // Applying new values
                            selectedItem.QtyOrdered = newQty;
                            selectedItem.SubTotal = newQty * selectedItem.ItemPrice; // Update subtotal

                            // Recalculate order total
                            double delivery = 5.00;
                            double newTotal = o.CalculateOrderTotal() + delivery;    // CalculateOrderTotal() does not include delivery fees but the oldTotal does.
                            if (newTotal > oldTotal)
                            {
                                Console.WriteLine($"Additional payment of ${(newTotal - oldTotal).ToString("0.00")} required.");
                                Console.Write("Proceed to payment? [Y/N]: ");
                                string proceedPayment = Console.ReadLine().ToUpper();

                                if (proceedPayment == "Y")
                                {
                                    string paymentMethod;
                                    while (true)
                                    {
                                        Console.WriteLine("Payment method: ");
                                        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
                                        paymentMethod = Console.ReadLine().ToUpper();

                                        if (paymentMethod == "CC" || paymentMethod == "PP" || paymentMethod == "CD")
                                        {
                                            break; // valid input, exit loop
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error: Invalid payment method. Please enter CC, PP, or CD.");
                                            continue; // re-prompt
                                        }
                                    }
                                    Console.WriteLine();
                                    o.OrderTotal = newTotal;
                                    Console.WriteLine("Payment successful.");
                                    Console.WriteLine($"Order {o.OrderId} updated. New quantity for {selectedItem.ItemName}: {selectedItem.QtyOrdered}");
                                }
                                else if (proceedPayment == "N")
                                {
                                    // Revert changes
                                    selectedItem.QtyOrdered = OldQty; // Revert to old quantity
                                    selectedItem.SubTotal = OldSubtotal; // Revert subtotal
                                    Console.WriteLine("Modification cancelled.");
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option.");
                                }
                            }
                            else if (newTotal < oldTotal)
                            {
                                Console.WriteLine($"Refund of ${(oldTotal - newTotal).ToString("0.00")} will be processed.");
                                o.OrderTotal = newTotal;

                                // Create a refund order for the refund stack
                                Order refundOrder = new Order(o.OrderId, o.OrderDateTime, oldTotal - newTotal, "Cancelled", o.DeliveryDateTime, o.DeliveryAddress, o.OrderPaymentMethod, true, o.Restaurant, o.Customer);

                                // Push to refund stack
                                refundstack.Push(refundOrder);
                            }
                            else
                            {

                                Console.WriteLine("No change in total amount.");
                                Console.WriteLine($"Order {o.OrderId} updated. New quantity for {selectedItem.ItemName}: {selectedItem.QtyOrdered}");
                            }

                        }
                    }
                    else if (modifyOption == 2)
                    {
                        string newAddress;
                        while (true)
                        {
                            Console.Write("Enter new delivery address: ");
                            newAddress = Console.ReadLine();

                            if (newAddress == null || newAddress == "")
                            {
                                Console.WriteLine("Error: Delivery address cannot be empty!");
                                continue; // go back to the top of the loop, re‑prompt
                            }

                            break; // valid input, exit loop
                        }
                        o.DeliveryAddress = newAddress;
                        Console.WriteLine($"Order {o.OrderId} updated. New Delivery address: {o.DeliveryAddress}");
                    }
                    else
                    {
                        Console.Write("Enter new delivery time (hh:mm): ");
                        string newTime = Console.ReadLine();
                        string newDeliveryDT = deliveryDate.ToString("dd/MM/yyyy") + " " + newTime;     // Old delivery date + " " + new time = new delivery date time
                        DateTime newDeliveryDateTime = Convert.ToDateTime(newDeliveryDT);
                        o.DeliveryDateTime = newDeliveryDateTime;

                        Console.WriteLine($"Order {o.OrderId} updated. New Delivery time: {newTime}");
                    }

                    break;
                }
            }

            if (selectedOrder == null)
            {
                Console.WriteLine("Error: Order not found. Please try again.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: That wasn't a number. Please try again.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Error: Number too large. Please try again.");
        }
    }
}

// Chloe Chan Xin: Advanced feature a) : Bulk processing of unprocessed orders for a current day
void BulkProcessOrders(Dictionary<int, Order> orderDict)
{
    Console.WriteLine();
    Console.WriteLine("Bulk Process Orders");
    Console.WriteLine("===================");

    // Identify pending orders
    List<Order> PendingOrders = new List<Order>();
    foreach (Order order in orderDict.Values)
    {
        if (order.OrderStatus == "Pending")
        {
            PendingOrders.Add(order);
        }
    }
    Console.WriteLine($"Total Pending orders: {PendingOrders.Count}");
    Console.WriteLine();

    int PendingCount = 0;
    int RejectedCount = 0;

    foreach (Order order in PendingOrders)
    {
        TimeSpan TimeRemaining = order.DeliveryDateTime - DateTime.Now;
        if (TimeRemaining.TotalHours < 1)
        {
            order.OrderStatus = "Rejected";
            refundstack.Push(order);
            RejectedCount++;
            Console.WriteLine($"Order {order.OrderId} rejected due to insufficient time for preparation.");
        }
        else
        {
            order.OrderStatus = "Preparing";
            PendingCount++;
            Console.WriteLine($"Order {order.OrderId} is now being prepared.");
        }
    }

    // Calculating statistics
    int processedCount = PendingCount + RejectedCount;
    int totalOrders = orderDict.Count;
    double ProcessedRate = (double)processedCount / totalOrders * 100;

    // Displaying summary statistics
    Console.WriteLine();
    Console.WriteLine("-----Bulk Processing Summary-----");
    Console.WriteLine($"Orders Processed: {processedCount} ");
    Console.WriteLine($"Number of pending orders: {PendingCount}");
    Console.WriteLine($"Number of rejected orders: {RejectedCount}");
    Console.WriteLine($"Processed Rate: {ProcessedRate.ToString("0.00")}%");
}

// Chloe Chan Xin: Bonuse feature: Create an order with a special offer (order total to be re-calculated due to discount)
void loadSpecialOffer()
{
    using (StreamReader sr = new StreamReader("specialoffers.csv"))
    {
        string? s = sr.ReadLine(); // Skip header line
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(',');
            string restaurantName = parts[0];
            string offerId = parts[1];
            string offerDesc = parts[2];
            double discountAmt;
            string data = parts[3];
            if (data == "-")
            {
                discountAmt = 0.0;
            }
            else
            {
                discountAmt = Convert.ToDouble(parts[3]);
            }

            SpecialOffer so = new SpecialOffer(offerId, offerDesc, discountAmt);
            specialOfferList.Add(so);
            // Find restaurant
            foreach (Restaurant r in RestaurantDict.Values)
            {
                if (r.RestaurantName == restaurantName)
                {
                    r.SpecialOffers.Add(so);
                }

            }
        }
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
            try
            {
                string[] parts = s.Split(",");
                if (parts.Length < 4)
                {
                    Console.WriteLine("Invalid line format! Skipping...");
                    continue;
                }
                string restaurantid = parts[0];
                string itemname = parts[1];
                string itemdesc = parts[2];
                double itemprice = Convert.ToDouble(parts[3]);
                string customise = "N/A";

                FoodItem fooditem = new FoodItem(itemname, itemdesc, itemprice, customise);
                fooditemlist.Add(fooditem);
                counter++;

                Restaurant r = SearchRestaurant(RestaurantDict, restaurantid);
                
                // Check restaurant exists
                if (!RestaurantDict.ContainsKey(restaurantid))
                {
                    Console.WriteLine($"Restaurant ID {restaurantid} not found. Skipping item {itemname}");
                }

                // Check if restaurant has a '001' menu
                if (!r.Menus.ContainsKey("001"))
                {
                    r.AddMenu(new Menu("001", "Main Menu"));
                }
                r.Menus["001"].AddFoodItem(fooditem);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid price format in line: {s}");
            }
            catch (Exception ex)
            {
                // Catch any unexpected error
                Console.WriteLine($"Error Processing Message: {s}");
                Console.WriteLine($"Details: " + ex.Message);
            }
        }
    }
    Console.WriteLine($"{counter} food items loaded!");
}

Restaurant SearchRestaurant(Dictionary<string, Restaurant> RestaurantDict, string restaurantid)
{
    if (RestaurantDict.ContainsKey(restaurantid))
    {
        return RestaurantDict[restaurantid];
    }
    return null;
}

void LoadRestaurant(Dictionary<string,Restaurant> RestaurantDict)
{
    int counter = 0;
    using (StreamReader sr = new StreamReader("restaurants.csv"))
    {
        string? s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            try
            {
                string[] parts = s.Split(",");
                // basic check 
                if (parts.Length < 3)
                {
                    Console.WriteLine("Invalid line format! Skipping...");
                    continue;
                }
                string id = parts[0];
                string name = parts[1];
                string email = parts[2];

                // check for duplicate restaurant ID's
                if (RestaurantDict.ContainsKey(id))
                {
                    Console.WriteLine($"Restaurant ID {id} not found. Skipping...");
                }

                Restaurant restaurant = new Restaurant(id, name, email);
                RestaurantDict.Add(id, restaurant);
                counter++;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid price format in line: {s}");
            }
            catch (Exception ex)
            {
                // Catch any unexpected error
                Console.WriteLine($"Error Processing Message: {s}");
                Console.WriteLine($"Details: " + ex.Message);
            }
        }
    }
    Console.WriteLine($"{counter} restaurants loaded!");
}

// 4) List all orders with basic information
void DisplayAllOrders(Dictionary<int,Order> orderDict)
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Date/Time",-20} {"Amount",-10} {"Status",-10}");
    Console.WriteLine($"{"--------",-10} {"----------",-15} {"-------------",-15} {"------------------",-20} {"------",-10} {"---------",-10}");
    foreach (Order order in orderDict.Values)
    {
        try
        {
            // check in case Customer or Restaurant is null
            string customerName = "Unknown";
            if (order.Customer != null)
            {
                customerName = order.Customer.CustomerName;
            }
            string restaurantName = "Unknown";
            if (order.Restaurant != null)
            {
                restaurantName = order.Restaurant.RestaurantName;
            }
            Console.WriteLine($"{order.OrderId,-10} {order.Customer.CustomerName,-15} {order.Restaurant.RestaurantName,-15} {order.DeliveryDateTime,-20} {order.OrderTotal,-10:F2} {order.OrderStatus,-10}");
        }
        catch (FormatException)
        {
            Console.WriteLine($"Invalid format while displaying order {order.OrderId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying order {order.OrderId}: {ex.Message}");
        }
    }
}

// 6) Process an order
void ProcessAnOrder(Dictionary<int,Order> orderDict)
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");
    try
    {
        Console.Write("Enter Restaurant ID: ");
        string resID = Console.ReadLine();

        Restaurant r = SearchRestaurant(RestaurantDict, resID);

        if (r.OrderQueue.Count == 0)
        {
            Console.WriteLine("No orders in the queue.");
            return;
        }

        Order currentOrder = r.OrderQueue.Peek();
        string CustName = "Unknown";
        if (currentOrder.Customer != null)
        {
            CustName = currentOrder.Customer.CustomerName;
        }
        Console.WriteLine($"Order {currentOrder.OrderId}");

        Console.WriteLine($"Customer: {CustName}");

        Console.WriteLine("Ordered Items: ");

        currentOrder.DisplayOrderedFoodItems();

        Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string choice = Console.ReadLine().ToLower();

        bool RemoveFromQueue = false;

        if (currentOrder.OrderStatus == "Pending")
        {
            if (choice == "c")
            {
                currentOrder.OrderStatus = "Preparing";
                Console.WriteLine($"Order {currentOrder.OrderId} confirmed. Status: {currentOrder.OrderStatus}");
            }
            else if (choice == "r")
            {
                currentOrder.OrderStatus = "Rejected";
                refundstack.Push(currentOrder);
                Console.WriteLine($"Order {currentOrder.OrderId} rejected. Added to refund stack.");
                RemoveFromQueue = true;

            }
            else
            {
                Console.WriteLine("Action cannot be done. Pending orders can only be Confirmed or Rejected.");
            }
        }

        else if (currentOrder.OrderStatus == "Preparing")
        {
            if (choice == "d")
            {
                currentOrder.OrderStatus = "Delivered";
                Console.WriteLine($"Order {currentOrder.OrderId} delivered.");
                RemoveFromQueue = true;
            }
            else
            {
                Console.WriteLine("Action cannot be done. Preparing orders can only be Delivered.");
            }
        }

        else if (currentOrder.OrderStatus == "Delivered")
        {
            Console.WriteLine("Action cannot be done. Order is already delivered.");
        }

        else if (currentOrder.OrderStatus == "Cancelled")
        {
            if (choice == "s")
            {
                Console.WriteLine($"Order {currentOrder.OrderId} skipped (Cancelled)");
                RemoveFromQueue = true;
            }
            else
            {
                Console.WriteLine("Action cannot be done. Cancelled orders can only be skipped.");
            }
        }

        if (RemoveFromQueue)
        {
            r.OrderQueue.Dequeue();
        }
    }
    catch(FormatException)
    {
        Console.WriteLine("Invalid input! Please enter the correct values.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
    Console.WriteLine();
}

// 8) Delete an exsiting order
void Deleteexistingorder(Dictionary<int, Order> orderDict, Stack<Order> refundstack)
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");
    try
    {
        // Prompt to enter customer email 
        Console.Write("Enter Customer Email: ");
        string CustEmail = Console.ReadLine();

        // Find the customer
        Customer customer = SearchCustomer(custDict, CustEmail);

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        // Display pending order
        Console.WriteLine("\nPending Orders: ");
        int PendingCount = 0;

        foreach (Order order in customer.OrderList.Values)
        {
            if (order.OrderStatus == "Pending")
            {
                Console.WriteLine(order.OrderId);
                PendingCount++;
            }
        }

        if (PendingCount == 0)
        {
            Console.WriteLine("No pending orders found.");
            return;
        }

        // Prompt for OrderID
        Console.Write("Enter Order ID: ");
        int orderId = Convert.ToInt32(Console.ReadLine());

        if (!customer.OrderList.ContainsKey(orderId))
        {
            Console.WriteLine("Order ID not found.");
            return;
        }

        Order selectedOrder = customer.OrderList[orderId];

        // Display Order Information
        Console.WriteLine($"\nCustomer: {customer.CustomerName}");
        selectedOrder.DisplayOrderedFoodItems();

        // Confirm deletion
        Console.Write("\nConfirm deletion? [Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();

        if (confirm == "Y")
        {
            // Cancel Order and push to refund stack
            selectedOrder.OrderStatus = "Cancelled";
            refundstack.Push(selectedOrder);

            Console.WriteLine($"Order {selectedOrder.OrderId} cancelled. Refund of {selectedOrder.OrderTotal} processed.");
        }
        else if (confirm == "N")
        {
            Console.WriteLine("Order deletion cancelled.");
        }
    }
    catch(FormatException)
    {
        Console.WriteLine("Invalid input! Please enter numbers where required");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
    Console.WriteLine();
}


// Ooi Yu Wen Advanced Feature: Display total order amount 
void DisplayTotalOrderAmt(Dictionary<string, Restaurant> RestaurantDict, Stack<Order> refundstack)
{
    double totalorder = 0;
    double totalrefund = 0;
    try
    {
        foreach (var kvp in RestaurantDict)
        {
            Restaurant r = kvp.Value;
            // check if got null
            if (r == null)
            {
                Console.WriteLine("Skipping null restaurant entry.");
                continue;
            }
            double restaurantTotalOrder = 0;
            double restaurantTotalRefund = 0;

            // delivered orders
            foreach (Order o in r.OrderQueue)
            {
                if (o.OrderStatus == "Delivered")
                {
                    double deliveryfee = 5.0;
                    restaurantTotalOrder += (o.OrderTotal - deliveryfee);
                }
            }
            // Refunds: either check refundstack or orders with status "Rejected"
            foreach (Order o in refundstack)
            {
                if (o.Restaurant.RestaurantId == r.RestaurantId)
                {
                    restaurantTotalRefund += o.OrderTotal;
                }
            }

            Console.WriteLine($"\nRestaurant {r.RestaurantId}: ");
            Console.WriteLine($"Total Orders (Delivered): {restaurantTotalOrder:F2}");
            Console.WriteLine($"Total Refunds: {restaurantTotalRefund:F2}");

            totalorder += restaurantTotalOrder;
            totalrefund += restaurantTotalRefund;
        }

        double finalamt = totalorder - totalrefund;
        Console.WriteLine("=====================================");
        Console.WriteLine($"Overal Total Orders: {totalorder:F2}");
        Console.WriteLine($"Overal Total Refunds: {totalrefund:F2}");
        Console.WriteLine($"Final Amount Gruberoo Earns: {finalamt:F2}");
    }
    catch(FormatException)
    {
        Console.WriteLine("Invalid data format detected while calculating totals");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error calculating totals: " + ex.Message);
    }
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
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Bulk process orders");
    Console.WriteLine("8. Display total order amount");
    Console.WriteLine("0. Exit");
}
