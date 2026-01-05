function BaixaTitulo(modulo)
{
    this.prefixo = "#ctl00_areaConteudo_";
    this.prefixo_s = "ctl00_areaConteudo_";
    this.modulo = modulo;
    this.iniciaTela = iniciaTelaBAIXATITULO;
    this.salvaFolha = salvaFolhaBAIXATITULO;
    this.atualizaJobs = atualizaJobs;
    this.verificaAlteracaoParcelas = verificaAlteracaoParcelas;
    this.carregaModelo = carregaModelo;
    this.alteraModelo = alteraModelo;
    this.salvaLancto = salvaLanctoBAIXATITULO;
    this.alteraLancto = alteraLancto;
    this.deletaLancto = deletaLancto;
    this.pegaValorOldLancto1 = pegaValorOldLancto1;
    this.alteraValorLancto1 = alteraValorLancto1;
    this.carregaJob = carregaJob;
    this.montaGridLanctos = montaGridLanctosBAIXATITULO;
    this.loadVencimentos = loadVencimentos;
    this.salvaVencimentos = salvaVencimentos;
    this.remanejaSequencia = remanejaSequencia;
    this.cancela = cancela;
    this.calculaTotais = calculaTotaisBAIXATITULO;
    this.calculaTotal = calculaTotal;
    this.limpaCampos = limpaCamposBAIXATITULO;
    this.ativaFormLancamento = ativaFormLancamentoBAIXATITULO;
    this.desativaFormLancamento = desativaFormLancamento;
    this.ativaFormTitulo = ativaFormTitulo;
    this.desativaFormTitulo = desativaFormTitulo;
}
BaixaTitulo.prototype = new Form;

function calculaTotal(tabela, spanTotal,tipo){
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/getSessionLanctos",
        data: "{modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Calculando valores..."),
        success: function(r) {
            var result = r.d;
            $("#boxLoading").hide();
            var inputs = $("#" + tabela + " input:checked[type='checkbox']");
            var valor = 0;
            for (var i = 0; i < inputs.length; i++) {
                var vlr = limpaNumero($(inputs[i]).attr("title"));
                valor = valor + vlr;
            }
            if (tipo == "PAGAR")
                $("#" + spanTotal).text(number_format(valor * -1, 2, ',', '.'));
            else
                $("#" + spanTotal).text(number_format(valor, 2, ',', '.'));

            tela.calculaTotais(result);
        },
        error: OnFailed,
        complete: function() {
            calculaTotalSelecionado();
        } 
    });
}

function calculaTotalSelecionado(){
    var totalAReceber = 0;
    var totalAPagar = 0;
    var selecionadosAPagar = $("#gridLanctosAPagar input:checked[type='checkbox']");
    var selecionadosAReceber = $("#gridLanctosAReceber input:checked[type='checkbox']");


    for(var i=0;i<selecionadosAPagar.length;i++)
    {
        totalAPagar = totalAPagar + limpaNumero($(selecionadosAPagar[i]).attr("title"));
    }

    for(var i=0;i<selecionadosAReceber.length;i++)
    {
        totalAReceber = totalAReceber + limpaNumero($(selecionadosAReceber[i]).attr("title"));
    }
    var total = totalAReceber + (totalAPagar*-1);
    $("#totalBaixa").text(number_format(total,2,',','.'));
}

function calculaTotaisBAIXATITULO(lista)
{

    var totalDebito = 0;
    var totalCredito = 0;
    var totalDiferenca = 0;
    var totalAReceber = 0;
    var totalAPagar = 0;
    var selecionadosAPagar = $("#gridLanctosAPagar input:checked[type='checkbox']");
    var selecionadosAReceber = $("#gridLanctosAReceber input:checked[type='checkbox']");


    for(var i=0;i<selecionadosAPagar.length;i++)
    {
        totalAPagar = totalAPagar + limpaNumero($(selecionadosAPagar[i]).attr("title"));
    }

    for(var i=0;i<selecionadosAReceber.length;i++)
    {
        totalAReceber = totalAReceber + limpaNumero($(selecionadosAReceber[i]).attr("title"));
    }
    
    for(var i=0;i<lista.length;i++)
    {
    
        if(lista[i].debCred == "D")
        {
            totalDebito = totalDebito + lista[i].valor;
        }
        else if(lista[i].debCred == "C")
        {
            totalCredito  = totalCredito + lista[i].valor;
        }
    }
    
    totalDebito = totalDebito + totalAPagar;
    totalCredito = totalCredito + totalAReceber;
    
    totalDiferenca = totalDebito - totalCredito;
    
    $(this.totalDebito).html(number_format(totalDebito,2,',','.'));
    $(this.totalCredito).html(number_format(totalCredito,2,',','.'));
    $(this.totalDiferenca).html(number_format(totalDiferenca,2,',','.'));
    
    if($(tela.hdSeqLote_lancto).val() == 1)
    {
        if(totalDiferenca < 0){
            $(tela.comboDebCred_lancto).val("D");
        }else if(totalDiferenca > 0){
            $(tela.comboDebCred_lancto).val("C");
        }else{
            $(tela.comboDebCred_lancto).val("0");
        }
    }
    calculaTotalSelecionado();
}

