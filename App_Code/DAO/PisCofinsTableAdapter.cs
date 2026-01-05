using System;
using System.Web;
using System.Data.SqlClient;
/// <summary>
/// Summary description for PisCofinsTableAdapter
/// </summary>
/// 
namespace RelatoriosDAOTableAdapters
{
    public partial class PisCofinsTableAdapter
    {
        public RelatoriosDAO.PisCofinsDataTable executar(string COD_CONTA, DateTime De, DateTime Ate)
        {
            string sql = "SELECT DISTINCT Cont.DATA, NF.NUMERO_NOTA, Empresas.NOME_RAZAO_SOCIAL, Empresas.CNPJ_CPF, " +
                         "NF.ENTRADA_SAIDA, NF.VALOR, NF_Itens.VALOR_BASE_IMP, " +
                         "NF_Itens.ALIQ_PIS, NF_Itens.VALOR_PIS, NF_Itens.ALIQ_COFINS, NF_Itens.VALOR_COFINS, " +
                         "NF.VALOR_ISS, " +
                         "NF_Itens.ALIQ_IR, NF_Itens.VALOR_IR, NF_Itens.ALIQ_CSLL, NF_Itens.VALOR_CSLL, " +
                         "Emissor.NOME_RAZAO_SOCIAL AS Empresa_Logada " +
                         "FROM [CAD_NOTAS_FISCAIS] NF, [ITENS_NOTA_FISCAL] NF_Itens, [LANCTOS_CONTAB] Cont, " +
                         "[CAD_EMPRESAS] Empresas, [CAD_EMPRESAS] Emissor " +
                         "WHERE NF.LOTE = NF_Itens.LOTE " +
                         "AND NF.LOTE = Cont.LOTE " +
                         "AND NF_Itens.LOTE = Cont.LOTE " +
                         "AND NF.COD_EMPRESA = NF_Itens.COD_EMPRESA " +
                         "AND NF.COD_EMPRESA = Cont.COD_EMPRESA " +
                         "AND NF_Itens.COD_EMPRESA = Cont.COD_EMPRESA " +
                         "AND Cont.COD_TERCEIRO = Empresas.COD_EMPRESA " +
                         "AND Cont.COD_EMPRESA = Emissor.COD_EMPRESA " +
                         "AND NF.NUMERO_NOTA = NF_Itens.NUMERO_NOTA " +
                         "AND Cont.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " " +
                         "AND Cont.COD_CONTA = '" + COD_CONTA + "' " +
                         "AND Cont.DATA BETWEEN '" + De.ToString("yyyyMMdd") + "' AND '" + Ate.ToString("yyyyMMdd") + "' " +
                         "ORDER BY Cont.DATA, NF.NUMERO_NOTA";

            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.PisCofinsDataTable tb = new RelatoriosDAO.PisCofinsDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
    }
}