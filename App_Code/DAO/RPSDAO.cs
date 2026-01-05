using System.Collections.Generic;
using System.Data;

public class RPSDAO
{
    private Conexao _conn;

	public RPSDAO(Conexao c)
	{
        _conn = c;
	}

    public string Load_IM_Emissor(int cod_faturamento_nf)
    {
        string sql = "SELECT CE.IM FROM CAD_EMPRESAS CE WHERE CE.COD_EMPRESA = (SELECT NF.COD_EMITENTE FROM FATURAMENTO_NF NF WHERE NF.COD_FATURAMENTO_NF = " + cod_faturamento_nf + ")";
        return _conn.scalar(sql).ToString();
    }

    public DataTable Load_RPS(List<int> cod_faturamento_nf)
    {
        string sql = @"SELECT DISTINCT NF.SERIE_RPS, NF.NUMERO_RPS,
                       YEAR(NF.DATA_EMISSAO_RPS) * 10000 + MONTH(NF.DATA_EMISSAO_RPS) * 100 + DAY(NF.DATA_EMISSAO_RPS) AS DATA_EMISSAO_RPS,
                       SUM(NF_SJ.VALOR) AS VALOR_SERVICOS, 0 AS VALOR_DEDUCOES, NF_SJ.COD_SERVICO_PREFEITURA,
                       
                       ISNULL((SELECT CT_1.Destacado FROM CAD_TRIBUTOS CT_1 WHERE CT_1.Cod_Tributos_Sys = 1
                       AND CT_1.COD_TRIBUTO IN (SELECT NF_TR_1.COD_TRIBUTO FROM FATURAMENTO_NF_TRIBUTOS NF_TR_1
                       WHERE NF_TR_1.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF)), 0) AS DESTACADO,
                       
                       ISNULL((SELECT NF_TR_2.ALIQUOTA FROM FATURAMENTO_NF_TRIBUTOS NF_TR_2
                       WHERE NF_TR_2.COD_TRIBUTO IN (SELECT CT_2.COD_TRIBUTO FROM CAD_TRIBUTOS CT_2 WHERE CT_2.Cod_Tributos_Sys = 1)
                       AND NF_TR_2.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS ALIQUOTA_ISS,
                       
                       NF.CPF_CNPJ_TOMADOR, NF.IM_TOMADOR, NF.IE_TOMADOR, NF.NOME_RAZAO_SOCIAL_TOMADOR,
                       ISNULL(NF.LOGRADOURO_TOMADOR, '') AS LOGRADOURO_TOMADOR, NF.ENDERECO_TOMADOR, NF.NUM_ENDERECO_TOMADOR,
                       NF.COMPL_ENDERECO_TOMADOR, NF.BAIRRO_TOMADOR, NF.MUNICIPIO_TOMADOR, NF.UF_TOMADOR, NF.CEP_TOMADOR, '' AS EMAIL_TOMADOR,
                       
                       ISNULL((SELECT NF_RET_1.VALOR FROM FATURAMENTO_NF_RETENCOES NF_RET_1
                       WHERE NF_RET_1.COD_RETENCAO IN (SELECT CR_1.COD_RETENCAO FROM CAD_RETENCOES CR_1 WHERE CR_1.cod_retencoes_sys = 3)
                       AND NF_RET_1.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS PIS_RETIDO,
                       
                       ISNULL((SELECT NF_RET_2.VALOR FROM FATURAMENTO_NF_RETENCOES NF_RET_2
                       WHERE NF_RET_2.COD_RETENCAO IN (SELECT CR_2.COD_RETENCAO FROM CAD_RETENCOES CR_2 WHERE CR_2.cod_retencoes_sys = 4)
                       AND NF_RET_2.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS COFINS_RETIDO,
                       
                       ISNULL((SELECT NF_RET_3.VALOR FROM FATURAMENTO_NF_RETENCOES NF_RET_3
                       WHERE NF_RET_3.COD_RETENCAO IN (SELECT CR_3.COD_RETENCAO FROM CAD_RETENCOES CR_3 WHERE CR_3.cod_retencoes_sys = 6)
                       AND NF_RET_3.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS INSS_RETIDO,
                       
                       ISNULL((SELECT NF_RET_4.VALOR FROM FATURAMENTO_NF_RETENCOES NF_RET_4
                       WHERE NF_RET_4.COD_RETENCAO IN (SELECT CR_4.COD_RETENCAO FROM CAD_RETENCOES CR_4 WHERE CR_4.cod_retencoes_sys = 2)
                       AND NF_RET_4.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS IR_RETIDO,
                       
                       ISNULL((SELECT NF_RET_5.VALOR FROM FATURAMENTO_NF_RETENCOES NF_RET_5
                       WHERE NF_RET_5.COD_RETENCAO IN (SELECT CR_5.COD_RETENCAO FROM CAD_RETENCOES CR_5 WHERE CR_5.cod_retencoes_sys = 5)
                       AND NF_RET_5.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF), 0) AS CSLL_RETIDO,
                       
                       SUM(NF_SJ.CARGA_IMPOSTOS) AS CARGA_IMPOSTOS, NF_SJ.IMPOSTOS, CS.NOME AS NOME_SERVICO,
                       (SELECT NF_N.DESCRICAO FROM FATURAMENTO_NF_NARRATIVAS NF_N WHERE NF_N.COD_FATURAMENTO_NF IN (NF.COD_FATURAMENTO_NF)) AS DESCRICAO_NARRATIVA,
                       NF.DISCRIMINACAO_OBSERVACAO,
                       
                       (SELECT STUFF((SELECT ', ' +
                       RIGHT('00' + CONVERT(NVARCHAR, DAY(NF_DV.DATA_VENCIMENTO)), 2) + '/' +
                       RIGHT('00' + CONVERT(NVARCHAR, MONTH(NF_DV.DATA_VENCIMENTO)), 2) + '/' +
                       CONVERT(NVARCHAR, YEAR(NF_DV.DATA_VENCIMENTO))
                       FROM FATURAMENTO_NF_DATA_VENCIMENTO NF_DV
                       WHERE NF_DV.COD_FATURAMENTO_NF = NF.COD_FATURAMENTO_NF
                       ORDER BY NF_DV.DATA_VENCIMENTO
                       FOR XML PATH('')), 1, 2, '')) AS DATA_VENCIMENTO
                       
                       FROM
                       FATURAMENTO_NF AS NF,
                       FATURAMENTO_NF_SERVICOS_JOBS AS NF_SJ,
                       CAD_SERVICOS AS CS,
                       CAD_EMPRESAS AS CE
                       
                       WHERE NF.COD_FATURAMENTO_NF = NF_SJ.COD_FATURAMENTO_NF
                       AND NF.COD_EMPRESA = CS.COD_EMPRESA
                       AND NF.COD_EMITENTE = CE.COD_EMPRESA
                       AND NF.COD_EMPRESA = CE.COD_EMPRESA_PAI
                       AND NF_SJ.COD_SERVICO = CS.COD_SERVICO
                       AND NF.COD_FATURAMENTO_NF IN (" + string.Join(", ", cod_faturamento_nf) + @")
                       
                       GROUP BY NF.COD_FATURAMENTO_NF, NF.SERIE_RPS, NF.NUMERO_RPS, NF.DATA_EMISSAO_RPS, NF_SJ.COD_SERVICO_PREFEITURA,
                       NF.CPF_CNPJ_TOMADOR, NF.IM_TOMADOR, NF.IE_TOMADOR, NF.NOME_RAZAO_SOCIAL_TOMADOR,
                       NF.LOGRADOURO_TOMADOR, NF.ENDERECO_TOMADOR, NF.NUM_ENDERECO_TOMADOR, NF.COMPL_ENDERECO_TOMADOR,
                       NF.BAIRRO_TOMADOR, NF.MUNICIPIO_TOMADOR, NF.UF_TOMADOR, NF.CEP_TOMADOR,
                       NF_SJ.IMPOSTOS, CS.NOME, NF.DISCRIMINACAO_OBSERVACAO";
        
        return _conn.dataTable(sql, "RPS");
    }

    public void Atualiza_Status(List<int> cod_faturamento_nf)
    {
        string sql = "UPDATE FATURAMENTO_NF SET STATUS = 'G' WHERE COD_FATURAMENTO_NF IN (" + string.Join(", ", cod_faturamento_nf) + ")";
        _conn.execute(sql);
    }
}