using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadUnidades : BaseEditCadForm
{
    unidadesDAO unidadesDAO;

    public FormEditCadUnidades()
        : base("UNIDADES")
    {
        unidadesDAO = new unidadesDAO(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title += "Cadastro/Edição de Unidades";
        subTitulo.Text += "Cadastro/Edição de Unidades";

        if (!_cadastro)
        {
            if (!Page.IsPostBack)
            {
                string sigla = Request.QueryString["id"];
                SUnidade unidade = unidadesDAO.load(sigla, SessionView.EmpresaSession);
                if (unidade != null)
                {
                    textDescricao.Text = unidade.descricao;
                    textSigla.Text = unidade.sigla;
                    textSigla.Enabled = false;
                }
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            SUnidade unidade = new SUnidade();
            unidade.descricao = textDescricao.Text;
            unidade.sigla = textSigla.Text;
            unidade.codEmpresa = SessionView.EmpresaSession;
            unidadesDAO.insert(unidade);
        }
        else
        {
            SUnidade unidade = new SUnidade();
            unidade.descricao = textDescricao.Text;
            unidade.sigla = textSigla.Text;
            unidade.codEmpresa = SessionView.EmpresaSession;
            unidadesDAO.update(unidade);
        }

        Response.Redirect("FormGridUnidades.aspx");
    }
}