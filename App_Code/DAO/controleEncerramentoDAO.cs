using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for controleEncerramentoDAO
/// </summary>
public class controleEncerramentoDAO
{
    private Conexao _conn;

    public controleEncerramentoDAO(Conexao c)
    {
        _conn = c;
    }

    public void insert(DateTime periodo, double lote)
    {
        string sql = "INSERT INTO CONTROLE_ENCERRAMENTO(PERIODO,COD_EMPRESA,LOTE)";
        sql += "VALUES";
        sql += "('" + periodo.ToString("yyyyMMdd") + "'," + HttpContext.Current.Session["empresa"] + "," + lote + ")";

        _conn.execute(sql);
    }

    public void delete(int codigoEncerramento)
    {
        string sql = "DELETE FROM CONTROLE_ENCERRAMENTO ";
        sql += " WHERE COD_ENCERRAMENTO=" + codigoEncerramento + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public void deleteLanctos(int codigoEncerramento)
    {
        string sql = "DELETE FROM lanctos_contab where LOTE <> 0 AND lote = (select lote from controle_encerramento ";
        sql += " WHERE COD_ENCERRAMENTO=" + codigoEncerramento + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + ")";
        _conn.execute(sql);

        sql = "DELETE FROM LANCTOS_CONTAB_MOEDA where lote = (select lote from controle_encerramento ";
        sql += " WHERE COD_ENCERRAMENTO=" + codigoEncerramento + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + ")";
        _conn.execute(sql);
    }

    public bool verificaEncerramento(DateTime periodo)
    {
        string sql = "select count(*) from controle_encerramento where periodo = '" + periodo.ToString("yyyyMMdd") + "'";
        int result = Convert.ToInt32(_conn.scalar(sql));

        if (result == 0)
            return false;
        else
            return true;
    }

    public DataTable listOfPeriodo(DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select * from CONTROLE_ENCERRAMENTO " +
                    " where PERIODO >= '"+inicio.ToString("yyyyMMdd")+"' and PERIODO <= '"+termino.ToString("yyyyMMdd")+"' " +
                    " and COD_EMPRESA="+codEmpresa;

        return _conn.dataTable(sql, "dados");
    }

    public void lista(ref DataTable tb, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "PERIODO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " DESC)  ";
        sql += "AS Row, *  ";
        sql += "    FROM CONTROLE_ENCERRAMENTO WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros()
    {
        string sql = "select count(cod_encerramento) from CONTROLE_ENCERRAMENTO WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
