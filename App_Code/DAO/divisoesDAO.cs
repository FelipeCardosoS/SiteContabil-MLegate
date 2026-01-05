using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class divisoesDAO
{
    private WSTimesheet.Service ws = new WSTimesheet.Service();
    private Conexao _conn;

    public divisoesDAO(Conexao c)
    {
        _conn = c;
    }

    public DataTable dePara(int codigoTs)
    {
        string sql = "select * from cad_divisoes where cod_divisao=" + codigoTs + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "job");
    }

    public void insert(string descricao, int cod_referencia, bool sincroniza)
    {
        string sql = "INSERT INTO CAD_DIVISOES(DESCRICAO,COD_REFERENCIA,COD_EMPRESA,SINCRONIZA)";
        sql += "VALUES";
        sql += "('" + descricao.Replace("'", "''") + "'," + cod_referencia + "," + HttpContext.Current.Session["empresa"] + ",'" + sincroniza + "')";

        _conn.execute(sql);
    }

    public void update(int cod_divisao, string descricao, bool sincroniza)
    {
        string sql = "UPDATE CAD_DIVISOES SET DESCRICAO='" + descricao.Replace("'", "''") + "', SINCRONIZA='" + sincroniza + "' ";
        sql += "WHERE COD_DIVISAO=" + cod_divisao + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete(int cod_divisao)
    {
        string sql = "DELETE FROM CAD_DIVISOES WHERE COD_DIVISAO='" + cod_divisao + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public DataTable load(int codigo)
    {
        string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_divisao=" + codigo + "";
        return _conn.dataTable(sql, "a");
    }

    public List<Hashtable> lista()
    {
        //string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_referencia in(1,2) ORDER BY DESCRICAO";
        string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        return _conn.reader(sql);
    }

    public void lista(ref DataTable tb)
    {
        //string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_referencia in(1,2) ORDER BY DESCRICAO";
        string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "  ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void listaSincronizacao(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_referencia in(1,2) ORDER BY " + colunaOrdenar + "";
        //string sql = "SELECT * FROM CAD_DIVISOES WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY " + colunaOrdenar + "";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_DIVISAO DESC";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS Row, *  ";
        sql += "    FROM CAD_DIVISOES WHERE 1=1 ";

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
        string sql = "select count(COD_DIVISAO) from CAD_DIVISOES WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaTimesheet(ref DataSet ds)
    {
        ds = ws.getCadastroDivisoes();
    }

    public int getCodigoXTimesheet(int codTimesheet)
    {
        string sql = "SELECT COD_DIVISAO FROM CAD_DIVISOES WHERE COD_REFERENCIA=" + codTimesheet + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
