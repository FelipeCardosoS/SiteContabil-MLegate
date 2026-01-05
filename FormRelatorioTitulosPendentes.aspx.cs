using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormRelatorioTitulosPendentes : BaseForm
{
    Divisao divisao;
    LinhaNegocio linhaNegocio;
    Cliente cliente;
    Job job;
    ContaContabil conta;
    Empresa empresa;

    DataTable tbDivisao = new DataTable("tbDivisao");
    DataTable tbLinhaNegocio = new DataTable("tbLinhaNegocio");
    DataTable tbCliente = new DataTable("tbCliente");
    DataTable tbJob = new DataTable("tbJob");
    DataTable tbConta = new DataTable("tbConta");
    DataTable tbTerceiro = new DataTable("terceiro");

    public FormRelatorioTitulosPendentes()
        : base("DEFAULT") {
    
        divisao = new Divisao(_conn);
        linhaNegocio = new LinhaNegocio(_conn);
        cliente = new Cliente(_conn);
        job = new Job(_conn);
        conta = new ContaContabil(_conn);
        empresa = new Empresa(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        subTitulo.Text = "Títulos Pendentes";

        string strMascaras = "$(\"#" + textPeriodo.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
            strMascaras, true);

        if (!Page.IsPostBack)
        {
            conta.listaAnaliticas(ref tbConta);
            textContaDe.DataSource = tbConta;
            textContaDe.DataValueField = "COD_CONTA";
            textContaDe.DataTextField = "DESCRICAO_COMPLETO";
            textContaDe.DataBind();

            textContaAte.DataSource = tbConta;
            textContaAte.DataValueField = "COD_CONTA";
            textContaAte.DataTextField = "DESCRICAO_COMPLETO";
            textContaAte.DataBind();

            divisao.lista(ref tbDivisao);
            textDivisaoDe.DataSource = tbDivisao;
            textDivisaoDe.DataValueField = "COD_DIVISAO";
            textDivisaoDe.DataTextField = "DESCRICAO";
            textDivisaoDe.DataBind();

            textDivisaoAte.DataSource = tbDivisao;
            textDivisaoAte.DataValueField = "COD_DIVISAO";
            textDivisaoAte.DataTextField = "DESCRICAO";
            textDivisaoAte.DataBind();

            linhaNegocio.lista(ref tbLinhaNegocio);
            textLinhaNegocioDe.DataSource = tbLinhaNegocio;
            textLinhaNegocioDe.DataValueField = "COD_LINHA_NEGOCIO";
            textLinhaNegocioDe.DataTextField = "DESCRICAO";
            textLinhaNegocioDe.DataBind();

            textLinhaNegocioAte.DataSource = tbLinhaNegocio;
            textLinhaNegocioAte.DataValueField = "COD_LINHA_NEGOCIO";
            textLinhaNegocioAte.DataTextField = "DESCRICAO";
            textLinhaNegocioAte.DataBind();

            cliente.lista(ref tbCliente);
            textClienteDe.DataSource = tbCliente;
            textClienteDe.DataValueField = "COD_EMPRESA";
            textClienteDe.DataTextField = "NOME_RAZAO_SOCIAL";
            textClienteDe.DataBind();

            textClienteAte.DataSource = tbCliente;
            textClienteAte.DataValueField = "COD_EMPRESA";
            textClienteAte.DataTextField = "NOME_RAZAO_SOCIAL";
            textClienteAte.DataBind();

            job.lista(ref tbJob, 'A');
            textJobDe.DataSource = tbJob;
            textJobDe.DataValueField = "COD_JOB";
            textJobDe.DataTextField = "DESCRICAO_COMPLETO";
            textJobDe.DataBind();

            textJobAte.DataSource = tbJob;
            textJobAte.DataValueField = "COD_JOB";
            textJobAte.DataTextField = "DESCRICAO_COMPLETO";
            textJobAte.DataBind();

            empresa.listaFornecedoresClientes(ref tbTerceiro);
            textTerceiroDe.DataSource = tbTerceiro;
            textTerceiroDe.DataValueField = "COD_EMPRESA";
            textTerceiroDe.DataTextField = "NOME_RAZAO_SOCIAL";
            textTerceiroDe.DataBind();

            textTerceiroAte.DataSource = tbTerceiro;
            textTerceiroAte.DataValueField = "COD_EMPRESA";
            textTerceiroAte.DataTextField = "NOME_RAZAO_SOCIAL";
            textTerceiroAte.DataBind();


            textContaDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textContaAte.Items.Insert(0, new ListItem("Escolha", "0"));
            textDivisaoDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textDivisaoAte.Items.Insert(0, new ListItem("Escolha", "0"));
            textLinhaNegocioDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textLinhaNegocioAte.Items.Insert(0, new ListItem("Escolha", "0"));
            textClienteDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textClienteAte.Items.Insert(0, new ListItem("Escolha", "0"));
            textJobDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textJobAte.Items.Insert(0, new ListItem("Escolha", "0"));
            textTerceiroDe.Items.Insert(0, new ListItem("Escolha", "0"));
            textTerceiroAte.Items.Insert(0, new ListItem("Escolha", "0"));


            Microsoft.Reporting.WebForms.ReportParameter[] parametro = new Microsoft.Reporting.WebForms.ReportParameter[3];

            parametro[0] = new Microsoft.Reporting.WebForms.ReportParameter("periodo", (textPeriodo.Text == "" ? "01/01/1900" : textPeriodo.Text));
            parametro[1] = new Microsoft.Reporting.WebForms.ReportParameter("filtros", "");
            parametro[2] = new Microsoft.Reporting.WebForms.ReportParameter("agrupador", "");
            ReportViewer1.LocalReport.SetParameters(parametro);
        }

        textDivisaoDe.Attributes.Add("onChange", "selecionaAte('" + textDivisaoDe.UniqueID + "', '" + textDivisaoAte.ID + "');");
        textLinhaNegocioDe.Attributes.Add("onChange", "selecionaAte('" + textLinhaNegocioDe.UniqueID + "','" + textLinhaNegocioAte.ID + "');");
        textClienteDe.Attributes.Add("onChange", "selecionaAte('" + textClienteDe.UniqueID + "','" + textClienteAte.ID + "');");
        textJobDe.Attributes.Add("onChange", "selecionaAte('" + textJobDe.UniqueID + "','" + textJobAte.ID + "');");
        textTerceiroDe.Attributes.Add("onChange", "selecionaAte('" + textTerceiroDe.UniqueID + "','" + textTerceiroAte.ID + "');");
    }

    protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        ReportParameter[] parametro = new ReportParameter[3];
        parametro[0] = new ReportParameter("periodo", (textPeriodo.Text == "" ? "01/01/1900" : textPeriodo.Text));

        string filtro = "";

        if (textContaDe.SelectedValue != "0" && textContaAte.Text != "0")
            filtro += "|Conta de: " + textContaDe.Text + " até " + textContaAte.Text + "|";

        if (textDivisaoDe.SelectedValue != "0" && textDivisaoAte.Text != "0")
            filtro += "|Divisão de: " + textDivisaoDe.Text + " até " + textDivisaoAte.Text + "|";

        if (textLinhaNegocioDe.SelectedValue != "0" && textLinhaNegocioAte.Text != "0")
            filtro += "|Linha Negócio de: " + textLinhaNegocioDe.Text + " até " + textLinhaNegocioAte.Text + "|";

        if (textClienteDe.SelectedValue != "0" && textClienteAte.Text != "0")
            filtro += "|Cliente de: " + textClienteDe.Text + " até " + textClienteAte.Text + "|";

        if (textJobDe.SelectedValue != "0" && textJobAte.Text != "0")
            filtro += "|Job de: " + textJobDe.Text + " até " + textJobAte.Text + "|";

        if (textTerceiroDe.SelectedValue != "0" && textTerceiroAte.Text != "0")
            filtro += "|Terceiro de: " + textTerceiroDe.Text + " até " + textTerceiroAte.Text + "|";

        parametro[1] = new ReportParameter("filtros", filtro);
        parametro[2] = new ReportParameter("agrupador", comboAgrupador.SelectedValue);
        ReportViewer1.LocalReport.SetParameters(parametro);
    }

    protected void selecionaAte(object sender, EventArgs e)
    {
        if (Request["__EVENTARGUMENT"] != null)
        {
            DropDownList comboIrmao = (DropDownList)FindControlRecursive(Page, Request["__EVENTARGUMENT"].ToString());
            DropDownList combo = (DropDownList)sender;
            if (comboIrmao != null)
            {
                comboIrmao.SelectedValue = combo.SelectedValue;
            }
        }
    }

    protected void textContaDe_SelectedIndexChanged(object sender, EventArgs e)
    {
        int comparaDeAte = String.Compare(textContaDe.SelectedValue, textContaAte.SelectedValue);
        if(comparaDeAte >= 0)
        {
            textContaAte.SelectedValue = textContaDe.SelectedValue;
        }
    }

    protected void textContaAte_SelectedIndexChanged(object sender, EventArgs e)
    {
        int comparaDeAte = String.Compare(textContaAte.SelectedValue, textContaDe.SelectedValue);
        if (comparaDeAte < 0)
        {
            textContaDe.SelectedValue = textContaAte.SelectedValue;
        }
    }
}