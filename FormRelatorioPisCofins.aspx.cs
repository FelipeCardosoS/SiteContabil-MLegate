using Microsoft.Reporting.WebForms;
using RelatoriosDAOTableAdapters;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormRelatorioPisCofins : BaseForm
{
    ContaContabil conta;
    DataTable tbConta = new DataTable("tbConta");

    public FormRelatorioPisCofins()
        : base("DEFAULT") {
        conta = new ContaContabil(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Conferência PIS/Cofins";

        string strMascaras = "$(\"#" + txtPeriodoDe.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + txtPeriodoDe.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + txtPeriodoAte.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + txtPeriodoAte.ClientID + "\").mask(\"99/99/9999\");";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras", strMascaras, true);

        if (!Page.IsPostBack)
        {
            conta.listaContasAnaliticas(ref tbConta);
            ddlCodigoConta.DataSource = tbConta;
            ddlCodigoConta.DataValueField = "COD_CONTA";
            ddlCodigoConta.DataTextField = "DESCRICAO";
            ddlCodigoConta.DataBind();
            ddlCodigoConta.Items.Insert(0, new ListItem("Escolha", "0"));
        }
        rptPisCofins.ShowPrintButton = true;
    }

    protected void rptPisCofins_ReportRefresh1(object sender, System.ComponentModel.CancelEventArgs e)
    {
        rptPisCofins.LocalReport.ReportPath = "Relatorios/PisCofins.rdlc";
        rptPisCofins.LocalReport.EnableExternalImages = true;
        PisCofinsTableAdapter adap = new PisCofinsTableAdapter();
        ReportDataSource src = new ReportDataSource("PisCofins");
        src.Value = adap.executar((ddlCodigoConta.Text == "Escolha" ? "0" : ddlCodigoConta.SelectedValue), Convert.ToDateTime((txtPeriodoDe.Text == "" ? "01/01/1900" : txtPeriodoDe.Text)), Convert.ToDateTime((txtPeriodoAte.Text == "" ? "01/01/1900" : txtPeriodoAte.Text)));
        rptPisCofins.LocalReport.DataSources.Clear();
        rptPisCofins.LocalReport.DataSources.Add(src);
        rptPisCofins.ServerReport.Refresh();
        rptPisCofins.ShowPrintButton = true;
    }
}