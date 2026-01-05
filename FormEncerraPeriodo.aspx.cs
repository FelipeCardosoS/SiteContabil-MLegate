using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FormEncerraPeriodo : BaseForm
{
    public FormEncerraPeriodo()
        : base("ENCERRAMENTO_PERIODO") { }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Encerramento de Período";

        string strMascaras = "$(\"#" + inicioTextBox.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + inicioTextBox.ClientID + "\").mask(\"99/9999\");";
        strMascaras += "$(\"#" + terminoTextBox.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + terminoTextBox.ClientID + "\").mask(\"99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), strMascaras, true);
    }

    protected override void montaTela()
    {
        base.montaTela();

        if (!Page.IsPostBack)
        {
            ContaContabil conta = new ContaContabil(_conn);
            DataTable contaTb = new DataTable();
            conta.listaAnaliticas(ref contaTb,true);
            contaApuracaoDropDownList.DataSource = contaTb;
            contaApuracaoDropDownList.DataTextField = "DESCRICAO_COMPLETO";
            contaApuracaoDropDownList.DataValueField = "COD_CONTA";
            contaApuracaoDropDownList.DataBind();
            contaApuracaoDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));

            Job job = new Job(_conn);
            DataTable jobTb = new DataTable();
            job.lista(ref jobTb, 'A');
            jobDropDownList.DataSource = jobTb;
            jobDropDownList.DataTextField = "DESCRICAO_COMPLETO";
            jobDropDownList.DataValueField = "COD_JOB";
            jobDropDownList.DataBind();
            jobDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            linhaNegocioDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            divisaoDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            clienteDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected void jobDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int jobEscolhido = Convert.ToInt32(jobDropDownList.SelectedValue);
        if (jobEscolhido > 0)
        {
            Job job = new Job(_conn);
            job.codigo = jobEscolhido;
            job.load();

            linhaNegocioDropDownList.Enabled = true;
            divisaoDropDownList.Enabled = true;
            clienteDropDownList.Enabled = true;
            LinhaNegocio linhaNegocio = new LinhaNegocio(_conn);
            DataTable linhaNegocioTb = new DataTable();
            linhaNegocio.lista(ref linhaNegocioTb);
            linhaNegocioDropDownList.DataSource = linhaNegocioTb;
            linhaNegocioDropDownList.DataTextField = "descricao";
            linhaNegocioDropDownList.DataValueField = "cod_linha_negocio";
            linhaNegocioDropDownList.DataBind();
            linhaNegocioDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            linhaNegocioDropDownList.SelectedValue = job.linhaNegocio.ToString();

            Divisao divisao = new Divisao(_conn);
            DataTable divisaoTb = new DataTable();
            divisao.lista(ref divisaoTb);
            divisaoDropDownList.DataSource = divisaoTb;
            divisaoDropDownList.DataTextField = "descricao";
            divisaoDropDownList.DataValueField = "cod_divisao";
            divisaoDropDownList.DataBind();
            divisaoDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            divisaoDropDownList.SelectedValue = job.divisao.ToString();

            Cliente cliente = new Cliente(_conn);
            DataTable clienteTb = new DataTable();
            cliente.lista(ref clienteTb);
            clienteDropDownList.DataSource = clienteTb;
            clienteDropDownList.DataTextField = "nome_razao_social";
            clienteDropDownList.DataValueField = "cod_empresa";
            clienteDropDownList.DataBind();
            clienteDropDownList.Items.Insert(0, new ListItem("Escolha", "0"));
            clienteDropDownList.SelectedValue = job.cliente.ToString();
        }
        else
        {
            linhaNegocioDropDownList.SelectedValue = "0";
            divisaoDropDownList.SelectedValue = "0";
            clienteDropDownList.SelectedValue = "0";
            linhaNegocioDropDownList.Enabled = false;
            divisaoDropDownList.Enabled = false;
            clienteDropDownList.Enabled = false;
        }
    }

    protected void encerrarButton_Click(object sender, EventArgs e)
    {
        EncerramentoPeriodo folha = new EncerramentoPeriodo(_conn);
        List<string> erros = new List<string>();
        DateTime inicio = Convert.ToDateTime("01/" + inicioTextBox.Text);
        DateTime termino = Convert.ToDateTime("01/" + terminoTextBox.Text);
        termino = termino.AddMonths(1).AddDays(-1);
        try
        {
            if (!folha.encerraPeriodo(inicio, termino, contaApuracaoDropDownList.SelectedValue,
                Convert.ToInt32(jobDropDownList.SelectedValue), Convert.ToInt32(linhaNegocioDropDownList.SelectedValue),
                Convert.ToInt32(divisaoDropDownList.SelectedValue), Convert.ToInt32(clienteDropDownList.SelectedValue), txtHistorico.Text))
            {
                erros.Add("Ocorreu um erro inesperado, informe o administrador.");
                errosFormulario(erros);
            }

            erros.Add("Encerramento concluído.");
            errosFormulario(erros);
        }
        catch(ApplicationException apex)
        {
            erros.Add(apex.Message);
            errosFormulario(erros);
        }

    }
}
