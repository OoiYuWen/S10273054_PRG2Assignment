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
    class Order // Done By Chan Xin Chloe
    {
        // Attributes
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }
        public Restaurant Restaurant { get; set; }  // Bidrectional Association
        public Customer Customer { get; set; }      // Bidrectional Association
        public List<OrderedFoodItem> OrderedList { get; set; } = new List<OrderedFoodItem>();   // OrderedList

        // Constructor
        public Order() { }
        public Order(int orderId, DateTime orderDateTime, double orderTotal, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid, Restaurant restaurant, Customer customer)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = orderPaymentMethod;
            OrderPaid = orderPaid;
            Restaurant = restaurant;
            Customer = customer;
        }

        // Methods
        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (OrderedFoodItem item in OrderedList)
            {
                total += item.SubTotal;
            }
            return total;
        }
        public void AddOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            OrderedList.Add(orderedFoodItem);
        }
        public bool RemoveOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            if (OrderedList.Remove(orderedFoodItem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine("Ordered Items");
            int count = 0;
            foreach (OrderedFoodItem item in OrderedList)
            {
                count++;
                Console.WriteLine($"{count}. {item.ItemName} - {item.QtyOrdered}");    
            }
            Console.WriteLine($"Delivery date/time: {DeliveryDateTime}");
            Console.WriteLine($"Total Amount: ${OrderTotal:F2}");
            Console.WriteLine($"Order Status: {OrderStatus}");
        }
        public override string ToString()
        {
            return "Order ID: " + OrderId + "\nOrder Date and Time: " + OrderDateTime + "\nOrder Total: " + OrderTotal + "\nOrder Status: " + OrderStatus + "\nDelivery Date and Time: " + DeliveryDateTime + "\nDelivery Address: " + DeliveryAddress + "\nOrder Payment Method: " + OrderPaymentMethod + "\nOrder Paid: " + OrderPaid;
        }
    }
}
