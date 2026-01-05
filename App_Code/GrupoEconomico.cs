using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Descrição resumida de GrupoEconomico
/// </summary>
public class GrupoEconomico
{

    private int _codigoGrupoEconomico;
    private int _codEmpresa;
    private string _descricao;

    private GrupoEconomicoDAO grupoEconomicoDAO;

    public GrupoEconomico(Conexao c)
    {
        grupoEconomicoDAO = new GrupoEconomicoDAO(c);
    }

    public GrupoEconomico() {}

    public int codigoGrupoEconomico
    {
        get
        {
            return _codigoGrupoEconomico;
        }

        set
        {
            _codigoGrupoEconomico = value;
        }
    }

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
        grupoEconomicoDAO.list(ref table, pagina, descricao, ordenacao);
    }
    public int totalRegistros(string descricao)
    {
        return grupoEconomicoDAO.totalRegistros(descricao);
    }

    public void load()
    {
        GrupoEconomico temp = grupoEconomicoDAO.load(codigoGrupoEconomico);
        codEmpresa = temp.codEmpresa;
        descricao = temp.descricao;
    }

    public List<string> novo()
    {
        List<string> erros = new List<string>();

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição está vazia");
        if(erros.Count == 0)
        {
            _descricao = new Regex("[" + '\"' + "']").Replace(_descricao.Trim(), "");
            grupoEconomicoDAO.insert(this);
        }

        return erros;
    }
    public List<string> alterar()
    {
        List<string> erros = new List<string>();

        if (_codigoGrupoEconomico == 0) return erros;

        if (_codigoGrupoEconomico < 0)
            erros.Add("Código inválido");

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição está vazia");

        if(erros.Count == 0)
        {
            _descricao = new Regex("[" + '\"' + "']").Replace(_descricao.Trim(), "");

            grupoEconomicoDAO.update(this);
        }

        return erros;
    }

    public void deletar()
    {
        if (_codigoGrupoEconomico == 0)
            return;

        grupoEconomicoDAO.delete(_codigoGrupoEconomico);
    }

    public List<GrupoEconomico> listaDescricao()
    {
        return grupoEconomicoDAO.list();
    }
}