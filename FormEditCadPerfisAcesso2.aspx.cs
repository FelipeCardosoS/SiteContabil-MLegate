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

public partial class FormEditCadPerfisAcesso2 : BaseEditCadForm
{
    private PerfilAcesso perfil;
    DataTable tbModulos = new DataTable("tbModulos");
    DataTable tbTarefasDisponiveis = new DataTable("tbTarefasDisponiveis");
    DataTable tbTarefasSelecionadas = new DataTable("tbTarefasSelecionadas");

    public FormEditCadPerfisAcesso2()
        : base("PERFIL_ACESSO")
    {
        perfil = new PerfilAcesso(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        _codigoTarefa = "ALT";
        Title += "Delegação de Tarefas";
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        lisTarefasDisponiveis.DataSource = tbTarefasDisponiveis;
        lisTarefasDisponiveis.DataTextField = "DESCRICAO";
        lisTarefasDisponiveis.DataValueField = "COD_TAREFA";

        listTarefasSelecionadas.DataSource = tbTarefasSelecionadas;
        listTarefasSelecionadas.DataTextField = "DESCRICAO";
        listTarefasSelecionadas.DataValueField = "COD_TAREFA";
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Perfis de Acesso", "FormGridPerfisAcesso.aspx");

        subTitulo.Text = "Edição";
        botaoSalvar.Text = "Concluído";

        Button botaoVoltar = new Button();
        botaoVoltar.Text = "< Voltar";
        botaoVoltar.ID = "botaoVoltar";
        botaoVoltar.CssClass = "botaoVoltar";
        botaoVoltar.Click += botaoVoltar_Click;
        areaBotoes.Controls.Add(botaoVoltar);
        montaTree();
    }

    private void montaTree()
    {
        if (!Page.IsPostBack)
        {
            tbModulos.Clear();
            perfil.codigo = Request.QueryString["id"].ToString();
            perfil.listaModulosPerfil(ref tbModulos);

            for (int i = 0; i < tbModulos.Rows.Count; i++)
            {
                if (tbModulos.Rows[i]["COD_MODULO_PAI"].ToString() == "")
                {
                    TreeNode no = new TreeNode();
                    no.Text = tbModulos.Rows[i]["DESCRICAO"].ToString();
                    no.Value = tbModulos.Rows[i]["COD_MODULO"].ToString();

                    treeView.Nodes.Add(no);
                    existeFilhos(no);
                }
            }
        }
    }

    private void existeFilhos(TreeNode no)
    {
        for (int y = 0; y < tbModulos.Rows.Count; y++)
        {
            if (no.Value == tbModulos.Rows[y]["COD_MODULO_PAI"].ToString())
            {
                TreeNode filho = new TreeNode();
                filho.Text = tbModulos.Rows[y]["DESCRICAO"].ToString();
                filho.Value = tbModulos.Rows[y]["COD_MODULO"].ToString();
                no.ChildNodes.Add(filho);
                existeFilhos(filho);
            }
        }
    }

    protected void treeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView tree = (TreeView)sender;
        if (tree != null)
        {
            if (tree.SelectedNode != null)
            {
                TreeNode selecionado = tree.SelectedNode;
                tbTarefasDisponiveis.Clear();
                tbTarefasSelecionadas.Clear();
                perfil.codigo = Request.QueryString["id"].ToString();
                perfil.listaTarefasDisponiveis(ref tbTarefasDisponiveis, selecionado.Value);
                perfil.listaTarefasSelecionadas(ref tbTarefasSelecionadas, selecionado.Value);

                if (tbTarefasSelecionadas.Rows.Count == 0 && tbTarefasDisponiveis.Rows.Count == 0)
                {
                    painelTarefas.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "alerta", "alert('Nenhuma tarefa encontrada para o módulo selecionado.')", true);
                }
                else
                {
                    painelTarefas.Visible = true;
                    lisTarefasDisponiveis.DataBind();
                    listTarefasSelecionadas.DataBind();
                }
            }
        }
    }

    protected void botaoColoca_Click(object sender, EventArgs e)
    {
        if (lisTarefasDisponiveis.GetSelectedIndices().Length > 0)
        {
            perfil.codigo = Request.QueryString["id"].ToString();
            List<STarefa> arrTarefas = new List<STarefa>();
            for (int i = 0; i < lisTarefasDisponiveis.GetSelectedIndices().Length; i++)
            {
                arrTarefas.Add(new STarefa(Convert.ToInt32(lisTarefasDisponiveis.Items[lisTarefasDisponiveis.GetSelectedIndices()[i]].Value),
                    treeView.SelectedNode.Value, "", lisTarefasDisponiveis.Items[lisTarefasDisponiveis.GetSelectedIndices()[i]].Text));
            }

            perfil.arrTarefas = arrTarefas;
            perfil.insereTarefas();

            tbTarefasDisponiveis.Clear();
            tbTarefasSelecionadas.Clear();

            perfil.listaTarefasDisponiveis(ref tbTarefasDisponiveis, treeView.SelectedNode.Value);
            perfil.listaTarefasSelecionadas(ref tbTarefasSelecionadas, treeView.SelectedNode.Value);
            lisTarefasDisponiveis.DataBind();
            listTarefasSelecionadas.DataBind();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                "alerta", "alert('Nenhuma tarefa selecionada.')", true);
        }
    }

    protected void botaoRetira_Click(object sender, EventArgs e)
    {
        if (listTarefasSelecionadas.GetSelectedIndices().Length > 0)
        {
            perfil.codigo = Request.QueryString["id"].ToString();
            List<STarefa> arrTarefas = new List<STarefa>();
            for (int i = 0; i < listTarefasSelecionadas.GetSelectedIndices().Length; i++)
            {
                arrTarefas.Add(new STarefa(Convert.ToInt32(listTarefasSelecionadas.Items[listTarefasSelecionadas.GetSelectedIndices()[i]].Value),
                    treeView.SelectedNode.Value, "", listTarefasSelecionadas.Items[listTarefasSelecionadas.GetSelectedIndices()[i]].Text));
            }

            perfil.arrTarefas = arrTarefas;
            perfil.deletaTarefas();

            tbTarefasDisponiveis.Clear();
            tbTarefasSelecionadas.Clear();
            perfil.listaTarefasDisponiveis(ref tbTarefasDisponiveis, treeView.SelectedNode.Value);
            perfil.listaTarefasSelecionadas(ref tbTarefasSelecionadas, treeView.SelectedNode.Value);
            lisTarefasDisponiveis.DataBind();
            listTarefasSelecionadas.DataBind();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                "alerta", "alert('Nenhuma tarefa selecionada.')", true);
        }
    }

    protected void botaoVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormEditCadPerfisAcesso.aspx?id="+Request.QueryString["id"].ToString());
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormGridPerfisAcesso.aspx");
    }
}
