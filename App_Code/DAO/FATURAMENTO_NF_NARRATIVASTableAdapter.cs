using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class FATURAMENTO_NF_NARRATIVASTableAdapter
    {
        public RelatoriosDAO.FATURAMENTO_NF_NARRATIVASDataTable GetData(int cod_faturamento_nf)
        {
            string sql = "SELECT * FROM FATURAMENTO_NF_NARRATIVAS WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf;

            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();

            RelatoriosDAO.FATURAMENTO_NF_NARRATIVASDataTable tb = new RelatoriosDAO.FATURAMENTO_NF_NARRATIVASDataTable();
            using (SqlCommand comando = new SqlCommand(sql, Connection))
            {
                Adapter.SelectCommand = comando;
                Adapter.Fill(tb);
                return tb;
            }
        }
    }
}