using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{
    //interfaz de acceso a la recuperacion de usuarios o por identificacion
    public interface IQueryGetCustomers
    {
        public Customer GetCustomer(string customerId);
        public List<Customer> GetCustomers();
    }
    // implementacion de la interface del comando obtencion de cliente
    public class QueryGetCustomers : IQueryGetCustomers
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion

        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public QueryGetCustomers(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public Customer GetCustomer(string customerId)
        {
            Logger.Trace("Ejecucion del comando obtencion de cliente en la base de datos");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetCustomer", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@CustomerCode", customerId);//configuracion de parametro

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            if (reader.Read())//comprobando y leyendo datos
                            {
                                return new Customer //retornando el modelo customer
                                {
                                    // llenando cada uno de los atributos
                                    ID_Customer = reader["NumeroIdentificacion"].ToString(),
                                    Full_name = reader["NombreCompleto"].ToString(),
                                    Born_date = reader["FechaNacimiento"].ToString(),

                                };
                            }
                            return new Customer(); // si no hay datos de lectura, devuelve Modelo vacio
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return new Customer();
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return new Customer();
            }
        }


        public List<Customer> GetCustomers()
        {
            Logger.Trace("Ejecucion del comando obtencion de productos en la base de datos");
            List<Customer> customers = new List<Customer>();
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetCustomers", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            while (reader.Read())//comprobando y leyendo datos
                            {
                                //agregando los productos a la lista
                                customers.Add(

                                    new Customer //creando el modelo customer
                                    {
                                        // llenando cada uno de los atributos
                                        Customer_code = Int32.Parse(reader["ClienteID"].ToString()),
                                        Full_name = reader["NombreCompleto"].ToString(),
                                        ID_Customer = reader["NumeroIdentificacion"].ToString(),
                                        Born_date = reader["FechaNacimiento"].ToString()

                                    }

                                    );

                            }
                            return customers; //devolviendo la lista
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return customers;
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return customers;
            }
        }

    }
}
