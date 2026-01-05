using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for moedaBalancoDAO
/// </summary>
public class moedaBalancoDAO
{
	private Conexao _conn;

    public moedaBalancoDAO(Conexao c)
	{
        _conn = c;
	}

    public DataTable loadBalanco()
    {
        string sql = "select * from CAD_MOEDA_BALANCO";
        return _conn.dataTable(sql, "CAD_MOEDA_BALANCO");
    }
}