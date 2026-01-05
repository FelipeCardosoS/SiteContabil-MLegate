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

public partial class FormEditCadUsuarios : BaseEditCadForm
{
    private Usuario usuario;
    private PerfilAcesso perfil;
    private DataTable tbPerfis = new DataTable("tbPerfis");

    public FormEditCadUsuarios()
        : base("USUARIO")
    {
        usuario = new Usuario(_conn);
        perfil = new PerfilAcesso(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Usuário";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Usuário";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (_cadastro)
        {
            H_COD_USUARIO.Value = "0";
            botaoSalvar.Text = "Cadastrar";
        }
        else
        {
            botaoSalvar.Text = "Alterar";
            campoSenha.Visible = false;
            campoConfirmacao.Visible = false;
            textSenha.Visible = false;
            textConfirmacao.Visible = false;

            if (!Page.IsPostBack)
            {
                usuario.id = Convert.ToInt32(Request.QueryString["id"]);
                usuario.load();

                H_COD_USUARIO.Value = usuario.id.ToString();
                textNome.Text = usuario.nome;
                comboPerfil.SelectedValue = usuario.perfil;
                textLogin.Text = usuario.email;
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Usuários", "FormGridUsuarios.aspx");
        if(_cadastro)
            subTitulo.Text = "Cadastro";
        else
            subTitulo.Text = "Edição";

        if (!Page.IsPostBack)
        {
            tbPerfis.Clear();
            perfil.lista(ref tbPerfis);
            comboPerfil.DataSource = tbPerfis;
            comboPerfil.DataValueField = "COD_PERFIL";
            comboPerfil.DataTextField = "DESCRICAO";
            comboPerfil.DataBind();
        }

        comboPerfil.Items.Insert(0,new ListItem("Escolha", "0"));
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            usuario.nome = textNome.Text;
            usuario.email = textLogin.Text;
            usuario.perfil = comboPerfil.SelectedValue;
            usuario.senha = textSenha.Text;
            List<string> erros = usuario.inserir();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridUsuarios.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            usuario.id = Convert.ToInt32(H_COD_USUARIO.Value);
            usuario.nome = textNome.Text;
            usuario.email = textLogin.Text;
            usuario.perfil = comboPerfil.SelectedValue;
            List<string> erros = usuario.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridUsuarios.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
