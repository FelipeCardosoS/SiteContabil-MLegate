using System;
using System.Data;
using System.Configuration;
using System.Web;

public class gruposContabeisDAO
{
    private Conexao _conn;

	public gruposContabeisDAO(Conexao c)
	{
        _conn = c;
	}

    public int insert(string descricao, string cod_conta, string regraExibicao)
    {
        string sql = "INSERT INTO CAD_GRUPOS_CONTABEIS(DESCRICAO,COD_CONTA,COD_EMPRESA, REGRA_EXIBICAO)";
        sql += "VALUES";
        sql += "('" + descricao + "','" + cod_conta + "'," + HttpContext.Current.Session["empresa"] + ",'"+regraExibicao+"');";
        sql += "SELECT SCOPE_IDENTITY();";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void update(int cod_grupo_contabil, string descricao, string cod_conta, string regraExibicao)
    {
        string sql = "UPDATE CAD_GRUPOS_CONTABEIS SET DESCRICAO='" + descricao + "',COD_CONTA = '" + cod_conta + "', REGRA_EXIBICAO='"+regraExibicao+"'";
        sql += " WHERE COD_GRUPO_CONTABIL="+cod_grupo_contabil+" AND COD_EMPRESA="+HttpContext.Current.Session["empresa"]+"";

        _conn.execute(sql);
    }

    public void delete(int cod_grupo_contabil)
    {
        string sql = "DELETE CAD_GRUPOS_CONTABEIS ";
        sql += " WHERE COD_GRUPO_CONTABIL=" + cod_grupo_contabil + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public DataTable load(int cod_grupo_contabil)
    {
        string sql = "SELECT * FROM CAD_GRUPOS_CONTABEIS ";
        sql += " WHERE COD_GRUPO_CONTABIL=" + cod_grupo_contabil + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return _conn.dataTable(sql, "grupo_contabil");
    }

    public int get_codigo(string descricao)
    {
        string sql = "SELECT COD_GRUPO_CONTABIL FROM CAD_GRUPOS_CONTABEIS ";
        sql += " WHERE DESCRICAO='" + descricao + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void lista(ref DataTable tb)
    {
        string sql = "select * from CAD_GRUPOS_CONTABEIS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_GRUPO_CONTABIL";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " DESC)  ";
        sql += "AS Row, *  ";
        sql += "    FROM CAD_GRUPOS_CONTABEIS WHERE 1=1 ";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '" + descricao + "%'";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string descricao)
    {
        string sql = "select COUNT(COD_GRUPO_CONTABIL) from CAD_GRUPOS_CONTABEIS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '" + descricao + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
