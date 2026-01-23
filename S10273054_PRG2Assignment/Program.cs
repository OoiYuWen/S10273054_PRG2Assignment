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



