using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using RelatoriosDAOTableAdapters;

public partial class FormRelatorioEmissaoNF : BaseForm
{
    private emissao_nf_DAO emissao_nf_DAO;

    public FormRelatorioEmissaoNF()
        : base("DEFAULT") {

        emissao_nf_DAO = new emissao_nf_DAO(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Nota Fiscal";

        if (!Page.IsPostBack)
        {
            rptEmissaoNF.LocalReport.ReportPath = "Relatorios/EmissaoNF.rdlc";
            rptEmissaoNF.LocalReport.EnableExternalImages = true;

            int cod_faturamento_nf = Convert.ToInt32(Request.QueryString["id"]);

            FATURAMENTO_NFTableAdapter adap1 = new FATURAMENTO_NFTableAdapter();
            ReportDataSource src1 = new ReportDataSource("dsFATURAMENTO_NF");
            src1.Value = adap1.GetData(cod_faturamento_nf);

            FATURAMENTO_NF_RETENCOESTableAdapter adap2 = new FATURAMENTO_NF_RETENCOESTableAdapter();
            ReportDataSource src2 = new ReportDataSource("dsFATURAMENTO_NF_RETENCOES");
            src2.Value = adap2.GetData(cod_faturamento_nf);

            FATURAMENTO_NF_SERVICOS_JOBSTableAdapter adap3 = new FATURAMENTO_NF_SERVICOS_JOBSTableAdapter();
            ReportDataSource src3 = new ReportDataSource("dsFATURAMENTO_NF_SERVICOS_JOBS");
            src3.Value = adap3.GetData(cod_faturamento_nf);

            FATURAMENTO_NF_DATA_VENCIMENTOTableAdapter adap4 = new FATURAMENTO_NF_DATA_VENCIMENTOTableAdapter();
            ReportDataSource src4 = new ReportDataSource("dsFATURAMENTO_NF_DATA_VENCIMENTO");
            src4.Value = adap4.GetData(cod_faturamento_nf);

            FATURAMENTO_NF_NARRATIVASTableAdapter adap5 = new FATURAMENTO_NF_NARRATIVASTableAdapter();
            ReportDataSource src5 = new ReportDataSource("dsFATURAMENTO_NF_NARRATIVAS");
            src5.Value = adap5.GetData(cod_faturamento_nf);

            FATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOSTableAdapter adap6 = new FATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOSTableAdapter();
            ReportDataSource src6 = new ReportDataSource("dsFATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOS");
            src6.Value = adap6.Impostos(cod_faturamento_nf);

            FATURAMENTO_NF_ESPACAMENTOTableAdapter adap7 = new FATURAMENTO_NF_ESPACAMENTOTableAdapter();
            ReportDataSource src7 = new ReportDataSource("dsFATURAMENTO_NF_ESPACAMENTO");
            src7.Value = adap7.Espacamento();

            rptEmissaoNF.LocalReport.DataSources.Clear();
            rptEmissaoNF.LocalReport.DataSources.Add(src1);
            rptEmissaoNF.LocalReport.DataSources.Add(src2);
            rptEmissaoNF.LocalReport.DataSources.Add(src3);
            rptEmissaoNF.LocalReport.DataSources.Add(src4);
            rptEmissaoNF.LocalReport.DataSources.Add(src5);
            rptEmissaoNF.LocalReport.DataSources.Add(src6);
            rptEmissaoNF.LocalReport.DataSources.Add(src7);
            rptEmissaoNF.ShowRefreshButton = false;
        }
    }
}