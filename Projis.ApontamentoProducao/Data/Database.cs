using MySql.Data.MySqlClient;

namespace Projis.ApontamentoProducao.Data
{
    public class Database
    {
        private readonly IConfiguration _configuration;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }
    }
}