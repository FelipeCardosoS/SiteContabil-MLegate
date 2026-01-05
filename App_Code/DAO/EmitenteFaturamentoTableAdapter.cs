using System.Data.SqlClient;
using System.Web;

namespace RelatoriosDAOTableAdapters
{
    public partial class EmitenteFaturamentoTableAdapter
    {
        public RelatoriosDAO.EmitenteFaturamentoDataTable GetData(string Page_IsPostBack, string Emitentes_Selecionados, string Tomadores_Selecionados, string De, string Ate, string Ordenacao)
        {
            if (!string.IsNullOrEmpty(De))
                De = De.Substring(6, 4) + De.Substring(3, 2) + De.Substring(0, 2);

            if (!string.IsNullOrEmpty(Ate))
                Ate = Ate.Substring(6, 4) + Ate.Substring(3, 2) + Ate.Substring(0, 2);

            string sql = "WITH LANCTOS_CONTABEIS (LOTE_PAI, SEQ_LOTE, VALOR, DATA) AS ";
            sql += "(SELECT LOTE_PAI, SEQ_LOTE, SUM(VALOR) AS VALOR, MAX(DATA) AS DATA FROM LANCTOS_CONTAB ";
            sql += "WHERE LOTE_PAI IN (SELECT LOTE FROM FATURAMENTO_NF) AND PENDENTE = 'False' AND SEQ_LOTE = 1 ";
            sql += "GROUP BY LOTE_PAI, SEQ_LOTE) ";
            sql += "SELECT LC.DATA AS DATA_RECEBIMENTO, NF.COD_EMITENTE, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS, ";
            sql += "CE.NOME_RAZAO_SOCIAL AS NOME_RAZAO_SOCIAL_CLIENTE, NF.NOME_RAZAO_SOCIAL_TOMADOR, ";
            sql += "(CASE WHEN CE.NOME_FANTASIA <> '' AND CJ.DESCRICAO <> '' THEN CE.NOME_FANTASIA + ' - ' +  CJ.DESCRICAO ";
            sql += "WHEN CE.NOME_FANTASIA = '' THEN CJ.DESCRICAO WHEN CJ.DESCRICAO = '' THEN CE.NOME_FANTASIA END) AS HISTORICO, ";
            sql += "NF_SJ.VALOR AS VALOR_BRUTO, ISNULL(CR.APRESENTACAO, 'Sem Retenções') AS APRESENTACAO, ";
            sql += "(ISNULL(NF_R.VALOR, 0) / NF.VALOR_SERVICOS) * NF_SJ.VALOR AS VALOR_RETENCOES, LC.VALOR AS VALOR_RECEBIMENTO ";
            sql += "FROM FATURAMENTO_NF NF ";
            sql += "LEFT JOIN FATURAMENTO_NF_RETENCOES NF_R ";
            sql += "ON NF.COD_FATURAMENTO_NF = NF_R.COD_FATURAMENTO_NF ";
            sql += "INNER JOIN FATURAMENTO_NF_SERVICOS_JOBS NF_SJ ";
            sql += "ON NF.COD_FATURAMENTO_NF = NF_SJ.COD_FATURAMENTO_NF ";
            sql += "INNER JOIN CAD_JOBS CJ ";
            sql += "ON NF_SJ.COD_JOB = CJ.COD_JOB ";
            sql += "INNER JOIN CAD_EMPRESAS CE ";
            sql += "ON NF_SJ.COD_CLIENTE = CE.COD_EMPRESA ";
            sql += "LEFT JOIN CAD_RETENCOES CR ";
            sql += "ON NF_R.COD_RETENCAO = CR.COD_RETENCAO ";
            sql += "LEFT JOIN LANCTOS_CONTABEIS LC ";
            sql += "ON LC.LOTE_PAI = NF.LOTE ";
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

            if (Ordenacao == "0")
                sql += "ORDER BY NF.COD_FATURAMENTO_NF DESC";
            else if (Ordenacao == "Parametro_ValorLiquido")
                sql += "ORDER BY VALOR_BRUTO";
            else
                sql += "ORDER BY " + Ordenacao;

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.EmitenteFaturamentoDataTable tb = new RelatoriosDAO.EmitenteFaturamentoDataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}