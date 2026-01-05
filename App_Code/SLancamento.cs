using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class SLancamento
{
    private double? _lote;
    private int _seqLote;
    private int? _detLote;
    private double? _lotePai;
    private int? _seqLotePai;
    private double? _duplicata;
    private double? _seqBaixa;
    private double? _detBaixa;
    private char? _debCred;
    private DateTime? _dataLancamento;
    private string _conta;
    private string _descConta;
    private int? _job;
    private string _descJob;
    private int? _linhaNegocio;
    private string _descLinhaNegocio;
    private int? _divisao;
    private string _descDivisao;
    private int? _cliente;
    private string _descCliente;
    private decimal? _qtd;
    private decimal? _valorUnit;
    private decimal? _valorBruto = 0;
    private decimal? _valor;
    private string _historico;
    private bool? _titulo;
    private List<SVencimento> _vencimentos;
    private List<CotacaoItem> _cotacao;
    private int? _terceiro;
    private string _descTerceiro;
    private bool? _pendente;
    private int? _modelo;
    private string _strData;
    private string _modulo;
    private string _numeroDocumento;
    private string _codOrigem;
    private string _seqOrigem;
    private bool _efetivado;
    private string _tipoLancto;
    private int _usuario;
    private string _descConsultor;
    private int _codConsultor;
    private int _codPlanilha;
    private int? _valorGrupo = 0;
    private int _codMoeda = 0;

    public string numeroDocumento
    {
        get { return _numeroDocumento; }
        set { _numeroDocumento = value; }
    }

    public string codOrigem
    {
        get { return _codOrigem; }
        set { _codOrigem = value; }
    }

    public string seqOrigem
    {
        get { return _seqOrigem; }
        set { _seqOrigem = value; }
    }

    public bool efetivado
    {
        get { return _efetivado; }
        set { _efetivado = value; }
    }

    public Nullable<double> lote
    {
        get { return _lote; }
        set { _lote = value; }
    }

    public int seqLote
    {
        get { return _seqLote; }
        set { _seqLote = value; }
    }

    public Nullable<int> detLote
    {
        get { return _detLote; }
        set { _detLote = value; }
    }

    public Nullable<double> lotePai
    {
        get { return _lotePai; }
        set { _lotePai = value; }
    }

    public Nullable<int> seqLotePai
    {
        get { return _seqLotePai; }
        set { _seqLotePai = value; }
    }

    public Nullable<double> duplicata
    {
        get { return _duplicata; }
        set { _duplicata = value; }
    }

    public Nullable<double> seqBaixa
    {
        get { return _seqBaixa; }
        set { _seqBaixa = value; }
    }

    public Nullable<double> detBaixa
    {
        get { return _detBaixa; }
        set { _detBaixa = value; }
    }

    public Nullable<char> debCred
    {
        get { return _debCred; }
        set { _debCred = value; }
    }

    public Nullable<DateTime> dataLancamento
    {
        get { return _dataLancamento; }
        set { _dataLancamento = value; }
    }

    public string conta
    {
        get { return _conta; }
        set { _conta = value; }
    }

    public string descConta
    {
        get { return _descConta; }
        set { _descConta = value; }
    }

    public Nullable<int> job
    {
        get { return _job; }
        set { _job = value; }
    }

    public string descJob
    {
        get { return _descJob; }
        set { _descJob = value; }
    }

    public Nullable<int> linhaNegocio
    {
        get { return _linhaNegocio; }
        set { _linhaNegocio = value; }
    }

    public string descLinhaNegocio
    {
        get { return _descLinhaNegocio; }
        set { _descLinhaNegocio = value; }
    }

    public Nullable<int> divisao
    {
        get { return _divisao; }
        set { _divisao = value; }
    }

    public string descDivisao
    {
        get { return _descDivisao; }
        set { _descDivisao = value; }
    }

    public Nullable<int> cliente
    {
        get { return _cliente; }
        set { _cliente = value; }
    }

    public string descCliente
    {
        get { return _descCliente; }
        set { _descCliente = value; }
    }

    public Nullable<int> terceiro
    {
        get { return _terceiro; }
        set { _terceiro = value; }
    }

    public string descTerceiro
    {
        get { return _descTerceiro; }
        set { _descTerceiro = value; }
    }

    public Nullable<decimal> qtd
    {
        get { return _qtd; }
        set { _qtd = value; }
    }

    public Nullable<decimal> valorUnit
    {
        get { return _valorUnit; }
        set { _valorUnit = value; }
    }

    public Nullable<decimal> valorBruto
    {
        get { return _valorBruto; }
        set { _valorBruto = value; }
    }

    public Nullable<decimal> valor
    {
        get { return _valor; }
        set { _valor = value; }
    }

    public string historico
    {
        get { return _historico; }
        set { _historico = value; }
    }

    public Nullable<bool> titulo
    {
        get { return _titulo; }
        set { _titulo = value; }
    }

    public List<SVencimento> vencimentos
    {
        get { return _vencimentos; }
        set { _vencimentos = value; }
    }

    public List<CotacaoItem> cotacao
    {
        get { return _cotacao; }
        set { _cotacao = value; }
    }

    public Nullable<bool> pendente
    {
        get { return _pendente; }
        set { _pendente = value; }
    }

    public Nullable<int> modelo
    {
        get { return _modelo; }
        set { _modelo = value; }
    }

    public string strData
    {
        get { return _strData; }
        set { _strData = value; }
    }

    public string dataLancamento_formatada
    {
        get 
        {
            if (_dataLancamento != null)
                return _dataLancamento.Value.ToString("dd/MM/yyyy");
            else
                return "";
        }
    }

    public string modulo
    {
        get { return _modulo; }
        set { _modulo = value; }
    }

    public string tipoLancto
    {
        get { return _tipoLancto; }
        set { _tipoLancto = value; }
    }

    public int usuario
    {
        get { return _usuario; }
        set { _usuario = value; }
    }

    public string descConsultor
    {
        get { return _descConsultor ; }
        set { _descConsultor = value; }
    }

    public int codConsultor
    {
        get { return _codConsultor; }
        set { _codConsultor = value; }
    }

    public int codPlanilha
    {
        get { return _codPlanilha; }
        set { _codPlanilha = value; }
    }

    public Nullable<int> valorGrupo
    {
        get { return _valorGrupo; }
        set { _valorGrupo = value; }
    }

    public int codMoeda
    {
        get { return _codMoeda; }
        set { _codMoeda = value; }
    }

    public SLancamento()
    {
    }

    public SLancamento(Nullable<int> lote, int seqLote, Nullable<int> detLote, Nullable<double> lotePai,
        Nullable<int> seqLotePai, Nullable<double> duplicata, Nullable<double> seqBaixa, Nullable<double> detBaixa, Nullable<char> debCred, Nullable<DateTime> dataLancamento,
        string conta, string descConta, Nullable<int> job, string descJob, Nullable<int> linhaNegocio, string descLinhaNegocio, Nullable<int> divisao,
        string descDivisao, Nullable<int> cliente, string descCliente, Nullable<decimal> qtd,Nullable<decimal> valorUnit,Nullable<decimal> valor, string historico,
        Nullable<bool> titulo, List<SVencimento> vencimentos, Nullable<int> terceiro, string descTerceiro, Nullable<bool> pendente,
        Nullable<int> modelo, string numeroDocumento, string descConsultor, int codConsultor, Nullable<decimal> valorBruto, int valorGrupo, int codMoeda)
	{
        _lote = lote;
        _seqLote = seqLote;
        _detLote = detLote;
        _lotePai = lotePai;
        _seqLotePai = seqLotePai;
        _duplicata = duplicata;
        _seqBaixa = seqBaixa;
        _detBaixa = detBaixa;
        _debCred = debCred;
        _dataLancamento = dataLancamento;
        _conta = conta;
        _descConta = descConta;
        _job = job;
        _descJob = descJob;
        _linhaNegocio = linhaNegocio;
        _descLinhaNegocio = descLinhaNegocio;
        _divisao = divisao;
        _descDivisao = descDivisao;
        _cliente = cliente;
        _descCliente = descCliente;
        _qtd = qtd;
        _valorUnit = valorUnit;
        _valorBruto = valorBruto;
        _valor = valor;
        _historico = historico;
        _titulo = titulo;
        _vencimentos = vencimentos;
        _pendente = pendente;
        _terceiro = terceiro;
        _descTerceiro = descTerceiro;
        _modelo = modelo;
        _numeroDocumento = numeroDocumento;
        _descConsultor = descConsultor;
        _codConsultor = codConsultor;
        _valorGrupo = valorGrupo;
        _codMoeda = codMoeda;
	}

    public SLancamento(Nullable<int> lote, int seqLote, Nullable<int> detLote, Nullable<double> lotePai,
        Nullable<int> seqLotePai, Nullable<double> duplicata, Nullable<double> seqBaixa, Nullable<double> detBaixa, Nullable<char> debCred, Nullable<DateTime> dataLancamento,
        string conta, string descConta, Nullable<int> job, string descJob, Nullable<int> linhaNegocio, string descLinhaNegocio, Nullable<int> divisao,
        string descDivisao, Nullable<int> cliente, string descCliente, Nullable<decimal> qtd, Nullable<decimal> valorUnit, Nullable<decimal> valor, string historico,
        Nullable<bool> titulo, List<SVencimento> vencimentos, Nullable<int> terceiro, string descTerceiro, Nullable<bool> pendente, Nullable<int> modelo, string numeroDocumento,
        string tipoLancto, string descConsultor, int codConsultor, Nullable<decimal> valorBruto, int valorGrupo, int codMoeda)
    {
        _lote = lote;
        _seqLote = seqLote;
        _detLote = detLote;
        _lotePai = lotePai;
        _seqLotePai = seqLotePai;
        _duplicata = duplicata;
        _seqBaixa = seqBaixa;
        _detBaixa = detBaixa;
        _debCred = debCred;
        _dataLancamento = dataLancamento;
        _conta = conta;
        _descConta = descConta;
        _job = job;
        _descJob = descJob;
        _linhaNegocio = linhaNegocio;
        _descLinhaNegocio = descLinhaNegocio;
        _divisao = divisao;
        _descDivisao = descDivisao;
        _cliente = cliente;
        _descCliente = descCliente;
        _qtd = qtd;
        _valorUnit = valorUnit;
        _valorBruto = valorBruto;
        _valor = valor;
        _historico = historico;
        _titulo = titulo;
        _vencimentos = vencimentos;
        _pendente = pendente;
        _terceiro = terceiro;
        _descTerceiro = descTerceiro;
        _modelo = modelo;
        _numeroDocumento = numeroDocumento;
        _tipoLancto = tipoLancto;
        _descConsultor = descConsultor;
        _codConsultor = codConsultor;
        _valorGrupo = valorGrupo;
        _codMoeda = codMoeda;
    }
}
