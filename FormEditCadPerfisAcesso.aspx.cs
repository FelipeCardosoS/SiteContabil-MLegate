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

public partial class FormEditCadPerfisAcesso : BaseEditCadForm
{
    private PerfilAcesso perfil;
    DataTable tbModulos = new DataTable("tbModulos");

    public FormEditCadPerfisAcesso()
        : base("PERFIL_ACESSO")
    {
        perfil = new PerfilAcesso(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Perfil de Acesso";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Perfil de Acesso";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (_cadastro)
        {
            H_COD_PERFIL.Value = "0";
        }
        else
        {
            if (!Page.IsPostBack)
            {
                perfil.codigo = Request.QueryString["id"].ToString();
                perfil.load();
                H_COD_PERFIL.Value = Request.QueryString["id"].ToString();
                textCodigoPerfil.Enabled = false;
                textCodigoPerfil.Text = perfil.codigo;
                textDescricao.Text = perfil.descricao;
                comboModelo.SelectedValue = (perfil.exigeModelo ? "1" : "0" );

                for (int i = 0; i < perfil.arrModulos.Count; i++)
                {
                    for (int x = 0; x < treeView.Nodes.Count; x++)
                    {
                        verificaNo(perfil.arrModulos[i].codigo, treeView.Nodes[x]);
                    }
                }
            }
        }
    }

    private void verificaNo(string codigoModulo, TreeNode no)
    {
        if (no.Value == codigoModulo)
        {
            no.Checked = true;

        }
        else
        {
            for (int y = 0; y < no.ChildNodes.Count; y++)
            {
                verificaNo(codigoModulo, no.ChildNodes[y]);
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();
        
        addSubTitulo("Perfis de Acesso", "FormGridPerfisAcesso.aspx");

        if (_cadastro)
            subTitulo.Text = "Cadastro";
        else
            subTitulo.Text = "Edição";

        botaoSalvar.Text = "Continuar >";

        montaTree();
    }

    private void montaTree()
    {
        if (!Page.IsPostBack)
        {
            tbModulos.Clear();
            listaModulos(ref tbModulos);

            for (int i = 0; i < tbModulos.Rows.Count; i++)
            {
                if (tbModulos.Rows[i]["COD_MODULO_PAI"].ToString() == "")
                {
                    TreeNode no = new TreeNode();
                    no.Text = tbModulos.Rows[i]["DESCRICAO"].ToString();
                    no.Value = tbModulos.Rows[i]["COD_MODULO"].ToString();
                    no.NavigateUrl = "javascript:void(0);";
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
                filho.NavigateUrl = "javascript:void(0);";
                no.ChildNodes.Add(filho);
                existeFilhos(filho);
            }
        }
    }

    private void verificaCheckNo(TreeNode no, ref List<SModulo> marcados)
    {
        if (no.Checked)
        {
            marcados.Add(new SModulo(no.Value, "", no.Text, ""));
        }

        for (int y = 0; y < no.ChildNodes.Count; y++)
        {
            verificaCheckNo(no.ChildNodes[y], ref marcados);
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            perfil.codigo = textCodigoPerfil.Text;
            perfil.descricao = textDescricao.Text;

            List<SModulo> arrModulos = new List<SModulo>();
            for (int i = 0; i < treeView.Nodes.Count; i++)
            {
                verificaCheckNo(treeView.Nodes[i], ref arrModulos);
            }
            perfil.arrModulos = arrModulos;
            List<string> erros = perfil.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormEditCadPerfisAcesso2.aspx?id=" + perfil.codigo);
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            perfil.codigo = textCodigoPerfil.Text;
            perfil.descricao = textDescricao.Text;
            perfil.exigeModelo = (comboModelo.SelectedValue == "0" ? false : true);

            List<SModulo> arrModulos = new List<SModulo>();
            for (int i = 0; i < treeView.Nodes.Count; i++)
            {
                verificaCheckNo(treeView.Nodes[i], ref arrModulos);
            }
            perfil.arrModulos = arrModulos;
            List<string> erros = perfil.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormEditCadPerfisAcesso2.aspx?id=" + perfil.codigo);
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
