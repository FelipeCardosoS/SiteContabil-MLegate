using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadLinhasNegocio : BaseEditCadForm
{
    public FormEditCadLinhasNegocio()
        : base("LINHA_NEGOCIO") { }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Modelo";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Modelo";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Linhas de Negócio", "FormGridLinhasNegocio.aspx");
        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {
            if (!_cadastro)
            {
                LinhaNegocio linhaNegocio = new LinhaNegocio(_conn);
                linhaNegocio.codigo = Convert.ToInt32(Request.QueryString["id"]);
                linhaNegocio.load();

                nomeTextBox.Text = linhaNegocio.descricao;
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        LinhaNegocio linhaNegocio = new LinhaNegocio(_conn);
        linhaNegocio.descricao = nomeTextBox.Text;
        List<string> erros = new List<string>();
        if (_cadastro)
        {
            erros = linhaNegocio.novo();
        }
        else
        {
            linhaNegocio.codigo = Convert.ToInt32(Request.QueryString["id"]);
            erros = linhaNegocio.alterar();
        }

        if (erros.Count > 0)
            errosFormulario(erros);
        else
            Response.Redirect("FormGridLinhasNegocio.aspx");
    }
}
