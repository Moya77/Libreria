using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{

    public interface IQueryGetBookByCode
    {
        public Book GetBookByCode(int code);
    }

    public class QueryGetBookByCode : IQueryGetBookByCode
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion

        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public QueryGetBookByCode(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public Book GetBookByCode(int code)
        {
            Logger.Trace("Ejecucion del comando obtencion del libro por codigo");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetBookByCode", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Book_code", code);//configuracion de parametro

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            if (reader.Read())//comprobando y leyendo datos
                            {
                                return new Book
                                {
                                    Codigo = Int32.Parse(reader["LibroID"].ToString()),
                                    Nombre = reader["Nombre"].ToString(),
                                    Empresa = reader["NombreEmpresa"].ToString()
                                };
                            }
                            return new Book();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return new Book();
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return new Book();
            }
        }




    }
}
