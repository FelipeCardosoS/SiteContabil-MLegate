using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.IO;

public class ContaContabil
{
    private string _codigo;
    private int _grupoContabil;
    private string _descricao;
    private char _debitoCredito;
    private string _contaSintetica;
    private int _analitica;
    private string _tipo;
    private int _jobDefault;
    private string _tipoImposto;
    private bool _geraCredito;
    private bool _retido;
    private bool _recibo;
    private string _codClassificacao;
    private string _codContaRef;
    private string _codContaDRE;
    private string _codContaBalanco;
    private string _codContaRefEcf;
    private int _codMoedaBalanco;
    private int _codMoedaMovimento;
    private int _cta;
    private int _job;

    private bool _ctaValida; // Verifico se o CTA está marcado como true no banco, assim evito o UPDATE para atualizar todas as linhas

    private contasDAO contaDAO;
    private tiposContaDAO tipoContaDAO;
    private List<string> erros;
    private DataTable loadPlanosContas;

    private string _Cod_Dre;
    private string _Cod_Balanco;

    public bool ctaValida 
    {
        get { return _ctaValida; }
        set { _ctaValida = value; }
    }

    public string codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public int cta
    {
        get { return _cta; }
        set { _cta = value; }
    }

    public int codMoedaBalanco
    {
        get { return _codMoedaBalanco; }
        set { _codMoedaBalanco = value; }
    }

    public int codMoedaMovimento
    {
        get { return _codMoedaMovimento; }
        set { _codMoedaMovimento = value; }
    }

