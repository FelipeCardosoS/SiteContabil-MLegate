using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de GestorJob
/// </summary>
public class GestorJob
{
	private Conexao _conn;
	private GestorJobDAO dao;
	private int codGestorJob;
	private int codJob;
	private int codGestor;
	private int codEmpresa;
	private DateTime dataInicio;
	private DateTime dataFim;

	public GestorJob()
	{
		
	}

	public GestorJob(Conexao c) : base()
	{
		_conn = c;
		dao = new GestorJobDAO(c);
	}

	public int CodGestorJob
	{
		get
		{
			return codGestorJob;
		}

		set
		{
			codGestorJob = value;
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

	public int CodGestor
	{
		get
		{
			return codGestor;
		}

		set
		{
			codGestor = value;
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

	public DateTime DataInicio
	{
		get
		{
			return dataInicio;
		}

		set
		{
			dataInicio = value;
		}
	}

	public DateTime DataFim
	{
		get
		{
			return dataFim;
		}

		set
		{
			dataFim = value;
		}
	}

	public List<string> valida(List<GestorJob> lista)
	{
		List<string> erros = new List<string>();

		if (lista.GroupBy(o => o.CodGestor).Where(o => o.Count() > 1).Count() > 1)
			erros.Add("Há Gestores Duplicados no Job!\n");

		int contGestor = 0;
		foreach(GestorJob gestor in lista)
		{
			List<string> errosGestor = new List<string>();
			errosGestor.Add("Gestor " + ++contGestor);

			if (gestor.CodGestor <= 0)
				erros.Add("Informe o Gestor");
			if (gestor.DataInicio == DateTime.MinValue)
				erros.Add("Informe a Data de Inicio do Gestor");
			if (gestor.DataFim < gestor.DataInicio)
				erros.Add("A Data Fim do Gestor deve ser posterior ao Inicio");

			if (errosGestor.Count > 1)
				erros.AddRange(errosGestor);
		}

		return erros;
	}
	public List<GestorJob> lista(int codJob)
	{
		DataTable tbGestorJob = dao.lista(codJob);
		List<GestorJob> lista = new List<GestorJob>();

		tbGestorJob.AsEnumerable().All(o => {
			lista.Add(new GestorJob { 
				CodGestorJob = Convert.ToInt32(o["COD_GESTOR_JOB"]),
				CodGestor = Convert.ToInt32(o["COD_GESTOR"]),
				CodJob = Convert.ToInt32(o["COD_JOB"]),
				CodEmpresa = Convert.ToInt32(o["COD_EMPRESA"]),
				DataInicio = Convert.ToDateTime(o["DATA_INICIO"]),
				DataFim = Convert.ToDateTime(o["DATA_FIM"])
			});

			return true;
		});

		return lista;
	}

	public void salva(int codJob, List<GestorJob> lista, List<int> listaDeletar = null)
	{
		dao.remove(codJob, listaDeletar);
		dao.salva(codJob, lista);
	}

	public void deleta(int codJob)
	{
		dao.remove(codJob);
	}
}