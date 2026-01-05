using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de TipoDespesaDAO
/// </summary>
public class TipoDespesaDAO
{

	private Conexao _conn;

	public TipoDespesaDAO(Conexao c)
	{
		_conn = c;
	}

	public TipoDespesaDAO()
	{
		_conn = new Conexao();
	}

	public DataTable lista()
	{
		string sql = "SELECT COD_TIPO_DESPESA, DESCRICAO, UNIDADE, TIPO_QUANTITATIVO, COD_EMPRESA FROM CAD_TIPOS_DESPESA WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "TIPOS_DESPESA");
	}

	public void listaPaginada(ref DataTable tb, string descricao, string unidade, int paginaAtual, string ordenacao)
	{
		if (string.IsNullOrEmpty(ordenacao))
			ordenacao = "COD_TIPO_DESPESA DESC";

		string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY CTD." + ordenacao + ") AS ROW, ";
		sql += "CTD.COD_TIPO_DESPESA, CTD.DESCRICAO, CTD.UNIDADE, ";
		sql += "(CASE WHEN (DATA_INICIO <= convert(datetime, dateadd(hour, -3, GETUTCDATE())) and DATA_FIM >= convert(datetime, dateadd(hour, -3, GETUTCDATE()))) THEN TDP.VALOR_REFERENCIA ELSE 0 END) AS VALOR_REFERENCIA ";
		sql += "FROM CAD_TIPOS_DESPESA CTD, TIPOS_DESPESA_PERIODOS TDP ";
		sql += "WHERE TDP.COD_TIPO_DESPESA = CTD.COD_TIPO_DESPESA ";
		sql += "AND CTD.COD_EMPRESA = TDP.COD_EMPRESA ";
		sql += "AND CTD.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		if (!string.IsNullOrEmpty(descricao))
			sql += " AND CTD.DESCRICAO LIKE '%" + descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";
		if (!string.IsNullOrEmpty(unidade))
			sql += " AND CTD.UNIDADE LIKE '%" + unidade.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";

		sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

