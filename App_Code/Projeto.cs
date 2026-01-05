using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de Projeto
/// </summary>
public class Projeto
{
	private Conexao _conn;
	private int codProjeto;
	private string descProjeto;

	public Projeto()
	{
	}

	public Projeto(Conexao c)
	{
		_conn = c;
	}

	public int CodProjeto
	{
		get
		{
			return codProjeto;
		}

		set
		{
			codProjeto = value;
		}
	}

	public string DescProjeto
	{
		get
		{
			return descProjeto;
		}

		set
		{
			descProjeto = value;
		}
	}


	public List<Projeto> lista()
	{
		return new List<Projeto>();
	}

	public List<Projeto> listaPorDivisao(int codDivisao)
	{
		return new List<Projeto>();
	}

	public void remove(int codProjeto)
	{

	}

	public void remove(List<Projeto> lista)
	{

	}

	public void remove(List<int> listaCodigo)
	{

	}

	public void insere()
	{

	}

	public void insere(Projeto projeto)
	{

	}
}