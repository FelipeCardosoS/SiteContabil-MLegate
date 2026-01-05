using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;

public class fechamentoDAO
{
    private Conexao _conn;

	public fechamentoDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(string periodo)
    {
        string sql = "INSERT INTO CONTROLE_FECHAMENTO(PERIODO,DATA,COD_EMPRESA)";
        sql += "VALUES";
        sql += "('" + periodo + "',convert(varchar(20), getdate(), 112)," + HttpContext.Current.Session["empresa"] + ")";

        _conn.execute(sql);
    }

    public void delete(string periodo)
    {
        string sql = "DELETE FROM CONTROLE_FECHAMENTO WHERE PERIODO='" + periodo + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CONTROLE_FECHAMENTO WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY PERIODO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "PERIODO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " DESC)  ";
        sql += "AS Row, *  ";
        sql += "    FROM CONTROLE_FECHAMENTO WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros()
    {
        string sql = "select count(PERIODO) from CONTROLE_FECHAMENTO WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
