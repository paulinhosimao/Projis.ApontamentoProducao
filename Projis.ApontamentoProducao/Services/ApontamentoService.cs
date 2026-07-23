using MySql.Data.MySqlClient;
using Projis.ApontamentoProducao.Data;
using Projis.ApontamentoProducao.Models;

namespace Projis.ApontamentoProducao.Services
{
    public class ApontamentoService
    {
        private readonly Database _database;

        public ApontamentoService(Database database)
        {
            _database = database;
        }

        public ApontamentoProducaoViewModel? BuscarOp(int filial, string codOs)
        {
            using var conn = _database.GetConnection();

            conn.Open();

            var cmd = new MySqlCommand(@"
            SELECT
                s.SEAL_FILIAL,
                s.SEAL_CODIGO,
                s.SEAL_NUM_OS,
                s.SEAL_DT_EMISSAO,
                s.SEAL_COD_PEDIDO,
                s.SEAL_COD_PARCEIRO,
                s.SEAL_STATUS,
                m.SEAM_ITEM,
                m.SEAM_SEQ,
                m.SEAM_COD_PRODUTO,
                m.SEAM_DESCRICAO,
                m.SEAM_COD_UNIDADE,
                m.SEAM_QTD,
                m.SEAM_QTD_REST,
                m.SEAM_QTD_APROVADA,
                m.SEAM_QTD_REPROVADA,
                m.SEAM_STATUS
            FROM seal s
            JOIN seam m
              ON m.SEAM_FILIAL = s.SEAL_FILIAL
             AND m.SEAM_COD_OS = s.SEAL_CODIGO
             AND m.SEAM_ITEM = 1
             AND m.SEAM_SEQ = 0
            WHERE s.SEAL_FILIAL = @filial
              AND s.SEAL_CODIGO = @codOs", conn);

            cmd.Parameters.AddWithValue("@filial", filial);
            cmd.Parameters.AddWithValue("@codOs", codOs);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new ApontamentoProducaoViewModel
            {
                Filial = Convert.ToInt32(reader["SEAL_FILIAL"]),
                CodigoOs = reader["SEAL_CODIGO"]?.ToString(),
                NumOs = reader["SEAL_NUM_OS"]?.ToString(),
                DtEmissao = reader["SEAL_DT_EMISSAO"] is DBNull ? null : (DateTime?)reader["SEAL_DT_EMISSAO"],
                CodPedido = reader["SEAL_COD_PEDIDO"]?.ToString(),
                CodCliente = reader["SEAL_COD_PARCEIRO"]?.ToString(),
                StatusOp = reader["SEAL_STATUS"]?.ToString(),

                Item = Convert.ToInt32(reader["SEAM_ITEM"]),
                Seq = Convert.ToInt32(reader["SEAM_SEQ"]),
                CodProduto = reader["SEAM_COD_PRODUTO"]?.ToString(),
                DescricaoProduto = reader["SEAM_DESCRICAO"]?.ToString(),
                Unidade = reader["SEAM_COD_UNIDADE"]?.ToString(),

                QuantidadeTotal =
                reader["SEAM_QTD"] is DBNull
                    ? 0m
                    : (decimal)Convert.ToDouble(reader["SEAM_QTD"]),

                            QuantidadeRestante =
                reader["SEAM_QTD_REST"] is DBNull
                    ? 0m
                    : (decimal)Convert.ToDouble(reader["SEAM_QTD_REST"]),

                            QuantidadeAprovada =
                reader["SEAM_QTD_APROVADA"] is DBNull
                    ? 0m
                    : (decimal)Convert.ToDouble(reader["SEAM_QTD_APROVADA"]),

                StatusItem = reader["SEAM_STATUS"]?.ToString()
            };
        }
    }
}