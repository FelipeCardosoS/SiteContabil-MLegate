using System;
using System.Web.UI.WebControls;

public partial class MasterRelatorio : System.Web.UI.MasterPage
{
    public Literal literalNomeRelatorio
    {
        get { return lNomeRelatorio; }
    }

    public Literal literalNomeEmpresa
    {
        get { return lNomeEmpresa; }
    }

    public Literal literalDetalhes
    {
        get { return lDetalhes; }
    }

    public Literal literalDataGeracao
    {
        get { return lDataGeracao; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
