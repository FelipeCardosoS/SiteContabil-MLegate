using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
using System.Linq;

public partial class FormGenericTitulos : BaseForm
{
    private Fornecedor fornecedor;
    private Cliente cliente;
    private Empresa empresa;
    private ContaContabil conta;
    private Job job;
    private LinhaNegocio linhaNegocio;
    private Divisao divisao;
    private Modelo modelo;
    private FolhaLancamento folha;

    private DataTable tbTerceiros = new DataTable("tbTerceiros");
    private DataTable tbFornecedores = new DataTable("tbFornecedores");
    private DataTable tbClientes = new DataTable("tbClientes");
    private DataTable tbContas = new DataTable("tbContas");
    private DataTable tbJobs = new DataTable("tbJobs");
    private DataTable tbLinhasNegocio = new DataTable("tbLinhasNegocio");
    private DataTable tbDivisoes = new DataTable("tbDivisoes");
    private DataTable tbModelosFornecedor = new DataTable("tbModelosFornecedor");
    private DataTable tbModelosCliente = new DataTable("tbModelosCliente");
    private DataTable tbModelosContabilidade = new DataTable("tbModelosContabilidade");

    public FormGenericTitulos()
        : base("")
    {
        empresa = new Empresa(_conn);
        fornecedor = new Fornecedor(_conn);
        cliente = new Cliente(_conn);
        conta = new ContaContabil(_conn);
        job = new Job(_conn);
        linhaNegocio = new LinhaNegocio(_conn);
        divisao = new Divisao(_conn);
        modelo = new Modelo(_conn);
        folha = new FolhaLancamento(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        if (Request.QueryString["modulo"] != null)
        {
            modulo = Request.QueryString["modulo"].ToString();
        }
        base.Page_PreLoad(sender, e);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        //Master.Body.Attributes.Add("onKeyDown", "salvaTecla()");
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/Generico.js"));
        //Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/toggleLoading.jquery.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/ContasPagar.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/ContasReceber.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/ModeloLancamento.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/FolhaLancamento/Contabilidade.js"));
        Master.GetScriptManager.Scripts.Add(new ScriptReference("Js/Metodos.js"));        

        btVencimento.Attributes.Add("onClick", "tela.loadVencimentos(1,'" + textParcelas.ClientID + "','" + textValorTotal.ClientID + "','" + botaoVencimento.ClientID + "','" + H_SEQLOTE_LANCTO.ClientID + "');return false;");
        btVencimento_lancto.Attributes.Add("onClick", "tela.loadVencimentos(-1,'" + textParcelas_lancto.ClientID + "','" + textValor_lancto.ClientID + "','" + botaoVencimento.ClientID + "','" + H_SEQLOTE_LANCTO.ClientID + "');return false;");
        btSalvarVencimento.Attributes.Add("onClick", "tela.salvaVencimentos('" + botaoSalvarVencimento.ClientID + "', '" + textValorTotal.ClientID + "','" + textValor_lancto.ClientID + "');return false;");
        botaoSalvar_lancto.Attributes.Add("onClick", "tela.salvaLancto('" + H_SEQLOTE_LANCTO.ClientID + "', '" + H_SEQLOTE_LANCTO_MIN.ClientID + "','" + H_SEQLOTE_LANCTO_MAX.ClientID + "', '" + textData.ClientID + "','" + comboDebCred_lancto.ClientID + "','" + comboConta_lancto.ClientID + "','" + comboJob_lancto.ClientID + "','" + textValor_lancto.ClientID + "','" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "','" + comboCliente_lancto.ClientID + "','" + comboGeraTitulo_lancto.ClientID + "','" + textParcelas_lancto.ClientID + "','" + comboTerceiro_lancto.ClientID + "','" + textHistorico_lancto.ClientID + "','" + textValorBruto.ClientID + "',0); return false;");
        botaoSalvar_item.Attributes.Add("onClick", "tela.salvaItem(); return false;");

        ///////////////// CHOSEN /////////////////

        //comboModelo.Attributes.Add("onChange", "tela.alteraModelo(this.value, -1);");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboModelo", "$('#" + comboModelo.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"}).change(function(){ tela.alteraModelo(this.value, -1); });", true);

        //comboFornecedor.Attributes.Add("onChange", "tela.carregaModelo(this.value, -1);");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboFornecedor", "$('#" + comboFornecedor.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"}).change(function(){ tela.carregaModelo(this.value, -1); });", true);

        //comboConsultor.Attributes.Add("onChange", "tela.alteraConsultorLancto1(this.value);");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboConsultor", "$('#" + comboConsultor.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"}).change(function(){ tela.alteraConsultorLancto1(this.value); });", true);

        //comboConta_lancto.Attributes.Add("onChange", "tela.selecionaJobDefault(this.value);");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboConta_lancto", "$('#" + comboConta_lancto.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"}).change(function(){ tela.selecionaJobDefault(this.value); });", true);

        //comboJob_lancto.Attributes.Add("onChange", "tela.carregaJob(this.value, '" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "', '" + comboCliente_lancto.ClientID + "');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboJob_lancto", "$('#" + comboJob_lancto.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"}).change(function(){ tela.carregaJob(this.value, '" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "', '" + comboCliente_lancto.ClientID + "'); });", true);

        //comboCliente_lancto
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboCliente_lancto", "$('#" + comboCliente_lancto.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);

        //comboConsultor_lancto
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboConsultor_lancto", "$('#" + comboConsultor_lancto.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);

        //comboTerceiro_lancto
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboTerceiro_lancto", "$('#" + comboTerceiro_lancto.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\", width:100 });", true);

        //////////////////////////////////////////

        //comboGeraTitulo_lancto.Attributes.Add("onChange", "exibeVencimentosTitulo(this.value);");
        comboGeraTitulo_lancto.Attributes.Add("onChange", "verificaContaGeraTitulo();");

        comboEtapaMoeda.Attributes.Add("onChange", "alteraEtapaMoeda();");
        comboEtapaMoedaInclusaLancto.Attributes.Add("onChange", "alteraEtapaMoeda();");

        textNumeroDocumento.Attributes.Add("onblur", "tela.alteraNumeroDocumentoLancto1(this);");
        textNumeroDocumento.Attributes.Add("onfocus", "tela.pegaNumeroDocumentoOldLancto1(this);");

        textQtd.Attributes.Add("onblur", "tela.alteraQtdLancto1(this);");
        textQtd.Attributes.Add("onfocus", "tela.pegaQtdOldLancto1(this);");

        textValorUnit.Attributes.Add("onblur", "tela.alteraValorLancto1(this);");
        textValorUnit.Attributes.Add("onfocus", "tela.pegaValorOldLancto1(this);");

        textQtd_lancto.Attributes.Add("onblur", "calculaValorTotal_lancto();");
        textValorUnit_lancto.Attributes.Add("onblur", "calculaValorTotal_lancto();");

        textValorBruto.Attributes.Add("onChange", "alteraValorBruto();");

        textParcelas.Attributes.Add("onChange", "tela.verificaAlteracaoParcelas(1, this);");
        
        if (modulo != "MODELO_LANCAMENTO")
        {
            textParcelas_lancto.Attributes.Add("onChange", "tela.verificaAlteracaoParcelas(-1, this);");
        }
        checkJobAtivo.Attributes.Add("onclick", "tela.atualizaJobs(this);");
        
        botaoCancelar_lancto.Attributes.Add("onClick", "tela.cancela();");
        botaoCancelar_item.Attributes.Add("onClick", "tela.cancelaItem();");
        //string strMascaras = "$(\"#" + textValorTotal.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        //strMascaras += "$(\"#" + textValorUnit.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        //strMascaras += "$(\"#" + textValorUnit_lancto.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        //strMascaras += "$(\"#" + textValor_lancto.ClientID + "\").priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});";
        string strMascaras = "$(\"#" + textData.ClientID + "\").mask(\"99/99/9999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras", strMascaras, true);

        textValorTotal.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textValorUnit.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textValorUnit_lancto.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textValor_lancto.Attributes.Add("onkeypress", "return Onlynumbers(event);");

        textValorNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIcmsNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIpiNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textFreteNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textDescontoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textBaseImpNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textCsllNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIrNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIssNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textPisRetidoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textCofinsRetidoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textCsllRetidoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIrRetidoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIssRetidoNota.Attributes.Add("onkeypress", "return Onlynumbers(event);");

        textQtdeItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textValorTotalItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textBaseImpItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIcmsItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textIpiItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textPisItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");
        textCofinsItem.Attributes.Add("onkeypress", "return Onlynumbers(event);");

        if (!Page.IsPostBack)
        {
			textHistorico_lancto.Attributes.Add("maxlength", textHistorico_lancto.MaxLength.ToString());
			produtosDAO produtoDAO = new produtosDAO(_conn);
            List<SProduto> listProdutos = produtoDAO.list(SessionView.EmpresaSession);
            comboProdutoItem.DataSource = listProdutos;
            comboProdutoItem.DataValueField = "codProduto";
            comboProdutoItem.DataTextField = "descricao";
            comboProdutoItem.DataBind();
            comboProdutoItem.Items.Insert(0, new ListItem("Escolha", "0"));

            job.lista(ref tbJobs, 'A');
            comboJob_lancto.DataSource = tbJobs;
            comboJob_lancto.DataTextField = "DESCRICAO_COMPLETO";
            comboJob_lancto.DataValueField = "COD_JOB";
            comboJob_lancto.DataBind();
            comboJob_lancto.Items.Insert(0, new ListItem("Escolha", "0"));

            //conta.listaAnaliticas(ref tbContas);
            //comboConta_lancto.DataSource = tbContas;
            //comboConta_lancto.DataTextField = "DESCRICAO_COMPLETO";
            //comboConta_lancto.DataValueField = "COD_CONTA";
            //comboConta_lancto.DataBind();
            //comboConta_lancto.Items.Insert(0, new ListItem("Escolha", "0"));

            DataTable tbConsultores = new DataTable("consultores");
            empresa.listaConsultores(ref tbConsultores);
            comboConsultor_lancto.DataSource = tbConsultores;
            comboConsultor_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
            comboConsultor_lancto.DataValueField = "COD_EMPRESA";
            comboConsultor_lancto.DataBind();

            comboConsultor.DataSource = tbConsultores;
            comboConsultor.DataTextField = "NOME_RAZAO_SOCIAL";
            comboConsultor.DataValueField = "COD_EMPRESA";
            comboConsultor.DataBind();

            moedaDAL moedaDAL = new moedaDAL(_conn);
            comboEtapaMoeda.DataSource = moedaDAL.loadTotal();
            comboEtapaMoeda.DataTextField = "DESCRICAO";
            comboEtapaMoeda.DataValueField = "COD_MOEDA";
            comboEtapaMoeda.DataBind();
            comboEtapaMoeda.Items.Insert(0, new ListItem("Moeda Padrão", "0"));

            comboEtapaMoedaInclusaLancto.DataSource = moedaDAL.loadTotal();
            comboEtapaMoedaInclusaLancto.DataTextField = "DESCRICAO";
            comboEtapaMoedaInclusaLancto.DataValueField = "COD_MOEDA";
            comboEtapaMoedaInclusaLancto.DataBind();
            comboEtapaMoedaInclusaLancto.Items.Insert(0, new ListItem("Moeda Padrão", "0"));

            switch (modulo)
            {
                case "CAP_INCLUSAO_TITULO":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "inciaObjetoTela", "var tela = new ContasPagar('" + modulo + "');", true);
                    ltTerceiro.Text = "Fornecedor";
                    Title += "Inclusão de Títulos à Pagar ";
                    comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));

                    fornecedor.lista(ref tbFornecedores);
                    comboFornecedor.DataSource = tbFornecedores;
                    comboFornecedor.DataTextField = "NOME_RAZAO_SOCIAL";
                    comboFornecedor.DataValueField = "COD_EMPRESA";
                    comboFornecedor.DataBind();

                    fornecedor.lista(ref tbFornecedores);
                    comboTerceiro_lancto.DataSource = tbFornecedores;
                    comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                    comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                    comboTerceiro_lancto.DataBind();

                    break;
                case "CAR_INCLUSAO_TITULO":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "inciaObjetoTela", "var tela = new ContasReceber('" + modulo + "');", true);
                    ltTerceiro.Text = "Cliente";
                    Title += "Inclusão de Títulos à Receber ";
                    comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));

