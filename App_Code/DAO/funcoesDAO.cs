using System;
using System.Data;
using System.Configuration;
using System.Web;

public class funcoesDAO
{
    private Conexao _conn;

    public funcoesDAO(Conexao c)
    {
        _conn = c;
    }

    public int insert(string descricao)
    {
        string sql = "INSERT INTO CAD_FUNCOES(DESCRICAO,COD_EMPRESA)VALUES('" + descricao.Replace("'", "''") + "'," + HttpContext.Current.Session["empresa"] + ")";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void update(int cod_funcao, string descricao)
    {
        string sql = "UPDATE CAD_FUNCOES SET DESCRICAO='" + descricao.Replace("'", "''") + "' WHERE COD_FUNCAO='" + cod_funcao + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public void delete(int cod_funcao)
    {
        string sql = "DELETE FROM CAD_FUNCOES WHERE COD_FUNCAO='" + cod_funcao + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public DataTable load(int cod_funcao)
    {
        string sql = "SELECT * FROM CAD_FUNCOES WHERE COD_FUNCAO='" + cod_funcao + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "funcao");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_FUNCOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_FUNCAO DESC";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS Row, *  ";
        sql += "    FROM CAD_FUNCOES WHERE 1=1 ";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string descricao)
    {
        string sql = "select COUNT(COD_FUNCAO) from CAD_FUNCOES WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
