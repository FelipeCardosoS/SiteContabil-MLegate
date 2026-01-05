using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for classificacaoContaDAO
/// </summary>
public class classificacaoContaDAO
{
    private Conexao _conn;

	public classificacaoContaDAO(Conexao conn)
	{
        _conn = conn;
	}

    public void insert(SClassificacaoConta o)
    {
        string sql = "INSERT INTO CAD_CLASSIFICACAO_CONTA(COD_CLASSIFICACAO,COD_EMPRESA,DESCRICAO)";
        sql += "VALUES('" + o.codClassificacao + "'," + o.codEmpresa + ",'" + o.descricao + "')";

        _conn.execute(sql);
    }


    public void update(SClassificacaoConta o)
    {
        string sql = "UPDATE CAD_CLASSIFICACAO_CONTA SET COD_CLASSIFICACAO='" + o.codClassificacao + "',COD_EMPRESA=" + o.codEmpresa + ",DESCRICAO='" + o.descricao + "' ";
        sql += " WHERE COD_CLASSIFICACAO=" + o.codClassificacao;

        _conn.execute(sql);
    }

    public void delete(string codigo, int codEmpresa)
    {
        string sql = "delete from CAD_CLASSIFICACAO_CONTA where COD_CLASSIFICACAO=" + codigo + " and COD_EMPRESA=" + codEmpresa;

        _conn.execute(sql);
    }

    public SClassificacaoConta load(string codigo, int codEmpresa)
    {
        string sql = "SELECT * FROM CAD_CLASSIFICACAO_CONTA WHERE COD_CLASSIFICACAO='" + codigo + "' AND COD_EMPRESA=" + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "dados");
        SClassificacaoConta o = null;
        if (tb.Rows.Count > 0)
        {
            o = createObject(tb.Rows[0]);
        }

        return o;
    }

    public List<SClassificacaoConta> lista(int codEmpresa)
    {
        string sql = "select * from cad_classificacao_conta where cod_empresa=" + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "dados");
        List<SClassificacaoConta> list = new List<SClassificacaoConta>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i]));
        }
        return list;
    }

    private SClassificacaoConta createObject(DataRow row)
    {
        SClassificacaoConta o = new SClassificacaoConta();
        if (row != null)
        {
            o.codClassificacao = row["COD_CLASSIFICACAO"].ToString();
            int v = 0;
            int.TryParse(row["COD_EMPRESA"].ToString(), out v);
            o.codEmpresa = v;
            o.descricao = row["DESCRICAO"].ToString();
        }
        return o;
    }

    public DataTable lista(string codigo, string descricao, int codEmpresa, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_CLASSIFICACAO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, CAD_CLASSIFICACAO_CONTA.*";
        sql += "    FROM CAD_CLASSIFICACAO_CONTA WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(codigo))
            sql += " AND COD_CLASSIFICACAO='" + codigo + "'";


        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "dados");
    }

    public int totalRegistros(string codigo, string descricao, int codEmpresa)
    {
        string sql = "select count(COD_CLASSIFICACAO) from CAD_CLASSIFICACAO_CONTA WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND DESCRICAO like '%" + descricao + "%'";

        if (!string.IsNullOrEmpty(codigo))
            sql += " AND tipo_imposto='" + codigo + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}