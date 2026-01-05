using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class FormRelatorioGerar : BaseForm
{
    Divisao divisao;
    LinhaNegocio linhaNegocio;
    Cliente cliente;
    Job job;
    ContaContabil conta;
    string empresas = "";
    public string consolidaEmpresa;
    moedaDAL moeda;
    public int tipoRelatorio;

    DataTable tbDivisao = new DataTable("tbDivisao");
    DataTable tbLinhaNegocio = new DataTable("tbLinhaNegocio");
    DataTable tbCliente = new DataTable("tbCliente");
    DataTable tbJob = new DataTable("tbJob");
    DataTable tbConta = new DataTable("tbConta");

    #region Declaração de Propriedades
    public HiddenField hdRelatorio
    {
        get { return hiddenRelatorio; }
    }

    public TextBox txtPeriodo
    {
        get 
        {
            if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                return textPeriodoTitPend;
            }
            else
            {
                return textPeriodo;
            }
        }
    }

    public CheckBox checkContasEncerr
    {
        get { return checkContasEncerramento; }
    }
    public CheckBox checkContasEncerrDRE
    {
        get { return checkContasEncerramentoDRE; }
    }

	public CheckBox checkContasZeradas
	{
		get
		{
			if (hiddenRelatorio.Value == "BALANCETE")
				return checkBoxContasZeradas;
			else
				return null;
		}
			
	}
    public TextBox textAteEncerramento 
    {
        get { return txtAteEncerramento; }
    }

    public string empresas_selecionadas
    {
        get
        {
            empresas = "";
            foreach (RepeaterItem item in gvempresas.Items)
            {
                decimal COD_EMPRESA = 0;
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("chk");
                if (check.Checked)
                {
                    decimal.TryParse(check.Value, out COD_EMPRESA);
                    empresas += ", " + Convert.ToString(COD_EMPRESA);
                }
            }
            if (string.IsNullOrEmpty(empresas))
            {
                empresas = Convert.ToString(SessionView.EmpresaSession);
            }
            else
            {
                empresas = empresas.Substring(2);
            }
            return empresas;
        }
    }

    public string empresas_selecionadas2
    {
        get
        {
            empresas = "";
            foreach (RepeaterItem item in Repeater1.Items)
            {
                decimal COD_EMPRESA = 0;
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("chk");
                if (check.Checked)
                {
                    decimal.TryParse(check.Value, out COD_EMPRESA);
                    empresas += ", " + Convert.ToString(COD_EMPRESA);
                }
            }
            if (string.IsNullOrEmpty(empresas))
            {
                empresas = Convert.ToString(SessionView.EmpresaSession);
            }
            else
            {
                empresas = empresas.Substring(2);
            }
            return empresas;
        }
    }


    public string empresas_selecionadas3
    {
        get
        {
            empresas = "";
            foreach (RepeaterItem item in Repeater2.Items)
            {
                decimal COD_EMPRESA = 0;
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("chk");
                if (check.Checked)
                {
                    decimal.TryParse(check.Value, out COD_EMPRESA);
                    empresas += ", " + Convert.ToString(COD_EMPRESA);
                }
            }
            if (string.IsNullOrEmpty(empresas))
            {
                empresas = Convert.ToString(SessionView.EmpresaSession);
            }
            else
            {
                empresas = empresas.Substring(2);
            }
            return empresas;
        }
    }
   

    public TextBox txtDiasVencimento
    {
        get
        {
            if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                return textDiasVencimento;
            }
            else
            {
                return null;
            }
        }
    }

    public DropDownList cmbMoeda
    {
        get
        {
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                return comboMoedaBalancete;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                return comboMoedaDRE;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                return comboMoedaRazao;
            }
            else
            {
                return null;
            }
        }
    }

    public DropDownList cmbOrdenacao
    {
        get
        {
            if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                return comboOrdenacaoTitPend;
            }
            else
            {
                return null;
            }
        }
    }

    public DropDownList cmbCPCR
    {
        get
        {
            if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                return comboCPCR;
            }
            else
            {
                return null;
            }
        }
    }

    public TextBox txtPeriodoDe
    {
        get
        {
            TextBox retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = textPeriodoDeDRE;
            }
            else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA" || hiddenRelatorio.Value == "MOV_BANCARIA")
            {
                retorno = textPeriodoDeMov;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = textPeriodoDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public TextBox txtPeriodoAte
    {
        get
        {
            TextBox retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = textPeriodoAteDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA" || hiddenRelatorio.Value == "MOV_BANCARIA")
			{
                retorno = textPeriodoAteMov;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = textPeriodoAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDivisaoDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDivisaoDe;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDivisaoDeDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDivisaoDeMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboDivisaoDeTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboDivisaoDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDivisaoAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDivisaoAte;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDivisaoAteDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDivisaoAteMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboDivisaoAteTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboDivisaoAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbLinhaNegocioDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboLinhaNegocioDe;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboLinhaNegocioDeDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboLinhaNegocioDeMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboLinhaNegocioDeTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboLinhaNegocioDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbLinhaNegocioAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboLinhaNegocioAte;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboLinhaNegocioAteDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboLinhaNegocioAteMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboLinhaNegocioAteTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboLinhaNegocioAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbClienteDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboClienteDe;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboClienteDeDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboClienteDeMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboClienteDeTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboClienteDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbClienteAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboClienteAte;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboClienteAteDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboClienteAteMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboClienteAteTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboClienteAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbJobDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboJobDe;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboJobDeDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboJobDeMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboJobDeTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboJobDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbJobAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboJobAte;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboJobAteDRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboJobAteMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboJobAteTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboJobAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbTerDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboTerDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbTerAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboTerAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDetalhamento1
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDetalhamento1;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDetalhamento1DRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDetalhamento1Mov;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDetalhamento2
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDetalhamento2;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDetalhamento2DRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDetalhamento2Mov;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDetalhamento3
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDetalhamento3;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDetalhamento3DRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDetalhamento3Mov;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbDetalhamento4
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDetalhamento4;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDetalhamento4DRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = comboDetalhamento4Mov;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }



    public DropDownList cmbDetalhamento5
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = comboDetalhamento5;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = comboDetalhamento5DRE;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = null;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbContaDe
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = null;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA" || hiddenRelatorio.Value == "MOV_BANCARIA")
			{
                retorno = comboContaDeMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboContaDeTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboContaDeRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbContaAte
    {
        get
        {
            DropDownList retorno;
            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = null;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA" || hiddenRelatorio.Value == "MOV_BANCARIA")
			{
                retorno = comboContaAteMov;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = comboContaAteTitPend;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = comboContaAteRAZAO;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public CheckBox checkBoxDuasMoedas
    {
        get
        {
            CheckBox retorno;

            if (hiddenRelatorio.Value == "BALANCETE")
            {
                retorno = checkBoxBalancete;
            }
            else if (hiddenRelatorio.Value == "DRE")
            {
                retorno = null;
            }
			else if (hiddenRelatorio.Value == "MOV_FINANC" || hiddenRelatorio.Value == "MOV_FINANC_BETA")
			{
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "TITULO_PENDENTE")
            {
                retorno = null;
            }
            else if (hiddenRelatorio.Value == "RAZAO")
            {
                retorno = checkBoxRazao;
            }
            else
            {
                retorno = null;
            }
            return retorno;
        }
    }

    public DropDownList cmbGrupo {
		get
		{
            DropDownList retorno;

            if (hiddenRelatorio.Value == "MOV_BANCARIA")
            {
                retorno = ddlAgrupar;
            }
            else
                retorno = null;

            return retorno;
		}
    }

    #endregion

    public FormRelatorioGerar()
        : base("RELATORIO")
    {
        divisao = new Divisao(_conn);
        linhaNegocio = new LinhaNegocio(_conn);
        cliente = new Cliente(_conn);
        job = new Job(_conn);
        conta = new ContaContabil(_conn);
        moeda = new moedaDAL(_conn);
    }

    protected void selecionaAte(object sender, EventArgs e)
    {
        if (Request["__EVENTARGUMENT"] != null)
        {
            DropDownList comboIrmao = (DropDownList)FindControlRecursive(Page, Request["__EVENTARGUMENT"].ToString());
            DropDownList combo = (DropDownList)sender;
            if (comboIrmao != null)
            {
                //comboIrmao.SelectedValue = combo.SelectedValue;
                comboIrmao.Enabled = true;
            }
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            consolidaEmpresa = ConfigurationManager.AppSettings["consolidaEmpresa"];
            montaGrid();
        }

        base.Page_Load(sender, e);
        addSubTitulo("Relatórios", "FormRelatorios.aspx");
        //Form.Target = "_blank";
        if (Request.QueryString["rel"] != null)
        {
            string strMascaras = "";
            hiddenRelatorio.Value = Request.QueryString["rel"].ToString();
			checkBoxContasZeradas.Visible = false;
			switch (Request.QueryString["rel"].ToString())
            {
                #region Gerando Balancete
                case "BALANCETE":

                    strMascaras = "$(\"#" + textPeriodo.ClientID + "\").mask(\"99/9999\");$(\"#" + txtAteEncerramento.ClientID + "\").mask(\"99/9999\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "mascaras",strMascaras, true);
                    subTitulo.Text = "Balancete";
                    Title += "Relatórios - Balancete";
                    textPeriodo.Text = DateTime.Now.ToString("MMyyyy");

                    comboMoedaBalancete.DataSource = moeda.loadTotal();
                    comboMoedaBalancete.DataTextField = "DESCRICAO";
                    comboMoedaBalancete.DataValueField = "COD_MOEDA";
                    comboMoedaBalancete.DataBind();
                    comboMoedaBalancete.Items.Insert(0, new ListItem("Padrão", "0"));

                    comboDivisaoDe.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDe.UniqueID + "', '" + comboDivisaoAte.ID + "');");
                    comboLinhaNegocioDe.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDe.UniqueID + "','" + comboLinhaNegocioAte.ID + "');");
                    comboClienteDe.Attributes.Add("onChange", "selecionaAte('" + comboClienteDe.UniqueID + "','" + comboClienteAte.ID + "');");
                    comboJobDe.Attributes.Add("onChange", "selecionaAte('" + comboJobDe.UniqueID + "','"+comboJobAte.ID+"');");

                    comboDetalhamento1.Attributes.Add("onChange", "ativaCombosFilhos(this,1,'');");
                    comboDetalhamento2.Attributes.Add("onChange", "ativaCombosFilhos(this,2,'');");
                    comboDetalhamento3.Attributes.Add("onChange", "ativaCombosFilhos(this,3,'');");
                    comboDetalhamento4.Attributes.Add("onChange", "ativaCombosFilhos(this,4,'');");
                    comboDetalhamento5.Attributes.Add("onChange", "ativaCombosFilhos(this,5,'');");

                    if (!Page.IsPostBack)
                    {
                        divisao.lista(ref tbDivisao);
                        comboDivisaoDe.DataSource = tbDivisao;
                        comboDivisaoDe.DataValueField = "COD_DIVISAO";
                        comboDivisaoDe.DataTextField = "DESCRICAO";
                        comboDivisaoDe.DataBind();

                        comboDivisaoAte.DataSource = tbDivisao;
                        comboDivisaoAte.DataValueField = "COD_DIVISAO";
                        comboDivisaoAte.DataTextField = "DESCRICAO";
                        comboDivisaoAte.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDe.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDe.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDe.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDe.DataBind();

                        comboLinhaNegocioAte.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAte.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAte.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAte.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDe.DataSource = tbCliente;
                        comboClienteDe.DataValueField = "COD_EMPRESA";
                        comboClienteDe.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDe.DataBind();

                        comboClienteAte.DataSource = tbCliente;
                        comboClienteAte.DataValueField = "COD_EMPRESA";
                        comboClienteAte.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAte.DataBind();

						//job.lista(ref tbJob, 'A');
						job.listaFiltroRelatorio(ref tbJob);
						comboJobDe.DataSource = tbJob;
                        comboJobDe.DataValueField = "COD_JOB";
                        comboJobDe.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDe.DataBind();

                        comboJobAte.DataSource = tbJob;
                        comboJobAte.DataValueField = "COD_JOB";
                        comboJobAte.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAte.DataBind();

                        comboDivisaoDe.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAte.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDe.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAte.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDe.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAte.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDe.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAte.Items.Insert(0, new ListItem("Escolha", "0"));
                    }
                    txtAteEncerramento.Text = "12/" + (DateTime.Now.Year - 1).ToString();
                    painelBalancete.Visible = true;
                    painelDRE.Visible = false;
                    painelMovFinanc.Visible = false;
                    painelTitPend.Visible = false;
                    painelRazao.Visible = false;
					checkBoxContasZeradas.Visible = true;
                    break;
                #endregion
                #region Gerando DRE
                case "DRE":
                    painelBalancete.Visible = false;
                    painelDRE.Visible = true;
                    painelMovFinanc.Visible = false;
                    painelTitPend.Visible = false;
                    painelRazao.Visible = false;
                    subTitulo.Text = "DRE";
                    Title += "Relatórios - DRE";

                    strMascaras = "$(\"#" + textPeriodoDeDRE.ClientID + "\").mask(\"99/9999\");";
                    strMascaras += "$(\"#" + textPeriodoAteDRE.ClientID + "\").mask(\"99/9999\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
                        strMascaras, true);

                    textPeriodoDeDRE.Text = DateTime.Now.AddMonths(-1).ToString("MMyyyy");
                    textPeriodoAteDRE.Text = DateTime.Now.ToString("MMyyyy");

                    comboMoedaDRE.DataSource = moeda.loadTotal();
                    comboMoedaDRE.DataTextField = "DESCRICAO";
                    comboMoedaDRE.DataValueField = "COD_MOEDA";
                    comboMoedaDRE.DataBind();
                    comboMoedaDRE.Items.Insert(0, new ListItem("Padrão", "0"));

                    comboDivisaoDeDRE.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDeDRE.UniqueID + "', '" + comboDivisaoAteDRE.ID + "');");
                    comboLinhaNegocioDeDRE.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDeDRE.UniqueID + "','" + comboLinhaNegocioAteDRE.ID + "');");
                    comboClienteDeDRE.Attributes.Add("onChange", "selecionaAte('" + comboClienteDeDRE.UniqueID + "','" + comboClienteAteDRE.ID + "');");
                    comboJobDeDRE.Attributes.Add("onChange", "selecionaAte('" + comboJobDeDRE.UniqueID + "','" + comboJobAteDRE.ID + "');");

                    comboDetalhamento1DRE.Attributes.Add("onChange", "ativaCombosFilhos(this,1,'DRE');");
                    comboDetalhamento2DRE.Attributes.Add("onChange", "ativaCombosFilhos(this,2,'DRE');");
                    comboDetalhamento3DRE.Attributes.Add("onChange", "ativaCombosFilhos(this,3,'DRE');");
                    comboDetalhamento4DRE.Attributes.Add("onChange", "ativaCombosFilhos(this,4,'DRE');");
                    comboDetalhamento5DRE.Attributes.Add("onChange", "ativaCombosFilhos(this,5,'DRE');");

                    if (!Page.IsPostBack)
                    {
                        divisao.lista(ref tbDivisao);
                        comboDivisaoDeDRE.DataSource = tbDivisao;
                        comboDivisaoDeDRE.DataValueField = "COD_DIVISAO";
                        comboDivisaoDeDRE.DataTextField = "DESCRICAO";
                        comboDivisaoDeDRE.DataBind();

                        comboDivisaoAteDRE.DataSource = tbDivisao;
                        comboDivisaoAteDRE.DataValueField = "COD_DIVISAO";
                        comboDivisaoAteDRE.DataTextField = "DESCRICAO";
                        comboDivisaoAteDRE.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDeDRE.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDeDRE.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDeDRE.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDeDRE.DataBind();

                        comboLinhaNegocioAteDRE.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAteDRE.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAteDRE.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAteDRE.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDeDRE.DataSource = tbCliente;
                        comboClienteDeDRE.DataValueField = "COD_EMPRESA";
                        comboClienteDeDRE.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDeDRE.DataBind();

                        comboClienteAteDRE.DataSource = tbCliente;
                        comboClienteAteDRE.DataValueField = "COD_EMPRESA";
                        comboClienteAteDRE.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAteDRE.DataBind();

                        //job.lista(ref tbJob, 'A');
                        job.listaFiltroRelatorio(ref tbJob);
                        comboJobDeDRE.DataSource = tbJob;
                        comboJobDeDRE.DataValueField = "COD_JOB";
                        comboJobDeDRE.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDeDRE.DataBind();

                        comboJobAteDRE.DataSource = tbJob;
                        comboJobAteDRE.DataValueField = "COD_JOB";
                        comboJobAteDRE.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAteDRE.DataBind();

                        comboDivisaoDeDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAteDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDeDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAteDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDeDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAteDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDeDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAteDRE.Items.Insert(0, new ListItem("Escolha", "0"));
                    }

                    break;
                #endregion
                #region Gerando Movimentação Financeira
                case "MOV_FINANC":
				case "MOV_FINANC_BETA":
                    tipoRelatorio = 1;

					strMascaras = "$(\"#" + textPeriodoDeMov.ClientID + "\").mask(\"99/99/9999\");";
                    strMascaras += "$(\"#" + textPeriodoAteMov.ClientID + "\").mask(\"99/99/9999\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
                        strMascaras, true);
                    subTitulo.Text = "Movimentação Financeira";
                    Title += "Relatórios - Movimentação Financeira";
                    textPeriodoDeMov.Text = DateTime.Now.AddMonths(-1).ToString("ddMMyyyy");
                    textPeriodoAteMov.Text = DateTime.Now.ToString("ddMMyyyy");

                    comboDivisaoDeMov.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDeMov.UniqueID + "', '" + comboDivisaoAteMov.ID + "');");
                    comboLinhaNegocioDeMov.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDeMov.UniqueID + "','" + comboLinhaNegocioAteMov.ID + "');");
                    comboClienteDeMov.Attributes.Add("onChange", "selecionaAte('" + comboClienteDeMov.UniqueID + "','" + comboClienteAteMov.ID + "');");
                    comboJobDeMov.Attributes.Add("onChange", "selecionaAte('" + comboJobDeMov.UniqueID + "','" + comboJobAteMov.ID + "');");

                    comboDetalhamento1Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,1,'Mov');");
                    comboDetalhamento2Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,2,'Mov');");
                    comboDetalhamento3Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,3,'Mov');");
                    comboDetalhamento4Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,4,'Mov');");

                    if (!Page.IsPostBack)
                    {
                        conta.listaAnaliticas(ref tbConta);
                        comboContaDeMov.DataSource = tbConta;
                        comboContaDeMov.DataValueField = "COD_CONTA";
                        comboContaDeMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaDeMov.DataBind();

                        comboContaAteMov.DataSource = tbConta;
                        comboContaAteMov.DataValueField = "COD_CONTA";
                        comboContaAteMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaAteMov.DataBind();

                        divisao.lista(ref tbDivisao);
                        comboDivisaoDeMov.DataSource = tbDivisao;
                        comboDivisaoDeMov.DataValueField = "COD_DIVISAO";
                        comboDivisaoDeMov.DataTextField = "DESCRICAO";
                        comboDivisaoDeMov.DataBind();

                        comboDivisaoAteMov.DataSource = tbDivisao;
                        comboDivisaoAteMov.DataValueField = "COD_DIVISAO";
                        comboDivisaoAteMov.DataTextField = "DESCRICAO";
                        comboDivisaoAteMov.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDeMov.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDeMov.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDeMov.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDeMov.DataBind();

                        comboLinhaNegocioAteMov.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAteMov.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAteMov.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAteMov.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDeMov.DataSource = tbCliente;
                        comboClienteDeMov.DataValueField = "COD_EMPRESA";
                        comboClienteDeMov.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDeMov.DataBind();

                        comboClienteAteMov.DataSource = tbCliente;
                        comboClienteAteMov.DataValueField = "COD_EMPRESA";
                        comboClienteAteMov.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAteMov.DataBind();

                        //job.lista(ref tbJob, 'A');
                        job.listaFiltroRelatorio(ref tbJob);
                        comboJobDeMov.DataSource = tbJob;
                        comboJobDeMov.DataValueField = "COD_JOB";
                        comboJobDeMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDeMov.DataBind();

                        comboJobAteMov.DataSource = tbJob;
                        comboJobAteMov.DataValueField = "COD_JOB";
                        comboJobAteMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAteMov.DataBind();

                        comboContaDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboContaAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                    }

                    painelBalancete.Visible = false;
                    painelDRE.Visible = false;
                    painelMovFinanc.Visible = true;
                    painelTitPend.Visible = false;
                    painelRazao.Visible = false;
                    botaoVaiRelatorioLayout2.Visible = true;
                    break;
                #endregion
                #region Gerando Titulos Pendentes
                case "TITULO_PENDENTE":
                    textDiasVencimento.Attributes.Add("onkeypress", "return OnlynumbersFiltrado(event);");
                    textDiasVencimento.Attributes.Add("onblur", "verificaNulo(this);");
                    strMascaras = "$(\"#" + textPeriodoTitPend.ClientID + "\").mask(\"99/99/9999\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
                        strMascaras, true);
                    subTitulo.Text = "Títulos Pendentes";
                    Title += "Relatórios - Títulos Pendentes";
                    textDiasVencimento.Text = "0";
                    textPeriodoTitPend.Text = DateTime.Now.ToString("ddMMyyyy");

                    comboDivisaoDeTitPend.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDeTitPend.UniqueID + "', '" + comboDivisaoAteTitPend.ID + "');");
                    comboLinhaNegocioDeTitPend.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDeTitPend.UniqueID + "','" + comboLinhaNegocioAteTitPend.ID + "');");
                    comboClienteDeTitPend.Attributes.Add("onChange", "selecionaAte('" + comboClienteDeTitPend.UniqueID + "','" + comboClienteAteTitPend.ID + "');");
                    comboJobDeTitPend.Attributes.Add("onChange", "selecionaAte('" + comboJobDeTitPend.UniqueID + "','" + comboJobAteTitPend.ID + "');");

                    if (!Page.IsPostBack)
                    {
                        conta.listaAnaliticas(ref tbConta);
                        comboContaDeTitPend.DataSource = tbConta;
                        comboContaDeTitPend.DataValueField = "COD_CONTA";
                        comboContaDeTitPend.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaDeTitPend.DataBind();

                        comboContaAteTitPend.DataSource = tbConta;
                        comboContaAteTitPend.DataValueField = "COD_CONTA";
                        comboContaAteTitPend.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaAteTitPend.DataBind();

                        divisao.lista(ref tbDivisao);
                        comboDivisaoDeTitPend.DataSource = tbDivisao;
                        comboDivisaoDeTitPend.DataValueField = "COD_DIVISAO";
                        comboDivisaoDeTitPend.DataTextField = "DESCRICAO";
                        comboDivisaoDeTitPend.DataBind();

                        comboDivisaoAteTitPend.DataSource = tbDivisao;
                        comboDivisaoAteTitPend.DataValueField = "COD_DIVISAO";
                        comboDivisaoAteTitPend.DataTextField = "DESCRICAO";
                        comboDivisaoAteTitPend.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDeTitPend.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDeTitPend.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDeTitPend.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDeTitPend.DataBind();

                        comboLinhaNegocioAteTitPend.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAteTitPend.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAteTitPend.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAteTitPend.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDeTitPend.DataSource = tbCliente;
                        comboClienteDeTitPend.DataValueField = "COD_EMPRESA";
                        comboClienteDeTitPend.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDeTitPend.DataBind();

                        comboClienteAteTitPend.DataSource = tbCliente;
                        comboClienteAteTitPend.DataValueField = "COD_EMPRESA";
                        comboClienteAteTitPend.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAteTitPend.DataBind();

                        //job.lista(ref tbJob, 'A');
                        job.listaFiltroRelatorio(ref tbJob);
                        comboJobDeTitPend.DataSource = tbJob;
                        comboJobDeTitPend.DataValueField = "COD_JOB";
                        comboJobDeTitPend.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDeTitPend.DataBind();

                        comboJobAteTitPend.DataSource = tbJob;
                        comboJobAteTitPend.DataValueField = "COD_JOB";
                        comboJobAteTitPend.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAteTitPend.DataBind();

                        comboContaDeTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboContaAteTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoDeTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAteTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDeTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAteTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDeTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAteTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDeTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAteTitPend.Items.Insert(0, new ListItem("Escolha", "0"));
                    }

                    painelBalancete.Visible = false;
                    painelDRE.Visible = false;
                    painelMovFinanc.Visible = false;
                    painelTitPend.Visible = true;
                    painelRazao.Visible = false;
                    break;
                #endregion
                #region Razao
                case "RAZAO":
                    botaoVaiRelatoriosintetico.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        strMascaras = "$(\"#" + textPeriodoDeRAZAO.ClientID + "\").mask(\"99/99/9999\");";
                        strMascaras += "$(\"#" + textPeriodoAteRAZAO.ClientID + "\").mask(\"99/99/9999\");";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
                            strMascaras, true);
                        subTitulo.Text = "Razão Contábil";
                        Title += "Relatórios - Razão Contábil";
                        textPeriodoDeRAZAO.Text = DateTime.Now.AddMonths(-1).ToString("ddMMyyyy");
                        textPeriodoAteRAZAO.Text = DateTime.Now.ToString("ddMMyyyy");
                    }

                    comboMoedaRazao.DataSource = moeda.loadTotal();
                    comboMoedaRazao.DataTextField = "DESCRICAO";
                    comboMoedaRazao.DataValueField = "COD_MOEDA";
                    comboMoedaRazao.DataBind();
                    comboMoedaRazao.Items.Insert(0, new ListItem("Padrão", "0"));

                    comboDivisaoDeRAZAO.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDeRAZAO.UniqueID + "', '" + comboDivisaoAteRAZAO.ID + "');");
                    comboLinhaNegocioDeRAZAO.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDeRAZAO.UniqueID + "','" + comboLinhaNegocioAteRAZAO.ID + "');");
                    comboClienteDeRAZAO.Attributes.Add("onChange", "selecionaAte('" + comboClienteDeRAZAO.UniqueID + "','" + comboClienteAteRAZAO.ID + "');");
                    comboJobDeRAZAO.Attributes.Add("onChange", "selecionaAte('" + comboJobDeRAZAO.UniqueID + "','" + comboJobAteRAZAO.ID + "');");
                    comboTerDeRAZAO.Attributes.Add("onChange", "selecionaAte('" + comboTerDeRAZAO.UniqueID + "','" + comboTerAteRAZAO.ID + "');");

                    if (!Page.IsPostBack)
                    {
                        conta.listaAnaliticas(ref tbConta);
                        comboContaDeRAZAO.DataSource = tbConta;
                        comboContaDeRAZAO.DataValueField = "COD_CONTA";
                        comboContaDeRAZAO.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaDeRAZAO.DataBind();

                        comboContaAteRAZAO.DataSource = tbConta;
                        comboContaAteRAZAO.DataValueField = "COD_CONTA";
                        comboContaAteRAZAO.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaAteRAZAO.DataBind();

                        divisao.lista(ref tbDivisao);
                        comboDivisaoDeRAZAO.DataSource = tbDivisao;
                        comboDivisaoDeRAZAO.DataValueField = "COD_DIVISAO";
                        comboDivisaoDeRAZAO.DataTextField = "DESCRICAO";
                        comboDivisaoDeRAZAO.DataBind();

                        comboDivisaoAteRAZAO.DataSource = tbDivisao;
                        comboDivisaoAteRAZAO.DataValueField = "COD_DIVISAO";
                        comboDivisaoAteRAZAO.DataTextField = "DESCRICAO";
                        comboDivisaoAteRAZAO.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDeRAZAO.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDeRAZAO.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDeRAZAO.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDeRAZAO.DataBind();

                        comboLinhaNegocioAteRAZAO.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAteRAZAO.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAteRAZAO.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAteRAZAO.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDeRAZAO.DataSource = tbCliente;
                        comboClienteDeRAZAO.DataValueField = "COD_EMPRESA";
                        comboClienteDeRAZAO.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDeRAZAO.DataBind();

                        comboClienteAteRAZAO.DataSource = tbCliente;
                        comboClienteAteRAZAO.DataValueField = "COD_EMPRESA";
                        comboClienteAteRAZAO.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAteRAZAO.DataBind();

						//job.lista(ref tbJob, 'A');
						job.listaFiltroRelatorio(ref tbJob);
						comboJobDeRAZAO.DataSource = tbJob;
                        comboJobDeRAZAO.DataValueField = "COD_JOB";
                        comboJobDeRAZAO.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDeRAZAO.DataBind();

                        comboJobAteRAZAO.DataSource = tbJob;
                        comboJobAteRAZAO.DataValueField = "COD_JOB";
                        comboJobAteRAZAO.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAteRAZAO.DataBind();

                        comboTerDeRAZAO.DataSource = tbCliente;
                        comboTerDeRAZAO.DataValueField = "COD_EMPRESA";
                        comboTerDeRAZAO.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboTerDeRAZAO.DataBind();

                        comboTerAteRAZAO.DataSource = tbCliente;
                        comboTerAteRAZAO.DataValueField = "COD_EMPRESA";
                        comboTerAteRAZAO.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboTerAteRAZAO.DataBind();

                        comboDivisaoDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboTerDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboTerAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));

                        comboContaDeRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboContaAteRAZAO.Items.Insert(0, new ListItem("Escolha", "0"));
                    }
                    painelBalancete.Visible = false;
                    painelDRE.Visible = false;
                    painelMovFinanc.Visible = false;
                    painelTitPend.Visible = false;
                    painelRazao.Visible = true;

                    break;
                #endregion
                #region Movimentação Bancária
                case "MOV_BANCARIA":
                    strMascaras = "$(\"#" + textPeriodoDeMov.ClientID + "\").mask(\"99/99/9999\");";
                    strMascaras += "$(\"#" + textPeriodoAteMov.ClientID + "\").mask(\"99/99/9999\");";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
                        strMascaras, true);
                    subTitulo.Text = "Movimentação Bancária";
                    Title += "Relatórios - Movimentação Bancária";
                    textPeriodoDeMov.Text = DateTime.Now.AddMonths(-1).ToString("ddMMyyyy");
                    textPeriodoAteMov.Text = DateTime.Now.ToString("ddMMyyyy");

                    comboDivisaoDeMov.Attributes.Add("onChange", "selecionaAte('" + comboDivisaoDeMov.UniqueID + "', '" + comboDivisaoAteMov.ID + "');");
                    comboLinhaNegocioDeMov.Attributes.Add("onChange", "selecionaAte('" + comboLinhaNegocioDeMov.UniqueID + "','" + comboLinhaNegocioAteMov.ID + "');");
                    comboClienteDeMov.Attributes.Add("onChange", "selecionaAte('" + comboClienteDeMov.UniqueID + "','" + comboClienteAteMov.ID + "');");
                    comboJobDeMov.Attributes.Add("onChange", "selecionaAte('" + comboJobDeMov.UniqueID + "','" + comboJobAteMov.ID + "');");

                    comboDetalhamento1Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,1,'Mov');");
                    comboDetalhamento2Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,2,'Mov');");
                    comboDetalhamento3Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,3,'Mov');");
                    comboDetalhamento4Mov.Attributes.Add("onChange", "ativaCombosFilhos(this,4,'Mov');");

                    if (!Page.IsPostBack)
                    {
                        ddlAgrupar.Items.Add(new ListItem("Tipo de Movimentação", "Classificacao"));
                        ddlAgrupar.Items.Add(new ListItem("Terceiro", "terceiro"));

                        conta.listaAnaliticas(ref tbConta, "BC");

                        comboContaDeMov.DataSource = tbConta;
                        comboContaDeMov.DataValueField = "COD_CONTA";
                        comboContaDeMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaDeMov.DataBind();

                        comboContaAteMov.DataSource = tbConta;
                        comboContaAteMov.DataValueField = "COD_CONTA";
                        comboContaAteMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboContaAteMov.DataBind();

                        divisao.lista(ref tbDivisao);
                        comboDivisaoDeMov.DataSource = tbDivisao;
                        comboDivisaoDeMov.DataValueField = "COD_DIVISAO";
                        comboDivisaoDeMov.DataTextField = "DESCRICAO";
                        comboDivisaoDeMov.DataBind();

                        comboDivisaoAteMov.DataSource = tbDivisao;
                        comboDivisaoAteMov.DataValueField = "COD_DIVISAO";
                        comboDivisaoAteMov.DataTextField = "DESCRICAO";
                        comboDivisaoAteMov.DataBind();

                        linhaNegocio.lista(ref tbLinhaNegocio);
                        comboLinhaNegocioDeMov.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioDeMov.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioDeMov.DataTextField = "DESCRICAO";
                        comboLinhaNegocioDeMov.DataBind();

                        comboLinhaNegocioAteMov.DataSource = tbLinhaNegocio;
                        comboLinhaNegocioAteMov.DataValueField = "COD_LINHA_NEGOCIO";
                        comboLinhaNegocioAteMov.DataTextField = "DESCRICAO";
                        comboLinhaNegocioAteMov.DataBind();

                        cliente.lista(ref tbCliente);
                        comboClienteDeMov.DataSource = tbCliente;
                        comboClienteDeMov.DataValueField = "COD_EMPRESA";
                        comboClienteDeMov.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteDeMov.DataBind();

                        comboClienteAteMov.DataSource = tbCliente;
                        comboClienteAteMov.DataValueField = "COD_EMPRESA";
                        comboClienteAteMov.DataTextField = "NOME_RAZAO_SOCIAL";
                        comboClienteAteMov.DataBind();

                        //job.lista(ref tbJob, 'A');
                        job.listaFiltroRelatorio(ref tbJob);
                        comboJobDeMov.DataSource = tbJob;
                        comboJobDeMov.DataValueField = "COD_JOB";
                        comboJobDeMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobDeMov.DataBind();

                        comboJobAteMov.DataSource = tbJob;
                        comboJobAteMov.DataValueField = "COD_JOB";
                        comboJobAteMov.DataTextField = "DESCRICAO_COMPLETO";
                        comboJobAteMov.DataBind();

                        comboContaDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboContaAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboDivisaoAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboLinhaNegocioAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboClienteAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobDeMov.Items.Insert(0, new ListItem("Escolha", "0"));
                        comboJobAteMov.Items.Insert(0, new ListItem("Escolha", "0"));
                    }

                    painelBalancete.Visible = false;
                    painelDRE.Visible = false;
                    painelMovFinanc.Visible = true;
                    painelTitPend.Visible = false;
                    painelRazao.Visible = false;
                    botaoVaiRelatorioLayout2.Visible = false;
                    divDetalhe.Visible = false;
                    ddlAgrupar.Visible = true;
                    filtrosMov.Visible = false;
                    break;

                #endregion
                default:
                    Response.Redirect("FormRelatorios.aspx", false);
                    break;
            }
        }
    }

    protected virtual void montaGrid()
    {
        empresasDAO empresas = new empresasDAO(_conn);
        gvempresas.DataSource = empresas.carregaEmpresa();
        gvempresas.DataBind();
        Repeater1.DataSource = empresas.carregaEmpresa();
        Repeater1.DataBind();
        Repeater2.DataSource = empresas.carregaEmpresa();
        Repeater2.DataBind();

    }

    protected void botaoVaiRelatorio_Click(object sender, EventArgs e)
    {
        tipoRelatorio = 2;
    }
}
