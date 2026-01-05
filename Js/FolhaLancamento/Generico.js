function Form() {
    this.exigeModelo = exigeModelo();
    this.statusTravaJob = travaJob();
    this.etapaAtual = 1;
    this.loteAtual = 0;
    this.prefixo = "#ctl00_areaConteudo_";
    this.prefixo_s = "ctl00_areaConteudo_";
    this.checkGeraCredito = this.prefixo + "checkGeraCredito";
    this.comboFornecedor = this.prefixo + "comboFornecedor";
    this.comboEtapaMoeda = this.prefixo + "comboEtapaMoeda";
    this.textValorTotal = this.prefixo + "textValorTotal";
    this.textValorUnit = this.prefixo + "textValorUnit";
    this.textValorBruto = this.prefixo + "textValorBruto";
    this.textGrupo = this.prefixo + "textGrupo";
    this.textQtd = this.prefixo + "textQtd";
    this.textParcelas = this.prefixo + "textParcelas";
    this.btVencimento = this.prefixo + "btVencimento";
    this.comboModelo = this.prefixo + "comboModelo";
    this.hdSeqLote_lancto = this.prefixo + "H_SEQLOTE_LANCTO";
    this.hdSeqLoteMin_lancto = this.prefixo + "H_SEQLOTE_LANCTO_MIN";
    this.hdSeqLoteMax_lancto = this.prefixo + "H_SEQLOTE_LANCTO_MAX";
    this.textData = this.prefixo + "textData";
    this.textNumeroDocumento = this.prefixo + "textNumeroDocumento";
    this.textNumeroDocumento_lancto = this.prefixo + "textNumeroDocumento_lancto";
    this.comboDebCred_lancto = this.prefixo + "comboDebCred_lancto";
    this.comboConta_lancto = this.prefixo + "comboConta_lancto";
    this.comboJob_lancto = this.prefixo + "comboJob_lancto";
    this.textQtd_lancto = this.prefixo + "textQtd_lancto";
    this.textValor_lancto = this.prefixo + "textValor_lancto";
    this.textValorUnit_lancto = this.prefixo + "textValorUnit_lancto";
    this.comboLinhaNegocio_lancto = this.prefixo + "comboLinhaNegocio_lancto";
    this.comboDivisao_lancto = this.prefixo + "comboDivisao_lancto";
    this.comboCliente_lancto = this.prefixo + "comboCliente_lancto";
    this.comboGeraTitulo_lancto = this.prefixo + "comboGeraTitulo_lancto";
    this.textParcelas_lancto = this.prefixo + "textParcelas_lancto";
    this.comboTerceiro_lancto = this.prefixo + "comboTerceiro_lancto";
    this.textHistorico_lancto = this.prefixo + "textHistorico_lancto";
    this.botaoSalvar_lancto = this.prefixo + "botaoSalvar_lancto";
    this.botaoSalvar = this.prefixo + "botaoSalvar";
    this.botaoVoltar = this.prefixo + "botaoVoltar";
    this.botaoCancelar_lancto = this.prefixo + "botaoCancelar_lancto";
    this.alteraFolha = alteraFolha;
    this.selecionaJobDefault = selecionaJobDefault;
    this.carregaContas = carregaContas;
    this.totalDebito = "#totalDebito";
    this.totalCredito = "#totalCredito";
    this.totalDiferenca = "#totalDiferenca";
    this.pegaQtdOldLancto1 = pegaQtdOldLancto1;
    this.alteraQtdLancto1 = alteraQtdLancto1;
    this.pegaNumeroDocumentoOldLancto1 = pegaNumeroDocumentoOldLancto1;
    this.alteraNumeroDocumentoLancto1 = alteraNumeroDocumentoLancto1;
    this.desativaGridLanctos = desativaGridLanctos;
    this.verificaContaGeraTitulo = verificaContaGeraTitulo;
    this.abreBaixas = abreBaixas;
    this.alteraConsultorLancto1 = alteraConsultorLancto1;
    this.comboConsultor = this.prefixo + "comboConsultor";
    this.comboConsultor_lancto = this.prefixo + "comboConsultor_lancto";
    //this.buscaTerceirosDisponiveis = buscaTerceirosDisponiveis;
    this.exibeEtapa = exibeEtapa;
    this.voltarTela = voltarTela;
    this.executaEtapa = executaEtapa;
    this.comboTipoNotaFiscal = this.prefixo + "comboTipoNotaFiscal";
    this.montaFormNotaFiscal = montaFormNotaFiscal;
    //campos formulário notafiscal
    this.hdNumeroNotaOld = this.prefixo + "numeroNotaOldHidden";
    this.textNumeroNota = this.prefixo + "textNumeroNota";
    this.textNumeroEletronica = this.prefixo + "textNumeroEletronica";
    this.comboEntradaSaidaNota = this.prefixo + "comboEntradaSaidaNota";
    this.textValorNota = this.prefixo + "textValorNota";
    this.areaFornecedorClienteNota = "#areaFornecedorClienteNota";
    this.textIcmsNota = this.prefixo + "textIcmsNota";
    this.textDescontoNota = this.prefixo + "textDescontoNota";
    this.textFreteNota = this.prefixo + "textFreteNota";
    this.textIpiNota = this.prefixo + "textIpiNota";
    this.textBaseImpNota = this.prefixo + "textBaseImpNota";
    this.labelIpiNota = "#labelIpiNota";
    this.labelFreteNota = "#labelFreteNota";
    this.camposServico = "#camposServico";
    this.textCsllNota = this.prefixo + "textCsllNota";
    this.textIrNota = this.prefixo + "textIrNota";
    this.textIssNota = this.prefixo + "textIssNota";
    this.textPisRetidoNota = this.prefixo + "textPisRetidoNota";
    this.textCofinsRetidoNota = this.prefixo + "textCofinsRetidoNota";
    this.textCsllRetidoNota = this.prefixo + "textCsllRetidoNota";
    this.textIrRetidoNota = this.prefixo + "textIrRetidoNota";
    this.textIssRetidoNota = this.prefixo + "textIssRetidoNota";
    this.comboProdutoItem = this.prefixo + "comboProdutoItem";
    this.textCfopItem = this.prefixo + "textCfopItem";
    this.textQtdeItem = this.prefixo + "textQtdeItem";
    this.textValorTotalItem = this.prefixo + "textValorTotalItem";
    this.textBaseImpItem = this.prefixo + "textBaseImpItem";
    this.textIcmsItem = this.prefixo + "textIcmsItem";
    this.textIpiItem = this.prefixo + "textIpiItem";
    this.textPisItem = this.prefixo + "textPisItem";
    this.textCofinsItem = this.prefixo + "textCofinsItem";
    this.labelCfopItem = "#labelCfopItem";
    this.labelIpiItem = "#labelIpiItem";
    this.labelIrItem = "#labelIrItem";
    this.labelCsllItem = "#labelCsllItem";
    this.hdProdutoServico = this.prefixo + "H_PRODUTO_SERVICO";
    this.hdOrdem = this.prefixo + "H_ORDEM";
    this.hdOrdemMin = this.prefixo + "H_ORDEM_MIN";
    this.hdOrdemMax = this.prefixo + "H_ORDEM_MAX";
    this.botaoSalvar_item = this.prefixo + "botaoSalvar_item";
    this.botaoCotacao = this.prefixo + "botaoCotacaoAbre";
    this.dataHidden = this.prefixo + "dataHidden";
    this.montaGridItens = montaGridItens;
    this.salvaNota = salvaNota;
    this.limpaCamposNota = limpaCamposNota;
    this.limpaCamposItemNota = limpaCamposItemNota;
    this.remanejaOrdem = remanejaOrdem;
    this.salvaItem = salvaItem;
    this.deletaItem = deletaItem;
    this.desativaGridItens = desativaGridItens;
    this.alteraItem = alteraItem;
    this.cancelaItem = cancelaItem;
    this.salvaTela = salvaTela;
    this.verificaGeraCredito = verificaGeraCredito;
	this.aliquotaImposto = aliquotaImposto;
    this.buscaLoteAnterior = false;
    this.coresInvertidas = false;
}

function inverterCoresTabela() {
    let corFundo = tela.coresInvertidas ? "#FFF" : "#000";
    let corLetra = tela.coresInvertidas ? "#000" : "#FFF";
    let texto = "Deixar a Tabela " + (tela.coresInvertidas ? "Escura" : "Clara");

    [...document.styleSheets[0].cssRules].find(x => x.selectorText.includes("table.gridLancamentos tr.linha td")).style["background-color"] = corFundo;
    [...document.styleSheets[0].cssRules].find(x => x.selectorText.includes("table.gridLancamentos tr.linha td")).style["color"] = corLetra;

    document.getElementsByClassName("botaoInverter")[0].textContent = texto;

    tela.coresInvertidas = !tela.coresInvertidas;
}

function exibeEtapa() {

    if (tela.modulo == "MODELO_LANCAMENTO" || tela.modulo == "C_INCLUSAO_LANCTO") {
        $("#box_etapas").hide();
        $(tela.botaoVoltar).hide();
    }

    $("#etapa1").attr("class", "etapa");
    $("#etapa2").attr("class", "etapa");
    $("#etapa3").attr("class", "etapa");

    $("#panelEtapa1").hide();
    $("#panelEtapa2").hide();
    $("#panelEtapa3").hide();

    if (tela.modulo == "MODELO_LANCAMENTO" || tela.modulo == "C_INCLUSAO_LANCTO") {
        $(tela.botaoSalvar).val("Concluído");
    }

    $(tela.botaoVoltar).removeAttr("disabled");

    if (tela.etapaAtual == 1) {
        $("#etapa1").attr("class", "etapa ativo");
        $("#panelEtapa1").show();
        $(tela.botaoVoltar).attr("disabled", "disabled");
    } else if (tela.etapaAtual == 2) {
        $("#etapa2").attr("class", "etapa ativo");
        $("#panelEtapa2").show();

    } else {
        $("#etapa3").attr("class", "etapa ativo");
        $("#panelEtapa3").show();
        $(tela.botaoSalvar).val("Concluir");
    }
}

function iniciaTela() {
    tela.exibeEtapa();
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "Default.aspx/getSessionLanctos",
            data: "{modulo:'" + tela.modulo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Montando tela..."),
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null || result != undefined) {
                    if (result.length > 0) {
                        tela.montaGridLanctos(result);
                        tela.calculaTotais(result);
                        tela.ativaFormTitulo();
                        tela.ativaFormLancamento();

                        $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                        $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                        $(tela.textQtd).val(number_format(result[0].qtd, 6, ',', '.'));
                        $(tela.textValorUnit).val(number_format(result[0].valorUnit, 6, ',', '.'));
                        $(tela.textValorBruto).val(number_format((result[0].valorBruto == null ? 0 : result[0].valorBruto), 2, ',', '.'));
                        $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                        $(tela.comboFornecedor).val(result[0].terceiro);
                        $(tela.textData).val(result[0].dataLancamento_formatada);
                        $(tela.textParcelas).val(result[0].vencimentos.length);
                        tela.carregaModelo(result[0].terceiro, result[0].modelo);
                        $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                        $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);
                        $(tela.comboConsultor).val(result[0].codConsultor);

                        var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                        newSeqLote++;
                        $(tela.hdSeqLote_lancto).val(newSeqLote);

                        $(tela.textQtd_lancto).val("1");
                        $(tela.textValorUnit_lancto).val("0");
                        $(tela.textValor_lancto).val("0");
                        $(tela.textParcelas_lancto).val("1");
                    }
                    else {
                        //$(tela.textNumeroDocumento).val("");
                        $(tela.textParcelas).val("1");
                        $(tela.textQtd).val("1");
                        $(tela.textValorTotal).val("0");
                        $(tela.textValorUnit).val("0");
                        $(tela.textValorBruto).val("0");
                        $(tela.hdSeqLote_lancto).val("2");
                        $(tela.hdSeqLoteMin_lancto).val("1");
                        $(tela.hdSeqLoteMax_lancto).val("1");
                        $(tela.textQtd_lancto).val("1");
                        $(tela.textValor_lancto).val("0");
                        $(tela.textValorUnit_lancto).val("0");
                        $(tela.textParcelas_lancto).val("1");
                        tela.desativaFormTitulo();
                        tela.desativaFormLancamento();
                    }
                } else {
                    //$(tela.textNumeroDocumento).val("");
                    $(tela.textParcelas).val("1");
                    $(tela.textQtd).val("1");
                    $(tela.textValorTotal).val("0");
                    $(tela.textValorUnit).val("0");
                    $(tela.textValorBruto).val("0");
                    $(tela.hdSeqLote_lancto).val("2");
                    $(tela.hdSeqLoteMin_lancto).val("1");
                    $(tela.hdSeqLoteMax_lancto).val("1");
                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                    tela.desativaFormTitulo();
                    tela.desativaFormLancamento();
                }
            },
            complete: function () {
                tela.carregaContas($(tela.hdSeqLote_lancto).val());

            },
            error: OnFailed
        });
    });
}

function calculaValorTotal() {
    if ($(tela.textQtd).val() == "")
        $(tela.textQtd).val("0");
    if ($(tela.textValorUnit).val() == "")
        $(tela.textValorUnit).val("0");

    var qtd = Math.round(limpaNumero($(tela.textQtd).val()) * 1000000) / 1000000;;
    var vlrUnit = Math.round(limpaNumero($(tela.textValorUnit).val()) * 1000000) / 1000000;;
    var vlrTotal = vlrUnit * qtd;

    $(tela.textValorTotal).val(number_format(vlrTotal, 2, ',', '.'));
}

function calculaValorTotal_lancto() {
    if ($(tela.textQtd_lancto).val() == "")
        $(tela.textQtd_lancto).val("0");
    if ($(tela.textValorUnit_lancto).val() == "")
        $(tela.textValorUnit_lancto).val("0");

    var qtd = Math.round(limpaNumero($(tela.textQtd_lancto).val()) * 1000000) / 1000000;
    var vlrUnit = Math.round(limpaNumero($(tela.textValorUnit_lancto).val()) * 1000000) / 1000000;
    var vlrTotal = vlrUnit * qtd;

    $(tela.textValor_lancto).val(number_format(vlrTotal, 2, ',', '.'));
}

function voltarTela() {
    if (tela.etapaAtual > 1) {
        if (tela.etapaAtual == 3) {
            tela.salvaNota();
        }
        tela.etapaAtual--;
        tela.exibeEtapa();
        return false;
    }
}

function limpaCamposNota() {
    $(tela.hdProdutoServico).val("");
    $(tela.textNumeroNota).val("");
    $(tela.textNumeroEletronica).val("");
    $(tela.comboEntradaSaidaNota).val("0");
    $(tela.textValorNota).val(0.00);
    $(tela.textIcmsNota).val(0.00);
    $(tela.textDescontoNota).val(0.00);
    $(tela.textBaseImpNota).val(0.00);
    $(tela.textFreteNota).val(0.00);
    $(tela.textIpiNota).val(0.00);
    $(tela.textCsllNota).val(0.00);
    $(tela.textIrNota).val(0.00);
    $(tela.textIssNota).val(0.00);
    $(tela.textPisRetidoNota).val(0.00);
    $(tela.textCofinsRetidoNota).val(0.00);
    $(tela.textCsllRetidoNota).val(0.00);
    $(tela.textIrRetidoNota).val(0.00);
    $(tela.textIssRetidoNota).val(0.00);
}

function limpaCamposItemNota() {
    $(tela.textCfopItem).val("");
    $(tela.comboProdutoItem).val("0");
    $(tela.textQtdeItem).val("1");
    $(tela.textValorTotalItem).val("0.00");
    $(tela.textBaseImpItem).val("0.00");
    $(tela.textIcmsItem).val("0.00");
    $(tela.textIpiItem).val("0.00");
    $(tela.textPisItem).val(number_format(tela.aliquotaImposto("PIS"), 2, ',', '.'));
    $(tela.textCofinsItem).val(number_format(tela.aliquotaImposto("COFINS"), 2, ',', '.'));
    $(tela.botaoSalvar_item).val("Inserir");
}

