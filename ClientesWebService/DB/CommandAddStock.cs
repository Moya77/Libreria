using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{

    public interface ICommandAddStock
    {
        public bool AddBookToStock(Stock stock);
    }

    public class CommandAddStock : ICommandAddStock
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion
                                                        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public CommandAddStock(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public bool AddBookToStock(Stock stock)
        {
            Logger.Trace("Ejecucion del comando registro stock en la base de datos");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection conection = new SqlConnection(_IConfiguration.GetConnectionString("Connection")))
                {
                    conection.Open();// abriendo la conexion
                    using (SqlCommand cmd = new("AddBookToStock", conection))// definiendo el comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando que es procedimiento almacenado
                                                                                  //definicion de parametros del procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Book_code", stock.Book_code);
                        cmd.Parameters.AddWithValue("@Description", stock.Description);
                        cmd.Parameters.AddWithValue("@price", stock.Price);
                        cmd.Parameters.AddWithValue("@CustomerCode", stock.CustomerCode);
                        cmd.Parameters.AddWithValue("@Incom_date", stock.Incom_date);

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
