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

public partial class FormEditCadFuncoesCliente : BaseEditCadForm
{
    private FuncaoCliente funcaoCliente;

    public FormEditCadFuncoesCliente()
        : base("FUNCAO_CLIENTE")
    {
        funcaoCliente = new FuncaoCliente(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Função";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Função";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            H_COD_FUNCAO.Value = "0";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                funcaoCliente.codigo = Convert.ToInt32(Request.QueryString["id"]);
                funcaoCliente.load();

                H_COD_FUNCAO.Value = funcaoCliente.codigo.ToString();
                textDescricao.Text = funcaoCliente.descricao;
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Funções de Cliente", "FormGridFuncoesCliente.aspx");
        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {

        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {

            funcaoCliente.descricao = textDescricao.Text;

            List<string> erros = funcaoCliente.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridFuncoesCliente.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            funcaoCliente.codigo = Convert.ToInt32(H_COD_FUNCAO.Value);
            funcaoCliente.descricao = textDescricao.Text;

            List<string> erros = funcaoCliente.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridFuncoesCliente.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
