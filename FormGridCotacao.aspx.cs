using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormGridCotacao : BaseGridForm
{
    cotacaoDAO cotacaoDAO;
    moedaDAL moedaDAL;
    
    public FormGridCotacao() : base("COTACAO")
    {
        cotacaoDAO = new cotacaoDAO(_conn);
        moedaDAL = new moedaDAL(_conn);
    }
    
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

    }

    protected override void montaTela()
    {
        base.montaTela();
        subTitulo.Text = "Cotação";
        botaoNovo.NavigateUrl = "FormEditCadCotacao.aspx";
    }

    protected override void montaGrid()
    {
        if (!Page.IsPostBack)
        {
            comboMoeda.DataSource = moedaDAL.loadTotal();
            comboMoeda.DataTextField = "DESCRICAO";
            comboMoeda.DataValueField = "COD_MOEDA";
            comboMoeda.DataBind();
            comboMoeda.Items.Insert(0, new ListItem("Selecione..", "0"));
        }
        
        totalRegistros = cotacaoDAO.totalCotacao();
        repeaterDados.DataSource = cotacaoDAO.load(Convert.ToInt32(comboMoeda.SelectedValue), paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
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

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        base.botaoDeletar_Click(sender, e);
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
                cotacaoDAO.delete(cod);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }

        montaGrid();
    }
}