                    cliente.lista(ref tbClientes);
                    comboFornecedor.DataSource = tbClientes;
                    comboFornecedor.DataTextField = "NOME_RAZAO_SOCIAL";
                    comboFornecedor.DataValueField = "COD_EMPRESA";
                    comboFornecedor.DataBind();

                    cliente.lista(ref tbClientes);
                    comboTerceiro_lancto.DataSource = tbClientes;
                    comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                    comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                    comboTerceiro_lancto.DataBind();

                    break;
                case "C_INCLUSAO_LANCTO":
                    Title += "Inclusão de Lançamentos ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "inciaObjetoTela", "var tela = new Contabilidade('" + modulo + "');", true);
                    labelNumeroDocumento.Visible = false;
                    labelTerceiro.Visible = false;
                    labelLiquido.Visible = false;
                    labelParcelas.Visible = false;
                    labelQtd.Visible = false;
                    labelConsultor.Visible = false;
                    labelValorUnit.Visible = false;
                    labelValorBruto.Visible = false;
                    labelEtapaMoeda.Visible = true;
                    comboFornecedor.Visible = false;
                    textValorTotal.Visible = false;
                    textParcelas.Visible = false;
                    btVencimento.Visible = false;
                    comboModelo.Enabled = true;
                    textData.Enabled = true;
                    textQtd.Visible = false;
                    textValorUnit.Visible = false;
                    textValorBruto.Enabled = true;
                    comboConsultor.Visible = false;

                    empresa.listaFornecedoresClientes(ref tbTerceiros);
                    comboTerceiro_lancto.DataSource = tbTerceiros;
                    comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                    comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                    comboTerceiro_lancto.DataBind();

                    modelo.listaTipoC(ref tbModelosContabilidade);
                    comboModelo.DataSource = tbModelosContabilidade;
                    comboModelo.DataTextField = "NOME";
                    comboModelo.DataValueField = "COD_MODELO";
                    comboModelo.DataBind();

