public class Contabilizacao_Servico
{
    public bool insert { get; set; }
    public bool update { get; set; }
    public int cod_servico { get; set; }
    public string nome_servico { get; set; }

    public string cod_conta_debito { get; set; }
    public string bruto_liquido_debito { get; set; }
    public bool gera_titulo_debito { get; set; }
    public string historico_debito { get; set; }

    public string cod_conta_credito { get; set; }
    public string bruto_liquido_credito { get; set; }
    public bool gera_titulo_credito { get; set; }
    public string historico_credito { get; set; }
}