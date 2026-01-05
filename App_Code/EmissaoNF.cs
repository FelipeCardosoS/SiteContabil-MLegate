using System;
using System.Collections.Generic;

public class EmissaoNF
{
    // --- CAMPOS NOVOS (REFORMA TRIBUTARIA 2026/27) --- // 
    public decimal valor_ibs { get; set; }
    public decimal aliquota_ibs { get; set; }
    public decimal valor_cbs { get; set; }
    public decimal aliquota_cbs { get; set; }
    // --- FIM DOS NOVOS CAMPOS --- //
    public int cod_emitente;
    public int cod_tomador;
    public int numero_nf;
    public int numero_rps;
    public DateTime data_emissao_nf;
    public DateTime data_emissao_rps;
    public DateTime data_rps;
    public DateTime data_competencia;
    public string cpf_cnpj_emitente;
    public int cod_natureza_operacao;
    public string nome_natureza_operacao;
    public string descricao_natureza_operacao;
    public string natureza_operacao;
    public int cod_prestacao_servico;
    public string nome_prestacao_servico;
    public string descricao_prestacao_servico;
    public string valor_total_servico_job;
    public string valor_total_liquido;
    public string valor_total_vencimento;
    public string observacoes;
    public List<EmissaoNF_Retencao> List_Retencao { get; set; }
    public List<EmissaoNF_Servico_Job> List_Servico_Job { get; set; }
    public List<EmissaoNF_Vencimento> List_Vencimento { get; set; }
    public List<EmissaoNF_Narrativa> List_Narrativa { get; set; }
}