function salvaItem() {
    var ordem = $(tela.hdOrdem).val();
    var cfop = "";
    var produtoServico = $(tela.hdProdutoServico).val();
    var produto = $(tela.comboProdutoItem).val();
    var descProduto = $(tela.comboProdutoItem + " option:selected").text();
    var qtde = limpaNumero($(tela.textQtdeItem).val());
    var valorTotal = limpaNumero($(tela.textValorTotalItem).val());
    var baseImp = limpaNumero($(tela.textBaseImpItem).val());
    var icms = limpaNumero($(tela.textIcmsItem).val());
    var ipi = 0;
    var pis = limpaNumero($(tela.textPisItem).val());
    var cofins = limpaNumero($(tela.textCofinsItem).val());
    if (produtoServico == "P") {
        ipi = limpaNumero($(tela.textIpiItem).val());
        cfop = limpaNumero($(tela.textCfopItem).val());
    }

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/salvaItem",
        async: false,
        data: "{ordem:" + ordem + ",cfop:'" + cfop + "',produto:" + produto + ",descProduto:'" + descProduto + "',qtde:" + qtde + ",valorTotal:" + valorTotal + ",baseImp:" + baseImp + ",icms:" + icms + ",ipi:" + ipi + ",pis:" + pis + ",cofins:" + cofins + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#boxLoadingItem").show();
            $("#boxLoadingItem").text("Salvando item...");
            $(tela.botaoSalvar).attr("disabled", "disabled");
            $(tela.botaoVoltar).attr("disabled", "disabled");
        },
        success: function (result) {
            $(tela.botaoSalvar).removeAttr("disabled");
            $(tela.botaoVoltar).removeAttr("disabled");
            $("#boxLoadingItem").hide();
            tela.remanejaOrdem(ordem);
            tela.limpaCamposItemNota();
        },
        error: OnFailed,
        complete: function () {

            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/getSessionNotaFiscal",
                async: false,
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#boxLoadingItem").show();
                    $("#boxLoadingItem").text("Carregando informações...");
                },
                success: function (result) {
                    $("#boxLoadingItem").hide();
                    if (result != null) {
                        var notaFiscal = result.d;
                        if (notaFiscal != null) {
                            tela.montaGridItens(notaFiscal.itens);
                        }
                    }
                },
                error: OnFailed
            });
        }
    });
}

function deletaItem(ordem) {
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/deletaItem",
        async: false,
        data: "{ordem:" + ordem + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#boxLoadingItem").show();
            $("#boxLoadingItem").text("Deletando item...");
        },
        success: function (result) {
            $("#boxLoadingItem").hide();
        },
        error: OnFailed,
        complete: function () {

            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/getSessionNotaFiscal",
                async: false,
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#boxLoadingItem").show();
                    $("#boxLoadingItem").text("Carregando informações...");
                },
                success: function (result) {
                    $("#boxLoadingItem").hide();
                    if (result != null) {
                        var notaFiscal = result.d;
                        if (notaFiscal != null) {

                            $(tela.hdOrdemMin).val(notaFiscal.itens[0].ordem);
                            $(tela.hdOrdemMax).val(notaFiscal.itens[notaFiscal.itens.length - 1].ordem);
                            var newOrdem = eval(notaFiscal.itens[eval(notaFiscal.itens.length - 1)].ordem);
                            newOrdem++;
                            $(tela.hdOrdem).val(newOrdem);

                            tela.montaGridItens(notaFiscal.itens);
                        }
                    }
                },
                error: OnFailed
            });
        }
    });
}

function alteraItem(ordem, linha) {
    var produtoServico = $(tela.hdProdutoServico).val();
    var metodo = "";
    if (produtoServico == "P") {
        metodo = "getItemProduto";
    } else {
        metodo = "getItemServico";
    }

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/" + metodo,
        async: false,
        data: "{ordem:" + ordem + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {

            $("#boxLoadingItem").show();
            $("#boxLoadingItem").text("Carregando item...");
            $(tela.botaoSalvar).attr("disabled", "disabled");
            $(tela.botaoVoltar).attr("disabled", "disabled");
        },
        success: function (result) {
            $("#boxLoadingItem").hide();
            if (result != null) {
                var item = result.d;
                if (item != null) {
                    tela.desativaGridItens();
                    $("#gridItensNotaFiscal tr[class='linhaSelecionada']").attr("class", "linha");
                    $(linha).attr("class", "linhaSelecionada");

                    $(tela.hdOrdem).val(item.ordem);
                    $(tela.comboProdutoItem).val(item.codigoProduto);
                    $(tela.textQtdeItem).val(number_format(item.qtde, 2, ',', '.'));
                    $(tela.textValorTotalItem).val(number_format(item.valorTotal, 2, ',', '.'));
                    $(tela.textBaseImpItem).val(number_format(item.valorBaseImp, 2, ',', '.'));
                    $(tela.textIcmsItem).val(number_format(item.aliquotaIcms, 2, ',', '.'));
                    $(tela.textIpiItem).val("0.00");
                    $(tela.textCfopItem).val("");
                    if (produtoServico == "P") {
                        $(tela.textCfopItem).val(item.cfop);
                        $(tela.textIpiItem).val(number_format(item.aliquotaIpi, 2, ',', '.'));
                    }
                    $(tela.textPisItem).val(number_format(item.aliquotaPis, 2, ',', '.'));
                    $(tela.textCofinsItem).val(number_format(item.aliquotaCofins, 2, ',', '.'));

                    $(tela.botaoSalvar_item).val("Alterar");
                }
            }
        },
        error: OnFailed
    });
}

function cancelaItem() {

    tela.limpaCamposItemNota();
    $("#gridItensNotaFiscal tr[class='linhaSelecionada']").attr("class", "linha");
    $(tela.botaoSalvar_lancto).val("Incluir");
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/getSessionNotaFiscal",
        async: false,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#boxLoadingItem").show();
            $("#boxLoadingItem").text("Carregando informações...");
        },
        success: function (result) {
            $("#boxLoadingItem").hide();
            $(tela.botaoSalvar).removeAttr("disabled");
            $(tela.botaoVoltar).removeAttr("disabled");
            if (result != null) {
                var notaFiscal = result.d;
                if (notaFiscal != null) {

                    $(tela.hdOrdemMin).val(notaFiscal.itens[0].ordem);
                    $(tela.hdOrdemMax).val(notaFiscal.itens[notaFiscal.itens.length - 1].ordem);
                    var newOrdem = eval(notaFiscal.itens[eval(notaFiscal.itens.length - 1)].ordem);
                    newOrdem++;
                    $(tela.hdOrdem).val(newOrdem);

                    tela.montaGridItens(notaFiscal.itens);
                }
            }
        },
        error: OnFailed
    });
}

function remanejaOrdem(ordem) {
    var menor = eval($(this.hdOrdemMin).val());
    var maior = eval($(this.hdOrdemMax).val());

    if (ordem >= maior) {
        $(this.hdOrdemMax).val(ordem);
        ordem++;
        $(this.hdOrdem).val(ordem);
    }
    else if (ordem == menor || ordem < maior) {
        $(this.hdOrdemMax).val(maior);
        maior++;
        $(this.hdOrdem).val(maior);
    }
    else {
        $(this.hdOrdemMax_lancto).val(maior);
        $(this.hdOrdem).val(maior);
    }
}

function executaEtapa() {
    if (tela.etapaAtual == 1) {
        var status = false;
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/validaFolha",
            async: false,
            data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#boxLoading").show();
                $("#boxLoading").text("Validando folha...");
            },
            success: function (result) {
                $("#boxLoading").hide();
                if (result != null) {
                    var erros = result.d;
                    if (erros != null) {
                        if (erros.length == 0) {
                            status = true;
                        } else {
                            var texto = "";
                            for (var i = 0; i < erros.length; i++) {
                                texto += erros[i] + "\n";
                            }
                            alert(texto);
                        }
                    }
                }

                $(tela.botaoSalvar).removeAttr("disabled");
                $(tela.botaoVoltar).removeAttr("disabled");
            },
            error: OnFailed,
            complete: function () {
                if (status) {
                    $.ajax({
                        type: "POST",
                        url: "FormGenericTitulos.aspx/getSessionNotaFiscal",
                        async: false,
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function () {
                            $(tela.botaoSalvar).attr("disabled", "disabled");
                            $(tela.botaoVoltar).attr("disabled", "disabled");
                        },
                        success: function (result) {
                            if (result != null) {
                                var notaFiscal = result.d;
                                if (notaFiscal != null) {
                                    $(tela.comboTipoNotaFiscal).val(notaFiscal.produtoServico);
                                }
                            }

                            $(tela.botaoSalvar).removeAttr("disabled");
                            $(tela.botaoVoltar).removeAttr("disabled");
                        },
                        error: OnFailed
                    });
                }
            }
        });

        return status;
    } else if (tela.etapaAtual == 2) {

        var tipoNotaFiscal = $(tela.comboTipoNotaFiscal).val();

        if (tipoNotaFiscal == "0") {
            alert("Tipo não especificado.");
            return false;
        }

        var status = false;
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/criaNotaFiscal",
            async: false,
            data: "{tipo: '" + tipoNotaFiscal + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(tela.botaoSalvar).attr("disabled", "disabled");
                $(tela.botaoVoltar).attr("disabled", "disabled");
            },
            success: function () {
                $(tela.botaoSalvar).removeAttr("disabled");
                $(tela.botaoVoltar).removeAttr("disabled");
                status = true;
            },
            error: OnFailed,
            complete: function () {
                if (status) {
                    tela.montaFormNotaFiscal();
                }
            }
        });

        return status;
    } else if (tela.etapaAtual == 3) {

        if ($(tela.textNumeroNota).val() == "0" || $(tela.textNumeroNota).val() == "") {
            alert("Número da Nota não pode ser 0 ou vazio!");
            return false;
        } else {
            var status = true;
            tela.salvaNota();
            status = salvaTela("FULL");
            return status;
        }
    }
}

function salvaTela(tipo) {
    var status = true;
    var err;

    if (tipo == "FULL") {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/validaNota",
            async: false,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(tela.botaoSalvar).attr("disabled", "disabled");
                $(tela.botaoVoltar).attr("disabled", "disabled");
                $(tela.botaoSalvar).val("Validando nota...");
            },
            success: function (result) {
                if (result != null) {
                    var erros = result.d;
                    if (erros != null) {
                        if (erros.length > 0) {
                            status = false;
                            var texto = "";
                            for (var i = 0; i < erros.length; i++) {
                                texto += erros[i] + "\n";
                            }
                            alert(texto);
                            $(tela.botaoSalvar).removeAttr("disabled");
                            $(tela.botaoVoltar).removeAttr("disabled");
                            $(tela.botaoSalvar).val("Concluir");
                        }
                    }
                }
            },
            error: function () {
                alert("Erro interno, informe o administrador do sistema.");
                $(tela.botaoSalvar).removeAttr("disabled");
                $(tela.botaoVoltar).removeAttr("disabled");
                $(tela.botaoSalvar).val("Concluir");
            }
        });
    }

    if (status) {
        var altera = true;
        var lote = _GET('lote');
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/verificaTipoAlteracao",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'modulo':'" + tela.modulo + "', 'lote':" + tela.loteAtual + "}",
            success: function (a) {
                if (a.d == "FULL" && lote != null) {
                    if (!confirm("Esta operação irá excluir as baixas realizadas,\rdeseja continuar?")) {
                        altera = false;
                    }
                }
            }
        });

        if (altera) {
            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/salvaTela",
                async: false,
                data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "',tipo:'" + tipo + "',lote:" + tela.loteAtual + ", numeroNotaOld:" + $(tela.hdNumeroNotaOld).val() + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(tela.botaoSalvar).val("Salvando dados...");
                },
                success: function (result) {
                    err = result
                },
                complete: function (a) {
                    if (err != null) {
                        var erros = err.d;
                        if (erros != null) {
                            if (erros.length > 0) {
                                status = false;
                                var texto = "";
                                for (var i = 0; i < erros.length; i++) {
                                    texto += erros[i] + "\n";
                                }
                                alert(texto);
                                $(tela.botaoSalvar).removeAttr("disabled");
                                $(tela.botaoVoltar).removeAttr("disabled");
                                $(tela.botaoSalvar).val("Concluir");
                            } else {

                                if (tela.loteAtual == 0) {
                                    $.ajax({
                                        type: "POST",
                                        url: "FormGenericTitulos.aspx/getSessionLoteGerado",
                                        data: "{modulo:'" + tela.modulo + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (r) {
                                            alert("Lote " + r.d + " gerado com sucesso.");
                                            document.location.reload();
                                        },
                                        error: OnFailed
                                    });
                                } else {
									alert("Folha alterada com sucesso.");
                                    if (tela.modulo == "CAP_INCLUSAO_TITULO") {
                                        document.location.href = "FormGridLanctosContasPagar.aspx";
                                    }
                                    else if (tela.modulo == "CAR_INCLUSAO_TITULO") {
                                        document.location.href = "FormGridLanctosContasReceber.aspx";
                                    }
                                    else if (tela.modulo == "C_INCLUSAO_LANCTO") {
                                        document.location.href = "FormGridLanctosContabilidade.aspx";
                                    }
                                    else { }
                                }
                            }
                        }
                    }
                },
                error: function () {
                    alert("Erro interno, informe o administrador do sistema.");
                    $(tela.botaoSalvar).removeAttr("disabled");
                    $(tela.botaoVoltar).removeAttr("disabled");
                    $(tela.botaoSalvar).val("Concluir");
                }
            });
        }
        else {
            OnBeforeSend("");
        }
    }
}

function aliquotaImposto(tipoImposto) {
    var aliquota = 0;
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/aliquotaImposto",
        async: false,
        data: "{TIPOIMPOSTO:'" + tipoImposto + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#boxLoadingItem").show();
            $("#boxLoadingItem").text("Carregando aliquota " + tipoImposto + "...");
        },
        success: function (result) {
            $("#boxLoadingItem").hide();
            if (result != null) {
                aliquota = eval(result.d);
            }
        },
        error: function () {
            aliquota = 0;
        }
    });

    return aliquota;
}

function desativaGridItens() {
    var celulas = $("#gridItensNotaFiscal tr td");
    for (var i = 0; i < celulas.length; i++) {
        $(celulas[i]).attr("onClick", "");
        if ($(celulas[i]).attr("class") == "deletar") {
            $(celulas[i]).find("a").attr("href", "javascript:void(0);");
        }
    }
}

function montaGridItens(lista) {
    $("#gridItensNotaFiscal tr[class='linha']").remove();
    $("#gridItensNotaFiscal tr[class='linhaMarcada']").remove();
    $("#gridItensNotaFiscal tr[class='linhaSelecionada']").remove();

    if (lista != null) {
        for (var i = 0; i < lista.length; i++) {
            var linha = "<tr class='linha' onmouseover=\"mudaCor(this);\" onmouseout=\"voltaCor(this);\">";
            linha += "<td onClick=\"tela.alteraLancto(" + lista[i].ordem + ", this.parentNode);\">";
            linha += lista[i].ordem;
            linha += "</td>";
            linha += "<td onClick=\"tela.alteraLancto(" + lista[i].ordem + ", this.parentNode);\">";
            linha += lista[i].descProduto;
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].qtde, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].valorTotal, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].valorBaseImp, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].aliquotaIcms, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].aliquotaPis, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='valor' onClick=\"tela.alteraItem(" + lista[i].ordem + ", this.parentNode);\">";
            linha += number_format(lista[i].aliquotaCofins, 2, ',', '.');
            linha += "</td>";
            linha += "<td class='deletar'>";
            linha += "<a href=\"javascript:tela.deletaItem(" + lista[i].ordem + ");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";
            linha += "</td>";
            linha += "</tr>";
            $("#gridItensNotaFiscal").append(linha);
        }
    }
}

