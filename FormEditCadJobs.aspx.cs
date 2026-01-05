using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Text;
using Newtonsoft.Json;

public partial class FormEditCadJobs : BaseEditCadForm
{
    public bool MedicalPrime;

    public FormEditCadJobs() : base("JOB") { }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Modelo";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Modelo";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
    }

    protected override void montaTela()
    {
        base.montaTela();
        addSubTitulo("Jobs", "FormGridJobs.aspx");

        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        botaoSalvar.Click -= botaoSalvar_Click;
		botaoSalvar.UseSubmitBehavior = false;
		
        if (!Page.IsPostBack)
        {
            tbxObsNF.Attributes.Add("maxlength", tbxObsNF.MaxLength.ToString());

            Cliente Cliente = new Cliente(_conn);
            LinhaNegocio LinhaNegocio = new LinhaNegocio(_conn);
            Divisao Divisao = new Divisao(_conn);
            Empresa empresa = new Empresa(_conn);
            Projeto projeto = new Projeto(_conn);
            Despesa despesa = new Despesa(_conn);

            DataTable dtCliente = new DataTable("dtCliente");
            Cliente.lista(ref dtCliente);

            DataTable dtLinhaNegocio = new DataTable("dtLinhaNegocio");
            LinhaNegocio.lista(ref dtLinhaNegocio);

            DataTable dtDivisao = new DataTable("dtDivisao");
            Divisao.lista(ref dtDivisao);

            List<Projeto> dtProjeto = new List<Projeto>();

            List<Despesa> dtDespesa = despesa.lista();


            DataTable dtConsultor = new DataTable("dtConsultor");
            empresa.listaConsultores(ref dtConsultor);

            ddlCliente.DataSource = dtCliente;
            ddlCliente.DataTextField = "NOME_FANTASIA";
            ddlCliente.DataValueField = "COD_EMPRESA";
            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new ListItem("Escolha", "0"));

            ddlMedico.DataSource = dtCliente;                           //Medical Prime
            ddlMedico.DataTextField = "NOME_FANTASIA";                  //Medical Prime
            ddlMedico.DataValueField = "COD_EMPRESA";                   //Medical Prime
            ddlMedico.DataBind();                                       //Medical Prime
            ddlMedico.Items.Insert(0, new ListItem("Escolha", "0"));    //Medical Prime

            ddlEquipe.DataSource = dtCliente;                           //Medical Prime
            ddlEquipe.DataTextField = "NOME_FANTASIA";                  //Medical Prime
            ddlEquipe.DataValueField = "COD_EMPRESA";                   //Medical Prime
            ddlEquipe.DataBind();                                       //Medical Prime
            ddlEquipe.Items.Insert(0, new ListItem("Escolha", "0"));    //Medical Prime

            ddlVendedor.DataSource = dtCliente;                         //Medical Prime
            ddlVendedor.DataTextField = "NOME_FANTASIA";                //Medical Prime
            ddlVendedor.DataValueField = "COD_EMPRESA";                 //Medical Prime
            ddlVendedor.DataBind();                                     //Medical Prime
            ddlVendedor.Items.Insert(0, new ListItem("Escolha", "0"));  //Medical Prime

            ddlLinhaNegocio.DataSource = dtLinhaNegocio;
            ddlLinhaNegocio.DataTextField = "DESCRICAO";
            ddlLinhaNegocio.DataValueField = "COD_LINHA_NEGOCIO";
            ddlLinhaNegocio.DataBind();
            ddlLinhaNegocio.Items.Insert(0, new ListItem("Escolha", "0"));

            ddlDivisao.DataSource = dtDivisao;
            ddlDivisao.DataTextField = "DESCRICAO";
            ddlDivisao.DataValueField = "COD_DIVISAO";
            ddlDivisao.DataBind();
            ddlDivisao.Items.Insert(0, new ListItem("Escolha", "0"));
 
            ddlProjeto.DataSource = dtProjeto;
            ddlProjeto.DataTextField = "DescProjeto";
            ddlProjeto.DataValueField = "CodProjeto";
            ddlProjeto.DataBind();
            ddlProjeto.Items.Insert(0, new ListItem("Escolha Divisão", "0"));

            ddlStatus.Items.Add(new ListItem("ATIVO", "A"));
            ddlStatus.Items.Add(new ListItem("INATIVO", "I"));

            ddlStatusTimesheet.Items.Add(new ListItem("ATIVO", "A"));
            ddlStatusTimesheet.Items.Add(new ListItem("INATIVO", "I"));

            string htmlComboGestores = "<option value='0'> Escolha... </option>";
            foreach(DataRow gestor in dtConsultor.Rows)
			{
                htmlComboGestores += "<option value='" + Convert.ToString(gestor["COD_EMPRESA"]) + "'> " + Convert.ToString(gestor["NOME_RAZAO_SOCIAL"]) + "</option>";
            }

            hdGestores.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlComboGestores));
            
            hdDespesas.Value = JsonConvert.SerializeObject(dtDespesa);

            if (!_cadastro)
            {
                Job Job = new Job(_conn);
                Job.codigo = Convert.ToInt32(Request.QueryString["id"]);
                Job.load();

                ddlCliente.SelectedValue = Job.cliente.ToString();
                ddlLinhaNegocio.SelectedValue = Job.linhaNegocio.ToString();
                ddlDivisao.SelectedValue = Job.divisao.ToString();
                tbxNome.Text = Job.nome;
                tbxDescricao.Text = Job.descricao;
                tbxObsNF.Text = Job.obsNF;

                ddlMedico.SelectedValue = Job.Medico.ToString();        //Medical Prime
                ddlEquipe.SelectedValue = Job.Equipe.ToString();        //Medical Prime
                ddlVendedor.SelectedValue = Job.Vendedor.ToString();    //Medical Prime
                tbxPaciente.Text = Job.Paciente;                        //Medical Prime
                tbxConvenio.Text = Job.Convenio;                        //Medical Prime

                dtProjeto = projeto.listaPorDivisao(Job.divisao);
                ddlProjeto.DataSource = dtProjeto;
                ddlProjeto.DataBind();
                ddlProjeto.SelectedValue = Convert.ToString(Job.CodProjeto);

                ddlStatus.SelectedValue = Convert.ToString(Job.status);
                hdGestorJob.Value = Job.Gestores == null ? "" : JsonConvert.SerializeObject(Job.Gestores);
                hdConsultorJob.Value = Job.Consultores == null ? "" : JsonConvert.SerializeObject(Job.Consultores);
                hdDespesaJob.Value = Job.Despesas == null ? "" : JsonConvert.SerializeObject(Job.Despesas);
            }
        }

    }
    [WebMethod]
    public static string getGestoresJob()
	{
        return Convert.ToString(HttpContext.Current.Session["ss_gestores_job"]);
    }

    [WebMethod]
    public static void salvaGestoresJob(string gestoresJob)
	{
        HttpContext.Current.Session["ss_gestores_job"] = gestoresJob;
    }

    [WebMethod]
    public static string getConsultoresJob()
    {
        return Convert.ToString(HttpContext.Current.Session["ss_consultores_job"]);
    }

    [WebMethod]
    public static void salvaConsultoresJob(string consultoresJob)
    {
        HttpContext.Current.Session["ss_consultores_job"] = consultoresJob;
    }

    [WebMethod]
    public static string getDespesasJob()
    {
        return Convert.ToString(HttpContext.Current.Session["ss_despesas_job"]);
    }

    [WebMethod]
    public static void salvaDespesasJob(string despesasJob)
    {
        HttpContext.Current.Session["ss_despesas_job"] = despesasJob;
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        Job Job = new Job(_conn);

        Job.cliente = Convert.ToInt32(ddlCliente.SelectedValue);
        Job.linhaNegocio = Convert.ToInt32(ddlLinhaNegocio.SelectedValue);
        Job.divisao = Convert.ToInt32(ddlDivisao.SelectedValue);
        Job.nome = tbxNome.Text;
        Job.descricao = tbxDescricao.Text;

        if (string.IsNullOrWhiteSpace(tbxObsNF.Text))
            Job.obsNF = string.Empty;
        else
            Job.obsNF = tbxObsNF.Text;
        
        Job.Medico = Convert.ToInt32(ddlMedico.SelectedValue);          //Medical Prime
        Job.Equipe = Convert.ToInt32(ddlEquipe.SelectedValue);          //Medical Prime
        Job.Vendedor = Convert.ToInt32(ddlVendedor.SelectedValue);      //Medical Prime
        Job.Paciente = tbxPaciente.Text;                                //Medical Prime
        Job.Convenio = tbxConvenio.Text;                                //Medical Prime

        List<GestorJob> listaGestores = JsonConvert.DeserializeObject<List<GestorJob>>(hdGestorJob.Value);
        List<int> listaGestoresDeletar = JsonConvert.DeserializeObject<List<int>>(hdGestorJobDeletar.Value);
        List<ConsultorJob> listaConsultores = JsonConvert.DeserializeObject<List<ConsultorJob>>(hdConsultorJob.Value);
        List<int> listaConsultoresDeletar = JsonConvert.DeserializeObject<List<int>>(hdConsultorJobDeletar.Value);
        List<DespesaJob> listaDespesas = JsonConvert.DeserializeObject<List<DespesaJob>>(hdDespesaJob.Value);
        List<int> listaDespesasDeletar = JsonConvert.DeserializeObject<List<int>>(hdDespesaJobDeletar.Value);

        Job.Gestores = listaGestores == null ? new List<GestorJob>() : listaGestores;
        Job.Consultores = listaConsultores == null ? new List<ConsultorJob>() : listaConsultores;
        Job.Despesas = listaDespesas == null ? new List<DespesaJob>() : listaDespesas;

        List<string> erros;

        if (_cadastro)
            erros = Job.novo();
        else
        {
            Job.codigo = Convert.ToInt32(Request.QueryString["id"]);
            erros = Job.alterar(listaGestoresDeletar, listaConsultoresDeletar, listaDespesasDeletar);
        }

        if (erros.Count > 0)
            errosFormulario(erros);
        else
            Response.Redirect("FormGridJobs.aspx");
    }
}