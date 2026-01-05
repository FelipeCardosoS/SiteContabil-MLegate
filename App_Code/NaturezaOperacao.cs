using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class NaturezaOperacao
{
    private naturezaOperacaoDAO naturezaOperacaoDAO;
    private List<string> erros;

    private int _cod_natureza_operacao;
    private string _nome;
    private string _descricao;
    private string _natureza_operacao;
    private int _cod_emitente;
    private bool _padrao;

    public int cod_natureza_operacao
    {
        get { return _cod_natureza_operacao; }
        set { _cod_natureza_operacao = value; }
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

    public string natureza_operacao
    {
        get { return _natureza_operacao; }
        set { _natureza_operacao = value; }
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

    public NaturezaOperacao(Conexao c)
    {
        naturezaOperacaoDAO = new naturezaOperacaoDAO(c);
    }

    public int totalRegistros(string nome, string descricao, string natureza_operacao, Nullable<int> emitente)
    {
        return naturezaOperacaoDAO.totalRegistros(nome, descricao, natureza_operacao, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, string natureza_operacao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        naturezaOperacaoDAO.listaPaginada(ref tb, nome, descricao, natureza_operacao, emitente, paginaAtual, ordenacao);
    }

    public void load()
    {
        DataTable linha = naturezaOperacaoDAO.load(_cod_natureza_operacao);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
            _natureza_operacao = linha.Rows[0]["NATUREZA_OPERACAO"].ToString();
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        naturezaOperacaoDAO.lista_Emitentes_Selecionados(ref tb, _cod_natureza_operacao);
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Natureza da Operação.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Natureza da Operação.");

        if (_natureza_operacao == "" || _natureza_operacao == null)
            erros.Add("Informe a Natureza da Operação (Prefeitura).");

        if (erros.Count == 0)
        {
            _cod_natureza_operacao = naturezaOperacaoDAO.novo(_nome, _descricao, _natureza_operacao);
        }
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_natureza_operacao == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Natureza da Operação.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Natureza da Operação.");

        if (_natureza_operacao == "" || _natureza_operacao == null)
            erros.Add("Informe a Natureza da Operação (Prefeitura).");

        if (erros.Count == 0)
        {
            naturezaOperacaoDAO.alterar(_cod_natureza_operacao, _nome, _descricao, _natureza_operacao);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_natureza_operacao == 0)
            erros.Add("Natureza da Operação " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            naturezaOperacaoDAO.deletar(_cod_natureza_operacao);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        naturezaOperacaoDAO.insert_Emitentes_Selecionados(_cod_natureza_operacao, _cod_emitente, _padrao);
    }

    public void update_Emitentes_Padrao()
    {
        naturezaOperacaoDAO.update_Emitentes_Padrao(_cod_natureza_operacao, _cod_emitente, _padrao);
    }

    public void delete_Emitentes_Deselecionados()
    {
        naturezaOperacaoDAO.delete_Emitentes_Deselecionados(_cod_natureza_operacao, _cod_emitente);
    }

    public void lista_Natureza_Operacao(ref DataTable tb, int cod_emitente)
    {
        naturezaOperacaoDAO.lista_Natureza_Operacao(ref tb, cod_emitente);
    }
}