using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FormContabilizarMoeda : BaseForm
{
    moedaDAL moedaDAL;
    lanctosContabDAO lanctosContabDAO;
    cotacaoDAO cotacaoDAO;
    contasDAO contasDAO;

    public FormContabilizarMoeda()
        : base("CONTABILIZAR_MOEDA")
    {
        moedaDAL = new moedaDAL(_conn);
        lanctosContabDAO = new lanctosContabDAO(_conn);
        cotacaoDAO = new cotacaoDAO(_conn);
        contasDAO = new contasDAO(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Contabilizar Moeda";

        if (!Page.IsPostBack)
        {
            comboMoeda.DataSource = moedaDAL.loadTotal();
            comboMoeda.DataTextField = "DESCRICAO";
            comboMoeda.DataValueField = "COD_MOEDA";
            comboMoeda.DataBind();
            comboMoeda.Items.Insert(0, new ListItem("Selecione..", "0"));
        }
    }

    protected void btnContabilizar_Click(object sender, EventArgs e)
    {
        if (comboMoeda.SelectedValue == "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertaErro3", "alert('Informe uma moeda!');", true);
        }
        else if (txtData.Text == "" || txtData.Text.Length != 7)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertaErro4", "alert('Informe o período para contabilizar!');", true);
        }
        else
        {
            if (contasDAO.loadContaCTA().Rows.Count > 0)
            {
                contabilizarMoeda(txtData.Text, Convert.ToInt32(comboMoeda.SelectedValue));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "CTA não localizado", "alert('Nenhuma conta CTA localizado no plano de contas!');", true);
            }

        }
    }

    public void contabilizarMoeda(string dtData, int codMoeda)
    {
        string lote = "";

        DateTime dtInicio;
        DateTime dtFinal;
        DataTable tableLanctos;
        try
        {
            dtInicio = Convert.ToDateTime("01/" + dtData);
            dtFinal = dtInicio.AddMonths(1).AddDays(-1);

            tableLanctos = lanctosContabDAO.lancamentosDiaCTA(dtInicio, dtFinal);
        }
        catch (Exception err) { throw new Exception(err.Message + "  | Seta valores na Data"); }

        //////////////// CONTABILIZA MOEDA /////////////////

        ///// CONTA CTA /////
        DataTable loadContaCTA = contasDAO.loadContaCTA();
        string codContaCTA;
        string descContaCTA;
        int codJobCTA;
        string descJobCTA;
        int codLinhaCTA;
        string descLinhaCTA;
        int codDivisaoCTA;
        string descDivisaoCTA;
        int codClienteCTA;
        string descClienteCTA;

        try
        {
            codContaCTA = loadContaCTA.Rows[0]["COD_CONTA"].ToString();
            descContaCTA = loadContaCTA.Rows[0]["DESCRICAO_CONTA"].ToString();
            codJobCTA = Convert.ToInt32(loadContaCTA.Rows[0]["COD_JOB"].ToString());
            descJobCTA = loadContaCTA.Rows[0]["DESCRICAO_JOB"].ToString();
            codLinhaCTA = Convert.ToInt32(loadContaCTA.Rows[0]["COD_LINHA_NEGOCIO"].ToString());
            descLinhaCTA = loadContaCTA.Rows[0]["DESCRICAO_LINHA_NEGOCIOS"].ToString();
            codDivisaoCTA = Convert.ToInt32(loadContaCTA.Rows[0]["COD_DIVISAO"].ToString());
            descDivisaoCTA = loadContaCTA.Rows[0]["DESCRICAO_DIVISAO"].ToString();
            codClienteCTA = Convert.ToInt32(loadContaCTA.Rows[0]["COD_CLIENTE"].ToString());
            descClienteCTA = loadContaCTA.Rows[0]["DESCRICAO_CLIENTE"].ToString();
        }
        catch (Exception err) { throw new Exception(err.Message + "  | Nenhuma conta de CTA encontrada! "); }

        ///////////////////////////////////

        ///// VALOR MÉDIO/DIÁRIO DO PERÍODO //////
        decimal cotacaoMedia = 0;
        decimal cotacaoDiaria = 0;
        decimal cotacaoUltimoDia = 0;
        try { cotacaoMedia = cotacaoDAO.cotacaoMediaMes(dtInicio, dtFinal, codMoeda); } catch (Exception err) { Response.Redirect("FormContabilizarMoeda.aspx?error=1&msg=Erro ao contabilizar a cotação média do período " + dtInicio.ToString("dd/MM/yyyy") + " a " + dtFinal.ToString("dd/MM/yyyy" + " error: " + err.Message)); }
        try { cotacaoUltimoDia = cotacaoDAO.cotacaoDiaria(dtInicio, dtFinal, codMoeda); } catch { Response.Redirect("FormContabilizarMoeda.aspx?error=1&msg=Erro ao contabilizar a cotação do último dia do mês! " + dtFinal.ToString("dd/MM/yyyy")); }
        ///////////////////////////////////

        decimal VALOR_CONVERTIDO = 0;
        decimal VALOR_DIFERENCA = 0;
        decimal VALOR_TOTAL = 0;
        List<SLancamento> lanctosMoeda = new List<SLancamento>();
        SLancamento lancto = new SLancamento();
        SLancamento lancto_CTA = new SLancamento();

        ///// ZERO O PERÍODO SELECIONADO /////
        lanctosContabDAO.lancamentosZeraPeriodoMoeda(dtInicio, dtFinal);
        //////////////////////////////////////

        foreach (DataRow dtRow in tableLanctos.Rows)
        {
            ////////////////////////////////////////
            #region VERIFICO SE MUDOU O LOTE E INCLUO O CTA
            if (lote != dtRow["LOTE"].ToString() && lote != "")
            {
                if (VALOR_TOTAL > 0)
                {
                    lancto_CTA.debCred = 'C';
                    lancto_CTA.conta = codContaCTA;
                    lancto_CTA.descConta = descContaCTA;
                    lancto_CTA.valor = VALOR_TOTAL;
                    lanctosMoeda.Add(lancto_CTA);
                }
                else if (VALOR_TOTAL < 0)
                {
                    lancto_CTA.debCred = 'D';
                    lancto_CTA.conta = codContaCTA;
                    lancto_CTA.descConta = descContaCTA;
                    lancto_CTA.valor = (-1) * VALOR_TOTAL;
                    lanctosMoeda.Add(lancto_CTA);
                }

                lancto = null;
                lote = dtRow["LOTE"].ToString();
                VALOR_TOTAL = 0;
            }
            #endregion

            /////////////////////////////
            #region VERIFICA O TIPO DE MOVIMENTO
            decimal CotacaoLancto = cotacaoDAO.retornaCotacao(Convert.ToInt32(dtRow["lote"]), Convert.ToInt32(dtRow["seq_lote"]), codMoeda);
            decimal CotacaoLote = cotacaoDAO.retornaCotacao(Convert.ToInt32(dtRow["lote"]), 0, codMoeda);

            if (CotacaoLancto != 0)
            {
                VALOR_CONVERTIDO = Math.Round(Convert.ToDecimal(dtRow["VALOR"]) / CotacaoLancto, 2);
            }
            else
            {
                if (CotacaoLote != 0)
                {
                    VALOR_CONVERTIDO = Math.Round(Convert.ToDecimal(dtRow["VALOR"]) / CotacaoLote, 2);
                }
                else
                {
                    int codConta = contasDAO.loadMoedaMovimento(dtRow["COD_CONTA"].ToString());
                    if (codConta == 1)
                    {
                        try
                        {
                            cotacaoDiaria = cotacaoDAO.cotacaoDiaria(Convert.ToDateTime(dtRow["DATA"]), codMoeda);
                            VALOR_CONVERTIDO = Math.Round(Convert.ToDecimal(dtRow["VALOR"]) / cotacaoDiaria, 2);
                        }
                        catch
                        {
                            Response.Redirect("FormContabilizarMoeda.aspx?error=1&msg=Erro na cotação do dia " + Convert.ToDateTime(dtRow["DATA"]).ToString("dd/MM/yyyy"));
                        }
                    }
                    else
                    {
                        VALOR_CONVERTIDO = Math.Round(Convert.ToDecimal(dtRow["VALOR"]) / cotacaoMedia, 2);
                    }
                }
            }
            #endregion

            ////////////
            #region CALCULO CTA
            if (dtRow["DEB_CRED"].ToString() == "D")
            {
                VALOR_TOTAL = VALOR_TOTAL + VALOR_CONVERTIDO;
            }
            else
            {
                VALOR_TOTAL = VALOR_TOTAL - VALOR_CONVERTIDO;
            }
            #endregion

            //////////////////
            #region CRIA O LANÇAMENTO
            lancto = new SLancamento();
            lancto.lote = Convert.ToDouble(dtRow["LOTE"]);
            lancto.seqLote = Convert.ToInt32(dtRow["SEQ_LOTE"]);
            lancto.detLote = Convert.ToInt32(dtRow["DET_LOTE"]);
            lancto.lotePai = Convert.ToDouble(dtRow["LOTE_PAI"]);
            lancto.seqLotePai = Convert.ToInt32(dtRow["SEQ_LOTE_PAI"]);
            lancto.duplicata = Convert.ToInt32(dtRow["DUPLICATA"]);
            lancto.seqBaixa = Convert.ToInt32(dtRow["SEQ_BAIXA"]);
            lancto.detBaixa = Convert.ToDouble(dtRow["DET_BAIXA"]);
            lancto.debCred = Convert.ToChar(dtRow["DEB_CRED"]);
            lancto.dataLancamento = Convert.ToDateTime(dtRow["DATA"]);
            lancto.conta = Convert.ToString(dtRow["COD_CONTA"]);
            lancto.descConta = Convert.ToString(dtRow["DESC_CONTA"]);
            lancto.job = Convert.ToInt32(dtRow["COD_JOB"]);
            lancto.descJob = Convert.ToString(dtRow["DESC_JOB"]);
            lancto.linhaNegocio = Convert.ToInt32(dtRow["COD_LINHA_NEGOCIO"]);
            lancto.descLinhaNegocio = Convert.ToString(dtRow["DESC_LINHA_NEGOCIO"]);
            lancto.divisao = Convert.ToInt32(dtRow["COD_DIVISAO"]);
            lancto.descDivisao = Convert.ToString(dtRow["DESC_DIVISAO"]);
            lancto.cliente = Convert.ToInt32(dtRow["COD_CLIENTE"]);
            lancto.descCliente = Convert.ToString(dtRow["DESC_CLIENTE"]);
            lancto.qtd = Convert.ToDecimal(dtRow["QTD"]);
            lancto.valorUnit = VALOR_CONVERTIDO;
            lancto.valor = VALOR_CONVERTIDO;
            lancto.historico = Convert.ToString(dtRow["HISTORICO"]);
            lancto.titulo = Convert.ToBoolean(dtRow["TITULO"]);
            lancto.terceiro = Convert.ToInt32(dtRow["COD_TERCEIRO"]);
            lancto.descTerceiro = Convert.ToString(dtRow["DESC_TERCEIRO"]);
            lancto.pendente = Convert.ToBoolean(dtRow["PENDENTE"]);
            lancto.modulo = Convert.ToString(dtRow["MODULO"]);
            lancto.modelo = Convert.ToInt32(dtRow["COD_MODELO"]);
            lancto.numeroDocumento = Convert.ToString(dtRow["NUMERO_DOCUMENTO"]);
            lancto.codOrigem = Convert.ToString(dtRow["COD_ORIGEM"]);
            lancto.seqOrigem = Convert.ToString(dtRow["SEQ_ORIGEM"]);
            lancto.efetivado = Convert.ToBoolean(dtRow["EFETIVADO"]);
            lancto.tipoLancto = Convert.ToString(dtRow["TIPO_LANCTO"]);
            lancto.usuario = Convert.ToInt32(dtRow["COD_USUARIO"]);
            lancto.codConsultor = Convert.ToInt32(dtRow["COD_CONSULTOR"]);
            lancto.descConsultor = Convert.ToString(dtRow["DESC_CONSULTOR"]);
            lancto.codPlanilha = Convert.ToInt32(dtRow["COD_PLANILHA"]);
            lancto.valorBruto = Convert.ToDecimal((string.IsNullOrEmpty(dtRow["VALOR_BRUTO"].ToString()) ? 0 : dtRow["VALOR_BRUTO"]));

            try
            {
                lancto.valorGrupo = Convert.ToInt32(dtRow["VALOR_GRUPO"]);
            }
            catch
            {
                lancto.valorGrupo = 0;
            }
            #endregion

            //////////////////
            #region LANCTOS CTA
            lancto_CTA = new SLancamento();
            lancto_CTA.lote = Convert.ToDouble(dtRow["LOTE"]);
            lancto_CTA.seqLote = 1 + Convert.ToInt32(dtRow["SEQ_LOTE"]);
            lancto_CTA.detLote = Convert.ToInt32(dtRow["DET_LOTE"]);
            lancto_CTA.lotePai = Convert.ToDouble(dtRow["LOTE_PAI"]);
            lancto_CTA.seqLotePai = Convert.ToInt32(dtRow["SEQ_LOTE_PAI"]);
            lancto_CTA.duplicata = Convert.ToInt32(dtRow["DUPLICATA"]);
            lancto_CTA.seqBaixa = Convert.ToInt32(dtRow["SEQ_BAIXA"]);
            lancto_CTA.detBaixa = Convert.ToDouble(dtRow["DET_BAIXA"]);
            lancto_CTA.debCred = Convert.ToChar(dtRow["DEB_CRED"]);
            lancto_CTA.dataLancamento = Convert.ToDateTime(dtRow["DATA"]);
            lancto_CTA.conta = Convert.ToString(dtRow["COD_CONTA"]);
            lancto_CTA.descConta = Convert.ToString(dtRow["DESC_CONTA"]);
            lancto_CTA.job = codJobCTA;
            lancto_CTA.descJob = descJobCTA;
            lancto_CTA.linhaNegocio = codLinhaCTA;
            lancto_CTA.descLinhaNegocio = descLinhaCTA;
            lancto_CTA.divisao = codDivisaoCTA;
            lancto_CTA.descDivisao = descDivisaoCTA;
            lancto_CTA.cliente = codClienteCTA;
            lancto_CTA.descCliente = descClienteCTA;
            lancto_CTA.qtd = Convert.ToDecimal(dtRow["QTD"]);
            lancto_CTA.valorUnit = VALOR_CONVERTIDO;
            lancto_CTA.valor = VALOR_CONVERTIDO;
            //lancto_CTA.historico = Convert.ToString(dtRow["HISTORICO"]);
            lancto_CTA.historico = "GANHOS / PERDAS NA CONVERSÃO DE BALANÇO - CTA - ( CALCULADO PELO SISTEMA )";
            lancto_CTA.titulo = Convert.ToBoolean(dtRow["TITULO"]);
            lancto_CTA.terceiro = Convert.ToInt32(dtRow["COD_TERCEIRO"]);
            lancto_CTA.descTerceiro = Convert.ToString(dtRow["DESC_TERCEIRO"]);
            lancto_CTA.pendente = Convert.ToBoolean(dtRow["PENDENTE"]);
            lancto_CTA.modulo = Convert.ToString(dtRow["MODULO"]);
            lancto_CTA.modelo = Convert.ToInt32(dtRow["COD_MODELO"]);
            lancto_CTA.numeroDocumento = Convert.ToString(dtRow["NUMERO_DOCUMENTO"]);
            lancto_CTA.codOrigem = Convert.ToString(dtRow["COD_ORIGEM"]);
            lancto_CTA.seqOrigem = Convert.ToString(dtRow["SEQ_ORIGEM"]);
            lancto_CTA.efetivado = Convert.ToBoolean(dtRow["EFETIVADO"]);
            lancto_CTA.tipoLancto = Convert.ToString(dtRow["TIPO_LANCTO"]);
            lancto_CTA.usuario = Convert.ToInt32(dtRow["COD_USUARIO"]);
            lancto_CTA.codConsultor = Convert.ToInt32(dtRow["COD_CONSULTOR"]);
            lancto_CTA.descConsultor = Convert.ToString(dtRow["DESC_CONSULTOR"]);
            lancto_CTA.codPlanilha = Convert.ToInt32(dtRow["COD_PLANILHA"]);
            lancto_CTA.valorBruto = Convert.ToDecimal((string.IsNullOrEmpty(dtRow["VALOR_BRUTO"].ToString()) ? 0 : dtRow["VALOR_BRUTO"]));

            try
            {
                lancto_CTA.valorGrupo = Convert.ToInt32(dtRow["VALOR_GRUPO"]);
            }
            catch
            {
                lancto_CTA.valorGrupo = 0;
            }
            #endregion

            // INSIRO NO LIST ///////
            lanctosMoeda.Add(lancto);

            // ALTERO O LOTE INICIAL ////
            if (lote == "") lote = dtRow["LOTE"].ToString();
        }

        ////////////////////////////////////////
        #region VERIFICO SE MUDOU O LOTE E INCLUO O CTA NA ÚLTIMA CONTA
        if (VALOR_TOTAL > 0)
        {
            lancto_CTA.debCred = 'C';
            lancto_CTA.conta = codContaCTA;
            lancto_CTA.descConta = descContaCTA;
            lancto_CTA.valor = VALOR_TOTAL;
            lanctosMoeda.Add(lancto_CTA);
        }
        else if (VALOR_TOTAL < 0)
        {
            lancto_CTA.debCred = 'D';
            lancto_CTA.conta = codContaCTA;
            lancto_CTA.descConta = descContaCTA;
            lancto_CTA.valor = (-1) * VALOR_TOTAL;
            lanctosMoeda.Add(lancto_CTA);
        }
        #endregion

        ///// FAZENDO A INCLUSÃO NA BASE /////
        insereListaMoeda(lanctosMoeda, codMoeda);

        ///////////////////////////////////////////
        ///// PROCESSO DE CONVERSÃO - BALANÇO /////
        ///////////////////////////////////////////

        ////// CRIO O LOTE ///////////////
        lote = lanctosContabDAO.getNewNumeroLote().ToString();

        ////// PEGO OS LANÇAMENTOS ///////
        tableLanctos = lanctosContabDAO.lancamentosBalanco(dtFinal);
        DataTable tableLanctosMoeda = lanctosContabDAO.lancamentosBalancoMoeda(dtFinal);

        ////// CRIO O LIST DOS FUTUROS LANÇAMENTOS
        lanctosMoeda = new List<SLancamento>();

        ////// SEQ_LOTE ///////
        int SEQ_LOTE = 0;

        ///////////////////////
        bool TemRegistronosDois = true;
        int x = 0;
        int y = 0;
        decimal ValorLancto = 0;
        decimal ValorLanctoMoeda = 0;
        string ContaLancto = "";
        string ContaLanctoMoeda = "";
        string TipoConta = "";
        string DescConta = "";
        decimal DiferencaAcumulada = 0;


        while (true)
        {
            if (tableLanctos.Rows.Count == x)
            {
                TemRegistronosDois = false;
                ValorLancto = 0;
                ContaLancto = "";
                if (tableLanctosMoeda.Rows.Count == y)
                {
                    break;
                }
                else
                {
                    ContaLanctoMoeda = tableLanctosMoeda.Rows[y]["COD_CONTA"].ToString();
                    ValorLanctoMoeda = Convert.ToDecimal(tableLanctosMoeda.Rows[y]["Valor"]);
                    TipoConta = tableLanctosMoeda.Rows[y]["COD_MOEDA_BALANCO"].ToString();
                    DescConta = tableLanctosMoeda.Rows[y]["DESC_CONTA"].ToString();
                    y++;
                }
            }

            if (tableLanctosMoeda.Rows.Count == y && tableLanctos.Rows.Count > x)
            {
                TemRegistronosDois = false;
                ValorLanctoMoeda = 0;
                ContaLanctoMoeda = "";
                ContaLancto = tableLanctos.Rows[x]["COD_CONTA"].ToString();
                ValorLancto = Convert.ToDecimal(tableLanctos.Rows[x]["Valor"]);
                TipoConta = tableLanctos.Rows[x]["COD_MOEDA_BALANCO"].ToString();
                DescConta = tableLanctos.Rows[x]["DESC_CONTA"].ToString();
                x++;
            }
            if (TemRegistronosDois)
            {
                /// Quando ainda existe informações em ambos datatable
                if (tableLanctos.Rows[x]["COD_CONTA"].ToString() == tableLanctosMoeda.Rows[y]["COD_CONTA"].ToString())
                {
                    ContaLancto = tableLanctos.Rows[x]["COD_CONTA"].ToString();
                    ValorLancto = Convert.ToDecimal(tableLanctos.Rows[x]["Valor"]);
                    TipoConta = tableLanctos.Rows[x]["COD_MOEDA_BALANCO"].ToString();
                    DescConta = tableLanctos.Rows[x]["DESC_CONTA"].ToString();

                    ContaLanctoMoeda = tableLanctosMoeda.Rows[y]["COD_CONTA"].ToString();
                    ValorLanctoMoeda = Convert.ToDecimal(tableLanctosMoeda.Rows[y]["Valor"]);
                    x++;
                    y++;
                }
                else if (Convert.ToDecimal(limpaString(tableLanctos.Rows[x]["COD_CONTA"].ToString())) > Convert.ToDecimal(limpaString(tableLanctosMoeda.Rows[y]["COD_CONTA"].ToString())))
                {
                    ValorLancto = 0;
                    ContaLancto = "";
                    ContaLanctoMoeda = tableLanctosMoeda.Rows[y]["COD_CONTA"].ToString();
                    ValorLanctoMoeda = Convert.ToDecimal(tableLanctosMoeda.Rows[y]["Valor"]);
                    TipoConta = tableLanctosMoeda.Rows[y]["COD_MOEDA_BALANCO"].ToString();
                    DescConta = tableLanctosMoeda.Rows[y]["DESC_CONTA"].ToString();
                    y++;
                }
                else
                {
                    ContaLancto = tableLanctos.Rows[x]["COD_CONTA"].ToString();
                    ValorLancto = Convert.ToDecimal(tableLanctos.Rows[x]["Valor"]);
                    TipoConta = tableLanctos.Rows[x]["COD_MOEDA_BALANCO"].ToString();
                    DescConta = tableLanctos.Rows[x]["DESC_CONTA"].ToString();
                    ValorLanctoMoeda = 0;
                    ContaLanctoMoeda = "";
                    x++;
                }
                /// Fim Quando ainda existe informações em ambos datatable

            }
            VALOR_DIFERENCA = Math.Round(((ValorLancto / cotacaoUltimoDia) - ValorLanctoMoeda), 2);

            if (TipoConta == "1" && VALOR_DIFERENCA != 0)
            {
                DiferencaAcumulada += VALOR_DIFERENCA;
                SEQ_LOTE++;
                lancto = new SLancamento();
                lancto.lote = Convert.ToDouble(lote);
                lancto.seqLote = SEQ_LOTE;
                lancto.detLote = 1;
                lancto.lotePai = 0;
                lancto.seqLotePai = 0;
                lancto.duplicata = 0;
                lancto.seqBaixa = 0;
                lancto.detBaixa = 0;
                lancto.dataLancamento = dtFinal;
                lancto.conta = (ContaLancto == "" ? ContaLanctoMoeda : ContaLancto);
                lancto.descConta = DescConta;
                lancto.job = codJobCTA;
                lancto.descJob = descJobCTA;
                lancto.linhaNegocio = codLinhaCTA;
                lancto.descLinhaNegocio = descLinhaCTA;
                lancto.divisao = codDivisaoCTA;
                lancto.descDivisao = descDivisaoCTA;
                lancto.cliente = codClienteCTA;
                lancto.descCliente = descClienteCTA;
                lancto.qtd = 1;
                lancto.valorUnit = 0;
                lancto.historico = "GANHOS E PERDAS - CÁLCULO CTA - MOEDA FINAL DE MÊS";
                lancto.titulo = false;
                lancto.terceiro = 0;
                lancto.descTerceiro = "";
                lancto.pendente = false;
                lancto.modulo = "C_INCLUSAO_LANCTO";
                lancto.modelo = 49;
                lancto.numeroDocumento = "0";
                lancto.codOrigem = "0";
                lancto.seqOrigem = "0";
                lancto.efetivado = true;
                lancto.tipoLancto = "N";
                lancto.codConsultor = 0;
                lancto.descConsultor = "N";
                lancto.codPlanilha = 0;
                lancto.valorBruto = 0;
                lancto.valorGrupo = 0;

                if (VALOR_DIFERENCA > 0)
                {
                    lancto.valor = VALOR_DIFERENCA;
                    lancto.debCred = 'D';
                    lancto.valorUnit = VALOR_DIFERENCA;
                    lanctosMoeda.Add(lancto);
                }
                else if (VALOR_DIFERENCA < 0)
                {
                    lancto.valor = (-1) * VALOR_DIFERENCA;
                    lancto.valorUnit = (-1) * VALOR_DIFERENCA;
                    lancto.debCred = 'C';
                    lanctosMoeda.Add(lancto);
                }

            }
        }

        if (DiferencaAcumulada != 0)
        {
            SEQ_LOTE++;
            lancto = new SLancamento();
            lancto.lote = Convert.ToDouble(lote);
            lancto.seqLote = SEQ_LOTE;
            lancto.detLote = 1;
            lancto.lotePai = 0;
            lancto.seqLotePai = 0;
            lancto.duplicata = 0;
            lancto.seqBaixa = 0;
            lancto.detBaixa = 0;
            lancto.dataLancamento = dtFinal;
            lancto.conta = codContaCTA;
            lancto.descConta = descContaCTA;
            lancto.job = codJobCTA;
            lancto.descJob = "JURIDICA - Geral";
            lancto.linhaNegocio = codLinhaCTA;
            lancto.descLinhaNegocio = "Geral";
            lancto.divisao = codDivisaoCTA;
            lancto.descDivisao = "Geral";
            lancto.cliente = codClienteCTA;
            lancto.descCliente = "Geral";
            lancto.qtd = 1;
            lancto.valorUnit = 0;
            lancto.historico = "GANHOS / PERDAS NA CONVERSÃO DE BALANÇO - CTA - ( CALCULADO PELO SISTEMA )";
            lancto.titulo = false;
            lancto.terceiro = 0;
            lancto.descTerceiro = "";
            lancto.pendente = false;
            lancto.modulo = "C_INCLUSAO_LANCTO";
            lancto.modelo = 49;
            lancto.numeroDocumento = "0";
            lancto.codOrigem = "0";
            lancto.seqOrigem = "0";
            lancto.efetivado = true;
            lancto.tipoLancto = "N";
            lancto.codConsultor = 0;
            lancto.descConsultor = "N";
            lancto.codPlanilha = 0;
            lancto.valorBruto = 0;
            lancto.valorGrupo = 0;

            if (DiferencaAcumulada > 0)
            {
                lancto.valor = DiferencaAcumulada;
                lancto.debCred = 'C';
                lancto.valorUnit = DiferencaAcumulada;
                lanctosMoeda.Add(lancto);
            }
            else
            {
                lancto.valor = (-1) * DiferencaAcumulada;
                lancto.valorUnit = (-1) * DiferencaAcumulada;
                lancto.debCred = 'D';
                lanctosMoeda.Add(lancto);
            }
        }

        insereListaMoeda(lanctosMoeda, codMoeda);

        Response.Redirect("FormContabilizarMoeda.aspx?error=0");
    }

    public void insereListaMoeda(List<SLancamento> lista, int codMoeda)
    {
        foreach (SLancamento listaRow in lista)
        {
            lanctosContabDAO.insertMoeda((double)listaRow.lote, (int)listaRow.seqLote, (int)listaRow.detLote, codMoeda, (double)listaRow.lotePai, (int)listaRow.seqLotePai, (double)listaRow.duplicata, (double)listaRow.seqBaixa,
                (double)listaRow.detBaixa, (char)listaRow.debCred, (DateTime)listaRow.dataLancamento, listaRow.conta, listaRow.descConta, (int)listaRow.job, listaRow.descJob, (int)listaRow.linhaNegocio,
                listaRow.descLinhaNegocio, (int)listaRow.divisao, listaRow.descDivisao, (int)listaRow.cliente, listaRow.descCliente, (decimal)listaRow.qtd, (decimal)listaRow.valorUnit, (decimal)listaRow.valor, listaRow.historico,
                (bool)listaRow.titulo, (int)listaRow.terceiro, listaRow.descTerceiro, (bool)listaRow.pendente, listaRow.modulo, (int)listaRow.modelo, listaRow.numeroDocumento, listaRow.descConsultor, listaRow.codConsultor,
                listaRow.codPlanilha, (decimal)listaRow.valorBruto, (int)listaRow.valorGrupo, "N");
        }

    }

    public string limpaString(string number)
    {
        return number.Replace(".", "")
            .Replace(",", "")
            .Replace("-", "")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace(":", "")
            .Replace(";", "");
    }
}