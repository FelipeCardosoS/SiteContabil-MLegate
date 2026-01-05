using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de TipoDespesaPeriodo
/// </summary>
public class TipoDespesaPeriodo
{
	private int codTipoDespesaPeriodo;
	private int codTipoDespesa;
	private int codEmpresa;
	private DateTime dataInicio;
	private DateTime dataFim;
	private decimal valorReferencia;

	public int CodTipoDespesaPeriodo
	{
		get
		{
			return codTipoDespesaPeriodo;
		}

		set
		{
			codTipoDespesaPeriodo = value;
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

	public decimal ValorReferencia
	{
		get
		{
			return valorReferencia;
		}

		set
		{
			valorReferencia = value;
		}
	}

	public TipoDespesaPeriodo()
	{
		//
		// TODO: Adicionar lógica do construtor aqui
		//
	}

	internal List<string> valida(List<TipoDespesaPeriodo> listaPeriodos)
	{
		List<string> erros = new List<string>();

		if (listaPeriodos.Count == 0)
			erros.Add("Insira ao menos 1 Valor Referência");

		//Para cada Período, o ValorReferencia deve ser maior que 0 e DataFim deve ser válida (mais recente que DataInicio)
		int contValorDespesa = 0;
		foreach (TipoDespesaPeriodo periodo in listaPeriodos)
		{
			List<string> errosPeriodo = new List<string>();
			errosPeriodo.Add("Período de Valor Referência " + ++contValorDespesa);

			if (periodo.ValorReferencia <= 0)
				erros.Add("Valor Referência Inválido");
			else if (periodo.DataFim < periodo.DataInicio)
				erros.Add("A Data Fim não pode ser anterior ao Inicio");
			if (errosPeriodo.Count > 1)
				erros.AddRange(errosPeriodo);
		}

		if (erros.Count > 0)
			erros.Insert(0, "Erros nos Períodos de Valor Referência:\n");

		return erros;

	}
}