using System;
using System.Data;
using System.Web;

public class contabilizacaoDAO
{
    private Conexao _conn;

	public contabilizacaoDAO(Conexao c)
	{
        _conn = c;
	}

    //SELECT FATURAMENTO_CONTABILIZACAO_: SERVICOS, RETENCOES, TRIBUTOS e GLOBAL
    public void Select_Servicos(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        string sql = "SELECT FCS.* FROM FATURAMENTO_CONTABILIZACAO_SERVICOS FCS, CAD_SERVICOS CS ";
        sql += "WHERE FCS.COD_SERVICO = CS.COD_SERVICO AND FCS.COD_EMPRESA = CS.COD_EMPRESA AND ";
        sql += "FCS.COD_EMITENTE = " + cod_emitente + " AND FCS.COD_EMPRESA = " + cod_empresa;
        sql += " ORDER BY CS.NOME";
        _conn.fill(sql, ref tb);
    }

    public void Select_Retencoes(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        string sql = "SELECT FCR.* FROM FATURAMENTO_CONTABILIZACAO_RETENCOES FCR, CAD_RETENCOES CR ";
        sql += "WHERE FCR.COD_RETENCAO = CR.COD_RETENCAO AND FCR.COD_EMPRESA = CR.COD_EMPRESA AND ";
        sql += "FCR.COD_EMITENTE = " + cod_emitente + " AND FCR.COD_EMPRESA = " + cod_empresa;
        sql += " ORDER BY CR.NOME";
        _conn.fill(sql, ref tb);
    }

    public void Select_Tributos(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        string sql = "SELECT FCT.* FROM FATURAMENTO_CONTABILIZACAO_TRIBUTOS FCT, CAD_TRIBUTOS CT ";
        sql += "WHERE FCT.COD_TRIBUTO = CT.COD_TRIBUTO AND FCT.COD_EMPRESA = CT.COD_EMPRESA AND ";
        sql += "FCT.COD_EMITENTE = " + cod_emitente + " AND FCT.COD_EMPRESA = " + cod_empresa;
        sql += " ORDER BY CT.NOME";
        _conn.fill(sql, ref tb);
    }

    public void Select_Global(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        string sql = "SELECT * FROM FATURAMENTO_CONTABILIZACAO_GLOBAL WHERE COD_EMITENTE = " + cod_emitente + " AND COD_EMPRESA = " + cod_empresa;
        _conn.fill(sql, ref tb);
    }



    //INSERT FATURAMENTO_CONTABILIZACAO_: SERVICOS, RETENCOES, TRIBUTOS e GLOBAL
    public void Insert_Servicos(int cod_emitente, int cod_servico, int cod_empresa, 
        string cod_conta_debito, string bruto_liquido_debito, bool gera_titulo_debito, string historico_debito, 
        string cod_conta_credito, string bruto_liquido_credito, bool gera_titulo_credito, string historico_credito)
    {
        string sql = "INSERT INTO FATURAMENTO_CONTABILIZACAO_SERVICOS ";
        sql += "(COD_EMITENTE, COD_SERVICO, COD_EMPRESA, ";
        sql += "COD_CONTA_DEBITO, BRUTO_LIQUIDO_DEBITO, GERA_TITULO_DEBITO, HISTORICO_DEBITO,  ";
        sql += "COD_CONTA_CREDITO, BRUTO_LIQUIDO_CREDITO, GERA_TITULO_CREDITO, HISTORICO_CREDITO) ";
        sql += "VALUES (" + cod_emitente + ", " + cod_servico + ", " + cod_empresa + ", '";
        sql += cod_conta_debito.Replace("'", "''") + "', '" + bruto_liquido_debito + "', '" + gera_titulo_debito + "', '" + historico_debito.Replace("'", "''") + "', '";
        sql += cod_conta_credito.Replace("'", "''") + "', '" + bruto_liquido_credito + "', '" + gera_titulo_credito + "', '" + historico_credito.Replace("'", "''") + "')";

        _conn.execute(sql);
    }

