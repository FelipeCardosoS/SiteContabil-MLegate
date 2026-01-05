using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridTributos : BaseGridForm
{
    private Tributo tributo;
    private Emitente emitente;
    private DataTable tbTributos = new DataTable("tbTributos");
    private DataTable tbEmitentes = new DataTable("tbEmitentes");

    private string fNome;
    private double? fAliquota;
    private int? fEmitente;

    public FormGridTributos()
        : base("FATURAMENTO_CADASTROS_TRIBUTOS")
    {
        tributo = new Tributo(_conn);
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
        Title += "Tributos";
        subTitulo.Text = "Impostos Sobre Vendas";
        botaoNovo.NavigateUrl = "FormEditCadTributos.aspx";

        dsDados.Tables.Add(tbTributos);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbTributos"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "CT.NOME"));
            comboOrdenar.Items.Add(new ListItem("Alíquota", "CT.ALIQUOTA"));
            comboOrdenar.Items.Add(new ListItem("Emitente", "dbo.Emitentes_Tributos(CT.COD_TRIBUTO)"));
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

        if (textAliquota.Text == "" || textAliquota.Text == "," || textAliquota.Text == ".")
            fAliquota = null;
        else
            fAliquota = Convert.ToDouble(textAliquota.Text.Replace(".", ","));

        if (comboEmitente.SelectedValue == "0")
            fEmitente = null;
        else
            fEmitente = Convert.ToInt32(comboEmitente.SelectedValue);

        totalRegistros = tributo.totalRegistros(fNome, fAliquota, fEmitente);
        tbTributos.Clear();
        tributo.listaPaginada(ref tbTributos, fNome, fAliquota, fEmitente, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadTributos.aspx?id=" + check.Value;
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
                            tributo.cod_tributo = Convert.ToInt32(check.Value);
                            tributo.nome = nome.Text.Replace("'", " ");
                            erros.AddRange(tributo.deletar());
                        }
                        catch
                        {
                            erros.Add("Tributo " + tributo.nome + ": Não foi possivel excluir, pois o mesmo está sendo utilizado.");
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