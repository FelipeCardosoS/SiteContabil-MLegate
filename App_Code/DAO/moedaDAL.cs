using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for moedaDAL
/// </summary>
public class moedaDAL
{
	private Conexao _conn;

    public moedaDAL(Conexao c)
	{
        _conn = c;
	}

    public SMoeda load(int codMoeda) 
    {
        SMoeda SMoeda = null;
        string sql = "select * from CAD_MOEDAS where COD_MOEDA=" + codMoeda;
        DataTable tb = _conn.dataTable(sql, "tiposImposto");
        if (tb.Rows.Count > 0)
        {
            SMoeda = createObject(tb.Rows[0]);
        }

        return SMoeda;
    }

    private SMoeda createObject(DataRow row)
    {
        SMoeda t = new SMoeda();
        t.codMoeda = Convert.ToInt32(row["COD_MOEDA"].ToString());
        t.descricao = row["DESCRICAO"].ToString();
        return t;
    }

    public DataTable loadTotal() 
    {
        string sql = "select * from CAD_MOEDAS";
        return _conn.dataTable(sql, "moeda");
    }

    public DataTable load(string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "DESCRICAO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, CAD_MOEDAS.*";
        sql += "    FROM CAD_MOEDAS WHERE 1=1 ";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND DESCRICAO like '%" + descricao + "%'";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "moeda");
    }

    public int totalMoeda()
    {
        string sql = "SELECT COUNT(*) AS TOTAL FROM CAD_MOEDAS ";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void delete(int cod)
    {
        string sql = "DELETE FROM CAD_MOEDAS WHERE COD_MOEDA = "+cod;
        _conn.execute(sql);
    }

    public bool novo(string descricao)
    {
        string sql = "INSERT INTO CAD_MOEDAS (DESCRICAO) VALUES ('"+descricao+"')";
        try
        {
            _conn.execute(sql);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public bool editar(int codMoeda, string descricao)
    {
        string sql = "UPDATE CAD_MOEDAS SET DESCRICAO = '" + descricao + "' WHERE COD_MOEDA = "+codMoeda;
        try
        {
            _conn.execute(sql);
        }
        catch
        {
            return false;
        }
        return true;
    }
}