using ClientesWebService.Models;

namespace Books.Models
{
    public class IncomingBook
    {
        public Customer customer { get; set; } = new Customer();
        public Book book { get; set; } = new Book();

        public Stock stock { get; set; } = new Stock();

        public string Result { get; set; } = string.Empty;
        public string messaje { get; set; } = string.Empty;
    }
}
