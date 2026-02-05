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
    class Customer //Done By Chan Xin Chloe
    {
        // Attribute
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public Dictionary<int, Order> OrderList { get; set; } = new Dictionary<int, Order>();

        // Constructors
        public Customer() { }
        public Customer(string emailAddress, string customerName)
        {
            EmailAddress = emailAddress;
            CustomerName = customerName;
        }

        // Methods
        public void AddOrder(Order order)
        {
            OrderList.Add(order.OrderId, order);
        }
        public void DisplayAllOrders()
        {
            foreach (Order order in OrderList.Values)
            {
                Console.WriteLine(order.ToString());
            }
        }
        public bool RemoveOrder(Order order)
        {
            return OrderList.Remove(order.OrderId);
        }
        public override string ToString()
        {
            return "Email Address: " + EmailAddress + "\tCustomer Name: " + CustomerName;
        }
    }
}
