using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;

public partial class FormAnaliseJobs : BaseForm
{
    private Job job;
    private Cliente cliente;
    private DataTable tbJobs = new DataTable("tbJobs");
    private DataTable tbClientes = new DataTable("tbClientes");

    public FormAnaliseJobs() 
        : base("ANALISE_JOBS") { }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Análise de Jobs Sincronizados";

            job = new Job(_conn);

            if (job.totalClassificacao() == 0)
            {
                ltMensagem.Visible = true;
                ltMensagem.Text = "Nenhum Job para sincronizar!";
            }
            else 
            {
                Painel.Visible = true;
                
                Cliente cliente = new Cliente(_conn);
                LinhaNegocio linhaNegocio = new LinhaNegocio(_conn);
                Divisao divisao = new Divisao(_conn);

                DataTable tbClientes = new DataTable("clientes");
                cliente.lista(ref tbClientes);

                comboCliente.DataSource = tbClientes;
                comboCliente.DataTextField = "NOME_RAZAO_SOCIAL";
                comboCliente.DataValueField = "COD_EMPRESA";
                comboCliente.DataBind();
                comboCliente.Items.Insert(0, new ListItem("Escolha", "0"));

                DataTable tbLinhaNegocio = new DataTable("linhasNegocio");
                linhaNegocio.lista(ref tbLinhaNegocio, 0);

                comboLinhaNegocio.DataSource = tbLinhaNegocio;
                comboLinhaNegocio.DataTextField = "DESCRICAO";
                comboLinhaNegocio.DataValueField = "COD_LINHA_NEGOCIO";
                comboLinhaNegocio.DataBind();
                comboLinhaNegocio.Items.Insert(0, new ListItem("Escolha", "0"));

                DataTable tbDivisoes = new DataTable("divisoes");
                divisao.lista(ref tbDivisoes);

                comboDivisao.DataSource = tbDivisoes;
                comboDivisao.DataTextField = "DESCRICAO";
                comboDivisao.DataValueField = "COD_DIVISAO";
                comboDivisao.DataBind();
                comboDivisao.Items.Insert(0, new ListItem("Escolha", "0"));

                ScriptManager.RegisterStartupScript(this, GetType(), "carregaJobs", "carregaJobs();", true);
            }
        }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<Job> carregaJobsAnalise()
    {
        Conexao c = new Conexao();
        List<Job> list = new List<Job>();
        Job job = new Job(c);
        return job.loadSincroniza();
    }

    [WebMethod]
    public static List<LinhaNegocio_ajax> loadlinhaNegocio(int cod_divisao)
    {
        Conexao c = new Conexao();
        linhasNegocioDAO _linhasNegocioDAO = new linhasNegocioDAO(c);
        List<LinhaNegocio_ajax> list_linhaNegocio = new List<LinhaNegocio_ajax>();
        DataTable tb = _linhasNegocioDAO.lista_ajax(cod_divisao);
        foreach (DataRow item in tb.Rows)
        {
            list_linhaNegocio.Add(new LinhaNegocio_ajax
            {
                Cod_Linha_Negocio = Convert.ToInt32(item["Cod_Linha_Negocio"].ToString()),
                Descricao = item["Descricao"].ToString()
            });
        }
        return list_linhaNegocio;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> salvar(List<Job> Jobs)
    {
        Conexao c = new Conexao();
        List<Job> list = new List<Job>();
        Job job = new Job(c);
        return job.novoList(Jobs);
    }
}