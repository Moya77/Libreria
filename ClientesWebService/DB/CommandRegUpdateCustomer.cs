using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{
    // Interface de acceso a el comando registro de cliente
    public interface ICommandRegUpdateCustomer
    {
        public bool RegUpdateCustomer(Customer customer);
    }
    // implementacion de la interface del comando registro de clientes
    public class CommandRegUpdateCustomer : ICommandRegUpdateCustomer
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion
        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public CommandRegUpdateCustomer(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public bool RegUpdateCustomer(Customer customer)
        {
            Logger.Trace("Ejecucion del comando insercion de cliente en la base de datos");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection conection = new SqlConnection(_IConfiguration.GetConnectionString("Connection")))
                {
                    conection.Open();// abriendo la conexion
                    using (SqlCommand cmd = new("RegUpdateCustomer", conection))// definiendo el comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando que es procedimiento almacenado
                        //definicion de parametros del procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Identificacion", customer.ID_Customer);
                        cmd.Parameters.AddWithValue("@Nombre", customer.Full_name);
                        cmd.Parameters.AddWithValue("@Fecha_nacimiento", customer.Born_date);
                        cmd.ExecuteNonQuery();// ejecucion del procedimiento almacenado
                        conection.Close();//cierre de conexion
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return false;
            }
        }
    }
}
