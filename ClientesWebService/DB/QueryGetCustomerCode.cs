using NLog;
using System.Data.SqlClient;

namespace ClientesWebService.DB
{

    public interface IQueryGetCustomerCode
    {
        public int GetCustomerCode(string Identificacion);
    }

    public class QueryGetCustomerCode : IQueryGetCustomerCode
    {
        private readonly IConfiguration _IConfiguration;//Acceso al appsettings con la cadena de conexion

        // Objeto logger para traceo y monitoreo de errores
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //inyeccion de la dependencia IConfiguration (se debe de definir el servicio en el Programs.cs)
        public QueryGetCustomerCode(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public int GetCustomerCode(string Identificacion)
        {
            Logger.Trace("Ejecucion del comando obtencion del codigo");
            try
            {
                //creacion de la conexion a la base de datos
                using (SqlConnection connection = new(_IConfiguration.GetConnectionString("Connection")))
                {
                    connection.Open();//abrir conexion
                    using (SqlCommand cmd = new("GetClientCode", connection))//definicion del comando
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;//indicando procedimiento almacenado
                        cmd.Parameters.AddWithValue("@Identificacion", Identificacion);//configuracion de parametro

                        using (SqlDataReader reader = cmd.ExecuteReader())//ejecutando la lectura
                        {
                            if (reader.Read())//comprobando y leyendo datos
                            {
                                return reader.GetInt32(0);
                            }
                            return 0;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.Error("Error en el comando SQL " + ex);
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error("Error de codigo " + ex);
                return 0;
            }
        }




    }
}
