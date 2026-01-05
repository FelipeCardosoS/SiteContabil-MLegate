using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridServicos : BaseGridForm
{
    private Servico servico;
    private Emitente emitente;
    private DataTable tbServicos = new DataTable("tbServicos");
    private DataTable tbEmitentes = new DataTable("tbEmitentes");

    private string fNome;
    private string fCod_Servico_Prefeitura;
    private double? fImpostos;
    private int? fEmitente;

    public FormGridServicos()
        : base("FATURAMENTO_CADASTROS_SERVICOS")
    {
        servico = new Servico(_conn);
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
        Title += "Serviços";
        subTitulo.Text = "Serviços";
        botaoNovo.NavigateUrl = "FormEditCadServicos.aspx";

        dsDados.Tables.Add(tbServicos);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbServicos"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "CS.NOME"));
            comboOrdenar.Items.Add(new ListItem("Código de Serviço (Prefeitura)", "CS.COD_SERVICO_PREFEITURA"));
            comboOrdenar.Items.Add(new ListItem("% Impostos", "CS.IMPOSTOS"));
            comboOrdenar.Items.Add(new ListItem("Emitente", "dbo.Emitentes_Servicos(CS.COD_SERVICO)"));
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

        if (textCodServicoPrefeitura.Text == "")
            fCod_Servico_Prefeitura = null;
        else
            fCod_Servico_Prefeitura = textCodServicoPrefeitura.Text;

        if (textImpostos.Text == "" || textImpostos.Text == "," || textImpostos.Text == ".")
            fImpostos = null;
        else
            fImpostos = Convert.ToDouble(textImpostos.Text.Replace(".", ","));

        if (comboEmitente.SelectedValue == "0")
            fEmitente = null;
        else
            fEmitente = Convert.ToInt32(comboEmitente.SelectedValue);

        totalRegistros = servico.totalRegistros(fNome, fCod_Servico_Prefeitura, fImpostos, fEmitente);
        tbServicos.Clear();
        servico.listaPaginada(ref tbServicos, fNome, fCod_Servico_Prefeitura, fImpostos, fEmitente, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadServicos.aspx?id=" + check.Value;
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
                            servico.cod_servico = Convert.ToInt32(check.Value);
                            servico.nome = nome.Text.Replace("'", " ");
                            erros.AddRange(servico.deletar());
                        }
                        catch
                        {
                            erros.Add("Serviço " + servico.nome + ": Não foi possivel excluir, pois o mesmo está sendo utilizado.");
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