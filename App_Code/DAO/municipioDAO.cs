using System.Data;

public class municipioDAO
{
	private Conexao _conn;
	public municipioDAO(Conexao conn)
	{
		_conn = conn;
	}

	public DataTable lista()
	{
		string sql = "select ID_MUNICIPIO, COD_MUNICIPIO, NOME_MUNICIPIO, COD_MUNICIPIO + ' - ' + NOME_MUNICIPIO as DESC_MUNICIPIO from CAD_MUNICIPIO order by DESC_MUNICIPIO";

		return _conn.dataTable(sql, "municipio");
	}
	public DataTable lista_externo()
	{
		string sql = "select ID_MUNICIPIO, COD_MUNICIPIO, NOME_MUNICIPIO, COD_MUNICIPIO + ' - ' + NOME_MUNICIPIO as DESC_MUNICIPIO from CAD_MUNICIPIO where COD_MUNICIPIO = (select max(COD_MUNICIPIO) from CAD_MUNICIPIO)";

		return _conn.dataTable(sql, "municipio_ext");
	}

}