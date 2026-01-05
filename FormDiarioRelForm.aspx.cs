using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using RelatoriosDAOTableAdapters;

public partial class FormDiarioRelForm : BaseForm
{
    public FormDiarioRelForm()
        : base("RELATORIO_DIARIO") { }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Livro Diário";

        string strMascaras = "$(\"#" + textPeriodoInicio.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + textPeriodoInicio.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textPeriodoTermino.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + textPeriodoTermino.ClientID + "\").mask(\"99/99/9999\");";
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras", strMascaras, true);
    }
    protected void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Microsoft.Reporting.WebForms.ReportParameter[] parametro = new Microsoft.Reporting.WebForms.ReportParameter[4];

        parametro[0] = new Microsoft.Reporting.WebForms.ReportParameter("periodoInicio", (textPeriodoInicio.Text == "" ? "01/01/1900" : textPeriodoInicio.Text));
        parametro[1] = new Microsoft.Reporting.WebForms.ReportParameter("periodoTermino", (textPeriodoTermino.Text == "" ? "01/01/1900" : textPeriodoTermino.Text));
        parametro[2] = new Microsoft.Reporting.WebForms.ReportParameter("inicioPagina", (textPaginaInicio.Text == "" ? "1" : textPaginaInicio.Text));
        parametro[3] = new Microsoft.Reporting.WebForms.ReportParameter("totalPaginas", (textTotalPaginas.Text == "" ? "500" : textTotalPaginas.Text));
        //parametro[4] = new Microsoft.Reporting.WebForms.ReportParameter("codEmpresa", );
        ReportViewer1.LocalReport.SetParameters(parametro);
    }
}
