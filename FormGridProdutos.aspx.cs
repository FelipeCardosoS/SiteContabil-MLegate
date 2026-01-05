using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormGridProdutos : BaseGridForm
{
    produtosDAO produtosDAO;

    public FormGridProdutos()
        : base("PRODUTOS") 
    {
        produtosDAO = new produtosDAO(_conn);
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

        subTitulo.Text = "Produtos";
        botaoNovo.NavigateUrl = "FormEditCadProdutos.aspx";

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
            comboOrdenar.Items.Add(new ListItem("Código", "COD_PRODUTO"));
            comboOrdenar.Items.Add(new ListItem("Descricao", "DESCRICAO"));
        }
    }

    protected override void montaGrid()
    {
        
        string descricao = "";
        bool? geraCredito = null;
        ordenacao = comboOrdenar.SelectedValue.Equals("0") ? "" : comboOrdenar.SelectedValue;

        if (textDescricao.Text != "")
        {
            descricao = textDescricao.Text;
        }

        if (comboCredito.SelectedValue != "")
        {
            geraCredito = Convert.ToBoolean(comboCredito.SelectedValue);
        }

        totalRegistros = produtosDAO.totalRegistros(descricao, geraCredito,SessionView.EmpresaSession);
        repeaterDados.DataSource = produtosDAO.lista(descricao, geraCredito,SessionView.EmpresaSession, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
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
            int cod = 0;
            int.TryParse(selecionados[i], out cod);            
            try
            {
                produtosDAO.delete(cod, SessionView.EmpresaSession);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }

        montaGrid();
    }
}