using Books.DB;
using Books.Models;
using ClientesWebService.DB;
using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Services
{

    [Route("api/[controller]")]
    [ApiController]
    public class BookServiceController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommandAddBook _ICommandAddBook;
        private readonly IQueryGetBookCode _IQueryGetBookCode;
        private readonly ICommandAddStock _ICommandAddStock;
        private readonly IQueryGetBookByCode _IQueryGetBookByCode;
        private readonly ICommandRetirarLibrosUsuario _ICommandRetirarLibrosUsuario;
        private readonly IQueryGetBooksUser _IQueryGetBooksUser;
        private readonly IQueryGetCustomers _IQueryGetCustomers;

        public BookServiceController(ICommandAddBook ICommandAddBook,
                                     IQueryGetBookCode iQueryGetBookCode,
                                     ICommandAddStock iCommandAddStock,
                                     IQueryGetBookByCode IQueryGetBookByCode,
                                     ICommandRetirarLibrosUsuario ICommandRetirarLibrosUsuario,
                                     IQueryGetBooksUser IQueryGetBooksUser,
                                     IQueryGetCustomers IQueryGetCustomers)
        {
            _ICommandAddBook = ICommandAddBook;
            _IQueryGetBookCode = iQueryGetBookCode;
            _ICommandAddStock = iCommandAddStock;
            _IQueryGetBookByCode = IQueryGetBookByCode;
            _ICommandRetirarLibrosUsuario = ICommandRetirarLibrosUsuario;
            _IQueryGetBooksUser = IQueryGetBooksUser;
            _IQueryGetCustomers = IQueryGetCustomers;
        }



        [HttpPost]
        public Book SaveBook([FromBody] Book book)
        {
            _ICommandAddBook.AddBook(book.Nombre, book.Empresa);
            book.Codigo = _IQueryGetBookCode.GetBookCode(book.Nombre);
            return book;
        }

        [HttpPost("IncommingBook")]
        public IncomingBook IncommingBook([FromBody] IncomingBook IncommingBook)
        {
            if (_ICommandAddStock.AddBookToStock(IncommingBook.stock))
            {
                IncommingBook.Result = "success";
                IncommingBook.messaje = "La entrada se a registrado de forma exitosa, ¿Desea agregar mas?";
            }
            else
            {
                IncommingBook.Result = "Error";
                IncommingBook.messaje = "Se produjo un error al intentar guardar la entrada";
            }

            return IncommingBook;
        }

        [HttpPost("RetirarLibros")]
        public OutBook RetirarLibros([FromBody] OutBook outbook)
        {
            List<Stock> books = new List<Stock>(outbook.UserBooks);
            foreach (Stock stock in books)
            {
                _ICommandRetirarLibrosUsuario.RetirarLibro(stock.Id_Stock, outbook.FechaSalida);
                outbook.UserBooks.Remove(stock);
            }

            return outbook;
        }

        [HttpPost("GetUserBooks")]
        public OutBook GetUserBooks([FromBody] OutBook outbook)
        {

            outbook.UserBooks = _IQueryGetBooksUser.GetBooksUser(outbook.Id_cliente);

            return outbook;

        }


        [HttpGet("GetBookById/{id}")]
        public Book GetBookById(int id)
        {
            return _IQueryGetBookByCode.GetBookByCode(id);
        }

        [HttpGet("GetListBooksUsers")]
        public Dictionary<string, ListaLibrosUsuarios> GetBookById()
        {
            Dictionary<string, ListaLibrosUsuarios> listaLibrosUsuarios = new();
            ListaLibrosUsuarios ListaCliente;
            foreach (Customer cliente in _IQueryGetCustomers.GetCustomers())
            {
                ListaCliente = new ListaLibrosUsuarios();
                ListaCliente.Customer = cliente;
                ListaCliente.Books = _IQueryGetBooksUser.GetBooksUser(cliente.Customer_code);
                if (ListaCliente.Books.Count > 0)
                {
                    listaLibrosUsuarios.Add(cliente.ID_Customer, ListaCliente);
                }
            }
            return listaLibrosUsuarios;
        }


    }
}
