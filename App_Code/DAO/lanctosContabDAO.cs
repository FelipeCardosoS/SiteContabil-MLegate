using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;

public class lanctosContabDAO
{
    private Conexao _conn;

    public lanctosContabDAO(Conexao c)
    {
        _conn = c;
    }

    public DateTime dataPrimeiroLancamento()
    {
        string sql = "select MIN(data) as data from lanctos_contab";
        DateTime data = Convert.ToDateTime(_conn.scalar(sql));
        return data;
    }

    public DataTable listTotalDia(DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select data, SUM(valor) as total,TIPO_LANCTO, lote " +
                    " from LANCTOS_CONTAB " +
                    " where DEB_CRED='D' and PENDENTE='False' " +
                    " and DATA >= '" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' " +
                    " and COD_EMPRESA =" + codEmpresa + " " +
                    " group by DATA,TIPO_LANCTO, lote " +
                    " order by data";

        return _conn.dataTable(sql, "dados");
    }

    public DataTable listLanctosDia(DateTime data, string tipoLancto, int codEmpresa, string Lote)
    {
        string sql = "select cgc.resultado, lc.COD_CONTA, lc.COD_JOB,valor,DEB_CRED, NUMERO_DOCUMENTO,HISTORICO,COD_TERCEIRO " +
                    " from LANCTOS_CONTAB lc, CAD_GRUPOS_CONTABEIS cgc , CAD_CONTAS cc  " +
                    " where  PENDENTE='False' " +
                    @" and lc.COD_EMPRESA = cc.COD_EMPRESA
                        and lc.cod_conta = cc.COD_CONTA
                        and cc.COD_EMPRESA = cgc.COD_EMPRESA
                        and cc.COD_GRUPO_CONTABIL = cgc.COD_GRUPO_CONTABIL " +
                    " and DATA = '" + data.ToString("yyyyMMdd") + "' " +
                    " and Lote = " + Lote.ToString()  +
                    " and lc.COD_EMPRESA =" + codEmpresa + " and TIPO_LANCTO='" + tipoLancto + "' " +
                    " order by LOTE,SEQ_LOTE";

        return _conn.dataTable(sql, "dados");
    }

    public DataTable listaLancamentosEncerramento(DateTime periodoInicio, DateTime periodoTermino)
    {
        string sql = "select lc.cod_conta,cc.descricao as desc_conta,lc.cod_job, cej.nome_razao_social + ' - ' + cj.descricao as desc_job, " +
                    " lc.cod_linha_negocio,cln.descricao as desc_linha_negocio,lc.cod_divisao,cd.descricao as desc_divisao, " +
                    " lc.cod_cliente,ce.nome_razao_social as desc_cliente, " +
                    " sum((case when lc.deb_cred = 'D' then valor else -valor end)) as valor, lc.desc_consultor, lc.cod_consultor  " +
                    " from lanctos_contab lc, cad_contas cc,  " +
                    " cad_grupos_contabeis cgc, cad_jobs cj, cad_linha_negocios cln, " +
                    " cad_divisoes cd, cad_empresas ce, cad_empresas cej " +
                    " where  " +
                    " (cgc.REGRA_EXIBICAO = 'R') " +
                    " and lc.pendente='False' " +
                    " and data >= '" + periodoInicio.ToString("yyyyMMdd") + "' and data <= '" + periodoTermino.ToString("yyyyMMdd") + "' " +
                    " and lc.cod_empresa = " + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " and cc.cod_grupo_contabil = cgc.cod_grupo_contabil " +
                    " and lc.cod_job = cj.cod_job " +
                    " and lc.cod_linha_negocio = cln.cod_linha_negocio " +
                    " and lc.cod_divisao = cd.cod_divisao " +
                    " and lc.cod_cliente = ce.cod_empresa " +
                    " and cj.cod_cliente = cej.cod_empresa " +
                    " and cj.cod_empresa = lc.cod_empresa " +
                    " and cc.cod_empresa = lc.cod_empresa " +
                    " group by " +
                    " lc.cod_conta,cc.descricao,lc.cod_job, cej.nome_razao_social + ' - ' + cj.descricao, " +
                    " lc.cod_linha_negocio,cln.descricao ,lc.cod_divisao,cd.descricao, " +
                    " lc.cod_cliente,ce.nome_razao_social, lc.desc_consultor, lc.cod_consultor ";

        return _conn.dataTable(sql, "lanctosEncerramento");
    }

    public DataTable listaLancamentosEncerramentoMoeda(DateTime periodoInicio, DateTime periodoTermino)
    {
        string sql = "select lc.cod_moeda, lc.cod_conta,cc.descricao as desc_conta,lc.cod_job, cej.nome_razao_social + ' - ' + cj.descricao as desc_job, " +
                    " lc.cod_linha_negocio,cln.descricao as desc_linha_negocio,lc.cod_divisao,cd.descricao as desc_divisao, " +
                    " lc.cod_cliente,ce.nome_razao_social as desc_cliente, " +
                    " sum((case when lc.deb_cred = 'D' then valor else -valor end)) as valor, lc.desc_consultor, lc.cod_consultor  " +
                    " from lanctos_contab_moeda lc, cad_contas cc,  " +
                    " cad_grupos_contabeis cgc, cad_jobs cj, cad_linha_negocios cln, " +
                    " cad_divisoes cd, cad_empresas ce, cad_empresas cej " +
                    " where  " +
                    " (cgc.REGRA_EXIBICAO = 'R') " +
                    " and lc.pendente='False' " +
                    " and data >= '" + periodoInicio.ToString("yyyyMMdd") + "' and data <= '" + periodoTermino.ToString("yyyyMMdd") + "' " +
                    " and lc.cod_empresa = " + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " and cc.cod_grupo_contabil = cgc.cod_grupo_contabil " +
                    " and lc.cod_job = cj.cod_job " +
                    " and lc.cod_linha_negocio = cln.cod_linha_negocio " +
                    " and lc.cod_divisao = cd.cod_divisao " +
                    " and lc.cod_cliente = ce.cod_empresa " +
                    " and cj.cod_cliente = cej.cod_empresa " +
                    " and cj.cod_empresa = lc.cod_empresa " +
                    " and cc.cod_empresa = lc.cod_empresa " +
                    " group by " +
                    " lc.cod_moeda, lc.cod_conta,cc.descricao,lc.cod_job, cej.nome_razao_social + ' - ' + cj.descricao, " +
                    " lc.cod_linha_negocio,cln.descricao ,lc.cod_divisao,cd.descricao, " +
                    " lc.cod_cliente,ce.nome_razao_social, lc.desc_consultor, lc.cod_consultor " +
                    " order by lc.cod_moeda, COD_CONTA ";

        return _conn.dataTable(sql, "lanctosEncerramento");
    }

