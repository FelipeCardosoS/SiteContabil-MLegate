using System.Collections.Generic;

public class Contabilizacao
{
    public int cod_emitente;
    public string cod_conta_diferenca { get; set; }
    public string debito_credito { get; set; }
    public string historico { get; set; }
    public bool gera_titulo { get; set; }
    public bool insert { get; set; }
    public bool update { get; set; }
    public List<Contabilizacao_Servico> List_Servico { get; set; }
    public List<Contabilizacao_Retencao> List_Retencao { get; set; }
    public List<Contabilizacao_Tributo> List_Tributo { get; set; }
}