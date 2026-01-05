using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for cotacaoDAO
/// </summary>
public class cotacaoDAO
{
	private Conexao _conn;

    public cotacaoDAO(Conexao c)
	{
        _conn = c;
	}

    public DataTable loadTotal()
    {
        string sql = "select * from CAD_COTACAO";
        return _conn.dataTable(sql, "cotacao");
    }

    public SCotacao load(int codCotacao) 
    {
        SCotacao SCotacao = null;
        string sql = "select *, CAD_MOEDAS.DESCRICAO as DESCRICAO from CAD_COTACAO, CAD_MOEDAS where COD_COTACAO=" + codCotacao;
        DataTable tb = _conn.dataTable(sql, "codcotacao");
        if (tb.Rows.Count > 0)
        {
            SCotacao = createObject(tb.Rows[0]);
        }

        return SCotacao;
    }

    private SCotacao createObject(DataRow row)
    {
        SCotacao t = new SCotacao();
        t.codMoeda = Convert.ToInt32(row["COD_MOEDA"].ToString());
        t.codCotacao = Convert.ToInt32(row["COD_MOEDA"].ToString());
        t.descrMoeda = row["DESCRICAO"].ToString();
        t.data = Convert.ToDateTime(row["DATA"]);
        t.valor = Convert.ToDecimal(row["VALOR"]);
        return t;
    }

    public DataTable load(int codMoeda, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = " , COD_COTACAO ASC ";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY DATA DESC " + tmpOrdenacao + " )  ";
        sql += " AS Row, CAD_COTACAO.*, CAD_MOEDAS.DESCRICAO ";
        sql += " FROM CAD_COTACAO, CAD_MOEDAS WHERE 1=1 AND CAD_COTACAO.COD_MOEDA = CAD_MOEDAS.COD_MOEDA ";

        if (codMoeda != 0)
            sql += " AND CAD_COTACAO.COD_MOEDA = "+codMoeda+" ";

        sql += " ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        return _conn.dataTable(sql, "cotacao");
    }

    public int totalCotacao()
    {
        string sql = "SELECT COUNT(*) AS TOTAL FROM CAD_COTACAO ";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void delete(int cod)
    {
        string sql = "DELETE FROM CAD_COTACAO WHERE COD_COTACAO = " + cod;
        _conn.execute(sql);
    }

    public void deletePeriodo(DateTime de, DateTime ate, int codMoeda) 
    {
        string sql = "DELETE FROM CAD_COTACAO WHERE COD_MOEDA = " + codMoeda + " AND DATA >= '" + de.ToString("yyyyMMdd") + "' AND DATA <= '" + ate.ToString("yyyyMMdd") + "' ";
        _conn.execute(sql);
    }

    public bool novo(int COD_MOEDA, string VALOR, DateTime DATA)
    {
        string sql = "INSERT INTO CAD_COTACAO (COD_MOEDA, VALOR, DATA) VALUES (" + COD_MOEDA + ",'" + VALOR.Replace(",",".") + "','" + DATA.ToString("yyyyMMdd") + "')";
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

    public bool editar(int codCotacao, int codMoeda, string valor, DateTime DATA)
    {
        string sql = "UPDATE CAD_COTACAO SET COD_MOEDA = " + codMoeda + ", VALOR = '" + valor.Replace(",", ".") + "', DATA = '" + DATA + "' WHERE COD_COTACAO = " + codCotacao;
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

    public decimal cotacaoMediaMes(DateTime dtInicio, DateTime dtFinal, int codMoeda) 
    {
        string sql = "SELECT ROUND(AVG(VALOR),4) FROM cad_cotacao WHERE DATA >= '" + dtInicio.ToString("yyyyMMdd") + "' AND DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND COD_MOEDA = " + codMoeda + " ";
        return Convert.ToDecimal(_conn.scalar(sql));
    }

    public decimal cotacaoDiaria(DateTime dia, int codMoeda) 
    {
        string sql = "select VALOR from cad_cotacao where DATA = '" + dia.ToString("yyyyMMdd") + " 00:00:00.000' AND COD_MOEDA = " + codMoeda + " ";
        return Convert.ToDecimal(_conn.scalar(sql));
    }

    public decimal cotacaoDiaria(DateTime inicio, DateTime final, int codMoeda)
    {
        string sql = "select TOP 1 VALOR from cad_cotacao where DATA >= '" + inicio.ToString("yyyyMMdd") + " 00:00:00.000' AND DATA <= '" + final.ToString("yyyyMMdd") + " 00:00:00.000' AND COD_MOEDA = " + codMoeda + " ORDER BY DATA DESC";
        return Convert.ToDecimal(_conn.scalar(sql));
    }

    public DataTable loadCotacaoLancamento(int lote, int seq_lote) 
    {
        string sql = " select x.cod_moeda, x.descricao, sum(x.valor) as valor from ( " +
                        " select moeda.cod_moeda as cod_moeda, moeda.descricao as descricao, 0 as valor from cad_moedas as moeda " +
                        " union all " +
                        " select moeda.cod_moeda as cod_moeda, moeda.descricao as descricao, ISNULL(cotacao.valor,0) as valor " +
                        " from cad_moedas as moeda left join LANCTOS_COTACAO as cotacao on moeda.cod_moeda = cotacao.cod_moeda where cotacao.lote = "+lote+" and cotacao.seq_lote = "+seq_lote+" " +
                        " ) x group by x.cod_moeda, x.descricao ";
        
        return _conn.dataTable(sql,"cotacao");
    }

    public decimal retornaCotacao(int lote, int seq_lote, int cod_moeda) {
        string sql = "select VALOR from LANCTOS_COTACAO where LOTE = " + lote + " and SEQ_LOTE = " + seq_lote + " and COD_MOEDA = " + cod_moeda;
        return Convert.ToDecimal(_conn.scalar(sql));
    }
}