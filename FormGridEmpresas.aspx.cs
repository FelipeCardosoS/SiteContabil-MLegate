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

public partial class FormGridEmpresas : BaseGridForm
{
    private Empresa empresa;
    private DataTable tbEmpresas = new DataTable("tbEmpresas");

    private string fTipo;
    private string fFisicaJuridica;
    private string fNomeRazaoSocial;
    private string fNomeFantasia;
    private string fCnpjCpf;
    private string fGrupoFinanceiro;
    private int fGrupoEconomico;

    public FormGridEmpresas()
        : base("EMPRESA")
    {
        empresa = new Empresa(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        comboFisicaJuridica.Attributes.Add("onChange", "alteraFisicaJuridicaCombo(this.value,'" + textNomeRazaoSocial.ClientID + "','" + textCnpjCpf.ClientID + "');");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alteraFisicaJuridica",
            "alteraFisicaJuridicaCombo($(\"#" + comboFisicaJuridica.ClientID + "\").val(),'" + textNomeRazaoSocial.ClientID + "','" + textCnpjCpf.ClientID + "');", true);
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

        this.Title += "Empresas";
        subTitulo.Text = "Empresas";
        botaoNovo.NavigateUrl = "FormEditCadEmpresas.aspx";
        dsDados.Tables.Add(tbEmpresas);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbEmpresas"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        GrupoFinanceiro grupoFinanceiro = new GrupoFinanceiro(_conn);
        List<GrupoFinanceiro> listaGruposFinanceiros = grupoFinanceiro.listaDescricao();



        //listaGrupos.ForEach(o => comboGrupoEconomico.Items.Add(new ListItem { Text = o.descricao, Value = Convert.ToString(o.codigoGrupoEconomico)}));


        GrupoEconomico grupoEconomico = new GrupoEconomico(_conn);
        List<GrupoEconomico> listaGrupos = grupoEconomico.listaDescricao();



        //listaGrupos.ForEach(o => comboGrupoEconomico.Items.Add(new ListItem { Text = o.descricao, Value = Convert.ToString(o.codigoGrupoEconomico)}));


        if (!Page.IsPostBack)
        {
            comboOrdenar.Items.Add(new ListItem("Nome Fantasia", "NOME_FANTASIA"));
            comboOrdenar.Items.Add(new ListItem("Razão Social", "NOME_RAZAO_SOCIAL"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            comboGrupoFinanceiro.DataSource = listaGruposFinanceiros;
            comboGrupoFinanceiro.DataTextField = "nome";
            comboGrupoFinanceiro.DataValueField = "nome";
            comboGrupoFinanceiro.DataBind();
            comboGrupoFinanceiro.Items.Insert(0, new ListItem("Escolha", "0"));

            comboGrupoEconomico.DataSource = listaGrupos;
            comboGrupoEconomico.DataTextField = "descricao";
            comboGrupoEconomico.DataValueField = "codigoGrupoEconomico";
            comboGrupoEconomico.DataBind();
            comboGrupoEconomico.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();

        fGrupoFinanceiro = Convert.ToString(comboGrupoFinanceiro.SelectedValue).Equals("0") ? null : Convert.ToString(comboGrupoFinanceiro.SelectedValue);

        fGrupoEconomico = Convert.ToInt32(comboGrupoEconomico.SelectedValue);

        if (comboTipo.SelectedValue == "0")
            fTipo = null;
        else
            fTipo = comboTipo.SelectedValue;

        if (comboFisicaJuridica.SelectedValue == "0")
            fFisicaJuridica = null;
        else
            fFisicaJuridica = comboFisicaJuridica.SelectedValue;

        if (fFisicaJuridica != null)
        {
            if (fFisicaJuridica == "FISICA")
            {
                if (textNomeRazaoSocial.Text == "")
                    fNomeRazaoSocial = null;
                else
                    fNomeRazaoSocial = textNomeRazaoSocial.Text;

                if (textCnpjCpf.Text == "")
                    fCnpjCpf = null;
                else
                    fCnpjCpf = limpaString(textCnpjCpf.Text);

                fNomeFantasia = null;
            }
            else if (fFisicaJuridica == "JURIDICA")
            {
                if (textNomeRazaoSocial.Text == "")
                    fNomeRazaoSocial = null;
                else
                    fNomeRazaoSocial = textNomeRazaoSocial.Text;

                if (textCnpjCpf.Text == "")
                    fCnpjCpf = null;
                else
                    fCnpjCpf = limpaString(textCnpjCpf.Text);

                if (textNomeFantasia.Text == "")
                    fNomeFantasia = null;
                else
                    fNomeFantasia = textNomeFantasia.Text;

            }
            else
            {
                fCnpjCpf = null;
                fNomeFantasia = null;
                fNomeRazaoSocial = null;
            }
        }
        else
        {
            fCnpjCpf = null;
            fNomeFantasia = null;
            fNomeRazaoSocial = null;
        }

        totalRegistros = empresa.totalRegistros(fTipo, fFisicaJuridica, fNomeFantasia, fNomeRazaoSocial, fCnpjCpf, fGrupoFinanceiro, fGrupoEconomico);
        tbEmpresas.Clear();
        empresa.listaPaginada(ref tbEmpresas, fTipo, fFisicaJuridica, fNomeFantasia, fNomeRazaoSocial, fCnpjCpf, fGrupoFinanceiro, fGrupoEconomico, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void botaoFiltrar_Click(object sender, EventArgs e)
    {

        base.botaoFiltrar_Click(sender, e);
        
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HiddenField hdFisicaJuridica = (HiddenField)item.FindControl("hdFisicaJuridica");
            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkAlterar = (HyperLink)item.FindControl("linkAlterar");
            Literal lCnpjCpf = (Literal)item.FindControl("lCnpjCpf");
            Literal lTelefone = (Literal)item.FindControl("lTelefone");

            if (lCnpjCpf != null)
            {
                if (lCnpjCpf.Text.Length > 0)
                {
                    if (hdFisicaJuridica != null)
                    {
                        if (hdFisicaJuridica.Value == "FISICA")
                        {
                            string tmpCpf = lCnpjCpf.Text;
                            if (tmpCpf.Length == 11)
                                lCnpjCpf.Text = tmpCpf.Substring(0, 3) + "." + tmpCpf.Substring(3, 3) + "." + tmpCpf.Substring(6, 3) + "-" + tmpCpf.Substring(9, 2);
                        }
                        else if (hdFisicaJuridica.Value == "JURIDICA")
                        {
                            string tmpCnpj = lCnpjCpf.Text;
                            if (tmpCnpj.Length == 14)
                                lCnpjCpf.Text = tmpCnpj.Substring(0, 2) + "." + tmpCnpj.Substring(2, 3) + "." + tmpCnpj.Substring(5, 3) + "/" + tmpCnpj.Substring(8, 4) + "-" + tmpCnpj.Substring(12, 2);
                        }
                        else { }
                    }
                }
            }

            if (lTelefone != null)
            {
                if (lTelefone.Text.Length > 0)
                {
                    string tempTelefone = lTelefone.Text;
                    if (tempTelefone.Length == 11)
                    {
                        lTelefone.Text = "(" + tempTelefone.Substring(0, 2) + ") " + tempTelefone.Substring(2, 5) + "-" + tempTelefone.Substring(7, 4);
                    }
                    else if (tempTelefone.Length == 10)
                    {
                        lTelefone.Text = "(" + tempTelefone.Substring(0, 2) + ") " + tempTelefone.Substring(2, 4) + "-" + tempTelefone.Substring(6, 4);
                    }
                }
            }
            linkAlterar.NavigateUrl = "FormEditCadEmpresas.aspx?id=" + check.Value;
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
            empresa.codigo = Convert.ToInt32(selecionados[i]);
            try
            {
                empresa.deletar();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Não foi possivel excluir, pois o mesmo está sendo utilizado.');", true);
            }
        }
        montaGrid();
    }
}