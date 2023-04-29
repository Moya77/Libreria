using ClientesWebService.Models;
using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{
    //interfaz de acceso a la recuperacion de productos
    public interface IQueryGetProducts
    {
        public List<Book> GetProducts(string? User_Id = "");
        public Book GetProduct(string Code);
    }
    // implementacion de la interface del comando obtencion de productos
    public class QueryGetProducts : IQueryGetProducts
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion

        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public QueryGetProducts(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public List<Book> GetProducts(string? User_Id = "")
        {
            Logger.Trace("Ejecucion del comando obtencion de productos en la base de datos");
            List<Book> products = new List<Book>();
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetProducts", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Identificacion", User_Id);//configuracion de parametro

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            while (reader.Read())//comprobando y leyendo datos
                            {
                                //agregando los productos a la lista
                                products.Add(

                                    new Book //retornando el modelo customer
                                    {
                                        // llenando cada uno de los atributos
                                        Codigo = Int32.Parse(reader["Codigo"].ToString()),
                                        Nombre = reader["Nombre"].ToString(),
                                        Empresa = reader["Categoria"].ToString()

                                    }

                                    );

                            }
                            return products; //devolviendo la lista
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return products;
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return products;
            }
        }

        // ********************************Obtencion de un solo producto**************************************************//
        public Book GetProduct(string Code)
        {
            Logger.Trace("Ejecucion del comando obtencion de producto en la base de datos");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetProduct", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Codigo", Code);//configuracion de parametro

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            if (reader.Read())//comprobando y leyendo datos
                            {
                                return new Book //retornando el modelo customer
                                {
                                    // llenando cada uno de los atributos
                                    Codigo = Int32.Parse(reader["Codigo"].ToString()),
                                    Nombre = reader["Nombre"].ToString(),
                                    Empresa = reader["Categoria"].ToString()

                                };

                            }
                            return new Book(); // si no hay datos de lectura, devuelve Modelo vacio
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
