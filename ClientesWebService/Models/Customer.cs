namespace ClientesWebService.Models
{
    public class Customer
    {
        public int Customer_code { get; set; }
        public string ID_Customer { get; set; }
        public string Full_name { get; set; }
        public string Born_date { get; set; }


        public Customer()
        {
            Customer_code = 0;
            ID_Customer = string.Empty; ;
            Full_name = string.Empty; ;
            Born_date = string.Empty;

        }
    }
}
