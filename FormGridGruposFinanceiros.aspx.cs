using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridGrupoFinanceiro : BaseGridForm
{
    GrupoFinanceiro grupoFinanceiro;
    DataTable tableGrupos;
    public FormGridGrupoFinanceiro()
    : base("GRUPO_FINANCEIRO")
    {
        grupoFinanceiro = new GrupoFinanceiro(_conn);
        tableGrupos = new DataTable("grupo_financeiro");
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
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

    protected override void montaGrid()
    {
        string descricao = String.Empty;
        base.montaGrid();

        if (textDescricao.Text != "")
            descricao = textDescricao.Text.Trim();


        totalRegistros = grupoFinanceiro.totalRegistros(descricao);
        grupoFinanceiro.lista(ref tableGrupos, paginaAtual, descricao, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    
    }

    protected override void montaTela()
    {
        base.montaTela();

        this.Title += "Grupos Financeiros";
        subTitulo.Text = "Grupos Financeiros";
        botaoNovo.NavigateUrl = "FormEditCadGruposFinanceiros.aspx";
        //dsDados.Tables.Add(tableGrupos);
        repeaterDados.DataSource = tableGrupos;
        repeaterDados.DataMember = tableGrupos.TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("A - Z", "ASC"));
            comboOrdenar.Items.Add(new ListItem("Z - A", "DESC"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", ""));
        }
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");

            linkAlterar.NavigateUrl = "FormEditCadGruposFinanceiros.aspx?id=" + check.Value;
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
            grupoFinanceiro.nome = Convert.ToString(selecionados[i]);
            try
            {
                grupoFinanceiro.deletar();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }
        montaGrid();
    }
}