    public void Insert_Retencoes(int cod_emitente, int cod_retencao, int cod_empresa, 
        string cod_conta_debito, bool gera_titulo_debito, int cod_terceiro_debito, string historico_debito, 
        string cod_conta_credito, bool gera_titulo_credito, int cod_terceiro_credito, string historico_credito)
    {
        string sql = "INSERT INTO FATURAMENTO_CONTABILIZACAO_RETENCOES ";
        sql += "(COD_EMITENTE, COD_RETENCAO, COD_EMPRESA, ";
        sql += "COD_CONTA_DEBITO, GERA_TITULO_DEBITO, COD_TERCEIRO_DEBITO, HISTORICO_DEBITO, ";
        sql += "COD_CONTA_CREDITO, GERA_TITULO_CREDITO, COD_TERCEIRO_CREDITO, HISTORICO_CREDITO) ";
        sql += "VALUES (" + cod_emitente + ", " + cod_retencao + ", " + cod_empresa + ", '";
        sql += cod_conta_debito.Replace("'", "''") + "', '" + gera_titulo_debito + "', " + cod_terceiro_debito + ", '" + historico_debito.Replace("'", "''") + "', '";
        sql += cod_conta_credito.Replace("'", "''") + "', '" + gera_titulo_credito + "', " + cod_terceiro_credito + ", '" + historico_credito.Replace("'", "''") + "')";

        _conn.execute(sql);
    }

    public void Insert_Tributos(int cod_emitente, int cod_tributo, int cod_empresa, 
        string cod_conta_debito, bool gera_titulo_debito, int cod_terceiro_debito, string historico_debito, 
        string cod_conta_credito, bool gera_titulo_credito, int cod_terceiro_credito, string historico_credito)
    {
        string sql = "INSERT INTO FATURAMENTO_CONTABILIZACAO_TRIBUTOS ";
        sql += "(COD_EMITENTE, COD_TRIBUTO, COD_EMPRESA, ";
        sql += "COD_CONTA_DEBITO, GERA_TITULO_DEBITO, COD_TERCEIRO_DEBITO, HISTORICO_DEBITO, ";
        sql += "COD_CONTA_CREDITO, GERA_TITULO_CREDITO, COD_TERCEIRO_CREDITO, HISTORICO_CREDITO) ";
        sql += "VALUES (" + cod_emitente + ", " + cod_tributo + ", " + cod_empresa + ", '";
        sql += cod_conta_debito.Replace("'", "''") + "', '" + gera_titulo_debito + "', " + cod_terceiro_debito + ", '" + historico_debito.Replace("'", "''") + "', '";
        sql += cod_conta_credito.Replace("'", "''") + "', '" + gera_titulo_credito + "', " + cod_terceiro_credito + ", '" + historico_credito.Replace("'", "''") + "')";

        _conn.execute(sql);
    }

    public void Insert_Global(int cod_emitente, int cod_empresa, string cod_conta_diferenca, string debito_credito, string historico, bool gera_titulo)
    {
        string sql = "INSERT INTO FATURAMENTO_CONTABILIZACAO_GLOBAL (COD_EMITENTE, COD_EMPRESA, COD_CONTA_DIFERENCA, DEBITO_CREDITO, HISTORICO, GERA_TITULO) ";
        sql += "VALUES (" + cod_emitente + ", " + cod_empresa + ", '" + cod_conta_diferenca.Replace("'", "''") + "', '";
        sql += debito_credito + "', '" + historico.Replace("'", "''") + "', '" + gera_titulo + "')";
        
        _conn.execute(sql);
    }



    //UPDATE FATURAMENTO_CONTABILIZACAO_: SERVICOS, RETENCOES, TRIBUTOS e GLOBAL
    public void Update_Servicos(int cod_emitente, int cod_servico, int cod_empresa,
        string cod_conta_debito, string bruto_liquido_debito, bool gera_titulo_debito, string historico_debito,
        string cod_conta_credito, string bruto_liquido_credito, bool gera_titulo_credito, string historico_credito)
    {
        string sql = "UPDATE FATURAMENTO_CONTABILIZACAO_SERVICOS SET ";
        sql += "COD_CONTA_DEBITO = '" + cod_conta_debito.Replace("'", "''") + "', BRUTO_LIQUIDO_DEBITO = '" + bruto_liquido_debito + "', GERA_TITULO_DEBITO = '" + gera_titulo_debito + "', HISTORICO_DEBITO = '" + historico_debito.Replace("'", "''") + "', ";
        sql += "COD_CONTA_CREDITO = '" + cod_conta_credito.Replace("'", "''") + "', BRUTO_LIQUIDO_CREDITO = '" + bruto_liquido_credito + "', GERA_TITULO_CREDITO = '" + gera_titulo_credito + "', HISTORICO_CREDITO = '" + historico_credito.Replace("'", "''") + "' ";
        sql += "WHERE COD_EMITENTE = " + cod_emitente + " AND COD_SERVICO = " + cod_servico + " AND COD_EMPRESA = " + cod_empresa;

        _conn.execute(sql);
    }

