using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for tiposImpostoDAO
/// </summary>
public class tiposImpostoDAO
{
    private Conexao _conn;

	public tiposImpostoDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(STipoImposto tipoImposto)
    {
        string sql = "insert into cad_tipos_imposto(tipo_imposto,descricao,cod_empresa)values('" + tipoImposto.tipoImposto + "','" + tipoImposto.descricao + "'," + tipoImposto.codEmpresa + ");";
        

        object result = _conn.scalar(sql);
    }

    public void update(STipoImposto tipoImposto)
    {
        string sql = "update cad_tipos_imposto set descricao='" + tipoImposto.descricao + "'  where tipo_imposto='" + tipoImposto.tipoImposto + "' and cod_empresa=" + tipoImposto.codEmpresa;

        _conn.execute(sql);
    }

    public void delete(string tipoImposto, int codEmpresa)
    {
        string sql = "delete from cad_tipos_imposto where tipo_imposto='" + tipoImposto + "' and cod_empresa=" + codEmpresa;

        _conn.execute(sql);
    }

    public STipoImposto load(string tipoImposto, int codEmpresa)
    {
        string sql = "select * from cad_tipos_imposto where tipo_imposto='" + tipoImposto + "' and cod_empresa=" + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "tiposImposto");
        STipoImposto t = null;
        if (tb.Rows.Count > 0)
        {
            t = createObject(tb.Rows[0]);
        }

        return t;
    }

    public List<STipoImposto> lista(int codEmpresa)
    {
        string sql = "select ";
        sql += " cad_tipos_imposto.*";
        sql += "    FROM cad_tipos_imposto WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        DataTable tb = _conn.dataTable(sql, "tipos_imposto");
        List<STipoImposto> list = new List<STipoImposto>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i]));
        }
        return list;
    }

    private STipoImposto createObject(DataRow row)
    {
        STipoImposto t = new STipoImposto();
        t.tipoImposto = row["TIPO_IMPOSTO"].ToString();
        t.descricao = row["DESCRICAO"].ToString();
        t.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        return t;
    }

    public DataTable lista(string tipoImposto, string descricao, int codEmpresa, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "TIPO_IMPOSTO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, cad_tipos_imposto.*";
        sql += "    FROM cad_tipos_imposto WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(tipoImposto))
            sql += " AND tipo_imposto='" + tipoImposto + "'";


        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "tipos_imposto");
    }

    public int totalRegistros(string tipoImposto, string descricao, int codEmpresa)
    {
        string sql = "select count(tipo_imposto) from cad_tipos_imposto WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND cad_tipos_imposto.DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(tipoImposto))
            sql += " AND cad_tipos_imposto.tipo_imposto='" + tipoImposto + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}