function montaFormNotaFiscal() {
    var notaFiscal;
    var status;
    var tipoNotaFiscal = $(tela.comboTipoNotaFiscal).val();
    var funcao = "";
    if (tipoNotaFiscal == "P") {
        funcao = "getSessionNotaFiscalProduto";
    } else {
        funcao = "getSessionNotaFiscalServico";
    }

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/" + funcao,
        async: false,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(tela.botaoSalvar).attr("disabled", "disabled");
            $(tela.botaoVoltar).attr("disabled", "disabled");
        },
        success: function (result) {
            tela.limpaCamposNota();
            $(tela.hdOrdem).val("1");
            $(tela.hdOrdemMin).val("1");
            $(tela.hdOrdemMax).val("1");

            if (result != null) {
                notaFiscal = result.d;
                if (notaFiscal != null) {
                    if (notaFiscal.itens.length > 0) {

                        $(tela.hdOrdemMin).val(notaFiscal.itens[0].ordem);
                        $(tela.hdOrdemMax).val(notaFiscal.itens[notaFiscal.itens.length - 1].ordem);
                        var newOrdem = eval(notaFiscal.itens[eval(notaFiscal.itens.length - 1)].ordem);
                        newOrdem++;
                        $(tela.hdOrdem).val(newOrdem);
                    }

                    $(tela.labelFreteNota).hide();
                    $(tela.labelIpiNota).hide();
                    $(tela.camposServico).hide();
                    $(tela.labelCfopItem).hide();
                    $(tela.labelIpiItem).hide();
                    $(tela.labelIrItem).hide();
                    $(tela.labelCsllItem).hide();

                    $(tela.areaFornecedorClienteNota).text($(tela.comboFornecedor + " option:selected").text());
                    $(tela.hdProdutoServico).val(notaFiscal.produtoServico);
                    if (notaFiscal.numeroNota > 0) {
                        $(tela.textNumeroNota).val(notaFiscal.numeroNota);
                        $(tela.textNumeroEletronica).val(notaFiscal.numeroEletronica);
                        $(tela.comboEntradaSaidaNota).val(notaFiscal.entradaSaida);
                        $(tela.textValorNota).val(number_format(notaFiscal.valor, 2, ",", "."));
                        $(tela.textIcmsNota).val(number_format(notaFiscal.icms, 2, ",", "."));
                        $(tela.textDescontoNota).val(number_format(notaFiscal.descontos, 2, ",", "."));
                        $(tela.textBaseImpNota).val(number_format(notaFiscal.baseImpostos, 2, ",", "."));

                        if (notaFiscal.produtoServico == "P") {
                            $(tela.textFreteNota).val(number_format(notaFiscal.frete, 2, ",", "."));
                            $(tela.textIpiNota).val(number_format(notaFiscal.ipi, 2, ",", "."));
                        } else {
                            $(tela.textCsllNota).val(number_format(notaFiscal.valorCsll, 2, ",", "."));
                            $(tela.textIrNota).val(number_format(notaFiscal.valorIR, 2, ",", "."));
                            $(tela.textIssNota).val(number_format(notaFiscal.valorIss, 2, ",", "."));
                            $(tela.textPisRetidoNota).val(number_format(notaFiscal.valorPisRetido, 2, ",", "."));
                            $(tela.textCofinsRetidoNota).val(number_format(notaFiscal.valorCofinsRetido, 2, ",", "."));
                            $(tela.textCsllRetidoNota).val(number_format(notaFiscal.valorCsllRetido, 2, ",", "."));
                            $(tela.textIrRetidoNota).val(number_format(notaFiscal.valorIrRetido, 2, ",", "."));
                            $(tela.textIssRetidoNota).val(number_format(notaFiscal.valorIssRetido, 2, ",", "."));
                        }
                    }

                    tela.limpaCamposItemNota();

                    if (notaFiscal.produtoServico == "P") {
                        $(tela.labelFreteNota).show();
                        $(tela.labelIpiNota).show();
                        $(tela.labelCfopItem).show();
                        $(tela.labelIpiItem).show();
                    } else {
                        $(tela.camposServico).show();
                        $(tela.labelIrItem).show();
                        $(tela.labelCsllItem).show();
                    }
                    status = true;
                }
            }
            $(tela.botaoSalvar).removeAttr("disabled");
            $(tela.botaoVoltar).removeAttr("disabled");
        },
        error: OnFailed,
        complete: function () {
            if (status) {
                montaGridItens(notaFiscal.itens);
            }
        }
    });
}

function salvaNota() {

    var produtoServico = $(tela.hdProdutoServico).val();
    var numeroNota = limpaNumero($(tela.textNumeroNota).val());
    var numeroEletronica = limpaNumero($(tela.textNumeroEletronica).val());
    var entradaSaida = $(tela.comboEntradaSaidaNota).val();
    var valor = limpaNumero($(tela.textValorNota).val());
    var icms = limpaNumero($(tela.textIcmsNota).val());
    var desconto = limpaNumero($(tela.textDescontoNota).val());
    var baseImposto = limpaNumero($(tela.textBaseImpNota).val());
    var frete = 0;
    var ipi = 0;
    var csll = 0;
    var ir = 0;
    var iss = 0;
    var pisRetido = 0;
    var cofinsRetido = 0;
    var csllRetido = 0;
    var irRetido = 0;
    var issRetido = 0;

    if (produtoServico == "P") {

        frete = limpaNumero($(tela.textFreteNota).val());
        ipi = limpaNumero($(tela.textIpiNota).val());
    } else {
        csll = limpaNumero($(tela.textCsllNota).val());
        ir = limpaNumero($(tela.textIrNota).val());
        iss = limpaNumero($(tela.textIssNota).val());
        pisRetido = limpaNumero($(tela.textPisRetidoNota).val());
        cofinsRetido = limpaNumero($(tela.textCofinsRetidoNota).val());
        csllRetido = limpaNumero($(tela.textCsllRetidoNota).val());
        irRetido = limpaNumero($(tela.textIrRetidoNota).val());
        issRetido = limpaNumero($(tela.textIssRetidoNota).val());
    }

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/salvaNota",
        data: "{produtoServico:'" + produtoServico + "',numeroNota: " + numeroNota + ",entradaSaida:'" + entradaSaida + "',valor:" + valor +
        ",icms:" + icms + ",desconto:" + desconto + ", baseImposto:" + baseImposto + ", frete:" + frete + ",ipi:" + ipi + ",csll:" + csll + ",ir:" + ir +
        ",iss:" + iss + ",pisRetido:" + pisRetido + ",cofinsRetido:" + cofinsRetido + ",csllRetido:" + csllRetido + ",irRetido:" + irRetido + ",issRetido:" + issRetido + ",numeroEletronica: '" + numeroEletronica + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: OnFailed
    });
}

function verificaGeraCredito() {
    var contas = new Array();
    var jsonContas = "{}";
    var geraCredito = false;

    $.ajax({
        type: "POST",
        async: false,
        url: "Default.aspx/getSessionLanctos",
        data: "{modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Montando tela..."),
        success: function (r) {
            var result = r.d;
            $("#boxLoading").hide();
            if (result != null || result != undefined) {
                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        contas.push(result[i].conta);
                    }
                    jsonContas = JSON.stringify(contas);
                }
            }
        },
        complete: function () {
            $.ajax({
                type: "POST",
                async: false,
                url: "FormGenericTitulos.aspx/verificaGeraCredito",
                data: "{contas:" + jsonContas + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: OnBeforeSend("Montando tela..."),
                success: function (result) {
                    geraCredito = result.d;
                },
                error: OnFailed
            });
        },
        error: OnFailed
    });
    return geraCredito;
}

function salvaFolha(input) {
    var result;
    if (tela.modulo != "MODELO_LANCAMENTO") {
        if (tela.executaEtapa()) {
			if (tela.etapaAtual == 1) {
				if (!verificaGeraCredito()) {
                    tela.salvaTela("MIN");
                    return false;
                }
            }
            if (tela.etapaAtual < 3) {
                tela.etapaAtual++;
            }
            tela.exibeEtapa();
        }
    } else {
        $.ajax({
            type: "POST",
            async: false,
            url: "FormGenericTitulos.aspx/salvaFolha",
            data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $(input).val("Processando...");
                $(input).attr("disabled", "disabled");
            },
            success: function (r) {

                result = r.d;

            },
            complete: function (a) {
                if (result != undefined) {
                    if (result.length == 0) {
                        alert("Folha salva com sucesso.");
                        document.location.reload();
                    }
                    else {
                        var texto = "";
                        for (var i = 0; i < result.length; i++) {
                            texto += result[i] + "\n";
                        }

                        alert(texto);
                    }
                    $(input).val("Próximo");
                    $(input).removeAttr("disabled");
                }
            },
            error: OnFailed
        });
    }
}

function alteraFolha(input, lote) {
    var boolFechamento = false;
    var boolFilhos = false;
    var tipoAlteracao = "FULL";
    var erros = 0;

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/verificaTipoAlteracao",
        data: "{modulo:'" + tela.modulo + "',lote:" + lote + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $(input).val("Processando...");
            $(input).attr("disabled", "disabled");
        },
        success: function (result) {
            tipoAlteracao = result.d;
        },
        error: function (result, msg) {
            erros++;
        },
        complete: function () {
            if (erros == 0) {
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/verificaFechamentoLote",
                    data: "{modulo:'" + tela.modulo + "',lote:" + lote + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        $(input).val("Processando...");
                        $(input).attr("disabled", "disabled");
                    },
                    success: function (result) {
                        boolFechamento = result.d;
                    },
                    error: function () {
                        erros++;
                    },
                    complete: function () {
                        if (erros == 0) {
                            if (!boolFechamento) {
                                alert("O período contábil deste lançamento/título encontra-se encerrado.\nFavor contactar o administrador do sistema para reabrir o período.");
                                $(input).val("Concluir");
                                $(input).removeAttr("disabled");
                            } else {
                                $.ajax({
                                    type: "POST",
                                    url: "FormGenericTitulos.aspx/verificaFilhosLote",
                                    data: "{modulo:'" + tela.modulo + "',lote:" + lote + "}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        boolFilhos = result.d;
                                    },
                                    error: function () {
                                        erros++;
                                    },
                                    complete: function () {
                                        if (erros == 0) {
                                            if (tipoAlteracao == "MIN") {
                                                boolFilhos = true;
                                            }

                                            if (!boolFilhos) {
                                                var confirma = window.confirm("Títulos já baixados - Para alterar este lançamento, deve-se antes ,\ncancelar-se sua baixa - Você deseja cancelar a baixa e reabrir o título?\nIsto significará que após as alterações, o usuário deverá processar a\nbaixa do título manualmente.Atenção, este título pode ter sido baixado\njuntamente com vários outros.Deseja prosseguir?");
                                                if (confirma) {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: "FormGenericTitulos.aspx/alteraFolha",
                                                        data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "',lote:" + lote + ", tipo: '" + tipoAlteracao + "'}",
                                                        contentType: "application/json; charset=utf-8",
                                                        dataType: "json",
                                                        success: function (r) {
                                                            var result = r.d;
                                                            if (result != undefined) {
                                                                if (result.length == 0) {
                                                                    alert("Folha de Lançamento alterada com sucesso.");
                                                                    if (tela.modulo == "CAP_INCLUSAO_TITULO") {
                                                                        document.location.href = "FormGridLanctosContasPagar.aspx";
                                                                    }
                                                                    else if (tela.modulo == "CAR_INCLUSAO_TITULO") {
                                                                        document.location.href = "FormGridLanctosContasReceber.aspx";
                                                                    }
                                                                    else if (tela.modulo == "C_INCLUSAO_LANCTO") {
                                                                        document.location.href = "FormGridLanctosContabilidade.aspx";
                                                                    }
                                                                    else { }
                                                                }
                                                                else {
                                                                    var texto = "";
                                                                    for (var i = 0; i < result.length; i++) {
                                                                        texto += result[i] + "\n";
                                                                    }

                                                                    alert(texto);
                                                                }
                                                                $(input).val("Concluir");
                                                                $(input).removeAttr("disabled");
                                                            }
                                                        },
                                                        error: OnFailed
                                                    });
                                                } else {
                                                    $(input).val("Concluir");
                                                    $(input).removeAttr("disabled");
                                                }
                                            } else {

                                                $.ajax({
                                                    type: "POST",
                                                    url: "FormGenericTitulos.aspx/alteraFolha",
                                                    data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "',lote:" + lote + ", tipo: '" + tipoAlteracao + "'}",
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (r) {
                                                        var result = r.d;
                                                        if (result != undefined) {
                                                            if (result.length == 0) {
                                                                alert("Folha de Lançamento alterada com sucesso.");
                                                                if (tela.modulo == "CAP_INCLUSAO_TITULO") {
                                                                    document.location.href = "FormGridLanctosContasPagar.aspx";
                                                                }
                                                                else if (tela.modulo == "CAR_INCLUSAO_TITULO") {
                                                                    document.location.href = "FormGridLanctosContasReceber.aspx";
                                                                }
                                                                else if (tela.modulo == "C_INCLUSAO_LANCTO") {
                                                                    document.location.href = "FormGridLanctosContabilidade.aspx";
                                                                }
                                                                else { }
                                                            }
                                                            else {
                                                                var texto = "";
                                                                for (var i = 0; i < result.length; i++) {
                                                                    texto += result[i] + "\n";
                                                                }

                                                                alert(texto);
                                                            }
                                                            $(input).val("Concluir");
                                                            $(input).removeAttr("disabled");
                                                        }
                                                    },
                                                    error: OnFailed
                                                });
                                            }
                                        } else {
                                            alert("Ocorreu um erro durante o processo, por favor tente novamente.");
                                            $(input).val("Concluir");
                                            $(input).removeAttr("disabled");
                                        }
                                    }
                                });
                            }
                        } else {
                            alert("Ocorreu um erro durante o processo, por favor tente novamente.");
                            $(input).val("Concluir");
                            $(input).removeAttr("disabled");
                        }
                    }
                });
            } else {
                alert("Ocorreu um erro durante o processo, por favor tente novamente.");
                $(input).val("Concluir");
                $(input).removeAttr("disabled");
            }
        }
    });


}

function aguarde() {
    $('#conteudo').toggleLoading({
        addClass: "my-class",
        addText: "Aguarde..."
    });
}

function atualizaJobs(input) {
    if (input.checked) {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getListaJobs",
            data: "{'status': 'true'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            beforeSend: function () {
                //aguarde();

                $(tela.comboJob_lancto).html("");
                $("<option value='0'>Carregando...</option>").appendTo(tela.comboJob_lancto);
                $(tela.comboJob_lancto).trigger("chosen:updated");

            },
            success: function (r) {
                var result = r.d;
                if (result.length > 0) {
                    $(tela.comboJob_lancto).html("");
                    $("<option value='0'>Escolha</option>").appendTo(tela.comboJob_lancto);

                    for (var i = 0; i < result.length; i++) {
                        $("<option value='" + result[i]["COD_JOB"] + "'>" + result[i]["DESCRICAO_COMPLETO"] + "</option>").appendTo(tela.comboJob_lancto);
                    }
                }
                else {
                    $(tela.comboJob_lancto).html("");
                    $("<option value='0'>Escolha</option>").appendTo(tela.comboJob_lancto);
                }

                $(tela.comboJob_lancto).trigger("chosen:updated");
            },
            complete: function () {
                tela.carregaJob(0);
                //aguarde();
            },
            error: OnFailed
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getListaJobs",
            data: "{status: false}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            beforeSend: function () {
                //aguarde();

                $(tela.comboJob_lancto).html("");
                $("<option value='0'>Carregando...</option>").appendTo(tela.comboJob_lancto);
                $(tela.comboJob_lancto).trigger("chosen:updated");
            },
            success: function (r) {
                var result = r.d;
                if (result.length > 0) {
                    $(tela.comboJob_lancto).html("");
                    $("<option value='0'>Escolha</option>").appendTo(tela.comboJob_lancto);

                    for (var i = 0; i < result.length; i++) {
                        $("<option value='" + result[i]["COD_JOB"] + "'>" + result[i]["DESCRICAO_COMPLETO"] + "</option>").appendTo(tela.comboJob_lancto);
                    }
                }
                else {
                    $(tela.comboJob_lancto).html("");
                    $("<option value='0'>Escolha</option>").appendTo(tela.comboJob_lancto);
                }

                $(tela.comboJob_lancto).trigger("chosen:updated");
            },
            complete: function () {
                tela.carregaJob(0);
                //aguarde();
            },
            error: OnFailed
        });
    }
}

