using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridTipoDespesas : BaseGridForm
{
    private TipoDespesa tipoDespesa;
    private DataTable tbTipoDespesa = new DataTable("tbTipoDespesa");
    private string fDescricao;
    private string fUnidade;

    public FormGridTipoDespesas() : base("TIPO_DESPESA")
	{
        tipoDespesa = new TipoDespesa(_conn);
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

    protected override void montaTela()
    {
        base.montaTela();

        this.Title += "Tipos de Despesa";
        subTitulo.Text = "Tipos de Despesa";
        botaoNovo.NavigateUrl = "FormEditCadDespesas.aspx";
        dsDados.Tables.Add(tbTipoDespesa);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbTipoDespesa"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Descrição", "DESCRICAO"));
            comboOrdenar.Items.Add(new ListItem("Unidade", "UNIDADE"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }
    protected override void montaGrid()
    {
        base.montaGrid();

        if (textDescricao.Text == "")
            fDescricao = null;
        else
            fDescricao = textDescricao.Text;

        if (textUnidade.Text == "")
            fUnidade = null;
        else
            fUnidade = textUnidade.Text;

        totalRegistros = tipoDespesa.totalRegistros(fDescricao, fUnidade);
        tipoDespesa.listaPaginada(ref tbTipoDespesa, fDescricao, fUnidade, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadTipoDespesas.aspx?id=" + check.Value;
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
            try
            {
                tipoDespesa.deleta(Convert.ToInt32(selecionados[i]));
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }
        montaGrid();
    }

}