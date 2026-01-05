using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadProdutos : BaseEditCadForm
{
    produtosDAO produtosDAO;
    public FormEditCadProdutos()
        : base("PRODUTOS")
    {
        produtosDAO = new produtosDAO(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title += "Cadastro/Edição de Produtos";
        subTitulo.Text += "Cadastro/Edição de Produtos";

        if (!Page.IsPostBack)
        {
            unidadesDAO unidadeDAO = new unidadesDAO(_conn);

            comboUnidades.DataSource = unidadeDAO.lista(SessionView.EmpresaSession);
            comboUnidades.DataTextField = "descricao";
            comboUnidades.DataValueField = "sigla";
            comboUnidades.DataBind();
            comboUnidades.Items.Insert(0, new ListItem("Escolha", "0"));
        }

        if (!_cadastro)
        {
            if (!Page.IsPostBack)
            {
                int codProduto = 0;
                int.TryParse(Request.QueryString["id"], out codProduto);
                SProduto produto = produtosDAO.load(codProduto, SessionView.EmpresaSession);
                if (produto != null)
                {
                    textDescricao.Text = produto.descricao;
                    creditoCheckBox.Checked = produto.geraCredito;
                    comboUnidades.SelectedValue = produto.sigla;
                }
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            SProduto produto = new SProduto();
            produto.descricao = textDescricao.Text;
            produto.geraCredito = creditoCheckBox.Checked;
            produto.codEmpresa = SessionView.EmpresaSession;
            produto.sigla = comboUnidades.SelectedValue;
            produtosDAO.insert(produto);
        }
        else
        {
            int codProduto = 0;
            int.TryParse(Request.QueryString["id"], out codProduto);
            SProduto produto = new SProduto();
            produto.codProduto = codProduto;
            produto.descricao = textDescricao.Text;
            produto.geraCredito = creditoCheckBox.Checked;
            produto.codEmpresa = SessionView.EmpresaSession;
            produto.sigla = comboUnidades.SelectedValue;
            produtosDAO.update(produto);
        }

        Response.Redirect("FormGridProdutos.aspx");
    }
}