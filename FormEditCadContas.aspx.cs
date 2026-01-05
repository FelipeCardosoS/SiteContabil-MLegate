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

public partial class FormEditCadContas : BaseEditCadForm
{
    ContaContabil conta;
    Job job;
    GrupoContabil grupoContabil;

    DataTable tbJobs = new DataTable("tbJobs");
    DataTable tbContaSintetica = new DataTable("tbContaSintetica");
    DataTable tbGrupoContabil = new DataTable("tbGrupoContabil");
    DataTable tbTiposConta = new DataTable("tbTiposConta");

    public bool checkBoxRetorno = false;

    public FormEditCadContas()
        :base("CONTA_CONTABIL")
    {
        conta = new ContaContabil(_conn);
        job = new Job(_conn);
        grupoContabil = new GrupoContabil(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Conta";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Conta";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        radioAnalitica.Items[0].Attributes.Add("onClick", "exibeJobDefault(this.value, this);");
        radioAnalitica.Items[1].Attributes.Add("onClick", "exibeJobDefault(this.value, this);");

        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            if(!Page.IsPostBack) checkBox.Checked = false;
        }
        else
        {
            botaoSalvar.Text = "Alterar";
            botaoSalvar.Enabled = (conta.PlanoConta(Convert.ToInt32(HttpContext.Current.Session["empresa"])) == 0 ? true : false);

            textCodigo.Enabled = false;

            if (!Page.IsPostBack)
            {
                conta.codigo = Request.QueryString["id"].ToString();
                conta.load();
                comboGrupoContabil.SelectedValue = conta.grupoContabil.ToString();
                textCodigo.Text = conta.codigo;
                textDescricao.Text = conta.descricao;
                radioDebCred.SelectedValue = conta.debitoCredito.ToString();
                comboContaSintetica.SelectedValue = conta.contaSintetica;
                radioAnalitica.SelectedValue = conta.analitica.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "selecionaRadioAnalitica", "exibeJobDefault(" + conta.analitica.ToString() + ",$(\"#" + radioAnalitica.ClientID + "_" + radioAnalitica.SelectedIndex + "\"))", true);
                comboTipo.SelectedValue = conta.tipo;
                comboTipoImposto.SelectedValue = conta.tipoImposto;
                checkGeraCredito.Checked = conta.geraCredito;
                checkRetido.Checked = conta.retido;
                checkRecibo.Checked = conta.recibo;
                comboClassificacao.SelectedValue = conta.codClassificacao;
                contaRefDropDown.SelectedValue = conta.codContaRef;
                comboMoedaBalanco.SelectedValue = conta.codMoedaBalanco.ToString();
                comboMoedaMovimento.SelectedValue = conta.codMoedaMovimento.ToString();
                checkBox.Checked = Convert.ToBoolean(conta.cta);
                ViewState["checkBoxRetorno"] = checkBox.Checked;
                checkBoxRetorno = checkBox.Checked;
                comboJob.SelectedValue = conta.job.ToString();
                Combo_Dre.SelectedValue = conta.codContaDRE;
                ComboBalanco.SelectedValue = conta.CodContaBalanco;

                if (conta.analitica == 1)
                {
                    comboJobDefault.SelectedValue = conta.jobDefault.ToString();
                }
                else
                {
                    comboJobDefault.SelectedValue = "0";
                }
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        addSubTitulo("Contas Contábeis", "FormGridContas.aspx");
        if (_cadastro)
            subTitulo.Text = "Cadastro";
        else
            subTitulo.Text = "Edição";

        if (!Page.IsPostBack)
        {
            tiposImpostoDAO tipoImpostoDAO = new tiposImpostoDAO(_conn);
            comboTipoImposto.DataSource = tipoImpostoDAO.lista(SessionView.EmpresaSession);
            comboTipoImposto.DataValueField = "tipoImposto";
            comboTipoImposto.DataTextField = "descricao";
            comboTipoImposto.DataBind();
            comboTipoImposto.Items.Insert(0, new ListItem("Nenhum", ""));

            tbContaSintetica.Clear();
            conta.listaSinteticas(ref tbContaSintetica);
            comboContaSintetica.DataSource = tbContaSintetica;
            comboContaSintetica.DataValueField = "COD_CONTA";
            comboContaSintetica.DataTextField = "DESCRICAO_COMPLETO";
            comboContaSintetica.DataBind();

            comboContaSintetica.Items.Insert(0, new ListItem("Escolha", ""));

            tbGrupoContabil.Clear();
            grupoContabil.lista(ref tbGrupoContabil);
            comboGrupoContabil.DataSource = tbGrupoContabil;
            comboGrupoContabil.DataValueField = "COD_GRUPO_CONTABIL";
            comboGrupoContabil.DataTextField = "DESCRICAO";
            comboGrupoContabil.DataBind();

            comboGrupoContabil.Items.Insert(0, new ListItem("Escolha", "0"));

            tbTiposConta.Clear();
            conta.listaTipos(ref tbTiposConta);
            comboTipo.DataSource = tbTiposConta;
            comboTipo.DataValueField = "COD_TIPO_CONTA";
            comboTipo.DataTextField = "DESCRICAO";
            comboTipo.DataBind();

            comboTipo.Items.Insert(0, new ListItem("Escolha", "0"));

            tbJobs.Clear();
            job.lista(ref tbJobs,'A');
            comboJobDefault.DataSource = tbJobs;
            comboJobDefault.DataTextField = "DESCRICAO_COMPLETO";
            comboJobDefault.DataValueField = "COD_JOB";
            comboJobDefault.DataBind();
            comboJobDefault.Items.Insert(0, new ListItem("Escolha", "0"));

            moedaBalancoDAO moedaBalancoDAO = new moedaBalancoDAO(_conn);
            comboMoedaBalanco.DataSource = moedaBalancoDAO.loadBalanco();
            comboMoedaBalanco.DataTextField = "DESCRICAO";
            comboMoedaBalanco.DataValueField = "COD_MOEDA_BALANCO";
            comboMoedaBalanco.DataBind();
            comboMoedaBalanco.Items.Insert(0, new ListItem("Escolha", "0"));

            moedaMovimentoDAO moedaMovimentoDAO = new moedaMovimentoDAO(_conn);
            comboMoedaMovimento.DataSource = moedaMovimentoDAO.loadMovimento();
            comboMoedaMovimento.DataTextField = "DESCRICAO";
            comboMoedaMovimento.DataValueField = "COD_MOEDA_MOVIMENTO";
            comboMoedaMovimento.DataBind();
            comboMoedaMovimento.Items.Insert(0, new ListItem("Escolha", "0"));

            comboJob.DataSource = tbJobs;
            comboJob.DataTextField = "DESCRICAO_COMPLETO";
            comboJob.DataValueField = "COD_JOB";
            comboJob.DataBind();
            comboJob.Items.Insert(0, new ListItem("Escolha","0"));

            classificacaoContaDAO classificacaoDAO = new classificacaoContaDAO(_conn);
            List<SClassificacaoConta> listClassificacao = classificacaoDAO.lista(SessionView.EmpresaSession);
            for (int i = 0; i < listClassificacao.Count; i++)
            {
                comboClassificacao.Items.Add(new ListItem(listClassificacao[i].codClassificacao + " - " + listClassificacao[i].descricao, listClassificacao[i].codClassificacao.ToString()));
            }
            comboClassificacao.Items.Insert(0, new ListItem("Escolha", "0"));

            contasRefDAO contaRefDAO = new contasRefDAO(_conn);
            List<SContaRef> listContasRef = contaRefDAO.list();
            foreach (SContaRef contaRef in listContasRef)
            {
                ListItem item = new ListItem(contaRef.codigoRef + " - " + contaRef.descricao, contaRef.codigoRef);
                if (contaRef.analiticaSintetica == "S")
                {
                    item.Attributes.Add("disabled", "disabled");
                }
                contaRefDropDown.Items.Add(item);
            }
            contaRefDropDown.Items.Insert(0, new ListItem("Selecione", ""));

            contaRefDAO = new contasRefDAO(_conn);
            listContasRef = contaRefDAO.list_ECF();
            foreach (SContaRef contaRef in listContasRef)
            {
                ListItem item = new ListItem(contaRef.codigoRef + " - " + contaRef.descricao, contaRef.codigoRef);
                if (contaRef.analiticaSintetica == "S")
                {
                    item.Attributes.Add("disabled", "disabled");
                }
                contaRefDropDownEcf.Items.Add(item);
            }
            contaRefDropDownEcf.Items.Insert(0, new ListItem("Selecione", ""));

            ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
            List<EstruturaDRE> listContasDre = _ContasDREDAO.list(SessionView.EmpresaSession);
            foreach (EstruturaDRE contaDRE in listContasDre)
            {
                ListItem item = new ListItem(contaDRE.Cod_DRE + " - " + contaDRE.Descricao, contaDRE.Cod_DRE);
                if (contaDRE.Analitica == "N")
                {
                    item.Attributes.Add("disabled", "disabled");
                }
                Combo_Dre.Items.Add(item);
            }
            Combo_Dre.Items.Insert(0, new ListItem("Selecione", ""));

            ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
            List<EstruturaBalanco> listContasBalanco = _ContasBalancoDAO.list(SessionView.EmpresaSession);
            foreach (EstruturaBalanco contaBalanco in listContasBalanco)
            {
                ListItem item = new ListItem(contaBalanco.Cod_Balanco + " - " + contaBalanco.Descricao, contaBalanco.Cod_Balanco);
                if (contaBalanco.Analitica == "N")
                {
                    item.Attributes.Add("disabled", "disabled");
                }
                ComboBalanco.Items.Add(item);
            }
            ComboBalanco.Items.Insert(0, new ListItem("Selecione", ""));
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            conta.grupoContabil = Convert.ToInt32(comboGrupoContabil.SelectedValue);
            conta.codigo = textCodigo.Text;
            conta.descricao = textDescricao.Text;
            conta.debitoCredito = Convert.ToChar(radioDebCred.SelectedValue);
            conta.contaSintetica = comboContaSintetica.SelectedValue;
            conta.analitica = Convert.ToInt32(radioAnalitica.SelectedValue);
            conta.tipo = comboTipo.SelectedValue;
            conta.tipoImposto = comboTipoImposto.SelectedValue;
            conta.geraCredito  = checkGeraCredito.Checked;
            conta.retido = checkRetido.Checked;
            conta.recibo = checkRecibo.Checked;
            conta.codClassificacao = comboClassificacao.SelectedValue;
            conta.codContaRef = contaRefDropDown.SelectedValue;
            conta.codContaRefEcf = contaRefDropDownEcf.SelectedValue;
            conta.codMoedaBalanco = Convert.ToInt32(comboMoedaBalanco.SelectedValue);
            conta.codMoedaMovimento = Convert.ToInt32(comboMoedaMovimento.SelectedValue);
            conta.cta = Convert.ToInt32(checkBox.Checked);
            conta.ctaValida = (ViewState["checkBoxRetorno"] == null ? false : (bool)ViewState["checkBoxRetorno"]); // Passo o valor do banco, se for true não preciso dar UPDATE na base, para zerar os CTAs.
            conta.job = Convert.ToInt32(comboJob.SelectedValue);
            conta.CodContaDRE = Combo_Dre.SelectedValue;
            conta.CodContaBalanco = ComboBalanco.SelectedValue;

            if (Convert.ToInt32(radioAnalitica.SelectedValue) == 0)
            {
                conta.jobDefault = 0;
            }
            else
            {
                conta.jobDefault = Convert.ToInt32(comboJobDefault.SelectedValue);
            }

            List<string> erros = conta.novo();

            if (erros.Count == 0)
            {
                Response.Redirect("FormGridContas.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            conta.grupoContabil = Convert.ToInt32(comboGrupoContabil.SelectedValue);
            conta.codigo = textCodigo.Text;
            conta.descricao = textDescricao.Text;
            conta.debitoCredito = Convert.ToChar(radioDebCred.SelectedValue);
            conta.contaSintetica = comboContaSintetica.SelectedValue;
            conta.analitica = Convert.ToInt32(radioAnalitica.SelectedValue);
            conta.tipo = comboTipo.SelectedValue;
            conta.tipoImposto = comboTipoImposto.SelectedValue;
            conta.geraCredito = checkGeraCredito.Checked;
            conta.retido = checkRetido.Checked;
            conta.recibo = checkRecibo.Checked;
            conta.codClassificacao = comboClassificacao.SelectedValue;
            conta.codContaRef = contaRefDropDown.SelectedValue;
            conta.codMoedaBalanco = Convert.ToInt32(comboMoedaBalanco.SelectedValue);
            conta.codMoedaMovimento = Convert.ToInt32(comboMoedaMovimento.SelectedValue);
            conta.cta = Convert.ToInt32(checkBox.Checked);
            conta.ctaValida = (bool)ViewState["checkBoxRetorno"]; // Passo o valor do banco, se for true não preciso dar UPDATE na base, para zerar os CTAs.
            conta.job = Convert.ToInt32(comboJob.SelectedValue);
            conta.CodContaDRE = Combo_Dre.SelectedValue;
            conta.CodContaBalanco = ComboBalanco.SelectedValue;

            if (Convert.ToInt32(radioAnalitica.SelectedValue) == 0)
            {
                conta.jobDefault = 0;
            }
            else
            {
                conta.jobDefault = Convert.ToInt32(comboJobDefault.SelectedValue);
            }

            List<string> erros = conta.alterar();

            if (erros.Count == 0)
            {
                Response.Redirect("FormGridContas.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }
}
