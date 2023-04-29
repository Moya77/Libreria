using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{

    public interface IQueryGetBooksUser
    {
        public List<Stock> GetBooksUser(int Id_cliente);
    }

    public class QueryGetBooksUser : IQueryGetBooksUser
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion

        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public QueryGetBooksUser(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public List<Stock> GetBooksUser(int Id_cliente)
        {
            List<Stock> booksUser = new List<Stock>();
            Logger.Trace("Ejecucion del comando obtencion del libros del cliente");
            try
            {

                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetBooksUser", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Id_Cliente", Id_cliente);//configuracion de parametro


                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            while (reader.Read())//comprobando y leyendo datos
                            {

                                booksUser.Add(new Stock
                                {
                                    Id_Stock = Int32.Parse(reader["StockID"].ToString()),
                                    Incom_date = reader["FechaIngreso"].ToString(),
                                    Description = reader["Descripcion"].ToString(),
                                    Price = Double.Parse(reader["Precio"].ToString())

                                });
                            }
                            return booksUser;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Error("Error de ejecucion " + ex);
                return booksUser;
            }
        }




    }
}
