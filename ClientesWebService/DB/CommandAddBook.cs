using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{

    public interface ICommandAddBook
    {
        public bool AddBook(string name_book, string company_name);
    }

    public class CommandAddBook : ICommandAddBook
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion
                                                        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public CommandAddBook(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public bool AddBook(string name_book, string company_name)
        {
            Logger.Trace("Ejecucion del comando registro libros en la base de datos");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection conection = new SqlConnection(_IConfiguration.GetConnectionString("Connection")))
                {
                    conection.Open();// abriendo la conexion
                    using (SqlCommand cmd = new("AddBook", conection))// definiendo el comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando que es procedimiento almacenado
                                                                                  //definicion de parametros del procedimiento almacenado
                        cmd.Parameters.AddWithValue("@name_book", name_book);
                        cmd.Parameters.AddWithValue("@company_name", company_name);

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
