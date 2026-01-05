using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
public partial class FormGridDelImportacao : BaseGridForm

{
    Nullable<DateTime> data;

    public FormGridDelImportacao()
        : base("IMPORTAR")
    {
    
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        string strMascaras = "$(\"#" + textData.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);

        if (!Page.IsPostBack)
            {
            botaoDeletar.Visible = false;
            botaoNovo.Visible = false;
            //DataTable table = new DataTable("table");
            //import.list_importacao();
            //comboOrdenar.DataValueField = "COD_PLANILHA";
            //comboOrdenar.DataTextField = "DATA";
            //comboOrdenar.DataBind();
            //comboOrdenar.Items.Insert(0, new ListItem("selecione", "0")); 

            comboOrdenar.Items.Add(new ListItem("Data", "DATA"));
            comboOrdenar.Items.Add(new ListItem("Valor", "Valor_Total"));
            comboOrdenar.Items.Add(new ListItem("Codigo", "cod_planilha"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            montaGrid();

            
        }
    }

    protected override void montaGrid()
    {
        ordenacao = comboOrdenar.SelectedValue;
        importao_planilhaDAO import = new importao_planilhaDAO(_conn);
        if (textData.Text == "")
            data = null;
        else
            data = Convert.ToDateTime(textData.Text);
        totalRegistros = import.list_totallinha(data);
        GVimportacao.DataSource = import.list_importacao(ordenacao, data);
        GVimportacao.DataBind();
        
        
        base.montaGrid();
    }
    protected override void botaoFiltrar_Click(object sender, EventArgs e)
    {

        base.botaoFiltrar_Click(sender, e);
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HyperLink linkdetalhes = (HyperLink)e.Item.FindControl("linkdetalhes");
        string codPlanilha = linkdetalhes.Attributes["CodPlanilha"];
        linkdetalhes.NavigateUrl = "javascript:popupf('FormPopDetalhes.aspx?codigo=" + codPlanilha + "');";
    }
    
    protected void linkDeletar_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        int codPlanilha = Convert.ToInt32(link.Attributes["CodPlanilha"]);
        importao_planilhaDAO impDAO = new importao_planilhaDAO(_conn);
        DataTable tb = impDAO.listLotes(codPlanilha);
        int erros = 0;
        FolhaLancamento folha = new FolhaLancamento(_conn);
        foreach (DataRow row in tb.Rows)
        {
            folha.modulo = row["MODULO"].ToString();

            List<string> err = folha.deletar(Convert.ToDouble(row["LOTE"]));
            erros += err.Count;
        }

        if (erros > 0)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Alguns lotes não foram deletados');", true);
            return;
        }

        importao_planilhaDAO imp = new importao_planilhaDAO(_conn);
        imp.delete(codPlanilha);
        imp.deletecontrole(codPlanilha);
        montaGrid();
    }

    
       
}