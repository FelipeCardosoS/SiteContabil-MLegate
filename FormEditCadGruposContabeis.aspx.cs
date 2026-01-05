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

public partial class FormEditCadGruposContabeis : BaseEditCadForm
{
    private GrupoContabil grupoContabil;

    public FormEditCadGruposContabeis()
        : base("GRUPO_CONTABIL")
    {
        grupoContabil = new GrupoContabil(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Grupo Contábil";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Grupo Contábil";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            H_COD_GRUPO_CONTABIL.Value = "0";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                grupoContabil.codigo = Convert.ToInt32(Request.QueryString["id"]);
                grupoContabil.load();

                H_COD_GRUPO_CONTABIL.Value = grupoContabil.codigo.ToString();
                textDescricao.Text = grupoContabil.descricao;
                textConta.Text = grupoContabil.conta;
                comboRegraExibicao.SelectedValue = grupoContabil.regraExibicao;
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Grupos Contábeis", "FormGridGruposContabeis.aspx");
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

            grupoContabil.descricao = textDescricao.Text;
            grupoContabil.conta = textConta.Text;
            grupoContabil.regraExibicao = comboRegraExibicao.SelectedValue;

            List<string> erros = grupoContabil.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposContabeis.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            grupoContabil.codigo = Convert.ToInt32(H_COD_GRUPO_CONTABIL.Value);
            grupoContabil.descricao = textDescricao.Text;
            grupoContabil.conta = textConta.Text;
            grupoContabil.regraExibicao = comboRegraExibicao.SelectedValue;

            List<string> erros = grupoContabil.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposContabeis.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
