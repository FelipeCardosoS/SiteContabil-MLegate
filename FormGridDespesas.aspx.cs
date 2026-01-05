using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGridCadDespesas : BaseGridForm
{
	private Despesa despesa;
	private DataTable tbDespesas = new DataTable("tbDespesas");

	private string fDescricao;
	private string fTipoDespesa;

	public FormGridCadDespesas() : base("DESPESA")
	{
		despesa = new Despesa(_conn);
	}
	protected void Page_Load(object sender, EventArgs e)
	{

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

        this.Title += "Despesas";
        subTitulo.Text = "Despesas";
        botaoNovo.NavigateUrl = "FormEditCadDespesas.aspx";
        dsDados.Tables.Add(tbDespesas);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbDespesas"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        TipoDespesa tipoDespesa = new TipoDespesa(_conn);
        List<TipoDespesa> listaTiposDespesa = tipoDespesa.lista();

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Descrição", "DESCRICAO"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            ddlTipoDespesa.DataSource = listaTiposDespesa;
            ddlTipoDespesa.DataValueField = "CodTipoDespesa";
            ddlTipoDespesa.DataTextField = "Display_Descricao";
            ddlTipoDespesa.DataBind();
            ddlTipoDespesa.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        fTipoDespesa = Convert.ToString(ddlTipoDespesa.SelectedValue).Equals("0") ? null : Convert.ToString(ddlTipoDespesa.SelectedValue);


        if (textDescricao.Text == "")
            fDescricao = null;
        else
            fDescricao = textDescricao.Text; 
        
        totalRegistros = despesa.totalRegistros(fDescricao, fTipoDespesa);
        tbDespesas.Clear();
        despesa.listaPaginada(ref tbDespesas, fDescricao, fTipoDespesa, paginaAtual, ordenacao);
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


            linkAlterar.NavigateUrl = "FormEditCadDespesas.aspx?id=" + check.Value;
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
                despesa.deleta(Convert.ToInt32(selecionados[i]));
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }
        montaGrid();
    }

}