    public void Update_Retencoes(int cod_emitente, int cod_retencao, int cod_empresa,
    string cod_conta_debito, bool gera_titulo_debito, int cod_terceiro_debito, string historico_debito,
    string cod_conta_credito, bool gera_titulo_credito, int cod_terceiro_credito, string historico_credito)
    {
        string sql = "UPDATE FATURAMENTO_CONTABILIZACAO_RETENCOES SET ";
        sql += "COD_CONTA_DEBITO = '" + cod_conta_debito.Replace("'", "''") + "', GERA_TITULO_DEBITO = '" + gera_titulo_debito + "', COD_TERCEIRO_DEBITO = " + cod_terceiro_debito + ", HISTORICO_DEBITO = '" + historico_debito.Replace("'", "''") + "', ";
        sql += "COD_CONTA_CREDITO = '" + cod_conta_credito.Replace("'", "''") + "', GERA_TITULO_CREDITO = '" + gera_titulo_credito + "', COD_TERCEIRO_CREDITO = " + cod_terceiro_credito + ", HISTORICO_CREDITO = '" + historico_credito.Replace("'", "''") + "' ";
        sql += "WHERE COD_EMITENTE = " + cod_emitente + " AND COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + cod_empresa;

        _conn.execute(sql);
    }

    public void Update_Tributos(int cod_emitente, int cod_tributo, int cod_empresa,
    string cod_conta_debito, bool gera_titulo_debito, int cod_terceiro_debito, string historico_debito,
    string cod_conta_credito, bool gera_titulo_credito, int cod_terceiro_credito, string historico_credito)
    {
        string sql = "UPDATE FATURAMENTO_CONTABILIZACAO_TRIBUTOS SET ";
        sql += "COD_CONTA_DEBITO = '" + cod_conta_debito.Replace("'", "''") + "', GERA_TITULO_DEBITO = '" + gera_titulo_debito + "', COD_TERCEIRO_DEBITO = " + cod_terceiro_debito + ", HISTORICO_DEBITO = '" + historico_debito.Replace("'", "''") + "', ";
        sql += "COD_CONTA_CREDITO = '" + cod_conta_credito.Replace("'", "''") + "', GERA_TITULO_CREDITO = '" + gera_titulo_credito + "', COD_TERCEIRO_CREDITO = " + cod_terceiro_credito + ", HISTORICO_CREDITO = '" + historico_credito.Replace("'", "''") + "' ";
        sql += "WHERE COD_EMITENTE = " + cod_emitente + " AND COD_TRIBUTO = " + cod_tributo + " AND COD_EMPRESA = " + cod_empresa;

        _conn.execute(sql);
    }

    public void Update_Global(int cod_emitente, int cod_empresa, string cod_conta_diferenca, string debito_credito, string historico, bool gera_titulo)
    {
        string sql = "UPDATE FATURAMENTO_CONTABILIZACAO_GLOBAL SET ";
        sql += "COD_CONTA_DIFERENCA = '" + cod_conta_diferenca.Replace("'", "''") + "', DEBITO_CREDITO = '" + debito_credito + "', ";
        sql += "HISTORICO = '" + historico.Replace("'", "''") + "', GERA_TITULO = '" + gera_titulo + "' ";
        sql += "WHERE COD_EMITENTE = " + cod_emitente + " AND COD_EMPRESA = " + cod_empresa;

        _conn.execute(sql);
    }