function verificaAlteracaoParcelas(seqLote, input) {
    if ($(input).val() > 0) {
        var parcelasAtual = eval($(input).val());
        var seqLoteCerto = 0;

        if (seqLote > 0)
            seqLoteCerto = seqLote;
        else
            seqLoteCerto = $(tela.hdSeqLote_lancto).val();

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getSessionLanctos",
            data: "{modulo:'" + tela.modulo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("carregando lançamentos"),
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null && result != undefined) {

                    if (result.length > 0) {
                        var atual = null;

                        for (var i = 0; i < result.length; i++) {
                            if (result[i].seqLote == seqLoteCerto)
                                atual = result[i];
                        }

                        if (atual != null) {
                            if (atual.vencimentos.length != parcelasAtual) {
                                if (seqLote > 0)
                                    tela.loadVencimentos(seqLoteCerto, input.id, tela.prefixo_s + "textValorTotal",
                                        tela.prefixo_s + "botaoVencimento", tela.prefixo_s + "H_SEQLOTE_LANCTO");
                                else
                                    tela.loadVencimentos(-1, input.id, tela.prefixo_s + "textValor_lancto",
                                        tela.prefixo_s + "botaoVencimento", tela.prefixo_s + "H_SEQLOTE_LANCTO");
                            }
                        }
                        else {
                            if (seqLote < 0)
                                tela.loadVencimentos(-1, input.id, tela.prefixo_s + "textValor_lancto",
                                    tela.prefixo_s + "botaoVencimento", tela.prefixo_s + "H_SEQLOTE_LANCTO");
                            else
                                alert("Lançamento selecionado para verificação das parcelas não existe.");
                        }
                    }
                }
            }
        });

    }
    else {
        var parcelasAtual = eval($(input).val());
        var seqLoteCerto = 0;

        if (seqLote > 0)
            seqLoteCerto = seqLote;
        else
            seqLoteCerto = $(this.hdSeqLote_lancto).val();

        alert("Não é possivel inserir Zero parcelas.");

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getSessionLanctos",
            data: "{modulo:'" + modulo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Carregando lançamentos..."),
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null && result != undefined) {
                    if (result.length > 0) {
                        var atual = null;

                        for (var i = 0; i < result.length; i++) {
                            if (result[i].seqLote == seqLoteCerto)
                                atual = result[i];
                        }

                        if (atual != null) {
                            if (atual.vencimentos.length == 0)
                                $(input).val("1");
                            else
                                $(input).val(atual.vencimentos.length);
                        }
                        else {
                            if (seqLote < 0)
                                $(input).val("1");
                            else
                                alert("Lançamento selecionado para verificação das parcelas não existe.");
                        }
                    }
                }
            }
        });
    }
}

function alteraConsultorLancto1(codConsultor) {
    var errosAlteracao;
    if (codConsultor == 0) {
        alert("Escolha um Consultor!");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/alteraConsultorLancto1",
        data: "{modulo:'" + tela.modulo + "' ,codConsultor: " + codConsultor + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Alterando consultor do lançamento..."),
        success: function (result) {
            errosAlteracao = result.d;
        },
        complete: function () {
            if (errosAlteracao.length == 0) {
                $("#boxLoading").hide();
            }
            else {
                var texto = "";
                for (var i = 0; i < errosAlteracao.length; i++) {
                    texto += errosAlteracao[i] + "\n";
                }
                alert(texto);
            }
        },
        error: OnFailed
    });
}

function carregaModelo(terceiro, modeloSelecionado) {
    var erroGetListaModelos = 0;
    var modelo = 0;
    var existeLoteAnterior = false;

    //    alert($(tela.prefixo + "comboFornecedor").chosen().val());
    //    alert($(tela.prefixo + "comboFornecedor").find("option:selected").text());

    if (modeloSelecionado <= 0) {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/loadTerceiro",
            data: "{terceiro: " + terceiro + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#boxLoading").show();
                $("#boxLoading").html("Buscando Terceiro...");
            },
            success: function (r) {
                var result = r.d;
                if (tela.modulo == "CAP_INCLUSAO_TITULO")
                    modelo = result.sugestaoModeloCP;
                else
                    modelo = result.sugestaoModeloCR;
            },
            error: OnFailed,
            complete: function () {
                var lote = _GET('lote');
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/totalLotesAnteriores",
                    data: "{modulo: '" + tela.modulo + "', codigoTerceiro: " + terceiro + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        $("#boxLoading").html("Contando lotes anteriores do terceiro...");
                    },
                    success: function (r) {
                        var result = r.d;

                        if (result > 0 && lote == null) {
                            existeLoteAnterior = true;
                        }
                    },
                    error: OnFailed,
                    complete: function () {
                        tela.buscaLoteAnterior = false;
                        if (existeLoteAnterior && lote == null) {
                            tela.buscaLoteAnterior = window.confirm("Deseja carregar a ultima folha realizada criada para o fornecedor?");
                        }
                        var data = $(tela.textData).val();
                        if (data == "")
                            data = $(tela.dataHidden).val();
                        if (tela.buscaLoteAnterior && lote == null) {
                            $.ajax({
                                type: "POST",
                                url: "FormGenericTitulos.aspx/carregaUltimoLote",
                                data: "{modulo:'" + tela.modulo + "',  codigoTerceiro: " + $(tela.prefixo + "comboFornecedor").chosen().val() + ", data: '" + data + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () {
                                    $("#boxLoading").html("Carregando a última folha de lançamentos do fornecedor..");
                                },
                                success: function (r) {
                                    var result = r.d;
                                    $("#boxLoading").hide();
                                    if (result != null || result != undefined) {
                                        if (result.length > 0) {
                                            tela.montaGridLanctos(result);
                                            tela.calculaTotais(result);
                                            tela.ativaFormTitulo();
                                            tela.ativaFormLancamento();

                                            $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                                            $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                                            $(tela.textQtd).val(number_format(result[0].qtd, 6, ',', '.'));
                                            $(tela.textValorUnit).val(number_format(result[0].valorUnit, 6, ',', '.'));
                                            $(tela.textValorBruto).val(number_format(result[0].valorBruto, 2, ',', '.'));
                                            $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                                            //$(tela.comboFornecedor).val(result[0].terceiro);
                                            //$(tela.textData).val(result[0].dataLancamento_formatada);
                                            $(tela.textParcelas).val(result[0].vencimentos.length);
                                            //tela.carregaModelo(result[0].terceiro, result[0].modelo);
                                            $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                                            $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                                            var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                                            newSeqLote++;
                                            $(tela.hdSeqLote_lancto).val(newSeqLote);

                                            $(tela.textQtd_lancto).val("1");
                                            $(tela.textValorUnit_lancto).val("0");
                                            $(tela.textValor_lancto).val("0");
                                            $(tela.textParcelas_lancto).val("1");
                                            $(tela.comboConsultor).val(result[0].codConsultor);
                                        }
                                        else {
                                            //$(tela.textNumeroDocumento).val("");
                                            $(tela.textParcelas).val("1");
                                            $(tela.textQtd).val("1");
                                            $(tela.textValorTotal).val("0");
                                            $(tela.textValorUnit).val("0");
                                            $(tela.textValorBruto).val("0");
                                            $(tela.hdSeqLote_lancto).val("2");
                                            $(tela.hdSeqLoteMin_lancto).val("1");
                                            $(tela.hdSeqLoteMax_lancto).val("1");
                                            $(tela.textQtd_lancto).val("1");
                                            $(tela.textValor_lancto).val("0");
                                            $(tela.textValorUnit_lancto).val("0");
                                            $(tela.textParcelas_lancto).val("1");
                                            tela.desativaFormTitulo();
                                            tela.desativaFormLancamento();
                                        }
                                    } else {
                                        //$(tela.textNumeroDocumento).val("");
                                        $(tela.textParcelas).val("1");
                                        $(tela.textQtd).val("1");
                                        $(tela.textValorTotal).val("0");
                                        $(tela.textValorUnit).val("0");
                                        $(tela.hdSeqLote_lancto).val("2");
                                        $(tela.hdSeqLoteMin_lancto).val("1");
                                        $(tela.hdSeqLoteMax_lancto).val("1");
                                        $(tela.textQtd_lancto).val("1");
                                        $(tela.textValor_lancto).val("0");
                                        $(tela.textValorUnit_lancto).val("0");
                                        $(tela.textParcelas_lancto).val("1");
                                        tela.desativaFormTitulo();
                                        tela.desativaFormLancamento();
                                    }
                                },
                                error: OnFailed
                            });
                        } else {
                            $.ajax({
                                type: "POST",
                                url: "FormGenericTitulos.aspx/getListaModelos",
                                data: "{modulo: '" + tela.modulo + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () {
                                    $("#boxLoading").html("Pegando lista de modelos...");
                                },
                                success: function (r) {
                                    var result = r.d;
                                    if (result != null || result != undefined) {
                                        if (result.length > 0) {
                                            $(tela.comboModelo).html("");
                                            $("<option value='0'>Escolha</option>").appendTo(tela.comboModelo);

                                            if (modelo != 0) {
                                                var existeModelo = 0;
                                                for (var i = 0; i < result.length; i++) {
                                                    if (result[i]["COD_MODELO"] == modelo)
                                                        existeModelo++;
                                                    if (result[i]["PADRAO_DEFAULT"] == "True") {
                                                        $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected' >" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                                        $(tela.comboModelo + ' ul.chosen-results').append('<li class="active-result">' + result[i]["NOME"] + '</li>');
                                                    } else {
                                                        $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                                    }
                                                }

                                                if (existeModelo > 0) {
                                                    $(tela.comboModelo).val(modelo);
                                                }
                                            }
                                            else {
                                                for (var i = 0; i < result.length; i++) {
                                                    if (result[i]["PADRAO_DEFAULT"] == "True") {
                                                        $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                                        modelo = result[i]["COD_MODELO"];
                                                    }
                                                    else {
                                                        $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                                    }

                                                }
                                            }
                                        }
                                        else {
                                            erroGetListaModelos++;
                                            alert("Não existe nenhum modelo para contas à pagar cadastrado no sistema.");
                                            tela.desativaFormTitulo();
                                            tela.desativaFormLancamento();
                                        }
                                    }
                                    else {
                                        erroGetListaModelos++;
                                    }
                                },
                                complete: function () {
                                    if (erroGetListaModelos == 0 && lote == null) {
                                        $.ajax({
                                            type: "POST",
                                            url: "FormGenericTitulos.aspx/loadLanctosModelo",
                                            data: "{modulo:'" + tela.modulo + "', cod_modelo: " + $(tela.comboModelo).val() + ", terceiroEscolhido: " + $(tela.prefixo + "comboFornecedor").chosen().val() + ", descTerceiroEscolhido: '" + $(tela.prefixo + "comboFornecedor").find("option:selected").text() + "', data: '" + data + "', valorbruto: '" + $(tela.prefixo + "textValorBruto").val().replace(",", ".") + "', codmoeda: '" + $(tela.prefixo + "comboEtapaMoeda").val() + "'}",
                                            contentType: "application/json; charset=utf-8",
                                            dataType: "json",
                                            beforeSend: function () {
                                                $("#boxLoading").html("Pegando lançamentos do modelo...");
                                            },
                                            success: function (r) {
                                                var result = r.d;
                                                $("#boxLoading").hide();
                                                if (result != null && result != undefined) {
                                                    if (result.length > 0) {
                                                        tela.montaGridLanctos(result);
                                                        tela.calculaTotais(result);

                                                        $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                                                        $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                                                        var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                                                        newSeqLote++;

                                                        $(tela.hdSeqLote_lancto).val(newSeqLote);

                                                        tela.ativaFormTitulo();
                                                        tela.ativaFormLancamento();
                                                        $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                                                        $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                                                        $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                                                        $(tela.textParcelas).val(result[0].vencimentos.length);
                                                        $(tela.comboConsultor).val(result[0].codConsultor);

                                                        if (tela.exigeModelo) {
                                                            $(tela.comboConta_lancto).attr("disabled", "disabled").trigger("chosen:updated");
                                                            $(tela.botaoSalvar_lancto).attr("disabled", "disabled");
                                                        }
                                                    }
                                                    else {
                                                        alert("O modelo escolhido não possui lançamentos configurados.");
                                                        desativaFormTitulo();
                                                        desativaFormLancamento();
                                                    }
                                                }
                                                else {
                                                    alert("O modelo escolhido não possui lançamentos configurados.");
                                                    tela.desativaFormTitulo();
                                                    tela.desativaFormLancamento();
                                                }
                                            },
                                            error: OnFailed
                                        });
                                    } else { $("#boxLoading").html(""); }
                                },
                                error: OnFailed
                            });
                        }
                    }
                });
            }
        });

    }
    else {
        modelo = modeloSelecionado;

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getListaModelos",
            data: "{modulo: '" + tela.modulo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#boxLoading").show();
                $("#boxLoading").html("Buscando lista de modelos...");
            },
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null || result != undefined) {
                    if (result.length > 0) {
                        $(tela.comboModelo).html("");
                        $("<option value='0'>Escolha</option>").appendTo(tela.comboModelo);

                        if (modelo != 0) {
                            for (var i = 0; i < result.length; i++) {
                                if (result[i]["COD_MODELO"] == modelo)
                                    $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                else
                                    $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                            }
                        }
                        else {
                            for (var i = 0; i < result.length; i++) {
                                if (result[i]["PADRAO_DEFAULT"] == "True") {
                                    $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                    modelo = result[i]["COD_MODELO"];
                                }
                                else {
                                    $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo(tela.comboModelo);
                                }

                            }
                        }

                    }
                    else {
                        alert("Não existe nenhum modelo para contas à pagar cadastrado no sistema.");

                        tela.desativaFormTitulo();
                        tela.desativaFormLancamento();
                    }
                }
            },
            error: OnFailed
        });
    }
}

function alteraModelo(modelo) {
    var realiza = false;

    realiza = window.confirm("Alterando o modelo você irá perder todos os lançamentos criados, deseja continuar?");

    if (realiza) {
        var terceiro = $(this.comboFornecedor).chosen().val();
        var descTerceiro = $(this.comboFornecedor + " option:selected").text();

        if ($(this.comboFornecedor).chosen().val() == "" || $(this.comboFornecedor).chosen().val() == undefined)
            terceiro = 0;

        if ($(this.comboFornecedor + " option:selected").text() == "" || $(this.comboFornecedor + " option:selected").text() == undefined)
            descTerceiro = "";

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/loadLanctosModelo",
            data: "{modulo:'" + tela.modulo + "', cod_modelo: " + modelo + ", terceiroEscolhido: " + terceiro + ", descTerceiroEscolhido: '" + descTerceiro + "',data:'" + $(tela.textData).val() + "', valorbruto:0,codmoeda:0}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Alterando modelo..."),
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null && result != undefined) {
                    if (result.length > 0) {

                        tela.montaGridLanctos(result);
                        tela.calculaTotais(result);

                        $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                        $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                        var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                        newSeqLote++;

                        $(tela.hdSeqLote_lancto).val(newSeqLote);

                        tela.ativaFormTitulo();
                        tela.ativaFormLancamento();

                        $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                        $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                        $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                        $(tela.textParcelas).val(result[0].vencimentos.length);
                        $(tela.comboConsultor).val(result[0].codConsultor);
                    }
                    else {
                        alert("O modelo escolhido não possui lançamentos configurados.");

                        if (tela.modulo != "C_INCLUSAO_LANCTO") {
                            tela.desativaFormTitulo();
                            tela.desativaFormLancamento();
                        }
                    }
                }
                else {
                    alert("O modelo escolhido não possui lançamentos configurados.");

                    if (tela.modulo != "C_INCLUSAO_LANCTO") {
                        tela.desativaFormTitulo();
                        tela.desativaFormLancamento();
                    }
                }
            },
            error: OnFailed
        });
    }
}

