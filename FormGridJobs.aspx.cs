using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridJobs : BaseGridForm
{
    private Job job;
    private Cliente cliente;
    private DataTable tbJobs = new DataTable("tbJobs");
    private DataTable tbClientes = new DataTable("tbClientes");

    private int? fCliente;
    private string fNome;
    private string fDescricao;

    public FormGridJobs()
        : base("JOB")
    {
        job = new Job(_conn);
        cliente = new Cliente(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
    }

    protected override void verificaTarefas()
    {
        bool aceitaCadastrar = false;
        bool aceitaAlterar = false;
        bool aceitaDeletar = false;

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

        Title += "Jobs";
        subTitulo.Text = "Jobs";
        botaoNovo.NavigateUrl = "FormEditCadJobs.aspx";
        dsDados.Tables.Add(tbJobs);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbjobs"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Cliente", "CE.NOME_RAZAO_SOCIAL"));
            comboOrdenar.Items.Add(new ListItem("Nome", "CJ.NOME"));
            comboOrdenar.Items.Add(new ListItem("Descrição", "CJ.DESCRICAO"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            cliente.lista(ref tbClientes);
            ddlCliente.DataSource = tbClientes;
            ddlCliente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlCliente.DataValueField = "COD_EMPRESA";
            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (ddlCliente.SelectedValue == "0")
            fCliente = null;
        else
            fCliente = Convert.ToInt32(ddlCliente.SelectedValue);

        if (string.IsNullOrEmpty(tbxNome.Text))
            fNome = null;
        else
            fNome = tbxNome.Text;

        if (string.IsNullOrEmpty(tbxDescricao.Text))
            fDescricao = null;
        else
            fDescricao = tbxDescricao.Text;

        totalRegistros = job.totalRegistros(fCliente, fNome, fDescricao);
        tbJobs.Clear();
        job.listaPaginada(ref tbJobs, fCliente, fNome, fDescricao, paginaAtual, ordenacao);
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
            linkAlterar.NavigateUrl = "FormEditCadJobs.aspx?id=" + check.Value;
        }
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
                    selecionados.Add(check.Value);
            }
        }

        for (int i = 0; i < selecionados.Count; i++)
        {
            job.codigo = Convert.ToInt32(selecionados[i]);

            try
            {
                job.deletar();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }
        montaGrid();
    }
}