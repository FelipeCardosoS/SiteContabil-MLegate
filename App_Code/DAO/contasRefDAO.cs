using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for contasRefDAO
/// </summary>
public class contasRefDAO
{
    private Conexao _conn;

	public contasRefDAO(Conexao conn)
	{
        _conn = conn;
	}

    public void insert(string codigoContaRef, string descricao, DateTime? iniValidade, DateTime? fimValidade, string analiticaSintetica)
    {
        string sql = "INSERT INTO CAD_CONTAS_REF(COD_CONTA_REF,DESCRICAO,INI_VALIDADE,FIM_VALIDADE,ANALITICA_SINTETICA)";
        sql += "VALUES";
        sql += "('" + codigoContaRef + "','" + descricao + "'," + (iniValidade.HasValue ? "'" + iniValidade.Value.ToString("yyyyMMdd") + "'" : "null") + "," + (fimValidade.HasValue ? "'" + fimValidade.Value.ToString("yyyyMMdd") + "'" : "null") + ",'" + analiticaSintetica + "');";

        _conn.execute(sql);
    }

    public void delete(string codigoContaRef)
    {
        string sql = "DELETE FROM CAD_CONTAS_REF WHERE COD_CONTA_REF='" + codigoContaRef + "'";

        _conn.execute(sql);
    }

    public List<SContaRef> list()
    {
        string sql = "SELECT * FROM CAD_CONTAS_REF";
        DataTable tb = _conn.dataTable(sql, "contas");
        List<SContaRef> list = new List<SContaRef>();
        foreach (DataRow row in tb.Rows)
        {
            list.Add(createObject(row));
        }

        return list;
    }

    public List<SContaRef> list_ECF()
    {
        string sql = "SELECT * FROM CAD_CONTAS_ECF";
        DataTable tb = _conn.dataTable(sql, "contas");
        List<SContaRef> list = new List<SContaRef>();
        foreach (DataRow row in tb.Rows)
        {
            list.Add(createObjectECF(row));
        }

        return list;
    }

    public SContaRef load(string codigo)
    {
        string sql = "SELECT * FROM CAD_CONTAS_REF WHERE COD_CONTA_REF='"+codigo+"'";
        DataTable tb = _conn.dataTable(sql, "contas");
        SContaRef list = null;
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            list = createObject(row);
        }

        return list;
    }

    private SContaRef createObject(DataRow row)
    {
        SContaRef c = new SContaRef();
        c.codigoRef = row["COD_CONTA_REF"].ToString();
        c.descricao = row["DESCRICAO"].ToString();
        c.iniValidade = null;
        if (row["INI_VALIDADE"] != DBNull.Value)
            c.iniValidade = Convert.ToDateTime(row["INI_VALIDADE"]);
        c.fimValidade = null;
        if (row["FIM_VALIDADE"] != DBNull.Value)
            c.fimValidade = Convert.ToDateTime(row["FIM_VALIDADE"]);
        c.analiticaSintetica = row["ANALITICA_SINTETICA"].ToString();
        return c;
    }

    private SContaRef createObjectECF(DataRow row)
    {
        SContaRef c = new SContaRef();
        c.codigoRef = row["COD_CONTA_ECF"].ToString();
        c.descricao = row["DESCRICAO"].ToString();
        c.iniValidade = null;
        if (row["INI_VALIDADE"] != DBNull.Value)
            c.iniValidade = Convert.ToDateTime(row["INI_VALIDADE"]);
        c.fimValidade = null;
        if (row["FIM_VALIDADE"] != DBNull.Value)
            c.fimValidade = Convert.ToDateTime(row["FIM_VALIDADE"]);
        c.analiticaSintetica = row["ANALITICA_SINTETICA"].ToString();
        return c;
    }
}