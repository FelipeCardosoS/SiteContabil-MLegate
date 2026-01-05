using System.Data;

public class paisDAO
{
	private Conexao _conn;
	public paisDAO(Conexao conn)
	{
		_conn = conn;
	}

	public DataTable lista()
	{
		string sql = "SELECT ID_PAIS, COD_PAIS, NOME_PAIS, COD_PAIS + ' - ' + NOME_PAIS as DESC_PAIS from CAD_PAIS order by DESC_PAIS";

		return _conn.dataTable(sql, "pais");
	}
}