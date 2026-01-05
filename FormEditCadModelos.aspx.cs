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

public partial class FormEditCadModelos : BaseEditCadForm
{
    private Modelo modelo;
    private Empresa empresa;
    private DataTable tbFornecedoresClientes = new DataTable("tbFornecedoresClientes");

    public FormEditCadModelos()
        : base("MODELO_LANCAMENTO")
    {
        empresa = new Empresa(_conn);
        modelo = new Modelo(_conn);
    }

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


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            H_COD_MODELO.Value = "0";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                modelo.codigo = Convert.ToInt32(Request.QueryString["id"]);
                modelo.load();

                H_COD_MODELO.Value = modelo.codigo.ToString();
                textNome.Text = modelo.nome;
                radioTipo.SelectedValue = modelo.tipo;
                textObservacao.Text = modelo.observacao;
                checkDefault.Checked = modelo.padraoDefault;
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Modelos de Lançamento", "FormGridModelos.aspx");
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

            modelo.nome = textNome.Text;
            modelo.tipo = radioTipo.SelectedValue;
            modelo.observacao = textObservacao.Text;
            modelo.padraoDefault = checkDefault.Checked;

            List<string> erros = modelo.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridModelos.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            modelo.codigo = Convert.ToInt32(H_COD_MODELO.Value);
            modelo.nome = textNome.Text;
            modelo.tipo = radioTipo.SelectedValue;
            modelo.observacao = textObservacao.Text;
            modelo.padraoDefault = checkDefault.Checked;

            List<string> erros = modelo.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridModelos.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
