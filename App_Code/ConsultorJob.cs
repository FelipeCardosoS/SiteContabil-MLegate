using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de ConsultorJob
/// </summary>
public class ConsultorJob
{
	private Conexao _conn;
	private ConsultorJobDAO consultorJobDAO;

	private int codConsultorJob;
	private int codEmpresa;
	private int codConsultor;
	private int codJob;
	private int codAprovador;
	private int codAprovadorRDV;
	private int codFaturamento;
	private decimal taxaConsultor;
	private decimal custoIntraDivisao;
	private string contatoAlocacao;
	private string telefoneContato;
	private string emailContato;
	private bool enviarEmailAprovador;
	private DateTime dataInicio;
	private DateTime dataFim;

	public int CodConsultorJob
	{
		get
		{
			return codConsultorJob;
		}

		set
		{
			codConsultorJob = value;
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

	public int CodConsultor
	{
		get
		{
			return codConsultor;
		}

		set
		{
			codConsultor = value;
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

	public int CodAprovador
	{
		get
		{
			return codAprovador;
		}

		set
		{
			codAprovador = value;
		}
	}

	public int CodAprovadorRDV
	{
		get
		{
			return codAprovadorRDV;
		}

		set
		{
			codAprovadorRDV = value;
		}
	}

	public int CodFaturamento
	{
		get
		{
			return codFaturamento;
		}

		set
		{
			codFaturamento = value;
		}
	}

	public decimal TaxaConsultor
	{
		get
		{
			return taxaConsultor;
		}

		set
		{
			taxaConsultor = value;
		}
	}

	public decimal CustoIntraDivisao
	{
		get
		{
			return custoIntraDivisao;
		}

		set
		{
			custoIntraDivisao = value;
		}
	}


	public string ContatoAlocacao
	{
		get
		{
			return contatoAlocacao;
		}

		set
		{
			contatoAlocacao = value;
		}
	}

	public string TelefoneContato
	{
		get
		{
			return telefoneContato;
		}

		set
		{
			telefoneContato = value;
		}
	}

	public string EmailContato
	{
		get
		{
			return emailContato;
		}

		set
		{
			emailContato = value;
		}
	}

	public bool EnviarEmailAprovador
	{
		get
		{
			return enviarEmailAprovador;
		}

		set
		{
			enviarEmailAprovador = value;
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

	public ConsultorJob()
	{

	}

	public ConsultorJob(Conexao c)
	{
		_conn = c;
		consultorJobDAO = new ConsultorJobDAO(c);
	}

	public List<string> valida(List<ConsultorJob> lista)
	{
		List<string> erros = new List<string>();

		if (lista.GroupBy(o => o.CodConsultor).Where(o => o.Count() > 1).Count() > 1)
			erros.Add("Há Consultores Duplicados no Job!\n");

		int contConsultor = 0;
		foreach (ConsultorJob consultor in lista)
		{
			List<string> errosConsultor = new List<string>();
			errosConsultor.Add("Gestor " + ++contConsultor);

			if (consultor.CodConsultor <= 0)
				erros.Add("Informe o Consultor");
			if (consultor.CodAprovador <= 0)
				erros.Add("Informe o Aprovador");
			if (consultor.CodAprovadorRDV <= 0)
				erros.Add("Informe o Aprovador RDV");
			if (consultor.TaxaConsultor < 0)
				erros.Add("Taxa Consultor Inválida");
			if (consultor.CustoIntraDivisao < 0)
				erros.Add("Custo Intra Divisão Inválido");
			if (consultor.DataInicio == DateTime.MinValue)
				erros.Add("Informe a Data de Inicio do Consultor");
			if (consultor.DataFim < consultor.DataInicio)
				erros.Add("A Data Fim do Consultor deve ser posterior ao Inicio");
			
			if (errosConsultor.Count > 1)
				erros.AddRange(errosConsultor);
		}

		return erros;
	}


	public List<ConsultorJob> lista(int codJob)
	{
		DataTable tbConsultorjob = consultorJobDAO.lista(codJob);

		List<ConsultorJob> listaConsultorJob = tbConsultorjob.AsEnumerable().Select(o => new ConsultorJob
		{
			CodConsultorJob = Convert.ToInt32(o["COD_CONSULTOR_JOB"]),
			CodEmpresa = Convert.ToInt32(o["COD_EMPRESA"]),
			CodConsultor = Convert.ToInt32(o["COD_CONSULTOR"]),
			CodJob = Convert.ToInt32(o["COD_JOB"]),
			CodAprovador = Convert.ToInt32(o["COD_APROVADOR"]),
			CodAprovadorRDV = Convert.ToInt32(o["COD_APROVADOR_RDV"]),
			TaxaConsultor = Convert.ToDecimal(o["TAXA_CONSULTOR"]),
			CustoIntraDivisao = Convert.ToDecimal(o["CUSTO_INTRADIVISAO"]),
			EnviarEmailAprovador = Convert.ToBoolean(o["ENVIAR_EMAIL_APROVADOR"]),
			ContatoAlocacao = Convert.ToString(o["CONTATO_ALOCACAO"]),
			TelefoneContato = Convert.ToString(o["TELEFONE_CONTATO_ALOCACAO"]),
			EmailContato = Convert.ToString(o["EMAIL_CONTATO_ALOCACAO"]),
			DataInicio = Convert.ToDateTime(o["DATA_INICIO"]),
			DataFim = Convert.ToDateTime(o["DATA_FIM"])
		}).ToList();

		return listaConsultorJob;
	}

	public void salva(int codJob, List<ConsultorJob> listaConsultorJob, List<int> listaDeletar = null)
	{
		consultorJobDAO.deleta(codJob, listaDeletar);
		consultorJobDAO.salva(codJob, listaConsultorJob);
	}

	public void deleta(int codJob)
	{
		consultorJobDAO.deleta(codJob);
	}
}