function salvaFolhaBAIXATITULO(input)
{
    var diferenca = limpaNumero($(this.totalDiferenca).text());
    var selecionadosAPagar = $("#gridLanctosAPagar input:checked[type='checkbox']");
    var selecionadosAReceber = $("#gridLanctosAReceber input:checked[type='checkbox']");
    var arr = new Array();
    var jsonArr = "";
    if ($(tela.textData).val() == "") {
        alert("Nenhuma data informada!");
        return;
    }

    if(diferenca == 0)
    {
        for(var i=0;i<selecionadosAPagar.length;i++)
        {
            arr.push($(selecionadosAPagar[i]).val());
        }
        
            for(var i=0;i<selecionadosAReceber.length;i++)
        {
            arr.push($(selecionadosAReceber[i]).val());
        }
        
        jsonArr = JSON.stringify(arr);

        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/baixaTitulo",
            data: "{modulo:'" + tela.modulo + "',data: '" + $(tela.textData).val() + "',arrSelecionados:" + jsonArr + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function() {
                $(input).val("Processando...");
                $(input).attr("disabled", "disabled");
            },
            success: function(r) {
                var result = r.d;
                if (result != undefined) {
                    if (result.length == 0) {
                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulos.aspx/getSessionBaixaGerada",
                            data: "{modulo:'" + tela.modulo + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (r) {
                                alert("Baixa " + r.d + " gerada com sucesso.");
                                document.location.href = 'FormBaixaTitulos.aspx';
                            },
                            error: OnFailed
                        });
                        
                        //document.location.reload();
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
    else
    {
        alert("Débito e Crédito não estão batendo.");
    }
}

function iniciaTelaBAIXATITULO()
{
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/getSessionLanctos",
        data: "{modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Montando tela..."),
        success: function(r) {
            var result = r.d;
            $("#boxLoading").hide();
            if (result != null || result != undefined) {
                if (result.length > 0) {
                    tela.montaGridLanctos(result);

                    tela.ativaFormLancamento();


                    $(tela.textData).val(result[0].dataLancamento_formatada);
                    $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                    $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                    var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                    newSeqLote++;
                    $(tela.hdSeqLote_lancto).val(newSeqLote);

                    $(tela.textValor_lancto).val("0");
                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                }
                else {
                    $(tela.hdSeqLote_lancto).val("1");
                    $(tela.hdSeqLoteMin_lancto).val("1");
                    $(tela.hdSeqLoteMax_lancto).val("1");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                    tela.ativaFormLancamento();
                }
            }
            else {
                $(tela.hdSeqLote_lancto).val("1");
                $(tela.hdSeqLoteMin_lancto).val("1");
                $(tela.hdSeqLoteMax_lancto).val("1");
                $(tela.textValor_lancto).val("0");
                $(tela.textQtd_lancto).val("1");
                $(tela.textValorUnit_lancto).val("0");
                $(tela.textParcelas_lancto).val("1");
                tela.ativaFormLancamento();
            }
            tela.calculaTotais(result);
        },
        complete: function() {
            carregaContas($(tela.hdSeqLote_lancto).val());
        },
        error: OnFailed
    });
}