		_conn.fill(sql, ref tb);
	}

	public int salva(TipoDespesa tipoDespesa)
	{
		string sql = "";

		if (tipoDespesa.CodTipoDespesa == 0)
		{
			sql = "INSERT INTO CAD_TIPOS_DESPESA (DESCRICAO, UNIDADE, TIPO_QUANTITATIVO, COD_EMPRESA) OUTUPUT INSERTED.COD_TIPO_DESPESA VALUES ('";
			sql += tipoDespesa.Descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', '" + tipoDespesa.Unidade.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', ";
			sql += Convert.ToInt32(tipoDespesa.TipoQuantitativo) + ", " + HttpContext.Current.Session["empresa"] + ")";
		}
		else
		{
			sql = "UPDATE CAD_TIPOS_DESPESA SET DESCRICAO = '" + tipoDespesa.Descricao.Trim().Replace("'", "''").Replace("\"", "\"\"");
			sql += "', UNIDADE = '" + tipoDespesa.Unidade.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', TIPO_QUANTITATIVO = " + Convert.ToInt32(tipoDespesa.TipoQuantitativo);
			sql += " OUTPUT INSERTED.COD_TIPO_DESPESA WHERE COD_TIPO_DESPESA = " + tipoDespesa.CodTipoDespesa;
			sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		}

		return Convert.ToInt32(_conn.scalar(sql));
	}

	public bool salvaPeriodo(int codTipoDespesa, TipoDespesaPeriodo periodo)
	{
		string sql = "DECLARE @CONFLITOS INT SELECT @CONFLITOS = COUNT(*) FROM TIPOS_DESPESA_PERIODOS ";
		sql += "WHERE (DATA_INICIO BETWEEN '" + periodo.DataInicio.ToString("dd-MM-yyyy") + "' AND '" + periodo.DataFim.ToString("dd-MM-yyyy") + "' ";
		sql += "OR DATA_FIM BETWEEN '" + periodo.DataInicio.ToString("dd-MM-yyyy") + "' AND '" + periodo.DataFim.ToString("dd-MM-yyyy") + "' ";
		sql += "OR '" + periodo.DataInicio.ToString("dd-MM-yyyy") + "' BETWEEN DATA_INICIO AND DATA_FIM ";
		sql += "OR '" + periodo.DataFim.ToString("dd-MM-yyyy") + "' BETWEEN DATA_INICIO AND DATA_FIM) ";
		sql += "AND COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		sql += " AND COD_TIPO_DESPESA_PERIODO <> " + periodo.CodTipoDespesaPeriodo;
		sql += " IF @CONFLITOS = 0 ";
		sql += "BEGIN ";

		if (periodo.CodTipoDespesaPeriodo == 0)
		{
			sql += "INSERT INTO TIPOS_DESPESA_PERIODOS (COD_TIPO_DESPESA, DATA_INICIO, DATA_FIM, VALOR_REFERENCIA, COD_EMPRESA) VALUES (";
			sql += codTipoDespesa + ", '" + periodo.DataInicio.ToString("dd-MM-yyyy") + "', '" + periodo.DataFim.ToString("dd-MM-yyyy") + "', " + Convert.ToString(periodo.ValorReferencia).Replace(",", ".");
			sql += ", " + HttpContext.Current.Session["empresa"] + ")";
		}
		else
		{
			sql += "UPDATE TIPOS_DESPESA_PERIODOS SET COD_TIPO_DESPESA = " + codTipoDespesa + ", DATA_INICIO = '" + periodo.DataInicio.ToString("dd-MM-yyyy") + "', DATA_FIM = '";
			sql += periodo.DataFim.ToString("dd-MM-yyyy") + "', VALOR_REFERENCIA = " + Convert.ToString(periodo.ValorReferencia).Replace(",", ".");
			sql += " WHERE COD_TIPO_DESPESA_PERIODO = " + periodo.CodTipoDespesaPeriodo + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		}

		sql += " END SELECT @CONFLITOS";

		int conflitos = Convert.ToInt32(_conn.scalar(sql));
		return conflitos == 0;
	}

	public void deleta(int codTipoDespesa)
	{
		if (codTipoDespesa == 0)
			return;

		string sql = "DELETE FROM CAD_TIPOS_DESPESA WHERE COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public void deleta(List<int> listaCodTipoDespesa)
	{
		if (listaCodTipoDespesa.Count == 0)
			return;

		string sql = "DELETE FROM CAD_TIPOS_DESPESA WHERE COD_TIPO_DESPESA in (" + string.Join(",", listaCodTipoDespesa)+ ") AND COD_TIPO_DESPESA NOT IN (SELECT DISTINCT COD_TIPO_DESPESA FROM CAD_DESPESAS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ") AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public double totalRegistros(string descricao, string unidade)
	{
		string sql = "SELECT COUNT(COD_TIPO_DESPESA) FROM CAD_TIPOS_DESPESA WHERE 1=1 ";
		sql += "AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		if (!string.IsNullOrEmpty(descricao))
			sql += " AND DESCRICAO LIKE '%" + descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";
		if (!string.IsNullOrEmpty(unidade))
			sql += " AND UNIDADE LIKE '%" + unidade.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";

		return Convert.ToDouble(_conn.scalar(sql));
	}

	internal DataTable load(int codTipoDespesa)
	{
		string sql = "SELECT COD_TIPO_DESPESA, DESCRICAO, UNIDADE, TIPO_QUANTITATIVO, COD_EMPRESA FROM CAD_TIPOS_DESPESA WHERE COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "TIPO_DESPESA");
	}

	internal DataTable loadPeriodos(int codTipoDespesa)
	{
		string sql = "SELECT COD_TIPO_DESPESA_PERIODO, COD_TIPO_DESPESA, DATA_INICIO, DATA_FIM, VALOR_REFERENCIA, COD_EMPRESA FROM TIPOS_DESPESA_PERIODOS WHERE COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "TIPO_DESPESA");
	}

	public void deletaPeriodos(int codTipoDespesa, List<int> listaDeletar)
	{
		if (listaDeletar == null || listaDeletar.Count == 0)
			return;

		string sql = "DELETE FROM TIPOS_DESPESA_PERIODOS WHERE COD_TIPO_DESPESA_PERIODO IN (" + string.Join(", ", listaDeletar) + ") AND COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public void deletaPeriodos(int codTipoDespesa)
	{
		string sql = "DELETE FROM TIPOS_DESPESA_PERIODOS WHERE COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}
}