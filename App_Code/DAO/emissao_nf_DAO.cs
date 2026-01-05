using System;
using System.Data;
using System.Web;

public class emissao_nf_DAO
{
    private Conexao _conn;

    public emissao_nf_DAO(Conexao c)
    {
        _conn = c;
    }

    public DateTime Ultima_Data_RPS(int cod_emitente)
    {
        string sql = "SELECT ISNULL(MAX(DATA_EMISSAO_RPS),'20000101') FROM FATURAMENTO_NF WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND COD_EMITENTE = " + cod_emitente;
        return Convert.ToDateTime(_conn.scalar(sql));
    }

    public int Nova_NF_Emitente(int cod_emitente)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_EMITENTE_" + cod_emitente + " (TEMP) VALUES (0); SELECT SCOPE_IDENTITY()";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    // --- ALTERAÇÃO: Adicionei os parametros de IBS e CBS no final ---
    public int Nova_NF_Faturamento(int numero_rps, DateTime data_emissao_rps, DateTime data_competencia, string serie_rps,
        int cod_emitente, string nome_razao_social_emitente, string indicador_cpf_cnpj_emitente, string cpf_cnpj_emitente, string im_emitente, string ie_emitente,
        string endereco_emitente, string num_endereco_emitente, string compl_endereco_emitente, string bairro_emitente, string municipio_emitente, string uf_emitente, string cep_emitente,
        int cod_tomador, string nome_razao_social_tomador, string indicador_cpf_cnpj_tomador, string cpf_cnpj_tomador, string im_tomador, string ie_tomador,
        string endereco_tomador, string num_endereco_tomador, string compl_endereco_tomador, string bairro_tomador, string municipio_tomador, string uf_tomador, string cep_tomador,
        int cod_natureza_operacao, string nome_natureza_operacao, string descricao_natureza_operacao, string natureza_operacao,
        int cod_prestacao_servico, string nome_prestacao_servico, string descricao_prestacao_servico, string valor_total_servico_job, string observacoes,
        decimal valor_ibs, decimal aliquota_ibs, decimal valor_cbs, decimal aliquota_cbs) // <--- NOVOS PARÂMETROS AQUI
    {
        //39 Variáveis + 5 Colunas que não precisam de Variáveis + 4 Colunas Reforma = 48 Colunas

        string sql = "INSERT INTO FATURAMENTO_NF (NUMERO_NF, DATA_EMISSAO_NF, NUMERO_RPS, DATA_EMISSAO_RPS, DATA_COMPETENCIA, TIPO_RPS, SERIE_RPS, ";
        sql += "COD_EMPRESA, COD_EMITENTE, NOME_RAZAO_SOCIAL_EMITENTE, INDICADOR_CPF_CNPJ_EMITENTE, CPF_CNPJ_EMITENTE, IM_EMITENTE, IE_EMITENTE, ";
        sql += "ENDERECO_EMITENTE, NUM_ENDERECO_EMITENTE, COMPL_ENDERECO_EMITENTE, BAIRRO_EMITENTE, MUNICIPIO_EMITENTE, UF_EMITENTE, CEP_EMITENTE, ";
        sql += "COD_TOMADOR, NOME_RAZAO_SOCIAL_TOMADOR, INDICADOR_CPF_CNPJ_TOMADOR, CPF_CNPJ_TOMADOR, IM_TOMADOR, IE_TOMADOR, ";
        sql += "ENDERECO_TOMADOR, NUM_ENDERECO_TOMADOR, COMPL_ENDERECO_TOMADOR, BAIRRO_TOMADOR, MUNICIPIO_TOMADOR, UF_TOMADOR, CEP_TOMADOR, ";
        sql += "COD_NATUREZA_OPERACAO, NOME_NATUREZA_OPERACAO, DESCRICAO_NATUREZA_OPERACAO, NATUREZA_OPERACAO, ";
        sql += "COD_PRESTACAO_SERVICO, NOME_PRESTACAO_SERVICO, DESCRICAO_PRESTACAO_SERVICO, SITUACAO_NF, VALOR_SERVICOS, DISCRIMINACAO_OBSERVACAO, ";
        // --- COLUNAS NOVAS ---
        sql += "VALOR_IBS, ALIQUOTA_IBS, VALOR_CBS, ALIQUOTA_CBS) ";

        sql += "VALUES (0, '" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "', " + numero_rps + ", '" + data_emissao_rps.ToString("yyyyMMdd") + "', " + Convert.ToString(data_competencia != Convert.ToDateTime("01/01/0001") ? "'" + data_competencia.ToString("yyyyMMdd") + "'" : "NULL") + ", 'RPSG', '" + serie_rps + "', ";
        sql += HttpContext.Current.Session["empresa"] + ", " + cod_emitente + ", '" + nome_razao_social_emitente.Replace("'", "''") + "', '" + indicador_cpf_cnpj_emitente + "', '" + cpf_cnpj_emitente + "', '" + im_emitente + "', '" + ie_emitente + "', ";
        sql += "'" + endereco_emitente.Replace("'", "''") + "', '" + num_endereco_emitente.Replace("'", "''") + "', '" + compl_endereco_emitente.Replace("'", "''") + "', '" + bairro_emitente.Replace("'", "''") + "', '" + municipio_emitente.Replace("'", "''") + "', '" + uf_emitente + "', '" + cep_emitente + "', ";
        sql += cod_tomador + ", '" + nome_razao_social_tomador.Replace("'", "''") + "', '" + indicador_cpf_cnpj_tomador + "', '" + cpf_cnpj_tomador + "', '" + im_tomador + "', '" + ie_tomador + "', ";
        sql += "'" + endereco_tomador.Replace("'", "''") + "', '" + num_endereco_tomador.Replace("'", "''") + "', '" + compl_endereco_tomador.Replace("'", "''") + "', '" + bairro_tomador.Replace("'", "''") + "', '" + municipio_tomador.Replace("'", "''") + "', '" + uf_tomador + "', '" + cep_tomador + "', ";
        sql += cod_natureza_operacao + ", '" + nome_natureza_operacao.Replace("'", "''") + "', '" + descricao_natureza_operacao.Replace("'", "''") + "', '" + natureza_operacao.Replace("'", "''") + "', ";
        sql += cod_prestacao_servico + ", '" + nome_prestacao_servico.Replace("'", "''") + "', '" + descricao_prestacao_servico.Replace("'", "''") + "', 'T', " + Convert.ToDecimal(valor_total_servico_job.Replace(".", ",")).ToString().Replace(",", ".") + ", '" + observacoes.Replace("'", "''") + "', ";

        // --- VALORES NOVOS (Note o Replace de vírgula por ponto para o SQL não dar erro) ---
        sql += valor_ibs.ToString().Replace(",", ".") + ", ";
        sql += aliquota_ibs.ToString().Replace(",", ".") + ", ";
        sql += valor_cbs.ToString().Replace(",", ".") + ", ";
        sql += aliquota_cbs.ToString().Replace(",", ".") + "); ";

        sql += "SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void Nova_NF_Faturamento_Retencao(int cod_faturamento_nf, int cod_retencao, string nome_retencao, string aliquota_retencao, string apresentacao_retencao, decimal valor_retencao)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_RETENCOES (COD_FATURAMENTO_NF, COD_RETENCAO, NOME, ALIQUOTA, APRESENTACAO, VALOR) ";
        sql += "VALUES (" + cod_faturamento_nf + ", " + cod_retencao + ", '" + nome_retencao.Replace("'", "''") + "', ";
        sql += Convert.ToDecimal(aliquota_retencao.Replace(".", ",")).ToString().Replace(",", ".") + ", '" + apresentacao_retencao.Replace("'", "''") + "', " + valor_retencao.ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    public void Nova_NF_Faturamento_Servico_Job(int cod_faturamento_nf, int cod_servico, string nome_servico, string cod_servico_prefeitura,
        int cod_job, int cod_cliente, int cod_linha_negocio, int cod_divisao, string valor_servico_job, decimal impostos_servico, decimal carga_impostos_servico)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_SERVICOS_JOBS (COD_FATURAMENTO_NF, COD_SERVICO, NOME_SERVICO, COD_SERVICO_PREFEITURA, ";
        sql += "COD_JOB, COD_CLIENTE, COD_LINHA_NEGOCIO, COD_DIVISAO, VALOR, IMPOSTOS, CARGA_IMPOSTOS) ";
        sql += "VALUES (" + cod_faturamento_nf + ", " + cod_servico + ", '" + nome_servico.Replace("'", "''") + "', '" + cod_servico_prefeitura.Replace("'", "''") + "', ";
        sql += cod_job + ", " + cod_cliente + ", " + cod_linha_negocio + ", " + cod_divisao + ", " + Convert.ToDecimal(valor_servico_job.Replace(".", ",")).ToString().Replace(",", ".") + ", ";
        sql += impostos_servico.ToString().Replace(",", ".") + ", " + carga_impostos_servico.ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    public void Nova_NF_Faturamento_Vencimento(int cod_faturamento_nf, DateTime data_vencimento, string valor_vencimento)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_DATA_VENCIMENTO (COD_FATURAMENTO_NF, DATA_VENCIMENTO, VALOR) ";
        sql += "VALUES (" + cod_faturamento_nf + ", '" + data_vencimento.ToString("yyyyMMdd") + "', " + Convert.ToDecimal(valor_vencimento.Replace(".", ",")).ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    public void Nova_NF_Faturamento_Narrativa(int cod_faturamento_nf, int cod_narrativa, string nome_narrativa, string descricao_narrativa)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_NARRATIVAS (COD_FATURAMENTO_NF, COD_NARRATIVA, NOME, DESCRICAO) ";
        sql += "VALUES (" + cod_faturamento_nf + ", " + cod_narrativa + ", '" + nome_narrativa.Replace("'", "''") + "', '" + descricao_narrativa.Replace("'", "''") + "')";

        _conn.execute(sql);
    }

    public void Nova_NF_Faturamento_Tributo(int cod_faturamento_nf, int cod_tributo, string nome_tributo, string aliquota_tributo, decimal valor_tributo, decimal valor_base)
    {
        string sql = "INSERT INTO FATURAMENTO_NF_TRIBUTOS (COD_FATURAMENTO_NF, COD_TRIBUTO, NOME, ALIQUOTA, VALOR, VALOR_BASE) ";
        sql += "VALUES (" + cod_faturamento_nf + ", " + cod_tributo + ", '" + nome_tributo.Replace("'", "''") + "', ";
        sql += Convert.ToDecimal(aliquota_tributo.Replace(".", ",")).ToString().Replace(",", ".") + ", " + valor_tributo.ToString().Replace(",", ".") + ", " + valor_base.ToString().Replace(",", ".") + ")";

        _conn.execute(sql);
    }

    //FormGridConsultaNF     || INÍCIO ||
    public void lista_Tomadores(ref DataTable tb)
    {
        string sql = "SELECT DISTINCT CE.COD_EMPRESA AS COD_TOMADOR, CE.NOME_RAZAO_SOCIAL AS NOME_RAZAO_SOCIAL_TOMADOR, ";
        sql += "STUFF((SELECT DISTINCT + '#' + RTRIM(CONVERT(char, COD_EMITENTE)) ";
        sql += "FROM FATURAMENTO_NF NF WHERE NF.COD_TOMADOR = CE.COD_EMPRESA ";
        sql += "FOR XML PATH('')), 1, 0, '') AS COD_EMITENTE ";
        sql += "FROM CAD_EMPRESAS CE WHERE CE.COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CE.COD_EMPRESA IN (SELECT DISTINCT NF.COD_TOMADOR FROM FATURAMENTO_NF NF) ";
        sql += "ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(int? emitente, int? tomador, int? numero_nf, int? numero_rps, DateTime? de, DateTime? ate, decimal? valor_total, string situacao_nf, string status)
    {
        string sql = "SELECT COUNT(COD_FATURAMENTO_NF) FROM FATURAMENTO_NF WHERE 1 = 1";

        if (emitente != null)
            sql += " AND COD_EMITENTE = " + emitente;

        if (tomador != null)
            sql += " AND COD_TOMADOR = " + tomador;

        if (numero_nf != null)
            sql += " AND NUMERO_NF LIKE '%" + numero_nf + "%'";

        if (numero_rps != null)
            sql += " AND NUMERO_RPS LIKE '%" + numero_rps + "%'";

        if (de != null)
            sql += " AND DATA_EMISSAO_RPS >= '" + de.Value.ToString("yyyyMMdd") + "'";

        if (ate != null)
            sql += " AND DATA_EMISSAO_RPS <= '" + ate.Value.ToString("yyyyMMdd") + "'";

        if (valor_total != null)
            sql += " AND VALOR_SERVICOS LIKE '%" + valor_total.ToString().Replace(",", ".") + "%'";

        if (situacao_nf != null)
            sql += " AND SITUACAO_NF = '" + situacao_nf + "'";

        if (status != null)
            sql += " AND STATUS = '" + status + "'";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, int? emitente, int? tomador, int? numero_nf, int? numero_rps,
        DateTime? de, DateTime? ate, decimal? valor_total, string situacao_nf, string status, int paginaAtual, string ordenacao)
    {
        if (string.IsNullOrEmpty(ordenacao))
            ordenacao = "COD_FATURAMENTO_NF DESC";

        string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + ordenacao + ") AS ROW, * ";
        sql += "FROM FATURAMENTO_NF WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        if (emitente != null)
            sql += " AND COD_EMITENTE = " + emitente;

        if (tomador != null)
            sql += " AND COD_TOMADOR = " + tomador;

        if (numero_nf != null)
            sql += " AND NUMERO_NF LIKE '%" + numero_nf + "%'";

        if (numero_rps != null)
            sql += " AND NUMERO_RPS LIKE '%" + numero_rps + "%'";

        if (de != null && ate != null)
            sql += " AND DATA_EMISSAO_RPS BETWEEN '" + de.Value.ToString("yyyyMMdd") + "' AND '" + ate.Value.ToString("yyyyMMdd") + "'";
        else if (de != null)
            sql += " AND DATA_EMISSAO_RPS >= '" + de.Value.ToString("yyyyMMdd") + "'";
        else if (ate != null)
            sql += " AND DATA_EMISSAO_RPS <= '" + ate.Value.ToString("yyyyMMdd") + "'";

        if (valor_total != null)
            sql += " AND VALOR_SERVICOS LIKE '%" + valor_total.ToString().Replace(",", ".") + "%'";

        if (situacao_nf != null)
            sql += " AND SITUACAO_NF = '" + situacao_nf + "'";

        if (status != null)
            sql += " AND STATUS = '" + status + "'";

        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }



    public int Atualiza_NF(int numero_nf, DateTime data_emissao_nf, int numero_rps, DateTime data_emissao_rps, string cpf_cnpj_emitente)
    {
        string sql = "UPDATE FATURAMENTO_NF SET NUMERO_NF = " + numero_nf + ", DATA_EMISSAO_NF = '" + data_emissao_nf.ToString("yyyyMMdd HH:mm:ss") + "' ";
        sql += " WHERE NUMERO_RPS = " + numero_rps + " AND DATA_EMISSAO_RPS = '" + data_emissao_rps.ToString("yyyyMMdd") + "' AND CPF_CNPJ_EMITENTE = '" + cpf_cnpj_emitente + "' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.executeReturnRows(sql));
    }

    public int Verifica_Baixa_Contabilizacao(int cod_faturamento_nf)
    {
        string sql = "SELECT COUNT(*) FROM LANCTOS_CONTAB ";
        sql += "WHERE LOTE_PAI IN (SELECT LOTE FROM FATURAMENTO_NF WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + ") ";
        sql += "AND PENDENTE = 'False' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void Cancelar(int cod_faturamento_nf)
    {
        string sql = "DELETE FROM LANCTOS_CONTAB WHERE LOTE_PAI IN (SELECT LOTE FROM FATURAMENTO_NF WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " AND LOTE <> 0) AND LOTE_PAI <> 0 AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);

        sql = "DELETE FROM LANCTOS_CONTAB WHERE LOTE IN (SELECT LOTE FROM FATURAMENTO_NF WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " AND LOTE <> 0) AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);

        sql = "UPDATE FATURAMENTO_NF SET SITUACAO_NF = 'C', LOTE = 0 WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }
    //FormGridConsultaNF     || FIM ||

    //Contabilizacao_Funcoes
    public void Adiciona_Lote_NF(int cod_faturamento_nf, double lote)
    {
        string sql = "UPDATE FATURAMENTO_NF SET LOTE = " + lote + " WHERE COD_FATURAMENTO_NF = " + cod_faturamento_nf + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public int Atualiza_Lote_NF(int numero_rps, DateTime data_emissao_rps, string cpf_cnpj_emitente, int numero_nf)
    {
        string sql = "DECLARE @LOTE AS TABLE (LOTE INT)\n";
        sql += "INSERT INTO @LOTE SELECT LOTE FROM FATURAMENTO_NF WHERE NUMERO_RPS = " + numero_rps + " AND DATA_EMISSAO_RPS = '" + data_emissao_rps.ToString("yyyyMMdd") + "' AND CPF_CNPJ_EMITENTE = '" + cpf_cnpj_emitente + "' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "\n";
        sql += "IF(SELECT COUNT(*) FROM @LOTE) = 0 BEGIN SELECT 0 END \n";
        sql += "ELSE IF EXISTS (SELECT 1 FROM LANCTOS_CONTAB WHERE LOTE IN (SELECT LOTE FROM @LOTE) AND NUMERO_DOCUMENTO NOT LIKE '%/NF-%') BEGIN UPDATE LANCTOS_CONTAB SET NUMERO_DOCUMENTO = NUMERO_DOCUMENTO + '/NF-" + numero_nf + "' OUTPUT INSERTED.LOTE WHERE LOTE IN (SELECT LOTE FROM @LOTE) END\n";
        sql += "ELSE BEGIN SELECT 1 END";

        //string sql = "UPDATE LANCTOS_CONTAB SET NUMERO_DOCUMENTO = NUMERO_DOCUMENTO + '/NF-" + numero_nf + "' OUTPUT INSERTED.LOTE";
        //sql += " WHERE LOTE IN (SELECT LOTE FROM FATURAMENTO_NF";
        //sql += " WHERE NUMERO_RPS = " + numero_rps + " AND DATA_EMISSAO_RPS = '" + data_emissao_rps.ToString("yyyyMMdd") + "' AND CPF_CNPJ_EMITENTE = '" + cpf_cnpj_emitente + "' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ") AND NUMERO_DOCUMENTO NOT LIKE '%/NF-%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}