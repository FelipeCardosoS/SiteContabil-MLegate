using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NFTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NFDataTable GetData(int cod_faturamento_nf)
        {
            string sql = "SELECT * FROM FATURAMENTO_NF WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf;

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NFDataTable tb = new RelatoriosDAO.FATURAMENTO_NFDataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}