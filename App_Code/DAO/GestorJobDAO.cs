using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de GestorJobDAO
/// </summary>
public class GestorJobDAO
{
	private Conexao _conn;

	public GestorJobDAO(Conexao c)
	{
		_conn = c;
	}

	public DataTable lista(int codJob)
	{
		string sql = "SELECT COD_GESTOR_JOB, COD_JOB, COD_GESTOR, DATA_INICIO, DATA_FIM, COD_EMPRESA FROM GESTOR_JOBS WHERE COD_JOB = " + codJob;

		return _conn.dataTable(sql, "gestor_job");
	}

	public void salva(int codJob, List<GestorJob> lista)
	{
		string sql = "";

		if (lista == null || lista.Count == 0)
			return;
		
		foreach(GestorJob gestorJob in lista)
		{
			if(gestorJob.CodGestorJob == 0)
			{
				sql += "INSERT INTO GESTOR_JOBS (COD_JOB, COD_GESTOR, DATA_INICIO, DATA_FIM, COD_EMPRESA) VALUES (";
				sql += codJob + ", " + gestorJob.CodGestor + ", '" + gestorJob.DataInicio.ToString("dd-MM-yyyy");
				sql += "', '" + gestorJob.DataFim.ToString("dd-MM-yyyy") + "', " + HttpContext.Current.Session["empresa"] + ");";
			}
			else
			{
				sql += "UPDATE GESTOR_JOBS SET COD_GESTOR = " + gestorJob.CodGestor + ", DATA_INICIO = '";
				sql += gestorJob.DataInicio.ToString("dd-MM-yyyy") + "', DATA_FIM = '" + gestorJob.DataFim.ToString("dd-MM-yyyy");
				sql += "' WHERE COD_GESTOR_JOB = " + gestorJob.CodGestorJob;
				sql +=" AND COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ";";
			}
		}

		_conn.execute(sql);
	}

	public void remove(int codJob)
	{
		string sql = "DELETE FROM GESTOR_JOBS WHERE COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public void remove(int codJob, List<int> listaDeletar)
	{
		if (listaDeletar == null || listaDeletar.Count == 0)
			return;

		string sql = "DELETE FROM GESTOR_JOBS WHERE COD_JOB = " + codJob + " AND COD_GESTOR_JOB IN (" + string.Join(", ", listaDeletar) + ") AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}
}