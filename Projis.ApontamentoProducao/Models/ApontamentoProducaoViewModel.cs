namespace Projis.ApontamentoProducao.Models
{
    public class ApontamentoProducaoViewModel
    {
        // Cabeçalho (seal)
        public int Filial { get; set; }
        public string CodigoOs { get; set; }      // SEAL_CODIGO
        public string NumOs { get; set; }         // SEAL_NUM_OS
        public DateTime? DtEmissao { get; set; }  // SEAL_DT_EMISSAO
        public string CodPedido { get; set; }     // SEAL_COD_PEDIDO
        public string CodCliente { get; set; }    // SEAL_COD_PARCEIRO
        public string StatusOp { get; set; }      // SEAL_STATUS

        // Item OP (seam)
        public int Item { get; set; }             // SEAM_ITEM
        public int Seq { get; set; }              // SEAM_SEQ
        public string CodProduto { get; set; }    // SEAM_COD_PRODUTO
        public string DescricaoProduto { get; set; } // SEAM_DESCRICAO
        public decimal QuantidadeTotal { get; set; } // SEAM_QTD
        public decimal QuantidadeRestante { get; set; } // SEAM_QTD_REST
        public decimal QuantidadeAprovada { get; set; } // SEAM_QTD_APROVADA
        public string Unidade { get; set; }       // SEAM_COD_UNIDADE
        public string StatusItem { get; set; }    // SEAM_STATUS

        // opcional: campos mais antigos
        public decimal Quantidade => QuantidadeTotal;
    }
}