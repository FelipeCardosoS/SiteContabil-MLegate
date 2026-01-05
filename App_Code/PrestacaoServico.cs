using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class PrestacaoServico
{
    private prestacaoServicosDAO prestacaoServicoDAO;
    private List<string> erros;

    private int _cod_prestacao_servico;
    private string _nome;
    private string _descricao;
    private int _cod_emitente;
    private bool _padrao;

    public int cod_prestacao_servico
    {
        get { return _cod_prestacao_servico; }
        set { _cod_prestacao_servico = value; }
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

    public PrestacaoServico(Conexao c)
    {
        prestacaoServicoDAO = new prestacaoServicosDAO(c);
    }

    public int totalRegistros(string nome, string descricao, Nullable<int> emitente)
    {
        return prestacaoServicoDAO.totalRegistros(nome, descricao, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        prestacaoServicoDAO.listaPaginada(ref tb, nome, descricao, emitente, paginaAtual, ordenacao);
    }

    public void load()
    {
        DataTable linha = prestacaoServicoDAO.load(_cod_prestacao_servico);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        prestacaoServicoDAO.lista_Emitentes_Selecionados(ref tb, _cod_prestacao_servico);
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Prestação de Serviço.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Prestação de Serviço.");

        if (erros.Count == 0)
        {
            _cod_prestacao_servico = prestacaoServicoDAO.novo(_nome, _descricao);
        }
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_prestacao_servico == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Prestação de Serviço.");

        if (_descricao == "" || _descricao == null)
            erros.Add("Informe a Descrição da Prestação de Serviço.");

        if (erros.Count == 0)
        {
            prestacaoServicoDAO.alterar(_cod_prestacao_servico, _nome, _descricao);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_prestacao_servico == 0)
            erros.Add("Prestação de Serviço " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            prestacaoServicoDAO.deletar(_cod_prestacao_servico);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        prestacaoServicoDAO.insert_Emitentes_Selecionados(_cod_prestacao_servico, _cod_emitente, _padrao);
    }

    public void update_Emitentes_Padrao()
    {
        prestacaoServicoDAO.update_Emitentes_Padrao(_cod_prestacao_servico, _cod_emitente, _padrao);
    }

    public void delete_Emitentes_Deselecionados()
    {
        prestacaoServicoDAO.delete_Emitentes_Deselecionados(_cod_prestacao_servico, _cod_emitente);
    }

    public void lista_Prestacao_Servicos(ref DataTable tb, int cod_emitente)
    {
        prestacaoServicoDAO.lista_Prestacao_Servicos(ref tb, cod_emitente);
    }
}