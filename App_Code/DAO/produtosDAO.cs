using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for produtosDAO
/// </summary>
public class produtosDAO
{
    private Conexao _conn;

    public produtosDAO(Conexao conn)
    {
        _conn = conn;
    }

    public int insert(SProduto produto)
    {
        string sql = "insert into cad_produtos(descricao,gera_credito,cod_empresa,sigla)values('" + produto.descricao + "'," + Convert.ToInt32(produto.geraCredito) + "," + produto.codEmpresa + ",'" + produto.sigla + "');";
        sql += "SELECT SCOPE_IDENTITY();";

        object result = _conn.scalar(sql);
        int cod = 0;
        int.TryParse(result.ToString(), out cod);
        return cod;
    }

    public void update(SProduto produto)
    {
        string sql = "update cad_produtos set descricao='" + produto.descricao + "', sigla='" + produto.sigla + "',gera_credito=" + Convert.ToInt32(produto.geraCredito) + " where cod_produto=" + produto.codProduto + " and cod_empresa=" + produto.codEmpresa;

        _conn.execute(sql);
    }

    public void delete(int codProduto, int codEmpresa)
    {
        string sql = "delete from cad_produtos where cod_produto=" + codProduto + " and cod_empresa=" + codEmpresa;

        _conn.execute(sql);
    }

    public SProduto load(int codProduto, int codEmpresa)
    {
        string sql = "select * from cad_produtos where cod_produto=" + codProduto + " and cod_empresa=" + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "produto");
        SProduto produto = null;
        if (tb.Rows.Count > 0)
        {
            produto = createObject(tb.Rows[0]);
        }

        return produto;
    }

    private SProduto createObject(DataRow row)
    {
        SProduto produto = new SProduto();
        produto.codProduto = Convert.ToInt32(row["COD_PRODUTO"]);
        produto.descricao = row["DESCRICAO"].ToString();
        produto.geraCredito = Convert.ToBoolean(row["GERA_CREDITO"]);
        produto.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        produto.sigla = row["SIGLA"].ToString();
        return produto;
    }

    public DataTable lista(string descricao, bool? gera_credito, int codEmpresa, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CAD_PRODUTOS.COD_PRODUTO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, CAD_PRODUTOS.*";
        sql += "    FROM CAD_PRODUTOS WHERE 1=1 ";

        sql += " and CAD_PRODUTOS.COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND CAD_PRODUTOS.DESCRICAO like '%" + descricao.Replace("'", "''") + "%'";

        if (gera_credito.HasValue)
            sql += " AND CAD_PRODUTOS.DEBITO_CREDITO='" + Convert.ToInt32(gera_credito.Value) + "'";


        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "produtos");
    }

    public List<SProduto> list(int codEmpresa)
    {
        string sql = "select * from CAD_PRODUTOS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + " order by descricao";

        DataTable tb = _conn.dataTable(sql, "produtos");
        List<SProduto> list = new List<SProduto>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i]));
        }
        return list;
    }

    public int totalRegistros(string descricao, bool? gera_credito, int codEmpresa)
    {
        string sql = "select count(COD_PRODUTO) from CAD_PRODUTOS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + codEmpresa + "";

        if (!string.IsNullOrEmpty(descricao))
            sql += " AND CAD_PRODUTOS.DESCRICAO like '%" + descricao.Replace("'", "''") + "%'";

        if (gera_credito.HasValue)
            sql += " AND CAD_PRODUTOS.DEBITO_CREDITO='" + Convert.ToInt32(gera_credito.Value) + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}