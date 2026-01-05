using System;

public partial class MasterEditCad : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        linkAlterarDados.NavigateUrl = "FormEditCadUsuarios.aspx?id=" + Session["usuario"];
        linkAlterarSenha.NavigateUrl = "FormEditSenhaUsuarios.aspx?id=" + Session["usuario"];
    }
}
