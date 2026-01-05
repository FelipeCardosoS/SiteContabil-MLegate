using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormGerarSped : BaseForm
{
    public FormGerarSped()
        : base("SPED") { }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), 
            "$('#" + textInicio.ClientID + "').unmask();"+
            "$('#" + textTermino.ClientID + "').unmask();" +
            "$('#" + textTermino.ClientID + "').mask('99/99/9999');" + 
            "$('#" + textInicio.ClientID + "').mask('99/99/9999');", true);

        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Gerar Sped Pis/Cofins";
        }
    }

    protected void botaoGerar_Click(object sender, EventArgs e)
    {
        DateTime inicio = new DateTime();
        DateTime termino = new DateTime();

        DateTime.TryParse(textInicio.Text, out inicio);
        DateTime.TryParse(textTermino.Text, out termino);
        string caminho = "Speds/sped_" + DateTime.Now.ToString("ddMMyyyyHmmss") + ".txt";
        GeracaoSped gerarSped = new GeracaoSped(inicio, termino, SessionView.EmpresaSession, _conn);
        gerarSped.DocumentoFiscal = checkDocumentoFiscal.Checked;

        if (gerarSped.gerar(Server.MapPath(caminho), checkLucroReal.Checked) == false)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alerta", "alert('Nenhuma contador cadastrado.')", true);
        }
        else
        {
            textoLiteral.Text = "<a href='" + caminho + "'>Arquivo gerado com sucesso, clique aqui para fazer download</a>";
        }
    }
}