using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for logDAO
/// </summary>
public class logDAO
{
    private Conexao _conn;

	public logDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(string descricao, int usuario, int empresa, string modulo, double lote)
    {
        string sql = "INSERT INTO CAD_LOG(DESCRICAO,COD_USUARIO,COD_EMPRESA,COD_MODULO,DATAHORA,LOTE)";
        sql += "VALUES";
        sql += "('" + descricao + "'," + usuario + "," + empresa + ",'" + modulo + "','" + DateTime.Now.ToString("yyyyMMdd H:mm:ss") + "',"+lote+")";

        _conn.execute(sql);
    }

    public DataTable list(string modulo)
    {
        string sql = "select * from cad_log where cod_modulo='" + modulo + "' and cod_empresa=" + HttpContext.Current.Session["empresa"] + " order by datahora desc";
        return _conn.dataTable(sql, "logs");
    }

    public DataTable list(string modulo, double lote)
    {
        string sql = "select * from cad_log where cod_modulo='" + modulo + "' and cod_empresa=" + HttpContext.Current.Session["empresa"] + " and lote="+lote + " order by datahora desc";
        return _conn.dataTable(sql, "logs");
    }
}