    //Função "Contabilizar" da classe "Contabilizacao_Funcoes.cs"     || INÍCIO ||
    public DataTable Busca_Dados_NF(int cod_faturamento_nf)
    {
        string sql = "SELECT * FROM FATURAMENTO_NF WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Dados_NF");
    }

    public DataTable Busca_Servicos_NF(int cod_faturamento_nf)
    {
        string sql = "SELECT * FROM FATURAMENTO_NF_SERVICOS_JOBS WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " ORDER BY COD_SERVICO";
        return _conn.dataTable(sql, "Busca_Servicos_NF");
    }

    public DataTable Busca_Retencoes_NF(int cod_faturamento_nf)
    {
        string sql = "SELECT * FROM FATURAMENTO_NF_RETENCOES WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " ORDER BY COD_RETENCAO";
        return _conn.dataTable(sql, "Busca_Retencoes_NF");
    }

    public DataTable Busca_Tributos_NF(int cod_faturamento_nf)
    {
        string sql = "SELECT * FROM FATURAMENTO_NF_TRIBUTOS WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " ORDER BY COD_TRIBUTO";
        return _conn.dataTable(sql, "Busca_Tributos_NF");
    }

    public DataTable Busca_Vencimentos_NF(int cod_faturamento_nf)
    {
        string sql = "SELECT * FROM FATURAMENTO_NF_DATA_VENCIMENTO WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " ORDER BY DATA_VENCIMENTO";
        return _conn.dataTable(sql, "Busca_Vencimentos_NF");
    }

    public DataTable Busca_Parametros_Servicos(int cod_emitente, int cod_servico)
    {
        string sql = "SELECT * FROM FATURAMENTO_CONTABILIZACAO_SERVICOS WHERE COD_EMITENTE = " + cod_emitente + " AND COD_SERVICO = " + cod_servico + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Parametros_Servicos");
    }

    public DataTable Busca_Parametros_Retencoes(int cod_emitente, int cod_retencao)
    {
        string sql = "SELECT * FROM FATURAMENTO_CONTABILIZACAO_RETENCOES WHERE COD_EMITENTE = " + cod_emitente + " AND COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Parametros_Retencoes");
    }

    public DataTable Busca_Parametros_Tributos(int cod_emitente, int cod_tributo)
    {
        string sql = "SELECT * FROM FATURAMENTO_CONTABILIZACAO_TRIBUTOS WHERE COD_EMITENTE = " + cod_emitente + " AND COD_TRIBUTO = " + cod_tributo + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Parametros_Tributos");
    }

    public DataTable Busca_Parametros_Global(int cod_emitente)
    {
        string sql = "SELECT * FROM FATURAMENTO_CONTABILIZACAO_GLOBAL WHERE COD_EMITENTE = " + cod_emitente + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Parametros_Global");
    }

    public DataTable Busca_Dados_Conta(string cod_conta)
    {
        string sql = "SELECT CC.*, CGC.REGRA_EXIBICAO FROM CAD_CONTAS cc, CAD_GRUPOS_CONTABEIS cgc WHERE cc.COD_CONTA = '" + cod_conta + "' AND cc.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND cc.ANALITICA <> 0 AND cc.COD_EMPRESA = CGC.COD_EMPRESA AND CC.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL";
        //string sql = "SELECT * FROM CAD_CONTAS WHERE COD_CONTA = '" + cod_conta + "' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND ANALITICA <> 0";
        return _conn.dataTable(sql, "Busca_Dados_Conta");
    }

    public DataTable Busca_Dados_Job(int cod_job)
    {
        string sql = "SELECT CJ.COD_JOB, CJ.DESCRICAO, CJ.COD_CLIENTE, CE.NOME_RAZAO_SOCIAL, CJ.COD_LINHA_NEGOCIO, ";
        sql += "CLN.DESCRICAO AS DESC_LINHA, CJ.COD_DIVISAO, CD.DESCRICAO AS DESC_DIVISAO ";
        sql += "FROM CAD_JOBS CJ, CAD_DIVISOES CD, CAD_EMPRESAS CE, CAD_LINHA_NEGOCIOS CLN ";
        sql += "WHERE CJ.COD_DIVISAO = CD.COD_DIVISAO AND CJ.COD_EMPRESA = CD.COD_EMPRESA ";
        sql += "AND CJ.COD_CLIENTE = CE.COD_EMPRESA AND CJ.COD_EMPRESA = CE.COD_EMPRESA_PAI ";
        sql += "AND CJ.COD_LINHA_NEGOCIO = CLN.COD_LINHA_NEGOCIO AND CJ.COD_EMPRESA = CLN.COD_EMPRESA ";
        sql += "AND CJ.COD_JOB = " + cod_job + " AND CJ.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "Busca_Dados_Job");
    }

    public string Busca_Nome_Empresa(int cod_empresa)
    {
        string sql = "SELECT NOME_RAZAO_SOCIAL FROM CAD_EMPRESAS WHERE COD_EMPRESA = " + cod_empresa + " AND COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"];
        return Convert.ToString(_conn.scalar(sql));
    }
    //Função "Contabilizar" da classe "Contabilizacao_Funcoes.cs"     || FIM ||
}