function selecionaJobDefault(conta) {
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/getContaContabil",
        data: "{codigo:'" + conta + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        beforeSend: OnBeforeSend("Carregando Job Default da conta..."),
		success: function (r) {
            var result = r.d;
            $("#boxLoading").hide();
            if (result != undefined && result != "") {
				var jobDefault = result.jobDefault;
				if (jobDefault > 0 && $(tela.comboJob_lancto).val() === "0") {
                    $(tela.comboJob_lancto).val(jobDefault).trigger("chosen:updated");;
                    tela.carregaJob(jobDefault);
                }
            }
        },
        complete: function (r) {
            tela.verificaContaGeraTitulo();
        },
        error: OnFailed
    });
}

//function buscaTerceirosDisponiveis() {
//    var buscaBanco = false;
//    var dados = null;
//    var tipoConta = "";

//    if (tela.modulo == "CAP_INCLUSAO_TITULO" || tela.modulo == "CAR_INCLUSAO_TITULO") {
//        if ($(tela.comboConta_lancto).val() != "0") {
//            $.ajax({
//                type: "POST",
//                async: false,
//                url: "FormGenericTitulos.aspx/getContaContabil",
//                data: "{codigo:'" + $(tela.comboConta_lancto).val() + "'}",
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                beforeSend: OnBeforeSend("Carregando Job Default da conta..."),
//                success: function(conta) {
//                    $("#boxLoading").hide();
//                    if (conta != undefined && conta != "") {
//                        tipoConta = conta.tipo;
//                    }
//                },
//                complete: function(r) {
//                    $.ajax({
//                        type: "POST",
//                        async: false,
//                        url: "FormGenericTitulos.aspx/carregaTerceirosDisponiveis",
//                        data: "{modulo:'" + tela.modulo + "'}",
//                        contentType: "application/json; charset=utf-8",
//                        dataType: "json",
//                        beforeSend: OnBeforeSend("Carregando terceiros..."),
//                        success: function(result) {
//                            $("#boxLoading").hide();
//                            dados = result;
//                            if (tela.modulo == "CAP_INCLUSAO_TITULO" || tela.modulo == "CAR_INCLUSAO_TITULO") {
//                                if (tipoConta == "CR" || tipoConta == "CP") {
//                                    if ($(tela.hdSeqLote_lancto).val() == "1") {
//                                        buscaBanco = true;
//                                    } else {
//                                        buscaBanco = false;
//                                    }
//                                } else {
//                                    if (tipoConta == "I" || tipoConta == "IR") {
//                                        buscaBanco = true;
//                                    } else {
//                                        buscaBanco = false;
//                                    }
//                                }
//                            } else {
//                                buscaBanco = true;
//                            }
//                        },
//                        complete: function() {
//                            if (!buscaBanco) {
//                                $(tela.comboTerceiro_lancto).html("");
//                                $("<option value='" + $(tela.comboFornecedor).val() + "'>" + $(tela.comboFornecedor + " option:selected").text() + "</option>").appendTo(tela.comboTerceiro_lancto);
//                            } else {
//                                if (dados != undefined) {
//                                    if (dados.length > 0) {
//                                        $(tela.comboTerceiro_lancto).html("");
//                                        $("<option value='0' selected='selected'>Escolha</option>").appendTo(tela.comboTerceiro_lancto);
//                                        for (var i = 0; i < dados.length; i++) {
//                                            $("<option value='" + dados[i].COD_EMPRESA + "'>" + dados[i].NOME_RAZAO_SOCIAL + "</option>").appendTo(tela.comboTerceiro_lancto);
//                                        }
//                                    } else {
//                                        $(tela.comboTerceiro_lancto).html("");
//                                        $("<option value='0'>Nenhum terceiro encontrado</option>").appendTo(tela.comboTerceiro_lancto);
//                                    }
//                                } else {
//                                    $(tela.comboTerceiro_lancto).html("");
//                                    $("<option value='0'>Nenhum terceiro encontrado</option>").appendTo(tela.comboTerceiro_lancto);
//                                }
//                            }
//                        },
//                        error: OnFailed
//                    });
//                },
//                error: OnFailed
//            });
//        }
//    }
//}

function salvaLancto() {
    var erros = new Array();
    var seqLote = eval($(this.hdSeqLote_lancto).val());
    var data = $(this.textData).val();
    var debCred = $(this.comboDebCred_lancto).val();
    var conta = $(this.comboConta_lancto).val();
    var descConta = $(this.comboConta_lancto + " option:selected").text();
    var job = $(this.comboJob_lancto).val()
    var descJob = $(this.comboJob_lancto + " option:selected").text();
    var qtd = limpaNumero($(this.textQtd_lancto).val());
    var valorUnit = limpaNumero($(this.textValorUnit_lancto).val());

    try { var valorBruto = limpaNumero($(this.textValorBruto).val()); }
    catch (err) { var valorBruto = 0; }

    try { var valorGrupo = limpaNumero($(this.valorGrupo).val()); }
    catch (err) { var valorGrupo = 0; }

    var codMoeda = $(this.comboEtapaMoeda + (get('modulo') == 'C_INCLUSAO_LANCTO' ? 'InclusaLancto' : '')).val();

    var valor = limpaNumero($(this.textValor_lancto).val());
    var linhaNegocio = $(this.comboLinhaNegocio_lancto).val();
    var descLinhaNegocio = $(this.comboLinhaNegocio_lancto + " option:selected").text();
    var divisao = $(this.comboDivisao_lancto).val();
    var descDivisao = $(this.comboDivisao_lancto + " option:selected").text();
    var cliente = $(this.comboCliente_lancto).val();
    var descCliente = $(this.comboCliente_lancto + " option:selected").text();
    var geraTitulo = $(this.comboGeraTitulo_lancto).val();
    var titulo = false;
    var parcelas = eval($(this.textParcelas_lancto).val());
    var terceiro = $(this.comboTerceiro_lancto).val();
    var descTerceiro = $(this.comboTerceiro_lancto + " option:selected").text();
    var historico = $(this.textHistorico_lancto).val().replace(/\'/g, ""); //Substitui todos os apóstrofos (') por vazio ().
    var modelo = $(this.comboModelo).val();
    var numeroDocumento = $(this.textNumeroDocumento_lancto).val();
    var totalParcelasAnterior = 0;
    var somaParcelasAnterior = 0;
    var tipoConta = "";
    var codConsultor = $(this.comboConsultor_lancto).val();
    var descConsultor = $(this.comboConsultor_lancto + " option:selected").text();

    if ($(this.textHistorico_lancto).val().replace(/\'/g, "") != "" && ~$(this.textHistorico_lancto).val().indexOf("'")) //Se conter apóstrofo (') exibirá a mensagem.
        alert("O apóstrofo (') do histórico será removido.");

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/getSessionLanctos",
        data: "{modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Pegando valor anterior de parcelas..."),
        success: function (r) {
            var result = r.d;
            if (geraTitulo == "S") {
                if (result != null && result != undefined) {
                    if (result.length > 0) {
                        var atual = null;

                        for (var i = 0; i < result.length; i++) {
                            if (result[i].seqLote == seqLote)
                                atual = result[i];
                        }
                        if (atual != null) {
                            if (atual.vencimentos != null) {
                                var nulas = 0;
                                for (var i = 0; i < atual.vencimentos.length; i++) {
                                    if (atual.vencimentos[i].data == null && atual.vencimentos[i].valor == null) {
                                        nulas = nulas + 1;
                                    } else {
                                        somaParcelasAnterior = somaParcelasAnterior + atual.vencimentos[i].valor;
                                    }
                                }
                                somaParcelasAnterior = Math.round(somaParcelasAnterior * 100) / 100;
                                if (nulas > 0)
                                    erros.push("Por favor, preencha as parcelas vindas do modelo.");

                                if (atual != null)
                                    totalParcelasAnterior = atual.vencimentos.length
                            }
                        }
                    }
                }
            }
        },
        complete: function () {
            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/getContaContabil",
                data: "{codigo:'" + conta + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: OnBeforeSend("Verificando tipo de conta contábil..."),
                success: function (r) {
                    var result = r.d;
                    if (result != undefined && result != "")
                        tipoConta = result.tipo;
                },
                complete: function () {

                    if (seqLote == undefined || seqLote == "") {
                        erros.push("Sequência do lançamento inválida");
                    }

                    if (data == "") {
                        erros.push("Data do lançamento em branco.");
                    }

                    if (debCred == "0") {
                        erros.push("Informe se o lançamento é Débito ou Crédito.");
                    }

                    if (conta == "0") {
                        erros.push("Informe a Conta Contábil do Lançamento");
                    }
                    else {
                        if (seqLote == 1) {
                            if ((tela.modulo == "CAP_INCLUSAO_TITULO" && tipoConta != "CP") || (tela.modulo == "CAR_INCLUSAO_TITULO" && tipoConta != "CR")) {
                                erros.push("Tipo de conta inválido.");
                            }
                        }
                    }

                    if (job == undefined || job == "0") {
                        erros.push("Informe o Job do Lançamento");
                    }

                    if (!isNaN(valor)) {
                        if (valor == 0 || valor == undefined)
                            erros.push("Informe o Valor do Lançamento");
                    }
                    else {
                        erros.push("Informe o valor do Lançamento");
                    }

                    if (linhaNegocio == "0") {
                        erros.push("Informe a Linha de Negócio do Lançamento");
                    }

                    if (divisao == "0") {
                        erros.push("Informe a Divisão do Lançamento");
                    }

                    if (cliente == "0") {
                        erros.push("Informe o Cliente do Lançamento");
                    }

                    if (geraTitulo == "") {
                        erros.push("Informe se o lançamento Gera Título ou não.");
                    }
                    else {
                        if (geraTitulo == "S") {
                            titulo = true;
                            if (!isNaN(parcelas)) {
                                if (parcelas <= 0) {
                                    erros.push("O número de parcelas deve ser maior que Zero.");
                                }
                                else {
                                    if (totalParcelasAnterior != parcelas) {
                                        erros.push("Total de parcelas informado é diferente do salvo anteriormente.");
                                    }

                                    if (somaParcelasAnterior != valor)
                                        erros.push("Soma das parcelas não bate com o Valor Total do lançamento.");

                                }
                            }
                            else {
                                erros.push("Informe a quantidade de parcelas.");
                            }

                            if (terceiro == "0") {
                                erros.push("Escolha um Terceiro para Gerar Título.");
                            }
                        }
                        else {
                            if (seqLote == 1) {
                                if (tela.modulo == "CAP_INCLUSAO_TITULO" || tela.modulo == "CAR_INCLUSAO_TITULO") {
                                    erros.push("O primeiro lançamento obrigatóriamente necessita Gerar Título.");
                                } else {
                                    if (tipoConta == "CR" || tipoConta == "CP" || tipoConta == "IR") {
                                        if (tipoConta == "CR")
                                            erros.push("Lançamentos com conta do tipo Contas à Receber obrigatóriamente necessita Gerar Título.");
                                        else if (tipoConta == "CP")
                                            erros.push("Lançamentos com conta do tipo Contas à Pagar obrigatóriamente necessita Gerar Título.");
                                        else if (tipoConta == "IR")
                                            erros.push("Lançamentos com conta do tipo Impostos à Recolher obrigatóriamente necessita Gerar Título.");
                                    }
                                }
                            }
                            else {
                                if (tipoConta == "CR" || tipoConta == "CP" || tipoConta == "IR") {
                                    if (tipoConta == "CR")
                                        erros.push("Lançamentos com conta do tipo Contas à Receber obrigatóriamente necessita Gerar Título.");
                                    else if (tipoConta == "CP")
                                        erros.push("Lançamentos com conta do tipo Contas à Pagar obrigatóriamente necessita Gerar Título.");
                                    else if (tipoConta == "IR")
                                        erros.push("Lançamentos com conta do tipo Impostos à Recolher obrigatóriamente necessita Gerar Título.");
                                }
                                else {
                                    titulo = false;
                                    parcelas = 0;
                                    terceiro = 0;
                                    descTerceiro = "";
                                }
                            }
                        }
                    }

                    if (historico == "")
                        erros.push("Preencha o histórico do lançamento.");

                    if (numeroDocumento == "")
                        erros.push("Informe o número de documento");

					if (erros.length == 0) {
						if (modelo == null) {
							modelo = 0;
						}

						if (codConsultor == null) {
							codConsultor = 0;
						}

                        var parametros = "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + ",data: '" + data + "', debCred: '" + debCred + "', conta: '" + conta + "', descConta: '" + descConta + "', ";
                        parametros += "job: " + job + ", descJob: '" + descJob + "', qtd: " + qtd + ", valorUnit: " + valorUnit + ", valorBruto: " + valorBruto + ", valor: " + valor + ", linhaNegocio: " + linhaNegocio + ", descLinhaNegocio: '" + descLinhaNegocio + "', ";
                        parametros += "divisao: " + divisao + ", descDivisao: '" + descDivisao + "', cliente: " + cliente + ", descCliente: '" + descCliente + "', geraTitulo: " + titulo + ", ";
                        parametros += "parcelas: " + parcelas + ", terceiro: " + terceiro + ", descTerceiro: '" + descTerceiro + "', historico: '" + historico + "', modelo: " + modelo + ", numeroDocumento:'" + numeroDocumento + "', descConsultor:'" + descConsultor + "', codConsultor:" + codConsultor + ", valorGrupo:" + valorGrupo + ", codMoeda:" + codMoeda + "}"

                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulos.aspx/salvaLancto",
                            data: parametros,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: OnBeforeSend("Salvando lançamento..."),
                            success: function (r) {
                                var result = r.d;
                                $("#boxLoading").hide();
                                if (tela.modulo != "C_INCLUSAO_LANCTO") {
                                    if (seqLote == 1) {
                                        $(tela.textNumeroDocumento).val(numeroDocumento);
                                        $(tela.textQtd).val(number_format(qtd, 6, ',', '.'));
                                        $(tela.textValorUnit).val(number_format(valorUnit, 6, ',', '.'));
                                        $(tela.textValorBruto).val(number_format(valorBruto, 2, ',', '.'));
                                        $(tela.textValorTotal).val(number_format(valor, 2, ',', '.'));
                                        $(tela.textParcelas).val(parcelas);
                                        $(tela.comboFornecedor).chosen().val(terceiro);
                                        $(tela.comboConsultor).val(codConsultor);
                                    }
                                }
                                tela.montaGridLanctos(result);
                                tela.calculaTotais(result);
                                tela.limpaCampos();
                                $(tela.botaoSalvar_lancto).val("Incluir");
                                tela.remanejaSequencia(seqLote);
                                $(tela.botaoSalvar).removeAttr("disabled");
                                tela.ativaFormTitulo();
                            },
                            complete: function () {
                                carregaContas($(tela.hdSeqLote_lancto).val());
                            },
                            error: OnFailed
                        });
                    }
                    else {
                        $("#boxLoading").hide();
                        var texto = "";
                        for (var i = 0; i < erros.length; i++) {
                            texto += erros[i] + "\n";
                        }

                        alert(texto);
                    }
                }
            });
        }
    });
}

