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

public partial class FormGridFechamento : BaseGridForm
{
    Fechamento fechamento;
    Button botaoFechar = new Button();
    DataTable tbFechamento = new DataTable("tbFechamento");

    public FormGridFechamento()
        : base("FECHAMENTO")
    {
        fechamento = new Fechamento(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
       
        botaoFechar.ID = "botaoFechar";
        botaoFechar.Text = "Abrir Período";
        botaoFechar.CssClass = "botaoDeletar";

        string strMascaras = "$(\"#" + textDataInicio.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textDataTermino.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textPeriodo.ClientID + "\").mask(\"99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);
    }

    protected override void verificaTarefas()
    {
        bool aceitaDeletar = false;
        bool aceitaCadastrar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "CAD")
                aceitaCadastrar = true;

            if (_tarefas[i].tarefa == "DEL")
                aceitaDeletar = true;
        }

        if (!aceitaCadastrar)
            botaoFechar.Enabled = false;
        if (!aceitaDeletar)
            botaoDeletar.Enabled = false;
    }

    protected override void montaTela()
    {
        base.montaTela();

        this.Title += "Liberação de Periodo";
        subTitulo.Text = "Liberação de Periodo";
        botaoNovo.Visible = false;

        areaBotoesGrid.Controls.AddAt(0,botaoFechar);

        dsDados.Tables.Add(tbFechamento);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbFechamento"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Descrição", "DESCRICAO"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        totalRegistros = fechamento.totalRegistros();
        tbFechamento.Clear();
        fechamento.listaPaginada(ref tbFechamento, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
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
            fechamento.deletar(selecionados[i]);
        }

        montaGrid();
    }

    protected void botaoExecutaFechamento_Click(object sender, EventArgs e)
    {
        List<string> erros = new List<string>();
        try
        {

            List<string> errosGeraSaldo = fechamento.geraSaldos(textPeriodo.Text);
            for (int i = 0; i < errosGeraSaldo.Count; i++)
            {
                erros.Add(errosGeraSaldo[i]);
            }

            if (errosGeraSaldo.Count == 0)
            {
                List<string> errosNovo = fechamento.novo(textPeriodo.Text);
                for (int i = 0; i < errosNovo.Count; i++)
                {
                    erros.Add(errosNovo[i]);
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
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}
