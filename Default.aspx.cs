using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services;

public partial class _Default : BaseForm
{

    public _Default()
        : base("DEFAULT") 
    {

    }
    
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title += "Dashboard";
        subTitulo.Text = "Dashboard";

        //PLANO DE CONTAS REFERENCIAL
        //ContaReferencial contaRef = new ContaReferencial();
        //contaRef.importar(@"c:\backups\AnexoIIADECofisPlanodeContasReferencial_CSV_2.txt", ';', _conn);

       //SPED
        //DateTime inicio = new DateTime(2011, 01, 01);
        //DateTime termino = new DateTime(2011, 07, 31);
        //Sped.AbstractGeracaoSped sped = new Sped.Contabil(inicio, termino, "G", 1, inicio, SessionView.EmpresaSession, _conn);
        //sped.gerar(@"c:\backups\spedcontabil_groupon.txt");
        
        
        Empresa empresa = new Empresa(_conn);
        empresa.codigo = Convert.ToInt32(Session["empresa"]);
        empresa.load();

        //if para Sicronizar com o timesheet***************************************

        if (Convert.ToBoolean(ConfigurationManager.AppSettings["sincronizaTimesheet"]))
        {
            Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/sincronizacao.js"));
            ScriptManager.RegisterStartupScript(this, GetType(), "iniciaSincronizacao", "iniciaSincronizacao();", true);
        }
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static List<string> salvar(List<SLancamento> lanctos, string modulo,DateTime data)
    {
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c, lanctos, modulo, data);
        return folha.salvar(null);
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static List<SLancamento> getSessionLanctos(string modulo)
    {
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }

        return arr;
    }

    [WebMethod]
    public static List<string> sincronizaJobs()
    {
        Conexao c = new Conexao();
        Job job = new Job(c);
        return job.sincroniza();
    }

    [WebMethod]
    public static List<string> sincronizaClientes()
    {
        Conexao c = new Conexao();
        Empresa empresa = new Empresa(c);
        return empresa.sincroniza();
    }

    [WebMethod]
    public static List<string> sincronizaLinhasNegocio()
    {
        Conexao c = new Conexao();
        LinhaNegocio linhaNegocio = new LinhaNegocio(c);
        return linhaNegocio.sincroniza();
    }

    [WebMethod]
    public static List<string> sincronizaDivisoes()
    {
        Conexao c = new Conexao();
        Divisao divisao = new Divisao(c);
        return divisao.sincroniza();
    }

    [WebMethod]
    public static int sincronizaJobLinhaNegocio()
    {
        Conexao c = new Conexao();
        Job job = new Job(c);
        return job.totalClassificacao();
    }
}