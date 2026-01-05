using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
//using ExtremeML.Packaging;
using System.IO;
using System.Data.SqlClient;
using System.Data;
//using System.Linq;


public partial class Ts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public class InputData
    {
        private int? _lote; 
        private int _seqLote; 
        private int? _detLote; 
        private double? _lotePai; 
        private int? _seqLotePai; 
        private double? _duplicata; 
        private double? _seqBaixa; 
        private double? _detBaixa; 
        private char? _debCred; 
        private string _dataLancamento; 
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
        private decimal? _valorBruto; 
        private decimal? _valor; 
        private string _historico; 
        private bool? _titulo; 
        private object _vencimentos; 
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
        private string _descConsultor;
        private int _codConsultor;

        public int? lote { get { return _lote; } set { _lote = value; } }
        public int seqLote { get { return _seqLote; } set { _seqLote = value; } }
        public int? detLote { get { return _detLote; } set { _detLote = value; } }
        public double? lotePai { get { return _lotePai; } set { _lotePai = value; } }
        public int? seqLotePai { get { return _seqLotePai; } set { _seqLotePai = value; } }
        public double? duplicata { get { return _duplicata; } set { _duplicata = value; } }
        public double? seqBaixa { get { return _seqBaixa; } set { _seqBaixa = value; } }
        public double? detBaixa { get { return _detBaixa; } set { _detBaixa = value; } }
        public char? debCred { get { return _debCred; } set { _debCred = value; } }
        public string dataLancamento { get { return _dataLancamento; } set { _dataLancamento = value; } }
        public string conta { get { return _conta; } set { _conta = value; } }
        public string descConta { get { return _descConta; } set { _descConta = value; } }
        public int? job { get { return _job; } set { _job = value; } }
        public string descJob { get { return _descJob; } set { _descJob = value; } }
        public int? linhaNegocio { get { return _linhaNegocio; } set { _linhaNegocio = value; } }
        public string descLinhaNegocio { get { return _descLinhaNegocio; } set { _descLinhaNegocio = value; } }
        public int? divisao { get { return _divisao; } set { _divisao = value; } }
        public string descDivisao { get { return _descDivisao; } set { _descDivisao = value; } }
        public int? cliente { get { return _cliente; } set { _cliente = value; } }
        public string descCliente { get { return _descCliente; } set { _descCliente = value; } }
        public decimal? qtd { get { return _qtd; } set { _qtd = value; } }
        public decimal? valorUnit { get { return _valorUnit; } set { _valorUnit = value; } }
        public decimal? valorBruto { get { return _valorBruto; } set { _valorBruto = value; } }
        public decimal? valor { get { return _valor; } set { _valor = value; } }
        public string historico { get { return _historico; } set { _historico = value; } }
        public bool? titulo { get { return _titulo; } set { _titulo = value; } }
        public object vencimentos { get { return _vencimentos; } set { _vencimentos = value; } }
        public int? terceiro { get { return _terceiro; } set { _terceiro = value; } }
        public string descTerceiro { get { return _descTerceiro; } set { _descTerceiro = value; } }
        public bool? pendente { get { return _pendente; } set { _pendente = value; } }
        public int? modelo { get { return _modelo; } set { _modelo = value; } }
        public string strData { get { return _strData; } set { _strData = value; } }
        public string modulo { get { return _modulo; } set { _modulo = value; } }
        public string numeroDocumento { get { return _numeroDocumento; } set { _numeroDocumento = value; } }
        public string codOrigem { get { return _codOrigem; } set { _codOrigem = value; } }
        public string seqOrigem { get { return _seqOrigem; } set { _seqOrigem = value; } }
        public bool efetivado { get { return _efetivado; } set { _efetivado = value; } }
        public string descConsultor { get { return _descConsultor; } set { _descConsultor = value; } }
        public int codConsultor { get { return _codConsultor; } set { _codConsultor = value; } }
    }

    [WebMethod]
    public static List<string> esperandoCorcao(List<InputData> folha)
    {
        Conexao c = new Conexao();
        
        List<SLancamento> lsl = new List<SLancamento>();
        SLancamento sl = new SLancamento();
        List<string> erros = new List<string>();

        //MLEGATE
        string empresajson = System.Configuration.ConfigurationManager.AppSettings["empresajson"];
        string usuariojson = System.Configuration.ConfigurationManager.AppSettings["usuariojson"];
        HttpContext.Current.Session["empresa"] = empresajson;
        HttpContext.Current.Session["usuario"] = usuariojson;

        //FBM
        //HttpContext.Current.Session["empresa"] = 6211;
        //HttpContext.Current.Session["usuario"] = 17;

        DateTime data = DateTime.Now;

        for (int i = 0; i < folha.Count; i++)
        {
            if (folha[i].dataLancamento != "")
            {
                data = Convert.ToDateTime(folha[i].dataLancamento);
            }

            if (folha[i].terceiro == null)
                folha[i].terceiro = 0;

            object dt = null;
            object vl = null;
            
            List<SVencimento> lsv = new List<SVencimento>();
            if (folha[i].modulo == "")
                folha[i].modulo = "CAP_INCLUSAO_TITULO";

            if (folha[i].vencimentos != null)
            {

                Dictionary<string, object> tmp = (Dictionary<string, object>)folha[i].vencimentos;
                tmp.TryGetValue("data", out dt);
                tmp.TryGetValue("valor", out vl);
                SVencimento sv = new SVencimento(Convert.ToDateTime(dt),Convert.ToDecimal(vl));
                lsv.Add(sv);

            }

            ContaContabil conta = new ContaContabil(c);
            conta.codigo = folha[i].conta;
            conta.load();
            folha[i].descConta = conta.codigo + " - " + conta.descricao;

            Job job = new Job(c);
            try
            {
                job.dePara(folha[i].job.Value);
                folha[i].job = job.codigo;
                Cliente cliente1 = new Cliente(c);
                cliente1.codigo = job.cliente;
                cliente1.load();
                folha[i].descJob = cliente1.nomeFantasia + " - " + job.descricao;
                //folha[i].cliente = job.cliente;
                folha[i].descCliente = cliente1.nomeFantasia;
                folha[i].descJob = cliente1.nomeFantasia + " - " + job.descricao;
                folha[i].linhaNegocio = job.linhaNegocio;
                folha[i].divisao = job.divisao;
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            Cliente cliente = new Cliente(c);
            try
            {
                cliente.dePara(folha[i].cliente.Value);
                folha[i].cliente = cliente.codigo;
                folha[i].descCliente = cliente.nomeFantasia;
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            LinhaNegocio linhaNegocio = new LinhaNegocio(c);
            try
            {
                linhaNegocio.dePara(folha[i].linhaNegocio.Value);
                folha[i].linhaNegocio = linhaNegocio.codigo;
                folha[i].descLinhaNegocio = linhaNegocio.descricao;
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            Divisao divisao = new Divisao(c);
            try
            {
                divisao.dePara(folha[i].divisao.Value);
                folha[i].divisao = divisao.codigo;
                folha[i].descDivisao = divisao.descricao;
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            if (folha[i].seqLote == 1)
            {
                Empresa emp = new Empresa(c);
                emp.deParaConsultor(folha[i].terceiro.Value);
                folha[i].terceiro = emp.codigo;
                folha[i].descTerceiro = emp.nome;
            }

            sl = new SLancamento(folha[i].lote, folha[i].seqLote, folha[i].detLote, folha[i].lotePai, folha[i].seqLotePai, folha[i].duplicata, folha[i].seqBaixa,
            folha[i].detBaixa, folha[i].debCred, Convert.ToDateTime(folha[i].dataLancamento), folha[i].conta, folha[i].descConta, folha[i].job, folha[i].descJob,
            folha[i].linhaNegocio, folha[i].descLinhaNegocio, folha[i].divisao, folha[i].descDivisao, folha[i].cliente, folha[i].descCliente,
            folha[i].qtd, folha[i].valorUnit, folha[i].valor, folha[i].historico, folha[i].titulo, lsv, folha[i].terceiro,
            folha[i].descTerceiro, folha[i].pendente, folha[i].modelo, folha[i].numeroDocumento, folha[i].descConsultor, folha[i].codConsultor, 0,0,0);

            lsl.Add(sl);

        }

        SLancamento ultimo = lsl[lsl.Count - 1];
        lsl.RemoveAt(lsl.Count - 1);
        lsl.Insert(0, ultimo);
        
        if (erros.Count == 0)
        {
            FolhaLancamento f = new FolhaLancamento(c, lsl, "CAP_INCLUSAO_TITULO", data);


            erros.AddRange(f.salvar(null));
        }

        return erros;
    }


}



