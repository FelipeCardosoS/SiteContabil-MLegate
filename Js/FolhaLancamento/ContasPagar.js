function ContasPagar(modulo)
{
    this.prefixo = "#ctl00_areaConteudo_";
    this.prefixo_s = "ctl00_areaConteudo_";
    this.modulo = modulo;
    this.iniciaTela = iniciaTela;
    this.salvaFolha = salvaFolha;
    this.atualizaJobs = atualizaJobs;
    this.verificaAlteracaoParcelas = verificaAlteracaoParcelas;
    this.carregaModelo = carregaModelo;
    this.alteraModelo = alteraModelo;
    this.salvaLancto = salvaLancto;
    this.alteraLancto = alteraLancto;
    this.deletaLancto = deletaLancto;
    this.pegaValorOldLancto1 = pegaValorOldLancto1;
    this.alteraValorLancto1 = alteraValorLancto1;
    this.carregaJob = carregaJob;
    this.montaGridLanctos = montaGridLanctos;
    this.loadVencimentos = loadVencimentos;
    this.salvaVencimentos = salvaVencimentos;
    this.remanejaSequencia = remanejaSequencia;
    this.cancela = cancela;
    this.calculaTotais = calculaTotais;
    this.limpaCampos = limpaCampos;
    this.ativaFormLancamento = ativaFormLancamento;
    this.desativaFormLancamento = desativaFormLancamento;
    this.ativaFormTitulo = ativaFormTitulo;
    this.desativaFormTitulo = desativaFormTitulo;
}
ContasPagar.prototype = new Form;