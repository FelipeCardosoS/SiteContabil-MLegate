using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class FolhaLancamento
{
    private List<SLancamento> _lanctos = new List<SLancamento>();
    private List<SLancamento> _banco = new List<SLancamento>();
    private List<SLancamento> _impostos = new List<SLancamento>();
    private List<SLancamento> _basesImposto = new List<SLancamento>();
    private List<SLancamento> _titulos = new List<SLancamento>();

    private List<string> erros;
    private List<CotacaoItem> _cotacaoItemLote;
    private lanctosContabDAO lanctoContabDAO;
    private fechamentoDAO fechamentoDAO;
    private controleEncerramentoDAO encerramentoDAO;
    private string _modulo;
    private DateTime _data;
    private Conexao conexao = new Conexao();
    private double _lote;
    private double _baixa;

    public string modulo
    {
        get { return _modulo; }
        set { _modulo = value; }
    }

    public double lote
    {
        get { return _lote; }
        set { _lote = value; }
    }

    public double baixa
    {
        get { return _baixa; }
        set { _baixa = value; }
    }

    public List<CotacaoItem> cotacaoItemLote
    {
        get { return _cotacaoItemLote; }
        set { _cotacaoItemLote = value; }
    }

    public FolhaLancamento(Conexao c)
    {
        conexao = c;
        lanctoContabDAO = new lanctosContabDAO(c);
        fechamentoDAO = new fechamentoDAO(c);
        encerramentoDAO = new controleEncerramentoDAO(c);
    }

    public FolhaLancamento(Conexao c, List<SLancamento> lanctos, string modulo, DateTime data)
        : this(c)
    {
        _lanctos = lanctos;
        _modulo = modulo;
        _data = data;
    }

    public bool salvaBackup(double lote, DateTime dataBackup, int codUsuarioBackup, string tipo)
    {
        try
        {
            lanctoContabDAO.salvaBackup(lote, dataBackup, codUsuarioBackup, tipo);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool verificaFilhos()
    {
        DataTable filhos = lanctoContabDAO.getFilhosLote(_lote);
        bool status = true;
        if (filhos.Rows.Count > 0)
        {
            for (int i = 0; i < filhos.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(filhos.Rows[i]["PENDENTE"]))
                {
                    status = false;
                    break;
                }

            }
        }

        return status;
    }

    public bool verificaFilhos(double lote)
    {
        DataTable filhos = lanctoContabDAO.getFilhosLote(lote);
        bool status = true;
        if (filhos.Rows.Count > 0)
        {
            for (int i = 0; i < filhos.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(filhos.Rows[i]["PENDENTE"]))
                {
                    status = false;
                    break;
                }

            }
        }

        return status;
    }

    public bool verificaFaturamento(double lote)
    {
        DataTable Fatura = lanctoContabDAO.getFilhosFaturamento(lote);
        bool status = true;
        if (Fatura.Rows.Count > 0)
        {
            status = false;
        }

        return status;
    }


    public bool verificaPeriodoFechamento(double lote)
    {
        DateTime data = lanctoContabDAO.getDataLote(lote);
        DataTable fechamentos = new DataTable("fechamentos");
        bool status = false;
        fechamentoDAO.lista(ref fechamentos);

        for (int i = 0; i < fechamentos.Rows.Count; i++)
        {
            DataRow row = fechamentos.Rows[i];

            string[] arrDta = row["PERIODO"].ToString().Split('/');
            DateTime periodo = new DateTime(Convert.ToInt32(arrDta[1]), Convert.ToInt32(arrDta[0]), 1);
            if (periodo.Month == data.Month && periodo.Year == data.Year)
            {
                status = true;
                break;
            }
        }

        return status;
    }

    public bool verificaPeriodoFechamentoBaixas(List<double> baixas)
    {
        int erros = 0;

        for (int x = 0; x < baixas.Count; x++)
        {
            DateTime data = lanctoContabDAO.getDataBaixa(baixas[x]);
            DataTable fechamentos = new DataTable("fechamentos");
            bool status = false;
            fechamentoDAO.lista(ref fechamentos);

            for (int i = 0; i < fechamentos.Rows.Count; i++)
            {
                DataRow row = fechamentos.Rows[i];

                string[] arrDta = row["PERIODO"].ToString().Split('/');
                DateTime periodo = new DateTime(Convert.ToInt32(arrDta[1]), Convert.ToInt32(arrDta[0]), 1);
                if (periodo.Month == data.Month && periodo.Year == data.Year)
                {
                    status = true;
                    break;
                }
            }

            if (!status)
                erros++;
        }

        return (erros == 0);

    }

    public bool verificaPeriodoFechamento(DateTime data)
    {
        //Evandro depois olhar aqui - Aqui eu inverti o status começava com true mudei para false e quando acha mudei de false para true, para respeitar a logica do seu if
        DataTable fechamentos = new DataTable("fechamentos");
        bool status = false;
        fechamentoDAO.lista(ref fechamentos);

        for (int i = 0; i < fechamentos.Rows.Count; i++)
        {
            DataRow row = fechamentos.Rows[i];

            string[] arrDta = row["PERIODO"].ToString().Split('/');
            DateTime periodo = new DateTime(Convert.ToInt32(arrDta[1]), Convert.ToInt32(arrDta[0]), 1);
            if (periodo.Month == data.Month && periodo.Year == data.Year)
            {
                status = true;
                break;
            }
        }

        return status;
    }

    public List<string> baixaTitulo(List<double> selecionados)
    {
        erros = new List<string>();
        if (!verificaPeriodoFechamento(_data))
        {
            erros.Add("Data da Baixa está fora de um período aberto de lançamento.");
        }
        else
        {
            if (_lanctos.Count == 0)
            {
                erros.Add("Nenhum lançamento foi inserido.");
            }
            else
            {
                if (selecionados.Count == 0)
                {
                    erros.Add("Nenhum lançamento foi escolhido para baixar.");
                }
                else
                {
                    SLancamento lancto1 = null;

                    for (int i = 0; i < _lanctos.Count; i++)
                    {
                        if (_lanctos[i].seqLote == 1)
                        {
                            lancto1 = _lanctos[i];
                            break;
                        }
                    }

                    if (lancto1 == null)
                    {
                        erros.Add("Não existe o lançamento padrão na folha.");
                    }
                    else
                    {
                        List<SLancamento> banco = new List<SLancamento>();
                        baixa = lanctoContabDAO.getNewNumeroBaixa();
                        double detalhamentoSelecionados = -1;
                        double detalhamentoBanco = 1;
                        double detalhamentoLanctos;
                        for (int i = 0; i < selecionados.Count; i++)
                        {

                            DataTable linha = lanctoContabDAO.carregaLote(selecionados[i]);

                            if (linha.Rows.Count == 0)
                            {
                                erros.Add("Lote " + selecionados[i] + " vazio.");
                            }
                            else
                            {
                                int ultimoSeqLote = Convert.ToInt32(linha.Rows[linha.Rows.Count - 1]["SEQ_LOTE"]);
                                int novoSeqLote = ultimoSeqLote + 1;
                                for (int x = 0; x < linha.Rows.Count; x++)
                                {
                                    DataRow row = linha.Rows[x];
                                    SLancamento l = new SLancamento();
                                    l.lote = Convert.ToDouble(row["LOTE"]);
                                    l.seqLote = novoSeqLote;
                                    l.detLote = Convert.ToInt32(row["DET_LOTE"]);
                                    l.lotePai = Convert.ToDouble(row["LOTE_PAI"]);
                                    l.seqLotePai = Convert.ToInt32(row["SEQ_LOTE_PAI"]);
                                    l.duplicata = Convert.ToDouble(row["DUPLICATA"]);
                                    l.seqBaixa = baixa;
                                    l.detBaixa = detalhamentoBanco;
                                    if (Convert.ToChar(row["DEB_CRED"]) == 'D')
                                        l.debCred = 'C';
                                    else
                                        l.debCred = 'D';

                                    l.qtd = Convert.ToDecimal(row["QTD"]);
                                    l.valorUnit = Convert.ToDecimal(row["VALOR_UNIT"]);

                                    try
                                    {
                                        l.valorBruto = Convert.ToDecimal(row["VALOR_BRUTO"]);
                                    }
                                    catch
                                    {
                                        l.valorBruto = 0;
                                    }
                                    l.valor = Convert.ToDecimal(row["VALOR"]);

                                    l.conta = lancto1.conta;
                                    l.descConta = lancto1.descConta;
                                    l.job = Convert.ToInt32(row["COD_JOB"]);
                                    l.descJob = Convert.ToString(row["DESC_JOB"]);
                                    l.linhaNegocio = Convert.ToInt32(row["COD_LINHA_NEGOCIO"]);
                                    l.descLinhaNegocio = Convert.ToString(row["DESC_LINHA_NEGOCIO"]);
                                    l.divisao = Convert.ToInt32(row["COD_DIVISAO"]);
                                    l.descDivisao = Convert.ToString(row["DESC_DIVISAO"]);
                                    l.cliente = Convert.ToInt32(row["COD_CLIENTE"]);
                                    l.descCliente = Convert.ToString(row["DESC_CLIENTE"]);
                                    l.pendente = false;
                                    l.dataLancamento = _data;
                                    l.historico = lancto1.historico;
                                    l.titulo = Convert.ToBoolean(row["TITULO"]);
                                    l.terceiro = Convert.ToInt32(row["COD_TERCEIRO"]);
                                    l.descTerceiro = Convert.ToString(row["DESC_TERCEIRO"]);
                                    l.modelo = Convert.ToInt32(row["COD_MODELO"]);
                                    l.modulo = Convert.ToString(row["MODULO"]);
                                    l.numeroDocumento = row["numero_documento"].ToString();
                                    l.descConsultor = row["desc_consultor"].ToString();
                                    l.codConsultor = Convert.ToInt32(row["cod_consultor"]);
                                    _banco.Add(l);
                                }

                                lanctoContabDAO.baixaLote(selecionados[i], baixa, _data, detalhamentoSelecionados);
                                detalhamentoSelecionados--;
                            }
                        }

                        detalhamentoLanctos = (detalhamentoBanco + 1);

                        for (int x = 0; x < _lanctos.Count; x++)
                        {
                            if (_lanctos[x].seqLote != 1)
                            {
                                double lote = lanctoContabDAO.getNewNumeroLote();
                                double duplic = 0;
                                if (_lanctos[x].titulo.Value)
                                    duplic = criaNumeroDuplicata();


                                SLancamento y = new SLancamento();
                                y.lote = lote;
                                y.seqLote = 1;
                                y.detLote = 1;
                                y.lotePai = 0;
                                y.seqLotePai = 0;
                                y.duplicata = duplic;

                                y.seqBaixa = baixa;
                                y.detBaixa = detalhamentoLanctos;
                                y.debCred = _lanctos[x].debCred;
                                y.qtd = _lanctos[x].qtd.Value;
                                y.valorUnit = _lanctos[x].valorUnit.Value;
                                y.valor = _lanctos[x].valor;
                                y.conta = _lanctos[x].conta;
                                y.descConta = _lanctos[x].descConta;
                                y.job = _lanctos[x].job;
                                y.descJob = _lanctos[x].descJob;
                                y.linhaNegocio = _lanctos[x].linhaNegocio;
                                y.descLinhaNegocio = _lanctos[x].descLinhaNegocio;
                                y.divisao = _lanctos[x].divisao;
                                y.descDivisao = _lanctos[x].descDivisao;
                                y.cliente = _lanctos[x].cliente;
                                y.descCliente = _lanctos[x].descCliente;
                                y.pendente = false;
                                y.dataLancamento = _data;
                                y.historico = _lanctos[x].historico;
                                y.titulo = _lanctos[x].titulo;
                                y.terceiro = _lanctos[x].terceiro;
                                y.descTerceiro = _lanctos[x].descTerceiro;
                                y.modelo = _lanctos[x].modelo;
                                y.modulo = "BAIXA_TITULO";
                                y.numeroDocumento = _lanctos[x].numeroDocumento;
                                y.descConsultor = _lanctos[x].descConsultor;
                                y.codConsultor = _lanctos[x].codConsultor;
                                y.valorBruto = 0;
                                _banco.Add(y);

                                SLancamento l = new SLancamento();
                                l.lote = lote;
                                l.seqLote = 2;
                                l.detLote = 1;
                                l.lotePai = 0;
                                l.seqLotePai = 0;
                                if (_lanctos[x].titulo.Value)
                                    l.duplicata = duplic;
                                else
                                    l.duplicata = 0;
                                l.seqBaixa = baixa;
                                l.detBaixa = detalhamentoBanco;
                                if (_lanctos[x].debCred == 'D')
                                    l.debCred = 'C';
                                else
                                    l.debCred = 'D';

                                l.qtd = _lanctos[x].qtd;
                                l.valorUnit = _lanctos[x].valorUnit;
                                l.valor = _lanctos[x].valor;

                                l.conta = lancto1.conta;
                                l.descConta = lancto1.descConta;
                                l.job = _lanctos[x].job;
                                l.descJob = _lanctos[x].descJob;
                                l.linhaNegocio = _lanctos[x].linhaNegocio;
                                l.descLinhaNegocio = _lanctos[x].descLinhaNegocio;
                                l.divisao = _lanctos[x].divisao;
                                l.descDivisao = _lanctos[x].descDivisao;
                                l.cliente = _lanctos[x].cliente;
                                l.descCliente = _lanctos[x].descCliente;
                                l.pendente = false;
                                l.dataLancamento = _data;
                                l.historico = lancto1.historico;
                                l.titulo = _lanctos[x].titulo;
                                l.terceiro = _lanctos[x].terceiro;
                                l.descTerceiro = _lanctos[x].descTerceiro;
                                l.modelo = _lanctos[x].modelo;
                                l.modulo = "BAIXA_TITULO";
                                l.numeroDocumento = _lanctos[x].numeroDocumento;
                                l.descConsultor = _lanctos[x].descConsultor;
                                l.codConsultor = _lanctos[x].codConsultor;
                                l.valorBruto = _lanctos[x].valorBruto;
                                _banco.Add(l);

                                if (_lanctos[x].titulo.Value)
                                {
                                    for (int z = 0; z < _lanctos[x].vencimentos.Count; z++)
                                    {
                                        double novoLote = criaNumeroTitulo();
                                        SLancamento parc = new SLancamento();
                                        parc.lote = novoLote;
                                        parc.seqLote = 1;
                                        parc.detLote = 1;
                                        parc.lotePai = lote;
                                        parc.seqLotePai = _lanctos[x].seqLote;
                                        parc.duplicata = duplic;
                                        parc.seqBaixa = 0;
                                        parc.detBaixa = 0;
                                        if (_lanctos[x].debCred == 'C')
                                        {
                                            parc.debCred = 'D';
                                            parc.modulo = "CAP_INCLUSAO_TITULO";
                                        }
                                        else
                                        {
                                            parc.debCred = 'C';
                                            parc.modulo = "CAR_INCLUSAO_TITULO";
                                        }

                                        parc.valor = _lanctos[x].vencimentos[z].valor;

                                        parc.valorUnit = parc.valor;
                                        parc.qtd = 1;

                                        parc.conta = _lanctos[x].conta;
                                        parc.descConta = _lanctos[x].descConta;
                                        parc.job = _lanctos[x].job;
                                        parc.descJob = _lanctos[x].descJob;
                                        parc.linhaNegocio = _lanctos[x].linhaNegocio;
                                        parc.descLinhaNegocio = _lanctos[x].descLinhaNegocio;
                                        parc.divisao = _lanctos[x].divisao;
                                        parc.descDivisao = _lanctos[x].descDivisao;
                                        parc.cliente = _lanctos[x].cliente;
                                        parc.descCliente = _lanctos[x].descCliente;
                                        parc.pendente = true;
                                        parc.dataLancamento = _lanctos[x].vencimentos[z].data;
                                        parc.historico = _lanctos[x].historico;
                                        parc.titulo = false;
                                        parc.terceiro = _lanctos[x].terceiro;
                                        parc.descTerceiro = _lanctos[x].descTerceiro;
                                        parc.modelo = _lanctos[x].modelo;
                                        parc.numeroDocumento = _lanctos[x].numeroDocumento;
                                        parc.descConsultor = _lanctos[x].descConsultor;
                                        parc.codConsultor = _lanctos[x].codConsultor;
                                        parc.valorBruto = 0;
                                        _banco.Add(parc);
                                    }
                                }
                                detalhamentoLanctos++;
                            }
                        }

                        //INSERE LANCTOS NA BASE
                        for (int i = 0; i < _banco.Count; i++)
                        {
                            SLancamento l = _banco[i];
                            lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, l.modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, l.valorBruto.Value, l.codMoeda);
                        }

                    }
                }
            }
        }

        return erros;
    }

    public List<SLancamento> carregaBaixa(double baixa)
    {
        erros = new List<string>();
        List<SLancamento> arr = new List<SLancamento>();

        if (baixa > 0)
        {
            DataTable linha = lanctoContabDAO.carregaBaixa(baixa);
            if (linha.Rows.Count > 0)
            {
                double detBaixa = 0;
                double loteAnt = 0;
                int seqLoteAnt = 0;
                SLancamento lancto = new SLancamento();
                decimal valor = 0;

                loteAnt = Convert.ToDouble(linha.Rows[0]["lote"]);
                seqLoteAnt = Convert.ToInt32(linha.Rows[0]["seq_lote"]);
                detBaixa = Convert.ToDouble(linha.Rows[0]["det_baixa"]);

                lancto.lote = Convert.ToDouble(linha.Rows[0]["lote"]);
                lancto.seqLote = Convert.ToInt32(linha.Rows[0]["seq_lote"]);
                lancto.detLote = Convert.ToInt32(linha.Rows[0]["det_lote"]);
                lancto.lotePai = Convert.ToDouble(linha.Rows[0]["lote_pai"]);
                lancto.seqLotePai = Convert.ToInt32(linha.Rows[0]["seq_lote_pai"]);
                lancto.duplicata = Convert.ToDouble(linha.Rows[0]["duplicata"]);
                lancto.debCred = Convert.ToChar(linha.Rows[0]["deb_cred"]);

                lancto.valor = Convert.ToDecimal(linha.Rows[0]["VALOR"]);
                lancto.conta = Convert.ToString(linha.Rows[0]["cod_conta"]);
                lancto.descConta = Convert.ToString(linha.Rows[0]["desc_conta"]);
                lancto.job = Convert.ToInt32(linha.Rows[0]["cod_job"]);
                lancto.descJob = Convert.ToString(linha.Rows[0]["desc_job"]);
                lancto.linhaNegocio = Convert.ToInt32(linha.Rows[0]["cod_linha_negocio"]);
                lancto.descLinhaNegocio = Convert.ToString(linha.Rows[0]["desc_linha_negocio"]);
                lancto.divisao = Convert.ToInt32(linha.Rows[0]["cod_divisao"]);
                lancto.descDivisao = Convert.ToString(linha.Rows[0]["desc_divisao"]);
                lancto.cliente = Convert.ToInt32(linha.Rows[0]["cod_cliente"]);
                lancto.descCliente = Convert.ToString(linha.Rows[0]["desc_cliente"]);
                lancto.pendente = Convert.ToBoolean(linha.Rows[0]["pendente"]);
                lancto.dataLancamento = Convert.ToDateTime(linha.Rows[0]["data"]);
                lancto.historico = Convert.ToString(linha.Rows[0]["historico"]);
                lancto.titulo = Convert.ToBoolean(linha.Rows[0]["titulo"]);
                lancto.terceiro = Convert.ToInt32(linha.Rows[0]["cod_terceiro"]);
                lancto.descTerceiro = Convert.ToString(linha.Rows[0]["desc_terceiro"]);
                lancto.modelo = Convert.ToInt32(linha.Rows[0]["cod_modelo"]);
                lancto.numeroDocumento = linha.Rows[0]["numero_documento"].ToString();
                lancto.strData = Convert.ToDateTime(linha.Rows[0]["data"]).ToString("dd/MM/yyyy");
                lancto.modulo = linha.Rows[0]["modulo"].ToString();
                lancto.descConsultor = linha.Rows[0]["desc_consultor"].ToString();
                lancto.codConsultor = Convert.ToInt32(linha.Rows[0]["cod_consultor"]);
                lancto.vencimentos = new List<SVencimento>();

                for (int i = 0; i < linha.Rows.Count; i++)
                {
                    DataRow row = linha.Rows[i];


                    if (Convert.ToDouble(row["det_baixa"]) != detBaixa)
                    {
                        lancto.valor = valor;
                        arr.Add(lancto);
                        lancto = new SLancamento();
                        lancto.lote = Convert.ToDouble(row["lote"]);
                        lancto.seqLote = Convert.ToInt32(row["seq_lote"]);
                        lancto.detLote = Convert.ToInt32(row["det_lote"]);
                        lancto.lotePai = Convert.ToDouble(row["lote_pai"]);
                        lancto.seqLotePai = Convert.ToInt32(row["seq_lote_pai"]);
                        lancto.duplicata = Convert.ToDouble(row["duplicata"]);
                        lancto.debCred = Convert.ToChar(row["deb_cred"]);
                        lancto.conta = Convert.ToString(row["cod_conta"]);
                        lancto.descConta = Convert.ToString(row["desc_conta"]);
                        lancto.job = Convert.ToInt32(row["cod_job"]);
                        lancto.descJob = Convert.ToString(row["desc_job"]);
                        lancto.linhaNegocio = Convert.ToInt32(row["cod_linha_negocio"]);
                        lancto.descLinhaNegocio = Convert.ToString(row["desc_linha_negocio"]);
                        lancto.divisao = Convert.ToInt32(row["cod_divisao"]);
                        lancto.descDivisao = Convert.ToString(row["desc_divisao"]);
                        lancto.cliente = Convert.ToInt32(row["cod_cliente"]);
                        lancto.descCliente = Convert.ToString(row["desc_cliente"]);
                        lancto.pendente = Convert.ToBoolean(row["pendente"]);
                        lancto.dataLancamento = Convert.ToDateTime(row["data"]);
                        lancto.historico = Convert.ToString(row["historico"]);
                        lancto.titulo = Convert.ToBoolean(row["titulo"]);
                        lancto.terceiro = Convert.ToInt32(row["cod_terceiro"]);
                        lancto.descTerceiro = Convert.ToString(row["desc_terceiro"]);
                        lancto.modelo = Convert.ToInt32(row["cod_modelo"]);
                        lancto.numeroDocumento = row["numero_documento"].ToString();
                        lancto.strData = Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy");
                        lancto.vencimentos = new List<SVencimento>();
                        lancto.modulo = linha.Rows[0]["modulo"].ToString();
                        lancto.descConsultor = linha.Rows[0]["desc_consultor"].ToString();
                        lancto.codConsultor = Convert.ToInt32(linha.Rows[0]["cod_consultor"]);

                        if (row["deb_cred"].ToString() == "C")
                            valor = -Convert.ToDecimal(row["valor"]);
                        else
                            valor = Convert.ToDecimal(row["valor"]);

                        loteAnt = Convert.ToDouble(row["lote"]);
                        seqLoteAnt = Convert.ToInt32(row["seq_lote"]);
                        detBaixa = Convert.ToDouble(row["det_baixa"]);
                    }
                    else
                    {
                        if (row["deb_cred"].ToString() == "C")
                            valor -= Convert.ToDecimal(row["valor"]);
                        else
                            valor += Convert.ToDecimal(row["valor"]);
                    }
                }

                lancto.valor = valor;
                arr.Add(lancto);
            }
            else
            {
                erros.Add("Baixa não possui lançamentos");
            }
        }
        else
        {
            erros.Add("Baixa inválida.");
        }

        return arr;
    }

    private void backupLoteTemporario(double lote)
    {
        DataTable lanctosContab = lanctoContabDAO.carregaLote(lote);
        for (int i = 0; i < lanctosContab.Rows.Count; i++)
        {
            DataRow row = lanctosContab.Rows[i];
            lanctoContabDAO.insertTemp(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(), Convert.ToDateTime(row["data_anterior"]),
                row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]));
        }
    }

    private void recuperaLoteTemporario(double lote)
    {
        DataTable lanctosContabTemp = lanctoContabDAO.carregaLoteTemp(lote);
        for (int i = 0; i < lanctosContabTemp.Rows.Count; i++)
        {
            DataRow row = lanctosContabTemp.Rows[i];
            lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(),
                Convert.ToDateTime(row["data_anterior"]), row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), Convert.ToInt32(row["COD_PLANILHA"]));
        }

        lanctoContabDAO.deleteLoteTemp(lote);
    }

    private void recuperaLotePaiTemporario(double lote)
    {
        DataTable lanctosContabTemp = lanctoContabDAO.carregaLotePaiTemp(lote);
        for (int i = 0; i < lanctosContabTemp.Rows.Count; i++)
        {
            DataRow row = lanctosContabTemp.Rows[i];
            lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(), Convert.ToDateTime(row["data_anterior"]),
                row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 60);
        }

        lanctoContabDAO.deleteLoteTemp(lote);
    }

    private void backupBaixaTemporario(double baixa)
    {
        DataTable lanctosContab = lanctoContabDAO.carregaBaixa(baixa);
        for (int i = 0; i < lanctosContab.Rows.Count; i++)
        {
            DataRow row = lanctosContab.Rows[i];
            lanctoContabDAO.insertTempBaixa(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(), Convert.ToDateTime(row["data_anterior"]),
                row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]));
        }
    }

    private void recuperaBaixaTemporario(double baixa)
    {
        bool recuperou = false;
        try
        {
            DataTable demaisTitulos = lanctoContabDAO.carregaRestoBaixaTemp(baixa);
            for (int i = 0; i < demaisTitulos.Rows.Count; i++)
            {
                DataRow row = demaisTitulos.Rows[i];
                lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                    Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                    Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                    row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                    Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                    row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                    Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                    row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                    row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                    Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(),
                    Convert.ToDateTime(row["data_anterior"]), row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 70);
            }

            DataTable contraPartidas = lanctoContabDAO.carregaContraPartidasBaixaTemp(baixa);
            for (int i = 0; i < contraPartidas.Rows.Count; i++)
            {
                DataRow row = contraPartidas.Rows[i];
                lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                    Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                    Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                    row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                    Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                    row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                    Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                    row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                    row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                    Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(),
                    Convert.ToDateTime(row["data_anterior"]), row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 95);
            }

            DataTable outrosFilhos = lanctoContabDAO.carregaTitulosDiferenteLoteTemp(baixa);
            for (int i = 0; i < outrosFilhos.Rows.Count; i++)
            {
                DataRow row = outrosFilhos.Rows[i];
                lanctoContabDAO.restauraBaixa(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToInt32(row["det_baixa"]));
            }
            recuperou = true;
        }
        catch
        {
            recuperou = false;
        }

        if (recuperou)
        {
            lanctoContabDAO.deleteBaixaTemp(baixa);
        }
        else
        {
            throw new Exception("Erro na recuperação das baixas");
        }
    }

    private void recuperaBaixaTemporario(double baixa, double lote)
    {
        bool recuperou = false;
        try
        {
            DataTable filhosLote = lanctoContabDAO.carregaBaixaPorLoteTemp(baixa, lote);
            for (int i = 0; i < filhosLote.Rows.Count; i++)
            {
                DataRow row = filhosLote.Rows[i];
                lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                    Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                    Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                    row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                    Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                    row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                    Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                    row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                    row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                    Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(),
                    Convert.ToDateTime(row["data_anterior"]), row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 96);
            }

            DataTable demaisTitulos = lanctoContabDAO.carregaRestoBaixaTemp(baixa, lote);
            for (int i = 0; i < demaisTitulos.Rows.Count; i++)
            {
                DataRow row = demaisTitulos.Rows[i];
                lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                    Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                    Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                    row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                    Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                    row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                    Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                    row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                    row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                    Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(),
                    Convert.ToDateTime(row["data_anterior"]), row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 97);
            }

            DataTable contraPartidas = lanctoContabDAO.carregaContraPartidasBaixaTemp(baixa, lote);
            for (int i = 0; i < contraPartidas.Rows.Count; i++)
            {
                DataRow row = contraPartidas.Rows[i];
                lanctoContabDAO.insert(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["lote_pai"]), Convert.ToInt32(row["seq_lote_pai"]),
                    Convert.ToDouble(row["duplicata"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToDouble(row["det_baixa"]),
                    Convert.ToChar(row["deb_cred"]), Convert.ToDateTime(row["data"]), row["cod_conta"].ToString(),
                    row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]), row["desc_job"].ToString(),
                    Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(), Convert.ToInt32(row["cod_divisao"]),
                    row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]), row["desc_cliente"].ToString(),
                    Convert.ToDecimal(row["qtd"]), Convert.ToDecimal(row["valor_unit"]), Convert.ToDecimal(row["valor"]),
                    row["historico"].ToString(), Convert.ToBoolean(row["titulo"]), Convert.ToInt32(row["cod_terceiro"]),
                    row["desc_terceiro"].ToString(), Convert.ToBoolean(row["pendente"]), row["modulo"].ToString(),
                    Convert.ToInt32(row["cod_modelo"]), row["numero_documento"].ToString(), Convert.ToDateTime(row["data_anterior"]),
                    row["tipo_lancto"].ToString(), Convert.ToInt32(row["cod_usuario"]), row["desc_consultor"].ToString(), Convert.ToInt32(row["cod_consultor"]), Convert.ToInt32(row["COD_PLANILHA"]), 98);
            }

            DataTable outrosFilhos = lanctoContabDAO.carregaTitulosDiferenteLoteTemp(baixa, lote);
            for (int i = 0; i < outrosFilhos.Rows.Count; i++)
            {
                DataRow row = outrosFilhos.Rows[i];
                lanctoContabDAO.restauraBaixa(Convert.ToDouble(row["lote"]), Convert.ToInt32(row["seq_lote"]),
                    Convert.ToInt32(row["det_lote"]), Convert.ToDouble(row["seq_baixa"]), Convert.ToInt32(row["det_baixa"]));
            }
            recuperou = true;
        }
        catch
        {
            recuperou = false;
        }

        if (recuperou)
        {
            lanctoContabDAO.deleteBaixaTemp(baixa);
        }
        else
        {
            throw new Exception("Erro na recuperação das baixas");
        }
    }

    public List<string> alterar()
    {
        erros = new List<string>();
        if (!verificaFilhos(_lote))
        {
            erros.Add("Filhos já baixados.");
        }
        else
        {
            bool fezBackup = false;
            bool alterou = false;
            List<string> err = new List<string>();

            try
            {
                backupLoteTemporario(_lote);
                fezBackup = true;
            }
            catch
            {
                fezBackup = false;
            }

            if (fezBackup)
            {
                lanctoContabDAO.deleteLote(_lote);
                try
                {
                    err = salvar(_lote);
                    alterou = true;
                }
                catch
                {
                    alterou = false;
                }

                if (!alterou)
                {
                    err.Add("Erro na rotina de alteração do lote.");
                    recuperaLoteTemporario(_lote);
                }
                else
                {
                    lanctoContabDAO.deleteLoteTemp(_lote);
                }
            }
            else
            {
                err.Add("Erro na tentativa de fazer o backup do lote.");
            }

            erros.AddRange(err);
        }

        return erros;
    }

    public List<string> alterarFull()
    {
        erros = new List<string>();
        List<double> baixas = new List<double>();
        List<double> baixasDeletadas = new List<double>();
        List<double> baixasErro = new List<double>();
        List<string> err = new List<string>();
        bool removeuBaixas = false;
        bool fezBackup = false;
        bool alterou = false;

        if (salvaBackup(_lote, DateTime.Now, Convert.ToInt32(HttpContext.Current.Session["usuario"]), "ALT"))
        {
            DataTable filhos = lanctoContabDAO.getBaixasLote(_lote);
            if (filhos.Rows.Count > 0)
            {
                for (int i = 0; i < filhos.Rows.Count; i++)
                {
                    if (Convert.ToDouble(filhos.Rows[i]["SEQ_BAIXA"]) > 0)
                    {
                        baixas.Add(Convert.ToDouble(filhos.Rows[i]["SEQ_BAIXA"]));
                    }
                }
            }
            for (int i = 0; i < baixas.Count; i++)
            {
                bool fezBackupBaixa = false;
                try
                {
                    backupBaixaTemporario(baixas[i]);
                    fezBackupBaixa = true;
                }
                catch
                {
                    fezBackupBaixa = false;
                }

                if (fezBackupBaixa)
                {
                    try
                    {
                        deletaBaixa(baixas[i]);
                        baixasDeletadas.Add(baixas[i]);
                    }
                    catch
                    {
                        baixasErro.Add(baixas[i]);
                    }
                }
                else
                {
                    baixasErro.Add(baixas[i]);
                }
            }

            if (baixasErro.Count == 0)
            {
                removeuBaixas = true;
            }
            else
            {
                removeuBaixas = false;
            }

            if (removeuBaixas)
            {
                try
                {
                    backupLoteTemporario(_lote);
                    fezBackup = true;
                }
                catch
                {
                    fezBackup = false;
                }

                if (fezBackup)
                {
                    lanctoContabDAO.deleteLote(_lote);
                    try
                    {
                        err = salvar(_lote);
                        alterou = true;
                    }
                    catch
                    {
                        alterou = false;
                    }

                    if (!alterou || err.Count > 0)
                    {
                        err.Add("Erro na rotina de alteração do lote.");
                        if (baixas.Count == 0)
                        {
                            recuperaLoteTemporario(_lote);
                        }
                        else
                        {
                            recuperaLotePaiTemporario(_lote);
                            for (int i = 0; i < baixasDeletadas.Count; i++)
                            {
                                recuperaBaixaTemporario(baixasDeletadas[i], _lote);
                            }
                        }
                    }
                    else
                    {
                        lanctoContabDAO.deleteLoteTemp(_lote);

                        for (int i = 0; i < baixasDeletadas.Count; i++)
                        {
                            lanctoContabDAO.deleteBaixaTemp(baixasDeletadas[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < baixasDeletadas.Count; i++)
                    {
                        recuperaBaixaTemporario(baixasDeletadas[i]);
                    }

                    err.Add("Erro na tentativa de fazer backup do lote.");
                }
            }
            else
            {
                for (int i = 0; i < baixasDeletadas.Count; i++)
                {
                    recuperaBaixaTemporario(baixasDeletadas[i]);
                }

                err.Add("Erro na rotina de remoção das baixas.");
            }
        }
        else
        {
            erros.Add("Erro no momento de salvar o backup da folha");
        }

        erros.AddRange(err);
        return erros;
    }

    public bool PodeDeletarBaixa(double Lote, List<SLancamento> arr)
    {
        if (lanctoContabDAO.verificaDeletaBaixa(Lote) > 0)
        {
            return true;
        }

        Conexao c = new Conexao();
        FolhaLancamento folha = new FolhaLancamento(c);
        if (arr == null)
        {
            return true;
        }
        List<SLancamento> banco = folha.carregaLote(Lote);

        contasDAO _contasDAO = new contasDAO(c);
        List<string> ContasImpostos = new List<string>();
        foreach (DataRow item in _contasDAO.loadPlanosContasImpostos().Rows)
        {
            ContasImpostos.Add(item["COD_CONTA"].ToString());
        }

        foreach (SLancamento item in banco)
        {
            if (arr.Where(x => x.seqLote == item.seqLote && (ContasImpostos.Contains(x.conta) || 
                                                            x.valor != item.valor || 
                                                            x.terceiro != item.terceiro || 
                                                            x.debCred != item.debCred ||
                                                            x.dataLancamento != item.dataLancamento)).Count() > 0)
            {
                return true;
            }
        }

        return false;
    }

    public List<string> alterarMin()
    {
        erros = new List<string>();
        if (salvaBackup(_lote, DateTime.Now, Convert.ToInt32(HttpContext.Current.Session["usuario"]), "ALT"))
        {
            for (int i = 0; i < _lanctos.Count; i++)
            {
                lanctoContabDAO.update(_lanctos[i].lote.Value, _lanctos[i].seqLote, _lanctos[i].conta, _lanctos[i].descConta,
                    _lanctos[i].job.Value, _lanctos[i].descJob, _lanctos[i].linhaNegocio.Value, _lanctos[i].descLinhaNegocio,
                    _lanctos[i].divisao.Value, _lanctos[i].descDivisao, _lanctos[i].cliente.Value, _lanctos[i].descCliente, _lanctos[i].historico,
                    _lanctos[i].numeroDocumento, _lanctos[i].descConsultor, _lanctos[i].codConsultor, _lanctos[i].valorBruto);

                if (_lanctos[i].vencimentos != null)
                {
                    if (_lanctos[i].vencimentos.Count > 0)
                    {

                        lanctoContabDAO.updateFilhos(_lanctos[i].lote.Value, _lanctos[i].seqLote, _lanctos[i].conta, _lanctos[i].descConta,
                            _lanctos[i].job.Value, _lanctos[i].descJob, _lanctos[i].linhaNegocio.Value, _lanctos[i].descLinhaNegocio,
                            _lanctos[i].divisao.Value, _lanctos[i].descDivisao, _lanctos[i].cliente.Value, _lanctos[i].descCliente,
                            _lanctos[i].historico, _lanctos[i].numeroDocumento, _lanctos[i].descConsultor, _lanctos[i].codConsultor);

                        lanctoContabDAO.updateBaixasFilhos(_lanctos[i].lote.Value, _lanctos[i].seqLote,
                            _lanctos[i].job.Value, _lanctos[i].descJob, _lanctos[i].linhaNegocio.Value, _lanctos[i].descLinhaNegocio,
                            _lanctos[i].divisao.Value, _lanctos[i].descDivisao, _lanctos[i].cliente.Value, _lanctos[i].descCliente, _lanctos[i].historico,
                            _lanctos[i].numeroDocumento, _lanctos[i].descConsultor, _lanctos[i].codConsultor);
                    }
                }
            }

            LogController logController = new LogController(conexao);

            logController.add(new Log(0, "Folha Alterada", Convert.ToInt32(HttpContext.Current.Session["usuario"]),
                    Convert.ToInt32(HttpContext.Current.Session["empresa"]), _modulo, DateTime.Now, _lote));
        }
        else
        {
            erros.Add("Erro no momento de salvar o backup da folha");
        }

        return erros;
    }

    private void verificaLancamentosZerados()
    {
        List<SLancamento> temp = _lanctos;
        _lanctos = new List<SLancamento>();
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].valor > 0)
                _lanctos.Add(temp[i]);
        }
    }

    private bool verificaVencimentosZerados()
    {
        int nulos = 0;
        _titulos.ForEach(titulo =>
            nulos += titulo.vencimentos.Count(vencimento => vencimento.data == null && vencimento.valor == null));
        return nulos == 0;
    }

    private bool verificaValorVencimentos()
    {
        bool sucesso = true;

        _titulos.ForEach(titulo =>
            {
                if (!sucesso) return;
                else if (titulo.vencimentos.Sum(vencimento => vencimento.valor.Value) != titulo.valor.Value)
                    sucesso = false;
            });
        return sucesso;
    }

    public List<string> validaFolha()
    {
        erros = new List<string>();

        if (_lanctos.Count == 0)
        {
            erros.Add("Nenhum lançamento encontrado.");
            return erros;
        }

        verificaLancamentosZerados();
        if (verificaPeriodoFechamento(_data))
        {
            if (verificaDebitoCredito())
            {
                pegaLanctosImposto();
                pegaLanctosBaseImposto();
                pegaLanctosTitulo();


                //int nulos = 0;
                //for (int i = 0; i < _titulos.Count; i++)
                //{
                //    for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                //    {
                //        if (_titulos[i].vencimentos[x].data == null && _titulos[i].vencimentos[x].valor == null)
                //            nulos++;
                //    }
                //}

                //if (nulos > 0)

                if(!verificaVencimentosZerados())
                {
                    erros.Add("Preencha as parcelas vindas do modelo.");
                }
                else if (!verificaValorVencimentos())
                {
                    erros.Add("A soma dos valores das parcelas não bate com o Valor Total informado.");
                }
                else
                {
                    if (_impostos.Count > 0)
                    {
                        if (_basesImposto.Count == 0)
                        {
                            erros.Add("Existindo uma conta de Imposto necessáriamente deve existir uma conta que seja Base de Imposto.");
                        }
                    }
                }
            }
            else
            {
                erros.Add("Total de Débito não bate com o Total de Crédito, verifique os lançamentos por favor.");
            }
        }
        else
        {
            erros.Add("Data da folha " + _data.ToString("dd/MM/yyyy") + " que está sendo lançada está fora de um período aberto.");
        }

        return erros;
    }

    public List<string> salvar(Nullable<double> alterado)
    {
        erros = new List<string>();
        decimal totalRateio = 0;
        decimal totalRateioQtd = 0;
        double lote = 0;
        verificaLancamentosZerados();

        _lanctos = _lanctos.OrderBy(x => x.seqLote).ToList();


        if (verificaPeriodoFechamento(_data))
        {
            if (!verificaDebitoCredito())
            {
                erros.Add("Total de Débito não bate com o Total de Crédito, verifique os lançamentos por favor.");
            }
            else
            {
                pegaLanctosImposto();
                pegaLanctosBaseImposto();
                pegaLanctosTitulo();

                int nulos = 0;
                for (int i = 0; i < _titulos.Count; i++)
                {
                    for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                    {
                        if (_titulos[i].vencimentos[x].data == null && _titulos[i].vencimentos[x].valor == null)
                            nulos++;
                    }
                }

                if (nulos > 0)
                {
                    erros.Add("Preencha as parcelas vindas do modelo.");
                }
                else
                {
                    if (_impostos.Count > 0)
                    {
                        if (_basesImposto.Count == 0)
                        {
                            erros.Add("Existindo uma conta de Imposto necessáriamente deve existir uma conta que seja Base de Imposto.");
                        }
                        else
                        {
                            decimal totalImpostos = 0;
                            decimal totalBaseImposto = 0;
                            int contadorDetalhamento = 1;

                            if (_modulo != "C_INCLUSAO_LANCTO")
                            {
                                SLancamento lancto1 = null;

                                for (int i = 0; i < _lanctos.Count; i++)
                                {
                                    if (_lanctos[i].seqLote == 1)
                                    {
                                        lancto1 = _lanctos[i];
                                        lancto1.valor = Math.Round(lancto1.valor.Value, 2);
                                        break;
                                    }
                                }

                                if (lancto1 == null)
                                {
                                    erros.Add("Não existe o lançamento padrão na folha.");
                                }
                                else
                                {
                                    lote = 0;
                                    if (alterado == null)
                                        lote = criaNumeroTitulo();
                                    else
                                        lote = alterado.Value;

                                    _lote = lote;

                                    for (int i = 0; i < _impostos.Count; i++)
                                    {
                                        totalImpostos += _impostos[i].valor.Value;
                                    }

                                    for (int i = 0; i < _basesImposto.Count; i++)
                                    {
                                        totalBaseImposto += _basesImposto[i].valor.Value;
                                    }

                                    //DETALHANDO LANCTO 1
                                    decimal totalDetalhamento = 0;
                                    for (int i = 0; i < _lanctos.Count; i++)
                                    {
                                        if (_lanctos[i].seqLote != 1 && !_impostos.Contains(_lanctos[i]))
                                        {
                                            if (_basesImposto.Contains(_lanctos[i]))
                                                totalDetalhamento += Math.Round(Convert.ToDecimal(_lanctos[i].valor.Value - ((totalImpostos / totalBaseImposto) * _lanctos[i].valor)), 2);
                                            else
                                                totalDetalhamento += _lanctos[i].valor.Value;
                                        }
                                    }



                                    totalRateio = 0;
                                    totalRateioQtd = 0;
                                    for (int i = 0; i < _lanctos.Count; i++)
                                    {
                                        if (_lanctos[i].seqLote != 1 && !_impostos.Contains(_lanctos[i]))
                                        {
                                            SLancamento x = new SLancamento();
                                            x.lote = lote;
                                            x.seqLote = 1;
                                            x.detLote = contadorDetalhamento;
                                            x.lotePai = 0;
                                            x.seqLotePai = 0;
                                            if (_titulos.Count == 0)
                                            {
                                                x.duplicata = 0;
                                            }
                                            else
                                            {
                                                for (int z = 0; z < _titulos.Count; z++)
                                                {
                                                    if (_titulos[z] == lancto1)
                                                    {
                                                        x.duplicata = _titulos[z].duplicata;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        x.duplicata = 0;
                                                    }
                                                }
                                            }
                                            x.seqBaixa = 0;
                                            x.detBaixa = 0;
                                            x.debCred = lancto1.debCred;

                                            if (_basesImposto.Contains(_lanctos[i]))
                                                x.valor = Math.Round(Convert.ToDecimal(((_lanctos[i].valor - ((totalImpostos / totalBaseImposto) * _lanctos[i].valor)) / totalDetalhamento) * lancto1.valor.Value), 2);
                                            else
                                                x.valor = Math.Round(Convert.ToDecimal((_lanctos[i].valor / totalDetalhamento) * lancto1.valor.Value), 2);

                                            x.qtd = Math.Round(Convert.ToDecimal(lancto1.qtd * (x.valor / lancto1.valor)), 9);
                                            x.valorUnit = Math.Round(Convert.ToDecimal(x.valor / x.qtd), 6);

                                            x.conta = lancto1.conta;
                                            x.descConta = lancto1.descConta;
                                            x.job = lancto1.job;
                                            x.descJob = lancto1.descJob;
                                            x.linhaNegocio = lancto1.linhaNegocio;
                                            x.descLinhaNegocio = lancto1.descLinhaNegocio;
                                            x.divisao = lancto1.divisao;
                                            x.descDivisao = lancto1.descDivisao;
                                            x.cliente = lancto1.cliente;
                                            x.descCliente = lancto1.descCliente;
                                            x.pendente = false;
                                            x.dataLancamento = _data;
                                            x.historico = lancto1.historico;
                                            x.titulo = lancto1.titulo;
                                            x.terceiro = lancto1.terceiro;
                                            x.descTerceiro = lancto1.descTerceiro;
                                            x.modelo = lancto1.modelo;
                                            x.numeroDocumento = lancto1.numeroDocumento;
                                            x.descConsultor = lancto1.descConsultor;
                                            x.codConsultor = lancto1.codConsultor;
                                            x.codPlanilha = lancto1.codPlanilha;
                                            x.codMoeda = lancto1.codMoeda;
                                            x.cotacao = lancto1.cotacao;
                                            contadorDetalhamento++;
                                            totalRateio += x.valor.Value;
                                            totalRateioQtd += x.qtd.Value;

                                            if (i == (_lanctos.Count - 1))
                                            {
                                                if ((lancto1.valor - totalRateio) != 0)
                                                {
                                                    x.valor += (lancto1.valor.Value - totalRateio);
                                                }

                                                if ((lancto1.qtd - totalRateioQtd) != 0)
                                                {
                                                    x.qtd += (lancto1.qtd.Value - totalRateioQtd);
                                                }
                                            }
                                            _banco.Add(x);
                                        }
                                    }

                                    //gerando os outros lançamentos
                                    for (int i = 0; i < _lanctos.Count; i++)
                                    {
                                        if (_lanctos[i].seqLote != 1 && !_impostos.Contains(_lanctos[i]))
                                        {
                                            SLancamento x = new SLancamento();
                                            x.lote = lote;
                                            x.seqLote = _lanctos[i].seqLote;
                                            x.detLote = 1;
                                            x.lotePai = 0;
                                            x.seqLotePai = 0;

                                            if (_titulos.Count == 0)
                                            {
                                                x.duplicata = 0;
                                            }
                                            else
                                            {
                                                for (int z = 0; z < _titulos.Count; z++)
                                                {
                                                    if (_titulos[z] == _lanctos[i])
                                                    {
                                                        x.duplicata = _titulos[z].duplicata;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        x.duplicata = 0;
                                                    }
                                                }
                                            }
                                            x.seqBaixa = 0;
                                            x.detBaixa = 0;
                                            x.debCred = _lanctos[i].debCred;
                                            x.valor = _lanctos[i].valor;
                                            x.valorUnit = _lanctos[i].valorUnit;
                                            x.qtd = _lanctos[i].qtd;
                                            x.conta = _lanctos[i].conta;
                                            x.descConta = _lanctos[i].descConta;
                                            x.job = _lanctos[i].job;
                                            x.descJob = _lanctos[i].descJob;
                                            x.linhaNegocio = _lanctos[i].linhaNegocio;
                                            x.descLinhaNegocio = _lanctos[i].descLinhaNegocio;
                                            x.divisao = _lanctos[i].divisao;
                                            x.descDivisao = _lanctos[i].descDivisao;
                                            x.cliente = _lanctos[i].cliente;
                                            x.descCliente = _lanctos[i].descCliente;
                                            x.pendente = false;
                                            x.dataLancamento = _data;
                                            x.historico = _lanctos[i].historico;
                                            x.titulo = _lanctos[i].titulo;
                                            x.terceiro = _lanctos[i].terceiro;
                                            x.descTerceiro = _lanctos[i].descTerceiro;
                                            x.modelo = _lanctos[i].modelo;
                                            x.numeroDocumento = _lanctos[i].numeroDocumento;
                                            x.descConsultor = _lanctos[i].descConsultor;
                                            x.codConsultor = _lanctos[i].codConsultor;
                                            x.codPlanilha = _lanctos[i].codPlanilha;
                                            x.codMoeda = _lanctos[i].codMoeda;
                                            x.cotacao = _lanctos[i].cotacao;
                                            _banco.Add(x);
                                        }
                                    }

                                    totalRateio = 0;
                                    totalRateioQtd = 0;
                                    //gerando os lançamentos de imposto
                                    for (int i = 0; i < _impostos.Count; i++)
                                    {
                                        totalRateio = 0;
                                        totalRateioQtd = 0;
                                        _impostos[i].valor = Math.Round(_impostos[i].valor.Value, 2);
                                        for (int x = 0; x < _basesImposto.Count; x++)
                                        {
                                            SLancamento l = new SLancamento();
                                            l.lote = lote;
                                            l.seqLote = _impostos[i].seqLote;
                                            l.detLote = (x + 1);
                                            l.lotePai = 0;
                                            l.seqLotePai = 0;
                                            if (_titulos.Count == 0)
                                            {
                                                l.duplicata = 0;
                                            }
                                            else
                                            {
                                                for (int z = 0; z < _titulos.Count; z++)
                                                {
                                                    if (_titulos[z] == _impostos[i])
                                                    {
                                                        l.duplicata = _titulos[z].duplicata;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        l.duplicata = 0;
                                                    }
                                                }
                                            }
                                            l.seqBaixa = 0;
                                            l.detBaixa = 0;
                                            l.debCred = _impostos[i].debCred;

                                            l.valor = Math.Round(Convert.ToDecimal((_basesImposto[x].valor / totalBaseImposto) * _impostos[i].valor), 2);


                                            l.qtd = Math.Round(Convert.ToDecimal(_impostos[i].qtd * (l.valor / _impostos[i].valor)), 9);
                                            l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);

                                            l.conta = _impostos[i].conta;
                                            l.descConta = _impostos[i].descConta;
                                            l.job = _impostos[i].job;
                                            l.descJob = _impostos[i].descJob;
                                            l.linhaNegocio = _impostos[i].linhaNegocio;
                                            l.descLinhaNegocio = _impostos[i].descLinhaNegocio;
                                            l.divisao = _impostos[i].divisao;
                                            l.descDivisao = _impostos[i].descDivisao;
                                            l.cliente = _impostos[i].cliente;
                                            l.descCliente = _impostos[i].descCliente;
                                            l.pendente = false;
                                            l.dataLancamento = _data;
                                            l.historico = _impostos[i].historico;
                                            l.titulo = _impostos[i].titulo;
                                            l.terceiro = _impostos[i].terceiro;
                                            l.descTerceiro = _impostos[i].descTerceiro;
                                            l.modelo = _impostos[i].modelo;
                                            l.numeroDocumento = _impostos[i].numeroDocumento;
                                            l.descConsultor = _impostos[i].descConsultor;
                                            l.codConsultor = _impostos[i].codConsultor;
                                            l.codPlanilha = _impostos[i].codPlanilha;
                                            l.codMoeda = _impostos[i].codMoeda;
                                            l.cotacao = _impostos[i].cotacao;
                                            totalRateio += l.valor.Value;
                                            totalRateioQtd += l.qtd.Value;

                                            if (x == (_basesImposto.Count - 1))
                                            {
                                                if ((_impostos[i].valor - totalRateio) != 0)
                                                {
                                                    l.valor += (_impostos[i].valor - totalRateio);
                                                }

                                                if ((_impostos[i].qtd - totalRateioQtd) != 0)
                                                {
                                                    l.qtd += (_impostos[i].qtd - totalRateioQtd);
                                                }
                                            }
                                            _banco.Add(l);
                                        }
                                    }

                                    //INSERE COTAÇÃO DO LOTE
                                    if (_banco.Count > 0)
                                    {
                                        if (cotacaoItemLote != null)
                                        {
                                            lanctoContabDAO.insertCotacao(_banco[0].lote.Value, 0, cotacaoItemLote);
                                        }
                                    }

                                    //INSERE LANCTOS NA BASE
                                    for (int i = 0; i < _banco.Count; i++)
                                    {
                                        SLancamento l = _banco[i];
                                        lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                            l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                            l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, _modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);

                                        if (l.cotacao != null)
                                        {
                                            lanctoContabDAO.insertCotacao(l.lote.Value, l.seqLote, l.cotacao);
                                        }
                                    }

                                    //VERIFICO SE O LANÇAMENTO É NO MOEDA, CASO SEJA NÃO GERA TÍTULOS
                                    if (_banco.Count > 0 && _banco[0].codMoeda == 0)
                                    {
                                        // ZERO O ARRAY DE LANÇAMENTOS
                                        _banco.Clear();

                                        //GERA TITULOS
                                        for (int i = 0; i < _titulos.Count; i++)
                                        {
                                            for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                                            {
                                                double novoLote = criaNumeroTitulo();
                                                int detalhamentoLancto1 = 1;
                                                int detalhamentoImposto = 1;
                                                //SE FOR O PRIMEIRO LANÇAMENTO
                                                if (_titulos[i].seqLote == 1 && !_impostos.Contains(_titulos[i]))
                                                {
                                                    totalRateio = 0;
                                                    totalRateioQtd = 0;
                                                    for (int y = 0; y < _lanctos.Count; y++)
                                                    {
                                                        SLancamento l = null;
                                                        if (_lanctos[y].seqLote != 1 && !_impostos.Contains(_lanctos[y]))
                                                        {
                                                            l = new SLancamento();
                                                            l.lote = novoLote;
                                                            l.seqLote = 1;
                                                            l.detLote = detalhamentoLancto1;
                                                            l.lotePai = lote;
                                                            l.seqLotePai = _titulos[i].seqLote;
                                                            l.duplicata = _titulos[i].duplicata;
                                                            l.seqBaixa = 0;
                                                            l.detBaixa = 0;
                                                            if (_titulos[i].debCred == 'C')
                                                            {
                                                                l.debCred = 'D';
                                                                l.modulo = "CAP_INCLUSAO_TITULO";
                                                            }
                                                            else
                                                            {
                                                                l.debCred = 'C';
                                                                l.modulo = "CAR_INCLUSAO_TITULO";
                                                            }

                                                            if (_basesImposto.Contains(_lanctos[y]))
                                                                l.valor = Math.Round(Convert.ToDecimal((_titulos[i].vencimentos[x].valor / _titulos[i].valor) * (((_lanctos[y].valor - ((totalImpostos / totalBaseImposto) * _lanctos[y].valor)) / totalDetalhamento) * lancto1.valor.Value)), 2);
                                                            else
                                                                l.valor = Math.Round(Convert.ToDecimal((_titulos[i].vencimentos[x].valor / _titulos[i].valor) * ((_lanctos[y].valor / totalDetalhamento) * lancto1.valor.Value)), 2);


                                                            l.qtd = Math.Round(Convert.ToDecimal(lancto1.qtd * (l.valor / lancto1.valor)), 9);
                                                            l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);

                                                            l.conta = _titulos[i].conta;
                                                            l.descConta = _titulos[i].descConta;
                                                            l.job = _titulos[i].job;
                                                            l.descJob = _titulos[i].descJob;
                                                            l.linhaNegocio = _titulos[i].linhaNegocio;
                                                            l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                            l.divisao = _titulos[i].divisao;
                                                            l.descDivisao = _titulos[i].descDivisao;
                                                            l.cliente = _titulos[i].cliente;
                                                            l.descCliente = _titulos[i].descCliente;
                                                            l.pendente = true;
                                                            l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                            l.historico = _titulos[i].historico;
                                                            l.titulo = false;
                                                            l.terceiro = _titulos[i].terceiro;
                                                            l.descTerceiro = _titulos[i].descTerceiro;
                                                            l.modelo = _titulos[i].modelo;
                                                            l.numeroDocumento = _titulos[i].numeroDocumento;
                                                            l.descConsultor = _titulos[i].descConsultor;
                                                            l.codConsultor = _titulos[i].codConsultor;
                                                            l.codPlanilha = _titulos[i].codPlanilha;
                                                            totalRateio += l.valor.Value;
                                                            totalRateioQtd += l.qtd.Value;


                                                            detalhamentoLancto1++;
                                                        }

                                                        if (y == (_lanctos.Count - 1))
                                                        {
                                                            if (l != null)
                                                            {
                                                                if ((_titulos[i].vencimentos[x].valor - totalRateio) != 0)
                                                                {
                                                                    l.valor += (_titulos[i].vencimentos[x].valor - totalRateio);
                                                                }

                                                                if ((lancto1.qtd - totalRateioQtd) != 0)
                                                                {
                                                                    l.qtd += (lancto1.qtd.Value - totalRateioQtd);
                                                                }
                                                            }
                                                        }

                                                        if (l != null)
                                                        {
                                                            _banco.Add(l);
                                                        }
                                                    }

                                                }
                                                //SE FOR LANÇAMENTO DO TIPO IMPOSTO
                                                else if (_impostos.Contains(_titulos[i]))
                                                {
                                                    totalRateio = 0;
                                                    totalRateioQtd = 0;
                                                    for (int y = 0; y < _basesImposto.Count; y++)
                                                    {
                                                        SLancamento l = new SLancamento();
                                                        l.lote = novoLote;
                                                        l.seqLote = 1;
                                                        l.detLote = detalhamentoImposto;
                                                        l.lotePai = lote;
                                                        l.seqLotePai = _titulos[i].seqLote;
                                                        l.duplicata = _titulos[i].duplicata;
                                                        l.seqBaixa = 0;
                                                        l.detBaixa = 0;
                                                        if (_titulos[i].debCred == 'C')
                                                        {
                                                            l.debCred = 'D';
                                                            l.modulo = "CAP_INCLUSAO_TITULO";
                                                        }
                                                        else
                                                        {
                                                            l.debCred = 'C';
                                                            l.modulo = "CAR_INCLUSAO_TITULO";
                                                        }

                                                        l.valor = Math.Round(Convert.ToDecimal((_basesImposto[y].valor / totalBaseImposto) * _titulos[i].vencimentos[x].valor), 2);

                                                        l.qtd = Math.Round(Convert.ToDecimal(_titulos[i].qtd * (l.valor / _titulos[i].valor)), 9);
                                                        l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);

                                                        l.conta = _titulos[i].conta;
                                                        l.descConta = _titulos[i].descConta;
                                                        l.job = _titulos[i].job;
                                                        l.descJob = _titulos[i].descJob;
                                                        l.linhaNegocio = _titulos[i].linhaNegocio;
                                                        l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                        l.divisao = _titulos[i].divisao;
                                                        l.descDivisao = _titulos[i].descDivisao;
                                                        l.cliente = _titulos[i].cliente;
                                                        l.descCliente = _titulos[i].descCliente;
                                                        l.pendente = true;
                                                        l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                        l.historico = _titulos[i].historico;
                                                        l.titulo = false;
                                                        l.terceiro = _titulos[i].terceiro;
                                                        l.descTerceiro = _titulos[i].descTerceiro;
                                                        l.modelo = _titulos[i].modelo;
                                                        l.numeroDocumento = _titulos[i].numeroDocumento;
                                                        l.descConsultor = _titulos[i].descConsultor;
                                                        l.codConsultor = _titulos[i].codConsultor;
                                                        l.codPlanilha = _titulos[i].codPlanilha;
                                                        totalRateio += l.valor.Value;
                                                        totalRateioQtd += l.qtd.Value;

                                                        if (y == (_basesImposto.Count - 1))
                                                        {

                                                            if ((_titulos[i].vencimentos[x].valor - totalRateio) != 0)
                                                            {
                                                                l.valor += (_titulos[i].vencimentos[x].valor - totalRateio);
                                                            }

                                                            if ((_titulos[i].qtd - totalRateioQtd) != 0)
                                                            {
                                                                l.qtd += (_titulos[i].qtd - totalRateioQtd);
                                                            }
                                                        }

                                                        detalhamentoImposto++;
                                                        _banco.Add(l);
                                                    }
                                                }
                                                //SE FOR LANÇAMENTO NORMAL
                                                else
                                                {
                                                    SLancamento l = new SLancamento();
                                                    l.lote = novoLote;
                                                    l.seqLote = 1;
                                                    l.detLote = 1;
                                                    l.lotePai = lote;
                                                    l.seqLotePai = _titulos[i].seqLote;
                                                    l.duplicata = _titulos[i].duplicata;
                                                    l.seqBaixa = 0;
                                                    l.detBaixa = 0;
                                                    if (_titulos[i].debCred == 'C')
                                                    {
                                                        l.debCred = 'D';
                                                        l.modulo = "CAP_INCLUSAO_TITULO";
                                                    }
                                                    else
                                                    {
                                                        l.debCred = 'C';
                                                        l.modulo = "CAR_INCLUSAO_TITULO";
                                                    }

                                                    l.valor = _titulos[i].vencimentos[x].valor;

                                                    l.valorUnit = _titulos[i].valorUnit;
                                                    l.qtd = 1;

                                                    l.conta = _titulos[i].conta;
                                                    l.descConta = _titulos[i].descConta;
                                                    l.job = _titulos[i].job;
                                                    l.descJob = _titulos[i].descJob;
                                                    l.linhaNegocio = _titulos[i].linhaNegocio;
                                                    l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                    l.divisao = _titulos[i].divisao;
                                                    l.descDivisao = _titulos[i].descDivisao;
                                                    l.cliente = _titulos[i].cliente;
                                                    l.descCliente = _titulos[i].descCliente;
                                                    l.pendente = true;
                                                    l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                    l.historico = _titulos[i].historico;
                                                    l.titulo = false;
                                                    l.terceiro = _titulos[i].terceiro;
                                                    l.descTerceiro = _titulos[i].descTerceiro;
                                                    l.modelo = _titulos[i].modelo;
                                                    l.numeroDocumento = _titulos[i].numeroDocumento;
                                                    l.descConsultor = _titulos[i].descConsultor;
                                                    l.codConsultor = _titulos[i].codConsultor;
                                                    l.codPlanilha = _titulos[i].codPlanilha;

                                                    _banco.Add(l);
                                                }
                                            }
                                        } //FIM GERA TITULO 

                                        //INSERE LANCTOS NA BASE
                                        for (int i = 0; i < _banco.Count; i++)
                                        {
                                            SLancamento l = _banco[i];
                                            lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                                l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                                l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, l.modulo, l.modelo.Value, l.numeroDocumento,
                                                l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);
                                        }
                                    }
                                    else
                                    {
                                        _banco.Clear();
                                    } //FIM 
                                }
                            }
                            else
                            {
                                //SE ESTIVER NO MÓDULO DE CONTABILIDADE

                                lote = 0;
                                if (alterado == null)
                                    lote = criaNumeroTitulo();
                                else
                                    lote = alterado.Value;
                                _lote = lote;

                                for (int i = 0; i < _impostos.Count; i++)
                                {
                                    totalImpostos += _impostos[i].valor.Value;
                                }

                                for (int i = 0; i < _basesImposto.Count; i++)
                                {
                                    totalBaseImposto += _basesImposto[i].valor.Value;
                                }



                                //gerando os lançamentos
                                for (int i = 0; i < _lanctos.Count; i++)
                                {
                                    if (!_impostos.Contains(_lanctos[i]))
                                    {
                                        SLancamento x = new SLancamento();
                                        x.lote = lote;
                                        x.seqLote = _lanctos[i].seqLote;
                                        x.detLote = 1;
                                        x.lotePai = 0;
                                        x.seqLotePai = 0;
                                        if (_titulos.Count == 0)
                                        {
                                            x.duplicata = 0;
                                        }
                                        else
                                        {
                                            for (int z = 0; z < _titulos.Count; z++)
                                            {
                                                if (_titulos[z] == _lanctos[i])
                                                {
                                                    x.duplicata = _titulos[z].duplicata;
                                                    break;
                                                }
                                                else
                                                {
                                                    x.duplicata = 0;
                                                }
                                            }
                                        }
                                        x.seqBaixa = 0;
                                        x.detBaixa = 0;
                                        x.debCred = _lanctos[i].debCred;
                                        x.valor = _lanctos[i].valor;
                                        x.valorUnit = _lanctos[i].valorUnit;
                                        x.qtd = _lanctos[i].qtd;
                                        x.conta = _lanctos[i].conta;
                                        x.descConta = _lanctos[i].descConta;
                                        x.job = _lanctos[i].job;
                                        x.descJob = _lanctos[i].descJob;
                                        x.linhaNegocio = _lanctos[i].linhaNegocio;
                                        x.descLinhaNegocio = _lanctos[i].descLinhaNegocio;
                                        x.divisao = _lanctos[i].divisao;
                                        x.descDivisao = _lanctos[i].descDivisao;
                                        x.cliente = _lanctos[i].cliente;
                                        x.descCliente = _lanctos[i].descCliente;
                                        x.pendente = false;
                                        x.dataLancamento = _data;
                                        x.historico = _lanctos[i].historico;
                                        x.titulo = _lanctos[i].titulo;
                                        x.terceiro = _lanctos[i].terceiro;
                                        x.descTerceiro = _lanctos[i].descTerceiro;
                                        x.modelo = _lanctos[i].modelo;
                                        x.numeroDocumento = _lanctos[i].numeroDocumento;
                                        x.descConsultor = _lanctos[i].descConsultor;
                                        x.codConsultor = _lanctos[i].codConsultor;
                                        x.codPlanilha = _lanctos[i].codPlanilha;
                                        _banco.Add(x);
                                    }
                                }

                                //gerando os lançamentos de imposto
                                totalRateio = 0;
                                totalRateioQtd = 0;

                                for (int i = 0; i < _impostos.Count; i++)
                                {
                                    totalRateio = 0;
                                    totalRateioQtd = 0;
                                    _impostos[i].valor = Math.Round(_impostos[i].valor.Value, 2);
                                    for (int x = 0; x < _basesImposto.Count; x++)
                                    {
                                        SLancamento l = new SLancamento();
                                        l.lote = lote;
                                        l.seqLote = _impostos[i].seqLote;
                                        l.detLote = (x + 1);
                                        l.lotePai = 0;
                                        l.seqLotePai = 0;
                                        if (_titulos.Count == 0)
                                        {
                                            l.duplicata = 0;
                                        }
                                        else
                                        {
                                            for (int z = 0; z < _titulos.Count; z++)
                                            {
                                                if (_titulos[z] == _impostos[i])
                                                {
                                                    l.duplicata = _titulos[z].duplicata;
                                                    break;
                                                }
                                                else
                                                {
                                                    l.duplicata = 0;
                                                }
                                            }
                                        }
                                        l.seqBaixa = 0;
                                        l.detBaixa = 0;
                                        l.debCred = _impostos[i].debCred;

                                        l.valor = Math.Round(Convert.ToDecimal((_basesImposto[x].valor / totalBaseImposto) * _impostos[i].valor), 2);


                                        l.qtd = Math.Round(Convert.ToDecimal(_impostos[i].qtd * (l.valor / _impostos[i].valor)), 9);
                                        l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);

                                        l.conta = _impostos[i].conta;
                                        l.descConta = _impostos[i].descConta;
                                        l.job = _impostos[i].job;
                                        l.descJob = _impostos[i].descJob;
                                        l.linhaNegocio = _impostos[i].linhaNegocio;
                                        l.descLinhaNegocio = _impostos[i].descLinhaNegocio;
                                        l.divisao = _impostos[i].divisao;
                                        l.descDivisao = _impostos[i].descDivisao;
                                        l.cliente = _impostos[i].cliente;
                                        l.descCliente = _impostos[i].descCliente;
                                        l.pendente = false;
                                        l.dataLancamento = _data;
                                        l.historico = _impostos[i].historico;
                                        l.titulo = _impostos[i].titulo;
                                        l.terceiro = _impostos[i].terceiro;
                                        l.descTerceiro = _impostos[i].descTerceiro;
                                        l.modelo = _impostos[i].modelo;
                                        l.numeroDocumento = _impostos[i].numeroDocumento;
                                        l.descConsultor = _impostos[i].descConsultor;
                                        l.codConsultor = _impostos[i].codConsultor;
                                        l.codPlanilha = _impostos[i].codPlanilha;
                                        totalRateio += l.valor.Value;
                                        totalRateioQtd += l.qtd.Value;

                                        if (x == (_basesImposto.Count - 1))
                                        {
                                            if ((_impostos[i].valor - totalRateio) != 0)
                                            {
                                                l.valor += (_impostos[i].valor - totalRateio);
                                            }

                                            if ((_impostos[i].qtd - totalRateioQtd) != 0)
                                            {
                                                l.qtd += (_impostos[i].qtd - totalRateioQtd);
                                            }
                                        }
                                        _banco.Add(l);
                                    }
                                }

                                //INSERE LANCTOS NA BASE
                                for (int i = 0; i < _banco.Count; i++)
                                {
                                    SLancamento l = _banco[i];
                                    lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                        l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                        l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, _modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);
                                }

                                _banco.Clear();

                                //GERA TITULOS
                                for (int i = 0; i < _titulos.Count; i++)
                                {
                                    for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                                    {
                                        double novoLote = criaNumeroTitulo();
                                        int detalhamentoImposto = 1;

                                        //SE FOR LANÇAMENTO DO TIPO IMPOSTO
                                        if (_impostos.Contains(_titulos[i]))
                                        {
                                            totalRateio = 0;
                                            totalRateioQtd = 0;
                                            for (int y = 0; y < _basesImposto.Count; y++)
                                            {
                                                SLancamento l = new SLancamento();
                                                l.lote = novoLote;
                                                l.seqLote = 1;
                                                l.detLote = detalhamentoImposto;
                                                l.lotePai = lote;
                                                l.seqLotePai = _titulos[i].seqLote;
                                                l.duplicata = _titulos[i].duplicata;
                                                l.seqBaixa = 0;
                                                l.detBaixa = 0;
                                                if (_titulos[i].debCred == 'C')
                                                {
                                                    l.debCred = 'D';
                                                    l.modulo = "CAP_INCLUSAO_TITULO";
                                                }
                                                else
                                                {
                                                    l.debCred = 'C';
                                                    l.modulo = "CAR_INCLUSAO_TITULO";
                                                }

                                                l.valor = Math.Round(Convert.ToDecimal((_basesImposto[y].valor / totalBaseImposto) * _titulos[i].vencimentos[x].valor), 2);

                                                l.qtd = Math.Round(Convert.ToDecimal(_titulos[i].qtd * (l.valor / _titulos[i].valor)), 9);
                                                l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);

                                                l.conta = _titulos[i].conta;
                                                l.descConta = _titulos[i].descConta;
                                                l.job = _titulos[i].job;
                                                l.descJob = _titulos[i].descJob;
                                                l.linhaNegocio = _titulos[i].linhaNegocio;
                                                l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                l.divisao = _titulos[i].divisao;
                                                l.descDivisao = _titulos[i].descDivisao;
                                                l.cliente = _titulos[i].cliente;
                                                l.descCliente = _titulos[i].descCliente;
                                                l.pendente = true;
                                                l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                l.historico = _titulos[i].historico;
                                                l.titulo = false;
                                                l.terceiro = _titulos[i].terceiro;
                                                l.descTerceiro = _titulos[i].descTerceiro;
                                                l.modelo = _titulos[i].modelo;
                                                l.numeroDocumento = _titulos[i].numeroDocumento;
                                                l.descConsultor = _titulos[i].descConsultor;
                                                l.codConsultor = _titulos[i].codConsultor;
                                                l.codPlanilha = _titulos[1].codConsultor;

                                                totalRateio += l.valor.Value;
                                                totalRateioQtd += l.qtd.Value;

                                                if (y == (_basesImposto.Count - 1))
                                                {

                                                    if ((_titulos[i].vencimentos[x].valor - totalRateio) != 0)
                                                    {
                                                        l.valor += (_titulos[i].vencimentos[x].valor - totalRateio);
                                                    }

                                                    if ((_titulos[i].qtd - totalRateioQtd) != 0)
                                                    {
                                                        l.qtd += (_titulos[i].qtd - totalRateioQtd);
                                                    }
                                                }

                                                detalhamentoImposto++;
                                                _banco.Add(l);
                                            }
                                        }
                                        //SE FOR LANÇAMENTO NORMAL
                                        else
                                        {
                                            SLancamento l = new SLancamento();
                                            l.lote = novoLote;
                                            l.seqLote = 1;
                                            l.detLote = 1;
                                            l.lotePai = lote;
                                            l.seqLotePai = _titulos[i].seqLote;
                                            l.duplicata = _titulos[i].duplicata;
                                            l.seqBaixa = 0;
                                            l.detBaixa = 0;
                                            if (_titulos[i].debCred == 'C')
                                            {
                                                l.debCred = 'D';
                                                l.modulo = "CAP_INCLUSAO_TITULO";
                                            }
                                            else
                                            {
                                                l.debCred = 'C';
                                                l.modulo = "CAR_INCLUSAO_TITULO";
                                            }

                                            l.valor = _titulos[i].vencimentos[x].valor;

                                            l.valorUnit = l.valor;
                                            l.qtd = 1;

                                            l.conta = _titulos[i].conta;
                                            l.descConta = _titulos[i].descConta;
                                            l.job = _titulos[i].job;
                                            l.descJob = _titulos[i].descJob;
                                            l.linhaNegocio = _titulos[i].linhaNegocio;
                                            l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                            l.divisao = _titulos[i].divisao;
                                            l.descDivisao = _titulos[i].descDivisao;
                                            l.cliente = _titulos[i].cliente;
                                            l.descCliente = _titulos[i].descCliente;
                                            l.pendente = true;
                                            l.dataLancamento = _titulos[i].vencimentos[x].data;
                                            l.historico = _titulos[i].historico;
                                            l.titulo = false;
                                            l.terceiro = _titulos[i].terceiro;
                                            l.descTerceiro = _titulos[i].descTerceiro;
                                            l.modelo = _titulos[i].modelo;
                                            l.numeroDocumento = _titulos[i].numeroDocumento;
                                            l.descConsultor = _titulos[i].descConsultor;
                                            l.codConsultor = _titulos[i].codConsultor;
                                            l.codPlanilha = _titulos[i].codPlanilha;
                                            _banco.Add(l);
                                        }
                                    }
                                }

                                //INSERE LANCTOS NA BASE
                                for (int i = 0; i < _banco.Count; i++)
                                {
                                    SLancamento l = _banco[i];
                                    lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                        l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                        l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, l.modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);
                                }
                            }
                        }
                    }
                    else
                    {
                        //SE NÃO ENCONTRAR IMPOSTOS NA FOLHA

                        if (_modulo != "C_INCLUSAO_LANCTO")
                        {
                            SLancamento lancto1 = null;

                            for (int i = 0; i < _lanctos.Count; i++)
                            {
                                if (_lanctos[i].seqLote == 1)
                                {
                                    lancto1 = _lanctos[i];
                                    lancto1.valor = Math.Round(lancto1.valor.Value, 2);
                                    break;
                                }
                            }

                            if (lancto1 == null)
                            {
                                erros.Add("Não existe o lançamento padrão na folha.");
                            }
                            else
                            {
                                lote = 0;
                                if (alterado == null)
                                    lote = criaNumeroTitulo();
                                else
                                    lote = alterado.Value;
                                _lote = lote;

                                int contadorDetalhamento = 1;
                                //DETALHANDO LANCTO 1
                                decimal totalDetalhamento = 0;
                                for (int i = 0; i < _lanctos.Count; i++)
                                {
                                    if (_lanctos[i].seqLote != 1 && !_impostos.Contains(_lanctos[i]))
                                    {
                                        totalDetalhamento += _lanctos[i].valor.Value;
                                    }
                                }

                                totalRateioQtd = 0;
                                totalRateio = 0;
                                for (int i = 0; i < _lanctos.Count; i++)
                                {
                                    if (_lanctos[i].seqLote != 1)
                                    {
                                        SLancamento x = new SLancamento();
                                        x.lote = lote;
                                        x.seqLote = 1;
                                        x.detLote = contadorDetalhamento;
                                        x.lotePai = 0;
                                        x.seqLotePai = 0;
                                        if (_titulos.Count == 0)
                                        {
                                            x.duplicata = 0;
                                        }
                                        else
                                        {
                                            for (int z = 0; z < _titulos.Count; z++)
                                            {
                                                if (_titulos[z] == lancto1)
                                                {
                                                    x.duplicata = _titulos[z].duplicata;
                                                    break;
                                                }
                                                else
                                                {
                                                    x.duplicata = 0;
                                                }
                                            }
                                        }
                                        x.seqBaixa = 0;
                                        x.detBaixa = 0;
                                        x.debCred = lancto1.debCred;
                                        x.valor = Math.Round(Convert.ToDecimal((_lanctos[i].valor / totalDetalhamento) * lancto1.valor.Value), 2);

                                        x.qtd = Math.Round(Convert.ToDecimal(lancto1.qtd * (x.valor / lancto1.valor)), 9);
                                        x.valorUnit = Math.Round(Convert.ToDecimal(x.valor / x.qtd), 6);
                                        x.valorBruto = lancto1.valorBruto;
                                        x.codMoeda = lancto1.codMoeda;
                                        x.cotacao = lancto1.cotacao;
                                        x.conta = lancto1.conta;

                                        x.descConta = lancto1.descConta;
                                        x.job = lancto1.job;
                                        x.descJob = lancto1.descJob;
                                        x.linhaNegocio = lancto1.linhaNegocio;
                                        x.descLinhaNegocio = lancto1.descLinhaNegocio;
                                        x.divisao = lancto1.divisao;
                                        x.descDivisao = lancto1.descDivisao;
                                        x.cliente = lancto1.cliente;
                                        x.descCliente = lancto1.descCliente;
                                        x.pendente = false;
                                        x.dataLancamento = _data;
                                        x.historico = lancto1.historico;
                                        x.titulo = lancto1.titulo;
                                        x.terceiro = lancto1.terceiro;
                                        x.descTerceiro = lancto1.descTerceiro;
                                        x.modelo = lancto1.modelo;
                                        x.numeroDocumento = lancto1.numeroDocumento;
                                        x.descConsultor = lancto1.descConsultor;
                                        x.codConsultor = lancto1.codConsultor;
                                        x.codPlanilha = lancto1.codPlanilha;

                                        totalRateio += x.valor.Value;
                                        totalRateioQtd += x.qtd.Value;

                                        if (i == (_lanctos.Count - 1))
                                        {
                                            if ((lancto1.valor - totalRateio) != 0)
                                            {
                                                x.valor += (lancto1.valor.Value - totalRateio);
                                            }

                                            if ((lancto1.qtd - totalRateioQtd) != 0)
                                            {
                                                x.qtd += (lancto1.qtd.Value - totalRateioQtd);
                                            }
                                        }
                                        contadorDetalhamento++;
                                        _banco.Add(x);
                                    }
                                }

                                //processa outros lancamentos
                                for (int i = 0; i < _lanctos.Count; i++)
                                {
                                    if (_lanctos[i].seqLote != 1)
                                    {
                                        SLancamento x = new SLancamento();
                                        x.lote = lote;
                                        x.seqLote = _lanctos[i].seqLote;
                                        x.detLote = 1;
                                        x.lotePai = 0;
                                        x.seqLotePai = 0;
                                        if (_titulos.Count == 0)
                                        {
                                            x.duplicata = 0;
                                        }
                                        else
                                        {
                                            for (int z = 0; z < _titulos.Count; z++)
                                            {
                                                if (_titulos[z] == _lanctos[i])
                                                {
                                                    x.duplicata = _titulos[z].duplicata;
                                                    break;
                                                }
                                                else
                                                {
                                                    x.duplicata = 0;
                                                }
                                            }
                                        }
                                        x.seqBaixa = 0;
                                        x.detBaixa = 0;
                                        x.debCred = _lanctos[i].debCred;
                                        x.valor = _lanctos[i].valor;
                                        x.valorUnit = _lanctos[i].valorUnit;
                                        x.valorBruto = _lanctos[i].valorBruto;
                                        x.codMoeda = _lanctos[i].codMoeda;
                                        x.cotacao = _lanctos[i].cotacao;
                                        x.qtd = _lanctos[i].qtd;
                                        x.conta = _lanctos[i].conta;
                                        x.descConta = _lanctos[i].descConta;
                                        x.job = _lanctos[i].job;
                                        x.descJob = _lanctos[i].descJob;
                                        x.linhaNegocio = _lanctos[i].linhaNegocio;
                                        x.descLinhaNegocio = _lanctos[i].descLinhaNegocio;
                                        x.divisao = _lanctos[i].divisao;
                                        x.descDivisao = _lanctos[i].descDivisao;
                                        x.cliente = _lanctos[i].cliente;
                                        x.descCliente = _lanctos[i].descCliente;
                                        x.pendente = false;
                                        x.dataLancamento = _data;
                                        x.historico = _lanctos[i].historico;
                                        x.titulo = _lanctos[i].titulo;
                                        x.terceiro = _lanctos[i].terceiro;
                                        x.descTerceiro = _lanctos[i].descTerceiro;
                                        x.modelo = _lanctos[i].modelo;
                                        x.numeroDocumento = _lanctos[i].numeroDocumento;
                                        x.descConsultor = _lanctos[i].descConsultor;
                                        x.codConsultor = _lanctos[i].codConsultor;
                                        x.codPlanilha = _lanctos[i].codPlanilha;
                                        _banco.Add(x);
                                    }
                                }

                                //INSERE COTAÇÃO DO LOTE
                                if (_banco.Count > 0)
                                {
                                    if (cotacaoItemLote != null)
                                    {
                                        lanctoContabDAO.insertCotacao(_banco[0].lote.Value, 0, cotacaoItemLote);
                                    }
                                }

                                //INSERE LANCTOS NA BASE
                                for (int i = 0; i < _banco.Count; i++)
                                {
                                    SLancamento l = _banco[i];
                                    lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                        l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                        l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, _modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, l.valorBruto.Value, l.codMoeda);

                                    if (l.cotacao != null)
                                    {
                                        lanctoContabDAO.insertCotacao(l.lote.Value, l.seqLote, l.cotacao);
                                    }
                                }

                                //VERIFICO SE O LANÇAMENTO É NO MOEDA, CASO SEJA NÃO GERA TÍTULOS
                                if (_banco.Count > 0 && _banco[0].codMoeda == 0)
                                {
                                    // ZERO O ARRAY DE LANÇAMENTOS
                                    _banco.Clear();

                                    //GERA TITULOS
                                    for (int i = 0; i < _titulos.Count; i++)
                                    {
                                        for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                                        {
                                            double novoLote = criaNumeroTitulo();
                                            int detalhamentoLancto1 = 1;
                                            //SE FOR O PRIMEIRO LANÇAMENTO
                                            if (_titulos[i].seqLote == 1)
                                            {

                                                totalRateio = 0;
                                                totalRateioQtd = 0;
                                                for (int y = 0; y < _lanctos.Count; y++)
                                                {
                                                    SLancamento l = null;
                                                    if (_lanctos[y].seqLote != 1)
                                                    {
                                                        l = new SLancamento();
                                                        l.lote = novoLote;
                                                        l.seqLote = 1;
                                                        l.detLote = detalhamentoLancto1;
                                                        l.lotePai = lote;
                                                        l.seqLotePai = _titulos[i].seqLote;
                                                        l.duplicata = _titulos[i].duplicata;
                                                        l.seqBaixa = 0;
                                                        l.detBaixa = 0;
                                                        if (_titulos[i].debCred == 'C')
                                                        {
                                                            l.debCred = 'D';
                                                            l.modulo = "CAP_INCLUSAO_TITULO";
                                                        }
                                                        else
                                                        {
                                                            l.debCred = 'C';
                                                            l.modulo = "CAR_INCLUSAO_TITULO";
                                                        }

                                                        l.valor = Math.Round(Convert.ToDecimal((_titulos[i].vencimentos[x].valor / _titulos[i].valor) * ((_lanctos[y].valor / totalDetalhamento) * lancto1.valor.Value)), 2);


                                                        l.qtd = Math.Round(Convert.ToDecimal(lancto1.qtd * (l.valor / lancto1.valor)), 9);
                                                        l.valorUnit = Math.Round(Convert.ToDecimal(l.valor / l.qtd), 6);
                                                        l.valorBruto = _titulos[i].valorBruto;
                                                        l.conta = _titulos[i].conta;
                                                        l.descConta = _titulos[i].descConta;
                                                        l.job = _titulos[i].job;
                                                        l.descJob = _titulos[i].descJob;
                                                        l.linhaNegocio = _titulos[i].linhaNegocio;
                                                        l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                        l.divisao = _titulos[i].divisao;
                                                        l.descDivisao = _titulos[i].descDivisao;
                                                        l.cliente = _titulos[i].cliente;
                                                        l.descCliente = _titulos[i].descCliente;
                                                        l.pendente = true;
                                                        l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                        l.historico = _titulos[i].historico;
                                                        l.titulo = false;
                                                        l.terceiro = _titulos[i].terceiro;
                                                        l.descTerceiro = _titulos[i].descTerceiro;
                                                        l.modelo = _titulos[i].modelo;
                                                        l.numeroDocumento = _titulos[i].numeroDocumento;
                                                        l.descConsultor = _titulos[i].descConsultor;
                                                        l.codConsultor = _titulos[i].codConsultor;
                                                        l.codPlanilha = _titulos[i].codPlanilha;
                                                        totalRateio += l.valor.Value;
                                                        totalRateioQtd += l.qtd.Value;
                                                        detalhamentoLancto1++;

                                                    }

                                                    if (y == (_lanctos.Count - 1))
                                                    {
                                                        if (l != null)
                                                        {
                                                            if ((_titulos[i].vencimentos[x].valor.Value - totalRateio) != 0)
                                                            {
                                                                l.valor += (_titulos[i].vencimentos[x].valor.Value - totalRateio);
                                                            }

                                                            if ((lancto1.qtd - totalRateioQtd) != 0)
                                                            {
                                                                l.qtd += (lancto1.qtd.Value - totalRateioQtd);
                                                            }
                                                        }
                                                    }

                                                    if (l != null)
                                                    {
                                                        _banco.Add(l);
                                                    }
                                                }
                                            }
                                            //SE FOR LANÇAMENTO NORMAL
                                            else
                                            {
                                                SLancamento l = new SLancamento();
                                                l.lote = novoLote;
                                                l.seqLote = 1;
                                                l.detLote = 1;
                                                l.lotePai = lote;
                                                l.seqLotePai = _titulos[i].seqLote;
                                                l.duplicata = _titulos[i].duplicata;
                                                l.seqBaixa = 0;
                                                l.detBaixa = 0;
                                                if (_titulos[i].debCred == 'C')
                                                {
                                                    l.debCred = 'D';
                                                    l.modulo = "CAP_INCLUSAO_TITULO";
                                                }
                                                else
                                                {
                                                    l.debCred = 'C';
                                                    l.modulo = "CAR_INCLUSAO_TITULO";
                                                }

                                                l.valor = _titulos[i].vencimentos[x].valor;

                                                l.valorUnit = l.valor;
                                                l.qtd = 1;

                                                l.conta = _titulos[i].conta;
                                                l.descConta = _titulos[i].descConta;
                                                l.job = _titulos[i].job;
                                                l.descJob = _titulos[i].descJob;
                                                l.linhaNegocio = _titulos[i].linhaNegocio;
                                                l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                                l.divisao = _titulos[i].divisao;
                                                l.descDivisao = _titulos[i].descDivisao;
                                                l.cliente = _titulos[i].cliente;
                                                l.descCliente = _titulos[i].descCliente;
                                                l.pendente = true;
                                                l.dataLancamento = _titulos[i].vencimentos[x].data;
                                                l.historico = _titulos[i].historico;
                                                l.titulo = false;
                                                l.terceiro = _titulos[i].terceiro;
                                                l.descTerceiro = _titulos[i].descTerceiro;
                                                l.modelo = _titulos[i].modelo;
                                                l.numeroDocumento = _titulos[i].numeroDocumento;
                                                l.descConsultor = _titulos[i].descConsultor;
                                                l.codConsultor = _titulos[i].codConsultor;
                                                l.codPlanilha = _titulos[i].codPlanilha;
                                                l.valorBruto = _titulos[i].valorBruto;
                                                if (l.valorBruto == null)
                                                    l.valorBruto = 0;
                                                _banco.Add(l);
                                            }
                                        }
                                    }


                                    //INSERE LANCTOS NA BASE
                                    for (int i = 0; i < _banco.Count; i++)
                                    {
                                        SLancamento l = _banco[i];
                                        lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                            l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                            l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, l.modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, l.valorBruto.Value, l.codMoeda);
                                    }
                                }
                                else
                                {
                                    _banco.Clear();
                                } //FIM 
                            }
                        }
                        else
                        {
                            //SE ESTIVER NO MODULO CONTABILIDADE

                            lote = 0;

                            if (alterado == null)
                                lote = criaNumeroTitulo();
                            else
                                lote = alterado.Value;
                            _lote = lote;

                            int contadorDetalhamento = 1;


                            //processa os lancamentos
                            for (int i = 0; i < _lanctos.Count; i++)
                            {
                                SLancamento x = new SLancamento();
                                x.lote = lote;
                                x.seqLote = _lanctos[i].seqLote;
                                x.detLote = 1;
                                x.lotePai = 0;
                                x.seqLotePai = 0;
                                if (_titulos.Count == 0)
                                {
                                    x.duplicata = 0;
                                }
                                else
                                {
                                    for (int z = 0; z < _titulos.Count; z++)
                                    {
                                        if (_titulos[z] == _lanctos[i])
                                        {
                                            x.duplicata = _titulos[z].duplicata;
                                            break;
                                        }
                                        else
                                        {
                                            x.duplicata = 0;
                                        }
                                    }
                                }
                                x.seqBaixa = 0;
                                x.detBaixa = 0;
                                x.debCred = _lanctos[i].debCred;
                                x.codMoeda = _lanctos[i].codMoeda;
                                x.valor = _lanctos[i].valor;
                                x.valorUnit = _lanctos[i].valorUnit;
                                x.qtd = _lanctos[i].qtd;
                                x.conta = _lanctos[i].conta;
                                x.descConta = _lanctos[i].descConta;
                                x.job = _lanctos[i].job;
                                x.descJob = _lanctos[i].descJob;
                                x.linhaNegocio = _lanctos[i].linhaNegocio;
                                x.descLinhaNegocio = _lanctos[i].descLinhaNegocio;
                                x.divisao = _lanctos[i].divisao;
                                x.descDivisao = _lanctos[i].descDivisao;
                                x.cliente = _lanctos[i].cliente;
                                x.descCliente = _lanctos[i].descCliente;
                                x.pendente = false;
                                x.dataLancamento = _data;
                                x.historico = _lanctos[i].historico;
                                x.titulo = _lanctos[i].titulo;
                                x.terceiro = _lanctos[i].terceiro;
                                x.descTerceiro = _lanctos[i].descTerceiro;
                                x.modelo = _lanctos[i].modelo;
                                x.numeroDocumento = _lanctos[i].numeroDocumento;
                                x.descConsultor = _lanctos[i].descConsultor;
                                x.codConsultor = _lanctos[i].codConsultor;
                                x.codPlanilha = _lanctos[i].codPlanilha;
                                _banco.Add(x);
                            }

                            //INSERE LANCTOS NA BASE
                            for (int i = 0; i < _banco.Count; i++)
                            {
                                SLancamento l = _banco[i];
                                lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                    l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                    l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, _modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);
                            }

                            _banco.Clear();

                            //GERA TITULOS
                            for (int i = 0; i < _titulos.Count; i++)
                            {
                                for (int x = 0; x < _titulos[i].vencimentos.Count; x++)
                                {
                                    double novoLote = criaNumeroTitulo();
                                    int detalhamentoLancto1 = 1;

                                    SLancamento l = new SLancamento();
                                    l.lote = novoLote;
                                    l.seqLote = 1;
                                    l.detLote = 1;
                                    l.lotePai = lote;
                                    l.seqLotePai = _titulos[i].seqLote;
                                    l.duplicata = _titulos[i].duplicata;
                                    l.seqBaixa = 0;
                                    l.detBaixa = 0;
                                    if (_titulos[i].debCred == 'C')
                                    {
                                        l.debCred = 'D';
                                        l.modulo = "CAP_INCLUSAO_TITULO";
                                    }
                                    else
                                    {
                                        l.debCred = 'C';
                                        l.modulo = "CAR_INCLUSAO_TITULO";
                                    }

                                    l.valor = _titulos[i].vencimentos[x].valor;

                                    l.valorUnit = l.valor;
                                    l.qtd = 1;

                                    l.conta = _titulos[i].conta;
                                    l.descConta = _titulos[i].descConta;
                                    l.job = _titulos[i].job;
                                    l.descJob = _titulos[i].descJob;
                                    l.linhaNegocio = _titulos[i].linhaNegocio;
                                    l.descLinhaNegocio = _titulos[i].descLinhaNegocio;
                                    l.divisao = _titulos[i].divisao;
                                    l.descDivisao = _titulos[i].descDivisao;
                                    l.cliente = _titulos[i].cliente;
                                    l.descCliente = _titulos[i].descCliente;
                                    l.pendente = true;
                                    l.dataLancamento = _titulos[i].vencimentos[x].data;
                                    l.historico = _titulos[i].historico;
                                    l.titulo = false;
                                    l.terceiro = _titulos[i].terceiro;
                                    l.descTerceiro = _titulos[i].descTerceiro;
                                    l.modelo = _titulos[i].modelo;
                                    l.numeroDocumento = _titulos[i].numeroDocumento;
                                    l.descConsultor = _titulos[i].descConsultor;
                                    l.codConsultor = _titulos[i].codConsultor;
                                    l.codPlanilha = _titulos[i].codPlanilha;

                                    _banco.Add(l);
                                }
                            }


                            //INSERE LANCTOS NA BASE
                            for (int i = 0; i < _banco.Count; i++)
                            {
                                SLancamento l = _banco[i];
                                lanctoContabDAO.insert(l.lote.Value, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value, l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value,
                                    l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao,
                                    l.cliente.Value, l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro, l.pendente.Value, l.modulo, l.modelo.Value, l.numeroDocumento, l.descConsultor, l.codConsultor, l.codPlanilha, (decimal)l.valorBruto, l.codMoeda);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            erros.Add("Data da folha " + _data.ToString("dd/MM/yyyy") + " que está sendo lançada está fora de um período aberto.");
        }
        if (erros.Count == 0)
        {
            LogController logController = new LogController(conexao);
            if (alterado == null)
            {
                logController.add(new Log(0, "Folha Inserida", Convert.ToInt32(HttpContext.Current.Session["usuario"]),
                    Convert.ToInt32(HttpContext.Current.Session["empresa"]), _modulo, DateTime.Now, lote));
            }
            else
            {
                logController.add(new Log(0, "Folha Alterada", Convert.ToInt32(HttpContext.Current.Session["usuario"]),
                    Convert.ToInt32(HttpContext.Current.Session["empresa"]), _modulo, DateTime.Now, alterado.Value));
            }
        }
        return erros;
    }

    public List<SLancamento> carregaLote(double lote)
    {
        erros = new List<string>();
        List<SLancamento> arr = new List<SLancamento>();

        if (lote > 0)
        {
            DataTable linha = lanctoContabDAO.carregaLote(lote);
            if (linha.Rows.Count != 0)
            {
                double loteAnt = 0;
                int seqLoteAnt = 0;
                SLancamento lancto = new SLancamento();
                decimal valor = 0;
                decimal qtd = 0;

                loteAnt = Convert.ToDouble(linha.Rows[0]["lote"]);
                seqLoteAnt = Convert.ToInt32(linha.Rows[0]["seq_lote"]);

                lancto.lote = Convert.ToDouble(linha.Rows[0]["lote"]);
                lancto.seqLote = Convert.ToInt32(linha.Rows[0]["seq_lote"]);
                lancto.detLote = Convert.ToInt32(linha.Rows[0]["det_lote"]);
                lancto.lotePai = Convert.ToDouble(linha.Rows[0]["lote_pai"]);
                lancto.seqLotePai = Convert.ToInt32(linha.Rows[0]["seq_lote_pai"]);
                lancto.duplicata = Convert.ToDouble(linha.Rows[0]["duplicata"]);
                lancto.debCred = Convert.ToChar(linha.Rows[0]["deb_cred"]);

                lancto.valor = Math.Round(Convert.ToDecimal(linha.Rows[0]["VALOR"]), 2);
                lancto.qtd = Math.Round(Convert.ToDecimal(linha.Rows[0]["QTD"]), 2);
                //lancto.valorUnit = Convert.ToDecimal(linha.Rows[0]["VALOR_UNIT"]);
                lancto.conta = Convert.ToString(linha.Rows[0]["cod_conta"]);
                lancto.descConta = Convert.ToString(linha.Rows[0]["desc_conta"]);
                lancto.job = Convert.ToInt32(linha.Rows[0]["cod_job"]);
                lancto.descJob = Convert.ToString(linha.Rows[0]["desc_job"]);
                lancto.linhaNegocio = Convert.ToInt32(linha.Rows[0]["cod_linha_negocio"]);
                lancto.descLinhaNegocio = Convert.ToString(linha.Rows[0]["desc_linha_negocio"]);
                lancto.divisao = Convert.ToInt32(linha.Rows[0]["cod_divisao"]);
                lancto.descDivisao = Convert.ToString(linha.Rows[0]["desc_divisao"]);//aki
                lancto.cliente = Convert.ToInt32(linha.Rows[0]["cod_cliente"]);
                lancto.descCliente = Convert.ToString(linha.Rows[0]["desc_cliente"]);
                lancto.pendente = Convert.ToBoolean(linha.Rows[0]["pendente"]);
                lancto.dataLancamento = Convert.ToDateTime(linha.Rows[0]["data"]);
                lancto.historico = Convert.ToString(linha.Rows[0]["historico"]);
                lancto.titulo = Convert.ToBoolean(linha.Rows[0]["titulo"]);
                lancto.terceiro = Convert.ToInt32(linha.Rows[0]["cod_terceiro"]);
                lancto.descTerceiro = Convert.ToString(linha.Rows[0]["desc_terceiro"]);
                lancto.modelo = Convert.ToInt32(linha.Rows[0]["cod_modelo"]);
                lancto.numeroDocumento = linha.Rows[0]["numero_documento"].ToString();
                lancto.usuario = Convert.ToInt32(linha.Rows[0]["cod_usuario"]);
                lancto.strData = Convert.ToDateTime(linha.Rows[0]["data"]).ToString("dd/MM/yyyy");
                lancto.descConsultor = linha.Rows[0]["desc_consultor"].ToString();
                lancto.codConsultor = Convert.ToInt32(linha.Rows[0]["cod_consultor"]);
                try { lancto.valorBruto = Convert.ToDecimal(linha.Rows[0]["VALOR_BRUTO"]); }
                catch { lancto.valorBruto = 0; }
                try { lancto.valorGrupo = Convert.ToInt32(linha.Rows[0]["VALOR_GRUPO"]); }
                catch { lancto.valorGrupo = 0; }

                lancto.vencimentos = new List<SVencimento>();


                for (int i = 0; i < linha.Rows.Count; i++)
                {
                    DataRow row = linha.Rows[i];

                    if (Convert.ToDouble(row["lote_pai"]) == 0)
                    {
                        if (Convert.ToInt32(row["seq_lote"]) != seqLoteAnt)
                        {
                            lancto.qtd = Math.Round(qtd, 2);
                            lancto.valor = Math.Round(valor, 2);
                            if (lancto.qtd != 0)
                                lancto.valorUnit = Math.Round(Math.Round(valor, 2) / Math.Round(qtd, 2), 2);
                            else
                                lancto.valorUnit = Math.Round(valor, 2);

                            arr.Add(lancto);
                            lancto = new SLancamento();
                            lancto.lote = Convert.ToDouble(row["lote"]);
                            lancto.seqLote = Convert.ToInt32(row["seq_lote"]);
                            lancto.detLote = Convert.ToInt32(row["det_lote"]);
                            lancto.lotePai = Convert.ToDouble(row["lote_pai"]);
                            lancto.seqLotePai = Convert.ToInt32(row["seq_lote_pai"]);
                            lancto.duplicata = Convert.ToDouble(row["duplicata"]);
                            lancto.debCred = Convert.ToChar(row["deb_cred"]);
                            lancto.conta = Convert.ToString(row["cod_conta"]);
                            lancto.descConta = Convert.ToString(row["desc_conta"]);
                            lancto.job = Convert.ToInt32(row["cod_job"]);
                            lancto.descJob = Convert.ToString(row["desc_job"]);
                            lancto.linhaNegocio = Convert.ToInt32(row["cod_linha_negocio"]);
                            lancto.descLinhaNegocio = Convert.ToString(row["desc_linha_negocio"]);
                            lancto.divisao = Convert.ToInt32(row["cod_divisao"]);
                            lancto.descDivisao = Convert.ToString(row["desc_divisao"]);//aki
                            lancto.cliente = Convert.ToInt32(row["cod_cliente"]);
                            lancto.descCliente = Convert.ToString(row["desc_cliente"]);
                            lancto.pendente = Convert.ToBoolean(row["pendente"]);
                            lancto.dataLancamento = Convert.ToDateTime(row["data"]);
                            lancto.historico = Convert.ToString(row["historico"]);
                            lancto.titulo = Convert.ToBoolean(row["titulo"]);
                            lancto.terceiro = Convert.ToInt32(row["cod_terceiro"]);
                            lancto.descTerceiro = Convert.ToString(row["desc_terceiro"]);
                            lancto.modelo = Convert.ToInt32(row["cod_modelo"]);
                            lancto.numeroDocumento = row["numero_documento"].ToString();
                            lancto.descConsultor = row["desc_consultor"].ToString();
                            lancto.codConsultor = Convert.ToInt32(row["cod_consultor"]);
                            lancto.strData = Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy");
                            lancto.valorUnit = Convert.ToDecimal(row["valor_unit"]);
                            lancto.vencimentos = new List<SVencimento>();
                            lancto.usuario = Convert.ToInt32(linha.Rows[0]["cod_usuario"]);
                            valor = Convert.ToDecimal(row["valor"]);
                            qtd = Convert.ToDecimal(row["qtd"]);
                            loteAnt = Convert.ToDouble(row["lote"]);
                            seqLoteAnt = Convert.ToInt32(row["seq_lote"]);
                            try { lancto.valorBruto = Convert.ToDecimal(row["VALOR_BRUTO"]); }
                            catch { lancto.valorBruto = 0; }

                            try { lancto.valorGrupo = Convert.ToInt32(row["VALOR_GRUPO"]); }
                            catch { lancto.valorGrupo = 0; }

                        }
                        else
                        {
                            valor += Math.Round(Convert.ToDecimal(row["valor"]), 2);
                            qtd += Math.Round(Convert.ToDecimal(row["qtd"]), 2);
                        }
                    }
                }
                lancto.valor = valor;
                lancto.qtd = qtd;
                if (lancto.qtd != 0)
                    lancto.valorUnit = valor / qtd;
                else
                    lancto.valorUnit = valor;
                arr.Add(lancto);


                valor = 0;
                loteAnt = 0;
                seqLoteAnt = 0;

                for (int i = 0; i < linha.Rows.Count; i++)
                {
                    DataRow row = linha.Rows[i];

                    if (Convert.ToDouble(row["lote_pai"]) > 0 && Convert.ToInt32(row["seq_lote_pai"]) > 0)
                    {
                        if (Convert.ToInt32(row["seq_lote"]) != seqLoteAnt || Convert.ToInt32(row["lote"]) != loteAnt)
                        {
                            for (int x = 0; x < linha.Rows.Count; x++)
                            {
                                if (Convert.ToDouble(linha.Rows[x]["lote"]) == Convert.ToDouble(row["lote"]) && Convert.ToInt32(linha.Rows[x]["seq_lote"]) == Convert.ToInt32(row["seq_lote"]) && Convert.ToDouble(linha.Rows[x]["det_baixa"]) <= 0)
                                {
                                    valor += Convert.ToDecimal(linha.Rows[x]["valor"]);
                                }
                            }

                            for (int y = 0; y < arr.Count; y++)
                            {
                                if (arr[y].lote == Convert.ToDouble(row["lote_pai"]) && arr[y].seqLote == Convert.ToInt32(row["seq_lote_pai"]) && Convert.ToDouble(row["det_baixa"]) <= 0)
                                {
                                    if (arr[y].vencimentos == null)
                                        arr[y].vencimentos = new List<SVencimento>();

                                    arr[y].vencimentos.Add(new SVencimento(Convert.ToDateTime(row["data"]), valor));
                                }
                            }

                            loteAnt = Convert.ToDouble(row["lote"]);
                            seqLoteAnt = Convert.ToInt32(row["seq_lote"]);
                            valor = 0;
                        }
                    }
                }


            }
            else
            {
                erros.Add("Lote não possui lançamentos.");
            }
        }
        else
        {
            erros.Add("Lote inválido.");
        }



        return arr;
    }

    public List<string> deletar(double lote)
    {
        erros = new List<string>();

        bool podeapagar = true;
        try
        {
            podeapagar = verificaFaturamento(lote);
        }
        catch (Exception)
        {

            throw;
        }
        if (podeapagar)
        {
            if (verificaPeriodoFechamento(lote))
            {
                if (!verificaFilhos(lote))
                {
                    erros.Add("Lote não pode ser deletado porque possui filhos baixados.");
                }
                else
                {
                    if (salvaBackup(lote, DateTime.Now, Convert.ToInt32(HttpContext.Current.Session["usuario"]), "DEL"))
                    {
                        lanctoContabDAO.deleteLote(lote);

                        LogController logController = new LogController(conexao);

                        logController.add(new Log(0, "Folha Removida", Convert.ToInt32(HttpContext.Current.Session["usuario"]),
                                Convert.ToInt32(HttpContext.Current.Session["empresa"]), "", DateTime.Now, lote));
                    }
                    else
                    {
                        erros.Add("Erro no momento de salvar o backup da folha.");
                    }
                }
            }
            else
            {
                erros.Add("Lote está fora de um período aberto.");
            }
        }
        else
        {
            erros.Add("Lote não pode ser deletado. Pois a origem deste lote é o faturamento.");
        }
        return erros;
    }

    public List<string> deletaBaixa(double baixa)
    {
        erros = new List<string>();
        if (lanctoContabDAO.verificaFilhosBaixa(baixa) == 0)
        {
            lanctoContabDAO.deleteFilhosBaixa(baixa);
            lanctoContabDAO.voltaLotesBaixados(baixa);
            lanctoContabDAO.deleteContraPartidas(baixa);

        }
        else
        {
            erros.Add("Baixa não pode ser deletada pois existem titulos filhos já baixados.");
        }

        return erros;
    }

    public int totalRegistrosPendentes(string tipo, Nullable<DateTime> periodoDe,
        Nullable<DateTime> periodoAte, Nullable<int> terceiro)
    {
        return lanctoContabDAO.totalRegistrosPendentes(tipo, periodoDe, periodoAte, terceiro);
    }

    public void listaPendentes(ref DataTable tb, string tipo, Nullable<DateTime> periodoDe,
        Nullable<DateTime> periodoAte, Nullable<int> terceiro, int paginaAtual, int pag)
    {
        lanctoContabDAO.listaPendentes(ref tb, tipo, periodoDe, periodoAte, terceiro, paginaAtual, pag);
    }

    public void listaPaginada(ref DataTable tb, Nullable<double> lote, Nullable<DateTime> dataInicio, Nullable<DateTime> dataTermino, string conta,
        Nullable<int> job, Nullable<int> terceiro, string documento, int paginaAtual, string ordenacao, Nullable<double> codplanilha)
    {
        lanctoContabDAO.lista(ref tb, _modulo, lote, dataInicio, dataTermino, conta, job, terceiro, documento, paginaAtual, ordenacao, codplanilha);
    }

    public int totalRegistros(Nullable<double> lote, Nullable<DateTime> dataInicio, Nullable<DateTime> dataTermino, string conta, Nullable<int> job,
        Nullable<int> terceiro, Nullable<double> codplanilha)
    {
        return lanctoContabDAO.totalRegistros(_modulo, lote, dataInicio, dataTermino, conta, job, terceiro, codplanilha);
    }

    public void listaBaixasPaginada(ref DataTable tb, double? baixa, Nullable<DateTime> dtInicio, Nullable<DateTime> dtTermino, int? terceiro, int paginaAtual, string ordenacao)
    {
        lanctoContabDAO.listaBaixas(ref tb, baixa, dtInicio, dtTermino, terceiro, paginaAtual, ordenacao);
    }

    public int totalRegistrosBaixa(double? baixa, Nullable<DateTime> dtInicio, Nullable<DateTime> dtTermino, int? terceiro)
    {
        return lanctoContabDAO.totalRegistrosBaixa(baixa, dtInicio, dtTermino, terceiro);
    }

    private bool verificaDebitoCredito()
    {
        bool resposta = false;
        decimal totalDebito = 0;
        decimal totalCredito = 0;

        if (_lanctos != null)
        {
            if (_lanctos.Count > 0)
            {
                for (int i = 0; i < _lanctos.Count; i++)
                {
                    if (_lanctos[i].debCred == 'D')
                        totalDebito += _lanctos[i].valor.Value;
                    else
                        totalCredito += _lanctos[i].valor.Value;
                }

                if (totalDebito == totalCredito)
                    resposta = true;
            }
        }

        return resposta;
    }

    private void pegaLanctosImposto()
    {
        if (_lanctos != null)
        {
            if (_lanctos.Count > 0)
            {
                for (int i = 0; i < _lanctos.Count; i++)
                {
                    ContaContabil conta = new ContaContabil(conexao);
                    conta.codigo = _lanctos[i].conta;
                    conta.load();
                    if (conta.tipo == "I" || conta.tipo == "IR")
                        _impostos.Add(_lanctos[i]);
                }
            }
        }
    }

    private void pegaLanctosBaseImposto()
    {
        if (_lanctos != null)
        {
            if (_lanctos.Count > 0)
            {
                for (int i = 0; i < _lanctos.Count; i++)
                {
                    ContaContabil conta = new ContaContabil(conexao);
                    conta.codigo = _lanctos[i].conta;
                    conta.load();
                    if (conta.tipo == "BI")
                        _basesImposto.Add(_lanctos[i]);
                }
            }
        }
    }

    private void pegaLanctosTitulo()
    {
        if (_lanctos != null)
        {
            if (_lanctos.Count > 0)
            {
                for (int i = 0; i < _lanctos.Count; i++)
                {
                    if (_lanctos[i].titulo.Value)
                    {
                        SLancamento l = _lanctos[i];
                        l.duplicata = criaNumeroDuplicata();
                        _titulos.Add(l);
                    }
                }
            }
        }
    }

    private double criaNumeroTitulo()
    {
        return lanctoContabDAO.getNewNumeroLote();
    }

    private double criaNumeroDuplicata()
    {
        return lanctoContabDAO.getNewNumeroDuplicata();
    }

    private double criaNumeroBaixa()
    {
        return lanctoContabDAO.getNewNumeroBaixa();
    }

    public SLancamento getLancamentoPai(double lote, int seqLote, string modulo)
    {
        DataTable tabela = lanctoContabDAO.getLancamentoPai(lote, seqLote, modulo);
        SLancamento l = new SLancamento();
        if (tabela.Rows.Count > 0)
        {
            DataRow row = tabela.Rows[0];
            l.lote = lote;
            l.seqLote = seqLote;
            l.detLote = Convert.ToInt32(row["det_lote"]);
            l.lotePai = Convert.ToDouble(row["lote_pai"]);
            l.seqLotePai = Convert.ToInt32(row["seq_lote_pai"]);
            l.duplicata = Convert.ToDouble(row["duplicata"]);
            l.seqBaixa = Convert.ToDouble(row["seq_baixa"]);
            l.detBaixa = Convert.ToDouble(row["det_baixa"]);
            l.debCred = Convert.ToChar(row["deb_cred"]);
            l.modulo = row["modulo"].ToString();
            l.valor = Convert.ToDecimal(row["valor"]);

            l.valorUnit = Convert.ToDecimal(row["valor_unit"]);
            l.qtd = Convert.ToDecimal(row["qtd"]);

            l.conta = row["cod_conta"].ToString();
            l.descConta = row["desc_conta"].ToString();
            l.job = Convert.ToInt32(row["cod_job"]);
            l.descJob = row["desc_job"].ToString();
            l.linhaNegocio = Convert.ToInt32(row["cod_linha_negocio"]);
            l.descLinhaNegocio = row["desc_linha_negocio"].ToString();
            l.divisao = Convert.ToInt32(row["cod_divisao"]);
            l.descDivisao = row["desc_divisao"].ToString();
            l.cliente = Convert.ToInt32(row["cod_cliente"]);
            l.descCliente = row["desc_cliente"].ToString();
            l.pendente = Convert.ToBoolean(row["pendente"]);
            l.dataLancamento = Convert.ToDateTime(row["data"]);
            l.historico = row["historico"].ToString();
            l.titulo = Convert.ToBoolean(row["titulo"]);
            l.terceiro = Convert.ToInt32(row["cod_terceiro"]);
            l.descTerceiro = row["desc_terceiro"].ToString();
            l.modelo = Convert.ToInt32(row["cod_modelo"]);
            l.numeroDocumento = row["numero_documento"].ToString();
            l.descConsultor = row["desc_consultor"].ToString();
            l.codConsultor = Convert.ToInt32(row["cod_consultor"]);
        }

        return l;
    }

    public double getLoteAnterior(string modulo, double codigoTerceiro)
    {
        return lanctoContabDAO.getLoteAnterior(modulo, codigoTerceiro);
    }

    public int totalLotesAnteriores(string modulo, double codigoTerceiro)
    {
        return lanctoContabDAO.totalLotesAnteriores(modulo, codigoTerceiro);
    }

    public string getStatusBaixas(double lote)
    {
        string status = "Sem Baixas";
        DataTable filhos = lanctoContabDAO.getFilhosBaixas(lote);

        if (filhos.Rows.Count > 0)
        {
            int semBaixar = 0;
            for (int i = 0; i < filhos.Rows.Count; i++)
            {
                DataRow row = filhos.Rows[i];
                if (Convert.ToInt32(row["seq_baixa"]) == 0)
                {
                    semBaixar++;
                }
            }

            if (semBaixar == 0)
                status = "Baixa Total";
            else
            {
                if (semBaixar < filhos.Rows.Count)
                    status = "Baixa Parcial";
                else
                    status = "Sem Baixas";
            }
        }

        return status;
    }

    public DataTable getBaixas(double lote, int seqLote)
    {
        DataTable filhos = lanctoContabDAO.getFilhosBaixas(lote, seqLote);
        return filhos;
    }
}