function alteraLancto(seqLote, linha) {
    //var check = $('.checkJobAtivo input:checkbox');
	//check.prop('checked', false);

	//--Gabriel
   // tela.atualizaJobs(check);

    //cancela();
    var altera = true;
    var seqLoteAtual = $(tela.prefixo + "H_SEQLOTE_LANCTO").val();
    var contaLancamento = "";
    var divisaoCod = 0;
    var totalVencimentos = 0;
    var terceiro = 0;

    tela.desativaGridLanctos();
    $(tela.botaoSalvar).attr("disabled", "disabled");
    //tela.desativaFormTitulo();
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/pegaLancto",
        data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLoteAtual + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Verificando se lançamento está nulo..."),
        success: function (l) {
            var lancto = l.d;
            if (lancto != null || lancto != undefined) {
                if (lancto.terceiro == null && lancto.vencimentos.length > 0) {
                    altera = false;
                    $("#boxLoading").hide();
                }
            }
        },
        complete: function () {
            if (altera) {
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/pegaLancto",
                    data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: OnBeforeSend("Carregando formulário..."),
                    success: function (r) {
                        var result = r.d;
                        $("#boxLoading").hide();
                        if (result != null || result != undefined) {
                            $("#gridLancamentos tr[class='linhaSelecionada']").attr("class", "linha");
                            $(linha).attr("class", "linhaSelecionada");

                            $(tela.hdSeqLote_lancto).val(seqLote);
                            tela.ativaFormLancamento();
                            $(tela.textNumeroDocumento_lancto).val(result.numeroDocumento);
                            $(tela.comboDebCred_lancto).val(result.debCred);
                            contaLancamento = result.conta;
							divisaoCod = result.divisao;
                            //$(tela.comboJob_lancto).val(result.job).trigger("chosen:updated");
                            $(tela.comboConsultor_lancto).val(result.codConsultor);
                            $(tela.textQtd_lancto).val(number_format(result.qtd, 6, ',', '.'));
                            $(tela.textValorUnit_lancto).val(number_format(result.valorUnit, 6, ',', '.'));
                            $(tela.textValor_lancto).val(number_format(result.valor, 2, ',', '.'));
                            $(tela.textValorBruto).val(number_format(result.valorBruto, 2, ',', '.'));
                            $(tela.textGrupo).val(result.valorGrupo);
                            $(tela.botaoSalvar_lancto).val("Alterar");

                            var qtdItemsLinhaNegocio = $(tela.prefixo + "comboLinhaNegocio_lancto > option").length;
                            var qtdItemsDivisao = $(tela.prefixo + "comboDivisao_lancto > option").length;
                            var qtdItemsCliente = $(tela.prefixo + "comboCliente_lancto > option").length;

							tela.carregaJob(result.job);

                            $(tela.comboLinhaNegocio_lancto).val(result.linhaNegocio);
                            $(tela.comboDivisao_lancto).val(result.divisao);
                            $(tela.comboCliente_lancto).val(result.cliente);

                            if (!(qtdItemsLinhaNegocio <= 1 && qtdItemsDivisao <= 1 && qtdItemsCliente <= 1)) {
                                $(tela.comboLinhaNegocio_lancto).removeAttr("disabled");
                                $(tela.comboDivisao_lancto).removeAttr("disabled");
                                $(tela.comboCliente_lancto).removeAttr("disabled");
                            }

                            if (result.titulo == true)
                                $(tela.comboGeraTitulo_lancto).val("S");
                            else if (result.titulo == false)
                                $(tela.comboGeraTitulo_lancto).val("N");
                            else
                                $(tela.comboGeraTitulo_lancto).val("0");

                            totalVencimentos = result.vencimentos.length;
                            terceiro = result.terceiro;
                            $(tela.textHistorico_lancto).val(result.historico);
                            $(tela.comboDivisao_lancto).val(result.divisao);

                            if (tela.statusTravaJob != false) {
                                $(tela.comboLinhaNegocio_lancto).attr("disabled", "disabled");
                                $(tela.comboDivisao_lancto).attr("disabled", "disabled");
                                $(tela.comboCliente_lancto).attr("disabled", "disabled");
                            }

                            if (tela.exigeModelo) {
                                $(tela.comboConta_lancto).attr("disabled", "disabled").trigger("chosen:updated");
                            }
                        }
                    },
                    complete: function () {
                        carregaContas($(tela.hdSeqLote_lancto).val());
                        $(tela.comboConta_lancto).val(contaLancamento).trigger("chosen:updated");
                        $(tela.comboDivisao_lancto).val(divisaoCod);
                        //tela.buscaTerceirosDisponiveis();
                        exibeVencimentosTitulo($(tela.comboGeraTitulo_lancto).val());
                        $(tela.textParcelas_lancto).val(totalVencimentos);
                        $(tela.comboTerceiro_lancto).val(terceiro).trigger("chosen:updated");
                    },
                    error: OnFailed
                });
            }
            else {
                alert("Salve o lançamento atual antes de alterar outro.");
            }
        },
        error: OnFailed
    });
}

function deletaLancto(seqLote) {
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/validaDeletaLancto",
        data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Verificando lançamento para exclusão..."),
        success: function (r) {
            var result = r.d;
            $("#boxLoading").hide();

            if (result != "") {
                alert(result);
            } else {
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/deletaLancto",
                    data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: OnBeforeSend("Removendo lançamento..."),
                    success: function (r) {
                        var result = r.d;
                        $("#boxLoading").hide();
                        if (result != null || result != undefined) {
                            tela.montaGridLanctos(result);
                            tela.calculaTotais(result);
                            if (result.length > 0) {
                                $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                                $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);
                                var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                                newSeqLote++;
                                $(tela.hdSeqLote_lancto).val(newSeqLote);
                            } else {
                                $(tela.hdSeqLoteMin_lancto).val("1");
                                $(tela.hdSeqLoteMax_lancto).val("1");
                                $(tela.hdSeqLote_lancto).val("1");
                            }
                        }
                    },
                    error: OnFailed
                });
            }
        },
        error: OnFailed
    });
}

function pegaValorOldLancto1(input) {
    $(tela.prefixo + "textValorUnit_old").val($(input).val());
}

function alteraValorLancto1(input) {
    if ($(input).val() == "")
        $(input).val("0");

    var valorUnit_old = limpaNumero($(tela.prefixo + "textValorUnit_old").val());
    var valorUnit = limpaNumero($(input).val());
    var errosAlteracao = null;

    if (valorUnit != valorUnit_old) {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/alteraValorLancto1",
            data: "{modulo:'" + tela.modulo + "' ,valor: " + valorUnit + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Alterando valor do lançamento..."),
            success: function (result) {
                errosAlteracao = result.d;
            },
            complete: function () {
                if (errosAlteracao.length == 0) {
                    $.ajax({
                        type: "POST",
                        url: "FormGenericTitulos.aspx/getSessionLanctos",
                        data: "{modulo:'" + tela.modulo + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: OnBeforeSend("Montando grid de lançamentos..."),
                        success: function (result) {
                            tela.montaGridLanctos(result.d);
                        },
                        error: OnFailed,
                        complete: function () {
                            $("#boxLoading").hide();
                            calculaValorTotal();
                            tela.loadVencimentos(1, tela.prefixo_s + "textParcelas", tela.prefixo_s + "textValorTotal",
                                tela.prefixo_s + "botaoVencimento", tela.prefixo_s + "H_SEQLOTE_LANCTO");
                        }
                    });

                }
                else {
                    var texto = "";
                    for (var i = 0; i < errosAlteracao.length; i++) {
                        texto += errosAlteracao[i] + "\n";
                    }
                    alert(texto);
                }
            },
            error: OnFailed
        });
    }
}

function pegaQtdOldLancto1(input) {
    $(tela.prefixo + "textQtd_old").val($(input).val());
}

function pegaNumeroDocumentoOldLancto1(input) {
    $(tela.prefixo + "textNumeroDocumento_old").val($(input).val());
}

function alteraValorBruto() {
    var valBru = parseFloat($(tela.prefixo + "textValorBruto").val().replace('.', '').replace(',', '.')).toFixed(2);
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/alteraValorBruto",
        data: "{modulo:'" + tela.modulo + "' , valorbruto: " + valBru + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Alterando o valor Bruto.."),
        success: function (result) {
            errosAlteracao = result.d;
        },
        complete: function () {
            $("#boxLoading").hide();
        },
        error: OnFailed
    });
}

function alteraEtapaMoeda() {
    var val = 0;

    if (tela.modulo == "C_INCLUSAO_LANCTO") {
        val = parseInt($(tela.prefixo + "comboEtapaMoedaInclusaLancto").val());
    } else {
        val = parseInt($(tela.prefixo + "comboEtapaMoeda").val());
    }
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/alteraEtapaMoeda",
        data: "{modulo:'" + tela.modulo + "' , codmoeda: " + val + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Alterando Moeda.."),
        success: function (result) {
            errosAlteracao = result.d;
        },
        complete: function () {
            $("#boxLoading").hide();
        },
        error: OnFailed
    });
    
    if (!confirm('Deseja alterar o tipo de moeda?')) {
        val = 0

        if (tela.modulo == "C_INCLUSAO_LANCTO") {
            parseInt($(tela.prefixo + "comboEtapaMoedaInclusaLancto").val(val));
        } else {
            parseInt($(tela.prefixo + "comboEtapaMoeda").val(val));
        }

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/alteraEtapaMoeda",
            data: "{modulo:'" + tela.modulo + "' , codmoeda: " + val + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Alterando Moeda.."),
            success: function (result) {
                errosAlteracao = result.d;
            },
            complete: function () {
                $("#boxLoading").hide();
            },
            error: OnFailed
        });
    }
}

function alteraNumeroDocumentoLancto1(input) {
    if ($(input).val() == "")
        $(input).val("0");
    var numeroDocumentoOld = $(tela.prefixo + "textNumeroDocumento_old").val();
    var numeroDocumento = $(input).val();
    var errosAlteracao = null;

    if (numeroDocumento != numeroDocumentoOld) {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/alteraNumeroDocumentoLancto1",
            data: "{modulo:'" + tela.modulo + "' ,numeroDocumento: '" + numeroDocumento + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Alterando numero documento do primeiro lançamento..."),
            success: function (result) {
                errosAlteracao = result.d;
            },
            complete: function () {
                if (errosAlteracao != null) {
                    if (errosAlteracao.length == 0) {
                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulos.aspx/getSessionLanctos",
                            data: "{modulo:'" + tela.modulo + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: OnBeforeSend("Montando grid de lançamentos..."),
                            success: function (result) {
                                tela.montaGridLanctos(result.d);
                            },
                            error: OnFailed,
                            complete: function () {
                                $("#boxLoading").hide();
                                calculaValorTotal();
                                //tela.loadVencimentos(1, tela.prefixo_s+ "textParcelas",tela.prefixo_s+ "textValorTotal", 
                                //    tela.prefixo_s+ "botaoVencimento", tela.prefixo_s+ "H_SEQLOTE_LANCTO");
                            }
                        });

                    }
                    else {
                        var texto = "";
                        for (var i = 0; i < errosAlteracao.length; i++) {
                            texto += errosAlteracao[i] + "\n";
                        }
                        alert(texto);
                    }
                } else {
                    alert("Erro fatal!");
                }
            },
            error: OnFailed
        });
    }
}

function alteraQtdLancto1(input) {
    if ($(input).val() == "")
        $(input).val("0");
    var qtd_old = limpaNumero($(tela.prefixo + "textQtd_old").val());
    var qtd = limpaNumero($(input).val());
    var errosAlteracao = null;

    if (qtd != qtd_old) {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/alteraQtdLancto1",
            data: "{modulo:'" + tela.modulo + "' ,qtd: " + qtd + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Alterando quantidade do primeiro lançamento..."),
            success: function (result) {
                errosAlteracao = result.d;
            },
            complete: function () {
                if (errosAlteracao.length == 0) {
                    $.ajax({
                        type: "POST",
                        url: "FormGenericTitulos.aspx/getSessionLanctos",
                        data: "{modulo:'" + tela.modulo + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: OnBeforeSend("Montando grid de lançamentos..."),
                        success: function (result) {
                            tela.montaGridLanctos(result.d);
                        },
                        error: OnFailed,
                        complete: function () {
                            $("#boxLoading").hide();
                            calculaValorTotal();
                            //tela.loadVencimentos(1, tela.prefixo_s+ "textParcelas",tela.prefixo_s+ "textValorTotal", 
                            //    tela.prefixo_s+ "botaoVencimento", tela.prefixo_s+ "H_SEQLOTE_LANCTO");
                        }
                    });

                }
                else {
                    var texto = "";
                    for (var i = 0; i < errosAlteracao.length; i++) {
                        texto += errosAlteracao[i] + "\n";
                    }
                    alert(texto);
                }
            },
            error: OnFailed
        });
    }
}

function carregaContas(seqLote) {
    if ($(tela.comboConta_lancto)[0].length > 1 && seqLote != 1 && !flagContas) {
        $(tela.comboConta_lancto).trigger("chosen:updated");
        return;
    }

    if (seqLote == 1) {
        var parametros = "";
        if (tela.modulo == "CAP_INCLUSAO_TITULO") {
            parametros = "{tipo: 'CP'}";
        }
        else if (tela.modulo == "CAR_INCLUSAO_TITULO") {
            parametros = "{tipo: 'CR'}";
        }
        else if (tela.modulo == "BAIXA_TITULO") {
            parametros = "{tipo: 'BC'}";
        }
        else if (tela.modulo == "MODELO_LANCAMENTO") {
            if (tela.tipo == "CR") {
                parametros = "{tipo: 'CR'}";
            }
            else if (tela.tipo == "CP") {
                parametros = "{tipo: 'CP'}";
            }
            else {
                parametros = "{tipo: ''}";
            }

        }
        else {
            parametros = "{tipo: ''}";
		}
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getListaContas",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                $(tela.comboConta_lancto).html("");
                $("<option value='0'>Carregando...</option>").appendTo(tela.comboConta_lancto);
            },
            success: function (r) {
                var result = r.d;
                if (result != null && result != undefined) {
                    if (result.length > 0) {
                        $(tela.comboConta_lancto).html("");
                        $("<option value='0'>Escolha</option>").appendTo(tela.comboConta_lancto);
                        for (var i = 0; i < result.length; i++) {
                            $("<option value='" + result[i]["COD_CONTA"] + "'>" + result[i]["DESCRICAO_COMPLETO"] + "</option>").appendTo(tela.comboConta_lancto);
                        }
                        $(tela.comboConta_lancto).trigger("chosen:updated");
                    }
                }
            }
        });
        flagContas = true;
    }
    else {
        var parametros = "";
        var url = "";

        //if (tela.modulo == "BAIXA_TITULO") {
        //    parametros = "{tipo: 'BC'}";
        //    url = "FormGenericTitulos.aspx/getListaContasDiferenteDe"
        //}
        //else {
        //    parametros = "{tipo: ''}";
        //    url = "FormGenericTitulos.aspx/getListaContas"
        //}

		parametros = "{tipo: ''}";
        url = "FormGenericTitulos.aspx/getListaContas";

        $.ajax({
            type: "POST",
            url: url,
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            beforeSend: function () {
                $(tela.comboConta_lancto).html("");
                $("<option value='0'>Carregando...</option>").appendTo(tela.comboConta_lancto);
            },
            success: function (r) {
                var result = r.d;
                if (result != null && result != undefined) {
                    if (result.length > 0) {
                        $(tela.comboConta_lancto).html("");
                        $("<option value='0'>Escolha</option>").appendTo(tela.comboConta_lancto);
                        for (var i = 0; i < result.length; i++) {
                            $("<option value='" + result[i]["COD_CONTA"] + "'>" + result[i]["DESCRICAO_COMPLETO"] + "</option>").appendTo(tela.comboConta_lancto);
                        }
                    }
                }
                $(tela.comboConta_lancto).trigger("chosen:updated");
            },
            error: OnFailed
        });

        flagContas = false;
    }
}

function exigeModelo() {
    var status = false;
    $.ajax({
        type: "POST",
        data: "{}",
        url: "FormGenericTitulos.aspx/exigeModelo",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (r) {
            status = r.d == true;
        }
    });
    return status;
}

function travaJob() {
    var status = false;
    $.ajax({
        type: "POST",
        data: "{}",
        url: "FormGenericTitulos.aspx/travaJob",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (r) {
            status = r.d == "true";
        }
    });
    return status;
}