    public int  grupoContabil
    {
        get { return _grupoContabil; }
        set { _grupoContabil = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public char debitoCredito
    {
        get { return _debitoCredito; }
        set { _debitoCredito = value; }
    }

    public string contaSintetica
    {
        get { return _contaSintetica; }
        set { _contaSintetica = value; }
    }

    public int analitica
    {
        get { return _analitica; }
        set { _analitica = value; }
    }

    public string tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }

    public int jobDefault
    {
        get { return _jobDefault; }
        set { _jobDefault = value; }
    }

    public string tipoImposto
    {
        get { return _tipoImposto; }
        set { _tipoImposto = value; }
    }

    public bool geraCredito
    {
        get { return _geraCredito; }
        set { _geraCredito = value; }
    }

    public bool retido
    {
        get { return _retido; }
        set { _retido = value; }
    }

    public bool recibo
    {
        get { return _recibo; }
        set { _recibo = value; }
    }

    public string codClassificacao
    {
        get { return _codClassificacao; }
        set { _codClassificacao = value; }
    }

    public string codContaRef
    {
        get { return _codContaRef; }
        set { _codContaRef = value; }
    }
    public string codContaDRE
    {
        get { return _codContaDRE; }
        set { _codContaDRE = value; }
    }

    public string codContaBalanco
    {
        get { return _codContaBalanco; }
        set { _codContaBalanco = value; }
    }
    public string codContaRefEcf
    {
        get { return _codContaRefEcf; }
        set { _codContaRefEcf = value; }
    }

    public int job
    {
        get { return _job; }
        set { _job = value; }
    }

    public string CodContaDRE
    {
        get { return _codContaDRE; }
        set { _codContaDRE = value; }
    }

    public string CodContaBalanco
    {
        get { return _codContaBalanco; }
        set { _codContaBalanco = value; }
    }

    public ContaContabil(Conexao c)
	{
        contaDAO = new contasDAO(c);
        tipoContaDAO = new tiposContaDAO(c);
	}


    public int PlanoConta(int empresa) 
    {
        return contaDAO.planodecontas(empresa);
    }

    public ContaContabil(Conexao c, string codigo)
        :this(c)
    {
        _codigo = codigo;
    }

    public ContaContabil(Conexao c, string codigo, int grupoContabil,string descricao, char debitoCredito,
        string contaSintetica, int analitica, string tipo, string codClassificacao, string codContaRef)
        : this(c)
    {
        _codigo = codigo;
        _grupoContabil = grupoContabil;
        _descricao = descricao;
        _debitoCredito = debitoCredito;
        _contaSintetica = contaSintetica;
        _analitica = analitica;
        _tipo = tipo;
        _codClassificacao = codClassificacao;
        _codContaRef = codContaRef;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código Inválido.");

        if (_grupoContabil == 0)
            erros.Add("Grupo Contábil Inválido.");

        if (_debitoCredito.ToString() == "")
            erros.Add("Conta deve ser de Débito ou Crédito.");

        if (_tipo == "")
            erros.Add("Conta deve ser Receber ou Pagar.");

        if(_cta == 1 && _job == 0)
            erros.Add("A conta CTA deve ter obrigatoriamente um Job selecionado.");

        if (_cta == 1 && _codMoedaBalanco != 2)
            erros.Add("A conta CTA exige que a Moeda de Balanço seja 'Histórico'.");

        //if (_analitica == 1 && string.IsNullOrEmpty(_codContaBalanco))
        //    erros.Add("Para contas analiticas é obrigatório identicar uma conta de Balanco");

        if (erros.Count == 0)
        {
            if (Convert.ToBoolean(cta) && !ctaValida) contaDAO.zeraCTA();

            contaDAO.insert((int)HttpContext.Current.Session["empresa"], _codigo, _grupoContabil, _descricao, _debitoCredito, _contaSintetica, _analitica,
                _tipo, _jobDefault, _job, _tipoImposto, _geraCredito, _retido, _recibo, _codClassificacao, _codContaRef, _codContaRefEcf, _codMoedaBalanco, _codMoedaMovimento, _cta, _codContaDRE, _codContaBalanco );

            loadPlanosContas = contaDAO.loadPlanosContas();
            foreach (DataRow row in loadPlanosContas.Rows)
            {
                contaDAO.insert((int)row["COD_EMPRESA"], _codigo, _grupoContabil, _descricao, _debitoCredito, _contaSintetica, _analitica,
                    _tipo, _jobDefault, _job, _tipoImposto, _geraCredito, _retido, _recibo, _codClassificacao, _codContaRef, _codContaRefEcf, _codMoedaBalanco, _codMoedaMovimento, _cta, _codContaDRE, _codContaBalanco);
            }
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código Inválido.");

        if (_grupoContabil == 0)
            erros.Add("Grupo Contábil Inválido.");

        if (_debitoCredito.ToString() == "")
            erros.Add("Conta deve ser de Débito ou Crédito.");

        if (_tipo == "")
            erros.Add("Conta deve ser Receber ou Pagar.");

        if (_cta == 1 && _job == 0)
            erros.Add("A conta CTA deve ter obrigatoriamente um Job selecionado.");

        if (_cta == 1 && _codMoedaBalanco != 2)
            erros.Add("A conta CTA exige que a Moeda de Balanço seja Histórico.");

        if (erros.Count == 0)
        {
            if (Convert.ToBoolean(cta) && !ctaValida) contaDAO.zeraCTA();
            
            contaDAO.update((int)HttpContext.Current.Session["empresa"], _codigo, _grupoContabil, _descricao, _debitoCredito, _contaSintetica, _analitica,
                _tipo, _jobDefault, _job, _tipoImposto, _geraCredito, _retido, _recibo, _codClassificacao, _codContaRef, _codContaRefEcf, _codMoedaBalanco, _codMoedaMovimento, _cta, _codContaDRE, _codContaBalanco);
            
            loadPlanosContas = contaDAO.loadPlanosContas();
            foreach (DataRow row in loadPlanosContas.Rows)
            {
                contaDAO.update((int)row["COD_EMPRESA"], _codigo, _grupoContabil, _descricao, _debitoCredito, _contaSintetica, _analitica,
                    _tipo, _jobDefault, _job, _tipoImposto, _geraCredito, _retido, _recibo, _codClassificacao, _codContaRef, _codContaRefEcf, _codMoedaBalanco, _codMoedaMovimento, _cta, _codContaDRE, _codContaBalanco);
            }
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código Inválido.");

        if (erros.Count == 0)
        {
            contaDAO.delete((int)HttpContext.Current.Session["empresa"], _codigo);
            
            loadPlanosContas = contaDAO.loadPlanosContas();
            foreach (DataRow row in loadPlanosContas.Rows)
            {
                contaDAO.delete((int)row["COD_EMPRESA"], _codigo);
            }
        }

        return erros;
    }

    public List<string> load()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código Inválido.");

        if (erros.Count == 0)
        {
            DataTable linha = contaDAO.load(_codigo);

            if (linha.Rows.Count > 0)
            {
                _grupoContabil = Convert.ToInt32(linha.Rows[0]["COD_GRUPO_CONTABIL"]);
                _descricao = linha.Rows[0]["DESCRICAO"].ToString();
                _debitoCredito = Convert.ToChar(linha.Rows[0]["DEBITO_CREDITO"]);
                _contaSintetica = linha.Rows[0]["COD_CONTA_SINTETICA"].ToString();
                _analitica = Convert.ToInt32(linha.Rows[0]["ANALITICA"]);
                _tipo = linha.Rows[0]["COD_TIPO_CONTA"].ToString();
                _jobDefault = Convert.ToInt32(linha.Rows[0]["COD_JOB_DEFAULT"]);
                _tipoImposto = linha.Rows[0]["TIPO_IMPOSTO"].ToString();
                _geraCredito = Convert.ToBoolean(Convert.ToInt32(linha.Rows[0]["GERA_CREDITO"]));
                _retido = Convert.ToBoolean(Convert.ToInt32(linha.Rows[0]["RETIDO"]));
                _recibo = Convert.ToBoolean(Convert.ToInt32(linha.Rows[0]["RECIBO"]));
                _codClassificacao = linha.Rows[0]["COD_CLASSIFICACAO"].ToString();
                _codContaRef = linha.Rows[0]["COD_CONTA_REF"].ToString();
                _codMoedaBalanco = Convert.ToInt32(linha.Rows[0]["COD_MOEDA_BALANCO"]);
                _codMoedaMovimento = Convert.ToInt32(linha.Rows[0]["COD_MOEDA_MOVIMENTO"]);
                _cta = Convert.ToInt32(linha.Rows[0]["CTA"]);
                _job = Convert.ToInt32(linha.Rows[0]["COD_JOB"]);
                _codContaDRE = linha.Rows[0]["cod_dre"].ToString();
                _codContaBalanco = linha.Rows[0]["cod_balanco"].ToString();
            }
        }
        return erros;
    }

    public bool existe(string codigo)
    {
        return contaDAO.existe(codigo);
    }

    public void listaAnaliticas(ref DataTable tb)
    {
        contaDAO.lista(ref tb, 1);
    }

    public void listaAnaliticas(ref DataTable tb, int grupoContabil)
    {
        contaDAO.lista(ref tb, 1, grupoContabil);
    }

    public void listaAnaliticas(ref DataTable tb, bool PL)
    {
        contaDAO.lista(ref tb, 1, PL);
    }
    public void listaAnaliticas(ref DataTable tb, string tipoConta)
    {
        contaDAO.lista(ref tb, 1, tipoConta);
    }

    public void listaSinteticas(ref DataTable tb)
    {
        contaDAO.lista(ref tb, 0);
    }

    public List<Hashtable> lista()
    {
        return contaDAO.lista();
    }

    public List<Hashtable> lista(string tipo)
    {
        return contaDAO.lista(tipo);
    }

    public List<Hashtable> listaDiferenteDe(string tipo)
    {
        return contaDAO.listaDiferenteDe(tipo);
    }

    public void lista(ref DataTable tb)
    {
        contaDAO.lista(ref tb);
    }

    public void listaTipos(ref DataTable tb)
    {
        tipoContaDAO.lista(ref tb);
    }

    public void listaPaginada(ref DataTable tb, Nullable<int>grupo, string codigo,string descricao,Nullable<char>debCred,
        Nullable<int> analitica, string tipo, int paginaAtual, string ordenacao)
    {
        contaDAO.lista(ref tb, grupo, codigo, descricao, debCred, analitica, tipo, paginaAtual, ordenacao);
    }

    public int totalRegistros(Nullable<int> grupo, string codigo, string descricao, Nullable<char> debCred,
        Nullable<int> analitica, string tipo)
    {
        return contaDAO.totalRegistros(grupo, codigo, descricao, debCred, analitica, tipo);
    }

    public void listaContasAnaliticas(ref DataTable tb)
    {
        contaDAO.listaContasAnaliticas(ref tb);
    }
}