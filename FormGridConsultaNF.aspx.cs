using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class FormGridConsultaNF : BaseGridForm
{
    private EmissaoNF_Funcoes _EmissaoNF_Funcoes;
    private Emitente _Emitente;
    private Contabilizacao_Funcoes _Contabilizacao_Funcoes;

    private DataTable tbNotasFiscais = new DataTable("tbNotasFiscais");
    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbTomadores = new DataTable("tbTomadores");

    private int? Emitente;
    private int? Tomador;
    private int? Numero_NF;
    private int? Numero_RPS;
    private DateTime? De;
    private DateTime? Ate;
    private decimal? Valor_Total;
    private string Situacao_NF;
    private string Status;

    private List<string> avisos = new List<string>();

    public FormGridConsultaNF()
        : base("FATURAMENTO_CONSULTA_NF")
    {
        _EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);
        _Emitente = new Emitente(_conn);
        _Contabilizacao_Funcoes = new Contabilizacao_Funcoes(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        this.btnAcaoRoboV2.Click += new EventHandler(this.btnAcaoRoboV2_Click);

        hdfBtnAcaoRoboV2Uid.Value = btnAcaoRoboV2.UniqueID;


        if (!IsPostBack)
        {
            BindCertificados();
        }
    }

    protected override void verificaTarefas()
    {
        botaoNovo.Visible = false;
        botaoContabilizar.Visible = true;
        botaoGerarArquivo.Visible = true;
        botaoImportarArquivo.Visible = true;
        botaoDeletar.Text = "Cancelar";

        bool aceitaConsultar = false;
        bool aceitaCancelar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "CONSULTAR")
                aceitaConsultar = true;

            if (_tarefas[i].tarefa == "CANCELAR")
                aceitaCancelar = true;
        }

        if (!aceitaCancelar)
            botaoDeletar.Enabled = false;

        if (!aceitaConsultar)
        {
            foreach (RepeaterItem item in repeaterDados.Items)
            {
                if (item.ItemType != ListItemType.Separator)
                {
                    HyperLink linkConsultar = (HyperLink)item.FindControl("linkConsultar");
                    linkConsultar.Enabled = false;
                }
            }
        }
    }

    //-----------------------------------------------------------------------------
    //METODO MONTA TELA (COMENTADO PRA USAR O QUE LEVA DIRETO SEM PUXAR DO BANCO)
    //-----------------------------------------------------------------------------

    //protected override void montaTela()
    //{
    //    base.montaTela();
    //    Title += "Consulta de Nota Fiscal";
    //    subTitulo.Text = "Consulta de Nota Fiscal";

    //    dsDados.Tables.Add(tbNotasFiscais);
    //    repeaterDados.DataSource = dsDados;
    //    repeaterDados.DataMember = dsDados.Tables["tbNotasFiscais"].TableName;
    //    repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

    //    if (!Page.IsPostBack)
    //    {
    //        comboOrdenar.Items.Add(new ListItem("Emitente", "NOME_RAZAO_SOCIAL_EMITENTE"));
    //        comboOrdenar.Items.Add(new ListItem("Tomador", "NOME_RAZAO_SOCIAL_TOMADOR"));
    //        comboOrdenar.Items.Add(new ListItem("Nº NF", "NUMERO_NF"));
    //        comboOrdenar.Items.Add(new ListItem("Nº RPS", "NUMERO_RPS"));
    //        comboOrdenar.Items.Add(new ListItem("Data de Emissão", "DATA_EMISSAO_RPS"));
    //        comboOrdenar.Items.Add(new ListItem("Valor Total", "VALOR_SERVICOS"));
    //        comboOrdenar.Items.Add(new ListItem("Situação NF", "SITUACAO_NF"));
    //        comboOrdenar.Items.Add(new ListItem("Status", "STATUS"));
    //        comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

    //        _Emitente.lista_Emitentes(ref tbEmitentes);
    //        ddlEmitente.DataSource = tbEmitentes;
    //        ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
    //        ddlEmitente.DataValueField = "COD_EMPRESA";
    //        ddlEmitente.DataBind();
    //        ddlEmitente.Items.Insert(0, new ListItem("Escolha", "0"));

    //        _EmissaoNF_Funcoes.lista_Tomadores(ref tbTomadores);
    //        ddlTomador.DataSource = tbTomadores;
    //        ddlTomador.DataTextField = "NOME_RAZAO_SOCIAL_TOMADOR";
    //        ddlTomador.DataValueField = "COD_TOMADOR";
    //        ddlTomador.DataBind();
    //        ddlTomador.Items.Insert(0, new ListItem("Escolha", "0"));
    //    }
    //}






    //METODO MONTA TELA NOVO SEM PUXAR DO BANCO
    protected override void montaTela()
    {
        base.montaTela();
        Title += "Consulta de Nota Fiscal";
        subTitulo.Text = "Consulta de Nota Fiscal";

        dsDados.Tables.Add(tbNotasFiscais);
        repeaterDados.DataSource = dsDados;
        repeaterDados.DataMember = dsDados.Tables["tbNotasFiscais"].TableName;
        repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

        if (!Page.IsPostBack)
        {
            // Ordenação (Mantido original)
            comboOrdenar.Items.Add(new ListItem("Emitente", "NOME_RAZAO_SOCIAL_EMITENTE"));
            comboOrdenar.Items.Add(new ListItem("Tomador", "NOME_RAZAO_SOCIAL_TOMADOR"));
            comboOrdenar.Items.Add(new ListItem("Nº NF", "NUMERO_NF"));
            comboOrdenar.Items.Add(new ListItem("Nº RPS", "NUMERO_RPS"));
            comboOrdenar.Items.Add(new ListItem("Data de Emissão", "DATA_EMISSAO_RPS"));
            comboOrdenar.Items.Add(new ListItem("Valor Total", "VALOR_SERVICOS"));
            comboOrdenar.Items.Add(new ListItem("Situação NF", "SITUACAO_NF"));
            comboOrdenar.Items.Add(new ListItem("Status", "STATUS"));
            comboOrdenar.Items.Insert(0, new ListItem("Escolha", "0"));

            // --- LOBOTOMIA NOS FILTROS (MOCK) ---
            // Comentamos as chamadas ao banco que travam a abertura

            /* _Emitente.lista_Emitentes(ref tbEmitentes);
            ddlEmitente.DataSource = tbEmitentes;
            ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlEmitente.DataValueField = "COD_EMPRESA";
            ddlEmitente.DataBind();
            */
            // Inserimos dados falsos para o filtro funcionar visualmente
            ddlEmitente.Items.Insert(0, new ListItem("Todos (Mock)", "0"));
            ddlEmitente.Items.Add(new ListItem("Empresa Teste LTDA", "1"));

            /*
            _EmissaoNF_Funcoes.lista_Tomadores(ref tbTomadores);
            ddlTomador.DataSource = tbTomadores;
            ddlTomador.DataTextField = "NOME_RAZAO_SOCIAL_TOMADOR";
            ddlTomador.DataValueField = "COD_TOMADOR";
            ddlTomador.DataBind();
            */
            ddlTomador.Items.Insert(0, new ListItem("Todos (Mock)", "0"));
            ddlTomador.Items.Add(new ListItem("Cliente Exemplo S/A", "10"));
        }
    }











    //-----------------------------------------------------------------------------
    //METODO MONTA GRID (COMENTADO PRA USAR O QUE LEVA DIRETO SEM PUXAR DO BANCO)
    //-----------------------------------------------------------------------------
    //protected override void montaGrid()
    //{
    //    base.montaGrid();

    //    avisos.Clear();

    //    if (ddlEmitente.Value == "0")
    //        Emitente = null;
    //    else
    //        Emitente = Convert.ToInt32(ddlEmitente.Value);

    //    if (ddlTomador.Value == "0")
    //        Tomador = null;
    //    else
    //        Tomador = Convert.ToInt32(ddlTomador.Value);

    //    if (string.IsNullOrEmpty(tbxNumeroNF.Text))
    //        Numero_NF = null;
    //    else
    //        Numero_NF = Convert.ToInt32(tbxNumeroNF.Text);

    //    if (string.IsNullOrEmpty(tbxNumeroRPS.Text))
    //        Numero_RPS = null;
    //    else
    //        Numero_RPS = Convert.ToInt32(tbxNumeroRPS.Text);

    //    if (string.IsNullOrEmpty(tbxDe.Value))
    //        De = null;
    //    else
    //    {
    //        string Verifica_Data_De = _EmissaoNF_Funcoes.Verifica_Data(tbxDe.Value, "Período Inicial");

    //        if (string.IsNullOrEmpty(Verifica_Data_De))
    //            De = Convert.ToDateTime(tbxDe.Value);
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('" + Verifica_Data_De + "');", true);
    //            return;
    //        }
    //    }

    //    if (string.IsNullOrEmpty(tbxAte.Value))
    //        Ate = null;
    //    else
    //    {
    //        string Verifica_Data_Ate = _EmissaoNF_Funcoes.Verifica_Data(tbxAte.Value, "Período Final");

    //        if (string.IsNullOrEmpty(Verifica_Data_Ate))
    //            Ate = Convert.ToDateTime(tbxAte.Value);
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('" + Verifica_Data_Ate + "');", true);
    //            return;
    //        }
    //    }

    //    if (string.IsNullOrEmpty(tbxValorTotal.Value) || tbxValorTotal.Value == "," || tbxValorTotal.Value == ".")
    //        Valor_Total = null;
    //    else
    //        Valor_Total = Convert.ToDecimal(tbxValorTotal.Value.Replace(".", ","));

    //    if (ddlSituacaoNF.Value == "0")
    //        Situacao_NF = null;
    //    else
    //        Situacao_NF = ddlSituacaoNF.Value;

    //    if (ddlStatus.Value == "0")
    //        Status = null;
    //    else
    //        Status = ddlStatus.Value;

    //    totalRegistros = _EmissaoNF_Funcoes.totalRegistros(Emitente, Tomador, Numero_NF, Numero_RPS, De, Ate, Valor_Total, Situacao_NF, Status);
    //    tbNotasFiscais.Clear();
    //    _EmissaoNF_Funcoes.listaPaginada(ref tbNotasFiscais, Emitente, Tomador, Numero_NF, Numero_RPS, De, Ate, Valor_Total, Situacao_NF, Status, paginaAtual, ordenacao);
    //    repeaterDados.DataBind();
    //    base.montaGrid();

    //    if (avisos.Count > 0)
    //        errosFormulario(avisos);
    //}









    //METODO MONTA GRID NOVO SEM PUXAR DO BANCO
    //protected override void montaGrid()
    //{
    //    base.montaGrid();

    //    avisos.Clear();

    //    // (Mantendo a lógica de leitura dos filtros para não quebrar a compilação)
    //    if (ddlEmitente.Value == "0") Emitente = null; else Emitente = Convert.ToInt32(ddlEmitente.Value);
    //    if (ddlTomador.Value == "0") Tomador = null; else Tomador = Convert.ToInt32(ddlTomador.Value);
    //    if (string.IsNullOrEmpty(tbxNumeroNF.Text)) Numero_NF = null; else Numero_NF = Convert.ToInt32(tbxNumeroNF.Text);
    //    if (string.IsNullOrEmpty(tbxNumeroRPS.Text)) Numero_RPS = null; else Numero_RPS = Convert.ToInt32(tbxNumeroRPS.Text);

    //    // Validação de datas (Mantida simplificada para não travar se estiver vazio)
    //    if (!string.IsNullOrEmpty(tbxDe.Value)) De = Convert.ToDateTime(tbxDe.Value);
    //    if (!string.IsNullOrEmpty(tbxAte.Value)) Ate = Convert.ToDateTime(tbxAte.Value);

    //    if (string.IsNullOrEmpty(tbxValorTotal.Value) || tbxValorTotal.Value == "," || tbxValorTotal.Value == ".")
    //        Valor_Total = null;
    //    else
    //        Valor_Total = Convert.ToDecimal(tbxValorTotal.Value.Replace(".", ","));

    //    if (ddlSituacaoNF.Value == "0") Situacao_NF = null; else Situacao_NF = ddlSituacaoNF.Value;
    //    if (ddlStatus.Value == "0") Status = null; else Status = ddlStatus.Value;


    //    // --- LOBOTOMIA NA GRID (MOCK DE DADOS) ---

    //    // 1. Limpamos qualquer tentativa anterior
    //    tbNotasFiscais.Clear();
    //    tbNotasFiscais.Columns.Clear();

    //    // 2. Criamos as colunas que o Repeater espera (Baseado no seu código e ordenação)
    //    // O nome dessas colunas deve bater com o que está no ASPX (Eval) e no código ItemDataBound
    //    tbNotasFiscais.Columns.Add("COD_FATURAMENTO_NF", typeof(int)); // ID para o Checkbox
    //    tbNotasFiscais.Columns.Add("NOME_RAZAO_SOCIAL_EMITENTE", typeof(string));
    //    tbNotasFiscais.Columns.Add("NOME_RAZAO_SOCIAL_TOMADOR", typeof(string));
    //    tbNotasFiscais.Columns.Add("NUMERO_NF", typeof(int));
    //    tbNotasFiscais.Columns.Add("NUMERO_RPS", typeof(int));
    //    tbNotasFiscais.Columns.Add("DATA_EMISSAO_RPS", typeof(DateTime));
    //    tbNotasFiscais.Columns.Add("VALOR_SERVICOS", typeof(decimal));
    //    tbNotasFiscais.Columns.Add("SITUACAO_NF", typeof(string)); // 'C' = Cancelada, 'T' = Ativa
    //    tbNotasFiscais.Columns.Add("STATUS", typeof(string)); // 'G'=Gerada, 'A'=Aceita
    //    tbNotasFiscais.Columns.Add("LOTE", typeof(string)); // Para o link de lote

    //    // Campos extras que podem ser usados no relatório ou detalhes (prevenção de erro)
    //    tbNotasFiscais.Columns.Add("CPF_CNPJ_TOMADOR", typeof(string));
    //    tbNotasFiscais.Columns.Add("SERIE_RPS", typeof(string));
    //    tbNotasFiscais.Columns.Add("VALOR_DEDUCOES", typeof(decimal));
    //    tbNotasFiscais.Columns.Add("ALIQUOTA_ISS", typeof(decimal));

    //    // --- DADOS REAIS DO BANCO CONTABILTESTE (MOCKADOS) ---

    //    // RPS 6721
    //    tbNotasFiscais.Rows.Add(26505, "MACSO LEGATE", "TRANSNORTE ENERGIA S.A", 6168, 6721, Convert.ToDateTime("05/12/2025"), 30000.00m, "T", "G", "0", "14683671000451", "1", 0m, 5);

    //    // RPS 6720
    //    tbNotasFiscais.Rows.Add(26504, "MACSO LEGATE", "CTC INFRA & CONSTRUÇÕES LTDA.", 6167, 6720, Convert.ToDateTime("05/12/2025"), 7621.46m, "T", "G", "0", "03998869000165", "1", 0m, 5);

    //    // RPS 6719
    //    tbNotasFiscais.Rows.Add(26503, "MACSO LEGATE", "COPABO INFRA - ESTRUTURA MARITIMA LTDA.", 6166, 6719, Convert.ToDateTime("05/12/2025"), 15245.20m, "T", "G", "0", "02406691000153", "1", 0m, 5);

    //    // RPS 6718
    //    tbNotasFiscais.Rows.Add(26502, "MACSO LEGATE", "EBN Comércio, Importação e Exportação Ltda", 6165, 6718, Convert.ToDateTime("05/12/2025"), 11225.00m, "T", "G", "0", "21111808000116", "1", 0m, 5);

    //    // RPS 6717
    //    tbNotasFiscais.Rows.Add(26501, "MACSO LEGATE", "Sinditêxtil Sind I F T G T E B L A C M B N T F A", 6164, 6717, Convert.ToDateTime("05/12/2025"), 724.40m, "T", "G", "0", "62636253000103", "1", 0m, 5);

    //    totalRegistros = 5;


    //    // Comentamos a chamada real ao banco:
    //    /*
    //    totalRegistros = _EmissaoNF_Funcoes.totalRegistros(...);
    //    _EmissaoNF_Funcoes.listaPaginada(ref tbNotasFiscais, ...);
    //    */

    //    // -----------------------------------------

    //    repeaterDados.DataBind();
    //    base.montaGrid();

    //    if (avisos.Count > 0)
    //        errosFormulario(avisos);
    //}





    //METODO MONTAGRID NOVO, PUXANDO DO BANCO, CASO FUNCIONAR, REMOVER OS OUTROS DOIS METODOS MONTAGRID
    // ------------------------------------------------------------------------
    // 🟢 MONTA GRID COM DADOS REAIS (CONSULTA SQL DIRETA)
    // ------------------------------------------------------------------------
    protected override void montaGrid()
    {
        base.montaGrid();
        avisos.Clear();

        // --- 1. LEITURA DOS FILTROS (Mantido igual) ---
        if (ddlEmitente.Value == "0") Emitente = null; else Emitente = Convert.ToInt32(ddlEmitente.Value);
        if (ddlTomador.Value == "0") Tomador = null; else Tomador = Convert.ToInt32(ddlTomador.Value);
        if (string.IsNullOrEmpty(tbxNumeroNF.Text)) Numero_NF = null; else Numero_NF = Convert.ToInt32(tbxNumeroNF.Text);
        if (string.IsNullOrEmpty(tbxNumeroRPS.Text)) Numero_RPS = null; else Numero_RPS = Convert.ToInt32(tbxNumeroRPS.Text);

        if (!string.IsNullOrEmpty(tbxDe.Value)) De = Convert.ToDateTime(tbxDe.Value);
        if (!string.IsNullOrEmpty(tbxAte.Value)) Ate = Convert.ToDateTime(tbxAte.Value);

        if (string.IsNullOrEmpty(tbxValorTotal.Value) || tbxValorTotal.Value == "," || tbxValorTotal.Value == ".")
            Valor_Total = null;
        else
            Valor_Total = Convert.ToDecimal(tbxValorTotal.Value.Replace(".", ","));

        if (ddlSituacaoNF.Value == "0") Situacao_NF = null; else Situacao_NF = ddlSituacaoNF.Value;
        if (ddlStatus.Value == "0") Status = null; else Status = ddlStatus.Value;


        // --- 2. PREPARAÇÃO DA TABELA ---
        tbNotasFiscais.Clear();
        tbNotasFiscais.Columns.Clear();

        // Colunas obrigatórias para o Repeater (Frontend)
        tbNotasFiscais.Columns.Add("COD_FATURAMENTO_NF", typeof(int));
        tbNotasFiscais.Columns.Add("NOME_RAZAO_SOCIAL_EMITENTE", typeof(string));
        tbNotasFiscais.Columns.Add("NOME_RAZAO_SOCIAL_TOMADOR", typeof(string));
        tbNotasFiscais.Columns.Add("NUMERO_NF", typeof(int));
        tbNotasFiscais.Columns.Add("NUMERO_RPS", typeof(int));
        tbNotasFiscais.Columns.Add("DATA_EMISSAO_RPS", typeof(DateTime));
        tbNotasFiscais.Columns.Add("VALOR_SERVICOS", typeof(decimal));
        tbNotasFiscais.Columns.Add("SITUACAO_NF", typeof(string));
        tbNotasFiscais.Columns.Add("STATUS", typeof(string));
        tbNotasFiscais.Columns.Add("LOTE", typeof(string));
        tbNotasFiscais.Columns.Add("CPF_CNPJ_TOMADOR", typeof(string));
        tbNotasFiscais.Columns.Add("SERIE_RPS", typeof(string));
        tbNotasFiscais.Columns.Add("VALOR_DEDUCOES", typeof(decimal));
        tbNotasFiscais.Columns.Add("ALIQUOTA_ISS", typeof(decimal));


        // --- 3. QUERY SQL REAL (SEM MOCK) ---
        string sql = @"
            SELECT TOP 50
                COD_FATURAMENTO_NF,
                'MACSO LEGATE'                                  AS NOME_RAZAO_SOCIAL_EMITENTE, 
                ISNULL(NOME_RAZAO_SOCIAL_TOMADOR, 'Consumidor') AS NOME_RAZAO_SOCIAL_TOMADOR,
                ISNULL(NUMERO_NF, 0)                            AS NUMERO_NF,
                ISNULL(NUMERO_RPS, 0)                           AS NUMERO_RPS,
                DATA_EMISSAO_RPS,
                ISNULL(VALOR_SERVICOS, 0)                       AS VALOR_SERVICOS,
                ISNULL(SITUACAO_NF, 'T')                        AS SITUACAO_NF,
                ISNULL(STATUS, 'G')                             AS STATUS,
                ISNULL(LOTE, '0')                               AS LOTE,
                CPF_CNPJ_TOMADOR,
                ISNULL(SERIE_RPS, '1')                          AS SERIE_RPS,
                ISNULL(VALOR_DEDUCOES, 0)                       AS VALOR_DEDUCOES,
                0                                               AS ALIQUOTA_ISS
            FROM 
                FATURAMENTO_NF WITH(NOLOCK)
            ORDER BY 
                COD_FATURAMENTO_NF DESC";

        try
        {
            // Pega a conexão do Web.config
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString;

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                    {
                        da.Fill(tbNotasFiscais); // Preenche a Grid com dados REAIS
                    }
                }
            }

            totalRegistros = tbNotasFiscais.Rows.Count;
        }
        catch (Exception ex)
        {
            avisos.Add("Erro ao carregar do banco: " + ex.Message);
        }

        repeaterDados.DataBind();
        base.montaGrid();

        if (avisos.Count > 0) errosFormulario(avisos);
    }


















    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
            Label lote = (Label)item.FindControl("lote");
            Label situacao_nf = (Label)item.FindControl("situacao_nf");
            Label status = (Label)item.FindControl("status");
            HyperLink linkLote = (HyperLink)item.FindControl("linkLote");
            HyperLink linkConsultar = (HyperLink)item.FindControl("linkConsultar");
            Label emitente = (Label)item.FindControl("emitente");
            Label numero_rps = (Label)item.FindControl("numero_rps");

            if (situacao_nf.Text != "C")
                situacao_nf.Text = "";
            else
                situacao_nf.Text = "CANCELADA";

            if (status != null)
            {
                switch (status.Text)
                {
                    case "G":
                        status.Text = "Gerada";
                        break;
                    case "A":
                        status.Text = "Aceita";
                        break;
                    case "R":
                        status.Text = "Rejeitada";
                        break;
                    case "C":
                        status.Text = "Cancelada";
                        break;
                    default:
                        status.Text = "";
                        break;
                }
            }
            else
                status.Text = "";

            linkConsultar.NavigateUrl = "FormRelatorioEmissaoNF.aspx?id=" + check.Value;

            if (lote.Text != "0")
                linkLote.NavigateUrl = "FormGenericTitulos.aspx?modulo=CAR_INCLUSAO_TITULO&lote=" + lote.Text;

            if (!situacao_nf.Text.Equals("CANCELADA") && (lote.Text.Equals("0") || string.IsNullOrEmpty(lote.Text)))
                avisos.Add("Nota Fiscal do Emitente " + emitente.Text + " com n° de RPS " + numero_rps.Text + " não foi contabilizada!");


        }
    }

    protected override void botaoContabilizar_Click(object sender, EventArgs e)
    {
        botaoContabilizar.Enabled = false;

        int Itens_Selecionados = 0;
        int cod_faturamento_nf;
        string erros = "";
        string Emitente = "";
        string Numero_RPS = "";

        foreach (RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType != ListItemType.Separator)
            {
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");

                if (check.Checked)
                {
                    Itens_Selecionados++;
                    cod_faturamento_nf = Convert.ToInt32(check.Value);

                    if (((Label)item.FindControl("situacao_nf")).Text.Equals("CANCELADA"))
                    {
                        Emitente = ((Label)item.FindControl("emitente")).Text;
                        Numero_RPS = ((Label)item.FindControl("numero_RPS")).Text;
                        erros = "Nota Fiscal está cancelada.";

                        break;
                    }

                    erros = _Contabilizacao_Funcoes.Contabilizar(cod_faturamento_nf);

                    if (!string.IsNullOrEmpty(erros))
                    {
                        Label emitente = (Label)item.FindControl("emitente");
                        Label numero_rps = (Label)item.FindControl("numero_rps");
                        Emitente = emitente.Text.Replace("'", " ");
                        Numero_RPS = numero_rps.Text;
                        break;
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(erros))
            ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Operação realizada com sucesso.');", true);
        else
        {
            if (Itens_Selecionados == 1)
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Foi encontrado um ou mais erros no\\nEmitente " + Emitente + " (Nº RPS " + Numero_RPS + ").\\nConforme segue:\\n\\n" + erros + "\\n\\nNenhum item foi Contabilizado.');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Somente os itens anteriores foram Contabilizados com sucesso.\\nPois, foi encontrado um ou mais erros no\\nEmitente " + Emitente + " (Nº RPS " + Numero_RPS + ").\\nConforme segue:\\n\\n" + erros + "');", true);
        }
        
        montaGrid();
        botaoContabilizar.Enabled = true;
    }

    protected override void botaoGerarArquivo_Click(object sender, EventArgs e)
    {
        List<int> cod_faturamento_nf = new List<int>();
        string erros = string.Empty;
        string Emitente = string.Empty;
        DateTime Data_Inicio = new DateTime();
        DateTime Data_Fim = new DateTime();

        foreach (RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType != ListItemType.Separator)
            {
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");

                if (check.Checked)
                {
                    cod_faturamento_nf.Add(Convert.ToInt32(check.Value));
                    Label emitente = (Label)item.FindControl("emitente");
                    Label dt_emissao = (Label)item.FindControl("dt_emissao");

                    if (((Label)item.FindControl("situacao_nf")).Text.Equals("CANCELADA"))
                        erros += "Nota Fiscal de " + ((Label)item.FindControl("emitente")).Text + " com n° de RPS " + ((Label)item.FindControl("numero_RPS")).Text + " está cancelada.";

                    if (string.IsNullOrEmpty(Emitente))
                        Emitente = emitente.Text.Replace("'", " ");
                    else
                    {
                        if (emitente.Text.Replace("'", " ") != Emitente)
                            erros = "Apenas um emitente pode ser selecionado por arquivo.";
                    }

                    if (Data_Inicio == DateTime.MinValue)
                    {
                        Data_Inicio = Convert.ToDateTime(dt_emissao.Text.ToString());
                        Data_Fim = Convert.ToDateTime(dt_emissao.Text.ToString());
                    }
                    else
                    {
                        if (Convert.ToDateTime(dt_emissao.Text.ToString()) < Data_Inicio)
                            Data_Inicio = Convert.ToDateTime(dt_emissao.Text.ToString());

                        if (Convert.ToDateTime(dt_emissao.Text.ToString()) > Data_Fim)
                            Data_Fim = Convert.ToDateTime(dt_emissao.Text.ToString());
                    }
                }
            }
        }

        if (cod_faturamento_nf.Count == 0)
            erros = "Nenhuma nota selecionada.";

        if (string.IsNullOrEmpty(erros))
        {
            try
            {
                Conexao _conn = new Conexao();
                RPSDAO _RPSDAO = new RPSDAO(_conn);

                string arq = Remove_Accents(Emitente.Replace(" ", "_").Replace("\\", "").Replace("/", "").Replace(",", "").Replace(".", "") + DateTime.Now.ToString("ddMMyyyy_hhmmss")) + ".txt";
                string caminho = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Temp\\" + arq;

                using (StreamWriter Escrever = new StreamWriter(File.Open(caminho, FileMode.CreateNew), Encoding.GetEncoding("iso-8859-1")))
                {
                    //Baseado no seguinte arquivo: ..\Contábil\Layouts e Tutoriais\Layouts\NFe_Layout_RPS (Versão 3.18).pdf
                    //Páginas: 18 ao 25
                    //Versão do Arquivo: 002

                    Escrever.WriteLine(Gera_Cabecalho(Data_Inicio, Data_Fim, _RPSDAO.Load_IM_Emissor(cod_faturamento_nf[0]), "002"));

                    DataTable RPS = _RPSDAO.Load_RPS(cod_faturamento_nf);
                    int Count = 0;

                    foreach (string item in Gera_Detalhe(RPS))
                    {
                        if (Count != RPS.Rows.Count)
                            Escrever.WriteLine(item);
                        else
                            Escrever.Write(item);

                        Count++;
                    }
                }

                _RPSDAO.Atualiza_Status(cod_faturamento_nf);
                montaGrid();

                HttpResponse response = HttpContext.Current.Response;
                response.ClearContent();
                response.Clear();
                response.ContentType = "text/plain";
                response.AddHeader("Content-Disposition", "attachment; filename=" + arq + ";");
                response.TransmitFile(caminho);
                response.Flush();
                response.End();

                ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Operação realizada com sucesso.');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Erro ao gerar o arquivo.');", true);
            }
        }
        else
            ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('" + erros + "');", true);
    }

    public static string Remove_Accents(string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }

        return sbReturn.ToString();
    }

    public string Gera_Cabecalho(DateTime dtinicio, DateTime dtfim, string iE_Emissor, string versao)
    {
        return "1" + versao + Gera_Campo(8, iE_Emissor.Replace("-", "").Replace(".", "").Replace(" ", "").Trim(), true) + dtinicio.ToString("yyyyMMdd") + dtfim.ToString("yyyyMMdd") /*+ ((char)13) + ((char)10)*/;
    }

    public List<string> Gera_Detalhe(DataTable RPS)
    {
        //Baseado no seguinte arquivo: ..\Contábil\Layouts e Tutoriais\Layouts\NFe_Layout_RPS (Versão 3.18).pdf
        //Páginas: 18 ao 25
        //Versão do Arquivo: 002

        List<string> List = new List<string>();
        string Linha;
        int Numero_Linhas = 0;
        decimal Valor_Total_Servicos = 0;
        decimal Valor_Total_Deducoes = 0;

        foreach (DataRow item in RPS.Rows)
        {
            Numero_Linhas++;

            Linha = "6" + Gera_Campo(5, "RPS", false);                                                                                  //Campo 01 e 02
            Linha += Gera_Campo(5, item["SERIE_RPS"].ToString(), false);                                                                //Campo 03
            Linha += Gera_Campo(12, item["NUMERO_RPS"].ToString(), true);                                                               //Campo 04
            Linha += Gera_Campo(8, item["DATA_EMISSAO_RPS"].ToString(), false);                                                         //Campo 05
            Linha += "T";                                                                                                               //Campo 06
            Linha += Gera_Campo(15, item["VALOR_SERVICOS"].ToString(), true);                                                           //Campo 07
            Valor_Total_Servicos += Convert.ToDecimal(item["VALOR_SERVICOS"].ToString());                                               //Totalizador
            Linha += Gera_Campo(15, item["VALOR_DEDUCOES"].ToString(), true);                                                           //Campo 08
            Valor_Total_Deducoes += Convert.ToDecimal(item["VALOR_DEDUCOES"].ToString());                                               //Totalizador
            Linha += Gera_Campo(5, item["COD_SERVICO_PREFEITURA"].ToString(), true);                                                    //Campo 09
            Linha += Convert.ToBoolean(item["DESTACADO"].ToString()) == false ?                                                         //Campo 10
                Gera_Campo(4, "0", true) :                                                                                              //Campo 10
                Gera_Campo(4, item["ALIQUOTA_ISS"].ToString(), true);                                                                   //Campo 10
            Linha += "2";                                                                                                               //Campo 11
            Linha += item["CPF_CNPJ_TOMADOR"].ToString().Length < 14 ? "1" : "2";                                                       //Campo 12
            Linha += Gera_Campo(14, item["CPF_CNPJ_TOMADOR"].ToString(), true);                                                         //Campo 13
            Linha += item["MUNICIPIO_TOMADOR"].ToString().ToLower().Replace("ã", "a") == "sao paulo" ?                                  //Campo 14
                Gera_Campo(8, item["IM_TOMADOR"].ToString(), true) :                                                                    //Campo 14
                Gera_Campo(8, "0", true);                                                                                               //Campo 14
            Linha += Gera_Campo(12, item["IE_TOMADOR"].ToString(), true);                                                               //Campo 15
            Linha += Gera_Campo(75, item["NOME_RAZAO_SOCIAL_TOMADOR"].ToString(), false);                                               //Campo 16
            Linha += Gera_Campo(3, item["LOGRADOURO_TOMADOR"].ToString(), false);                                                       //Campo 17
            Linha += Gera_Campo(50, item["ENDERECO_TOMADOR"].ToString(), false);                                                        //Campo 18
            Linha += Gera_Campo(10, item["NUM_ENDERECO_TOMADOR"].ToString(), false);                                                    //Campo 19
            Linha += Gera_Campo(30, item["COMPL_ENDERECO_TOMADOR"].ToString(), false);                                                  //Campo 20
            Linha += Gera_Campo(30, item["BAIRRO_TOMADOR"].ToString(), false);                                                          //Campo 21
            Linha += Gera_Campo(50, item["MUNICIPIO_TOMADOR"].ToString(), false);                                                       //Campo 22
            Linha += Gera_Campo(2, item["UF_TOMADOR"].ToString(), false);                                                               //Campo 23
            Linha += Gera_Campo(8, item["CEP_TOMADOR"].ToString(), false);                                                              //Campo 24
            Linha += Gera_Campo(75, item["EMAIL_TOMADOR"].ToString(), false);                                                           //Campo 25
            Linha += Gera_Campo(15, item["PIS_RETIDO"].ToString(), true);                                                               //Campo 26
            Linha += Gera_Campo(15, item["COFINS_RETIDO"].ToString(), true);                                                            //Campo 27
            Linha += Gera_Campo(15, item["INSS_RETIDO"].ToString(), true);                                                              //Campo 28
            Linha += Gera_Campo(15, item["IR_RETIDO"].ToString(), true);                                                                //Campo 29
            Linha += Gera_Campo(15, item["CSLL_RETIDO"].ToString(), true);                                                              //Campo 30
            Linha += Gera_Campo(15, item["CARGA_IMPOSTOS"].ToString(), true);                                                           //Campo 31
            Linha += Gera_Campo(5, item["IMPOSTOS"].ToString(), true);                                                                  //Campo 32
            Linha += Gera_Campo(10, "IBPT", false);                                                                                     //Campo 33
            Linha += Gera_Campo(12, "0", true);                                                                                         //Campo 34
            Linha += Gera_Campo(12, "0", true);                                                                                         //Campo 35
            Linha += Gera_Campo(7, "0", true);                                                                                          //Campo 36
            Linha += Gera_Campo(10, "0", true);                                                                                         //Campo 37
            Linha += Gera_Campo(10, "", false);                                                                                         //Campo 38
            Linha += Gera_Campo(15, "0", true);                                                                                         //Campo 39
            Linha += Gera_Campo(175, "", false);                                                                                        //Campo 40
            Linha += Gera_Campo(500, item["NOME_SERVICO"].ToString().Replace("|", "")                                                   //Campo 41
                + "| |" + item["DESCRICAO_NARRATIVA"].ToString().Replace(Environment.NewLine, "").Replace("\t", " ").Replace("|", "")   //Campo 41
                + "| |" + item["DISCRIMINACAO_OBSERVACAO"].ToString().Replace("|", "").Replace("\n", "|")                               //Campo 41
                + "| |" + "Vencimentos: " + item["DATA_VENCIMENTO"].ToString().Replace("|", ""), false);                                //Campo 41

            List.Add(Linha);
        }

        //Rodapé
        Linha = "9" + Gera_Campo(7, Numero_Linhas.ToString(), true) + Gera_Campo(15, Valor_Total_Servicos.ToString(), true) + Gera_Campo(15, Valor_Total_Deducoes.ToString(), true);
        List.Add(Linha);

        return List;
    }

    public string Gera_Campo(int tamanho, string campo, bool numerico)
    {
        if (numerico)
        {
            try
            {
                decimal campotemp = Math.Round(Convert.ToDecimal(campo), 2, MidpointRounding.AwayFromZero);
                campo = campotemp.ToString().Replace(",", "").Replace(".", "");
            }
            catch (Exception)
            {
                campo = "0";
            }
            return campo.PadLeft(tamanho, '0').Substring(0, tamanho);
        }
        else
            return campo.PadRight(tamanho, ' ').Substring(0, tamanho);
    }

    protected void btnImportarCSV_Click(object sender, EventArgs e)
    {
        string Caminho = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Temp\\" + fulImportarCSV.FileName;
        fulImportarCSV.SaveAs(Caminho);

        List<string> erros = new List<string>();
        List<EmissaoNF> List_EmissaoNF = new List<EmissaoNF>();
        StreamReader StreamReader = new StreamReader(Caminho);
        string Line;
        string[] Array;
        int Count = 0;

        while ((Line = StreamReader.ReadLine()) != null)
        {
            Array = Line.Split(';');

            //Situação da Nota Fiscal = T (Válida).
            //Situação da Nota Fiscal = C (Cancelada).
            if (Array[22] == "T" && !string.IsNullOrEmpty(Array[6])) //Array[22] = Coluna W (Situação da Nota Fiscal).
            {
                EmissaoNF EmissaoNF = new EmissaoNF();
                EmissaoNF.numero_nf = Convert.ToInt32(Array[1]);
                EmissaoNF.data_emissao_nf = Convert.ToDateTime(Array[2]);
                EmissaoNF.numero_rps = Convert.ToInt32(Array[6]);
                EmissaoNF.data_emissao_rps = Convert.ToDateTime(Array[7]);
                EmissaoNF.cpf_cnpj_emitente = Array[10].Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace(";", "").Replace("|", "").Replace("_", "");
                List_EmissaoNF.Add(EmissaoNF);

                Count++;
            }
        }

        StreamReader.Dispose();
        StreamReader.Close();

        erros.AddRange(_EmissaoNF_Funcoes.Atualiza_NF(List_EmissaoNF));
        erros.AddRange(_Contabilizacao_Funcoes.Atualiza_Lote_NF(List_EmissaoNF));

        if (erros.Count == 0)
            ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Operação realizada com sucesso.\\n\\n" + Count + " Registros foram importados.');", true);
        else
            errosFormulario(erros);

        montaGrid();
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada
        List<string> erros = new List<string>();

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (erros.Count == 0)
        {
            foreach (RepeaterItem item in repeaterDados.Items)
            {
                if (item.ItemType != ListItemType.Separator)
                {
                    HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                    Label emitente = (Label)item.FindControl("emitente");
                    Label numero_rps = (Label)item.FindControl("numero_rps");
                    Label situacao_nf = (Label)item.FindControl("situacao_nf");

                    if (check.Checked)
                    {
                        if (Convert.ToString(situacao_nf.Text) != "CANCELADA")
                        {
                            try
                            {
                                _EmissaoNF_Funcoes.cod_faturamento_nf = Convert.ToInt32(check.Value);
                                _EmissaoNF_Funcoes.emitente = emitente.Text.Replace("'", " ");
                                _EmissaoNF_Funcoes.numero_rps = Convert.ToInt32(numero_rps.Text);

                                int Contador = _EmissaoNF_Funcoes.Verifica_Baixa_Contabilizacao();

                                if (Contador == 0)
                                    erros.AddRange(_EmissaoNF_Funcoes.Cancelar());
                                else
                                    erros.Add("Emitente " + _EmissaoNF_Funcoes.emitente + " (Nº RPS " + _EmissaoNF_Funcoes.numero_rps + "): Existem baixas para esta Nota Fiscal, reverta as baixas para poder cancelar.");
                            }
                            catch
                            {
                                erros.Add("Emitente " + _EmissaoNF_Funcoes.emitente + " (Nº RPS " + _EmissaoNF_Funcoes.numero_rps + "): Não foi possivel cancelar. Erro interno.");
                            }
                        }
                        else
                            erros.Add("Emitente " + emitente.Text.Replace("'", " ") + " (Nº RPS " + numero_rps.Text + "): Já está cancelada.");
                    }
                }
            }
            montaGrid();

            if (erros.Count > 0)
                errosFormulario(erros);
        }
        else
            errosFormulario(erros);
    }


    // ---------------------------------------------------------
    // 🟢 BOTÃO DO ROBÔ V2 (COM LOG EM ARQUIVO + UPDATE VISUAL)
    // ---------------------------------------------------------
    protected void btnAcaoRoboV2_Click(object sender, EventArgs e)
    {
        mpeImportarCSV.Hide();

        // ====== LOG DIR (App_Data) ======
        string logDir = Server.MapPath("~/App_Data");
        System.IO.Directory.CreateDirectory(logDir);

        string tracePath = System.IO.Path.Combine(logDir, "RoboTrace.txt");
        string execLogPath = System.IO.Path.Combine(logDir, "Log_Ultima_Execucao.txt");
        string errPath = System.IO.Path.Combine(logDir, "RoboErro.txt");

        Action<string> Trace = delegate (string msg)
        {
            try
            {
                System.IO.File.AppendAllText(
                    tracePath,
                    DateTime.Now.ToString("s") + " | " + msg + Environment.NewLine
                );
            }
            catch { }
        };

        // (Opcional) Debug visual — depois você remove
        ScriptManager.RegisterStartupScript(
            this, GetType(), "DBG_HIT",
            "alert('✅ CHEGUEI NO SERVIDOR: btnAcaoRoboV2_Click');",
            true
        );

        Trace("Entrou no btnAcaoRoboV2_Click");

        // ====== PATH DO EXE ======
        string caminhoExecutavel = Server.MapPath("~/_MotorNFSe_REFORMA_TRIB/FC.NFSe.Sandbox.exe");
        Trace("caminhoExecutavel=" + caminhoExecutavel);

        if (!System.IO.File.Exists(caminhoExecutavel))
        {
            Trace("ERRO: Robô não encontrado");
            ScriptManager.RegisterStartupScript(this, GetType(), "Erro", "alert('Robô não encontrado!');", true);
            return;
        }

        // ====== COLETA RPS ======
        System.Text.StringBuilder listaRps = new System.Text.StringBuilder();
        System.Collections.Generic.List<int> idsParaAtualizar = new System.Collections.Generic.List<int>();
        int qtdSelecionada = 0;

        foreach (System.Web.UI.WebControls.RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType == System.Web.UI.WebControls.ListItemType.Separator) continue;

            System.Web.UI.HtmlControls.HtmlInputCheckBox check =
                (System.Web.UI.HtmlControls.HtmlInputCheckBox)item.FindControl("check");
            System.Web.UI.WebControls.Label lblRps =
                (System.Web.UI.WebControls.Label)item.FindControl("numero_rps");

            if (check != null && check.Checked)
            {
                listaRps.Append((lblRps != null ? lblRps.Text : "") + " ");
                idsParaAtualizar.Add(Convert.ToInt32(check.Value));
                qtdSelecionada++;
            }
        }

        string argsRps = listaRps.ToString().Trim();
        Trace("qtdSelecionada=" + qtdSelecionada + " | argsRps=[" + argsRps + "]");

        if (qtdSelecionada == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Aviso", "alert('Selecione uma nota!');", true);
            return;
        }

        // ====== CERTIFICADO SELECIONADO ======
        int certId;
        if (!int.TryParse(hdfCertSelecionado.Value, out certId) || certId <= 0)
        {
            Trace("ERRO: hdfCertSelecionado inválido: " + (hdfCertSelecionado.Value ?? ""));
            ScriptManager.RegisterStartupScript(this, GetType(), "AvisoCert", "alert('Selecione um certificado antes de continuar.');", true);
            return;
        }

        var certRow = GetCertificadoById(certId);
        if (certRow == null)
        {
            Trace("ERRO: Certificado não encontrado. certId=" + certId);
            ScriptManager.RegisterStartupScript(this, GetType(), "AvisoCert2", "alert('Certificado não encontrado (ou inativo).');", true);
            return;
        }

        string senhaPlain = DecryptSenhaCompat(certRow.SenhaDb);
        string certTempPath = null;

        try
        {
            // ====== MATERIALIZA CERT TEMP ======
            certTempPath = SalvarCertificadoTemp(certId, certRow.ArquivoNome, certRow.ArquivoBin);
            Trace("certTempPath=" + certTempPath);

            // valida se abre (senha/arquivo batem)
            try
            {
                var _ = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    certRow.ArquivoBin,
                    senhaPlain,
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet |
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.PersistKeySet |
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable
                );
            }
            catch (Exception exCert)
            {
                Trace("ERRO: Certificado não abriu: " + exCert.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "CertInvalido",
                    "alert('O certificado selecionado não pôde ser aberto. Verifique o arquivo/senha armazenados no banco.');", true);
                return;
            }

            // ====== PROCESS START ======
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = caminhoExecutavel;
            info.Arguments = argsRps;
            info.WorkingDirectory = System.IO.Path.GetDirectoryName(caminhoExecutavel);
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            // ENV VARS com certificado + senha
            info.EnvironmentVariables["NFSE_CERT_PATH"] = certTempPath;
            info.EnvironmentVariables["NFSE_CERT_PASS"] = senhaPlain;
            info.EnvironmentVariables["NFSE_CERT_ID"] = certId.ToString();
            info.EnvironmentVariables["NFSE_CERT_ALIAS"] = certRow.Alias ?? "";
            info.EnvironmentVariables["NFSE_CERT_THUMBPRINT"] = certRow.Thumbprint ?? "";

            Trace("Iniciando Process.Start... wd=" + info.WorkingDirectory);

            using (System.Diagnostics.Process p = System.Diagnostics.Process.Start(info))
            {
                if (p == null)
                    throw new Exception("Falha ao iniciar o robô (Process.Start retornou null).");

                string saidaSucesso = p.StandardOutput.ReadToEnd();
                string saidaErro = p.StandardError.ReadToEnd();

                bool finalizou = p.WaitForExit(30000);
                if (!finalizou)
                {
                    try { p.Kill(); } catch { }
                    Trace("TIMEOUT: robô não finalizou em 30s");
                    throw new Exception("Timeout: o robô não finalizou em 30 segundos.");
                }

                int exitCode = p.ExitCode;
                Trace("Process finalizou. ExitCode=" + exitCode);

                // Log consolidado
                string conteudoLog =
                    "DATA: " + DateTime.Now.ToString() +
                    "\nEXIT: " + exitCode +
                    "\nARGS: " + argsRps +
                    "\nCERT_ID: " + certId +
                    "\nCERT_THUMB: " + (certRow.Thumbprint ?? "") +
                    "\nCERT_PATH: " + (certTempPath ?? "") +
                    "\nERROS:\n" + saidaErro +
                    "\nSUCESSO:\n" + saidaSucesso;

                try { System.IO.File.WriteAllText(execLogPath, conteudoLog); } catch { }

                // Análise retorno
                if (!string.IsNullOrEmpty(saidaSucesso) && saidaSucesso.Contains("<Sucesso>true</Sucesso>"))
                {
                    AtualizarStatusNotas(idsParaAtualizar);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Sucesso",
                        "alert('SUCESSO! Notas aceitas. O status foi atualizado na tela.');", true);
                }
                else if (!string.IsNullOrEmpty(saidaSucesso) && saidaSucesso.Contains("<Sucesso>false</Sucesso>"))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Rejeicao",
                        "alert('A Prefeitura rejeitou. Verifique App_Data/Log_Ultima_Execucao.txt.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ErroTecnico",
                        "alert('Erro técnico. O Robô não retornou confirmação. Verifique App_Data/Log_Ultima_Execucao.txt.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(
                    errPath,
                    DateTime.Now.ToString("s") + " | " + ex.ToString() + Environment.NewLine
                );
            }
            catch { }

            string msg = (ex.Message ?? "").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
            ScriptManager.RegisterStartupScript(this, GetType(), "ErroGeral", "alert('Erro crítico: " + msg + "');", true);
        }
        finally
        {
            TryDeleteFile(certTempPath);
            Trace("finally: apagou certTemp e vai montar grid");
            montaGrid();
        }
    }





    // 👇 MÉTODO AUXILIAR DE UPDATE (MANTENHA ELE NA CLASSE)
    private void AtualizarStatusNotas(List<int> ids)
    {
        if (ids.Count == 0) return;
        string idsIn = string.Join(",", ids);

        // Atualiza para 'A' (Aceita)
        string sql = "UPDATE FATURAMENTO_NF SET STATUS = 'A' WHERE COD_FATURAMENTO_NF IN (" + idsIn + ")";

        try
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString;
            using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            avisos.Add("Nota enviada, mas erro ao atualizar status no banco: " + ex.Message);
        }
    }


    // Certificados FELIPE -----




    private class CertDbRow
    {
        public int CertId;
        public string Alias;
        public string ArquivoNome;
        public byte[] ArquivoBin;
        public string SenhaDb;
        public string Thumbprint;
        public DateTime ValidadeFim;
    }

    private CertDbRow GetCertificadoById(int certId)
    {
        using (var conn = new SqlConnection(GetConnString()))
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
SELECT CertId, Alias, ArquivoNome, ArquivoBin, Senha, Thumbprint, ValidadeFim
FROM dbo.NFSe_Certificados
WHERE CertId = @CertId AND Ativo = 1;";
            cmd.Parameters.AddWithValue("@CertId", certId);

            conn.Open();
            using (var rd = cmd.ExecuteReader())
            {
                if (!rd.Read()) return null;

                var row = new CertDbRow();
                row.CertId = Convert.ToInt32(rd["CertId"]);
                row.Alias = rd["Alias"] == DBNull.Value ? "" : rd["Alias"].ToString();
                row.ArquivoNome = rd["ArquivoNome"] == DBNull.Value ? "" : rd["ArquivoNome"].ToString();
                row.ArquivoBin = rd["ArquivoBin"] == DBNull.Value ? null : (byte[])rd["ArquivoBin"];
                row.SenhaDb = rd["Senha"] == DBNull.Value ? "" : rd["Senha"].ToString();
                row.Thumbprint = rd["Thumbprint"] == DBNull.Value ? "" : rd["Thumbprint"].ToString();
                row.ValidadeFim = rd["ValidadeFim"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(rd["ValidadeFim"]);

                return row;
            }
        }
    }

    private string DecryptSenhaCompat(string senhaDb)
    {
        if (string.IsNullOrEmpty(senhaDb)) return "";

        // Compat: se já existirem registros antigos em texto puro, não derruba o fluxo
        try
        {
            return Aes256Crypto.Decrypt(senhaDb);
        }
        catch
        {
            return senhaDb; // fallback: considera que está em texto puro
        }
    }

    private string SalvarCertificadoTemp(int certId, string arquivoNome, byte[] bin)
    {
        if (bin == null || bin.Length == 0)
            throw new Exception("Certificado no banco está sem arquivo (ArquivoBin vazio).");

        string dir = Server.MapPath("~/App_Data/NFSeCertTemp");
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string ext = Path.GetExtension(arquivoNome ?? "");
        if (string.IsNullOrEmpty(ext)) ext = ".pfx";

        string safeName = "cert_" + certId.ToString() + "_" + Guid.NewGuid().ToString("N") + ext;
        string fullPath = Path.Combine(dir, safeName);

        File.WriteAllBytes(fullPath, bin);
        return fullPath;
    }

    private void TryDeleteFile(string path)
    {
        try
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                File.Delete(path);
        }
        catch { }
    }


    private string GetConnString()
    {
        return ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString;
    }

    private void BindCertificados()
    {
        var dt = new DataTable();

        using (var conn = new SqlConnection(GetConnString()))
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
            SELECT CertId, Alias, ArquivoNome, ValidadeFim
            FROM dbo.NFSe_Certificados
            WHERE Ativo = 1
            ORDER BY ValidadeFim DESC, Alias ASC;";

            conn.Open();
            using (var da = new SqlDataAdapter(cmd))
                da.Fill(dt);
        }

        dt.Columns.Add("ValidadeFimFmt", typeof(string));
        dt.Columns.Add("StatusTexto", typeof(string));
        dt.Columns.Add("BadgeClass", typeof(string));

        var hoje = DateTime.Today;

        foreach (DataRow r in dt.Rows)
        {
            var fim = Convert.ToDateTime(r["ValidadeFim"]);
            r["ValidadeFimFmt"] = fim.ToString("dd/MM/yyyy");

            var dias = (fim.Date - hoje).Days;
            if (dias < 0) { r["StatusTexto"] = "Vencido"; r["BadgeClass"] = "expired"; }
            else if (dias <= 30) { r["StatusTexto"] = "Vence em 30 dias"; r["BadgeClass"] = "warn"; }
            else { r["StatusTexto"] = "Válido"; r["BadgeClass"] = "ok"; }
        }

        rptCertificados.DataSource = dt;
        rptCertificados.DataBind();
    }

    protected void btnSalvarCertificado_Click(object sender, EventArgs e)
    {
        lblUploadErro.Text = "";

        try
        {
            var alias = (txtAliasCert.Text ?? "").Trim();
            if (alias.Length == 0)
                throw new Exception("Informe o Alias / Identificação do certificado.");

            var senha = txtSenhaCert.Text ?? "";
            if (senha.Length == 0)
                throw new Exception("Informe a senha do certificado.");

            if (!fuCertificado.HasFile)
                throw new Exception("Selecione um arquivo .pfx ou .p12 antes de salvar.");

            var fileName = System.IO.Path.GetFileName(fuCertificado.FileName);
            var ext = (System.IO.Path.GetExtension(fileName) ?? "").ToLowerInvariant();
            if (ext != ".pfx" && ext != ".p12")
                throw new Exception("Arquivo inválido. Envie um certificado no formato .pfx ou .p12.");

            byte[] bin;
            using (var ms = new System.IO.MemoryStream())
            {
                fuCertificado.PostedFile.InputStream.CopyTo(ms);
                bin = ms.ToArray();
            }

            System.Security.Cryptography.X509Certificates.X509Certificate2 cert;
            try
            {
                cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    bin,
                    senha,
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet |
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.PersistKeySet |
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable
                );
            }
            catch
            {
                throw new Exception("Não foi possível abrir o certificado. Verifique se o arquivo e a senha estão corretos.");
            }

            var validadeFim = cert.NotAfter;
            var thumb = (cert.Thumbprint ?? "").Trim().ToUpperInvariant();
            if (thumb.Length == 0)
                throw new Exception("Não foi possível obter o Thumbprint do certificado.");

            // Criptografa a senha para armazenar no banco (AES-256)
            var senhaCriptografada = Aes256Crypto.Encrypt(senha);

            using (var conn = new System.Data.SqlClient.SqlConnection(GetConnString()))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
INSERT INTO dbo.NFSe_Certificados
    (Alias, ArquivoNome, ArquivoBin, Senha, ValidadeFim, Thumbprint, Ativo, DataCadastro)
VALUES
    (@Alias, @ArquivoNome, @ArquivoBin, @Senha, @ValidadeFim, @Thumbprint, 1, GETDATE());
";
                cmd.Parameters.AddWithValue("@Alias", alias);
                cmd.Parameters.AddWithValue("@ArquivoNome", (object)fileName ?? DBNull.Value);
                cmd.Parameters.Add("@ArquivoBin", System.Data.SqlDbType.VarBinary, -1).Value = bin;
                cmd.Parameters.Add("@Senha", System.Data.SqlDbType.NVarChar, 600).Value = senhaCriptografada;
                cmd.Parameters.AddWithValue("@ValidadeFim", validadeFim);
                cmd.Parameters.AddWithValue("@Thumbprint", thumb);

                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    // 2601 e 2627 = UNIQUE/PK violado
                    if (ex.Number == 2601 || ex.Number == 2627)
                    {
                        // Se for especificamente o índice/constraint do Thumbprint, personaliza a msg
                        var msg = (ex.Message ?? "");

                        if (msg.IndexOf("UX_NFSe_Certificados_Thumbprint", StringComparison.OrdinalIgnoreCase) >= 0
                            || msg.IndexOf("Thumbprint", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            lblUploadErro.Text =
                                "Este certificado já está cadastrado (mesmo Thumbprint). " +
                                "Para usar outro, selecione um certificado diferente ou remova o existente na lista.";
                        }
                        else
                        {
                            lblUploadErro.Text =
                                "Não foi possível salvar porque já existe um registro com os mesmos dados (duplicidade).";
                        }

                        // Mantém modal aberto para exibir a mensagem
                        try { mpeUploadCert.Show(); } catch { }
                        return;
                    }

                    // Truncation (coluna pequena) — mensagem amigável
                    if (ex.Number == 8152)
                    {
                        lblUploadErro.Text =
                            "Não foi possível salvar porque algum campo excede o tamanho permitido no banco. " +
                            "Verifique principalmente o tamanho da coluna 'Senha' e 'ArquivoNome'.";
                        try { mpeUploadCert.Show(); } catch { }
                        return;
                    }

                    // Qualquer outro erro de SQL: mensagem genérica + (opcional) log
                    lblUploadErro.Text = "Erro ao salvar no banco de dados. Detalhe: " + ex.Message;
                    try { mpeUploadCert.Show(); } catch { }
                    return;
                }
            }

            // Sucesso: recarrega lista e limpa campos
            BindCertificados();
            txtAliasCert.Text = "";
            txtSenhaCert.Text = "";
            hdfVoltarListaAposUpload.Value = "1";
        }
        catch (Exception ex)
        {
            lblUploadErro.Text = ex.Message;

            // mantém modal aberto para mostrar o erro
            try { mpeUploadCert.Show(); } catch { }
        }
    }


    protected void btnConfirmarCertificado_Click(object sender, EventArgs e)
    {
        // Reusa o mesmo fluxo do robô
        btnAcaoRoboV2_Click(sender, e);
    }








}