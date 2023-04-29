using ClientesWebService.Models;

namespace Books.Models
{
    public class ListaLibrosUsuarios
    {
        public Customer Customer { get; set; } = new Customer();

        public List<Stock> Books { get; set; } = new List<Stock>();

        public ListaLibrosUsuarios() { }

    }
}
