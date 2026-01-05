using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridNaturezaOperacao : BaseGridForm
{
    private NaturezaOperacao natureza_operacao;
    private Emitente emitente;
    private DataTable tbNaturezaOperacao = new DataTable("tbNaturezaOperacao");
    private DataTable tbEmitentes = new DataTable("tbEmitentes");

    private string fNome;
    private string fDescricao;
    private string fNatureza_Operacao;
    private int? fEmitente;

    public FormGridNaturezaOperacao()
        : base("FATURAMENTO_CADASTROS_NATUREZA_OPERACAO")
    {
        natureza_operacao = new NaturezaOperacao(_conn);
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
        this.Title += "Natureza da Operação";
        subTitulo.Text = "Natureza da Operação";
        botaoNovo.NavigateUrl = "FormEditCadNaturezaOperacao.aspx";

        dsDados.Tables.Add(tbNaturezaOperacao);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbNaturezaOperacao"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "CNO.NOME"));
            comboOrdenar.Items.Add(new ListItem("Descrição", "CNO.DESCRICAO"));
            comboOrdenar.Items.Add(new ListItem("Natureza da Operação (Prefeitura)", "CNO.NATUREZA_OPERACAO"));
            comboOrdenar.Items.Add(new ListItem("Emitente", "dbo.Emitentes_Natureza_Operacao(CNO.COD_NATUREZA_OPERACAO)"));
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

        if (textNaturezaOperacao.Text == "")
            fNatureza_Operacao = null;
        else
            fNatureza_Operacao = textNaturezaOperacao.Text;

        if (comboEmitente.SelectedValue == "0")
            fEmitente = null;
        else
            fEmitente = Convert.ToInt32(comboEmitente.SelectedValue);

        totalRegistros = natureza_operacao.totalRegistros(fNome, fDescricao, fNatureza_Operacao, fEmitente);
        tbNaturezaOperacao.Clear();
        natureza_operacao.listaPaginada(ref tbNaturezaOperacao, fNome, fDescricao, fNatureza_Operacao, fEmitente, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadNaturezaOperacao.aspx?id=" + check.Value;
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
                            natureza_operacao.cod_natureza_operacao = Convert.ToInt32(check.Value);
                            natureza_operacao.nome = nome.Text.Replace("'", " ");
                            erros.AddRange(natureza_operacao.deletar());
                        }
                        catch
                        {
                            erros.Add("Natureza da Operação " + natureza_operacao.nome + ": Não foi possivel excluir, pois o mesmo está sendo utilizado.");
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