using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadClassificacaoConta : BaseEditCadForm
{
    public FormEditCadClassificacaoConta()
        : base("CLASSIFICACAO_CONTA")
    {
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        subTitulo.Text = "Cadastro/Edição Classificação Conta";

        if (!_cadastro)
        {
            codigoText.Enabled = false;
        }

        if (!Page.IsPostBack)
        {
            if (!_cadastro)
            {
                classificacaoContaDAO classificacaoDAO = new classificacaoContaDAO(_conn);

                SClassificacaoConta classificacao = classificacaoDAO.load(Request.QueryString["id"].ToString(), SessionView.EmpresaSession);
                codigoText.Text = classificacao.codClassificacao;
                descricaoText.Text = classificacao.descricao;
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        classificacaoContaDAO classificacaoDAO = new classificacaoContaDAO(_conn);
        SClassificacaoConta classificacao = new SClassificacaoConta();
        classificacao.codClassificacao = codigoText.Text;
        classificacao.descricao = descricaoText.Text;
        classificacao.codEmpresa = SessionView.EmpresaSession;

        if (_cadastro)
        {
            classificacaoDAO.insert(classificacao);
        }
        else
        {
            classificacaoDAO.update(classificacao);
        }

        Response.Redirect("FormGridClassificacaoConta.aspx");
    }
}