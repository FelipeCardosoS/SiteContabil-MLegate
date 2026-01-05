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

public partial class FormEditCadContatosEmpresa : BaseEditCadForm
{
    private Empresa empresa;
    private ContatoEmpresa contatoEmpresa;
    private FuncaoCliente funcoesCliente;
    private DataTable tbFuncoesCliente = new DataTable("tbFuncoesCliente");
    private DataTable tbEmpresas = new DataTable("tbEmpresas");

    public FormEditCadContatosEmpresa()
        : base("CONTATO_EMPRESA")
    {
        contatoEmpresa = new ContatoEmpresa(_conn);
        funcoesCliente = new FuncaoCliente(_conn);
        empresa = new Empresa(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Contato";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Contato";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            H_COD_CONTATO.Value = "0";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                contatoEmpresa.codigo = Convert.ToInt32(Request.QueryString["id"]);
                contatoEmpresa.load();

                H_COD_CONTATO.Value = contatoEmpresa.codigo.ToString();
                comboEmpresa.SelectedValue = contatoEmpresa.empresa.ToString();
                comboFuncao.SelectedValue = contatoEmpresa.funcao.ToString();
                textNomeCompleto.Text = contatoEmpresa.nome;
                textCep.Text = contatoEmpresa.cep;
                textEndereco.Text = contatoEmpresa.endereco;
                textNumero.Text = contatoEmpresa.numero;
                textBairro.Text = contatoEmpresa.bairro;
                textCidade.Text = contatoEmpresa.cidade;
                textEstado.Text = contatoEmpresa.estado;
                textTelefone.Text = contatoEmpresa.telefone;
                textEmail.Text = contatoEmpresa.email;
                radioEnviar.SelectedValue = contatoEmpresa.enviar.ToString();
            }
        }

        string strMascaras = "$(\"#" + textCep.ClientID + "\").mask(\"99999-999\");";
        strMascaras += "$(\"#" + textTelefone.ClientID + "\").mask(\"(99) 9999-9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Contatos da Empresa", "FormGridContatosEmpresa.aspx");
        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {
            funcoesCliente.lista(ref tbFuncoesCliente);
            comboFuncao.DataSource = tbFuncoesCliente;
            comboFuncao.DataTextField = "DESCRICAO";
            comboFuncao.DataValueField = "COD_FUNCAO";
            comboFuncao.DataBind();
            comboFuncao.Items.Insert(0, new ListItem("Escolha", "0"));

            empresa.listaFornecedoresClientes(ref tbEmpresas);
            comboEmpresa.DataSource = tbEmpresas;
            comboEmpresa.DataTextField = "NOME_RAZAO_SOCIAL";
            comboEmpresa.DataValueField = "COD_EMPRESA";
            comboEmpresa.DataBind();
            comboEmpresa.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            contatoEmpresa.empresa = Convert.ToInt32(comboEmpresa.SelectedValue);
            contatoEmpresa.funcao = Convert.ToInt32(comboFuncao.SelectedValue);
            contatoEmpresa.nome = textNomeCompleto.Text;
            contatoEmpresa.cep = limpaString(textCep.Text);
            contatoEmpresa.endereco = textEndereco.Text;
            contatoEmpresa.numero = textNumero.Text;
            contatoEmpresa.bairro = textBairro.Text;
            contatoEmpresa.cidade = textCidade.Text;
            contatoEmpresa.estado = textEstado.Text;
            contatoEmpresa.telefone = limpaString(textTelefone.Text);
            contatoEmpresa.email = textEmail.Text;
            contatoEmpresa.enviar = Convert.ToInt32(radioEnviar.SelectedValue);

            List<string> erros = contatoEmpresa.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridContatosEmpresa.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            contatoEmpresa.empresa = Convert.ToInt32(comboEmpresa.SelectedValue);
            contatoEmpresa.codigo = Convert.ToInt32(H_COD_CONTATO.Value);
            contatoEmpresa.funcao = Convert.ToInt32(comboFuncao.SelectedValue);
            contatoEmpresa.nome = textNomeCompleto.Text;
            contatoEmpresa.cep = limpaString(textCep.Text);
            contatoEmpresa.endereco = textEndereco.Text;
            contatoEmpresa.numero = textNumero.Text;
            contatoEmpresa.bairro = textBairro.Text;
            contatoEmpresa.cidade = textCidade.Text;
            contatoEmpresa.estado = textEstado.Text;
            contatoEmpresa.telefone = limpaString(textTelefone.Text);
            contatoEmpresa.email = textEmail.Text;
            contatoEmpresa.enviar = Convert.ToInt32(radioEnviar.SelectedValue);

            List<string> erros = contatoEmpresa.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridContatosEmpresa.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
