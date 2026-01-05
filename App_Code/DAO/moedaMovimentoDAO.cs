using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for moedaMovimentoDAO
/// </summary>
public class moedaMovimentoDAO
{
	private Conexao _conn;

    public moedaMovimentoDAO(Conexao c)
	{
        _conn = c;
	}

    public DataTable loadMovimento()
    {
        string sql = "select * from CAD_MOEDA_MOVIMENTO";
        return _conn.dataTable(sql, "CAD_MOEDA_MOVIMENTO");
    }
}