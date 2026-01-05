using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class FuncaoCliente
{
    private int _codigo;
    private string _descricao;

    private funcoesDAO funcaoDAO;
    private List<string> erros;

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

	public FuncaoCliente(Conexao c)
	{
        funcaoDAO = new funcoesDAO(c);
	}

    public FuncaoCliente(Conexao c, int codigo)
        :this(c)
    {
        _codigo = codigo;
    }

    public FuncaoCliente(Conexao c, int codigo, string descricao)
        : this(c, codigo)
    {
        _descricao = descricao;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe uma descrição.");

        if (erros.Count == 0)
        {
            funcaoDAO.insert(_descricao);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código Inválido.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe uma descrição.");

        if (erros.Count == 0)
        {
            funcaoDAO.update(_codigo,_descricao);
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código Inválido.");


        if (erros.Count == 0)
        {
            funcaoDAO.delete(_codigo);
        }

        return erros;
    }

    public void load()
    {
        DataTable linha = funcaoDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
        }
    }

    public void lista(ref DataTable tb)
    {
        funcaoDAO.lista(ref tb);
    }

    public void listaPaginada(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        funcaoDAO.lista(ref tb, descricao, paginaAtual, ordenacao);
    }

    public int totalRegistros(string descricao)
    {
        return funcaoDAO.totalRegistros(descricao);
    }
}
