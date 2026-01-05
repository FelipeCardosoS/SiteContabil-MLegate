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

public partial class FormGridLanctosContasReceber : BaseGridForm
{
    FolhaLancamento folha;
    ContaContabil conta;
    Job job;
    DataTable tbLanctos = new DataTable("tbLanctos");
    DataTable tbContas = new DataTable("tbContas");
    DataTable tbJobs = new DataTable("tbJobs");
    Nullable<double> fLote;
    Nullable<double> fcodplanilha;
    Nullable<DateTime> fDataInicio;
    Nullable<DateTime> fDataTermino;
    string fDocumento;
    string fConta;
    Nullable<int> fJob;
    Nullable<int> fTerceiro;
    Cliente cliente;
    private DataTable tbClientes = new DataTable("tbClientes");

    public FormGridLanctosContasReceber()
        : base("CAR_GRID_LANCTO")
    {
        folha = new FolhaLancamento(_conn);
        conta = new ContaContabil(_conn);
        job = new Job(_conn);
        folha.modulo = "CAR_INCLUSAO_TITULO";
        cliente = new Cliente(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        string strMascaras = "$(\"#" + textDataInicio.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textDataTermino.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);
    }

    protected override void verificaTarefas()
    {
        bool aceitaDeletar = false;
        bool aceitaAlterar = false;
        bool aceitaCadastrar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "ALT")
                aceitaAlterar = true;

            if (_tarefas[i].tarefa == "DEL")
                aceitaDeletar = true;
        }

        for (int i = 0; i < _modulos.Count; i++)
        {
            if (_modulos[i].codigo == "CAR_INCLUSAO_TITULO")
                aceitaCadastrar = true;
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

        this.Title += "Contas à Receber - Laçamentos";
        subTitulo.Text = "Contas à Receber - Laçamentos";
        botaoNovo.NavigateUrl = "FormGenericTitulos.aspx?modulo=CAR_INCLUSAO_TITULO";
        dsDados.Tables.Add(tbLanctos);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbLanctos"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Job", "cod_job"));
            comboOrdenar.Items.Add(new ListItem("Conta", "cod_conta"));
            comboOrdenar.Items.Add(new ListItem("Data", "data"));
            comboOrdenar.Items.Add(new ListItem("Lote", "lote"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

        conta.listaSinteticas(ref tbContas);
        comboConta.DataSource = tbContas;
        comboConta.DataTextField = "DESCRICAO_COMPLETO";
        comboConta.DataValueField = "COD_CONTA";
        comboConta.DataBind();
        comboConta.Items.Insert(0, new ListItem("Escolha", "0"));

        job.lista(ref tbJobs, 'A');
        comboJob.DataSource = tbJobs;
        comboJob.DataTextField = "DESCRICAO_COMPLETO";
        comboJob.DataValueField = "COD_JOB";
        comboJob.DataBind();
        comboJob.Items.Insert(0, new ListItem("Escolha", "0"));

        cliente.lista(ref tbClientes);
        comboTerceiro.DataSource = tbClientes;
        comboTerceiro.DataTextField = "NOME_RAZAO_SOCIAL";
        comboTerceiro.DataValueField = "COD_EMPRESA";
        comboTerceiro.DataBind();
        comboTerceiro.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textcodplanilha.Text != "")
            fcodplanilha = Convert.ToDouble(textcodplanilha.Text);
        else
            fcodplanilha = null;

        if (textLote.Text != "")
            fLote = Convert.ToDouble(textLote.Text);
        else
            fLote = null;

        if (textDataInicio.Text != "")
            fDataInicio = Convert.ToDateTime(textDataInicio.Text);
        else
            fDataInicio = null;

        if (textDataTermino.Text != "")
            fDataTermino = Convert.ToDateTime(textDataTermino.Text);
        else
            fDataTermino = null;

        if (textDocumento.Text != "")
            fDocumento = Convert.ToString(textDocumento.Text);
        else
            fDocumento = null;

        if (comboConta.SelectedValue != "0")
            fConta = comboConta.SelectedValue;
        else
            fConta = null;

        if (comboJob.SelectedValue != "0")
            fJob = Convert.ToInt32(comboJob.SelectedValue);
        else
            fJob = null;

        if (comboTerceiro.SelectedValue != "0")
            fTerceiro = Convert.ToInt32(comboTerceiro.SelectedValue);
        else
            fTerceiro = null;

        totalRegistros = folha.totalRegistros(fLote, fDataInicio, fDataTermino, fConta, fJob, fTerceiro, fcodplanilha);
        tbLanctos.Clear();
        folha.listaPaginada(ref tbLanctos, fLote, fDataInicio, fDataTermino, fConta, fJob, fTerceiro, fDocumento, paginaAtual, ordenacao, fcodplanilha);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void botaoFiltrar_Click(object sender, EventArgs e)
    {

        base.botaoFiltrar_Click(sender, e);
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;
            DataRowView rowItem = (DataRowView)e.Item.DataItem;
            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");

            linkAlterar.NavigateUrl = "FormGenericTitulos.aspx?modulo=CAR_INCLUSAO_TITULO&lote=" + check.Value;

            HyperLink linkHistorico = (HyperLink)item.FindControl("linkHistorico");
            HtmlContainerControl tip = (HtmlContainerControl)item.FindControl("tip");
            Empresa empresa = new Empresa(_conn);
            empresa.codigo = Convert.ToInt32(rowItem["terceiro"]);
            empresa.load();

            string historic = rowItem["historico"].ToString() + " - Nº Doc: " + rowItem["numero_documento"] + " - Terceiro: " + empresa.nome;

            if (historic.Length > 60)
                linkHistorico.Text = historic.Substring(0, 60) + "<strong>...</strong>";
            else
                linkHistorico.Text = historic;


            tip.InnerHtml = historic;

            Literal literalStatusBaixa = (Literal)item.FindControl("literalStatusBaixa");

            FolhaLancamento folha = new FolhaLancamento(_conn);
            literalStatusBaixa.Text = folha.getStatusBaixas(Convert.ToDouble(check.Value));

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
                "$(\"#" + linkHistorico.ClientID + "\").tooltip({ tip: '#" + tip.ClientID + "'});", true);


            HyperLink linkTerceiros = (HyperLink)item.FindControl("linkTerceiros");
            HtmlContainerControl tipTerceiros = (HtmlContainerControl)item.FindControl("tipTerceiros");

            if (rowItem["terceiros"].ToString().Length > 60)
                linkTerceiros.Text = rowItem["terceiros"].ToString().Substring(0, 60) + "<strong>...</strong>";
            else
                linkTerceiros.Text = rowItem["terceiros"].ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
                "$(\"#" + linkTerceiros.ClientID + "\").tooltip({ tip: '#" + tipTerceiros.ClientID + "'});", true);
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
                {
                    selecionados.Add(check.Value);
                }
            }
        }

        List<string> erros = new List<string>();

        for (int i = 0; i < selecionados.Count; i++)
        {
            List<string> err = folha.deletar(Convert.ToDouble(selecionados[i]));
            for (int x = 0; x < err.Count; x++)
            {
                erros.Add(err[x]);
            }
        }
        if (erros.Count == 0)
        {
            montaGrid();
        }
        else
        {
            errosFormulario(erros);
        }
    }
}
