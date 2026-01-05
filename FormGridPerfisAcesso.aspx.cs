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

public partial class FormGridPerfisAcesso : BaseGridForm
{
    private PerfilAcesso perfil;
    private DataTable tbPerfis = new DataTable("tbPerfis");

    private string fDescricao;

    public FormGridPerfisAcesso()
        : base("PERFIL_ACESSO")
    {
        perfil = new PerfilAcesso(_conn);
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

        this.Title += "Perfis de Acesso";
        subTitulo.Text = "Perfis de Acesso";
        botaoNovo.NavigateUrl = "FormEditCadPerfisAcesso.aspx";

        dsDados.Tables.Add(tbPerfis);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbPerfis"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
            comboOrdenar.Items.Add(new ListItem("Descrição", "DESCRICAO"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textDescricao.Text == "")
            fDescricao = null;
        else
            fDescricao = textDescricao.Text;

        totalRegistros = perfil.totalRegistros(fDescricao);
        tbPerfis.Clear();
        perfil.listaPaginada(ref tbPerfis, fDescricao, paginaAtual, ordenacao);
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

            linkAlterar.NavigateUrl = "FormEditCadPerfisAcesso.aspx?id=" + check.Value;
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
            perfil.codigo = selecionados[i];
            perfil.deletar();
        }

        montaGrid();
    }
}
