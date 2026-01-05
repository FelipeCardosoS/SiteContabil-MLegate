using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de ValorDespesaJob
/// </summary>
public class ValorDespesaJob
{
	private int codEmpresa;
	private int codTipoDespesa;
	private int codJob;
	private DateTime dataInicio;
	private DateTime? dataFim;
	private decimal valorLimite;

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

	public int CodTipoDespesa
	{
		get
		{
			return codTipoDespesa;
		}

		set
		{
			codTipoDespesa = value;
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

	public DateTime? DataFim
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

	public ValorDespesaJob()
	{
		//
		// TODO: Adicionar lógica do construtor aqui
		//
	}

	public List<string> valida(List<ValorDespesaJob> lista)
	{
		List<string> erros = new List<string>();

		List<ValorDespesaJob> listaOrdenada = lista.OrderBy(o => o.DataInicio).ToList();
		bool sucesso = true;
		int i = 0;
		for(i = 0; i < listaOrdenada.Count() - 1; i++)
		{
			if (listaOrdenada[i].DataFim.HasValue)
				break;

			if(listaOrdenada[i].DataInicio > listaOrdenada[i].DataFim.Value || listaOrdenada[i].DataFim.Value >= listaOrdenada[i + 1].DataInicio)
			{
				sucesso = false;
				break;
			}
		}

		if(!sucesso || i < listaOrdenada.Count() - 1 || (listaOrdenada[i].DataFim.HasValue && listaOrdenada[i].DataInicio > listaOrdenada[i].DataFim.Value))
		{
			erros.Add("Há interferência entre Períodos de Valor Limite nesta Despesa");
		}

		int contValorDespesa = 0;
		foreach (ValorDespesaJob valorDespesa in lista)
		{
			List<string> errosValorDespesa = new List<string>();
			errosValorDespesa.Add("Período de Valor Despesa " + ++contValorDespesa);

			if (valorDespesa.valorLimite <= 0)
				erros.Add("Valor Limite Inválido");
			if (valorDespesa.DataInicio == DateTime.MinValue)
				erros.Add("Informe a Data de Inicio do Período");
			if (!valorDespesa.DataFim.HasValue && valorDespesa.DataInicio < lista.Max(o => o.DataInicio))
				erros.Add("O Período Aberto deve ser o mais recente.");
			else if (valorDespesa.DataFim.HasValue && valorDespesa.DataFim < valorDespesa.DataInicio)
				erros.Add("A Data Fim não pode ser anterior ao Inicio");
			if (errosValorDespesa.Count > 1)
				erros.AddRange(errosValorDespesa);
		}

		return erros;
	}


}