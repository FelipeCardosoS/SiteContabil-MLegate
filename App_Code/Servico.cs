using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class Servico
{
    private servicosDAO servicoDAO;
    private List<string> erros;

    private int _cod_servico;
    private string _nome;
    private string _cod_servico_prefeitura;
    private string _impostos;
    private int _cod_emitente;

    public int cod_servico
    {
        get { return _cod_servico; }
        set { _cod_servico = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string cod_servico_prefeitura
    {
        get { return _cod_servico_prefeitura; }
        set { _cod_servico_prefeitura = value; }
    }

    public string impostos
    {
        get { return _impostos; }
        set { _impostos = value; }
    }

    public int cod_emitente
    {
        get { return _cod_emitente; }
        set { _cod_emitente = value; }
    }

    public Servico(Conexao c)
    {
        servicoDAO = new servicosDAO(c);
    }

    public int totalRegistros(string nome, string cod_servico_prefeitura, Nullable<double> impostos, Nullable<int> emitente)
    {
        return servicoDAO.totalRegistros(nome, cod_servico_prefeitura, impostos, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, string cod_servico_prefeitura, Nullable<double> impostos, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        servicoDAO.listaPaginada(ref tb, nome, cod_servico_prefeitura, impostos, emitente, paginaAtual, ordenacao);
    }

    public void load()
    {
        DataTable linha = servicoDAO.load(_cod_servico);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _cod_servico_prefeitura = linha.Rows[0]["COD_SERVICO_PREFEITURA"].ToString();
            _impostos = linha.Rows[0]["IMPOSTOS"].ToString();
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        servicoDAO.lista_Emitentes_Selecionados(ref tb, _cod_servico);
    }

    public void Load_Servicos(ref DataTable tb, int Cod_Emitente)
    {
        servicoDAO.Load_Servicos(ref tb, Cod_Emitente);
    }

    public void Load_Servicos(ref DataTable tb)
    {
        servicoDAO.Load_Servicos(ref tb);
    }

    public List<Servico> List_Servico(Conexao c, int Cod_Emitente)
    {
        DataTable tbServico = new DataTable();
        Load_Servicos(ref tbServico, Cod_Emitente);
        List<Servico> lisservico = new List<Servico>();

        foreach (DataRow row in tbServico.Rows)
        {
            lisservico.Add(new Servico(c)
            {
                nome = row["Nome"].ToString(),
                cod_servico = Convert.ToInt32(row["COD_SERVICO"].ToString())
            });
        }

        return lisservico;
    }

    public List<string> List_ServicoEmitente(Conexao c, int Cod_Tributo, int Cod_Emitente)
    {
        servicosDAO _servicosDAO = new servicosDAO(c);
        DataTable tbServico = _servicosDAO.List_ServicoEmitente(Cod_Tributo, Cod_Emitente);
        List<string> lisservicoemitente = new List<string>();

        foreach (DataRow row in tbServico.Rows)
            lisservicoemitente.Add(row["COD_SERVICO"].ToString());

        return lisservicoemitente;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome do Serviço.");

        if (_cod_servico_prefeitura == "" || _cod_servico_prefeitura == null)
            erros.Add("Informe o Código de Serviço (Prefeitura).");

        if (_impostos == "" || _impostos == null)
            erros.Add("Informe a % Aproximada dos Impostos Sobre Vendas da Empresa.");

        if (erros.Count == 0)
            _cod_servico = servicoDAO.novo(_nome, _cod_servico_prefeitura, Convert.ToDouble(_impostos));

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_servico == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome do Serviço.");

        if (_cod_servico_prefeitura == "" || _cod_servico_prefeitura == null)
            erros.Add("Informe o Código de Serviço (Prefeitura).");

        if (_impostos == "" || _impostos == null)
            erros.Add("Informe a % Aproximada dos Impostos Sobre Vendas da Empresa.");

        if (erros.Count == 0)
            servicoDAO.alterar(_cod_servico, _nome, _cod_servico_prefeitura, Convert.ToDouble(_impostos));

        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_servico == 0)
            erros.Add("Serviço " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            servicoDAO.deletar(_cod_servico);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        servicoDAO.insert_Emitentes_Selecionados(_cod_servico, _cod_emitente);
    }

    public void delete_Emitentes_Deselecionados()
    {
        servicoDAO.delete_Emitentes_Deselecionados(_cod_servico, _cod_emitente);
    }

    public void lista_Servicos(ref DataTable tb, int cod_emitente)
    {
        servicoDAO.lista_Servicos(ref tb, cod_emitente);
    }
}