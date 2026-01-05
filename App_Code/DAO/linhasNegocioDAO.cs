using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class linhasNegocioDAO
{
    private WSTimesheet.Service ws = new WSTimesheet.Service();
    private Conexao _conn;

    public linhasNegocioDAO(Conexao c)
    {
        _conn = c;
    }

    public DataTable dePara(int codigoTs)
    {
        string sql = "select * from cad_linha_negocios where cod_linha_negocio=" + codigoTs + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "linhaNegocio");
    }

    public void insert(string descricao, int cod_referencia)
    {
        string sql = "INSERT INTO CAD_LINHA_NEGOCIOS(DESCRICAO,COD_REFERENCIA,COD_EMPRESA)";
        sql += "VALUES";
        sql += "('" + descricao.Replace("'", "''") + "'," + cod_referencia + "," + HttpContext.Current.Session["empresa"] + ")";

        _conn.execute(sql);
    }

    public void update(int cod_linha_negocio, string descricao)
    {
        string sql = "UPDATE CAD_LINHA_NEGOCIOS SET DESCRICAO='" + descricao.Replace("'", "''") + "' ";
        sql += "WHERE COD_LINHA_NEGOCIO=" + cod_linha_negocio + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete(int cod_linha_negocio)
    {
        string sql = "DELETE FROM CAD_LINHA_NEGOCIOS WHERE COD_LINHA_NEGOCIO='" + cod_linha_negocio + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public DataTable load(int codigo)
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_linha_negocio=" + codigo + "";
        return _conn.dataTable(sql, "a");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, int Cod_Divisao)
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_linha_negocio in (select cod_linha_negocio from associa_div_linha_negocio where cod_divisao = " + Cod_Divisao.ToString() + ") ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public DataTable lista_ajax(int Cod_Divisao)
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and cod_linha_negocio in (select cod_linha_negocio from associa_div_linha_negocio where cod_divisao = " + Cod_Divisao.ToString() + ") ORDER BY DESCRICAO";
        return _conn.dataTable(sql, "a");
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY " + colunaOrdenar + "";
        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> lista()
    {
        string sql = "SELECT * FROM CAD_LINHA_NEGOCIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        return _conn.reader(sql);
    }

    public void lista(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_LINHA_NEGOCIO DESC";

        string sql = "SELECT * FROM (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS Row, *  ";
        sql += "    FROM CAD_LINHA_NEGOCIOS WHERE 1=1 ";

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
        string sql = "select COUNT(COD_LINHA_NEGOCIO) from CAD_LINHA_NEGOCIOS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaTimesheet(ref DataSet ds)
    {
        ds = ws.getCadastroProjetos();
    }

    public int getCodigoXTimesheet(int codTimesheet)
    {
        string sql = "SELECT COD_LINHA_NEGOCIO FROM CAD_LINHA_NEGOCIOS WHERE COD_REFERENCIA=" + codTimesheet + "  AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
