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

public partial class FormBaixaTitulos : BaseForm
{
    FolhaLancamento folha;
    Empresa empresa;
    Job job;
    ContaContabil conta;
    DataTable tbFornecedoresClientes = new DataTable("tbFornecedoresClientes");
    DataTable tbPendentesAReceber = new DataTable("tbPendentesAReceber");
    DataTable tbPendentesAPagar = new DataTable("tbPendentesAPagar");
    DataTable tbTerceiros = new DataTable("tbTerceiros");
    DataTable tbJobs = new DataTable("tbJobs");
    DataTable tbContas = new DataTable("tbContas");
    int pag = 20;

    string fTipo;
    Nullable<DateTime> fPeriodoDe;
    Nullable<DateTime> fPeriodoAte;
    Nullable<int> fTerceiro;

    protected int range = 20;
    public int getPaginaAtual(string tipo)
    {

            if (ViewState["paginaAtual_"+tipo] == null || (int)ViewState["paginaAtual_"+tipo] < 1)
                return 1;
            else
                return (int)ViewState["paginaAtual_"+tipo];
    }

    public void setPaginaAtual(string tipo, int value)
    {
        if (ViewState["paginaAtual_"+tipo] == null)
            ViewState.Add("paginaAtual_"+tipo, value);
        else
            ViewState["paginaAtual_"+tipo] = value;
    }

    public int getTotalPaginas(string tipo)
    {
        if (ViewState["totalPaginas_"+tipo] == null || (int)ViewState["totalPaginas_"+tipo] < 1)
            return 1;
        else
            return (int)ViewState["totalPaginas_"+tipo];
    }

    public void setTotalPaginas(string tipo, int value)
    {
        if (ViewState["totalPaginas_" + tipo] == null)
            ViewState.Add("totalPaginas_" + tipo, value);
        else
            ViewState["totalPaginas_" + tipo] = value;
    }

    public double getTotalRegistros(string tipo)
    {
        if (ViewState["totalRegistros_" + tipo] == null || (double)ViewState["totalRegistros_" + tipo] < 1)
            return 1;
        else
            return (double)ViewState["totalRegistros_" + tipo];
    }

    public void setTotalRegistros(string tipo, double value)
    {
        if (ViewState["totalRegistros_" + tipo] == null)
            ViewState.Add("totalRegistros_" + tipo, value);
        else
            ViewState["totalRegistros_" + tipo] = value;
    }

