using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de Class1
/// </summary>
public class RegraContabil
{
	private Conexao _conn;
	private RegrasContabeisDAO dao;

	private int codRegraContabil;
	private int codEmpresa;
	private int codEmpresaPai;
	private string nome;
	private string codConta;
	private decimal valor;
	private string tipoValor;
	private int diaVencimento;
	private int codTerceiro;
	private string histrorico;
	private bool titulo;
	private string tipoRegra;

	public RegraContabil()
	{
	}

	public RegraContabil(Conexao c)
	{
		_conn = c;
		dao = new RegrasContabeisDAO(c);
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

	public string Nome
	{
		get
		{
			return nome;
		}

		set
		{
			nome = value;
		}
	}

	public string CodConta
	{
		get
		{
			return codConta;
		}

		set
		{
			codConta = value;
		}
	}

	public decimal Valor
	{
		get
		{
			return valor;
		}

		set
		{
			valor = value;
		}
	}

	public string TipoValor
	{
		get
		{
			return tipoValor;
		}

		set
		{
			tipoValor = value;
		}
	}

	public int DiaVencimento
	{
		get
		{
			return diaVencimento;
		}

		set
		{
			diaVencimento = value;
		}
	}

	public int CodTerceiro
	{
		get
		{
			return codTerceiro;
		}

		set
		{
			codTerceiro = value;
		}
	}

	public string Historico
	{
		get
		{
			return histrorico;
		}

		set
		{
			histrorico = value;
		}
	}

	public bool Titulo
	{
		get
		{
			return titulo;
		}

		set
		{
			titulo = value;
		}
	}

	public int CodEmpresaPai
	{
		get
		{
			return codEmpresaPai;
		}

		set
		{
			codEmpresaPai = value;
		}
	}

	public string TipoRegra
	{
		get
		{
			return tipoRegra;
		}

		set
		{
			tipoRegra = value;
		}
	}

	public int CodRegraContabil
	{
		get
		{
			return codRegraContabil;
		}

		set
		{
			codRegraContabil = value;
		}
	}

	public List<string> valida(List<RegraContabil> lista)
	{
		List<string> erros = new List<string>();
		
		if (lista == null)
			return erros;
		int countRegra = 0;
		foreach(RegraContabil regra in lista)
		{
			++countRegra;

			List<string> errosRegra = regra.valida();
			if(errosRegra.Count > 0)
			{
				erros.Add("Regra " + countRegra + ":\n");
				erros.AddRange(errosRegra);
			}
		}

		List<string> errosGerais = new List<string>();

		if (lista.Where(o => o.TipoRegra.Equals("DESPESA_CONTA")).Count() > 1)
			errosGerais.Add("Há mais de uma Conta de Despesa!");
		if (lista.Where(o => o.TipoRegra.Equals("FORNECEDOR_CONTA")).Count() > 1)
			errosGerais.Add("Há mais de uma Conta de Fornecedor!");
		if (lista.Where(o => o.TipoRegra.Equals("IR_DESCONTO")).Count() > 1)
			errosGerais.Add("Há mais de uma Regra de Desconto de IR!");
		if (lista.Where(o => o.TipoRegra.Equals("PCC_DESCONTO")).Count() > 1)
			errosGerais.Add("Há mais de uma Regra de Desconto de PCC!");

		if(errosGerais.Count > 0)
		{
			erros.Add("\nOutros Erros:\n");
			erros.AddRange(errosGerais);
		}

		return erros;
	}

	public List<string> valida()
	{
		List<string> erros = new List<string>();

		List<string> errosRegra = new List<string>();
		bool preenchido = false;

		if (String.IsNullOrEmpty(CodConta))
		{
			erros.Add("Informe a Conta");
		}
		else
		{
			preenchido = true;
		}

		if (string.IsNullOrEmpty(TipoRegra))
		{
			erros.Add("A Regra Contábil não possui tipo");
		}
		else if (!TipoRegra.EndsWith("CONTA"))
		{
			if (Valor == 0)
			{
				erros.Add("Defina um valor maior que 0");
			}
			else
			{
				preenchido = true;
			}

			if (String.IsNullOrEmpty(TipoValor))
			{
				erros.Add("Defina o Tipo do Valor");
			}
			else
			{
				preenchido = true;
			}

			if (DiaVencimento == 0)
			{
				erros.Add("Informe o Dia do Vencimento");
			}
			else if (DiaVencimento > 31)
			{
				erros.Add("Dia do Vencimento inválido");
				preenchido = true;
			}
			else
			{
				preenchido = true;
			}

			if (CodTerceiro == 0 && Titulo)
			{
				erros.Add("Defina o Terceiro do Título");
			}
			else
			{
				preenchido = true;
			}

			if (String.IsNullOrEmpty(Historico))
			{
				erros.Add("Informe o Histórico");
			}
			else
			{
				preenchido = true;
			}
		}

		if (preenchido && erros.Count() > 0)
		{
			return erros;
		}

		else if (!preenchido)
		{
			TipoRegra = string.Empty;
		}

		return erros;

	}

	public List<RegraContabil> lista(int codEmpresa)
	{
		DataTable tb = dao.lista(codEmpresa);

		List<RegraContabil> listaRegrasContabeis = new List<RegraContabil>();
		tb.AsEnumerable().All(o =>
		{
			listaRegrasContabeis.Add(new RegraContabil
			{
				CodRegraContabil = Convert.ToInt32(o["COD_REGRA_CONTABIL"]),
				CodEmpresaPai = Convert.ToInt32(o["COD_EMPRESA_PAI"]),
				CodEmpresa = codEmpresa,
				Nome = Convert.ToString(o["NOME"]),
				CodConta = Convert.ToString(o["COD_CONTA"]),
				Valor = Convert.ToDecimal(o["VALOR"]),
				TipoValor = Convert.ToString(o["TIPO_VALOR"]),
				DiaVencimento = Convert.ToInt32(o["DIA_VENCIMENTO"]),
				CodTerceiro = Convert.ToInt32(o["COD_TERCEIRO"]),
				Historico = Convert.ToString(o["HISTORICO"]),
				Titulo = Convert.ToBoolean(o["TITULO"]),
				TipoRegra = Convert.ToString(o["TIPO_REGRA"])
			});
			return true;
		});

		return listaRegrasContabeis;
	}

	public List<string> salva(List<RegraContabil> listaRegras, List<int> listaDeletar, int codEmpresa)
	{
		List<string> erros = valida(listaRegras);
		if (erros.Count > 0)
			return erros;

		remove(listaDeletar, codEmpresa);
		dao.salva(listaRegras, codEmpresa);

		return erros;
	}

	public void remove(List<int> listaDeletar, int codEmpresa)
	{
		dao.removeLista(listaDeletar, codEmpresa);
	}
}