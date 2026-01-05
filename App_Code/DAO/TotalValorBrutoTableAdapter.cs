using System.Data.SqlClient;
using System.Web;

namespace RelatoriosDAOTableAdapters
{
    public partial class TotalValorBrutoTableAdapter
    {
        public RelatoriosDAO.TotalValorBrutoDataTable GetData(string Page_IsPostBack, string Emitentes_Selecionados, string Tomadores_Selecionados, string De, string Ate)
        {
            if (!string.IsNullOrEmpty(De))
                De = De.Substring(6, 4) + De.Substring(3, 2) + De.Substring(0, 2);
            
            if (!string.IsNullOrEmpty(Ate))
                Ate = Ate.Substring(6, 4) + Ate.Substring(3, 2) + Ate.Substring(0, 2);

            string sql = "SELECT NF.COD_EMITENTE, SUM(NF.VALOR_SERVICOS) AS TOTAL_VALOR_BRUTO ";
            sql += "FROM FATURAMENTO_NF NF ";
            sql += "WHERE NF.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";

            if (!string.IsNullOrEmpty(Page_IsPostBack))
                sql += "AND NF.SITUACAO_NF <> 'C' ";
            else
                sql += "AND NF.SITUACAO_NF = 'Nenhum Registro' ";

            if (!string.IsNullOrEmpty(Emitentes_Selecionados))
                sql += "AND NF.COD_EMITENTE IN (" + Emitentes_Selecionados.Substring(2) + ") ";

            if (!string.IsNullOrEmpty(Tomadores_Selecionados))
                sql += "AND NF.COD_TOMADOR IN (" + Tomadores_Selecionados.Substring(2) + ") ";

            if (!string.IsNullOrEmpty(De) && !string.IsNullOrEmpty(Ate))
                sql += "AND NF.DATA_EMISSAO_RPS BETWEEN '" + De + "' AND '" + Ate + "' ";
            else if (!string.IsNullOrEmpty(De))
                sql += "AND NF.DATA_EMISSAO_RPS >= '" + De + "' ";
            else if (!string.IsNullOrEmpty(Ate))
                sql += "AND NF.DATA_EMISSAO_RPS <= '" + Ate + "' ";

            sql += "GROUP BY NF.COD_EMITENTE";

            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.TotalValorBrutoDataTable tb = new RelatoriosDAO.TotalValorBrutoDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
    }
}