    public FormBaixaTitulos()
        : base("BAIXA_TITULO")
    {
        folha = new FolhaLancamento(_conn);
        empresa = new Empresa(_conn);
        job = new Job(_conn);
        conta = new ContaContabil(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        
        Title += "Baixa de Títulos";

        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/Generico.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/BaixaTitulo/BaixaTitulo.js"));

        ///////// CHOSEN /////////
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboModelo", "$('#" + comboTerceiro.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true); //.change(function(){ tela.alteraModelo(this.value, -1); })

        btVencimento_lancto.Attributes.Add("onClick", "tela.loadVencimentos(-1,'" + textParcelas_lancto.ClientID + "','" + textValor_lancto.ClientID + "','" + botaoVencimento.ClientID + "','" + H_SEQLOTE_LANCTO.ClientID + "');return false;");
        botaoSalvar_lancto.Attributes.Add("onClick", "tela.salvaLancto('" + H_SEQLOTE_LANCTO.ClientID + "', '" + H_SEQLOTE_LANCTO_MIN.ClientID + "','" + H_SEQLOTE_LANCTO_MAX.ClientID + "', '" + textData.ClientID + "','" + comboDebCred_lancto.ClientID + "','" + comboConta_lancto.ClientID + "','" + comboJob_lancto.ClientID + "','" + textValor_lancto.ClientID + "','" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "','" + comboCliente_lancto.ClientID + "','" + comboGeraTitulo_lancto.ClientID + "','" + textParcelas_lancto.ClientID + "','" + comboTerceiro_lancto.ClientID + "','" + textHistorico_lancto.ClientID + "', '0', '0'); return false;");
        btSalvarVencimento.Attributes.Add("onClick", "tela.salvaVencimentos('" + botaoSalvarVencimento.ClientID + "', '','" + textValor_lancto.ClientID + "');return false;");
        
        comboGeraTitulo_lancto.Attributes.Add("onChange", "verificaContaGeraTitulo();");
        //comboGeraTitulo_lancto.Attributes.Add("onChange", "exibeVencimentosTitulo(this.value);");
        comboJob_lancto.Attributes.Add("onChange", "tela.carregaJob(this.value, '" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "', '" + comboCliente_lancto.ClientID + "');");

        checkJobAtivo.Attributes.Add("onclick", "tela.atualizaJobs(this);");
        botaoCancelar_lancto.Attributes.Add("onClick", "tela.cancela();");
        comboConta_lancto.Attributes.Add("onChange", "tela.selecionaJobDefault(this.value);");

        textQtd_lancto.Attributes.Add("onblur", "calculaValorTotal_lancto();");
        textValorUnit_lancto.Attributes.Add("onblur", "calculaValorTotal_lancto();");
        textParcelas_lancto.Attributes.Add("onChange", "tela.verificaAlteracaoParcelas(-1, this);");

        string strMascaras = "$(\"#" + textData.ClientID + "\").mask(\"99/99/9999\");";
        //strMascaras += "$(\"#" + textValor_lancto.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        //strMascaras += "$(\"#" + textValorUnit_lancto.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        strMascaras += "$(\"#" + textPeriodoDe.ClientID + "\").mask(\"99/99/9999\");";
        strMascaras += "$(\"#" + textPeriodoAte.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);

        textValorUnit_lancto.Attributes.Add("onkeyup", "formataValor(this);");
        textValor_lancto.Attributes.Add("onkeyup", "formataValor(this);");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "inciaObjetoTela", "var tela = new BaixaTitulo('" + modulo + "');", true);
        
        if (!Page.IsPostBack)
        {
            Txtpag.Text = "20";

            empresa.listaFornecedoresClientesBaixar(ref tbFornecedoresClientes);
            comboTerceiro.DataSource = tbFornecedoresClientes;
            comboTerceiro.DataTextField = "DESCRICAO_MISTA";
            comboTerceiro.DataValueField = "COD_EMPRESA";
            comboTerceiro.DataBind();
            comboTerceiro.Items.Insert(0, new ListItem("Escolha", "0"));

            job.lista(ref tbJobs, 'A');
            comboJob_lancto.DataSource = tbJobs;
            comboJob_lancto.DataTextField = "DESCRICAO_COMPLETO";
            comboJob_lancto.DataValueField = "COD_JOB";
            comboJob_lancto.DataBind();
            comboJob_lancto.Items.Insert(0, new ListItem("Escolha", "0"));

            conta.listaAnaliticas(ref tbContas);
            comboConta_lancto.DataSource = tbContas;
            comboConta_lancto.DataTextField = "DESCRICAO_COMPLETO";
            comboConta_lancto.DataValueField = "COD_CONTA";
            comboConta_lancto.DataBind();
            comboConta_lancto.Items.Insert(0, new ListItem("Escolha", "0"));

            empresa.listaFornecedoresClientes(ref tbTerceiros);
            comboTerceiro_lancto.DataSource = tbTerceiros;
            comboTerceiro_lancto.DataTextField = "DESCRICAO_MISTA";
            comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
            comboTerceiro_lancto.DataBind();

            DataTable tbConsultores = new DataTable("consultores");
            empresa.listaConsultores(ref tbConsultores);
            comboConsultor_lancto.DataSource = tbConsultores;
            comboConsultor_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
            comboConsultor_lancto.DataValueField = "COD_EMPRESA";
            comboConsultor_lancto.DataBind();

            comboConsultor_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboTerceiro_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboCliente_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboDivisao_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboLinhaNegocio_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            checkJobAtivo.Checked = true;

            montaGrid("CP");
            montaGrid("CR");
        }

        botaoSalvar.Attributes.Add("onClick", "tela.salvaFolha(this);return false;");
        //textData.Text = DateTime.Now.ToString("ddMMyyyy");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIniciaTela", "tela.iniciaTela();", true);

        
    }

