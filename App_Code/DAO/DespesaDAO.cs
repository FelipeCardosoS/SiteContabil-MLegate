using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de DespesaDAO
/// </summary>
public class DespesaDAO
{

	private Conexao _conn;

	public DespesaDAO(Conexao c)
	{
		_conn = c;
	}

	public DespesaDAO()
	{
		_conn = new Conexao();
	}

	public DataTable lista()
	{
		string sql = "SELECT COD_DESPESA, COD_TIPO_DESPESA, COD_EMPRESA, DESCRICAO, VALOR_LIVRE, ANEXO_OBRIGATORIO FROM CAD_DESPESAS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "CAD_DESPESAS");
	}

	public void salva(Despesa despesa)
	{
		string sql = "";

		if (despesa.CodDespesa == 0)
			sql = "INSERT INTO CAD_DESPESAS (COD_TIPO_DESPESA, DESCRICAO, VALOR_LIVRE, ANEXO_OBRIGATORIO, COD_EMPRESA) VALUES (" + despesa.CodTipoDespesa + ", '" + despesa.Descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', " + Convert.ToInt32(despesa.ValorLivre) + ", " + Convert.ToInt32(despesa.AnexoObrigatorio) + ", " + HttpContext.Current.Session["empresa"] + ")";
		else
			sql = "UPDATE CAD_DESPESAS SET COD_TIPO_DESPESA = " + despesa.CodTipoDespesa + ", DESCRICAO = '" + despesa.Descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', VALOR_LIVRE = " + Convert.ToInt32(despesa.ValorLivre) + ", ANEXO_OBRIGATORIO = " + Convert.ToInt32(despesa.AnexoObrigatorio) + " WHERE COD_DESPESA = " + despesa.CodDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public void deleta(int codDespesa)
	{
		string sql = "DELETE FROM CAD_DESPESAS WHERE COD_DESPESA = " + codDespesa + "AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public double totalRegistros(string descricao, string tipoDespesa)
	{
		string sql = "SELECT COUNT(COD_DESPESA) FROM CAD_DESPESAS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		if (!string.IsNullOrEmpty(descricao))
			sql += " AND DESCRICAO LIKE '%" + descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";
		if (!string.IsNullOrEmpty(tipoDespesa) && !tipoDespesa.Equals("0"))
			sql += " AND COD_TIPO_DESPESA = " + tipoDespesa;

		return Convert.ToDouble(_conn.scalar(sql));
	}

	internal DataTable load(int codDespesa)
	{
		string sql = "SELECT COD_DESPESA, COD_TIPO_DESPESA, COD_EMPRESA, DESCRICAO, VALOR_LIVRE, ANEXO_OBRIGATORIO FROM CAD_DESPESAS WHERE COD_DESPESA = " + codDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "CAD_DESPESAS");

	}

	public void listaPaginada(ref DataTable tb, string descricao, string tipoDespesa, int paginaAtual, string ordenacao)
	{
		if (string.IsNullOrEmpty(ordenacao))
			ordenacao = "COD_DESPESA DESC";

		string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY CD." + ordenacao + ") AS ROW, ";
		sql += "CD.COD_DESPESA, CD.DESCRICAO, CTD.DESCRICAO + ' (' + CTD.UNIDADE + ')' AS TIPO_DESPESA ";
		sql += "FROM CAD_DESPESAS CD, CAD_TIPOS_DESPESA CTD ";
		sql += "WHERE CD.COD_TIPO_DESPESA = CTD.COD_TIPO_DESPESA ";
		sql += "AND CD.COD_EMPRESA = CTD.COD_EMPRESA ";
		sql += "AND CD.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		if (!string.IsNullOrEmpty(descricao))
			sql += " AND CD.DESCRICAO LIKE '%" + descricao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "%'";
		if (!string.IsNullOrEmpty(tipoDespesa) && !tipoDespesa.Equals("0"))
			sql += " AND CD.COD_TIPO_DESPESA = " + tipoDespesa;

		sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

		_conn.fill(sql, ref tb);
	}

	public bool possuiDependenciaTipoDespesa(int codTipoDespesa)
	{
		string sql = "SELECT COUNT(*) FROM CAD_DESPESAS WHERE COD_TIPO_DESPESA = " + codTipoDespesa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return Convert.ToInt32(_conn.scalar(sql)) > 0;
	}
}