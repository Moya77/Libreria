using ClientesWebService.DB;
using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace ClientesWebService.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersServiceController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommandRegUpdateCustomer _ICommandRegUpdateCustomer;
        private readonly IQueryGetCustomers _IQueryGetCustomers;
        private readonly IQueryGetCustomerCode _IQueryGetCustomerCode;

        public CustomersServiceController(ICommandRegUpdateCustomer ICommandRegUpdateCustomer,
                                          IQueryGetCustomers IQueryGetCustomers,
                                          IQueryGetCustomerCode iQueryGetCustomerCode)
        {
            _ICommandRegUpdateCustomer = ICommandRegUpdateCustomer;
            _IQueryGetCustomers = IQueryGetCustomers;
            _IQueryGetCustomerCode = iQueryGetCustomerCode;
        }

        // GET Obtener Cliente
        [HttpGet("{Identificacion}")]
        public Customer GetCustomer(string Identificacion)
        {
            Logger.Trace("Ejecucion de obtencion de Cliente (service)");
            try
            {
                return _IQueryGetCustomers.GetCustomer(Identificacion);

            }
            catch (Exception ex)
            {
                Logger.Error("Error al obtener el cliente: " + ex.Message);
                return new Customer();
            }
        }

        [HttpGet]
        public List<Customer> GetCustomers()
        {
            Logger.Trace("Ejecucion de obtencion de Clientes (service)");
            try
            {
                return _IQueryGetCustomers.GetCustomers();

            }
            catch (Exception ex)
            {
                Logger.Error("Error al obtener el cliente: " + ex.Message);
                return new List<Customer>();
            }
        }

        // POST Registrar Cliente
        [HttpPost]
        public Customer RegCustomer([FromBody] Customer customer)
        {
            Logger.Trace("Ejecucion de registro de Cliente (service)");
            try
            {
                _ICommandRegUpdateCustomer.RegUpdateCustomer(customer);
                customer.Customer_code = Int32.Parse(_IQueryGetCustomerCode.GetCustomerCode(customer.ID_Customer).ToString());
                return customer;
            }
            catch (Exception ex)
            {
                Logger.Error("Error al obtener el cliente: " + ex.Message);
                return new Customer();
            }
        }



        // PUT Actualizar Cliente
        [HttpPut]
        public void UpdateCustomer([FromBody] Customer customer)
        {
            Logger.Trace("Ejecucion de actualizacion de Cliente (service)");

            try
            {
                _ICommandRegUpdateCustomer.RegUpdateCustomer(customer);

            }
            catch (Exception ex)
            {
                Logger.Error("Error al obtener el cliente: " + ex.Message);
            }
        }


    }
}