function carregaJob(job) {
    //if (job != 0) {
    //    var check = $('.checkJobAtivo input:checkbox');
    //    check.prop('checked', false);
    //    tela.atualizaJobs(check);
    //}

	if (job > 0) {
        var linhaNegocio = 0;
        var divisao = 0;
		var cliente = 0;
		if ($(this.comboLinhaNegocio_lancto)[0].length <= 1) {
			$(this.comboLinhaNegocio_lancto).html("");
			$("<option value='0'>Carregando...</option>").appendTo(this.comboLinhaNegocio_lancto);
		}

		if ($(this.comboDivisao_lancto)[0].length <= 1) {
			$(this.comboDivisao_lancto).html("");
			$("<option value='0'>Carregando...</option > ").appendTo(this.comboDivisao_lancto);
		}

		if ($(this.comboCliente_lancto)[0].length <= 1) {
			$(this.comboCliente_lancto).html("");
			$("<option value='0'>Carregando...</option>").appendTo(this.comboCliente_lancto);
		}

        //$(this.comboLinhaNegocio_lancto).html("");
        //$(this.comboDivisao_lancto).html("");
        //$(this.comboCliente_lancto).html("");

        //$("<option value='0'>Carregando...</option>").appendTo(this.comboLinhaNegocio_lancto);
        //$("<option value='0'>Carregando...</option>").appendTo(this.comboDivisao_lancto);
        //$("<option value='0'>Carregando...</option>").appendTo(this.comboCliente_lancto);

        $(this.botaoSalvar_lancto).attr("disabled", "disabled");
        $(this.botaoCancelar_lancto).attr("disabled", "disabled");

        $.ajax({
            type: "POST",
            async: false,
            url: "FormGenericTitulos.aspx/loadJob",
            data: "{job: " + job + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var result = r.d;
                linhaNegocio = eval(result[1]);
                divisao = eval(result[3]);
				cliente = eval(result[5]);
				status = result[7];
				nome = result[8];
				
				if ($(tela.comboJob_lancto).val() === null || $(tela.comboJob_lancto).val() === '0') {
					if (nome !== null && document.querySelector(tela.comboJob_lancto + ' option[value="' + job + '"]') === null) {
						$('<option value='+job + '>' + nome + '</option>').appendTo(tela.comboJob_lancto);	
					}

					$(tela.comboJob_lancto).val(job).trigger("chosen:updated");
				}
			},
			complete: function () {

				if ($(tela.comboLinhaNegocio_lancto)[0].length <= 1) {

					$.ajax({
						type: "POST",
						url: "FormGenericTitulos.aspx/getListaLinhasNegocio",
						data: "{}",
						async: false,
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (r) {
							var result = r.d;
							if (result.length > 0) {
								$(tela.comboLinhaNegocio_lancto).html("");
								$("<option value='0'>Escolha</option>").appendTo(tela.comboLinhaNegocio_lancto);
								for (var i = 0; i < result.length; i++) {
									if (eval(result[i]["COD_LINHA_NEGOCIO"]) == linhaNegocio) {
										$("<option value='" + result[i]["COD_LINHA_NEGOCIO"] + "' selected='selected'>" + result[i]["DESCRICAO"] + "</option>").appendTo(tela.comboLinhaNegocio_lancto);
									}
									else {
										$("<option value='" + result[i]["COD_LINHA_NEGOCIO"] + "'>" + result[i]["DESCRICAO"] + "</option>").appendTo(tela.comboLinhaNegocio_lancto);
									}
								}

								$(tela.comboLinhaNegocio_lancto).removeAttr("disabled");

							}
							else {
								$(tela.comboLinhaNegocio_lancto).html("");
								$("<option value='0'>Escolha</option>").appendTo(tela.comboLinhaNegocio_lancto);
								$(tela.comboLinhaNegocio_lancto).attr("disabled", "disabled");
							}
						},
						error: OnFailed
					});
				}
				if ($(tela.comboDivisao_lancto)[0].length <= 1) {

					$.ajax({
						type: "POST",
						url: "FormGenericTitulos.aspx/getListaDivisoes",
						async: false,
						data: "{}",
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (r) {
							var result = r.d;
							if (result.length > 0) {
								$(tela.comboDivisao_lancto).html("");
								$("<option value='0'>Escolha</option>").appendTo(tela.comboDivisao_lancto);

								for (var i = 0; i < result.length; i++) {
									if (eval(result[i]["COD_DIVISAO"]) == divisao) {
										$("<option value='" + result[i]["COD_DIVISAO"] + "' selected='selected'>" + result[i]["DESCRICAO"] + "</option>").appendTo(tela.comboDivisao_lancto);
									}
									else {
										$("<option value='" + result[i]["COD_DIVISAO"] + "'>" + result[i]["DESCRICAO"] + "</option>").appendTo(tela.comboDivisao_lancto);
									}
								}

								$(tela.comboDivisao_lancto).removeAttr("disabled");

							}
							else {
								$(tela.comboDivisao_lancto).html("");
								$("<option value='0'>Escolha</option>").appendTo(tela.comboDivisao_lancto);
								$(tela.comboDivisao_lancto).attr("disabled", "disabled");
							}
						},
						error: OnFailed
					});
				}

				if ($(tela.comboCliente_lancto)[0].length <= 1) {

					var jobtemp = $(tela.comboJob_lancto).val();
					if (jobtemp == null || jobtemp == undefined) {
						jobtemp = 0;
					}
					$.ajax({
						type: "POST",
						async: false,
						url: "FormGenericTitulos.aspx/getListaClientes",
						data: "{'travaJob':" + tela.statusTravaJob + ",'codigoJob':" + jobtemp + "}",
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (r) {
							var result = r.d;

							$(tela.comboCliente_lancto).html("");
							$("<option value='0'>Escolha</option>").appendTo(tela.comboCliente_lancto);

							if (tela.statusTravaJob == true && result.length > 0) {
								for (var i = 0; i < result.length; i++) {
									$("<option value='" + result[i]["COD_EMPRESA"] + "' selected='selected'>" + result[i]["NOME_RAZAO_SOCIAL"] + "</option>").appendTo(tela.comboCliente_lancto);
								}
							}
							else if (result.length > 0) {
								$(tela.comboCliente_lancto).html("");
								$("<option value='0'>Escolha</option>").appendTo(tela.comboCliente_lancto);

								for (var i = 0; i < result.length; i++) {
									if (eval(result[i]["COD_EMPRESA"]) == cliente) {
										$("<option value='" + result[i]["COD_EMPRESA"] + "' selected='selected'>" + result[i]["NOME_RAZAO_SOCIAL"] + "</option>").appendTo(tela.comboCliente_lancto);
									}
									else {
										$("<option value='" + result[i]["COD_EMPRESA"] + "'>" + result[i]["NOME_RAZAO_SOCIAL"] + "</option>").appendTo(tela.comboCliente_lancto);
									}
								}
							}
							else {

								$(tela.comboCliente_lancto).attr("disabled", "disabled");
							}
							$(tela.comboCliente_lancto).removeAttr("disabled").trigger("chosen:updated");
						},
						error: OnFailed
					});

				}

				$(tela.botaoSalvar_lancto).removeAttr("disabled");
				$(tela.botaoCancelar_lancto).removeAttr("disabled");

				$(tela.comboLinhaNegocio_lancto).val(linhaNegocio);
				$(tela.comboLinhaNegocio_lancto).trigger("chosen:updated");

				$(tela.comboDivisao_lancto).val(divisao);
				$(tela.comboDivisao_lancto).trigger("chosen:updated");

				$(tela.comboCliente_lancto).val(cliente);
				$(tela.comboCliente_lancto).trigger("chosen:updated");

			}
               
        });
    } else {
        $(tela.comboLinhaNegocio_lancto).html("");
        $(tela.comboDivisao_lancto).html("");
        $(tela.comboCliente_lancto).html("");

        $("<option value='0'>Escolha</option>").appendTo(tela.comboLinhaNegocio_lancto);
        $("<option value='0'>Escolha</option>").appendTo(tela.comboDivisao_lancto);
        $("<option value='0'>Escolha</option>").appendTo(tela.comboCliente_lancto);

        $(tela.comboLinhaNegocio_lancto).attr("disabled", "disabled");
        $(tela.comboDivisao_lancto).attr("disabled", "disabled");
        $(tela.comboCliente_lancto).attr("disabled", "disabled");
    }

    if (tela.statusTravaJob) {
        $(tela.comboLinhaNegocio_lancto).attr("disabled", "disabled");
        $(tela.comboDivisao_lancto).attr("disabled", "disabled");
        $(tela.comboCliente_lancto).attr("disabled", "disabled").trigger("chosen:updated");
    }

    if (tela.exigeModelo) {
        $(tela.comboConta_lancto).attr("disabled", "disabled").trigger("chosen:updated");
    }
}

function desativaGridLanctos() {
    var celulas = $("#gridLancamentos tr td");
    for (var i = 0; i < celulas.length; i++) {
        $(celulas[i]).attr("onClick", "");
        if ($(celulas[i]).attr("class") == "deletar") {
            $(celulas[i]).find("a").attr("href", "javascript:void(0);");
        }
    }
}

function montaGridLanctos(lista) {
    $("#gridLancamentos tr[class='linha']").remove();
    $("#gridLancamentos tr[class='linhaMarcada']").remove();
    $("#gridLancamentos tr[class='linhaSelecionada']").remove();

    for (var i = 0; i < lista.length; i++) {
        var totalBaixas = 0;
        $.ajax({
            type: "POST",
            async: false,
            url: "FormGenericTitulos.aspx/getTotalBaixas",
            data: "{lote:" + lista[i].lote + ",seqLote:" + lista[i].seqLote + ",modulo:'" + tela.modulo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                totalBaixas = result.d;
            },
            complete: function () {
                var linha = "<tr class='linha' onmouseover=\"mudaCor(this);\" onmouseout=\"voltaCor(this);\">";
                linha += "<td id='linhaSeqLote' style=\"display:none;\">";
                linha += lista[i].seqLote;
                linha += "</td>";
                linha += "<td>" + lista[i].valorGrupo + "</td>";
                linha += "<td class='debCred' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += lista[i].debCred;
                linha += "</td>";
                linha += "<td class='conta' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += lista[i].descConta;
                linha += "</td>";
                linha += "<td class='job' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += lista[i].descJob;
                linha += "</td>";
                linha += "<td class='linhaNegocio'  style=\"display:none;\">";
                linha += lista[i].descLinhaNegocio;
                linha += "</td>";
                linha += "<td class='divisao' style=\"display:none;\">";
                linha += lista[i].descDivisao;
                linha += "</td>";
                linha += "<td class='valor' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += number_format(lista[i].valor, 2, ',', '.');
                linha += "</td>";
                linha += "<td class='historico' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += lista[i].historico;
                linha += "</td>";
                linha += "<td class='titulo' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                if (lista[i].titulo)
                    linha += "<img src='Imagens/icones/tick.gif' alt='' />";
                else
                    linha += " - "
                linha += "</td>";
                linha += "<td class='historico' onClick=\"tela.alteraLancto(" + lista[i].seqLote + ", this.parentNode);\">";
                linha += lista[i].descTerceiro;
                linha += "</td>";
                linha += "<td class='cotacao'>";
                linha += "<a onclick='loadCotacao(2," + lista[i].seqLote + ");' href='#'><img src='Imagens/dindin.png' width='24' height='18'></a>";
                linha += "</td>";
                if (totalBaixas > 0 && tela.buscaLoteAnterior == false) {
                    linha += "</td>";
                    linha += "<td class='historico'>";
                    linha += "<a href=\"javascript:abreBaixas(" + lista[i].lote + "," + lista[i].seqLote + ",'" + tela.modulo + "');\">Baixas</a>"
                    linha += "</td>";
                } else {
                    linha += "</td>";
                    linha += "<td class='historico'>";
                    linha += ""
                    linha += "</td>";
                }
                linha += "<td class='deletar'>";

                if (lista[i].seqLote != 1)
                    linha += "<a href=\"javascript:tela.deletaLancto(" + lista[i].seqLote + ");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";
                else
                    linha += "";

                linha += "</td>";
                linha += "<td class='parcelas' style=\"display:none;\">";
                linha += "</td>";
                linha += "<td class='terceiro'  style=\"display:none;\">";
                linha += lista[i].descTerceiro;
                linha += "</td>";
                linha += "</tr>";
                $("#gridLancamentos").append(linha);
            },
            error: OnFailed
        });
    }
}

function abreBaixas(lote, seqLote, modulo) {
    var html = "";
    $.ajax({
        type: "POST",
        async: false,
        url: "FormGenericTitulos.aspx/getBaixas",
        data: "{lote:" + lote + ",seqLote:" + seqLote + ",modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            html = result.d;
            $find('ctl00_areaConteudo_popBaixas').show();
            $("#dadosBaixa").html("carregando...");
        },
        complete: function () {
            $("#dadosBaixa").html(html);

        },
        error: OnFailed
    });

}

function loadVencimentos(seqLote, textParcelas, textValorTotal, botaoVencimento_server, hSeqLote_lancto) {
    var parcelas = parseInt($("#" + textParcelas).val());
    var valorTotal = limpaNumero($("#" + textValorTotal).val());
    var tmpSeqLote = 0;

    if (parcelas > 0) {
        if (seqLote == 1) {
            tmpSeqLote = 1;
        }
        else if (seqLote < 0) {
            tmpSeqLote = eval($("#" + hSeqLote_lancto).val());
        }

        $("#" + botaoVencimento_server).click();

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/loadVencimentos",
            data: "{modulo:'" + tela.modulo + "',origem:" + seqLote + ", seqLote: " + tmpSeqLote + ",parcelas: " + parcelas + ",valorTotal: " + valorTotal + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#formVencimento").html("carregando...");
            },
            success: function (r) {
                var result = r.d;
                $("#formVencimento").html(result);
                var qtdInputs = $("#formVencimento input[type='text']").length;

                for (var i = 1; i <= (qtdInputs / 2); i++) {

                    var idData = "vencimento_" + i + "_dta";
                    var idValor = "vencimento_" + i + "_vlr";

                    if ($("#" + idData) != null && $("#" + idData) != undefined) {
                        $("#" + idData).mask("99/99/9999");
                    }

                    if ($("#" + idValor) != null && $("#" + idValor) != undefined) {
                        //$("#"+idValor).priceFormat({prefix:'', centsSeparator:',', thousandsSeparator:'.'});
                        $("#" + idValor).keypress(function () {
                            return Onlynumbers(event);
                        });
                    }
                }
            },
            error: OnFailed
        });
    }
    else {
        alert("O número de parcelas deve ser obrigatóriamente maior que Zero.");
    }
}

function loadCotacao(tipoCotacao, seq_lote) {
    //Tipo Cotacao
    // 1 - Cotação do Lote
    // 2 - Cotação do Lançamento

    var lote = (_GET('lote') == null ? 0 : _GET('lote'));
    var modulo = (_GET('modulo') == null ? "none" : _GET('modulo'));

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/loadCotacao",
        data: "{tipoCotacao:" + tipoCotacao + ",lote:" + lote + ",seq_lote:" + seq_lote + ", modulo:'" + modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Carregando cotação..."),
        success: function (r) {
            var result = r.d;
            $("#boxLoading").html("");
            $(".tblCotacao").html(result);
        },
        error: OnFailed
    });

    $(tela.botaoCotacao).click();
}

var CotacaoItem = function () {
    this.codMoeda = 0;
    this.valorMoeda = 0.00000;
    this.descrMoeda = "";
}

