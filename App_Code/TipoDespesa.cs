using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de TipoDespesa
/// </summary>
public class TipoDespesa
{
	public Conexao _conn;
	private TipoDespesaDAO tipoDespesaDAO;
	private TipoDespesaPeriodo tipoDespesaPeriodo;

	private int codTipoDespesa;
	private int codEmpresa;
	private string descricao;
	private string unidade;
	private List<TipoDespesaPeriodo> listaPeriodos;
	private bool tipoQuantitativo;

	public TipoDespesa(Conexao c) : base()
	{
		_conn = c;
		tipoDespesaDAO = new TipoDespesaDAO(c);
	}

	public TipoDespesa()
	{
		tipoDespesaPeriodo = new TipoDespesaPeriodo();
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

	public string Unidade
	{
		get
		{
			return unidade;
		}

		set
		{
			unidade = value;
		}
	}

	public string Descricao
	{
		get
		{
			return descricao;
		}

		set
		{
			descricao = value;
		}
	}


	public List<TipoDespesaPeriodo> ListaPeriodos
	{
		get
		{
			return listaPeriodos;
		}

		set
		{
			listaPeriodos = value;
		}
	}

	public bool TipoQuantitativo
	{
		get
		{
			return tipoQuantitativo;
		}

		set
		{
			tipoQuantitativo = value;
		}
	}

	public string Display_Descricao
	{
		get
		{
			return descricao + " (" + unidade + ")";
		}
	}
	public List<string> valida()
	{
		List<string> erros = new List<string>();

		if (String.IsNullOrEmpty(Descricao) || Descricao.Trim().Length == 0)
			erros.Add("Informe a Descrição do Tipo de Despesa");
		if (string.IsNullOrEmpty(Unidade) || Unidade.Trim().Length == 0)
			erros.Add("Informe a Unidade do Tipo de Despesa");
		else if (Unidade.Trim().Length > 5)
			erros.Add("A Unidade deve conter no máximo 5 caracteres");

		if(TipoQuantitativo)
			erros.AddRange(tipoDespesaPeriodo.valida(listaPeriodos));

		return erros;
	}


	public List<TipoDespesa> lista()
	{
		DataTable tbTipoDespesa = tipoDespesaDAO.lista();

		List<TipoDespesa> listaTipoDespesa = new List<TipoDespesa>((from rowTipoDespesa in tbTipoDespesa.AsEnumerable()
											  select new TipoDespesa()
											  {
												  CodTipoDespesa = Convert.ToInt32(rowTipoDespesa["COD_TIPO_DESPESA"]),
												  CodEmpresa = Convert.ToInt32(rowTipoDespesa["COD_EMPRESA"]),
												  Descricao = Convert.ToString(rowTipoDespesa["DESCRICAO"]),
												  Unidade = Convert.ToString(rowTipoDespesa["UNIDADE"]),
												  TipoQuantitativo = Convert.ToBoolean(rowTipoDespesa["TIPO_QUANTITATIVO"]),
												  ListaPeriodos = new List<TipoDespesaPeriodo>()
											  }));

		return listaTipoDespesa;
	}

	public void listaPaginada(ref DataTable tb, string descricao, string unidade, int paginaAtual, string ordenacao)
	{
		tipoDespesaDAO.listaPaginada(ref tb, descricao, unidade, paginaAtual, ordenacao);
	}

	public double totalRegistros(string descricao, string unidade)
	{
		return tipoDespesaDAO.totalRegistros(descricao, unidade);
	}

	public TipoDespesa load(int codTipoDespesa)
	{
		DataTable tbTipoDespesa = tipoDespesaDAO.load(codTipoDespesa);

		if (tbTipoDespesa.Rows.Count == 0)
			return new TipoDespesa();

		TipoDespesa tipoDespesa = new TipoDespesa
								{
									CodTipoDespesa = Convert.ToInt32(tbTipoDespesa.Rows[0]["COD_TIPO_DESPESA"]),
									CodEmpresa = Convert.ToInt32(tbTipoDespesa.Rows[0]["COD_EMPRESA"]),
									Descricao = Convert.ToString(tbTipoDespesa.Rows[0]["DESCRICAO"]),
									Unidade = Convert.ToString(tbTipoDespesa.Rows[0]["UNIDADE"]),
									TipoQuantitativo = Convert.ToBoolean(tbTipoDespesa.Rows[0]["TIPO_QUANTITATIVO"])
								};


		DataTable tbPeriodos = tipoDespesaDAO.loadPeriodos(codTipoDespesa);

		List<TipoDespesaPeriodo> listaPeriodos = new List<TipoDespesaPeriodo>((from rowPeriodo in tbPeriodos.AsEnumerable()
																			   select new TipoDespesaPeriodo()
																			   {
																				   CodTipoDespesaPeriodo = Convert.ToInt32(rowPeriodo["COD_TIPO_DESPESA_PERIODO"]),
																				   CodTipoDespesa = Convert.ToInt32(rowPeriodo["COD_TIPO_DESPESA"]),
																				   CodEmpresa = Convert.ToInt32(rowPeriodo["COD_EMPRESA"]),
																				   DataInicio = Convert.ToDateTime(rowPeriodo["DATA_INICIO"]),
																				   DataFim = Convert.ToDateTime(rowPeriodo["DATA_FIM"]),
																				   ValorReferencia = Convert.ToDecimal(rowPeriodo["VALOR_REFERENCIA"])
																			   }));

		tipoDespesa.listaPeriodos = listaPeriodos;

		return tipoDespesa;

	}



	public List<string> salva()
	{
		return salva(this, new List<int>());
	}

	public List<string> salva(TipoDespesa tipoDespesa)
	{
		List<string> erros = tipoDespesa.valida();

		if (erros.Count > 0)
			return erros;

		tipoDespesaDAO.salva(tipoDespesa);
		return erros;
	}

	public List<string> salva(TipoDespesa tipoDespesa, List<int> listaDeletar)
	{
		List<string> erros = tipoDespesa.valida();

		if (erros.Count > 0)
			return erros;
		int codTipoDespesa = 0;

		if (tipoDespesa.CodTipoDespesa != 0)
		{
			TipoDespesa tipoDespesaAtual = load(tipoDespesa.CodTipoDespesa);

			if (tipoDespesaAtual.TipoQuantitativo && !tipoDespesa.TipoQuantitativo)
				tipoDespesaDAO.deletaPeriodos(tipoDespesa.CodTipoDespesa);

			tipoDespesaDAO.salva(tipoDespesa);
			codTipoDespesa = tipoDespesa.CodTipoDespesa;
		}
		else
			codTipoDespesa = tipoDespesaDAO.salva(tipoDespesa);

		if (!tipoDespesa.TipoQuantitativo)
		{
			tipoDespesaDAO.deletaPeriodos(tipoDespesa.CodTipoDespesa);
			listaDeletar = new List<int>();
			tipoDespesa.ListaPeriodos = new List<TipoDespesaPeriodo>
			{
				new TipoDespesaPeriodo
				{
					CodTipoDespesaPeriodo = 0,
					CodTipoDespesa = codTipoDespesa,
					DataInicio = new DateTime(1900, 1, 1),
					DataFim = DateTime.MaxValue,
					ValorReferencia = 1
				}
			};
		}

		string errosPeriodo = salvaPeriodos(codTipoDespesa, tipoDespesa.listaPeriodos, listaDeletar);

		if (!string.IsNullOrEmpty(errosPeriodo))
			erros.Add(errosPeriodo);

		return erros;
	}

	public string salvaPeriodos(int codTipoDespesa, List<TipoDespesaPeriodo> listaPeriodos, List<int> listaDeletar)
	{
		tipoDespesaDAO.deletaPeriodos(codTipoDespesa, listaDeletar);
		foreach(TipoDespesaPeriodo periodo in listaPeriodos)
		{
			if (!tipoDespesaDAO.salvaPeriodo(codTipoDespesa, periodo))
				return "Há erros no período de " + periodo.DataInicio.ToString("dd/MM/yyyy") + " à " + periodo.DataFim.ToString("dd/MM/yyyy");
		}

		return string.Empty;
	}

	public void deleta()
	{
		deleta(CodTipoDespesa);
	}

	public void deleta(int codTipoDespesa)
	{
		Despesa despesa = new Despesa(_conn);
		if (!despesa.possuiDependenciaTipoDespesa(codTipoDespesa))
		{
			tipoDespesaDAO.deletaPeriodos(codTipoDespesa);
			tipoDespesaDAO.deleta(codTipoDespesa);
		}
	}

	public void deleta(List<int> listaTiposDespesa)
	{
		foreach(int codTipoDespesa in listaTiposDespesa)
		{
			deleta(codTipoDespesa);
		}
	}


}