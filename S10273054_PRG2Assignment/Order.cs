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
        public List<Customer> CustomerList { get; set; } = new List<Customer>();

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
    }
}
