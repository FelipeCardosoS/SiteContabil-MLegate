using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de RegrasContabeisDAO
/// </summary>
public class RegrasContabeisDAO
{
	private Conexao _conn;

	public RegrasContabeisDAO()
	{
		_conn = new Conexao();
	}

	public RegrasContabeisDAO(Conexao c)
	{
		_conn = c;
	}

	public DataTable lista(int cod_empresa)
	{
		int codEmpresaPai = Convert.ToInt32(HttpContext.Current.Session["empresa"]);
		string sql = "SELECT COD_REGRA_CONTABIL, NOME, COD_CONTA, VALOR, TIPO_VALOR, DIA_VENCIMENTO, COD_TERCEIRO, HISTORICO, TITULO, TIPO_REGRA, COD_EMPRESA, COD_EMPRESA_PAI FROM CAD_REGRAS_CONTABEIS WHERE COD_EMPRESA_PAI = " + codEmpresaPai + " AND  COD_EMPRESA = " + cod_empresa;

		DataTable tb = _conn.dataTable(sql, "tbRegrasContabeis");
		return tb;
	}

	public void salva(List<RegraContabil> listaRegrasContabeis, int codEmpresa)
	{
		if (listaRegrasContabeis == null || listaRegrasContabeis.Count == 0)
			return;

		int codEmpresaPai = Convert.ToInt32(HttpContext.Current.Session["empresa"]);

		string sql = "";

		foreach(RegraContabil regra in listaRegrasContabeis)
		{
			if(regra.CodRegraContabil == 0)
			{
				sql += "INSERT INTO CAD_REGRAS_CONTABEIS (COD_EMPRESA_PAI, COD_EMPRESA, NOME, COD_CONTA, VALOR, TIPO_VALOR, DIA_VENCIMENTO, COD_TERCEIRO, HISTORICO, TITULO, TIPO_REGRA)";
				sql += "VALUES (" + codEmpresaPai + ", " + codEmpresa + ", '" + regra.Nome.Replace("'", "''").Replace("\"", "\"\"") + "', '" + regra.CodConta.Replace("'", "''").Replace("\"", "\"\"");
				sql += "', " + Convert.ToString(regra.Valor).Replace(",", ".") + ", '" + regra.TipoValor + "', " + regra.DiaVencimento + ", " + regra.CodTerceiro + ", '" + regra.Historico.Replace("'", "''").Replace("\"", "\"\"");
				sql += "', " + Convert.ToInt32(regra.Titulo) + ", '" + regra.TipoRegra.Replace("'", "''").Replace("\"", "\"\"") + "');";
			}
			else
			{
				sql += "UPDATE CAD_REGRAS_CONTABEIS SET NOME = '" + regra.Nome.Replace("'", "''").Replace("\"", "\"\"") + "', COD_CONTA = '" + regra.CodConta.Replace("'", "''").Replace("\"", "\"\"");
				sql += "', VALOR = " + Convert.ToString(regra.Valor).Replace(",", ".") + ", TIPO_VALOR = '" + regra.TipoValor + "', DIA_VENCIMENTO = " + regra.DiaVencimento + ", COD_TERCEIRO = ";
				sql += regra.CodTerceiro + ", HISTORICO = '" + regra.Historico.Replace("'", "''").Replace("\"", "\"\"") + "', TITULO = " + Convert.ToInt32(regra.Titulo) + ", TIPO_REGRA = '";
				sql += regra.TipoRegra.Replace("'", "''").Replace("\"", "\"\"") + "' WHERE COD_REGRA_CONTABIL = " + regra.CodRegraContabil + " AND COD_EMPRESA = " + codEmpresa + " AND COD_EMPRESA_PAI = ";
				sql += codEmpresaPai + ";";
			}
		}

		_conn.execute(sql);
	}

	public void removeLista(List<int> listaDeletar, int codEmpresa)
	{
		if (listaDeletar == null || listaDeletar.Count == 0)
			return;

		string sql = "DELETE FROM CAD_REGRAS_CONTABEIS WHERE COD_EMPRESA = " + codEmpresa + " AND COD_EMPRESA_PAI = " + Convert.ToInt32(HttpContext.Current.Session["empresa"]);
		sql += " AND COD_REGRA_CONTABIL IN (" + string.Join(", ", listaDeletar) + ")";

		_conn.execute(sql);
	}
}