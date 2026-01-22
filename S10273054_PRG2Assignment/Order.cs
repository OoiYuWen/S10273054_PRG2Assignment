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
    class Order
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
        public List<OrderedFoodItem> OrderList { get; set; } = new List<OrderedFoodItem>();   // OrderedList

        // Constructor
        public Order() { }
        public Order(int orderId, DateTime orderDateTime, double orderTotal, string orderStatus, DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            OrderTotal = orderTotal;
            OrderStatus = orderStatus;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = orderPaymentMethod;
            OrderPaid = orderPaid;
        }

        // Methods
        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (OrderedFoodItem item in OrderList)
            {
                total += item.SubTotal;
            }
            return total;
        }
        public void AddOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            OrderList.Add(orderedFoodItem);
        }
        public bool RemoveOrderedFoodItem(OrderedFoodItem orderedFoodItem)
        {
            if (OrderList.Remove(orderedFoodItem))
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
            foreach (OrderedFoodItem item in OrderList)
            {
                Console.WriteLine("Quantity ordered: " + item.QtyOrdered + "\tSubtotal: " + item.SubTotal);
            }
        }
        public override string ToString()
        {
            return "Order ID: " + OrderId + "\nOrder Date and Time: " + OrderDateTime + "\nOrder Total: " + OrderTotal + "\nOrder Status: " + OrderStatus + "\nDelivery Date and Time: " + DeliveryDateTime + "\nDelivery Address: " + DeliveryAddress + "\nOrder Payment Method: " + OrderPaymentMethod + "\nOrder Paid: " + OrderPaid;
        }
    }
}