    protected void botaoFiltrar_Click(object sender, EventArgs e)
    {
        if (comboTipo.SelectedValue == "T")
        {
            repeaterLanctosAPagar.Visible = true;
            repeaterLanctosAReceber.Visible = true;

            montaGrid("CP");
            montaGrid("CR");
        }
        else if (comboTipo.SelectedValue == "CP")
        {
            repeaterLanctosAPagar.Visible = true;
            repeaterLanctosAReceber.Visible = false;
            montaGrid(comboTipo.SelectedValue);
        }
        else if (comboTipo.SelectedValue == "CR")
        {
            repeaterLanctosAPagar.Visible = false;
            repeaterLanctosAReceber.Visible = true;
            montaGrid(comboTipo.SelectedValue);
        }
    }

    public void repeaterLanctosAReceber_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        RepeaterItem item = e.Item;
        DataRowView rowItem = (DataRowView)e.Item.DataItem;
        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
        HiddenField hdValor = (HiddenField)item.FindControl("hdValor");
        HiddenField hiddenLotePai = (HiddenField)item.FindControl("hiddenLotePai");
        HiddenField hiddenSeqLotePai = (HiddenField)item.FindControl("hiddenSeqLotePai");
        HiddenField hiddenSeqBaixa = (HiddenField)item.FindControl("hiddenSeqBaixa");
        HiddenField hiddenModuloPai = (HiddenField)item.FindControl("hiddenModuloPai");
        HyperLink linkEditaLote = (HyperLink)item.FindControl("linkEditaLote");
        Literal historicoLiteral = (Literal)item.FindControl("historicoLiteral");

        FolhaLancamento folha = new FolhaLancamento(_conn);
        SLancamento lancto1 = folha.getLancamentoPai(Convert.ToDouble(hiddenLotePai.Value), 1, hiddenModuloPai.Value);
        SLancamento lanctoPai = folha.getLancamentoPai(Convert.ToDouble(hiddenLotePai.Value), Convert.ToInt32(hiddenSeqLotePai.Value), hiddenModuloPai.Value);
        historicoLiteral.Text = rowItem["historico"] + " - Nº Doc: "+lanctoPai.numeroDocumento +" - Terceiro: " + lancto1.descTerceiro;