    public void insertMoeda(double lote, int seqLote, int detLote, int codMoeda, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
    string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
    string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
    bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
    string descConsultor, int codConsultor, int codPlanilha, decimal valorBruto, int valorGrupo, string TipoLancto)
    {
        string sql = "";

        try
        {
            sql = "INSERT INTO LANCTOS_CONTAB_MOEDA (LOTE,SEQ_LOTE,DET_LOTE,COD_MOEDA,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
            sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
            sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,COD_USUARIO, DESC_CONSULTOR, COD_CONSULTOR, COD_PLANILHA, VALOR_BRUTO, VALOR_GRUPO, TIPO_LANCTO)";
            sql += "VALUES";
            sql += "(" + lote + "," + seqLote + "," + detLote + "," + codMoeda + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
            sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
            sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
            sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + dataLancamento.ToString("yyyyMMdd") + "'," + HttpContext.Current.Session["usuario"] + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + "," + valorBruto.ToString().Replace(",", ".") + "," + valorGrupo + ",'" + TipoLancto + "')";

            _conn.execute(sql);
        }
        catch (Exception err)
        {
            throw new Exception(err.Message + "  CONSULTA: " + sql);
        }
    }

    public void insert(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
        string descConsultor, int codConsultor, int codPlanilha, decimal valorBruto, int codMoeda)
    {
        string sql = "INSERT INTO " + (codMoeda != 0 ? "LANCTOS_CONTAB_MOEDA" : "LANCTOS_CONTAB") + "(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,COD_USUARIO, DESC_CONSULTOR, COD_CONSULTOR, COD_PLANILHA, VALOR_BRUTO " + (codMoeda != 0 ? ", COD_MOEDA, INSERCAO_MANUAL)" : ")") + " ";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + dataLancamento.ToString("yyyyMMdd") + "'," + HttpContext.Current.Session["usuario"] + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + "," + valorBruto.ToString().Replace(",", ".") + " " + (codMoeda != 0 ? "," + codMoeda + ", 'True')" : ")") + " ";

        _conn.execute(sql);
    }

    public void insert(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
    string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
    string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
    bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
        DateTime dataAnterior, string descConsultor, int codConsultor, int codPlanilha)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,COD_USUARIO,DESC_CONSULTOR, COD_CONSULTOR, COD_PLANILHA)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + dataAnterior.ToString("yyyyMMdd") + "'," + HttpContext.Current.Session["usuario"] + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + ")";

        _conn.execute(sql);
    }

    public void insert(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
    string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
    string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
    bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
        string tipoLancto, string descConsultor, int codConsultor, int codPlanilha, decimal valorBruto)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,TIPO_LANCTO,COD_USUARIO, desc_consultor, cod_consultor, COD_PLANILHA, VALOR_BRUTO)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + tipoLancto + "'," + HttpContext.Current.Session["usuario"] + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + "," + valorBruto.ToString().Replace(",", ".") + ")";
        _conn.execute(sql);
    }

    public void insert(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
        DateTime dataAnterior, string tipoLancto, string descConsultor, int codConsultor, int codPlanilha, decimal valorBruto)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,DESC_CONSULTOR,COD_CONSULTOR, COD_PLANILHA, VALOR_BRUTO)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + dataAnterior.ToString("yyyyMMdd") + "','" + tipoLancto + "'," + HttpContext.Current.Session["usuario"] + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + "," + valorBruto.ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    public void insert(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento,
        DateTime dataAnterior, string tipoLancto, int codUsuario, string descConsultor, int codConsultor, int codPlanilha, decimal valorBruto)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,DESC_CONSULTOR, COD_CONSULTOR, COD_PLANILHA, valor_bruto)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ",'" + dataAnterior.ToString("yyyyMMdd") + "','" + tipoLancto + "'," + codUsuario + ",'" + descConsultor + "'," + codConsultor + "," + codPlanilha + "," + valorBruto.ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    public void insertCotacao(double lote, int seq_lote, List<CotacaoItem> cotacao)
    {
        string sql = "DELETE FROM LANCTOS_COTACAO WHERE LOTE = " + lote + " AND SEQ_LOTE = " + seq_lote + " ";

        foreach (CotacaoItem cota in cotacao)
        {
            if (cota.valorMoeda != 0)
            {
                sql += " INSERT INTO LANCTOS_COTACAO (LOTE,SEQ_LOTE,COD_MOEDA,VALOR) VALUES (" + lote + "," + seq_lote + "," + cota.codMoeda + "," + cota.valorMoeda.ToString().Replace(",", ".") + ") ";
            }
        }

        _conn.execute(sql);
    }

    public void salvaBackup(double lote, DateTime dataBackup, int codUsuarioBackup, string tipo)
    {
        string sql = "insert into LANCTOS_CONTAB_BACKUP(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,DATA_BACKUP, COD_USUARIO_BACKUP, TIPO,DESC_CONSULTOR,COD_CONSULTOR, VALOR_BRUTO) ";
        sql += "(select LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,'" + dataBackup.ToString("yyyyMMdd") + "'," + codUsuarioBackup + ", '" + tipo + "', DESC_CONSULTOR, COD_CONSULTOR, VALOR_BRUTO from lanctos_contab where lote=" + lote + ")";

        _conn.execute(sql);
    }

    public void insertTemp(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento, DateTime dataAnterior, string tipoLancto, int codUsuario, string descConsultor, int codConsultor)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB_TEMP(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA,DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,DESC_CONSULTOR,COD_CONSULTOR)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ", '" + dataAnterior.ToString("yyyyMMdd") + "','" + tipoLancto + "'," + codUsuario + ",'" + descConsultor + "'," + codConsultor + ")";

        _conn.execute(sql);
    }

    public void insertTempBaixa(double lote, int seqLote, int detLote, double lotePai, int seqLotePai, double duplicata, double seqBaixa, double detBaixa, char debCred, DateTime dataLancamento,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo, int terceiro, string descTerceiro, bool pendente, string modulo, int modelo, string numeroDocumento, DateTime dataAnterior, string tipoLancto, int codUsuario, string descConsultor, int codConsultor)
    {
        string sql = "INSERT INTO LANCTOS_CONTAB_TEMP_BAIXA(LOTE,SEQ_LOTE,DET_LOTE,LOTE_PAI,SEQ_LOTE_PAI,DUPLICATA,SEQ_BAIXA,DET_BAIXA,DEB_CRED,DATA,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO,COD_TERCEIRO,DESC_TERCEIRO,PENDENTE,MODULO,COD_MODELO,NUMERO_DOCUMENTO,COD_EMPRESA, DATA_ANTERIOR,TIPO_LANCTO,COD_USUARIO,DESC_CONSULTOR,COD_CONSULTOR)";
        sql += "VALUES";
        sql += "(" + lote + "," + seqLote + "," + detLote + "," + lotePai + "," + seqLotePai + "," + duplicata + ", " + seqBaixa + ", " + detBaixa + ", '" + debCred + "','" + dataLancamento.ToString("yyyyMMdd") + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "'," + qtd.ToString().Replace(",", ".") + ", " + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + terceiro + ",'" + descTerceiro + "','" + pendente + "','" + modulo + "'," + modelo + ",'" + numeroDocumento + "'," + HttpContext.Current.Session["empresa"] + ", '" + dataAnterior.ToString("yyyyMMdd") + "', '" + tipoLancto + "'," + codUsuario + ",'" + descConsultor + "'," + codConsultor + ")";

        _conn.execute(sql);
    }

    public void update(double lote, int seqLote, string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, string historico, string numeroDocumento, string descConsultor, int codConsultor, decimal? valor_bruto)
    {
        string sql = "update lanctos_contab set cod_conta='" + conta + "', DESC_CONTA='" + descConta + "',COD_JOB=" + job + ",DESC_JOB='" + descJob + "',COD_LINHA_NEGOCIO=" + linhaNegocio + ",DESC_LINHA_NEGOCIO='" + descLinhaNegocio + "',COD_DIVISAO=" + divisao + ",DESC_DIVISAO='" + descDivisao + "', ";
        sql += "COD_CLIENTE=" + cliente + ",DESC_CLIENTE='" + descCliente + "', historico='" + historico + "', numero_documento='" + numeroDocumento + "', desc_consultor='" + descConsultor + "', cod_consultor=" + codConsultor + ", valor_bruto = " + valor_bruto.ToString().Replace(",", ".") + " where lote=" + lote + " and seq_lote=" + seqLote + " ";

        _conn.execute(sql);
    }

    public void updateFilhos(double lote, int seqLote, string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
    string descDivisao, int cliente, string descCliente, string historico, string numeroDocumento, string descConsultor, int codConsultor)
    {
        string sql = "update lanctos_contab set cod_conta='" + conta + "', DESC_CONTA='" + descConta + "',COD_JOB=" + job + ",DESC_JOB='" + descJob + "',COD_LINHA_NEGOCIO=" + linhaNegocio + ",DESC_LINHA_NEGOCIO='" + descLinhaNegocio + "',COD_DIVISAO=" + divisao + ",DESC_DIVISAO='" + descDivisao + "', ";
        sql += "COD_CLIENTE=" + cliente + ",DESC_CLIENTE='" + descCliente + "', historico='" + historico + "', numero_documento='" + numeroDocumento + "', desc_Consultor='" + descConsultor + "', cod_consultor=" + codConsultor + " where lote_pai=" + lote + " and seq_lote_pai=" + seqLote + " and det_baixa <= 0 and seq_baixa >= 0  ";

        _conn.execute(sql);
    }

    public void updateMinLancto(double lote, int seqLote, string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, string descConsultor, int codConsultor)
    {
        string sql = "update lanctos_contab set cod_conta='" + conta + "', DESC_CONTA='" + descConta + "',COD_JOB=" + job + ",DESC_JOB='" + descJob + "',COD_LINHA_NEGOCIO=" + linhaNegocio + ",DESC_LINHA_NEGOCIO='" + descLinhaNegocio + "',COD_DIVISAO=" + divisao + ",DESC_DIVISAO='" + descDivisao + "', ";
        sql += "COD_CLIENTE=" + cliente + ",DESC_CLIENTE='" + descCliente + "', desc_consultor='" + descConsultor + "', cod_consultor=" + codConsultor + " where lote_pai=" + lote + " and seq_lote_pai=" + seqLote + " ";

        _conn.execute(sql);
    }

    public DataTable getFilhosLancamento(double lote, int seqLote)
    {
        string sql = "select * from lanctos_contab where lote_pai=" + lote + " and seq_lote_pai=" + seqLote + "";
        return _conn.dataTable(sql, "filho");
    }

    public void updateBaixasFilhos(double lote, int seqLote, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, string historico, string numeroDocumento, string descConsultor, int codConsultor)
    {
        string sql = "update lanctos_contab set COD_JOB=" + job + ",DESC_JOB='" + descJob + "',COD_LINHA_NEGOCIO=" + linhaNegocio + ",DESC_LINHA_NEGOCIO='" + descLinhaNegocio + "',COD_DIVISAO=" + divisao + ",DESC_DIVISAO='" + descDivisao + "', ";
        sql += "COD_CLIENTE=" + cliente + ",DESC_CLIENTE='" + descCliente + "', historico='" + historico + "', numero_documento='" + numeroDocumento + "', desc_consultor='" + descConsultor + "', cod_consultor=" + codConsultor + " where lote_pai=" + lote + " and seq_lote_pai=" + seqLote + " and det_baixa < 0 and seq_baixa > 0 ";

        _conn.execute(sql);
    }

    public void deleteLote(double lote)
    {
        if (lote != 0)
        {
            string sql = "DELETE FROM LANCTOS_CONTAB WHERE (LOTE = " + lote + " OR (LOTE_PAI = " + lote + " and LOTE_PAI <> 0)) AND LOTE <> 0 and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);
        }
    }

    public void deleteLoteTemp(double lote)
    {
        if (lote != 0)
        {
            string sql = "DELETE FROM LANCTOS_CONTAB_TEMP WHERE (LOTE=" + lote + " OR (LOTE_PAI=" + lote + " and LOTE_PAI <> 0)) AND LOTE <> 0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

            _conn.execute(sql);
        }
    }

    public DateTime getDataLote(double lote)
    {
        string sql = "SELECT TOP 1 DATA FROM LANCTOS_CONTAB WHERE LOTE=" + lote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

        return Convert.ToDateTime(_conn.scalar(sql));
    }

    public DateTime getDataBaixa(double baixa)
    {
        string sql = "SELECT TOP 1 DATA FROM LANCTOS_CONTAB WHERE SEQ_BAIXA=" + baixa + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

        return Convert.ToDateTime(_conn.scalar(sql));
    }

    public int verificaFilhosBaixa(double baixa)
    {
        string sql = "select count(lote) from lanctos_contab where lote_pai in (select lote from lanctos_contab where seq_baixa=" + baixa + " and  modulo = 'BAIXA_TITULO' and titulo='True' and lote_pai=0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + ") and pendente = 'False'";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public int verificaDeletaBaixa(double Lote)
    {
        string sql = "select count(*) from LANCTOS_CONTAB where lote = "+Lote+" and cod_conta in (select cod_conta from CAD_CONTAS where cod_tipo_conta in ('BI', 'I'))";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void deleteContraPartidas(double baixa)
    {
        string sql = "delete from lanctos_contab where seq_baixa=" + baixa + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        _conn.execute(sql);
    }

    public void deleteFilhosBaixa(double baixa)
    {
        string sql = "delete from lanctos_contab where lote_pai in (select lote from lanctos_contab where lote <> 0 and seq_baixa=" + baixa + " and  modulo = 'BAIXA_TITULO' and lote_pai=0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + ")";
        _conn.execute(sql);
    }

    public void voltaLotesBaixados(double baixa)
    {
        string sql = "update lanctos_contab set seq_baixa=0,det_baixa=0 , pendente = 'True',data=data_anterior WHERE SEQ_BAIXA =" + baixa + " and det_baixa <= 0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        _conn.execute(sql);
    }

    public void baixaLote(double lote, double seq_baixa, DateTime data, double det_baixa)
    {
        string sql = "UPDATE LANCTOS_CONTAB SET PENDENTE='False', SEQ_BAIXA=" + seq_baixa + ", DATA_ANTERIOR = DATA, DATA='" + data.ToString("yyyyMMdd") + "', DET_BAIXA=" + det_baixa + " WHERE LOTE=" + lote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

        _conn.execute(sql);
    }

    public DataTable carregaBaixa(double baixa)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB WHERE SEQ_BAIXA=" + baixa + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaBaixaPorLoteTemp(double baixa, double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and lote_pai=" + lote + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaTitulosDiferenteLoteTemp(double baixa, double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and lote_pai<>" + lote + " and det_baixa < 0 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaTitulosDiferenteLoteTemp(double baixa)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and det_baixa < 0 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public void restauraBaixa(double lote, int seq_lote, int det_lote, double seq_baixa, int det_baixa)
    {
        string sql = "UPDATE LANCTOS_CONTAB SET SEQ_BAIXA=" + seq_baixa + ", det_baixa=" + det_baixa + " where lote=" + lote + " and seq_lote=" + seq_lote + " and det_lote=" + det_lote + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public DataTable carregaContraPartidasBaixaTemp(double baixa, double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and lote_pai<>" + lote + " and det_baixa = 1 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaContraPartidasBaixaTemp(double baixa)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and det_baixa = 1 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaRestoBaixaTemp(double baixa, double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and lote_pai<>" + lote + " and det_baixa > 1 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaRestoBaixaTemp(double baixa)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " and det_baixa > 1 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DET_BAIXA";
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable deleteBaixaTemp(double baixa)
    {
        string sql = "DELETE FROM LANCTOS_CONTAB_TEMP_BAIXA WHERE SEQ_BAIXA=" + baixa + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "baixa");
    }

    public DataTable carregaLote(double lote)
    {
        string sql = " select x.LOTE,x.SEQ_LOTE,x.DET_LOTE,x.LOTE_PAI,x.SEQ_LOTE_PAI,x.DUPLICATA,x.SEQ_BAIXA,x.DET_BAIXA,x.DEB_CRED,x.DATA,x.COD_CONTA, " +
        " x.desc_conta,x.COD_JOB,x.DESC_JOB,x.COD_LINHA_NEGOCIO,x.DESC_LINHA_NEGOCIO,x.COD_DIVISAO,x.DESC_DIVISAO, " +
        " x.COD_CLIENTE,x.DESC_CLIENTE,x.QTD,x.VALOR_UNIT,x.VALOR,x.HISTORICO,x.TITULO,x.COD_TERCEIRO,x.DESC_TERCEIRO,x.PENDENTE,x.MODULO, " +
        " x.COD_MODELO,x.COD_EMPRESA,x.NUMERO_DOCUMENTO,x.COD_ORIGEM,x.SEQ_ORIGEM,x.EFETIVADO,x.DATA_ANTERIOR,x.TIPO_LANCTO,x.COD_USUARIO, " +
        " x.COD_CONSULTOR,x.DESC_CONSULTOR, x.VALOR_BRUTO, isnull(x.VALOR_GRUPO,0) as VALOR_GRUPO , isnull(cln.DESCRICAO,'') as DESCR_LINHA_NEGOCIO,  " +
        " isnull(cd.DESCRICAO,'') as DESCR_DIVISAO, isnull(ce.NOME_FANTASIA,'') as DESCR_CLIENTE, isnull(cj.DESCRICAO,'') as DESCR_JOB from (SELECT lanctos_contab.LOTE   " +
        " ,lanctos_contab.SEQ_LOTE  ,lanctos_contab.DET_LOTE  ,lanctos_contab.LOTE_PAI   " +
        " ,lanctos_contab.SEQ_LOTE_PAI  ,lanctos_contab.DUPLICATA  ,lanctos_contab.SEQ_BAIXA  ,lanctos_contab.DET_BAIXA   " +
        " ,lanctos_contab.DEB_CRED  ,lanctos_contab.DATA  ,lanctos_contab.COD_CONTA   " +
        " ,lanctos_contab.COD_CONTA + ' - ' + cad_contas.descricao as desc_conta  ,lanctos_contab.COD_JOB  ,lanctos_contab.DESC_JOB   " +
        " ,lanctos_contab.COD_LINHA_NEGOCIO  ,lanctos_contab.DESC_LINHA_NEGOCIO  ,lanctos_contab.COD_DIVISAO  ,lanctos_contab.DESC_DIVISAO   " +
        " ,lanctos_contab.COD_CLIENTE  ,lanctos_contab.DESC_CLIENTE  ,lanctos_contab.QTD  ,lanctos_contab.VALOR_UNIT  ,lanctos_contab.VALOR   " +
        " ,lanctos_contab.HISTORICO  ,lanctos_contab.TITULO  ,lanctos_contab.COD_TERCEIRO  ,lanctos_contab.DESC_TERCEIRO   " +
        " ,lanctos_contab.PENDENTE  ,lanctos_contab.MODULO  ,lanctos_contab.COD_MODELO  ,lanctos_contab.COD_EMPRESA   " +
        " ,lanctos_contab.NUMERO_DOCUMENTO  ,lanctos_contab.COD_ORIGEM  ,lanctos_contab.SEQ_ORIGEM  ,lanctos_contab.EFETIVADO   " +
        " ,lanctos_contab.DATA_ANTERIOR  ,lanctos_contab.TIPO_LANCTO  ,lanctos_contab.COD_USUARIO, lanctos_contab.COD_CONSULTOR,  " +
        " lanctos_contab.DESC_CONSULTOR, lanctos_contab.VALOR_BRUTO, lanctos_contab.VALOR_GRUPO FROM LANCTOS_CONTAB, cad_contas  " +
        " WHERE lanctos_contab.cod_conta = cad_contas.cod_conta and (lanctos_contab.LOTE=" + lote + " OR lanctos_contab.LOTE_PAI=" + lote + ")  " +
        " AND lanctos_contab.COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " AND cad_contas.COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + ") x  " +
        " LEFT JOIN CAD_LINHA_NEGOCIOS cln on x.COD_LINHA_NEGOCIO = cln.COD_LINHA_NEGOCIO " +
        " LEFT JOIN CAD_DIVISOES cd on x.COD_DIVISAO = cd.COD_DIVISAO " +
        " LEFT JOIN CAD_EMPRESAS ce on x.COD_CLIENTE = ce.COD_EMPRESA " +
        " LEFT JOIN CAD_JOBS cj on x.COD_JOB = cj.COD_JOB " +
        " ORDER BY x.LOTE,x.SEQ_LOTE,x.DET_LOTE ";

        return _conn.dataTable(sql, "lote");
    }

    public DataTable carregaLoteTemp(double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP WHERE (LOTE=" + lote + " OR LOTE_PAI=" + lote + ") AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY LOTE,SEQ_LOTE,DET_LOTE";
        return _conn.dataTable(sql, "lote");
    }

    public DataTable carregaLotePaiTemp(double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB_TEMP WHERE LOTE=" + lote + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY LOTE,SEQ_LOTE,DET_LOTE";
        return _conn.dataTable(sql, "lote");
    }

    public double getNewNumeroLote()
    {
        string sql = "INSERT INTO CONTROLE_LOTE(LIXO)VALUES(0);SELECT SCOPE_IDENTITY();";

        return Convert.ToDouble(_conn.scalar(sql));
    }

    public double getNewNumeroDuplicata()
    {
        string sql = "INSERT INTO CONTROLE_DUPLICATA(LIXO)VALUES(0);SELECT SCOPE_IDENTITY();";

        return Convert.ToDouble(_conn.scalar(sql));
    }

    public double getNewNumeroBaixa()
    {
        string sql = "INSERT INTO CONTROLE_BAIXA(LIXO)VALUES(0);SELECT SCOPE_IDENTITY();";

        return Convert.ToDouble(_conn.scalar(sql));
    }

    public DataTable getFilhosLote(double lote)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB WHERE LOTE_PAI=" + lote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + "";

        return _conn.dataTable(sql, "filhos");
    }

    public DataTable getFilhosFaturamento(double lote)
    {
        string sql = "SELECT * FROM faturamento_nf WHERE LOTE =" + lote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + "";

        return _conn.dataTable(sql, "Fatura");
    }

    public DataTable getBaixasLote(double lote)
    {
        string sql = "SELECT DISTINCT SEQ_BAIXA FROM LANCTOS_CONTAB WHERE LOTE_PAI=" + lote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " and seq_baixa > 0";

        return _conn.dataTable(sql, "filhos");
    }

    public int totalRegistrosPendentes(string modulo, Nullable<DateTime> periodoDe, Nullable<DateTime>
        periodoAte, Nullable<int> terceiro)
    {
        string sql = "select count(*) from lanctos_contab where 1=1 ";
        sql += " and lanctos_contab.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        sql += " and lanctos_contab.modulo='" + modulo + "' and pendente='True' ";

        if (periodoDe != null)
            sql += " and lanctos_contab.data >='" + periodoDe.Value.ToString("yyyyMMdd") + "'";

        if (periodoAte != null)
            sql += " and lanctos_contab.data <='" + periodoAte.Value.ToString("yyyyMMdd") + "'";

        if (terceiro != null)
            sql += " and lanctos_contab.cod_terceiro=" + terceiro + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPendentes(ref DataTable tb, string modulo, Nullable<DateTime> periodoDe, Nullable<DateTime>
        periodoAte, Nullable<int> terceiro, int paginaAtual, int pag)
    {

        string sql = "select x.* from (SELECT  a.*, ROW_NUMBER() OVER (ORDER BY a.DATA DESC) as Row from (select y.* from (select distinct lote,lote_pai,seq_lote_pai, data, lanctos_contab.cod_empresa, modulo,pendente, historico,cad_contas.descricao as desc_conta, cad_jobs.descricao as desc_job,  " +
                    "(select top 1 modulo from lanctos_contab lancto_pai where lancto_pai.lote=lanctos_contab.lote_pai ) as modulo_pai," +
                    " case when cad_empresas.fisica_juridica = 'FISICA' then cad_empresas.nome_razao_social " +
                    " else cad_empresas.nome_fantasia end as desc_terceiro,cod_terceiro, " +
                    " (select min(cod_conta) from lanctos_contab lc1 where lc1.lote = lanctos_contab.lote and seq_lote =  " +
                    " (select min(seq_lote) from lanctos_contab lc2 where lc2.lote = lanctos_contab.lote)) as cod_conta, " +
                    " (select min(seq_lote) from lanctos_contab lc1 where lc1.lote = lanctos_contab.lote) as seq_lote, " +
                    " (select min(cod_job) from lanctos_contab lc1 where lc1.lote = lanctos_contab.lote and seq_lote =  " +
                    " (select min(seq_lote) from lanctos_contab lc2 where lc2.lote = lanctos_contab.lote)) as cod_job, " +
                    " (select sum(valor) from lanctos_contab lc2 where lc2.lote = lanctos_contab.lote and lc2.deb_cred = 'D') as Debitos, " +
                    " (select sum(valor) from lanctos_contab lc2 where lc2.lote = lanctos_contab.lote and lc2.deb_cred = 'C') as Creditos, " +
                    " (select max(seq_baixa) from lanctos_contab lc2 where lc2.lote = lanctos_contab.lote_pai) as seq_baixa " +
                    " from lanctos_contab left join cad_empresas on lanctos_contab.cod_terceiro = cad_empresas.cod_empresa, " +
                    " cad_contas, cad_jobs " +
                    " where lanctos_contab.cod_conta = cad_contas.cod_conta and cad_contas.cod_empresa = lanctos_contab.cod_empresa and " +
                    " lanctos_contab.cod_job = cad_jobs.cod_job ";
        sql += " and lanctos_contab.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        sql += " and lanctos_contab.modulo='" + modulo + "' and pendente='True' ";

        if (periodoDe != null)
            sql += " and lanctos_contab.data >='" + periodoDe.Value.ToString("yyyyMMdd") + "'";

        if (periodoAte != null)
            sql += " and lanctos_contab.data <='" + periodoAte.Value.ToString("yyyyMMdd") + "'";

        if (terceiro != null)
            sql += " and lanctos_contab.cod_terceiro=" + terceiro + "";

        sql += "   ) y ) a) x where x.row <= " + (((paginaAtual - 1) * pag) + pag) + " AND x.row >=" + ((paginaAtual - 1) * pag) + " ORDER BY x.DATA, x.COD_TERCEIRO";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string modulo, Nullable<double> lote, Nullable<DateTime> dataInicio, Nullable<DateTime> dataTermino, string conta,
        Nullable<int> job, Nullable<int> terceiro, string documento, int paginaAtual, string ordenacao, Nullable<double> codplanilha)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "lote";

        string sql = "select vw.* , " +
                    " dbo.fn_concat_terceiro(VW.lote,VW.cod_empresa) as terceiros, " +
                    " dbo.fn_concat_Job(VW.lote,VW.cod_empresa) as desc_job, " +
                    " (SELECT MAX(CC.COD_CONTA + ' - ' + LC.COD_CONTA ) " +
                    "         FROM LANCTOS_CONTAB LC, CAD_CONTAS CC  " +
                    "         WHERE LC.COD_CONTA = CC.COD_CONTA " +
                    " 		  AND LC.LOTE = VW.LOTE  " +
                    "           AND LC.SEQ_LOTE = (SELECT MIN(DENTRO.SEQ_LOTE) FROM LANCTOS_CONTAB DENTRO WHERE DENTRO.LOTE = VW.LOTE)) AS desc_conta, " +
                    " (SELECT MAX(LC.HISTORICO ) " +
                    "         FROM LANCTOS_CONTAB LC " +
                    "         WHERE LC.LOTE = VW.LOTE  " +
                    "           AND LC.SEQ_LOTE = (SELECT MIN(DENTRO.SEQ_LOTE) FROM LANCTOS_CONTAB DENTRO WHERE DENTRO.LOTE = VW.LOTE)) AS historico, " +
                    "(SELECT MAX(LC.cod_terceiro ) " +
                    " FROM LANCTOS_CONTAB LC " +
                    " WHERE LC.LOTE = VW.LOTE  " +
                    "   AND LC.SEQ_LOTE = (SELECT MIN(DENTRO.SEQ_LOTE) FROM LANCTOS_CONTAB DENTRO WHERE DENTRO.LOTE = VW.LOTE)) AS terceiro, " +
                    "(SELECT MAX(LC.numero_documento ) " +
                    " FROM LANCTOS_CONTAB LC " +
                    " WHERE LC.LOTE = VW.LOTE  " +
                    "   AND LC.SEQ_LOTE = (SELECT MIN(DENTRO.SEQ_LOTE) FROM LANCTOS_CONTAB DENTRO WHERE DENTRO.LOTE = VW.LOTE)) AS numero_documento " +
                    " from " +
                    " (select ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " DESC) AS Row , " +
                    " lanctos_contab.lote, MAX(lanctos_contab.seq_baixa) SEQ_BAIXA, lanctos_contab.data,  " +
                    " lanctos_contab.cod_empresa, lanctos_contab.modulo, lanctos_contab.pendente,  " +
                    " sum(case when lanctos_contab.deb_cred = 'D' then lanctos_contab.valor else 0 end) as debitos, " +
                    " sum(case when lanctos_contab.deb_cred = 'D' then 0 else lanctos_contab.valor end) as creditos " +
                    " from lanctos_contab " +
                    " where  ";

        sql += " cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        sql += " and modulo='" + modulo + "' and pendente='False' and seq_baixa=0";

        if (lote != null)
            sql += " and lote=" + lote.Value + "";

        if (codplanilha != null)
            sql += " and cod_planilha=" + codplanilha.Value + "";

        if (dataInicio != null)
            sql += " and data >='" + dataInicio.Value.ToString("yyyyMMdd") + "'";

        if (dataTermino != null)
            sql += " and data <= '" + dataTermino.Value.ToString("yyyyMMdd") + "'";

        if (!string.IsNullOrEmpty(documento))
            sql += " and numero_documento like '%" + documento + "%'";

        if (conta != null)
            sql += " and cod_conta='" + conta + "'";

        if (job != null)
            sql += " and cod_job=" + job.Value + "";


        if (terceiro != null)
            sql += " and lote in (select distinct lote from lanctos_contab where cod_terceiro=" + terceiro.Value + ") ";

        sql += " group by lanctos_contab.lote, lanctos_contab.data,  " +
                " lanctos_contab.cod_empresa, lanctos_contab.modulo, lanctos_contab.pendente " +
                " ) vw";
        //PAGINACAO
        sql += " where vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string modulo, Nullable<double> lote, Nullable<DateTime> dataInicio, Nullable<DateTime> dataTermino, string conta,
        Nullable<int> job, Nullable<int> terceiro, Nullable<double> codplanilha)
    {
        string sql = "select count(x.lote) from (select distinct lote from lanctos_contab " +
                     "  ";
        sql += " where cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
        sql += " and modulo='" + modulo + "' and pendente='False' and seq_baixa=0 ";

        if (lote != null)
            sql += " and lote=" + lote.Value + "";

        if (codplanilha != null)
            sql += " and cod_planilha=" + codplanilha.Value + "";
        if (dataInicio != null)

            sql += " and data >='" + dataInicio.Value.ToString("yyyyMMdd") + "'";

        if (dataTermino != null)
            sql += " and data <= '" + dataTermino.Value.ToString("yyyyMMdd") + "'";

        if (conta != null)
            sql += " and cod_conta='" + conta + "'";

        if (job != null)
            sql += " and cod_job=" + job.Value + "";

        if (terceiro != null)
            sql += " and lote in (select distinct lote from lanctos_contab where cod_terceiro=" + terceiro.Value + ") ";

        sql += ") x";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaBaixas(ref DataTable tb, double? baixa, Nullable<DateTime> dtInicio, Nullable<DateTime> dtTermino,
        int? terceiro, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "SEQ_BAIXA";

        string sql = "select vw.* from (SELECT  ROW_NUMBER() OVER (ORDER BY p." + tmpOrdenacao + " DESC) AS Row, ";
        sql += " p.*, dbo.fn_concat_terceiro_baixa(p.seq_baixa,p.cod_empresa) as Terceiros from (select seq_baixa, cod_conta, descricao,cod_empresa,data, sum(case when deb_cred = 'D' then valor else -valor end) as valor " +
                    " from (select lanctos_contab.*, cad_contas.descricao as descricao " +
                    " from lanctos_contab, cad_contas where LANCTOS_CONTAB.COD_EMPRESA = CAD_CONTAS.COD_EMPRESA and lanctos_contab.seq_baixa <> 0 " +
                    " and cad_contas.cod_tipo_conta = 'BC' and cad_contas.cod_conta = lanctos_contab.cod_conta) x " +
                    " group by seq_baixa, cod_conta, descricao,cod_empresa,data) p ";
        sql += " where p.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

        if (baixa != null)
            sql += " AND seq_baixa = " + baixa.Value + " ";

        if (dtInicio != null)
            sql += " AND DATA >= '" + dtInicio.Value.ToString("yyyyMMdd") + "' ";

        if (dtTermino != null)
            sql += " AND DATA <= '" + dtTermino.Value.ToString("yyyyMMdd") + "' ";

        if (terceiro != null)
            sql += " and seq_baixa in (select distinct seq_baixa from lanctos_contab where cod_terceiro=" + terceiro.Value + ") ";


        sql += " ) as vw where 1=1 ";
        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);

    }

    public int totalRegistrosBaixa(double? baixa, Nullable<DateTime> dtInicio, Nullable<DateTime> dtTermino,
        int? terceiro)
    {
        string sql = "select count(*) from " +
                    " (select seq_baixa, cod_conta, descricao,cod_empresa,data, sum(case when deb_cred = 'D' then valor else -valor end) as valor " +
                    " from (select lanctos_contab.*, cad_contas.descricao as descricao " +
                    " from lanctos_contab, cad_contas where LANCTOS_CONTAB.COD_EMPRESA = CAD_CONTAS.COD_EMPRESA and lanctos_contab.seq_baixa <> 0 " +
                    " and cad_contas.cod_tipo_conta = 'BC' and cad_contas.cod_conta = lanctos_contab.cod_conta) x " +
                    " where cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

        if (baixa != null)
            sql += " AND seq_baixa = " + baixa.Value + " ";

        if (dtInicio != null)
            sql += " AND DATA >= '" + dtInicio.Value.ToString("yyyyMMdd") + "' ";

        if (dtTermino != null)
            sql += " AND DATA <= '" + dtTermino.Value.ToString("yyyyMMdd") + "' ";

        if (terceiro != null)
            sql += " and seq_baixa in (select distinct seq_baixa from lanctos_contab where cod_terceiro=" + terceiro.Value + ") ";


        sql += " group by seq_baixa, cod_conta, descricao,cod_empresa,data) p";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable getLancamentoPai(double lote, int seqLote, string modulo)
    {
        string sql = "select distinct * from lanctos_contab where lote=" + lote + " and seq_lote=" + seqLote + " and cod_empresa=" + HttpContext.Current.Session["empresa"] + " and modulo='" + modulo + "'";

        return _conn.dataTable(sql, "lancamento");
    }

    public double getLoteAnterior(string modulo, double codigoTerceiro)
    {
        string sql = "select lote from lanctos_contab where lote in (select  top 1 lote from lanctos_contab where LOTE_PAI=0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + " and modulo='" + modulo + "' and COD_TERCEIRO = " + codigoTerceiro + " " +
                    " group by lote " +
                    " order by LOTE desc)";

        return Convert.ToDouble(_conn.scalar(sql));
    }

    public int totalLotesAnteriores(string modulo, double codigoTerceiro)
    {
        string sql = "select  isnull(COUNT(*),0) from lanctos_contab where LOTE_PAI=0 and cod_empresa=" + HttpContext.Current.Session["empresa"] + " and modulo='" + modulo + "' and COD_TERCEIRO = " + codigoTerceiro + " ";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable getFilhosBaixas(double lote)
    {
        string sql = "select distinct lote,seq_baixa,data from lanctos_contab where lote_pai=" + lote + "";
        return _conn.dataTable(sql, "FilhosBaixas");
    }

    public DataTable getFilhosBaixas(double lote, int seqLote)
    {
        string sql = " select lote,seq_baixa,det_baixa,data,data_anterior,sum(valor) as valor " +
                    " from lanctos_contab  " +
                    " where lote_pai=" + lote + " and seq_lote_pai=" + seqLote + " and seq_baixa > 0  " +
                    " group by lote,seq_baixa,det_baixa,data,data_anterior";

        return _conn.dataTable(sql, "FilhosBaixas");
    }

    public int getFornecedor(double lote, int codEmpresa)
    {
        string sql = "select distinct cod_terceiro from lanctos_contab where lote=" + lote + " and seq_lote=1 and pendente='False' and cod_empresa=" + codEmpresa;

        object result = _conn.scalar(sql);
        return Convert.ToInt32(result);
    }

    public DateTime getData(double lote, int codEmpresa)
    {
        string sql = "select distinct data from lanctos_contab where lote=" + lote + " and seq_lote=1 and pendente='False' and cod_empresa=" + codEmpresa;

        object result = _conn.scalar(sql);
        return Convert.ToDateTime(result);
    }

    public DataTable lancamentosRecibo(DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select lanctos_contab.*,case when cod_terceiro = 0 and modulo <> 'C_INCLUSAO_LANCTO' " +
                    " then (select distinct cod_terceiro from LANCTOS_CONTAB dentro  " +
                    " where dentro.LOTE=LANCTOS_CONTAB.LOTE and dentro.SEQ_LOTE=1 and dentro.COD_EMPRESA=" + codEmpresa + ") " +
                    " else COD_TERCEIRO end as COD_TERCEIRO_CORRETO from LANCTOS_CONTAB,CAD_CONTAS " +
                    " where PENDENTE='False' and DATA>= '" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' " +
                    " and LANCTOS_CONTAB.COD_EMPRESA = " + codEmpresa + " and CAD_CONTAS.COD_EMPRESA=" + codEmpresa + " " +
                    " AND LANCTOS_CONTAB.COD_CONTA = CAD_CONTAS.COD_CONTA " +
                    " AND CAD_CONTAS.RECIBO=1";

        return _conn.dataTable(sql, "lanctos");
    }

    public DataTable lancamentosReciboSumValor(DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select (case when x.historico like '%alugue%' or x.historico like '%alugué%' then '99' else '01' end) as tipo, isnull(sum(case when DEB_CRED = 'c' then x.valor else - x.valor end),0) as valor from (select lanctos_contab.*,case when cod_terceiro = 0 and modulo <> 'C_INCLUSAO_LANCTO' " +
                    " then (select distinct cod_terceiro from LANCTOS_CONTAB dentro  " +
                    " where dentro.LOTE=LANCTOS_CONTAB.LOTE and dentro.SEQ_LOTE=1 and dentro.COD_EMPRESA=" + codEmpresa + ") " +
                    " else COD_TERCEIRO end as COD_TERCEIRO_CORRETO from LANCTOS_CONTAB,CAD_CONTAS " +
                    " where PENDENTE='False' and DATA>= '" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' " +
                    " and LANCTOS_CONTAB.COD_EMPRESA = " + codEmpresa + " and CAD_CONTAS.COD_EMPRESA=" + codEmpresa + " " +
                    " and LANCTOS_CONTAB.TIPO_LANCTO <> 'E' AND LANCTOS_CONTAB.COD_CONTA = CAD_CONTAS.COD_CONTA " +
                    " AND CAD_CONTAS.RECIBO=1) x group by (case when x.historico like '%alugue%' or x.historico like '%alugué%' then '99' else '01' end)";

        return _conn.dataTable(sql, "Resumo");
    }

    public DataTable lancamentosReciboBlocoF525(DateTime inicio, DateTime termino, int codEmpresa, bool documentofiscal)
    {
        string sql = "select (case when x.historico like '%alugue%' or x.historico like '%alugué%' then '99' else '01' end) as tipo, x.cod_empresa, emp.cnpj_cpf, " + (documentofiscal ? "(case when isnull(x.numero_documento,'') = '' then 'S/N' else x.numero_documento end) as numero_documento" : "x.cod_cliente, (select CNPJ_CPF from cad_empresas where cod_empresa = x.cod_cliente) as CNPJ") + " , isnull(sum(case when deb_cred = 'C' then x.valor else -x.valor end),0) as valor from (select LANCTOS_CONTAB.*,case when cod_terceiro = 0 and modulo <> 'C_INCLUSAO_LANCTO'  then " +
                    " (select distinct cod_terceiro from LANCTOS_CONTAB dentro   where dentro.LOTE=LANCTOS_CONTAB.LOTE and " +
                    " dentro.SEQ_LOTE=1 and dentro.COD_EMPRESA=" + codEmpresa + ")  else COD_TERCEIRO end as COD_TERCEIRO_CORRETO from " +
                    " LANCTOS_CONTAB,CAD_CONTAS where PENDENTE='False' and DATA>= '" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "'  " +
                    " and LANCTOS_CONTAB.TIPO_LANCTO <> 'E' and LANCTOS_CONTAB.COD_EMPRESA = " + codEmpresa + " and CAD_CONTAS.COD_EMPRESA=" + codEmpresa + " AND LANCTOS_CONTAB.COD_CONTA = CAD_CONTAS.COD_CONTA  " +
                    " AND CAD_CONTAS.RECIBO=1) x, CAD_EMPRESAS emp where x.COD_EMPRESA = emp.COD_EMPRESA  group by x.cod_empresa, emp.cnpj_cpf, (case when x.historico like '%alugue%' or x.historico like '%alugué%' then '99' else '01' end), " + (documentofiscal ? "x.numero_documento" : "x.cod_cliente");

        return _conn.dataTable(sql, "lanctos");
    }

    public DataTable BuscaAliqImpostos(int codEmpresa)
    {
        string sql = "select * from TIPOS_IMPOSTO_ALIQUOTA where COD_EMPRESA = " + codEmpresa;

        return _conn.dataTable(sql, "AliqImpostos");
    }


    public DataTable lancamentosDia(DateTime dtInicio, DateTime dtFinal)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB WHERE LANCTOS_CONTAB.DATA >= '" + dtInicio.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB.DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " and PENDENTE = 'false' ORDER BY LOTE, SEQ_LOTE, DET_LOTE ";
        return _conn.dataTable(sql, "lanctos");
    }

    public DataTable lancamentosDiaCTA(DateTime dtInicio, DateTime dtFinal)
    {
        string sql = "SELECT * FROM LANCTOS_CONTAB WHERE TIPO_LANCTO <> 'E' AND LANCTOS_CONTAB.DATA >= '" + dtInicio.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB.DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " and PENDENTE = 'false' ORDER BY LOTE, SEQ_LOTE, DET_LOTE ";
        return _conn.dataTable(sql, "lanctos");
    }

    public void lancamentosZeraPeriodoMoeda(DateTime dtInicio, DateTime dtFinal)
    {
        string sql = "DELETE LANCTOS_CONTAB_MOEDA WHERE LANCTOS_CONTAB_MOEDA.DATA >= '" + dtInicio.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB_MOEDA.DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB_MOEDA.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND LANCTOS_CONTAB_MOEDA.INSERCAO_MANUAL = 'False' ";
        _conn.execute(sql);
    }

    public DataTable lancamentosBalanco(DateTime dtFinal)
    {
        string sql = "SELECT CAD_CONTAS.DESCRICAO AS DESC_CONTA, LANCTOS_CONTAB.COD_CONTA ,SUM((CASE WHEN LANCTOS_CONTAB.DEB_CRED = 'D' THEN LANCTOS_CONTAB.VALOR ELSE -LANCTOS_CONTAB.VALOR END)) AS VALOR, CAD_CONTAS.COD_MOEDA_BALANCO AS COD_MOEDA_BALANCO " +
                       " FROM LANCTOS_CONTAB, CAD_CONTAS  " +
                       " WHERE LANCTOS_CONTAB.COD_CONTA = CAD_CONTAS.COD_CONTA AND LANCTOS_CONTAB.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND LANCTOS_CONTAB.DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB.COD_EMPRESA = CAD_CONTAS.COD_EMPRESA and LANCTOS_CONTAB.pendente = 'false' " +
                       " GROUP BY LANCTOS_CONTAB.COD_CONTA, CAD_CONTAS.COD_MOEDA_BALANCO, CAD_CONTAS.DESCRICAO ORDER BY LANCTOS_CONTAB.COD_CONTA ";

        return _conn.dataTable(sql, "lanctos");
    }

    public DataTable lancamentosBalancoMoeda(DateTime dtFinal)
    {
        string sql = "SELECT CAD_CONTAS.DESCRICAO AS DESC_CONTA, LANCTOS_CONTAB_MOEDA.COD_CONTA, SUM((CASE WHEN LANCTOS_CONTAB_MOEDA.DEB_CRED = 'D' THEN LANCTOS_CONTAB_MOEDA.VALOR ELSE -LANCTOS_CONTAB_MOEDA.VALOR END)) AS VALOR, CAD_CONTAS.COD_MOEDA_BALANCO AS COD_MOEDA_BALANCO " +
                       " FROM LANCTOS_CONTAB_MOEDA, CAD_CONTAS  " +
                       " WHERE LANCTOS_CONTAB_MOEDA.COD_CONTA = CAD_CONTAS.COD_CONTA AND LANCTOS_CONTAB_MOEDA.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND LANCTOS_CONTAB_MOEDA.DATA <= '" + dtFinal.ToString("yyyyMMdd") + "' AND LANCTOS_CONTAB_MOEDA.COD_EMPRESA = CAD_CONTAS.COD_EMPRESA  and LANCTOS_CONTAB_MOEDA.pendente = 'false' " +
                       " GROUP BY LANCTOS_CONTAB_MOEDA.COD_CONTA, CAD_CONTAS.COD_MOEDA_BALANCO, CAD_CONTAS.DESCRICAO ORDER BY LANCTOS_CONTAB_MOEDA.COD_CONTA ";

        return _conn.dataTable(sql, "lanctos");
    }

    public int retornaCodCliente(int lote)
    {
        string sql = "select ISNULL(sum(COD_CLIENTE),0) as COD_CLIENTE from LANCTOS_CONTAB where LOTE = " + lote + " and SEQ_LOTE = 1 and COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DateTime retornaDataEmissao(int lote)
    {
        string sql = "select TOP 1 isnull(max(DATA),GETDATE()) as DATA from LANCTOS_CONTAB where LOTE = " + lote + " and SEQ_LOTE = 1 and COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return Convert.ToDateTime(_conn.scalar(sql));
    }
}
