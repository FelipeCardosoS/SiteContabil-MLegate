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

public partial class FormGridContas : BaseGridForm
{
    private ContaContabil conta;
    Button botaoImportar = new Button();
    GrupoContabil grupoContabil;
    ContaContabil contaContabil;
    DataTable tbGrupoContabil = new DataTable("tbGrupoContabil");
    DataTable tbTiposConta = new DataTable("tbTiposConta");
    private DataTable tbContas = new DataTable("tbContas");

    private int? fGrupo;
    private string fCodigo;
    private string fDescricao;
    private char? fDebCred;
    private int? fAnalitica;
    private string fTipo;

    public FormGridContas()
        : base("CONTA_CONTABIL")
    {
        conta = new ContaContabil(_conn);
        grupoContabil = new GrupoContabil(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Contas Contábeis";
    }

    protected override void verificaTarefas()
    {
        contaContabil = new ContaContabil(_conn);

        bool aceitaDeletar = false;
        bool aceitaAlterar = false;
        bool aceitaCadastrar = false;
        bool aceitaImportar = false;

        int aceitarPlanoDeContas = contaContabil.PlanoConta(Convert.ToInt32(HttpContext.Current.Session["empresa"]));

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "CAD" && 0 == aceitarPlanoDeContas)
                aceitaCadastrar = true;

            if (_tarefas[i].tarefa == "DEL" && 0 == aceitarPlanoDeContas)
                aceitaDeletar = true;

            if (_tarefas[i].tarefa == "IMPORT" && 0 == aceitarPlanoDeContas)
                aceitaImportar = true;

            if (_tarefas[i].tarefa == "ALT")
                aceitaAlterar = true;
        }

        if (!aceitaCadastrar){
            botaoNovo.Enabled = false;
            botaoNovo.Visible = false;
        }

        if (!aceitaDeletar){
            botaoDeletar.Enabled = false;
            botaoDeletar.Visible = false;
        }

        if (!aceitaImportar){
            botaoImportar.Enabled = false;
        }

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

        subTitulo.Text = "Contas Contábeis";
        botaoNovo.NavigateUrl = "FormEditCadContas.aspx";
        dsDados.Tables.Add(tbContas);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbContas"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
            comboOrdenar.Items.Add(new ListItem("Código", "CAD_CONTAS.COD_CONTA"));
            comboOrdenar.Items.Add(new ListItem("Descricao","CAD_CONTAS.DESCRICAO"));
        }

        botaoImportar.ID = "botaoImportar";
        botaoImportar.Text = "Importar";
        botaoImportar.CssClass = "botaoDeletar";
        areaBotoesGrid.Controls.Add(botaoImportar);

        if (!Page.IsPostBack)
        {
            tbGrupoContabil.Clear();
            grupoContabil.lista(ref tbGrupoContabil);

            comboGrupo.DataSource = tbGrupoContabil;
            comboGrupo.DataValueField = "COD_GRUPO_CONTABIL";
            comboGrupo.DataTextField = "DESCRICAO";
            comboGrupo.DataBind();

            comboGrupo.Items.Insert(0, new ListItem("Escolha", "0"));

            tbTiposConta.Clear();
            conta.listaTipos(ref tbTiposConta);
            comboTipo.DataSource = tbTiposConta;
            comboTipo.DataValueField = "COD_TIPO_CONTA";
            comboTipo.DataTextField = "DESCRICAO";
            comboTipo.DataBind();

            comboTipo.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (comboGrupo.SelectedValue == "0")
            fGrupo = null;
        else
            fGrupo = Convert.ToInt32(comboGrupo.SelectedValue);

        if (textCodigo.Text == "")
            fCodigo = null;
        else
            fCodigo = textCodigo.Text;

        if (textDescricao.Text == "")
            fDescricao = null;
        else
            fDescricao = textDescricao.Text;

        if (comboDebCred.SelectedValue == "0")
            fDebCred = null;
        else
            fDebCred = Convert.ToChar(comboDebCred.SelectedValue);

        if (comboAnalitica.SelectedValue == "0")
            fAnalitica = null;
        else
            fAnalitica = Convert.ToInt32(comboAnalitica.SelectedValue);

        if (comboTipo.SelectedValue == "0")
            fTipo = null;
        else
            fTipo = comboTipo.SelectedValue;

        totalRegistros = conta.totalRegistros(fGrupo, fCodigo,fDescricao,fDebCred,fAnalitica,fTipo);
        tbContas.Clear();
        conta.listaPaginada(ref tbContas, fGrupo, fCodigo,fDescricao,fDebCred,fAnalitica,fTipo, paginaAtual, ordenacao);
        repeaterDados.DataSource = tbContas;
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

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");

            linkAlterar.NavigateUrl = "FormEditCadContas.aspx?id=" + check.Value;
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

        for (int i = 0; i < selecionados.Count; i++)
        {
            conta.codigo = selecionados[i];
            
            try
            {
                conta.deletar();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }

        montaGrid();
    }

    protected void botaoIniciar_Click(object sender, EventArgs e)
    {
        Importacao importacao = new Importacao(_conn, ETipoImportacao.CONTA_CONTABIL);
        List<string> erros = importacao.iniciar(arquivo.PostedFile);

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
