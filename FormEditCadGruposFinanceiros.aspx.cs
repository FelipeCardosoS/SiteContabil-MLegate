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

public partial class FormEditCadGruposFinanceiros : BaseEditCadForm
{
    private GrupoFinanceiro grupoFinanceiro;

    public FormEditCadGruposFinanceiros()
        : base("GRUPO_FINANCEIRO")
    {
        grupoFinanceiro = new GrupoFinanceiro(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Grupo Financeiro";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Grupo Financeiro";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                grupoFinanceiro.nome = Convert.ToString(Request.QueryString["id"]);
                grupoFinanceiro.load();

                textDescricao.Text = grupoFinanceiro.descricao;
                textNome.Text = grupoFinanceiro.nome;
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Grupos Financeiros", "FormGridGruposFinanceiros.aspx");
        if (_cadastro)
            subTitulo.Text = "Cadastro";
        else
            subTitulo.Text = "Edição";

        if (!Page.IsPostBack)
        {

        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {

            grupoFinanceiro.descricao = textDescricao.Text;
            grupoFinanceiro.nome = textNome.Text;

            List<string> erros = grupoFinanceiro.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposFinanceiros.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            grupoFinanceiro.descricao = textDescricao.Text;
            grupoFinanceiro.nome = textNome.Text;

            List<string> erros = grupoFinanceiro.alterar(Convert.ToString(Request.QueryString["id"]));
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposFinanceiros.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
