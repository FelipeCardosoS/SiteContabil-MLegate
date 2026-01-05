using System.Data.SqlClient;
using System.Web;

namespace RelatoriosDAOTableAdapters
{
    public partial class TotalValorBrutoRecebimentoTableAdapter
    {
        public RelatoriosDAO.TotalValorBrutoRecebimentoDataTable GetData(string Page_IsPostBack, string Emitentes_Selecionados, string Tomadores_Selecionados, string De_Rec, string Ate_Rec, string De, string Ate)
        {
            if (!string.IsNullOrEmpty(De_Rec))
                De_Rec = De_Rec.Substring(6, 4) + De_Rec.Substring(3, 2) + De_Rec.Substring(0, 2);
            
            if (!string.IsNullOrEmpty(Ate_Rec))
                Ate_Rec = Ate_Rec.Substring(6, 4) + Ate_Rec.Substring(3, 2) + Ate_Rec.Substring(0, 2);

            if (!string.IsNullOrEmpty(De))
                De = De.Substring(6, 4) + De.Substring(3, 2) + De.Substring(0, 2);
            
            if (!string.IsNullOrEmpty(Ate))
                Ate = Ate.Substring(6, 4) + Ate.Substring(3, 2) + Ate.Substring(0, 2);

            string sql = "WITH LANCTOS_CONTABEIS (LOTE_PAI, SEQ_LOTE, VALOR, DATA) AS ";
            sql += "(SELECT LOTE_PAI, SEQ_LOTE, SUM(VALOR) AS VALOR, MAX(DATA) AS DATA FROM LANCTOS_CONTAB ";
            sql += "WHERE LOTE_PAI IN (SELECT LOTE FROM FATURAMENTO_NF) AND PENDENTE = 'False' AND SEQ_LOTE = 1 ";
            sql += "GROUP BY LOTE_PAI, SEQ_LOTE) ";
            sql += "SELECT NF.COD_EMITENTE, SUM(NF.VALOR_SERVICOS) AS TOTAL_VALOR_BRUTO, SUM(LC.VALOR) AS TOTAL_VALOR_RECEBIMENTO ";
            sql += "FROM FATURAMENTO_NF NF, LANCTOS_CONTABEIS LC ";
            sql += "WHERE NF.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND LC.LOTE_PAI = NF.LOTE ";

            if (!string.IsNullOrEmpty(Page_IsPostBack))
                sql += "AND NF.SITUACAO_NF <> 'C' ";
            else
                sql += "AND NF.SITUACAO_NF = 'Nenhum Registro' ";

            if (!string.IsNullOrEmpty(Emitentes_Selecionados))
                sql += "AND NF.COD_EMITENTE IN (" + Emitentes_Selecionados.Substring(2) + ") ";

            if (!string.IsNullOrEmpty(Tomadores_Selecionados))
                sql += "AND NF.COD_TOMADOR IN (" + Tomadores_Selecionados.Substring(2) + ") ";

            if (!string.IsNullOrEmpty(De_Rec) && !string.IsNullOrEmpty(Ate_Rec))
                sql += "AND LC.DATA BETWEEN '" + De_Rec + "' AND '" + Ate_Rec + "' ";
            else if (!string.IsNullOrEmpty(De_Rec))
                sql += "AND LC.DATA >= '" + De_Rec + "' ";
            else if (!string.IsNullOrEmpty(Ate_Rec))
                sql += "AND LC.DATA <= '" + Ate_Rec + "' ";

            if (!string.IsNullOrEmpty(De) && !string.IsNullOrEmpty(Ate))
                sql += "AND NF.DATA_EMISSAO_RPS BETWEEN '" + De + "' AND '" + Ate + "' ";
            else if (!string.IsNullOrEmpty(De))
                sql += "AND NF.DATA_EMISSAO_RPS >= '" + De + "' ";
            else if (!string.IsNullOrEmpty(Ate))
                sql += "AND NF.DATA_EMISSAO_RPS <= '" + Ate + "' ";

            sql += "GROUP BY NF.COD_EMITENTE";

            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.TotalValorBrutoRecebimentoDataTable tb = new RelatoriosDAO.TotalValorBrutoRecebimentoDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
    }
}