using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NF_DATA_VENCIMENTOTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NF_DATA_VENCIMENTODataTable GetData(int cod_faturamento_nf)
        {
            string sql = "SELECT * FROM FATURAMENTO_NF_DATA_VENCIMENTO WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf;

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NF_DATA_VENCIMENTODataTable tb = new RelatoriosDAO.FATURAMENTO_NF_DATA_VENCIMENTODataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}