                    comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));
                    
                    break;
                case "MODELO_LANCAMENTO":
                    Title += "Folha de Lançamento Modelo ";
                    btVencimento.Visible = false;
                    btVencimento_lancto.Visible = false;
                    comboJob_lancto.Enabled = true;
                    botaoSalvar_lancto.Attributes.Add("onClick", "tela.salvaLancto('" + H_SEQLOTE_LANCTO.ClientID + "', '" + H_SEQLOTE_LANCTO_MIN.ClientID + "','" + H_SEQLOTE_LANCTO_MAX.ClientID + "', '" + textData.ClientID + "','" + comboDebCred_lancto.ClientID + "','" + comboConta_lancto.ClientID + "','" + comboJob_lancto.ClientID + "','" + textValor_lancto.ClientID + "','" + comboLinhaNegocio_lancto.ClientID + "','" + comboDivisao_lancto.ClientID + "','" + comboCliente_lancto.ClientID + "','" + comboGeraTitulo_lancto.ClientID + "','" + textParcelas_lancto.ClientID + "','" + comboTerceiro_lancto.ClientID + "','" + textHistorico_lancto.ClientID + "','0'); return false;");

                    modelo.codigo = Convert.ToInt32(Request.QueryString["modelo"]);
                    modelo.load();

                    boxModelo.Visible = true;
                    boxModelo.Text = "<strong>Modelo:</strong> " + modelo.nome;

                    textGrupoDoc.Text = "Grupo";
                    textGrupo.Visible = true;
                    textNumeroDocumento_lancto.Visible = false;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "inciaObjetoTela", "var tela = new ModeloLancamento('" + modulo + "'," + Request.QueryString["modelo"] + ",'" + Request.QueryString["tipo"] + "');", true);
                    if (Request.QueryString["tipo"] != null && Request.QueryString["modelo"] != null)
                    {
                        int codModelo = Convert.ToInt32(Request.QueryString["modelo"]);
                        string tipo = Request.QueryString["tipo"].ToString();

                        form_inclusao_titulo.Visible = false;

                        switch (tipo)
                        {
                            case "CP":

                                ltTerceiro.Text = "Fornecedor";
                                comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));

                                fornecedor.lista(ref tbFornecedores);
                                comboFornecedor.DataSource = tbFornecedores;
                                comboFornecedor.DataTextField = "NOME_RAZAO_SOCIAL";
                                comboFornecedor.DataValueField = "COD_EMPRESA";
                                comboFornecedor.DataBind();

                                fornecedor.lista(ref tbFornecedores);
                                comboTerceiro_lancto.DataSource = tbFornecedores;
                                comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                                comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                                comboTerceiro_lancto.DataBind();

                                break;
                            case "CR":

                                ltTerceiro.Text = "Cliente";
                                comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));

                                cliente.lista(ref tbClientes);
                                comboFornecedor.DataSource = tbClientes;
                                comboFornecedor.DataTextField = "NOME_RAZAO_SOCIAL";
                                comboFornecedor.DataValueField = "COD_EMPRESA";
                                comboFornecedor.DataBind();

                                cliente.lista(ref tbClientes);
                                comboTerceiro_lancto.DataSource = tbClientes;
                                comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                                comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                                comboTerceiro_lancto.DataBind();

                                break;
                            case "C":

                                labelTerceiro.Visible = false;
                                labelLiquido.Visible = false;
                                labelParcelas.Visible = false;
                                comboFornecedor.Visible = false;
                                textValorTotal.Visible = false;
                                textParcelas.Visible = false;
                                btVencimento.Visible = false;
                                comboModelo.Enabled = true;
                                textData.Enabled = true;

                                modelo.listaTipoC(ref tbModelosContabilidade);
                                comboModelo.DataSource = tbModelosContabilidade;
                                comboModelo.DataTextField = "NOME";
                                comboModelo.DataValueField = "COD_MODELO";
                                comboModelo.DataBind();

                                empresa.listaFornecedoresClientes(ref tbTerceiros);
                                comboTerceiro_lancto.DataSource = tbTerceiros;
                                comboTerceiro_lancto.DataTextField = "NOME_RAZAO_SOCIAL";
                                comboTerceiro_lancto.DataValueField = "COD_EMPRESA";
                                comboTerceiro_lancto.DataBind();

                                comboModelo.Items.Insert(0, new ListItem("Escolha", "0"));

                                break;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alteraNenhumTipoEnviado", "alert('Nenhum Tipo enviado!');", true);
                        Response.Redirect("FormGridModelos.aspx", true);
                    }
                    break;
            }

            comboConsultor_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboConsultor.Items.Insert(0, new ListItem("Escolha", "0"));
            comboFornecedor.Items.Insert(0, new ListItem("Escolha", "0"));
            comboTerceiro_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboCliente_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboDivisao_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            comboLinhaNegocio_lancto.Items.Insert(0, new ListItem("Escolha", "0"));
            checkJobAtivo.Checked = true;

            botaoVoltar.Attributes.Add("onclick", "tela.voltarTela();return false;");
            if (!(bool)HttpContext.Current.Session["exige_contribuicoes"])
                botaoConcluido.Attributes.Add("onclick", "tela.salvaTela('MIN'); return false;");
            else
                botaoConcluido.Enabled = false;

            if (Request.QueryString["lote"] != null)
            {
                double lote = Convert.ToDouble(Request.QueryString["lote"]);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "setalote", "tela.loteAtual = " + lote + ";", true);
                if (modulo == "C_INCLUSAO_LANCTO")
                    botaoSalvar.Attributes.Add("onClick", "tela.alteraFolha(this," + Request.QueryString["lote"] + ");return false;");
                else
                    botaoSalvar.Attributes.Add("onClick", "tela.salvaFolha(this);return false;");

                labelLote.Visible = true;
                labelLote.Text = "Lote: " + Request.QueryString["lote"].ToString();

                //carrega folha de lançamento
                List<SLancamento> l = folha.carregaLote(lote);
                Session["ss_lancamentos_" + modulo] = l;

                //carrega log de alterações 
                LogController logController = new LogController(_conn);
                List<Log> logs = logController.list(modulo, lote);
                string texto = "";
                
                for (int i = 0; i < logs.Count; i++)
                {
                    Usuario usuario = new Usuario(_conn);
                    usuario.id=  logs[i].usuario;
                    usuario.load();
                    texto += logs[i].descricao + " por: " + usuario.nome + " em " + logs[i].dataHora.ToString("dd/MM/yyyy H:mm:ss") + " - ";
                }
                logLiteral.Text = texto;

                //carrega notaFiscal
                notaFiscalDAO notaDAO = new notaFiscalDAO(_conn);
                SNotaFiscal nota = notaDAO.getOfLote(lote, SessionView.EmpresaSession);
                if (nota != null)
                {
                    numeroNotaOldHidden.Value = nota.numeroNota.ToString();
                    nota.listItens();
                    SessionView.NotaFiscalSession = nota;
                }
            }
            else
            {
                botaoSalvar.Attributes.Add("onClick", "tela.salvaFolha(this);return false;");
                labelLote.Visible = false;
                textData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //dataHidden.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIniciaTela", "tela.iniciaTela();", true);
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        switch (modulo)
        {
            case "CAR_INCLUSAO_TITULO":
                subTitulo.Text = "Inclusão de Título à Receber";
                break;
            case "CAP_INCLUSAO_TITULO":
                subTitulo.Text = "Inclusão de Título à Pagar";
                break;
            case "C_INCLUSAO_LANCTO":
                subTitulo.Text = "Inclusão de Lançamentos";
                break;
            case "MODELO_LANCAMENTO":
                addSubTitulo("Modelos", "FormGridModelos.aspx");
                subTitulo.Text = "Lançamentos";
                break;
        }
    }

    [WebMethod]
    public static double aliquotaImposto(string TIPOIMPOSTO)
    {
        Conexao conn = new Conexao();
        aliquotasTipoImpostoDAO aliquotaDAO = new aliquotasTipoImpostoDAO(conn);
        SAliquotaImposto aliq =  aliquotaDAO.load(TIPOIMPOSTO, false, SessionView.EmpresaSession);
        if (aliq == null)
            return 0;
        return aliq.aliquota;
    }

    [WebMethod]
    public static string travaJob() 
    {
        return ConfigurationManager.AppSettings["bloqueiaJob"].ToString();
    }

    [WebMethod]
    public static bool exigeModelo()
    {
        return (bool)HttpContext.Current.Session["exige_modelo"];
    }

    [WebMethod]
    public static bool verificaGeraCredito(ArrayList contas)
    {
        Conexao conn = new Conexao();
        contasDAO contaDAO = new contasDAO(conn);
        List<string> list = new List<string>();
        for (int i = 0; i < contas.Count; i++)
        {
            list.Add(contas[i].ToString());
        }
        return contaDAO.existeGeraCredito(list);
    }

    [WebMethod]
    public static List<string> salvaTela(string modulo, string data, string tipo, double lote, double numeroNotaOld)
    {
        List<string> erros = new List<string>();
        Conexao conn = new Conexao();
        string alteracaoFolha = "FULL";
        try
        {
            //valida folha de lançamento
            erros = validaFolha(modulo, data);
            if (erros.Count > 0)
            {
                return erros;
            }

            if (tipo == "FULL")
            {
                //valida nota fiscal
                erros = validaNota();
                if (erros.Count > 0)
                {
                    return erros;
                }
            }

            //CRIA CLASSE DE FOLHA DE LANÇAMENTO
            List<SLancamento> arr = new List<SLancamento>();
            if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
            {
                arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            }
            FolhaLancamento folha = new FolhaLancamento(conn, arr, modulo, Convert.ToDateTime(data));

            //VERIFICA SE EXISTE COTAÇÃO NO LOTE
            if (HttpContext.Current.Session["cotacaoLote_" + modulo] != null)
            {
                folha.cotacaoItemLote = (List<CotacaoItem>)HttpContext.Current.Session["cotacaoLote_" + modulo];
            }

            //VERIFICA SE É ALTERAÇÃO OU NOVA FOLHA
            if (lote == 0)
            {
                erros = folha.salvar(null);
            }
            else
            {
                alteracaoFolha = verificaTipoAlteracao(modulo, lote);
                folha.lote = lote;
                if (alteracaoFolha == "FULL")
                {
                    folha.alterarFull();  //aki netooooooo
                }
                else
                {
                    folha.alterarMin();
                }
            }

            if (tipo == "FULL")
            {
                if (erros.Count == 0)
                {
                    NotaFiscalClass notaClass = new NotaFiscalClass(new notaFiscalDAO(conn), new itemNotaFiscalDAO(conn, new notaFiscalDAO(conn)), new empresasDAO(conn));
                    SNotaFiscal nota = SessionView.NotaFiscalSession;
                    if (lote > 0)
                    {
                        notaClass.deletar(numeroNotaOld, nota.entradaSaida, lote, nota.produtoServico, nota.codEmpresa);
                    }
                    
                    nota.lote = folha.lote;

                    foreach (SItemNotaFiscal item in nota.itens)
                    {
                        item.numeroNota = nota.numeroNota;
                        item.entradaSaida = nota.entradaSaida;
                        item.lote = nota.lote;
                        item.produtoServico = nota.produtoServico;
                        item.codEmpresa = nota.codEmpresa;
                    }
                    
                    if (!notaClass.salvar(nota))
                    {
                        erros = notaClass.erros;
                    }
                }
            }

            if (erros.Count == 0)
            {
                HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = folha.lote;
                //Zera sessão cotação lote
                SessionView.NotaFiscalSession = null;
            }
        }
        catch (Exception ex)
        {
            erros.Add(ex.Message);
        }

        return erros;
    }

    [WebMethod]
    public static List<string> validaNota()
    {
        Conexao conn = new Conexao();
        NotaFiscalClass notaClass = new NotaFiscalClass(new notaFiscalDAO(conn), new itemNotaFiscalDAO(conn,new notaFiscalDAO(conn)), new empresasDAO(conn));
        List<string> erros = new List<string>();
        if (!notaClass.valida(SessionView.NotaFiscalSession))
        {
            erros = notaClass.erros;
        }

        return erros;
    }

    [WebMethod]
    public static void criaNotaFiscal(string tipo)
    {
        if (SessionView.NotaFiscalSession == null)
        {
            if (tipo == "P")
            {
                SNotaFiscalProduto nota = new SNotaFiscalProduto();
                nota.produtoServico = tipo;
                SessionView.NotaFiscalSession = nota;
            }
            else
            {
                SNotaFiscalServico nota = new SNotaFiscalServico();
                nota.produtoServico = tipo;
                SessionView.NotaFiscalSession = nota;
            }
        }
        else
        {
            SNotaFiscal notaFiscal = SessionView.NotaFiscalSession;
            if (notaFiscal.produtoServico != tipo)
            {
                if (tipo == "P")
                {
                    SNotaFiscalProduto nota = new SNotaFiscalProduto();
                    nota.produtoServico = tipo;
                    SessionView.NotaFiscalSession = nota;
                }
                else
                {
                    SNotaFiscalServico nota = new SNotaFiscalServico();
                    nota.produtoServico = tipo;
                    SessionView.NotaFiscalSession = nota;
                     
                }
            }
        }
    }

    [WebMethod]
    public static SNotaFiscal getSessionNotaFiscal()
    {
        return SessionView.NotaFiscalSession;
    }

    [WebMethod]
    public static double getSessionLoteGerado(string modulo)
    {
        double lote = 0;
        if(HttpContext.Current.Session["ss_lote_gerado_" + modulo] != null)
            lote = Convert.ToDouble(HttpContext.Current.Session["ss_lote_gerado_" + modulo]);
        return lote;
    }

    [WebMethod]
    public static double getSessionBaixaGerada(string modulo)
    {
        double lote = 0;
        if (HttpContext.Current.Session["ss_baixa_gerada_" + modulo] != null)
            lote = Convert.ToDouble(HttpContext.Current.Session["ss_baixa_gerada_" + modulo]);
        return lote;
    }

    [WebMethod]
    public static SNotaFiscalProduto getSessionNotaFiscalProduto()
    {
        return (SNotaFiscalProduto)SessionView.NotaFiscalSession;
    }

    [WebMethod]
    public static SNotaFiscalServico getSessionNotaFiscalServico()
    {
        return (SNotaFiscalServico)SessionView.NotaFiscalSession;
    }

    [WebMethod]
    public static void deletaItem(int ordem)
    {
        SNotaFiscal nota = SessionView.NotaFiscalSession;
        List<SItemNotaFiscal> itens = nota.itens;
        foreach (SItemNotaFiscal item in itens)
        {
            if (item.ordem == ordem)
            {
                itens.Remove(item);
                break;
            }
        }

        for (int i = 0; i < itens.Count; i++)
        {
            itens[i].ordem = i + 1;
        }

        nota.itens = itens;
        SessionView.NotaFiscalSession = nota;
    }

    [WebMethod]
    public static SItemNotaFiscalProduto getItemProduto(int ordem)
    {
        SNotaFiscal nota = SessionView.NotaFiscalSession;
        List<SItemNotaFiscal> itens = nota.itens;
        SItemNotaFiscalProduto produto = null;
        foreach (SItemNotaFiscal item in itens)
        {
            if (item.ordem == ordem)
            {
                produto = (SItemNotaFiscalProduto)item;
                break;
            }
        }

        return produto;
    }

    [WebMethod]
    public static SItemNotaFiscalServico getItemServico(int ordem)
    {
        SNotaFiscal nota = SessionView.NotaFiscalSession;
        List<SItemNotaFiscal> itens = nota.itens;
        SItemNotaFiscalServico produto = null;
        foreach (SItemNotaFiscal item in itens)
        {
            if (item.ordem == ordem)
            {
                produto = (SItemNotaFiscalServico)item;
                break;
            }
        }

        return produto;
    }

    [WebMethod]
    public static void salvaItem(int ordem, string cfop, int produto, string descProduto, double qtde, double valorTotal, double baseImp, double icms, double ipi, double pis, double cofins)
    {
        SNotaFiscal nota = SessionView.NotaFiscalSession;
        List<SItemNotaFiscal> itens = nota.itens;
        SItemNotaFiscal atual = null;
        foreach (SItemNotaFiscal item in itens)
        {
            if (item.ordem == ordem)
            {
                atual = item;
                break ;
            }
        }
        if (atual != null)
        {
            atual.numeroNota = nota.numeroNota;
            atual.entradaSaida = nota.entradaSaida;
            atual.lote = nota.lote;
            atual.produtoServico = nota.produtoServico;
            atual.codEmpresa = nota.codEmpresa;

            if (nota.produtoServico == "P")
            {
                SItemNotaFiscalProduto prod = (SItemNotaFiscalProduto)atual;
                prod.ordem = ordem;
                prod.cfop = cfop;
                prod.codigoProduto = produto;
                prod.descProduto = descProduto;
                prod.qtde = qtde;
                prod.valorTotal = valorTotal;
                prod.valorBaseImp = baseImp;
                prod.aliquotaIcms = icms;
                prod.aliquotaIpi = ipi;
                prod.aliquotaPis = pis;
                prod.aliquotaCofins = cofins;

                for (int i = 0; i < itens.Count; i++)
                {
                    if (itens[i].ordem == ordem)
                    {
                        itens[i] = prod;
                        break;
                    }
                }
            }
            else
            {
                SItemNotaFiscalServico serv = (SItemNotaFiscalServico)atual;
                serv.ordem = ordem;
                serv.codigoProduto = produto;
                serv.descProduto = descProduto;
                serv.qtde = qtde;
                serv.valorTotal = valorTotal;
                serv.valorBaseImp = baseImp;
                serv.aliquotaIcms = icms;
                serv.aliquotaPis = pis;
                serv.aliquotaCofins = cofins;

                for (int i = 0; i < itens.Count; i++)
                {
                    if (itens[i].ordem == ordem)
                    {
                        itens[i] = serv;
                        break;
                    }
                }
            }
            nota.itens = itens;
        }
        else
        {

            if (nota.produtoServico == "P")
            {
                SItemNotaFiscalProduto itemProd = new SItemNotaFiscalProduto();
                itemProd.numeroNota = nota.numeroNota;
                itemProd.entradaSaida = nota.entradaSaida;
                itemProd.lote = nota.lote;
                itemProd.produtoServico = nota.produtoServico;
                itemProd.codEmpresa = nota.codEmpresa;
                itemProd.ordem = ordem;
                itemProd.cfop = cfop;
                itemProd.codigoProduto = produto;
                itemProd.descProduto = descProduto;
                itemProd.qtde = qtde;
                itemProd.valorTotal = valorTotal;
                itemProd.valorBaseImp = baseImp;
                itemProd.aliquotaIcms = icms;
                itemProd.aliquotaIpi = ipi;
                itemProd.aliquotaPis = pis;
                itemProd.aliquotaCofins = cofins;
                nota.itens.Add(itemProd);
            }
            else
            {
                SItemNotaFiscalServico itemServ = new SItemNotaFiscalServico();
                itemServ.numeroNota = nota.numeroNota;
                itemServ.entradaSaida = nota.entradaSaida;
                itemServ.lote = nota.lote;
                itemServ.produtoServico = nota.produtoServico;
                itemServ.codEmpresa = nota.codEmpresa;
                itemServ.ordem = ordem;
                itemServ.codigoProduto = produto;
                itemServ.descProduto = descProduto;
                itemServ.qtde = qtde;
                itemServ.valorTotal = valorTotal;
                itemServ.valorBaseImp = baseImp;
                itemServ.aliquotaIcms = icms;
                itemServ.aliquotaPis = pis;
                itemServ.aliquotaCofins = cofins;
                nota.itens.Add(itemServ);
            }
        }
        SessionView.NotaFiscalSession = nota;
    }

    [WebMethod]
    public static void salvaNota(string produtoServico, double numeroNota, string entradaSaida,double valor,double icms,double desconto,double baseImposto,
        double frete,double ipi,double csll,double ir,double iss,double pisRetido,double cofinsRetido,double csllRetido,double irRetido,double issRetido, string numeroEletronica)
    {
        SNotaFiscal nota = SessionView.NotaFiscalSession;
        nota.numeroNota = numeroNota;
        nota.entradaSaida = entradaSaida;
        nota.valor = valor;
        nota.icms = icms;
        nota.descontos = desconto;
        nota.baseImpostos = baseImposto;
        nota.codEmpresa = SessionView.EmpresaSession;
        nota.numeroEletronica = numeroEletronica;

        foreach (SItemNotaFiscal item in nota.itens)
        {
            item.numeroNota = numeroNota;
            item.entradaSaida = entradaSaida;
        }
        
        if (produtoServico == "P")
        {
            SNotaFiscalProduto produto = (SNotaFiscalProduto)nota;
            produto.frete = frete;
            produto.ipi = ipi;
            SessionView.NotaFiscalSession = produto;
        }
        else
        {
            SNotaFiscalServico servico = (SNotaFiscalServico)nota;
            servico.valorCsll = csll;
            servico.valorIR = ir;
            servico.valorIss = iss;
            servico.valorPisRetido = pisRetido;
            servico.valorCofinsRetido = cofinsRetido;
            servico.valorCsllRetido = csllRetido;
            servico.valorIrRetido = irRetido;
            servico.valorIssRetido = issRetido;
            SessionView.NotaFiscalSession = servico;
        }
    }

    #region Lançamentos
    [WebMethod]
    public static List<Hashtable> carregaTerceirosDisponiveis(string modulo)
    {
        List<Hashtable> terceiros = new List<Hashtable>();
        Conexao c = new Conexao();
        switch (modulo)
        {
            case "CAP_INCLUSAO_TITULO":
                Fornecedor fornecedor = new Fornecedor(c);
                terceiros = fornecedor.lista();


                break;
            case "CAR_INCLUSAO_TITULO":
                Cliente cliente = new Cliente(c);
                terceiros = cliente.lista();

                break;
            case "C_INCLUSAO_LANCTO":
                Empresa empresa = new Empresa(c);
                terceiros = empresa.listaFornecedoresClientes();

                break;
        }

        return terceiros;
    }

    [WebMethod]
    public static List<SLancamento> carregaUltimoLote(string modulo, double codigoTerceiro, string data)
    {
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);

        List<SLancamento> lanctos = folha.carregaLote(folha.getLoteAnterior(modulo, codigoTerceiro));
        for (int i = 0; i < lanctos.Count; i++)
        {
            lanctos[i].dataLancamento = Convert.ToDateTime(data);
            if (lanctos[i].vencimentos != null)
            {
                for (int x = 0; x < lanctos[i].vencimentos.Count; x++)
                {
                    lanctos[i].vencimentos[x].data = Convert.ToDateTime(data);
                }
            }
        }

        HttpContext.Current.Session["ss_lancamentos_" + modulo] = lanctos;
        return getSessionLanctos(modulo);
    }

    [WebMethod]
    public static int totalLotesAnteriores(string modulo, double codigoTerceiro)
    {
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);
        return folha.totalLotesAnteriores(modulo, codigoTerceiro);
    }

    [WebMethod]
    public static List<string> salvaFolha(string modulo, string data)
    {
        Conexao c = new Conexao();
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }
        FolhaLancamento folha = new FolhaLancamento(c, arr, modulo, Convert.ToDateTime(data));
        
        List<string> erros =  folha.salvar(null);

        if (erros.Count == 0)
        {
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;
            HttpContext.Current.Session["ss_lote_gerado_" + modulo] = folha.lote;
        }
        return erros;
    }

    [WebMethod]
    public static List<string> validaFolha(string modulo, string data)
    {
        Conexao c = new Conexao();
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }
        FolhaLancamento folha = new FolhaLancamento(c, arr, modulo, Convert.ToDateTime(data));

        List<string> erros = folha.validaFolha();
        return erros;
    }

    [WebMethod]
    public static List<string> baixaTitulo(string modulo, string data, List<double> arrSelecionados)
    {
        Conexao c = new Conexao();
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }
        FolhaLancamento folha = new FolhaLancamento(c, arr, modulo, Convert.ToDateTime(data));

        List<string> erros =  folha.baixaTitulo(arrSelecionados);

        if (erros.Count == 0)
        {
            //HttpContext.Current.Response.Clear();
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;
            HttpContext.Current.Session["ss_baixa_gerada_" + modulo] = folha.baixa;
        }

        return erros;
    }

    [WebMethod]
    public static string verificaTipoAlteracao(string modulo, double lote)
    {
        string tipo = "FULL";
        int erros = 0;
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);
        List<SLancamento> arr = new List<SLancamento>();
        //DateTime dt = Convert.ToDateTime(data);
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];

		}
        List<SLancamento> banco = folha.carregaLote(lote);

		if (banco.Count != arr.Count) return tipo;

		foreach (SLancamento arrLancto in arr)
		{
			SLancamento bancoLancto = banco.Where(o => o.seqLote == arrLancto.seqLote).FirstOrDefault();
			if (bancoLancto == null) return tipo;

			if ((bancoLancto.dataLancamento != arrLancto.dataLancamento)
						|| (bancoLancto.titulo != arrLancto.titulo)
						|| (bancoLancto.debCred != arrLancto.debCred)
						|| (bancoLancto.terceiro != arrLancto.terceiro)
						|| (bancoLancto.valor != arrLancto.valor))
			{
				return tipo;
			}

		}

		//verifica datas
		//for (int i = 0; i < banco.Count; i++)
		//{
		//	for (int x = 0; x < arr.Count; x++)
		//	{
		//		if (banco[i].seqLote == arr[x].seqLote)
		//		{
		//			if ((banco[i].dataLancamento != arr[x].dataLancamento)
		//				|| (banco[i].titulo != arr[x].titulo)
		//				|| (banco[i].debCred != arr[x].debCred)
		//				|| (banco[i].terceiro != arr[x].terceiro)
		//				|| (banco[i].valor != arr[x].valor))
		//			{
		//				erros++;
		//				break;
		//			}
		//		}
		//	}

		//	if (erros > 0)
		//	{
		//		break;
		//	}
		//}

		//if (banco.Count != arr.Count)
  //      {
  //          erros++;
  //      }

        if (erros == 0)
        {
            int totalVencimentosBanco = 0;
            int totalVencimentoAtual = 0;
            for (int i = 0; i < banco.Count; i++)
            {
                totalVencimentosBanco += banco[i].vencimentos.Count;
            }

            for (int i = 0; i < arr.Count; i++)
            {
                totalVencimentoAtual += arr[i].vencimentos.Count;
            }

            if(totalVencimentosBanco != totalVencimentoAtual)
            {
                erros++;
            }

            for (int i = 0; i < banco.Count; i++)
            {
                if (banco[i].vencimentos != null)
                {
                    if (banco[i].vencimentos.Count > 0)
                    {
                        for (int t = 0; t < banco[i].vencimentos.Count; t++)
                        {
                            for (int x = 0; x < arr.Count; x++)
                            {
                                if (arr[x].seqLote == banco[i].seqLote)
                                {
                                    if (arr[x].vencimentos != null)
                                    {
                                        if (arr[x].vencimentos.Count > 0)
                                        {
                                            if (arr[x].vencimentos.Count == banco[i].vencimentos.Count)
                                            {
                                                bool existe = false;
                                                for (int z = 0; z < arr[x].vencimentos.Count; z++)
                                                {
                                                    if (arr[x].vencimentos[z].data == banco[i].vencimentos[t].data)
                                                    {
                                                        existe = true;
                                                        break;
                                                    }
                                                }

                                                if (!existe)
                                                {
                                                    erros++;
                                                }
                                            }
                                            else
                                            {
                                                erros++;
                                            }
                                        }
                                        else
                                        {
                                            erros++;
                                        }
                                    }
                                    else
                                    {
                                        erros++;
                                    }
                                }
                            }

                            if (erros > 0)
                            {
                                break;
                            }
                        }
                        if (erros > 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        if (erros == 0)
        {
            //verifica valores
            decimal totalDebBanco = 0;
            decimal totalCredBanco = 0;

            decimal totalDeb = 0;
            decimal totalCred = 0;

            for (int i = 0; i < banco.Count; i++)
            {
                if (banco[i].debCred.Value == 'D')
                {
                    totalDebBanco += banco[i].valor.Value;
                }
                else
                {
                    totalCredBanco += banco[i].valor.Value;
                }
            }

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].debCred.Value == 'D')
                {
                    totalDeb += arr[i].valor.Value;
                }
                else
                {
                    totalCred += arr[i].valor.Value;
                }
            }

            if (totalDebBanco != totalDeb)
            {
                erros++;
            }
            else
            {
                if (totalCredBanco != totalCred)
                {
                    erros++;
                }
            }

            if (erros == 0)
            {
                for (int i = 0; i < banco.Count; i++)
                {
                    if (banco[i].vencimentos != null)
                    {
                        if (banco[i].vencimentos.Count > 0)
                        {
                            for (int t = 0; t < banco[i].vencimentos.Count; t++)
                            {
                                for (int x = 0; x < arr.Count; x++)
                                {
                                    if (arr[x].seqLote == banco[i].seqLote)
                                    {
                                        if (arr[x].vencimentos != null)
                                        {
                                            if (arr[x].vencimentos.Count > 0)
                                            {
                                                if (arr[x].vencimentos.Count == banco[i].vencimentos.Count)
                                                {
                                                    bool existe = false;
                                                    for (int z = 0; z < arr[x].vencimentos.Count; z++)
                                                    {
                                                        if (arr[x].vencimentos[z].valor == banco[i].vencimentos[t].valor)
                                                        {
                                                            existe = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!existe)
                                                    {
                                                        erros++;
                                                    }
                                                }
                                                else
                                                {
                                                    erros++;
                                                }
                                            }
                                            else
                                            {
                                                erros++;
                                            }
                                        }
                                        else
                                        {
                                            erros++;
                                        }
                                    }
                                }
                                if (erros > 0)
                                {
                                    break;
                                }
                            }
                            if (erros > 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (erros == 0)
        {
            tipo = "MIN";
        }
        else
        {
            if (!folha.PodeDeletarBaixa(lote, (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo]))
            {
                tipo = "MIN";
            }
        }

        return tipo;
    }

    [WebMethod]
    public static List<string> alteraFolha(string modulo, string data, double lote, string tipo)
    {
        Conexao c = new Conexao();
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }
        FolhaLancamento folha = new FolhaLancamento(c, arr, modulo, Convert.ToDateTime(data));
        folha.lote = lote;

		//if (arr.Where(x => x.dataLancamento != Convert.ToDateTime(data)).Count() != 0)
		//{
		//	tipo = "FULL";
		//}

		List<string> erros = new List<string>();
        if (tipo == "FULL")
        {
            erros = folha.alterarFull();
        }
        else
        {
            erros = folha.alterarMin();
        }

        if (erros.Count == 0)
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;

        return erros;
    }

    [WebMethod]
    public static bool verificaFilhosLote(string modulo, double lote)
    {
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);
        folha.modulo = modulo;

        return folha.verificaFilhos(lote);
    }

    [WebMethod]
    public static bool verificaFechamentoLote(string modulo, double lote)
    {
        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);
        folha.modulo = modulo;
        return folha.verificaPeriodoFechamento(lote);
    }

    [WebMethod]
    public static List<string> salvaLanctosModelo(string modulo, int codigo)
    {
        Conexao c = new Conexao();
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }
        Modelo modelo = new Modelo(c);
        modelo.codigo = codigo;
        modelo.arrLancamentos = arr;

        List<string> erros = modelo.salvaLancamentos();

        if (erros.Count == 0)
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;

        return erros;
    }

    [WebMethod]
    public static List<string> alteraConsultorLancto1(string modulo, int codConsultor)
    {
        List<string> erros = new List<string>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;
            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == 1)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual != null)
                {
                    atual.codConsultor = codConsultor;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    erros.Add("Lançamento não foi encontrado.");
                }
            }
            else
            {
                erros.Add("Lista de lançamentos vazia.");
            }
        }
        else
        {
            erros.Add("Lista de lançamentos vazia.");
        }

        return erros;
    }

    [WebMethod]
    public static List<string> alteraValorLancto1(string modulo, decimal valor)
    {
        List<string> erros = new List<string>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;
            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == 1)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual != null)
                {
                    atual.valorUnit = valor;
                    atual.valor = atual.qtd * valor;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    erros.Add("Lançamento não foi encontrado.");
                }
            }
            else
            {
                erros.Add("Lista de lançamentos vazia.");
            }
        }
        else
        {
            erros.Add("Lista de lançamentos vazia.");
        }
        
        return erros;
    }

    [WebMethod]
    public static List<string> alteraQtdLancto1(string modulo, decimal qtd)
    {
        List<string> erros = new List<string>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;
            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == 1)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual != null)
                {
                    atual.qtd = qtd;
                    atual.valor = atual.valorUnit * qtd;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    erros.Add("Lançamento não foi encontrado.");
                }
            }
            else
            {
                erros.Add("Lista de lançamentos vazia.");
            }
        }
        else
        {
            erros.Add("Lista de lançamentos vazia.");
        }

        return erros;
    }

    [WebMethod]
    public static List<string> alteraNumeroDocumentoLancto1(string modulo, string numeroDocumento)
    {
        List<string> erros = new List<string>();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;
            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == 1)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual != null)
                {
                    atual.numeroDocumento = numeroDocumento;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    erros.Add("Lançamento não foi encontrado.");
                }
            }
            else
            {
                erros.Add("Lista de lançamentos vazia.");
            }
        }
        else
        {
            erros.Add("Lista de lançamentos vazia.");
        }

        return erros;
    }

    [WebMethod]
    public static Empresa loadTerceiro(int terceiro)
    {
        Conexao c = new Conexao();
        Empresa empresa = new Empresa(c);
        if (terceiro > 0)
        {
            empresa.codigo = terceiro;
            empresa.load();
        }

        return empresa;
    }

    [WebMethod]
    public static List<SLancamento> loadLanctosModelo(string modulo, int cod_modelo, int terceiroEscolhido, string descTerceiroEscolhido, string data, decimal valorbruto, int codmoeda)
    {
        Conexao c = new Conexao();
        Modelo modelo = new Modelo(c);

        if (cod_modelo > 0)
        {
            modelo.codigo = cod_modelo;
            modelo.load();

            if (modulo == "MODELO_LANCAMENTO")
            {
                List<SLancamento> arr = new List<SLancamento>();

                for (int i = 0; i < modelo.arrLancamentos.Count; i++)
                {
                    SLancamento temp = modelo.arrLancamentos[i];
                    if (!(terceiroEscolhido == 0 && (descTerceiroEscolhido == "" || descTerceiroEscolhido == null)))
                    {
                        temp.terceiro = terceiroEscolhido;
                        temp.descTerceiro = descTerceiroEscolhido;
                    }
                    arr.Add(temp);
                }

                HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
            }
            else
            {
                List<SLancamento> arr = new List<SLancamento>();

                for (int i = 0; i < modelo.arrLancamentos.Count; i++)
                {
                    SLancamento temp = modelo.arrLancamentos[i];
                    temp.dataLancamento = Convert.ToDateTime(data);
                    temp.valorBruto = valorbruto;
                    temp.codMoeda = codmoeda;
                    bool atualizaTerceiro = false;

                    //O TERCEIRO NÃO FOI ESCOLHIDO?
                    if (temp.terceiro != null)
                    {
                        if (temp.terceiro == 0)
                        {
                            atualizaTerceiro = true;
                        }
                    }
                    else
                    {
                        atualizaTerceiro = true;
                    }

                    if (atualizaTerceiro)
                    {

                        if (!(terceiroEscolhido == 0 && (descTerceiroEscolhido == "" || descTerceiroEscolhido == null)))
                        {
                            if((bool)temp.titulo){
                                temp.terceiro = terceiroEscolhido;
                                temp.descTerceiro = descTerceiroEscolhido;
                            }
                        }
                    }
                    else
                    {
                        bool faz = true;
                        if (modulo == "CAP_INCLUSAO_TITULO" || modulo == "CAR_INCLUSAO_TITULO")
                        {
                            if (temp.seqLote > 1)
                            {
                                ContaContabil conta = new ContaContabil(c);
                                conta.codigo = temp.conta;
                                conta.load();

                                if (conta.tipo == "CP" || conta.tipo == "CR")
                                    faz = true;
                                else
                                    faz = false;
                            }
                            else
                            {
                                faz = true;
                            }
                        }
                        else
                        {
                            faz = false;
                        }

                        if (faz)
                        {
                            if (temp.terceiro == 0){

                                if (!(terceiroEscolhido == 0 && (descTerceiroEscolhido == "" || descTerceiroEscolhido == null)))
                                {
                                    temp.terceiro = terceiroEscolhido;
                                    temp.descTerceiro = descTerceiroEscolhido;
                                }

                            }
                        }
                    }
                    arr.Add(temp);
                }

                HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
            }
        }
        else
        {
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = null;
        }

        return getSessionLanctos(modulo);
    }

    [WebMethod]
    public static string alteraValorBruto(string modulo, decimal valorbruto) 
    {
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];

            for (int i = 0; i < arr.Count; i++)
            {
                arr[i].valorBruto = valorbruto;
            }
        }
        else 
        {
            return "Nenhum lançamento encontrado!";
        }
        return "Valor Bruto alterado!";
    }

    [WebMethod]
    public static string alteraEtapaMoeda(string modulo, int codmoeda)
    {
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];

            for (int i = 0; i < arr.Count; i++)
            {
                arr[i].codMoeda = codmoeda;
            }
        }
        else
        {
            return "Nenhum lançamento encontrado!";
        }
        return "Moeda alterada!";
    }

    [WebMethod]
    public static List<SLancamento> getSessionLanctos(string modulo)
    {
        List<SLancamento> arr = new List<SLancamento>();
        if (HttpContext.Current.Session["ss_lancamentos_"+modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        }

        return arr;
    }

    [WebMethod]
    public static SLancamento pegaLancto(string modulo, int seqLote)
    {
        SLancamento atual = null;

        if (HttpContext.Current.Session["ss_lancamentos_"+modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_"+modulo];

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].seqLote == seqLote)
                {
                    atual = arr[i];
                    break;
                }
            }
        }

        return atual;
    }

    [WebMethod]
    public static string validaDeletaLancto(string modulo, int seqLote) 
    {
        string grupoNaoPermitido = "";

        if (modulo != "MODELO_LANCAMENTO")
        {
            if ((bool)HttpContext.Current.Session["exige_modelo"])
            {
                if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
                {
                    List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
                    if (arr != null)
                    {
                        for (int i = 0; i < arr.Count; i++)
                        {
                            if (arr[i].seqLote == seqLote)
                            {
                                if (arr[i].valorGrupo == 0) break;

                                int res = (from x in arr where x.valorGrupo == arr[i].valorGrupo select x).Count();

                                if (res == 1)
                                    grupoNaoPermitido = "Você deve ter ao menos 1 lançamento do grupo " + arr[i].valorGrupo + ".";

                                break;
                            }
                        }
                    }
                }
            }
        }
        return grupoNaoPermitido;
    }

    [WebMethod]
    public static List<SLancamento> deletaLancto(string modulo, int seqLote)
    {
        List<SLancamento> arr = null;
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == seqLote)
                    {
                        arr.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        return arr;
    }

    [WebMethod]
    public static List<SLancamento> salvaLancto(string modulo, int seqLote, string data, char debCred, string conta, string descConta, int job, string descJob,
        decimal qtd, decimal valorUnit, decimal valor, int linhaNegocio, string descLinhaNegocio, int divisao, string descDivisao, int cliente, string descCliente, 
        bool geraTitulo, int parcelas,int terceiro, string descTerceiro, string historico, int modelo, string numeroDocumento, string descConsultor, int codConsultor, decimal valorBruto, int valorGrupo, int codMoeda)
    {
        Conexao conn = new Conexao();
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;

            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == seqLote)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual == null)
                {
                    List<SVencimento> vencimentos = new List<SVencimento>();
                    if (geraTitulo)
                    {
                        decimal valores = valor / parcelas;
                        for (int i = 0; i < parcelas; i++)
                        {
                            if (i == 0)
                            {
                                vencimentos.Add(new SVencimento(DateTime.Now, valores));
                            }
                            else
                            {
                                vencimentos.Add(new SVencimento(DateTime.Now.AddMonths(i), valores));
                            }
                        }
                    }

                    atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred, Convert.ToDateTime(data), conta, descConta,
                        job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                        descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, valorGrupo, codMoeda);

                    arr.Add(atual);
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    atual.debCred = debCred;
                    atual.conta = conta;
                    atual.descConta = descConta;
                    atual.dataLancamento = Convert.ToDateTime(data);
                    atual.job = job;
                    atual.descJob = descJob;
                    atual.linhaNegocio = linhaNegocio;
                    atual.descLinhaNegocio = descLinhaNegocio;
                    atual.divisao = divisao;
                    atual.descDivisao = descDivisao;
                    atual.cliente = cliente;
                    atual.descCliente = descCliente;
                    atual.qtd = qtd;
                    atual.valorUnit = valorUnit;
                    atual.valor = valor;
                    atual.titulo = geraTitulo;
                    atual.terceiro = terceiro;
                    atual.descTerceiro = descTerceiro;
                    atual.historico = historico;
                    atual.modelo = modelo;
                    atual.numeroDocumento = numeroDocumento;
                    atual.descConsultor = descConsultor;
                    atual.codConsultor = codConsultor;
                    atual.valorBruto = valorBruto;
                    atual.codMoeda = codMoeda;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
            }
            else
            {
                arr = new List<SLancamento>();
                List<SVencimento> vencimentos = new List<SVencimento>();
                if (geraTitulo)
                {
                    decimal valores = valor / parcelas;
                    for (int i = 0; i < parcelas; i++)
                    {
                        if (i == 0)
                        {
                            vencimentos.Add(new SVencimento(DateTime.Now, valores));
                        }
                        else
                        {
                            vencimentos.Add(new SVencimento(DateTime.Now.AddMonths(i), valores));
                        }
                    }
                }

                atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred, Convert.ToDateTime(data), conta, descConta,
                    job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                    descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, 0, codMoeda);

                arr.Add(atual);
                HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
            }

        }
        else
        {
            List<SLancamento> arr = new List<SLancamento>();
            List<SVencimento> vencimentos = new List<SVencimento>();
            if (geraTitulo)
            {
                decimal valores = valor / parcelas;
                for (int i = 0; i < parcelas; i++)
                {
                    if (i == 0)
                    {
                        vencimentos.Add(new SVencimento(DateTime.Now, valores));
                    }
                    else
                    {
                        vencimentos.Add(new SVencimento(DateTime.Now.AddMonths(i), valores));
                    }
                }
            }

            SLancamento atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred, Convert.ToDateTime(data), conta, descConta,
                job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, 0, codMoeda);

            arr.Add(atual);
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
        }

        return getSessionLanctos(modulo);
    }

    [WebMethod]
    public static List<SLancamento> salvaLanctoMODELO_LANCAMENTO(string modulo, int seqLote, char debCred, string conta, string descConta, int job, string descJob,
        decimal qtd, decimal valorUnit, decimal valor, int linhaNegocio, string descLinhaNegocio, int divisao, string descDivisao, int cliente, string descCliente,
        bool geraTitulo, int parcelas, int terceiro, string descTerceiro, string historico, int modelo, string numeroDocumento, string descConsultor, int codConsultor, decimal valorBruto, int valorGrupo)
    {
        Conexao conn = new Conexao();

        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;
            int indice = 0;

            if (arr != null)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == seqLote)
                    {
                        atual = arr[i];
                        indice = i;
                        break;
                    }
                }

                if (atual == null)
                {
                    List<SVencimento> vencimentos = new List<SVencimento>();
                    if (geraTitulo)
                    {
                        for (int i = 0; i < parcelas; i++)
                        {
                            if (i == 0)
                            {
                                vencimentos.Add(new SVencimento(null, null));
                            }
                            else
                            {
                                vencimentos.Add(new SVencimento(null, null));
                            }
                        }
                    }

                    atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred, null, conta, descConta,
                        job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                        descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, valorGrupo, 0);

                    arr.Add(atual);
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    atual.debCred = debCred;
                    atual.conta = conta;
                    atual.descConta = descConta;
                    atual.dataLancamento = null;
                    atual.job = job;
                    atual.descJob = descJob;
                    atual.linhaNegocio = linhaNegocio;
                    atual.descLinhaNegocio = descLinhaNegocio;
                    atual.divisao = divisao;
                    atual.descDivisao = descDivisao;
                    atual.cliente = cliente;
                    atual.descCliente = descCliente;
                    atual.qtd = qtd;
                    atual.valorUnit = valorUnit;
                    atual.valor = valor;
                    atual.titulo = geraTitulo;
                    atual.terceiro = terceiro;
                    atual.descTerceiro = descTerceiro;
                    atual.historico = historico;
                    atual.modelo = modelo;
                    atual.numeroDocumento = numeroDocumento;
                    atual.descConsultor = descConsultor;
                    atual.codConsultor = codConsultor;
                    atual.valorGrupo = valorGrupo;

                    if (atual.vencimentos.Count != parcelas)
                    {
                        atual.vencimentos.Clear();

                        for (int i = 0; i < parcelas; i++)
                        {
                            if (i == 0)
                            {
                                atual.vencimentos.Add(new SVencimento(null, null));
                            }
                            else
                            {
                                atual.vencimentos.Add(new SVencimento(null, null));
                            }
                        }
                    }

                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
            }
            else
            {
                arr = new List<SLancamento>();
                List<SVencimento> vencimentos = new List<SVencimento>();
                if (geraTitulo)
                {
                    for (int i = 0; i < parcelas; i++)
                    {
                        if (i == 0)
                        {
                            vencimentos.Add(new SVencimento(null,null));
                        }
                        else
                        {
                            vencimentos.Add(new SVencimento(null,null));
                        }
                    }
                }

                atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred,null, conta, descConta,
                    job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                    descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, 0, 0);

                arr.Add(atual);
                HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
            }

        }
        else
        {
            List<SLancamento> arr = new List<SLancamento>();
            List<SVencimento> vencimentos = new List<SVencimento>();
            if (geraTitulo)
            {
                for (int i = 0; i < parcelas; i++)
                {
                    if (i == 0)
                    {
                        vencimentos.Add(new SVencimento(null,null));
                    }
                    else
                    {
                        vencimentos.Add(new SVencimento(null,null));
                    }
                }
            }

            SLancamento atual = new SLancamento(null, seqLote, null, null, null, null, null, null, debCred, null, conta, descConta,
                job, descJob, linhaNegocio, descLinhaNegocio, divisao, descDivisao, cliente,
                descCliente, qtd, valorUnit, valor, historico, geraTitulo, vencimentos, terceiro, descTerceiro, true, modelo, numeroDocumento, descConsultor, codConsultor, valorBruto, 0, 0);

            arr.Add(atual);
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
        }

        return getSessionLanctos(modulo);
    }

    [WebMethod]
    public static List<Hashtable> getListaModelos(string modulo)
    {
        Conexao conn = new Conexao();
        Modelo x = new Modelo(conn);
        List<Hashtable> retorno = new List<Hashtable>();

        if (modulo == "CAP_INCLUSAO_TITULO")
        {

            retorno = x.listaTipoCP();
        }
        else
        {
            retorno = x.listaTipoCR();
        }
        return retorno;
    }

    [WebMethod]
    public static List<Hashtable> getListaContas(string tipo)
    {
        Conexao conn = new Conexao();
        ContaContabil x = new ContaContabil(conn);
        return x.lista(tipo);
    }

    [WebMethod]
    public static List<Hashtable> getListaContasDiferenteDe(string tipo)
    {
        Conexao conn = new Conexao();
        ContaContabil x = new ContaContabil(conn);
        return x.listaDiferenteDe(tipo);
    }

    [WebMethod]
    public static ContaContabil getContaContabil(string codigo)
    {
        Conexao conn = new Conexao();
        ContaContabil x = new ContaContabil(conn, codigo);
        x.load();
        return x;
    }

    [WebMethod]
    public static List<Hashtable> getListaLinhasNegocio()
    {
        Conexao conn = new Conexao();
        LinhaNegocio x = new LinhaNegocio(conn);
        return x.lista();
    }

    [WebMethod]
    public static List<Hashtable> getListaDivisoes()
    {
        Conexao conn = new Conexao();
        Divisao x = new Divisao(conn);
        return x.lista();
    }

    [WebMethod]
    public static List<Hashtable> getListaClientes(bool travaJob, int codigoJob)
    {
        Conexao conn = new Conexao();
        Cliente x = new Cliente(conn);
        //if(travaJob)
        //    return x.listaClientesJob(codigoJob);
        //else
            return x.lista();
    }

    [WebMethod]
    public static List<Hashtable> getListaJobs(bool status)
    {
        Conexao conn = new Conexao();
        Job x = new Job(conn);

        if (status)
        {
            return x.lista('A');
        }
        else
        {
            return x.lista();
        }
    }

    [WebMethod]
    public static ArrayList loadJob(int job)
    {
        Conexao conn = new Conexao();
        Job j = new Job(conn);
        j.codigo = job;
        j.load();

        ArrayList dados = new ArrayList();
        dados.Add(j.codigo);
        dados.Add(j.linhaNegocio);
        dados.Add(j.linhaNegocioDesc);
        dados.Add(j.divisao);
        dados.Add(j.divisaoDesc);
        dados.Add(j.cliente);
        dados.Add(j.clienteDesc);
		dados.Add(j.status);
		dados.Add(j.nome);

        return dados;
    }

    [WebMethod]
    public static int getTotalBaixas(double? lote, int seqLote, string modulo)
    {
        if (lote != null)
        {
            Conexao conn = new Conexao();
            FolhaLancamento folha = new FolhaLancamento(conn);
            DataTable baixas = folha.getBaixas(lote.Value, seqLote);

            return baixas.Rows.Count;
        }
        else
        {
            return 0;
        }
    }

    [WebMethod]
    public static string getBaixas(double lote, int seqLote, string modulo)
    {
        Conexao conn = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(conn);
        DataTable baixas = folha.getBaixas(lote, seqLote);
        List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
        SLancamento atual = null;

        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].seqLote == seqLote)
            {
                atual = arr[i];
                break;
            }
        }

        string html = "";
        if (baixas.Rows.Count > 0)
        {
            html = "<table id='tabelaGrid' cellspacing=\"0\" celpadding=\"0\" style=\"width:350px;\">";
            html += "<tr class=\"titulo\">";
            html += "<td>Data Prevista</td>";
            html += "<td>Valor</td>";
            html += "<td>Baixa</td>";
            html += "<td>Data Efetiva</td>";
            html += "</tr>";
            for (int i = 0; i < baixas.Rows.Count; i++)
            {
                DataRow row = baixas.Rows[i];
                if (Convert.ToInt32(row["det_baixa"]) < 0)
                {
                    html += "<tr>";
                    html += "<td>" + Convert.ToDateTime(row["data_anterior"]).ToString("dd/MM/yyyy") + "</td>";
                    html += "<td>" + Convert.ToDecimal(row["valor"]).ToString() + "</td>";
                    html += "<td><a href=\"FormDetalheBaixa.aspx?id=" + row["seq_baixa"].ToString() + "&pop=1\" target=\"_blank\">" + row["seq_baixa"].ToString() + "</a></td>";
                    html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>";
                }
            }
            html += "</table>";
        }

        return html;
    }

    [WebMethod]
    public static string loadVencimentos(string modulo,int origem, int seqLote, int parcelas, decimal valorTotal)
    {
        string html = "";

        if (origem == 1)
        {
            html = "<input type=\"hidden\" id=\"H_SEQLOTE_VENCIMENTO\" value=\"" + seqLote + "\" />";
        }
        else
        {
            html = "<input type=\"hidden\" id=\"H_SEQLOTE_VENCIMENTO\" value=\"-1\" />";
        }

        if (HttpContext.Current.Session["ss_lancamentos_"+modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            SLancamento atual = null;

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].seqLote == seqLote)
                {
                    atual = arr[i];
                    break;
                }
            }


            if (atual != null)
            {
                if (atual.vencimentos.Count > 0)
                {
                    for (int i = 0; i < atual.vencimentos.Count; i++)
                    {
                        if (atual.vencimentos[i].data == null && atual.vencimentos[i].valor == null)
                        {
                            atual.vencimentos.Clear();
                            break;
                        }
                    }
                }

                if (atual.valor == valorTotal)
                {
                    if (atual.vencimentos.Count == parcelas)
                    {
                        for (int i = 0; i < atual.vencimentos.Count; i++)
                        {
                            html += "<span class=\"linha\">";
                            html += "<label>" + (i + 1) + " - </label><input type=\"text\" id=\"vencimento_" + (i + 1) + "_dta\" value=\"" + atual.vencimentos[i].data.Value.ToString("ddMMyyyy") + "\" />";
                            html += "<input type=\"text\" id=\"vencimento_" + (i + 1) + "_vlr\" value=\"" + String.Format("{0:0.00}",atual.vencimentos[i].valor.Value) + "\" />";
                            html += "</span>";
                        }
                    }
                    else if (atual.vencimentos.Count < parcelas)
                    {
                        int newParcelas = (parcelas - atual.vencimentos.Count) + atual.vencimentos.Count;
                        decimal somaAtual = 0;
                        decimal diferenca = 0;
                        decimal valores = 0;

                        for (int i = 0; i < atual.vencimentos.Count; i++)
                        {
                            somaAtual += atual.vencimentos[i].valor.Value;
                        }

                        diferenca = valorTotal - somaAtual;
                        valores = Math.Round(diferenca / (parcelas - atual.vencimentos.Count),2);

                        for (int i = 0; i < atual.vencimentos.Count; i++)
                        {
                            html += "<span class=\"linha\">";
                            html += "<label>" + (i + 1) + " - </label><input type=\"text\" id=\"vencimento_" + (i + 1) + "_dta\" value=\"" + atual.vencimentos[i].data.Value.ToString("ddMMyyyy") + "\" />";
                            html += "<input type=\"text\" id=\"vencimento_" + (i + 1) + "_vlr\" value=\"" + String.Format("{0:0.00}", atual.vencimentos[i].valor.Value) + "\" />";
                            html += "</span>";
                        }

                        for (int i = (atual.vencimentos.Count + 1); i <= parcelas; i++)
                        {
                            html += "<span class=\"linha\">";
                            html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                            html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + String.Format("{0:0.00}", valores) + "\" />";
                            html += "</span>";
                        }
                    }
                    else if (atual.vencimentos.Count > parcelas)
                    {
                        decimal valores = Math.Round((valorTotal / parcelas),2);
                        decimal tmp = 0;
                        for (int i = 1; i <= parcelas; i++)
                        {
                            tmp = tmp + valores;

                            if (i == parcelas)
                            {
                                decimal acrecimo = 0;
                                if (tmp < valorTotal)
                                {
                                    acrecimo = valorTotal - tmp;
                                }

                                html += "<span class=\"linha\">";
                                html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                                html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + String.Format("{0:0.00}", (valores + acrecimo)) + "\" />";
                                html += "</span>";
                            }
                            else
                            {
                                html += "<span class=\"linha\">";
                                html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                                html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + (String.Format("{0:0.00}", valores)) + "\" />";
                                html += "</span>";
                            }
                        }
                    }
                    else { }
                }
                else
                {
                    decimal valores = Math.Round((valorTotal / parcelas), 2);
                    decimal tmp = 0;

                    for (int i = 0; i < atual.vencimentos.Count; i++)
                    {
                        tmp = tmp + valores;

                        html += "<span class=\"linha\">";
                        html += "<label>" + (i+1) + " - </label><input type=\"text\" id=\"vencimento_" + (i + 1) + "_dta\" value=\"" + atual.vencimentos[i].data.Value.ToString("ddMMyyyy") + "\" />";
                        html += "<input type=\"text\" id=\"vencimento_" + (i + 1) + "_vlr\" value=\"" + String.Format("{0:0.00}", valores) + "\" />";
                        html += "</span>";
                    }

                    for (int i = (atual.vencimentos.Count + 1); i <= parcelas; i++)
                    {
                        tmp = tmp + valores;

                        if (i == parcelas)
                        {
                            decimal acrecimo = 0;
                            if (tmp < valorTotal)
                            {
                                acrecimo = valorTotal - tmp;
                            }

                            html += "<span class=\"linha\">";
                            html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                            html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + String.Format("{0:0.00}", (valores + acrecimo)) + "\" />";
                            html += "</span>";
                        }
                        else
                        {
                            html += "<span class=\"linha\">";
                            html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                            html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + (String.Format("{0:0.00}", valores)) + "\" />";
                            html += "</span>";
                        }
                    }
                }
            }
            else
            {
                decimal valores = Math.Round((valorTotal / parcelas),2);
                decimal tmp = 0;
                for (int i = 1; i <= parcelas; i++)
                {
                    tmp = tmp + valores;

                    if (i == parcelas)
                    {
                        decimal acrecimo = 0;
                        if (tmp < valorTotal)
                        {
                            acrecimo = valorTotal - tmp;
                        }

                        html += "<span class=\"linha\">";
                        html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                        html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + String.Format("{0:0.00}", (valores + acrecimo)) + "\" />";
                        html += "</span>";
                    }
                    else
                    {
                        html += "<span class=\"linha\">";
                        html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                        html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + (String.Format("{0:0.00}", valores)) + "\" />";
                        html += "</span>";
                    }
                }
            }
        }
        else
        {
            decimal valores = Math.Round((valorTotal / parcelas), 2);
            decimal tmp = 0;
            for (int i = 1; i <= parcelas; i++)
            {
                tmp = tmp + valores;

                if (i == parcelas)
                {
                    decimal acrecimo = 0;
                    if (tmp < valorTotal)
                    {
                        acrecimo = valorTotal - tmp;
                    }

                    html += "<span class=\"linha\">";
                    html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                    html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + String.Format("{0:0.00}", (valores + acrecimo)) + "\" />";
                    html += "</span>";
                }
                else
                {
                    html += "<span class=\"linha\">";
                    html += "<label>" + (i) + " - </label><input type=\"text\" id=\"vencimento_" + i + "_dta\" />";
                    html += "<input type=\"text\" id=\"vencimento_" + i + "_vlr\" value=\"" + (String.Format("{0:0.00}", valores)) + "\" />";
                    html += "</span>";
                }
            }
        }

        return html;
    }

    [WebMethod]
    public static void salvaVencimentos(string modulo, int seqLote, decimal qtd, decimal valorUnit, decimal valorTotal, ArrayList datas, ArrayList valores, decimal valorBruto)
    {
        if (HttpContext.Current.Session["ss_lancamentos_" + modulo] != null)
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];
            if (arr != null)
            {
                SLancamento atual = null;
                int indice = 0;
                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == seqLote)
                    {
                        indice = i;
                        atual = arr[i];
                        break;
                    }
                }
                if (atual != null)
                {
                    List<SVencimento> newListaVencimentos = new List<SVencimento>();
                    for (int i = 0; i < datas.Count; i++)
                    {
                        newListaVencimentos.Add(new SVencimento(Convert.ToDateTime(datas[i]),
                            Convert.ToDecimal(valores[i])));
                    }

                    atual.vencimentos = newListaVencimentos;
                    atual.valor = valorTotal;
                    arr[indice] = atual;
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
                else
                {
                    List<SVencimento> newListaVencimentos = new List<SVencimento>();
                    for (int i = 0; i < datas.Count; i++)
                    {
                        newListaVencimentos.Add(new SVencimento(Convert.ToDateTime(datas[i]),
                            Convert.ToDecimal(valores[i])));
                    }

                    SLancamento lancamento = new SLancamento(null, seqLote, null, null, null, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, qtd, valorUnit, valorTotal, null, null, newListaVencimentos, null, null, null, null, null, null, 0, valorBruto, 0, 0);

                    arr.Add(lancamento);
                    HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
                }
            }
            else
            {
                List<SVencimento> newListaVencimentos = new List<SVencimento>();
                for (int i = 0; i < datas.Count; i++)
                {
                    newListaVencimentos.Add(new SVencimento(Convert.ToDateTime(datas[i]),
                        Convert.ToDecimal(valores[i])));
                }

                SLancamento lancamento = new SLancamento(null, seqLote, null, null, null, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, qtd, valorUnit, valorTotal, null, null, newListaVencimentos, null, null, null, null, null, null, 0, valorBruto, 0, 0);

                arr = new List<SLancamento>();
                arr.Add(lancamento);
                HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
            }
        }
        else
        {
            List<SVencimento> newListaVencimentos = new List<SVencimento>();
            for (int i = 0; i < datas.Count; i++)
            {
                newListaVencimentos.Add(new SVencimento(Convert.ToDateTime(datas[i]),
                    Convert.ToDecimal(valores[i])));
            }

            SLancamento lancamento = new SLancamento(null, seqLote, null, null, null, null, null, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, qtd, valorUnit, valorTotal, null, null, newListaVencimentos, null, null, null, null, null, null, 0, valorBruto, 0, 0);

            List<SLancamento> arr = new List<SLancamento>();
            arr.Add(lancamento);
            HttpContext.Current.Session["ss_lancamentos_" + modulo] = arr;
        }
    }

    [WebMethod]
    public static string loadCotacao(int tipoCotacao, int lote, int seq_lote, string modulo) 
    {
        //Tipo Cotacao
        // 1 - Cotação do Lote
        // 2 - Cotação do Lançamento

        string html = "<tr><td colspan=\"2\"><input id=\"tipoCotacao\" type=\"hidden\" value=\"" + tipoCotacao + "\"><input id=\"seqLote\" type=\"hidden\" value=\"" + seq_lote + "\"></td></tr>";

        if (tipoCotacao == 1)
        {
            if (HttpContext.Current.Session["cotacaoLote_" + modulo] != null)
            {
                List<CotacaoItem> list = (List<CotacaoItem>)HttpContext.Current.Session["cotacaoLote_" + modulo];

                foreach(CotacaoItem cotacao in list)
                {
                    html += "<tr class=\"linRegistro\">" +
                            "    <td class=\"codMoeda\" style=\"display:none;\">" + cotacao.codMoeda + "</td>" +
                            "    <td class=\"descrMoeda\" style=\"display:none;\">" + cotacao.descrMoeda + "</td>" +
                            "    <td class=\"valorMoeda\">" + cotacao.descrMoeda + ":<span style=\"width:50%;\"><input class=\"txtTaxa\" type=\"text\" value=\"" + cotacao.valorMoeda + "\" style=\"width:70px;\" /></span></td>" +
                            "</tr>";
                }
                return html;
            }
        }
        else 
        {
            List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];

            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].seqLote == seq_lote)
                {
                    if(arr[i].cotacao != null)
                    {
                        foreach (CotacaoItem cotacao in arr[i].cotacao)
                        {
                            html += "<tr class=\"linRegistro\">" +
                                    "    <td class=\"codMoeda\" style=\"display:none;\">" + cotacao.codMoeda + "</td>" +
                                    "    <td class=\"descrMoeda\" style=\"display:none;\">" + cotacao.descrMoeda + "</td>" +
                                    "    <td class=\"valorMoeda\">" + cotacao.descrMoeda + ":<span style=\"width:50%;\"><input class=\"txtTaxa\" type=\"text\" value=\"" + cotacao.valorMoeda + "\" style=\"width:70px;\" /></span></td>" +
                                    "</tr>";
                        }
                        return html;
                    }
                    break;
                }
            }
        }

        Conexao conn = new Conexao();
        cotacaoDAO cotacaoDAO = new cotacaoDAO(conn);
        DataTable dt = cotacaoDAO.loadCotacaoLancamento(lote,seq_lote);

        foreach(DataRow row in dt.Rows)
        {
            html += "<tr class=\"linRegistro\">" +
                    "    <td class=\"codMoeda\" style=\"display:none;\">"+row["cod_moeda"]+"</td>" +
                    "    <td class=\"descrMoeda\" style=\"display:none;\">" + row["descricao"] + "</td>" +
                    "    <td class=\"valorMoeda\">" + row["descricao"] + ":<span style=\"width:50%;\"><input class=\"txtTaxa\" type=\"text\" value=\"" + row["valor"] + "\" style=\"width:70px;\" /></span></td>" +
                    "</tr>";
        }
        return html;
    }

    [WebMethod]
    public static string salvaCotacao(int tipoCotacao, int seq_lote, List<CotacaoItem> CotacaoItem, string modulo) 
    {
        try
        {
            //Tipo de Cotação 1 é pra folha inteira, o tipo de cotação seria por lançamento
            if (tipoCotacao == 1)
            {
                HttpContext.Current.Session["cotacaoLote_" + modulo] = CotacaoItem;
            }
            else
            {
                List<SLancamento> arr = (List<SLancamento>)HttpContext.Current.Session["ss_lancamentos_" + modulo];

                for (int i = 0; i < arr.Count; i++)
                {
                    if (arr[i].seqLote == seq_lote)
                    {
                        arr[i].cotacao = CotacaoItem;
                        break;
                    }
                }
            }
            return "true";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
#endregion
}
