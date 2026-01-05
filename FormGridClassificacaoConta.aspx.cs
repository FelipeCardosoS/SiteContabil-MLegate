using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormGridClassificacaoConta : BaseGridForm
{
    public FormGridClassificacaoConta()
        : base("CLASSIFICACAO_CONTA")
    {
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Classificação de Contas";
            botaoNovo.NavigateUrl = "FormEditCadClassificacaoConta.aspx";

            if (!Page.IsPostBack)
            {
                comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
                comboOrdenar.Items.Add(new ListItem("Código", "COD_CLASSIFICACAO"));
                comboOrdenar.Items.Add(new ListItem("Descricao", "DESCRICAO"));
            }
        }
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

        string descricao = "";
        string codigo = "";

        if (descricaoText.Text != "")
        {
            descricao = descricaoText.Text;
        }

        if (codigoText.Text != "")
        {
            codigo = codigoText.Text;
        }
        classificacaoContaDAO classificacaoContaDAO = new classificacaoContaDAO(_conn);
        totalRegistros = classificacaoContaDAO.totalRegistros(codigo, descricao, SessionView.EmpresaSession);
        repeaterDados.DataSource = classificacaoContaDAO.lista(codigo, descricao, SessionView.EmpresaSession, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        List<string> selecionados = new List<string>();
        classificacaoContaDAO classificacaoContaDAO = new classificacaoContaDAO(_conn);
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
            string cod = selecionados[i];
            
            try
            {
                classificacaoContaDAO.delete(cod, SessionView.EmpresaSession);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }

        montaGrid();
    }
}