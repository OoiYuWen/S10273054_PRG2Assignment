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
    class SpecialOffer //Done By Ooi Yu Wen 
    {
        public string OfferCode { get; set; }
        public string OfferDesc { get; set; }
        public double Discount { get; set; }
        public List<Order> SpecialOrderList { get; set; } = new List<Order>();

        public SpecialOffer() { }
        public SpecialOffer(string offerCode,string offerDesc,double discount)
        {
            OfferCode = offerCode;
            OfferDesc = offerDesc;
            Discount = discount;
        }
        public override string ToString()
        {
            return $"OfferCode: {OfferCode}, OfferDesc: {OfferDesc}, Discount: {Discount*100}";
        }
    }
}
