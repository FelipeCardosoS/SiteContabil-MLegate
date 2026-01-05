using System;
using System.Data;
using System.Configuration;
using System.Web;

public class tiposContaDAO
{
    private Conexao _conn;

	public tiposContaDAO(Conexao c)
	{
        _conn = c;
	}

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_TIPO_CONTA ORDER BY DESCRICAO";

        _conn.fill(sql, ref tb);
    }
}