function salvaCotacao() {
    var tipoCotacao = $("#tipoCotacao").val();
    var seqLote = $("#seqLote").val();
    var arrCotacao = new Array;
    var modulo = (_GET('modulo') == null ? "none" : _GET('modulo'));

    $(".tblCotacao tr.linRegistro").each(function () {
        var codMoeda = parseInt($(this).closest("tr").find(".codMoeda").text());
        var valorMoeda = parseFloat(limpaNumero($(this).closest("tr").find(".txtTaxa").val())).toFixed(5);
        var descrMoeda = $(this).closest("tr").find(".descrMoeda").text();

        var cotacao = new CotacaoItem();
        cotacao.codMoeda = codMoeda;
        cotacao.valorMoeda = valorMoeda;
        cotacao.descrMoeda = descrMoeda;

        arrCotacao.push(cotacao);
    });

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/salvaCotacao",
        data: "{tipoCotacao:" + tipoCotacao + ", seq_lote:" + seqLote + ", CotacaoItem:" + JSON.stringify(arrCotacao) + ", modulo:'" + modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Salvando cotação..."),
        success: function (r) {
            var result = r.d;
            $("#boxLoading").html("");
        },
        error: OnFailed
    });
}

function salvaVencimentos(botaoSalvar_server, textValorTotal, textValor_lancto) {
    var qtdInputs = $("#formVencimento input[type='text']").length;
    var seqLote = eval($("#H_SEQLOTE_VENCIMENTO").val());
    var tempSeqLote = 0;
    var qtd;
    var valorUnit;
    var valorBruto = 0;
    var valorTotal = 0;

    var strDt = $(tela.prefixo + "textData").val();
    var arrDt = strDt.split("/");
    var dataTela = new Date(arrDt[2], arrDt[1], arrDt[0]);

    if (seqLote == 1) {
        qtd = limpaNumero($(tela.textQtd).val());
        valorUnit = limpaNumero($(tela.textValorUnit).val());
        valorBruto = limpaNumero($(tela.textValorBruto).val());
        valorTotal = limpaNumero($("#" + textValorTotal).val());
        tempSeqLote = seqLote;
    }
    else {
        qtd = limpaNumero($(tela.textQtd_lancto).val());
        valorUnit = limpaNumero($(tela.textValorUnit_lancto).val());
        try {
            var valorBruto = limpaNumero($(this.textValorBruto).val());
        }
        catch (err) {
            var valorBruto = 0;
        }
        //        valorBruto = limpaNumero($(tela.textValorBruto).val());
        valorTotal = limpaNumero($("#" + textValor_lancto).val());
        tempSeqLote = $(tela.prefixo + "H_SEQLOTE_LANCTO").val();
    }

    var datas = new Array();
    var errosData = new Array();
    var valores = new Array();
    var errosValores = new Array();

    for (var i = 1; i <= (qtdInputs / 2); i++) {
        var id = "vencimento_" + i + "_dta";
        var str = $("#" + id).val();
        var arr = str.split("/");
        var dt = new Date(arr[2], arr[1], arr[0]);

        if (dt < dataTela) {
            errosData.push("Data (" + str + ") é menor que a informada na folha.");
        }

        if ($("#" + id).val() == "" || $("#" + id).val() == null) {
            errosData.push("Informe a data da " + i + "ª parcela.");
            break;
        }

        datas.push($("#" + id).val());
    }

    for (var i = 1; i <= (qtdInputs / 2); i++) {
        var id = "vencimento_" + i + "_vlr";
        if ($("#" + id).val() == "" || $("#" + id).val() == null) {
            errosValores.push("Informe o valor da " + i + "ª parcela.")
            break;
        }
        valores.push($("#" + id).val());
    }

    if (errosData.length == 0 && errosValores.length == 0) {
        var tmpValor = 0;
        var jsonDatas = "";
        var jsonValores = "";

        jsonDatas = JSON.stringify(datas);
        jsonValores = JSON.stringify(valores);

        for (var i = 0; i < valores.length; i++) {
            tmpValor = tmpValor + limpaNumero(valores[i]);
        }


        if (valorTotal.toFixed(2) == tmpValor.toFixed(2)) {

            $("#" + botaoSalvar_server).click();

            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/salvaVencimentos",
                data: "{modulo:'" + tela.modulo + "', seqLote: " + tempSeqLote + ",qtd: " + qtd + ", valorUnit: " + valorUnit + ", valorBruto: " + valorBruto + ", valorTotal: " + valorTotal + ",datas: " + jsonDatas + ", valores: " + jsonValores + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    $("#formVencimento").html(result.d);
                },
                error: OnFailed
            });

        }
        else {
            alert("A soma dos valores das parcelas não bate com o Valor Total informado.");
        }
    }
    else {

        var texto = "";
        for (var i = 0; i < errosValores.length; i++) {
            texto += errosValores[i] + "\n";
        }

        for (var i = 0; i < errosData.length; i++) {
            texto += errosData[i] + "\n";
        }

        alert(texto);
    }
}

function remanejaSequencia(seqLote) {
    var menor = eval($(this.hdSeqLoteMin_lancto).val());
    var maior = eval($(this.hdSeqLoteMax_lancto).val());

    if (seqLote == maior || seqLote > maior) {
        $(this.hdSeqLoteMax_lancto).val(seqLote);
        seqLote++;
        $(this.hdSeqLote_lancto).val(seqLote);
    }
    else if (seqLote == menor || seqLote < maior) {
        $(this.hdSeqLoteMax_lancto).val(maior);
        maior++;
        $(this.hdSeqLote_lancto).val(maior);
    }
    else {
        $(this.hdSeqLoteMax_lancto).val(maior);
        $(this.hdSeqLote_lancto).val(maior);
    }
}

function cancela() {

    var maior = eval($(tela.prefixo + "H_SEQLOTE_LANCTO_MAX").val());
    var seqLote = $(tela.prefixo + "H_SEQLOTE_LANCTO").val();
    var deleta = false;

    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/pegaLancto",
        data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Verificando se lançamento está nulo..."),
        success: function (l) {
            var lancto = l.d;
            if (lancto != null || lancto != undefined) {
                if (lancto.terceiro == null && lancto.vencimentos.length > 0) {
                    deleta = true;
                }
            }
        },
        complete: function () {
            if (deleta) {
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/deletaLancto",
                    data: "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: OnBeforeSend("Cancelando lançamento..."),
                    success: function (result) {
                        $("#boxLoading").hide();
                    },
                    error: OnFailed,
                    complete: function () {
                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulos.aspx/getSessionLanctos",
                            data: "{modulo:'" + tela.modulo + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: OnBeforeSend("Montando grid de lançamentos..."),
                            success: function (r) {
                                var result = r.d;
                                $("#boxLoading").hide();
                                if (result != null || result != undefined) {
                                    maior++;
                                    $(tela.hdSeqLote_lancto).val(maior);

                                    $("#gridLancamentos tr[class='linhaSelecionada']").attr("class", "linha");
                                    $(tela.botaoSalvar_lancto).val("Incluir");

                                    tela.limpaCampos();
                                    tela.montaGridLanctos(result);
                                    $(tela.botaoSalvar).removeAttr("disabled");
                                    tela.ativaFormTitulo();

                                }
                            },
                            error: OnFailed,
                            complete: function () {
                                carregaContas($(tela.hdSeqLote_lancto).val());
                            }
                        });
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/getSessionLanctos",
                    data: "{modulo:'" + tela.modulo + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: OnBeforeSend("Montando grid de lançamentos..."),
                    success: function (r) {
                        var result = r.d;
                        $("#boxLoading").hide();
                        maior++;
                        $(tela.hdSeqLote_lancto).val(maior);

                        $("#gridLancamentos tr[class='linhaSelecionada']").attr("class", "linha");
                        $(tela.botaoSalvar_lancto).val("Incluir");

                        tela.limpaCampos();
                        tela.montaGridLanctos(result);
                        $(tela.botaoSalvar).removeAttr("disabled");
                        tela.ativaFormTitulo();
                    },
                    error: OnFailed,
                    complete: function () {
                        carregaContas($(tela.hdSeqLote_lancto).val());
                    }
                });
            }
        },
        error: OnFailed
    });

    if (tela.exigeModelo) {
        $(tela.comboConta_lancto).attr("disabled", "disabled").trigger("chosen:updated");
        $(tela.botaoSalvar_lancto).attr("disabled", "disabled");
    }
}

function calculaTotais(lista) {

    var totalDebito = 0;
    var totalCredito = 0;
    var totalDiferenca = 0;

    for (var i = 0; i < lista.length; i++) {

        if (lista[i].debCred == "D") {
            totalDebito = totalDebito + lista[i].valor;
        }
        else if (lista[i].debCred == "C") {
            totalCredito = totalCredito + lista[i].valor;
        }
    }

    totalDiferenca = totalDebito - totalCredito;

    $(this.totalDebito).html(number_format(totalDebito, 2, ',', '.'));
    $(this.totalCredito).html(number_format(totalCredito, 2, ',', '.'));
    $(this.totalDiferenca).html(number_format(totalDiferenca.toFixed(2), 2, ',', '.'));
}

function mudaCor(linha) {
    if ($(linha).attr("class") == "linha") {
        $(linha).attr("class", "linhaMarcada");
    }
}

function voltaCor(linha) {
    if ($(linha).attr("class") == "linhaMarcada") {
        $(linha).attr("class", "linha");
    }
}

function verificaContaGeraTitulo() {
    if ($(tela.comboGeraTitulo_lancto).val() == "S") {
        var contaSelecionada = $(tela.comboConta_lancto).val();
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/getContaContabil",
            data: "{codigo:'" + contaSelecionada + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: OnBeforeSend("Carregando dados da conta contábil..."),
            success: function (r) {
                var result = r.d;
                $("#boxLoading").hide();
                if (result != null && result != undefined) {
                    if (result.tipo == "CR" || result.tipo == "CP" || result.tipo == "I" || result.tipo == "IR") {
                        //tela.buscaTerceirosDisponiveis();
                        exibeVencimentosTitulo($(tela.comboGeraTitulo_lancto).val());
                    } else {
                        alert("Como a conta escolhida não é do tipo Contas à Pagar ou Contas à Receber ela não deve gerar título.");
                        $(tela.comboGeraTitulo_lancto).val("N");
                    }
                } else {
                    $("#boxLoading").html("Erro na verificação da conta.");
                }
            },
            error: OnFailed
        });
    } else {
        exibeVencimentosTitulo($(tela.comboGeraTitulo_lancto).val());
    }
    $(this.comboTerceiro_lancto + "_chosen").css("width", "250");
}

function get(paramName) {
    var sURL = window.document.URL.toString();
    if (sURL.indexOf("?") > 0) {
        var arrParams = sURL.split("?");
        var arrURLParams = arrParams[1].split("&");
        var arrParamNames = new Array(arrURLParams.length);
        var arrParamValues = new Array(arrURLParams.length);

        var i = 0;
        for (i = 0; i < arrURLParams.length; i++) {
            var sParam = arrURLParams[i].split("=");
            arrParamNames[i] = sParam[0];
            if (sParam[1] != "")
                arrParamValues[i] = unescape(sParam[1]);
            else
                arrParamValues[i] = "No Value";
        }

        for (i = 0; i < arrURLParams.length; i++) {
            if (arrParamNames[i] == paramName) {
                //alert("Parameter:" + arrParamValues[i]);
                return arrParamValues[i];
            }
        }
        return "No Parameters Found";
    }
}

function limpaCampos() {
    //  $(this.textNumeroDocumento_lancto).val("");
    $(this.comboDebCred_lancto).val("0");
    $(this.comboConta_lancto).val("0").trigger("chosen:updated");
    $(this.comboJob_lancto).val("0").trigger("chosen:updated");
    $(this.textQtd_lancto).val("1");
    $(this.comboLinhaNegocio_lancto).val("0");
    $(this.comboDivisao_lancto).val("0");
    $(this.comboCliente_lancto).val("0").trigger("chosen:updated");
    $(this.comboJob_lancto).val("0");
    $(this.textValorUnit_lancto).val("0");
    $(this.textValor_lancto).val("0");
    //  $(this.textQtd_lancto).val("0");
    //  $(this.textValorBruto).val("0");
    $(this.textGrupo).val("");
    exibeVencimentosTitulo("0");
    $(this.textParcelas_lancto).val("1");
    $(this.comboTerceiro_lancto).val("0");
    //  $(this.textHistorico_lancto).val("");
    $(this.comboDebCred_lancto).focus();
}

function ativaFormLancamento() {
    $(this.textNumeroDocumento_lancto).removeAttr("disabled");
    $(this.comboDebCred_lancto).removeAttr("disabled");
    $(this.comboJob_lancto).removeAttr("disabled").trigger("chosen:updated");
    $(this.textQtd_lancto).removeAttr("disabled");
    $(this.textValorUnit_lancto).removeAttr("disabled");
    $(this.comboGeraTitulo_lancto).removeAttr("disabled");
    $(this.textParcelas_lancto).removeAttr("disabled");
    $(this.comboTerceiro_lancto).removeAttr("disabled").trigger("chosen:updated");
    $(this.textHistorico_lancto).removeAttr("disabled");
    $(this.botaoSalvar_lancto).removeAttr("disabled");
    $(this.botaoCancelar_lancto).removeAttr("disabled");
    $(this.comboConsultor_lancto).removeAttr("disabled").trigger("chosen:updated");
    $(this.comboConta_lancto).removeAttr("disabled").trigger("chosen:updated");
}

function desativaFormLancamento() {
    $(this.textNumeroDocumento_lancto).attr("disabled", "disabled");
    $(this.comboDebCred_lancto).attr("disabled", "disabled");
    $(this.comboJob_lancto).attr("disabled", "disabled").trigger("chosen:updated");
    $(this.textQtd_lancto).attr("disabled", "disabled");
    $(this.textValorUnit_lancto).attr("disabled", "disabled");
    $(this.comboGeraTitulo_lancto).attr("disabled", "disabled");
    $(this.textParcelas_lancto).attr("disabled", "disabled");
    $(this.textHistorico_lancto).attr("disabled", "disabled");
    $(this.botaoSalvar_lancto).attr("disabled", "disabled");
    $(this.botaoCancelar_lancto).attr("disabled", "disabled");
    $(this.comboTerceiro_lancto).attr("disabled", true).trigger("chosen:updated");
    $(this.comboConsultor_lancto).attr("disabled", true).trigger("chosen:updated");
    $(this.comboConta_lancto).attr('disabled', true).trigger("chosen:updated");
    $(this.comboConsultor).attr('disabled', true).trigger("chosen:updated");
}

function ativaFormTitulo() {

    $(this.textNumeroDocumento).removeAttr("disabled");
    $(this.textData).removeAttr("disabled");
    $(this.textQtd).removeAttr("disabled");
    $(this.textValorUnit).removeAttr("disabled");
    $(this.textValorBruto).removeAttr("disabled");
    $(this.textParcelas).removeAttr("disabled");
    $(this.btVencimento).removeAttr("disabled");
    $(this.comboModelo).removeAttr("disabled").trigger("chosen:updated");
    $(this.comboConsultor).removeAttr("disabled").trigger("chosen:updated");
    $(this.comboConta_lancto).removeAttr("disabled").trigger("chosen:updated");
}

function desativaFormTitulo() {

    $(this.textNumeroDocumento).attr("disabled", "disabled");
    $(this.textData).attr("disabled", "disabled");
    $(this.textQtd).attr("disabled", "disabled");
    $(this.textValorUnit).attr("disabled", "disabled");
    $(this.textValorBruto).attr("disabled", "disabled");
    $(this.textParcelas).attr("disabled", "disabled");
    $(this.btVencimento).attr("disabled", "disabled");
    $(tela.comboModelo).attr('disabled', true).trigger("chosen:updated");
    $(tela.comboConsultor).attr('disabled', true).trigger("chosen:updated");
}

function OnBeforeSend(texto) {
    $("#boxLoading").show();
    $("#boxLoading").html(texto);
}

function OnFailed(ajax, msg) {
    $("#boxLoading").hide();
    var msgUrl = "Erro ao chamar o método: " + this.url + " \r\n ";
    var msgDados = "Dados para chamar o método: " + this.data + " \r\n ";
    alert(msgUrl + msgDados + ajax.responseText.replace('\'', ''));
}
