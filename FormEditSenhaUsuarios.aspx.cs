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

public partial class FormEditSenhaUsuarios : BaseEditCadForm
{
    public Usuario usuario;

    public FormEditSenhaUsuarios()
        : base("USUARIO")
    {
        usuario = new Usuario(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);
        _codigoTarefa = "ALT_SENHA";
        Title += "Edição de Senha";
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        H_COD_USUARIO.Value = Request.QueryString["id"];
    }

    protected override void montaTela()
    {
        base.montaTela();
        botaoSalvar.Text = "Alterar";
        addSubTitulo("Usuários", "FormGridUsuarios.aspx");
        subTitulo.Text = "Edição de Senha";

		if (Convert.ToInt32(Request.QueryString["id"]) != Convert.ToInt32(Session["usuario"]))
		{
			fieldAtual.Visible = false;
			legendaSenha.InnerText = "Informe a Nova senha do Usuário";
		}
	}

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
            usuario.id = Convert.ToInt32(H_COD_USUARIO.Value);
			usuario.idSessao = Convert.ToInt32(Session["usuario"]);
			usuario.senha = textAtual.Text;
            List<string> erros = usuario.alterarSenha(textSenha.Text);
            if (erros.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlteraSenha", "alert('Senha alterada com sucesso!')", true);
            }
            else
            {
                errosFormulario(erros);
            }
    }
}
