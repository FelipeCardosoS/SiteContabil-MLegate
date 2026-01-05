using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class MasterForm : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Body1.Attributes.Add("onload", "horizontal('')");
        linkAlterarDados.NavigateUrl = "FormEditCadUsuarios.aspx?id=" + Session["usuario"];
        linkAlterarSenha.NavigateUrl = "FormEditSenhaUsuarios.aspx?id=" + Session["usuario"];
    }

    public ScriptManager GetScriptManager
    {
        get { return ScriptManager1; }
    }

    public HtmlGenericControl Body
    {
        get { return Body1; }
    }
}
