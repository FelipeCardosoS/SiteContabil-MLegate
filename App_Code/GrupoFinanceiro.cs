using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Descrição resumida de GrupoEconomico
/// </summary>
public class GrupoFinanceiro
{

    private int _codEmpresa;
    private string _nome;
    private string _descricao;
    

    private GrupoFinanceiroDAO grupoFinanceiroDAO;

    public GrupoFinanceiro(Conexao c)
    {
        grupoFinanceiroDAO = new GrupoFinanceiroDAO(c);
    }

    public GrupoFinanceiro() {}

    public int codEmpresa
    {
        get
        {
            return _codEmpresa;
        }

        set
        {
            _codEmpresa = value;
        }
    }

    public string nome
    {
        get
        {
            return _nome;
        }

        set
        {
            _nome = value;
        }
    }

    public string descricao
    {
        get
        {
            return _descricao;
        }

        set
        {
            _descricao = value;
        }
    }

    public void lista(ref DataTable table, int pagina, string descricao, string ordenacao)
    {
        grupoFinanceiroDAO.list(ref table, pagina, descricao, ordenacao);
    }
    public int totalRegistros(string descricao)
    {
        return grupoFinanceiroDAO.totalRegistros(descricao);
    }

    public void load()
    {
        GrupoFinanceiro temp = grupoFinanceiroDAO.load(nome);
        codEmpresa = temp.codEmpresa;
        nome = temp.nome;
        descricao = temp.descricao;
    }

    public List<string> novo()
    {
        List<string> erros = new List<string>();

        if (_nome == "" || _nome == null)
            erros.Add("Descrição está vazia");
        if(erros.Count == 0)
        {
            _nome = new Regex("[" + '\"' + "']").Replace(_nome.Trim(), "");
            grupoFinanceiroDAO.insert(this);
        }

        return erros;
    }
    public List<string> alterar(string nomeAntigo)
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrEmpty(_nome))
            erros.Add("Nome inválido");

        if (string.IsNullOrEmpty(_descricao))
            erros.Add("Descrição está vazia");

        if(erros.Count == 0)
        {
            _nome = new Regex("[" + '\"' + "']").Replace(_nome.Trim(), "");

            grupoFinanceiroDAO.update(this, nomeAntigo);
        }

        return erros;
    }

    public void deletar()
    {
        if (string.IsNullOrEmpty(_nome))
            return;

        grupoFinanceiroDAO.delete(_nome);
    }

    public List<GrupoFinanceiro> listaDescricao()
    {
        return grupoFinanceiroDAO.list();
    }
}