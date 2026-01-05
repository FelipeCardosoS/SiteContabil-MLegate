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

public partial class FormGridBaixas : BaseGridForm
{
    FolhaLancamento folha;
    private DataTable tbBaixas = new DataTable("tbBaixas");
    Nullable<DateTime> fDtInicio;
    Nullable<DateTime> fDtTermino;

    public FormGridBaixas()
        : base("GRID_BAIXA_TITULO")
    {
        folha = new FolhaLancamento(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        string strMascaras = "$(\"#" + textDataInicio.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textDataTermino.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);
    }

    protected override void verificaTarefas()
    {
        bool aceitaDeletar = false;
        bool aceitaCadastrar = false;

        for (int i = 0; i < _modulos.Count; i++)
        {
            if (_modulos[i].codigo == "BAIXA_TITULO")
                aceitaCadastrar = true;
        }
        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "DEL")
                aceitaDeletar = true;
        }

        if (!aceitaCadastrar)
            botaoNovo.Enabled = false;
        if (!aceitaDeletar)
            botaoDeletar.Enabled = false;
    }

    protected override void montaTela()
    {
        base.montaTela();

        this.Title += "Baixas";
        subTitulo.Text = "Baixas";
        botaoNovo.NavigateUrl = "FormBaixaTitulos.aspx";
        dsDados.Tables.Add(tbBaixas);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbBaixas"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        botaoNovo.Text = "Baixar";
        if (!Page.IsPostBack)
        {
            Empresa empresa = new Empresa(_conn);
            DataTable tbFornecedores = new DataTable("tbFornecedores");

            empresa.listaFornecedoresClientes(ref tbFornecedores);
            comboTerceiro.DataSource = tbFornecedores;
            comboTerceiro.DataTextField = "NOME_RAZAO_SOCIAL";
            comboTerceiro.DataValueField = "COD_EMPRESA";
            comboTerceiro.DataBind();
            comboTerceiro.Items.Insert(0, new ListItem("Escolha", "0"));

            comboOrdenar.Items.Add(new ListItem("Baixa", "SEQ_BAIXA"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void montaGrid()
    {
        base.montaGrid();
        double? baixa = null;
        int? terceiro = null;

        if (textBaixa.Text != "")
            baixa = Convert.ToDouble(textBaixa.Text);

        if (comboTerceiro.SelectedValue != "0")
            terceiro = Convert.ToInt32(comboTerceiro.SelectedValue);

        if (textDataInicio.Text == "")
            fDtInicio = null;
        else
            fDtInicio = Convert.ToDateTime(textDataInicio.Text);

        if (textDataTermino.Text == "")
            fDtTermino = null;
        else
            fDtTermino = Convert.ToDateTime(textDataTermino.Text);

        totalRegistros = folha.totalRegistrosBaixa(baixa,fDtInicio, fDtTermino, terceiro);
        tbBaixas.Clear();
        folha.listaBaixasPaginada(ref tbBaixas, baixa,fDtInicio, fDtTermino,terceiro, paginaAtual, ordenacao);
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            HyperLink linkDetalhe = (HyperLink)item.FindControl("linkDetalhe");

            linkDetalhe.NavigateUrl = "FormDetalheBaixa.aspx?id=" + check.Value;
        }
    }

    protected override void botaoFiltrar_Click(object sender, EventArgs e)
    {

        base.botaoFiltrar_Click(sender, e);
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        List<double> selecionados = new List<double>();
        foreach (RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType != ListItemType.Separator)
            {
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                if (check.Checked)
                {
                    selecionados.Add(Convert.ToDouble(check.Value));
                }
            }
        }


        List<string> erros = new List<string>();
        FolhaLancamento folha = new FolhaLancamento(_conn);

        if (folha.verificaPeriodoFechamentoBaixas(selecionados))
        {
            for (int i = 0; i < selecionados.Count; i++)
            {
                List<string> err = folha.deletaBaixa(selecionados[i]);

                for (int x = 0; x < err.Count; x++)
                {
                    erros.Add(err[x]);
                }
            }
        }
        else
        {
            erros.Add("Foram escolhidas baixas de um período encerrado, não será possível apagar as baixas selecionadas.");
        }

        if(erros.Count == 0){
            montaGrid();
        }else{
            errosFormulario(erros);
        }
    }
}
