using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridNarrativas : BaseGridForm
{
    private Narrativa narrativa;
    private Emitente emitente;
    private DataTable tbNarrativas = new DataTable("tbNarrativas");
    private DataTable tbEmitentes = new DataTable("tbEmitentes");

    private string fNome;
    private string fDescricao;
    private int? fEmitente;

    public FormGridNarrativas()
        : base("FATURAMENTO_CADASTROS_NARRATIVAS")
    {
        narrativa = new Narrativa(_conn);
        emitente = new Emitente(_conn);
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
        this.Title += "Narrativas";
        subTitulo.Text = "Narrativas";
        botaoNovo.NavigateUrl = "FormEditCadNarrativas.aspx";

        dsDados.Tables.Add(tbNarrativas);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbNarrativas"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "CN.NOME"));
            comboOrdenar.Items.Add(new ListItem("Descrição", "CN.DESCRICAO"));
            comboOrdenar.Items.Add(new ListItem("Emitente", "dbo.Emitentes_Narrativas(CN.COD_NARRATIVA)"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            emitente.lista_Emitentes(ref tbEmitentes);
            comboEmitente.DataSource = tbEmitentes;
            comboEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            comboEmitente.DataValueField = "COD_EMPRESA";
            comboEmitente.DataBind();
            comboEmitente.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textNome.Text == "")
            fNome = null;
        else
            fNome = textNome.Text;

        if (textDescricao.Text == "")
            fDescricao = null;
        else
            fDescricao = textDescricao.Text;

        if (comboEmitente.SelectedValue == "0")
            fEmitente = null;
        else
            fEmitente = Convert.ToInt32(comboEmitente.SelectedValue);

        totalRegistros = narrativa.totalRegistros(fNome, fDescricao, fEmitente);
        tbNarrativas.Clear();
        narrativa.listaPaginada(ref tbNarrativas, fNome, fDescricao, fEmitente, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadNarrativas.aspx?id=" + check.Value;
        }
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada
        List<string> erros = new List<string>();

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (erros.Count == 0)
        {
            foreach (RepeaterItem item in repeaterDados.Items)
            {
                if (item.ItemType != ListItemType.Separator)
                {
                    HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                    Label nome = (Label)item.FindControl("nome");

                    if (check.Checked)
                    {
                        try
                        {
                            narrativa.cod_narrativa = Convert.ToInt32(check.Value);
                            narrativa.nome = nome.Text.Replace("'", " ");
                            erros.AddRange(narrativa.deletar());
                        }
                        catch
                        {
                            erros.Add("Narrativa " + narrativa.nome + ": Não foi possivel excluir, pois o mesmo está sendo utilizado.");
                        }
                    }
                }
            }
            montaGrid();

            if (erros.Count > 0)
                errosFormulario(erros);
        }
        else
            errosFormulario(erros);
    }
}