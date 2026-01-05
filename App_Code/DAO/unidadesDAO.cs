using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for unidadesDAO
/// </summary>
public class unidadesDAO
{
    private Conexao _conn;

	public unidadesDAO(Conexao conn)
	{
        _conn = conn;
	}

    public int insert(SUnidade unidade)
    {
        string sql = "insert into cad_unidades_medidas(sigla,descricao,cod_empresa)values('" + unidade.sigla + "','" + unidade.descricao + "'," + unidade.codEmpresa + ");";
        sql += "SELECT SCOPE_IDENTITY();";

        object result = _conn.scalar(sql);
        int cod = 0;
        int.TryParse(result.ToString(), out cod);
        return cod;
    }

    public void update(SUnidade unidade)
    {
        string sql = "update cad_unidades_medidas set descricao='" + unidade.descricao + "'  where sigla='" + unidade.sigla + "' and cod_empresa=" + unidade.codEmpresa;

        _conn.execute(sql);
    }

    public void delete(string sigla, int codEmpresa)
    {
        string sql = "delete from cad_unidades_medidas where sigla='" + sigla + "' and cod_empresa=" + codEmpresa;

        _conn.execute(sql);
    }

    public SUnidade load(string sigla, int codEmpresa)
    {
        string sql = "select * from cad_unidades_medidas where sigla='" + sigla + "' and cod_empresa=" + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "produto");
        SUnidade unidade = null;
        if (tb.Rows.Count > 0)
        {
            createObject(tb.Rows[0]);
        }

        return unidade;
    }

    public SUnidade createObject(DataRow row)
    {
        SUnidade unidade = new SUnidade();
        unidade.sigla = row["SIGLA"].ToString();
        unidade.descricao = row["DESCRICAO"].ToString();
        unidade.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        return unidade;
    }

    public List<SUnidade> lista(int codEmpresa)
    {
        string sql = "select cad_unidades_medidas.* ";
        sql += "    FROM cad_unidades_medidas WHERE 1=1 ";
        sql += " and COD_EMPRESA = " + codEmpresa + "";

        DataTable tb = _conn.dataTable(sql, "produto");
        List<SUnidade> list = new List<SUnidade>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i]));
        }

        return list;
    }

    public DataTable lista(string sigla, string descricao, int codEmpresa, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "SIGLA";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, cad_unidades_medidas.*";
        sql += "    FROM cad_unidades_medidas WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(sigla))
            sql += " AND sigla='" + sigla + "'";


        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "produtos");
    }

    public int totalRegistros(string sigla, string descricao, int codEmpresa)
    {
        string sql = "select count(SIGLA) from cad_unidades_medidas WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND cad_unidades_medidas.DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(sigla))
            sql += " AND cad_unidades_medidas.sigla='" + sigla + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}