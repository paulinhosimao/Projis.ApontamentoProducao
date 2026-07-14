using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Projis.ApontamentoProducao.Models;

namespace Projis.ApontamentoProducao.Controllers
{
    public class ApontamentoProducaoController : Controller
    {
        private readonly IConfiguration _config;

        public ApontamentoProducaoController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index(int filial = 17, string codOs = "000010")
        {
            
            var model = BuscarOpDoBanco(filial, codOs);

            if (model == null)
            {
                model = new ApontamentoProducaoViewModel
                {
                    Filial = filial,
                    CodigoOs = codOs,
                    DescricaoProduto = "- OP não encontrada",
                    StatusOp = "NF"
                };
            }

            return View(model);
        }



        private ApontamentoProducaoViewModel? BuscarOpDoBanco(int filial, string codOs)
        {
            try
            {
                var cs = _config.GetConnectionString("DefaultConnection");
                using var conn = new MySqlConnection(cs);
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
             AND m.SEAM_SEQ  = 0
            WHERE s.SEAL_FILIAL = @filial
              AND s.SEAL_CODIGO = @codOs;", conn);

                cmd.Parameters.AddWithValue("@filial", filial);
                cmd.Parameters.AddWithValue("@codOs", codOs);

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    Console.WriteLine("Não encontrou seal+seam para " + codOs);
                    return null;
                }

                // LOG: listar colunas disponíveis
                Console.WriteLine("Colunas retornadas:");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"  {i}: {reader.GetName(i)} ({reader.GetFieldType(i).Name})");
                }

                Console.WriteLine("Encontrou OP " + reader["SEAL_CODIGO"]);

                // Mapeamento só com strings e ints (sem conversão de float ainda)
                var model = new ApontamentoProducaoViewModel
                {
                    Filial = Convert.ToInt32(reader["SEAL_FILIAL"]),
                    CodigoOs = reader["SEAL_CODIGO"] as string,
                    NumOs = reader["SEAL_NUM_OS"] as string,
                    DtEmissao = reader["SEAL_DT_EMISSAO"] is DBNull ? null : (DateTime?)reader["SEAL_DT_EMISSAO"],
                    CodPedido = reader["SEAL_COD_PEDIDO"] as string,
                    CodCliente = reader["SEAL_COD_PARCEIRO"] as string,
                    StatusOp = reader["SEAL_STATUS"] as string,

                    Item = Convert.ToInt32(reader["SEAM_ITEM"]),
                    Seq = Convert.ToInt32(reader["SEAM_SEQ"]),
                    CodProduto = reader["SEAM_COD_PRODUTO"] as string,
                    DescricaoProduto = reader["SEAM_DESCRICAO"] as string,
                    Unidade = reader["SEAM_COD_UNIDADE"] as string,

                    QuantidadeTotal = reader["SEAM_QTD"] is DBNull ? 0m : (decimal)Convert.ToDouble(reader["SEAM_QTD"]),
                    QuantidadeRestante = reader["SEAM_QTD_REST"] is DBNull ? 0m : (decimal)Convert.ToDouble(reader["SEAM_QTD_REST"]),
                    QuantidadeAprovada = reader["SEAM_QTD_APROVADA"] is DBNull ? 0m : (decimal)Convert.ToDouble(reader["SEAM_QTD_APROVADA"]),
                    StatusItem = reader["SEAM_STATUS"] as string
                };

                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro MySQL: " + ex.GetType().Name + " - " + ex.Message);
                throw; // temporariamente, para ver na tela qual é a exceção
            }
        }



        //[HttpPost]
        //public IActionResult IncluirApontamento(int filial, string codOs, int item, int seq, decimal qtd, decimal peso)
        //{
        //    // update seam set SEAM_QTD_APROVADA = SEAM_QTD_APROVADA + qtd, ...
        //}
    }
}
