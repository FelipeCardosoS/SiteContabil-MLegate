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

public partial class FormGridContatosEmpresa : BaseGridForm
{
    private Empresa empresa;
    private ContatoEmpresa contatoEmpresa;
    private DataTable tbContatosEmpresa = new DataTable("tbContatosEmpresa");
    private DataTable tbEmpresas = new DataTable("tbEmpresas");

    private string fNomeCompleto;
    private int? fEmpresa;
    private string fEmail;

    public FormGridContatosEmpresa()
        : base("CONTATO_EMPRESA")
    {
        contatoEmpresa = new ContatoEmpresa(_conn);
        empresa = new Empresa(_conn);
    }


    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Contatos";
    }

    protected override void verificaTarefas()
    {
        bool aceitaDeletar = false;
        bool aceitaAlterar = false;
        bool aceitaCadastrar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "CAD")
                aceitaCadastrar = true;

            if (_tarefas[i].tarefa == "ALT")
                aceitaAlterar = true;

            if (_tarefas[i].tarefa == "DEL")
                aceitaDeletar = true;
        }

        if (!aceitaCadastrar)
            botaoNovo.Enabled = false;
        if (!aceitaDeletar)
            botaoDeletar.Enabled = false;

        if (!aceitaAlterar)
        {
            foreach (RepeaterItem item in repeaterDados.Items)
            {
                if (item.ItemType != ListItemType.Separator)
                {
                    HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");
                    linkAlterar.Enabled = false;
                }
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        subTitulo.Text = "Contatos da Empresa";
        botaoNovo.NavigateUrl = "FormEditCadContatosEmpresa.aspx";
        dsDados.Tables.Add(tbContatosEmpresa);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbContatosEmpresa"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "NOME_COMPLETO"));
            comboOrdenar.Items.Add(new ListItem("E-mail", "EMAIL"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
        }

        if (!Page.IsPostBack)
        {
            empresa.listaFornecedoresClientes(ref tbEmpresas);
            comboEmpresa.DataSource = tbEmpresas;
            comboEmpresa.DataTextField = "NOME_RAZAO_SOCIAL";
            comboEmpresa.DataValueField = "COD_EMPRESA";
            comboEmpresa.DataBind();
            comboEmpresa.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textNomeCompleto.Text == "")
            fNomeCompleto = null;
        else
            fNomeCompleto = textNomeCompleto.Text;

        if (comboEmpresa.SelectedValue == "0")
            fEmpresa = null;
        else
            fEmpresa = Convert.ToInt32(comboEmpresa.SelectedValue);

        if (textEmail.Text == "")
            fEmail = null;
        else
            fEmail = textEmail.Text;

        totalRegistros = contatoEmpresa.totalRegistros(fNomeCompleto, fEmpresa, fEmail);
        tbContatosEmpresa.Clear();
        contatoEmpresa.listaPaginada(ref tbContatosEmpresa,fNomeCompleto, fEmpresa, fEmail, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");
            Literal lTelefone = (Literal)item.FindControl("lTelefone");

            if (lTelefone != null)
            {
                if (lTelefone.Text.Length > 0)
                {
                    string tempTelefone = lTelefone.Text;
                    lTelefone.Text = "(" + tempTelefone.Substring(0, 2) + ") " + tempTelefone.Substring(2, 4) + "-" + tempTelefone.Substring(4, 4);
                }
            }

            linkAlterar.NavigateUrl = "FormEditCadContatosEmpresa.aspx?id=" + check.Value;
        }
    }

    protected override void botaoFiltrar_Click(object sender, EventArgs e)
    {

        base.botaoFiltrar_Click(sender, e);
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        List<string> selecionados = new List<string>();
        foreach (RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType != ListItemType.Separator)
            {
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                if (check.Checked)
                {
                    selecionados.Add(check.Value);
                }
            }
        }

        for (int i = 0; i < selecionados.Count; i++)
        {
            contatoEmpresa.codigo = Convert.ToInt32(selecionados[i]);            
            try
            {
                contatoEmpresa.deletar();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }

        montaGrid();
    }
}
