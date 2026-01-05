using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de DespesaJobDAO
/// </summary>
public class DespesaJobDAO
{
	private Conexao _conn;

	public DespesaJobDAO(Conexao c)
	{
		_conn = c;
	}

	public DataTable listaDespesaJobs(int codJob)
	{
		string sql = "SELECT COD_DESPESA_JOB, COD_JOB, COD_DESPESA, COBRAR_CLIENTE, VALOR_LIMITE, COD_EMPRESA FROM DESPESA_JOBS WHERE COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "DESPESA_JOBS");
	}

	public void salva(int codJob, List<DespesaJob> listaDespesaJob)
	{
		if (listaDespesaJob == null || listaDespesaJob.Count == 0)
			return;
		
		string sql = string.Empty;

		foreach(DespesaJob despesaJob in listaDespesaJob)
		{
			if(despesaJob.CodDespesaJob == 0)
			{
				sql += "INSERT INTO DESPESA_JOBS (COD_JOB, COD_DESPESA, COBRAR_CLIENTE, VALOR_LIMITE, COD_EMPRESA) VALUES (";
				sql += codJob + ", " + despesaJob.CodDespesa + ", " + Convert.ToInt32(despesaJob.CobrarCliente) + ", ";
				sql += Convert.ToString(despesaJob.ValorLimite).Replace(",", ".") + ", " + HttpContext.Current.Session["empresa"] + ");";
			}
			else
			{
				sql += "UPDATE DESPESA_JOBS SET COD_JOB = " + codJob + ", COD_DESPESA = " + despesaJob.CodDespesa;
				sql += ", COBRAR_CLIENTE = " + Convert.ToInt32(despesaJob.CobrarCliente);
				sql += ", VALOR_LIMITE = " + Convert.ToString(despesaJob.ValorLimite).Replace(",", ".");
				sql += " WHERE COD_DESPESA_JOB = " + despesaJob.CodDespesaJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ";";
			}
		}

		_conn.execute(sql);
	}

	public void deletaDespesaJob(int codJob)
	{
		string sql = "DELETE FROM DESPESA_JOBS WHERE COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		_conn.execute(sql);
	}

	public void deletaDespesaJob(int codJob, List<int> listaDeletar)
	{
		if (listaDeletar == null || listaDeletar.Count == 0)
			return;

		string sql = "DELETE FROM DESPESA_JOBS WHERE COD_JOB =" + codJob + " AND COD_DESPESA_JOB IN (" + string.Join(", ", listaDeletar) + ") AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		_conn.execute(sql);
	}

	public void deletaDespesaJob(int codJob, int codDespesaJob)
	{
		string sql = "DELETE FROM DESPESA_JOBS WHERE COD_JOB =" + codJob + " AND COD_DESPESA_JOB = " + codDespesaJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
		_conn.execute(sql);
	}
}