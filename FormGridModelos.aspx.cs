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

public partial class FormGridModelos : BaseGridForm
{
    private Modelo modelo;
    private DataTable tbModelos = new DataTable("tbModelos");

    private string fNome;
    private string fTipo;
    private string fDefault;

    public FormGridModelos()
        : base("MODELO_LANCAMENTO")
    {
        modelo = new Modelo(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
    }

    protected override void verificaTarefas()
    {
        bool aceitaAlterar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {

            if (_tarefas[i].tarefa == "ALT")
                aceitaAlterar = true;
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

        this.Title += "Modelos de Lançamento";
        subTitulo.Text = "Modelos de Lançamento";
        botaoNovo.NavigateUrl = "FormEditCadModelos.aspx";
        dsDados.Tables.Add(tbModelos);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbModelos"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome", "NOME"));
            comboOrdenar.Items.Add(new ListItem("Tipo", "TIPO"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textNome.Text == "")
            fNome = null;
        else
            fNome = textNome.Text;

        if (comboTipo.SelectedValue == "0")
            fTipo = null;
        else
            fTipo = comboTipo.SelectedValue;

        if (comboDefault.SelectedValue == "0")
            fDefault = null;
        else
            fDefault = comboDefault.SelectedValue;


        totalRegistros = modelo.totalRegistros(fNome, fTipo, fDefault);
        tbModelos.Clear();
        modelo.listaPaginada(ref tbModelos,fNome,fTipo,fDefault, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HiddenField hiddenTipo = (HiddenField)item.FindControl("hiddenTipo");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");
            HyperLink linkLanctos = (HyperLink)item.FindControl("linkLanctos");
            Literal lDefault = (Literal)item.FindControl("lDefault");

            if (lDefault.Text == "True")
            {
                lDefault.Text = "<img src='Imagens/icones/tick.gif' alt='' />";
            }
            else
            {
                lDefault.Text = " - ";
            }

            linkAlterar.NavigateUrl = "FormEditCadModelos.aspx?id=" + check.Value;
            switch (hiddenTipo.Value)
            {
                case "CP":
                    linkLanctos.NavigateUrl = "FormGenericTitulos.aspx?modulo=MODELO_LANCAMENTO&tipo=CP&modelo=" + check.Value;
                    break;
                case "CR":
                    linkLanctos.NavigateUrl = "FormGenericTitulos.aspx?modulo=MODELO_LANCAMENTO&tipo=CR&modelo=" + check.Value;
                    break;
                case "C":
                    linkLanctos.NavigateUrl = "FormGenericTitulos.aspx?modulo=MODELO_LANCAMENTO&tipo=C&modelo=" + check.Value;
                    break;
            }
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
            modelo.codigo = Convert.ToInt32(selecionados[i]);
            List<string> err = modelo.deletar();
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
