using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormPopDetalhes : Permissao
{
    public FormPopDetalhes()
        : base("IMPORTAR")
    {
    
    }

    string cod_planilha = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        cod_planilha = Request.QueryString["codigo"];
        if (!Page.IsPostBack)
        {
            montaGrid();
        }
    }
    protected void montaGrid()
    {
        importao_planilhaDAO import = new importao_planilhaDAO(_conn);
        GVdetalhes.DataSource = import.list_detalhe(cod_planilha);
        GVdetalhes.DataBind();
        
    }

}