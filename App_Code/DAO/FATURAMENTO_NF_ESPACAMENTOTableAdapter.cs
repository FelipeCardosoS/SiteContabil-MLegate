using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NF_ESPACAMENTOTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NF_ESPACAMENTODataTable Espacamento()
        {
            string sql = "SELECT TOP 30 ROW_NUMBER() OVER(ORDER BY COD_CONTA DESC) AS ROW FROM LANCTOS_CONTAB";

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NF_ESPACAMENTODataTable tb = new RelatoriosDAO.FATURAMENTO_NF_ESPACAMENTODataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}