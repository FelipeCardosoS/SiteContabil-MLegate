using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NF_SERVICOS_JOBSTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NF_SERVICOS_JOBSDataTable GetData(int cod_faturamento_nf)
        {
            string sql = "SELECT FSJ.COD_SERVICO, FSJ.NOME_SERVICO, FSJ.VALOR, CS.COD_SERVICO_PREFEITURA, CS.IMPOSTOS";
            sql += " FROM FATURAMENTO_NF_SERVICOS_JOBS FSJ";
            sql += " INNER JOIN CAD_SERVICOS CS ON FSJ.COD_SERVICO = CS.COD_SERVICO";
            sql += " INNER JOIN FATURAMENTO_NF FNF ON FSJ.COD_FATURAMENTO_NF = FNF.COD_FATURAMENTO_NF";
            sql += " WHERE FSJ.COD_FATURAMENTO_NF = " + cod_faturamento_nf;
            sql += " ORDER BY FSJ.NOME_SERVICO";

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NF_SERVICOS_JOBSDataTable tb = new RelatoriosDAO.FATURAMENTO_NF_SERVICOS_JOBSDataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}