function salvaLanctoBAIXATITULO() 
{
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
        success: function(r) {
            var result = r.d;
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
                                if (atual.vencimentos[i].data == null && atual.vencimentos[i].valor == null)
                                    nulas = nulas + 1;
                            }

                            if (nulas > 0)
                                erros.push("Por favor, preencha as parcelas vindas do modelo.");

                            if (atual != null)
                                totalParcelasAnterior = atual.vencimentos.length
                        }
                    }
                }
            }
        },
        complete: function() {
            $.ajax({
                type: "POST",
                url: "FormGenericTitulos.aspx/getContaContabil",
                data: "{codigo:'" + conta + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: OnBeforeSend("Verificando tipo de conta contábil..."),
                success: function(r) {
                    var result = r.d;
                    if (result != undefined && result != "")
                        tipoConta = result.tipo;
                },
                complete: function() {
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
                            if (tipoConta != "BC") {
                                erros.push("Tipo de conta inválido.");
                            }
                        }
                    }

                    if (job == "0") {
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

                    if (historico == "")
                        erros.push("Preencha o histórico do lançamento.");

                    if (seqLote != "1") {
                        if (numeroDocumento == "")
                            erros.push("Informe o número do documento.");
                    }

                    if (erros.length == 0) {
                        var parametros = "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + ",data: '" + data + "', debCred: '" + debCred + "', conta: '" + conta + "', descConta: '" + descConta + "', ";
                        parametros += "job: " + job + ", descJob: '" + descJob + "', qtd: " + qtd + ", valorUnit: " + valorUnit + ", valor: " + valor + ", linhaNegocio: " + linhaNegocio + ", descLinhaNegocio: '" + descLinhaNegocio + "', ";
                        parametros += "divisao: " + divisao + ", descDivisao: '" + descDivisao + "', cliente: " + cliente + ", descCliente: '" + descCliente + "', geraTitulo: " + titulo + ", ";
                        parametros += "parcelas: " + parcelas + ", terceiro: " + terceiro + ", descTerceiro: '" + descTerceiro + "', historico: '" + historico + "', modelo: 0,numeroDocumento:'" + numeroDocumento + "', descConsultor:'" + descConsultor + "', codConsultor:" + codConsultor + ", valorBruto: 0, valorGrupo:0, codMoeda:0}";

                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulos.aspx/salvaLancto",
                            data: parametros,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: OnBeforeSend("Salvando lançamento..."),
                            success: function(r) {
                                var result = r.d;
                                $("#boxLoading").hide();
                                tela.montaGridLanctos(result);
                                tela.calculaTotais(result);

                                $(tela.botaoSalvar_lancto).val("Incluir");
                                tela.remanejaSequencia(seqLote);
                                tela.limpaCampos();
                                $(tela.botaoSalvar).removeAttr("disabled");
                                tela.ativaFormTitulo();
                            },
                            complete: function() {
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

function montaGridLanctosBAIXATITULO(lista)
{
    $("#gridLancamentos tr[class='linha']").remove();
    $("#gridLancamentos tr[class='linhaMarcada']").remove();
    $("#gridLancamentos tr[class='linhaSelecionada']").remove();
    
    for(var i=0;i<lista.length;i++)
    {
        var linha = "<tr class='linha' onmouseover=\"mudaCor(this);\" onmouseout=\"voltaCor(this);\">";
        linha += "<td id='linhaSeqLote' style=\"display:none;\">";
        linha += lista[i].seqLote;
        linha += "</td>";
        linha += "<td>0</td>"; //BaixaTitulo.js
        linha += "<td class='debCred' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        linha += lista[i].debCred;
        linha += "</td>";
        linha += "<td class='conta' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        linha += lista[i].descConta;
        linha += "</td>";
        linha += "<td class='job' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        linha += lista[i].descJob;
        linha += "</td>";
        linha += "<td class='linhaNegocio'  style=\"display:none;\">";
        linha += lista[i].descLinhaNegocio;
        linha += "</td>";
        linha += "<td class='divisao' style=\"display:none;\">";
        linha += lista[i].descDivisao;
        linha += "</td>";
        linha += "<td class='valor' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        linha += number_format(lista[i].valor,2,',','.');
        linha += "</td>";
        linha += "<td class='historico' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        linha += lista[i].historico;
        linha += "</td>";
        linha += "<td class='titulo' onClick=\"tela.alteraLancto("+lista[i].seqLote+", this.parentNode);\">";
        if(lista[i].titulo)
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
        linha += "<td class='deletar'>";
        
        if(lista[i].seqLote != 1)
            linha += "<a href=\"javascript:tela.deletaLancto("+lista[i].seqLote+");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";
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
    }
}

function ativaFormLancamentoBAIXATITULO() {
    if ($(this.hdSeqLote_lancto).val() == "1") {
        $(this.textNumeroDocumento_lancto).attr("disabled", "disabled");
    } else {
        $(this.textNumeroDocumento_lancto).removeAttr("disabled");
    }
    $(this.comboDebCred_lancto).removeAttr("disabled");
    $(this.comboConta_lancto).removeAttr("disabled");
    $(this.comboJob_lancto).removeAttr("disabled");
    $(this.textQtd_lancto).removeAttr("disabled");
    $(this.textValorUnit_lancto).removeAttr("disabled");
    $(this.textValor_lancto).removeAttr("disabled");
    $(this.comboGeraTitulo_lancto).removeAttr("disabled");
    $(this.textParcelas_lancto).removeAttr("disabled");
    $(this.comboTerceiro_lancto).removeAttr("disabled");
    $(this.textHistorico_lancto).removeAttr("disabled");
    $(this.botaoSalvar_lancto).removeAttr("disabled");
    $(this.botaoCancelar_lancto).removeAttr("disabled");
}

function limpaCamposBAIXATITULO() {
    //$(this.textNumeroDocumento_lancto).val("");
    $(this.comboDebCred_lancto).val("0");
    $(this.comboConta_lancto).val("0");
    $(this.textQtd_lancto).val("1");
    $(this.textValorUnit_lancto).val("0");
    $(this.textValor_lancto).val("0");
    $(this.comboGeraTitulo_lancto).val("N");
    exibeVencimentosTitulo("0");
    $(this.textParcelas_lancto).val("1");
    $(this.comboTerceiro_lancto).val("0");
    $(this.textHistorico_lancto).val("");
    $(this.comboDebCred_lancto).focus();
    this.ativaFormLancamento();
}