public class Contabilizacao_Tributo
{
    public bool insert { get; set; }
    public bool update { get; set; }
    public int cod_tributo { get; set; }
    public string nome_tributo { get; set; }

    public string cod_conta_debito { get; set; }
    public bool gera_titulo_debito { get; set; }
    public int cod_terceiro_debito { get; set; }
    public string historico_debito { get; set; }

    public string cod_conta_credito { get; set; }
    public bool gera_titulo_credito { get; set; }
    public int cod_terceiro_credito { get; set; }
    public string historico_credito { get; set; }
}