        if(hiddenModuloPai.Value.Equals("BAIXA_TITULO"))
            linkEditaLote.NavigateUrl = "FormDetalheBaixa.aspx?id=" + hiddenSeqBaixa.Value;
        else
            linkEditaLote.NavigateUrl = "FormGenericTitulos.aspx?modulo=" + hiddenModuloPai.Value + "&lote=" + hiddenLotePai.Value;
        check.Attributes.Add("onClick", "tela.calculaTotal('gridLanctosAReceber', 'boxTotalAReceber','RECEBER');");
    }

    public void repeaterLanctosAPagar_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        DataRowView rowItem = (DataRowView)e.Item.DataItem;
        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
        HiddenField hdValor = (HiddenField)item.FindControl("hdValor");
        HiddenField hiddenLotePai = (HiddenField)item.FindControl("hiddenLotePai");
        HiddenField hiddenSeqLotePai = (HiddenField)item.FindControl("hiddenSeqLotePai");
        HiddenField hiddenSeqBaixa = (HiddenField)item.FindControl("hiddenSeqBaixa");
        HiddenField hiddenModuloPai = (HiddenField)item.FindControl("hiddenModuloPai");
        HyperLink linkEditaLote = (HyperLink)item.FindControl("linkEditaLote");
        Literal historicoLiteral = (Literal)item.FindControl("historicoLiteral");

        FolhaLancamento folha = new FolhaLancamento(_conn);
        SLancamento lancto1 = folha.getLancamentoPai(Convert.ToDouble(hiddenLotePai.Value), 1, hiddenModuloPai.Value);
        SLancamento lanctoPai = folha.getLancamentoPai(Convert.ToDouble(hiddenLotePai.Value), Convert.ToInt32(hiddenSeqLotePai.Value), hiddenModuloPai.Value);
        historicoLiteral.Text = rowItem["historico"] + " - Nº Doc: " + lanctoPai.numeroDocumento + " - Terceiro: " + lancto1.descTerceiro;

        if (hiddenModuloPai.Value.Equals("BAIXA_TITULO"))
            linkEditaLote.NavigateUrl = "FormDetalheBaixa.aspx?id=" + hiddenSeqBaixa.Value;
        else
            linkEditaLote.NavigateUrl = "FormGenericTitulos.aspx?modulo=" + hiddenModuloPai.Value + "&lote=" + hiddenLotePai.Value;
        check.Attributes.Add("onClick", "tela.calculaTotal('gridLanctosAPagar', 'boxTotalAPagar','PAGAR');");
    }

    protected override void montaTela()
    {
        base.montaTela();
        subTitulo.Text = "Baixa de Títulos";
    }

    protected virtual void montaGrid(string tipo)
    {
        int.TryParse(Txtpag.Text, out pag);
        if (pag > 2000)
        {
            pag = 2000;
            Txtpag.Text = "2000";
        }
        range = pag;

        if (textPeriodoDe.Text != "")
            fPeriodoDe = Convert.ToDateTime(textPeriodoDe.Text);
        else
            fPeriodoDe = null;

        if (textPeriodoAte.Text != "")
            fPeriodoAte = Convert.ToDateTime(textPeriodoAte.Text);
        else
            fPeriodoAte = null;

        if (comboTerceiro.SelectedValue != "0")
            fTerceiro = Convert.ToInt32(comboTerceiro.SelectedValue);
        else
            fTerceiro = null;

        switch (tipo)
        {
            case "CP":
                //repeaterLanctosAPagar.Visible = true;
                //repeaterLanctosAReceber.Visible = false;

                setTotalRegistros(tipo, folha.totalRegistrosPendentes("CAP_INCLUSAO_TITULO", fPeriodoDe, fPeriodoAte, fTerceiro));
                folha.listaPendentes(ref tbPendentesAPagar, "CAP_INCLUSAO_TITULO", fPeriodoDe, fPeriodoAte, fTerceiro, getPaginaAtual(tipo), pag);
                repeaterLanctosAPagar.DataSource = tbPendentesAPagar;
                repeaterLanctosAPagar.DataBind();
                break;
            case "CR":
                //repeaterLanctosAPagar.Visible = false;
                //repeaterLanctosAReceber.Visible = true;

                setTotalRegistros(tipo, folha.totalRegistrosPendentes("CAR_INCLUSAO_TITULO", fPeriodoDe, fPeriodoAte, fTerceiro));
                folha.listaPendentes(ref tbPendentesAReceber, "CAR_INCLUSAO_TITULO", fPeriodoDe, fPeriodoAte, fTerceiro, getPaginaAtual(tipo), pag);
                repeaterLanctosAReceber.DataSource = tbPendentesAReceber;
                repeaterLanctosAReceber.DataBind();
                break;
        }

        Label lNenhumRegistro = (Label)FindControlRecursive(this, "labelNenhumRegistro_" + tipo);
        if (getTotalRegistros(tipo) <= 0)
        {
            lNenhumRegistro.Visible = true;
        }
        else
        {
            lNenhumRegistro.Visible = false;
        }
        
        this.montaPaginacao(tipo);
    }

    protected void montaPaginacao(string tipo)
    {
        int.TryParse(Txtpag.Text, out pag);
        if (pag > 2000)
        {
            pag = 2000;
            Txtpag.Text = "2000";
        }
        range = pag;
        HtmlContainerControl boxPaginacao = (HtmlContainerControl)FindControlRecursive(this, "boxPaginacao_" + tipo);
        LinkButton linkPrimeira = (LinkButton)FindControlRecursive(this, "linkPrimeira_" + tipo);
        LinkButton linkUltima = (LinkButton)FindControlRecursive(this, "linkUltima_" + tipo);
        LinkButton linkAnterior = (LinkButton)FindControlRecursive(this, "linkPaginacaoAnterior_" + tipo);
        LinkButton linkProximo = (LinkButton)FindControlRecursive(this, "linkPaginacaoProximo_" + tipo);

        this.setTotalPaginas(tipo,Convert.ToInt32(Math.Ceiling((getTotalRegistros(tipo) / this.range))));
        if ((getTotalRegistros(tipo) > 0 && getTotalPaginas(tipo) > 1))
        {
            boxPaginacao.Visible = true;
            linkPrimeira.Visible = true;
            linkUltima.Visible = true;
            linkAnterior.Visible = true;
            linkProximo.Visible = true;

            Paginacao paginacao = new Paginacao(getPaginaAtual(tipo), getTotalPaginas(tipo), range);

            paginacao.getInicioFim();

            if (getPaginaAtual(tipo) <= 1)
            {
                linkPrimeira.Enabled = false;
                linkAnterior.Enabled = false;
            }
            else
            {
                linkPrimeira.Enabled = true;
                linkAnterior.Enabled = true;
            }

            if (getPaginaAtual(tipo) >= getTotalPaginas(tipo))
            {
                linkUltima.Enabled = false;
                linkProximo.Enabled = false;
            }
            else
            {
                linkUltima.Enabled = true;
                linkProximo.Enabled = true;
            }

            for (int i = 1; i <= 10; i++)
            {
                LinkButton link = (LinkButton)FindControlRecursive(this, "linkPaginacao" + i + "_" + tipo);
                link.Visible = false;
            }

            int contPaginacao = 1;
            for (int i = paginacao.inicio; i <= paginacao.fim; i++)
            {
                LinkButton link = (LinkButton)FindControlRecursive(this, "linkPaginacao" + contPaginacao + "_" + tipo);
                if (link != null)
                {
                    if (getPaginaAtual(tipo) == i)
                    {
                        link.CssClass = "atual";
                        link.Enabled = false;
                    }
                    else
                    {
                        link.CssClass = "";
                        link.Enabled = true;
                    }
                    link.Visible = true;
                    link.Click += new EventHandler(linkPaginacao_Click);
                    link.Text = i.ToString();
                }
                contPaginacao++;
            }
        }
        else
        {
            linkPrimeira.Visible = false;
            linkUltima.Visible = false;
            linkAnterior.Visible = false;
            linkProximo.Visible = false;

            for (int i = 1; i <= 10; i++)
            {
                LinkButton link = (LinkButton)FindControlRecursive(this, "linkPaginacao" + i + "_" + tipo);
                link.Visible = false;
            }
        }
    }

    protected virtual void linkPaginacao_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["Tipo"];
        if (link != null)
            setPaginaAtual(tipo,Convert.ToInt32(link.Text));

        this.montaGrid(tipo);
    }

    protected virtual void linkPaginacaoAnterior_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["Tipo"];
        int pagina = getPaginaAtual(tipo);
        pagina--;
        setPaginaAtual(tipo, pagina);
        this.montaGrid(tipo);
    }

    protected virtual void linkPaginacaoProximo_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["Tipo"];
        int pagina = getPaginaAtual(tipo);
        pagina++;
        setPaginaAtual(tipo, pagina);
        this.montaGrid(tipo);
    }

    protected virtual void linkPrimeira_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["Tipo"];
        setPaginaAtual(tipo, 1);
        this.montaGrid(tipo);
    }

    protected virtual void linkUltima_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["Tipo"];
        setPaginaAtual(tipo, getTotalPaginas(tipo));
        this.montaGrid(tipo);
    }
}
