namespace ClientesWebService.Models
{
    public class Stock
    {
        public int Id_Stock { get; set; } = 0;
        public int Book_code { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public int CustomerCode { get; set; } = 0;
        public string Incom_date { get; set; } = string.Empty;
    }
}
