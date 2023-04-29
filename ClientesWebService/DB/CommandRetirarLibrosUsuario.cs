using NLog;
using System.Data.SqlClient;

namespace Books.DB
{

    public interface ICommandRetirarLibrosUsuario
    {
        public bool RetirarLibro(int id_stock, string fechaRetiro);
    }
    public class CommandRetirarLibrosUsuario : ICommandRetirarLibrosUsuario
    {
        private readonly IConfiguration _IConfiguration;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public CommandRetirarLibrosUsuario(IConfiguration IConfiguration)
        {

            _IConfiguration = IConfiguration;
        }
        public bool RetirarLibro(int id_stock, string fechaRetiro)
        {
            try
            {
                Logger.Info("Ejecucion del comando retirar libros");
                using (SqlConnection cn = new SqlConnection(_IConfiguration.GetConnectionString("Connection")))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("RetirarLibrosCliente", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_stock", @id_stock);
                        cmd.Parameters.AddWithValue("@FechaRetiro", fechaRetiro);
                        cmd.ExecuteNonQuery();
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }

    }
}
