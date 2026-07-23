using MySql.Data.MySqlClient;
using Projis.ApontamentoProducao.Data;

namespace Projis.ApontamentoProducao.Services
{
    public class UsuarioService
    {
        private readonly Database _database;

        public UsuarioService(Database database)
        {
            _database = database;
        }

        public bool ValidarUsuario(string usuario, string senha)
        {
            using var conn = _database.GetConnection();

            conn.Open();

            string sql = @"
                SELECT Id
                FROM sys_user
                WHERE UserName=@usuario
                  AND PassWord=@senha
                  AND Inactive=0
                LIMIT 1";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@senha", senha);

            var resultado = cmd.ExecuteScalar();

            return resultado != null;
        }
    }
}