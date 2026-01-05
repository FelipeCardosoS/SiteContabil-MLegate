using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de Despesa
/// </summary>
public class Despesa
{
	private DespesaDAO despesaDAO;
	private int codDespesa;
	private int codEmpresa;
	private int codTipoDespesa;
	private string descricao;
	private bool valorLivre;
	private bool anexoObrigatorio;

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

	public bool ValorLivre
	{
		get
		{
			return valorLivre;
		}

		set
		{
			valorLivre = value;
		}
	}

	public bool AnexoObrigatorio
	{
		get
		{
			return anexoObrigatorio;
		}

		set
		{
			anexoObrigatorio = value;
		}
	}

	public Despesa()
	{

	}

	public Despesa(Conexao c)
	{
		despesaDAO = new DespesaDAO(c);
	}

	public List<string> valida()
	{
		List<string> erros = new List<string>();

		if (String.IsNullOrEmpty(Descricao) || Descricao.Trim().Length == 0)
			erros.Add("Informe a Descrição");
		if (codTipoDespesa == 0)
			erros.Add("Informe o Tipo de Despesa");

		return erros;
	}

	public List<string> valida(Despesa despesa)
	{
		return despesa.valida();
	}

	public List<Despesa> lista()
	{
		DataTable tbDespesa = despesaDAO.lista();

		List<Despesa> listaDespesa = new List<Despesa>((from rowDespesa in tbDespesa.AsEnumerable()
																	select new Despesa()
																	{
																		CodDespesa = Convert.ToInt32(rowDespesa["COD_DESPESA"]),
																		CodTipoDespesa = Convert.ToInt32(rowDespesa["COD_TIPO_DESPESA"]),
																		CodEmpresa = Convert.ToInt32(rowDespesa["COD_EMPRESA"]),
																		Descricao = Convert.ToString(rowDespesa["DESCRICAO"]),
																		ValorLivre = Convert.ToBoolean(rowDespesa["VALOR_LIVRE"]),
																		AnexoObrigatorio = Convert.ToBoolean(rowDespesa["ANEXO_OBRIGATORIO"]),
																	}));

		return listaDespesa;
	}

	public Despesa load(int codDespesa)
	{
		DataTable tbDespesa = despesaDAO.load(codDespesa);

		if (tbDespesa.Rows.Count == 0)
			return new Despesa();

		Despesa despesa = new Despesa
		{
			CodDespesa = Convert.ToInt32(tbDespesa.Rows[0]["COD_DESPESA"]),
			CodTipoDespesa = Convert.ToInt32(tbDespesa.Rows[0]["COD_TIPO_DESPESA"]),
			CodEmpresa = Convert.ToInt32(tbDespesa.Rows[0]["COD_EMPRESA"]),
			Descricao = Convert.ToString(tbDespesa.Rows[0]["DESCRICAO"]),
			ValorLivre = Convert.ToBoolean(tbDespesa.Rows[0]["VALOR_LIVRE"]),
			AnexoObrigatorio = Convert.ToBoolean(tbDespesa.Rows[0]["ANEXO_OBRIGATORIO"])
		};

		return despesa;
	}

	public List<string> salva(Despesa despesa)
	{
		List<string> erros = valida(despesa);
		if (erros.Count != 0)
			return erros;
		
		despesaDAO.salva(despesa);

		return erros;

	}

	public double totalRegistros(string descricao, string tipoDespesa)
	{
		return despesaDAO.totalRegistros(descricao, tipoDespesa);
	}

	public void listaPaginada(ref DataTable tb, string descricao, string tipoDespesa, int paginaAtual, string ordenacao)
	{
		despesaDAO.listaPaginada(ref tb, descricao, tipoDespesa, paginaAtual, ordenacao);
	}

	public void deleta(int codDespesa)
	{
		despesaDAO.deleta(codDespesa);
	}

	public bool possuiDependenciaTipoDespesa(int codTipoDespesa)
	{
		return despesaDAO.possuiDependenciaTipoDespesa(codTipoDespesa);
	}
}