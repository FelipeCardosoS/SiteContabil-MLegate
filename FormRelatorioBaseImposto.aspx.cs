using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using RelatoriosDAOTableAdapters;

public partial class FormBaseImpostoRelForm : BaseForm
{
    public FormBaseImpostoRelForm()
        : base("BASE_IMPOSTO"){
    
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Base para Cálculo de Impostos";

        string strMascaras = "$(\"#" + txtDe.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + txtDe.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + txtAte.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + txtAte.ClientID + "\").mask(\"99/99/9999\");";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras", strMascaras, true);

        if (!Page.IsPostBack)
        {
            ReportViewer1.LocalReport.ReportPath = "Relatorios/BaseImposto.rdlc";
            ReportViewer1.LocalReport.EnableExternalImages = true;
            baseCalculoImpostoTableAdapter adap = new baseCalculoImpostoTableAdapter();
            ReportDataSource src = new ReportDataSource("baseCalculoImposto");
            src.Value = adap.executar(DateTime.Now, DateTime.Now);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(src);
            ReportViewer1.ServerReport.Refresh();
        }
    }


    protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        ReportViewer1.LocalReport.ReportPath = "Relatorios/BaseImposto.rdlc";
        ReportViewer1.LocalReport.EnableExternalImages = true;
        baseCalculoImpostoTableAdapter adap = new baseCalculoImpostoTableAdapter();
        ReportDataSource src = new ReportDataSource("baseCalculoImposto");
        src.Value = adap.executar((txtDe.Text == "" ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(txtDe.Text)), (txtAte.Text == "" ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(txtAte.Text)));
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(src);
        ReportViewer1.ServerReport.Refresh();
    }
}