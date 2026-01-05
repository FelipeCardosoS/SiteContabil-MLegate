using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de DespesaJob
/// </summary>
public class DespesaJob
{
	private Conexao _conn;
	private DespesaJobDAO despesaJobDAO;
	private ValorDespesaJob valorDespesaJob;

	private int codDespesaJob;
	private int codEmpresa;
	private int codJob;
	private int codDespesa;
	private bool cobrarCliente;
	private decimal valorLimite;

	public int CodDespesaJob
	{
		get
		{
			return codDespesaJob;
		}

		set
		{
			codDespesaJob = value;
		}
	}

	public int CodEmpresa
	{
		get
		{
			return codEmpresa;
		}

		set
		{
			codEmpresa = value;
		}
	}

	public int CodJob
	{
		get
		{
			return codJob;
		}

		set
		{
			codJob = value;
		}
	}

	public int CodDespesa
	{
		get
		{
			return codDespesa;
		}

		set
		{
			codDespesa = value;
		}
	}

	public bool CobrarCliente
	{
		get
		{
			return cobrarCliente;
		}

		set
		{
			cobrarCliente = value;
		}
	}

	public decimal ValorLimite
	{
		get
		{
			return valorLimite;
		}

		set
		{
			valorLimite = value;
		}
	}

	public DespesaJob()
	{
	}

	public DespesaJob(Conexao c)
	{
		_conn = c;
		despesaJobDAO = new DespesaJobDAO(c);
		valorDespesaJob = new ValorDespesaJob();
	}

	public List<string> valida(List<DespesaJob> lista)
	{
		List<string> erros = new List<string>();

		if (lista.GroupBy(o => o.CodDespesa).Where(o => o.Count() > 1).Count() > 1)
			erros.Add("Há Despesas Duplicadas no Job!\n");
		
		int contDespesa = 0;
		foreach (DespesaJob despesa in lista)
		{
			List<string> errosDespesa = new List<string>();
			errosDespesa.Add("Gestor " + ++contDespesa);

			if (despesa.CodDespesa <= 0)
				erros.Add("Informe a Despesa");
			if (despesa.ValorLimite <= 0)
				erros.Add("Informe o Valor Limite");

			if (errosDespesa.Count > 1)
				erros.AddRange(errosDespesa);
		}

		return erros;
	}

	public List<DespesaJob> lista(int codJob)
	{
		DataTable tbDespesaJobs = despesaJobDAO.listaDespesaJobs(codJob);

		List<DespesaJob> listaDespesaJobs = tbDespesaJobs.AsEnumerable().Select(o => new DespesaJob {
			CodDespesaJob = Convert.ToInt32(o["COD_DESPESA_JOB"]),
			CodEmpresa = Convert.ToInt32(o["COD_EMPRESA"]),
			CodDespesa = Convert.ToInt32(o["COD_DESPESA"]),
			CodJob = Convert.ToInt32(o["COD_JOB"]),
			CobrarCliente = Convert.ToBoolean(o["COBRAR_CLIENTE"]),
			ValorLimite = Convert.ToInt32(o["VALOR_LIMITE"])
		}).ToList();

		return listaDespesaJobs;
	}

	public void salva(int codJob, List<DespesaJob> listaDespesaJob, List<int> listaDeletar = null)
	{
		despesaJobDAO.deletaDespesaJob(codJob, listaDeletar);
		despesaJobDAO.salva(codJob, listaDespesaJob);
		
	}

	public void deleta(int codJob)
	{
		despesaJobDAO.deletaDespesaJob(codJob);
	}
}