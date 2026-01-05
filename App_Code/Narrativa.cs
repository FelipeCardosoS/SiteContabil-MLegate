using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class Narrativa
{
    private narrativasDAO narrativaDAO;
    private List<string> erros;

    private int _cod_narrativa;
    private string _nome;
    private string _descricao;
    private int _cod_emitente;
    private bool _padrao;

    public int cod_narrativa
    {
        get { return _cod_narrativa; }
        set { _cod_narrativa = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public int cod_emitente
    {
        get { return _cod_emitente; }
        set { _cod_emitente = value; }
    }

    public bool padrao
    {
        get { return _padrao; }
        set { _padrao = value; }
    }

    public Narrativa(Conexao c)
    {
        narrativaDAO = new narrativasDAO(c);
    }

    public int totalRegistros(string nome, string descricao, Nullable<int> emitente)
    {
        return narrativaDAO.totalRegistros(nome, descricao, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        narrativaDAO.listaPaginada(ref tb, nome, descricao, emitente, paginaAtual, ordenacao);
    }

    public void load()
    {
        DataTable linha = narrativaDAO.load(_cod_narrativa);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        narrativaDAO.lista_Emitentes_Selecionados(ref tb, _cod_narrativa);
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Narrativa.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Narrativa.");

        if (erros.Count == 0)
        {
            _cod_narrativa = narrativaDAO.novo(_nome, _descricao);
        }
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_narrativa == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Narrativa.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Narrativa.");

        if (erros.Count == 0)
        {
            narrativaDAO.alterar(_cod_narrativa, _nome, _descricao);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_narrativa == 0)
            erros.Add("Narrativa " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            narrativaDAO.deletar(_cod_narrativa);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        narrativaDAO.insert_Emitentes_Selecionados(_cod_narrativa, _cod_emitente, _padrao);
    }

    public void update_Emitentes_Padrao()
    {
        narrativaDAO.update_Emitentes_Padrao(_cod_narrativa, _cod_emitente, _padrao);
    }

    public void delete_Emitentes_Deselecionados()
    {
        narrativaDAO.delete_Emitentes_Deselecionados(_cod_narrativa, _cod_emitente);
    }

    public void lista_Narrativas(ref DataTable tb, int cod_emitente)
    {
        narrativaDAO.lista_Narrativas(ref tb, cod_emitente);
    }
}