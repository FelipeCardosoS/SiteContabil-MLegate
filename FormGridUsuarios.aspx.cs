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

public partial class FormGridUsuarios : BaseGridForm
{
    private Usuario usuario;
    private PerfilAcesso perfil;
    private DataTable tbUsuarios = new DataTable("tbUsuarios");
    private DataTable tbPerfis = new DataTable("tbPerfis");

    private string fNome;
    private string fPerfil;

    public FormGridUsuarios()
        : base("USUARIO")
    {
        usuario = new Usuario(_conn);
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
        bool aceitaAlterarSenha = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "CAD")
                aceitaCadastrar = true;

            if (_tarefas[i].tarefa == "ALT")
                aceitaAlterar = true;

            if (_tarefas[i].tarefa == "ALT_SENHA")
                aceitaAlterarSenha = true;

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

        if (!aceitaAlterarSenha)
        {
            foreach (RepeaterItem item in repeaterDados.Items)
            {
                if (item.ItemType != ListItemType.Separator)
                {
                    HyperLink linkAlterarSenha = (HyperLink)item.FindControl("linkAlterarSenha");
                    linkAlterarSenha.Enabled = false;
                }
            }
        }
            
    }

    protected override void montaTela()
    {
        base.montaTela();

        this.Title += "Usuários";
        subTitulo.Text = "Usuários";
        botaoNovo.NavigateUrl = "FormEditCadUsuarios.aspx";
        dsDados.Tables.Add(tbUsuarios);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbUsuarios"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Insert(0,new ListItem("Escolha", "0"));
            comboOrdenar.Items.Add(new ListItem("Nome", "NOME_COMPLETO"));
            comboOrdenar.Items.Add(new ListItem("Perfil de Acesso", "COD_PERFIL"));
        }

        if (!Page.IsPostBack)
        {
            perfil.lista(ref tbPerfis);
            comboPerfil.DataSource = tbPerfis;
            comboPerfil.DataValueField = "COD_PERFIL";
            comboPerfil.DataTextField = "DESCRICAO";
            comboPerfil.DataBind();
            comboPerfil.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        if (textNome.Text == "")
            fNome = null;
        else
            fNome = textNome.Text;

        if (comboPerfil.SelectedValue == "0")
            fPerfil = null;
        else
            fPerfil = comboPerfil.SelectedValue;

        totalRegistros = usuario.totalRegistros(fNome, fPerfil);
        tbUsuarios.Clear();
        usuario.listaPaginada(ref tbUsuarios, fNome,fPerfil, paginaAtual, ordenacao);
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
            HyperLink linkAlterarSenha = (HyperLink)item.FindControl("linkAlterarSenha");

            linkAlterar.NavigateUrl = "FormEditCadUsuarios.aspx?id="+check.Value;
            linkAlterarSenha.NavigateUrl = "FormEditSenhaUsuarios.aspx?id=" + check.Value;
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
            usuario.id = Convert.ToInt32(selecionados[i]);
            usuario.deletar();
        }

        montaGrid();
    }
}
