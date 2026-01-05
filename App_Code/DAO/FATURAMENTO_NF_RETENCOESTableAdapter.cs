using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NF_RETENCOESTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NF_RETENCOESDataTable GetData(int cod_faturamento_nf)
        {
            string sql = "SELECT * FROM FATURAMENTO_NF_RETENCOES WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf;

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NF_RETENCOESDataTable tb = new RelatoriosDAO.FATURAMENTO_NF_RETENCOESDataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}