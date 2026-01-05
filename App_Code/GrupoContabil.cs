using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class GrupoContabil
{
    private int _codigo;
    private string _descricao;
    private string _conta;
    private string _regraExibicao;
    private gruposContabeisDAO grupoContabilDAO;
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

    public string conta
    {
        get { return _conta; }
        set { _conta = value; }
    }

    public string regraExibicao
    {
        get { return _regraExibicao; }
        set { _regraExibicao = value; }
    }

	public GrupoContabil(Conexao c)
	{
        grupoContabilDAO = new gruposContabeisDAO(c);
	}

    public GrupoContabil(Conexao c, int codigo)
        :this(c)
    {
        _codigo = codigo;
    }

    public GrupoContabil(Conexao c, int codigo, string descricao, string conta)
        : this(c,codigo)
    {
        _descricao = descricao;
        _conta = conta;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição está vazia");


        if (erros.Count == 0)
        {
            grupoContabilDAO.insert(_descricao, _conta, _regraExibicao);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido");

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição está vazia");


        if (erros.Count == 0)
        {
            grupoContabilDAO.update(_codigo,_descricao, _conta, _regraExibicao);
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido");


        if (erros.Count == 0)
        {
            grupoContabilDAO.delete(_codigo);
        }

        return erros;
    }

    public void load()
    {
        DataTable linha = grupoContabilDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
            _conta = linha.Rows[0]["COD_CONTA"].ToString();
            _regraExibicao = linha.Rows[0]["REGRA_EXIBICAO"].ToString();
        }
    }

    public int getCodigo(string descricao)
    {
        return grupoContabilDAO.get_codigo(descricao);
    }

    public void lista(ref DataTable tb)
    {
        grupoContabilDAO.lista(ref tb);
    }

    public void listaPaginada(ref DataTable tb,string descricao, int paginaAtual, string ordenacao)
    {
        grupoContabilDAO.lista(ref tb, descricao, paginaAtual, ordenacao);
    }

    public int totalRegistros(string descricao)
    {
        return grupoContabilDAO.totalRegistros(descricao);
    }
}
