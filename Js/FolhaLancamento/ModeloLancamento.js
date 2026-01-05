function ModeloLancamento(modulo,codigo,tipo)
{
    this.modulo = modulo;
    this.codigo = codigo;
    this.tipo = tipo;
    this.iniciaTela = iniciaTelaMODELO_LANCAMENTO;
    this.salvaFolha = salvaFolhaMODELO_LANCAMENTO;
    this.atualizaJobs = atualizaJobs;
    this.verificaAlteracaoParcelas = verificaAlteracaoParcelas;
    this.carregaModelo = carregaModeloMODELO_LANCAMENTO;
    this.alteraModelo = alteraModelo;
    this.salvaLancto = salvaLanctoMODELO_LANCAMENTO;
    this.alteraLancto = alteraLancto;
    this.deletaLancto = deletaLancto;
    this.pegaValorOldLancto1 = pegaValorOldLancto1;
    this.alteraValorLancto1 = alteraValorLancto1;
    this.carregaJob = carregaJob;
    this.montaGridLanctos = montaGridLanctosMODELO_LANCAMENTO;
    this.loadVencimentos = loadVencimentos;
    this.salvaVencimentos = salvaVencimentos;
    this.remanejaSequencia = remanejaSequencia;
    this.cancela = cancela;
    this.calculaTotais = calculaTotais;
    this.limpaCampos = limpaCampos;
    this.ativaFormLancamento = ativaFormLancamentoMODELO_LANCAMENTO;
    this.desativaFormLancamento = desativaFormLancamento;
    this.ativaFormTitulo = ativaFormTitulo;
    this.desativaFormTitulo = desativaFormTitulo;
}
ModeloLancamento.prototype = new Form;

function iniciaTelaMODELO_LANCAMENTO() {
    tela.exibeEtapa();
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
                    tela.calculaTotais(result);
                    tela.ativaFormLancamento();

                    $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                    $(tela.textQtd).val(number_format(result[0].qtd, 6, ',', '.'));
                    $(tela.textValorUnit).val(number_format(result[0].valorUnit, 6, ',', '.'));
                    $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                    $(tela.comboFornecedor).val(result[0].terceiro);
                    $(tela.textData).val(result[0].data);
                    $(tela.textParcelas).val(result[0].vencimentos.length);
                    $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                    $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                    var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                    newSeqLote++;
                    $(tela.hdSeqLote_lancto).val(newSeqLote);
                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                    carregaContas($(tela.hdSeqLote_lancto).val());
                }
                else {
                    tela.carregaModelo();
                    $(tela.textNumeroDocumento).val("");
                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                }
            }
            else {
                tela.carregaModelo();
                $(tela.textNumeroDocumento).val("");
                $(tela.textQtd_lancto).val("1");
                $(tela.textValorUnit_lancto).val("0");
                $(tela.textValor_lancto).val("0");
                $(tela.textParcelas_lancto).val("1");
            }
        },
        error: OnFailed
    });
}

function salvaFolhaMODELO_LANCAMENTO(input)
{
    $.ajax({
        type: "POST",
        url: "FormGenericTitulos.aspx/salvaLanctosModelo",
        data: "{modulo:'" + tela.modulo + "', codigo:" + tela.codigo + "}",
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
                    alert("Lançamentos salvos perfeitamente no modelo.");
                    document.location.href = "FormGridModelos.aspx";
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

function carregaModeloMODELO_LANCAMENTO()
{
    var erroGetListaModelos = 0;
         
    if(this.codigo > 0)
    {
        $.ajax({
            type: "POST",
            url: "FormGenericTitulos.aspx/loadLanctosModelo",
            data: "{modulo:'" + tela.modulo + "', cod_modelo: " + tela.codigo + ", terceiroEscolhido: 0, descTerceiroEscolhido: '', data:'',valorbruto:0,codmoeda:0}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function() {
                $("#boxLoading").html("Pegando lançamentos do modelo...");
            },
            success: function(r) {
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

                        $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                        $(tela.textParcelas).val(result[0].vencimentos.length);

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
            },
            complete: function() {
                carregaContas($(tela.hdSeqLote_lancto).val());
            },
            error: OnFailed
        });
    }
    else
    {
        alert("Modelo inválido.");
    }
}

function salvaLanctoMODELO_LANCAMENTO() 
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
    var modelo = this.codigo;
    var numeroDocumento = $(this.textNumeroDocumento_lancto).val();
    var tipoConta = "";
    var codConsultor = $(this.comboConsultor_lancto).val();
    var descConsultor = $(this.comboConsultor_lancto + " option:selected").text();
    var valorGrupo = $(this.textGrupo).val();

    if ($(this.textHistorico_lancto).val().replace(/\'/g, "") != "" && ~$(this.textHistorico_lancto).val().indexOf("'")) //Se conter apóstrofo (') exibirá a mensagem.
        alert("O apóstrofo (') do histórico será removido.");

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

            if (valorGrupo == "" || valorGrupo == null) {
                erros.push("Informe o grupo contábil");
            }

            if (isNaN(valorGrupo)) {
                erros.push("Informe um valor numérico para o grupo contábil");
            }

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
                    if ((tela.tipo == "CP" && tipoConta != "CP") || (tela.tipo == "CR" && tipoConta != "CR")) {
                        erros.push("Tipo de conta inválido.");
                    }
                }
            }

            if (job == "0") {
                erros.push("Informe o Job do Lançamento");
            }

            if (isNaN(valor)) {
                erros.push("Informe o Valor do Lançamento");
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
                        if (parcelas <= 0)
                            erros.push("O número de parcelas deve ser maior que Zero.");
                    } else {
                        erros.push("Informe a quantidade de parcelas");
                    }

                    //if(terceiro == "0")
                    //erros.push("Escolha um Terceiro para Gerar Título.");
                }
                else {
                    if (seqLote == 1) {
                        if (tela.tipo == "CP" || tela.tipo == "CR") {
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

            if (erros.length == 0) {
                var parametros = "{modulo:'" + tela.modulo + "', seqLote: " + seqLote + ", debCred: '" + debCred + "', conta: '" + conta + "', descConta: '" + descConta + "', ";
                parametros += "job: " + job + ", descJob: '" + descJob + "', qtd: " + qtd + ", valorUnit: " + valorUnit + ", valor: " + valor + ", linhaNegocio: " + linhaNegocio + ", descLinhaNegocio: '" + descLinhaNegocio + "', ";
                parametros += "divisao: " + divisao + ", descDivisao: '" + descDivisao + "', cliente: " + cliente + ", descCliente: '" + descCliente + "', geraTitulo: " + titulo + ", ";
                parametros += "parcelas: " + parcelas + ", terceiro: " + terceiro + ", descTerceiro: '" + descTerceiro + "', historico: '" + historico + "', modelo: " + modelo + ", numeroDocumento:'" + numeroDocumento + "', descConsultor: '" + descConsultor + "', codConsultor: " + codConsultor + ", valorBruto: 0, valorGrupo:" + valorGrupo + " }"

                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulos.aspx/salvaLanctoMODELO_LANCAMENTO",
                    data: parametros,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: OnBeforeSend("Salvando lançamento..."),
                    success: function (r) {
                        var result = r.d;
                        $("#boxLoading").hide();
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

function montaGridLanctosMODELO_LANCAMENTO(lista)
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
        linha += "<td>"+lista[i].valorGrupo+"</td>";
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
        
        if(tela.tipo == "CP" || tela.tipo == "CR")
        {
            if(lista[i].seqLote != 1)
                linha += "<a href=\"javascript:tela.deletaLancto("+lista[i].seqLote+");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";
            else
                linha += "";
        }
        else
        {
            linha += "<a href=\"javascript:tela.deletaLancto("+lista[i].seqLote+");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";
        }

        linha += "</td>";
        linha += "<td></td>";
        linha += "<td class='parcelas' style=\"display:none;\">";
        linha += "</td>";
        linha += "<td class='terceiro'  style=\"display:none;\">";
        linha += lista[i].descTerceiro;
        linha += "</td>";
        linha += "</tr>"; 
        $("#gridLancamentos").append(linha);
    }
}

function ativaFormLancamentoMODELO_LANCAMENTO() {
    $(this.textNumeroDocumento_lancto).attr("disabled", "disabled");
    $(this.comboDebCred_lancto).removeAttr("disabled");
    $(this.comboConta_lancto).removeAttr("disabled");
    $(this.comboJob_lancto).removeAttr("disabled");
    $(this.textQtd_lancto).removeAttr("disabled");
    $(this.textValorUnit_lancto).removeAttr("disabled");
    $(this.comboGeraTitulo_lancto).removeAttr("disabled");
    $(this.textParcelas_lancto).removeAttr("disabled");
    $(this.comboTerceiro_lancto).removeAttr("disabled");
    $(this.textHistorico_lancto).removeAttr("disabled");
    $(this.botaoSalvar_lancto).removeAttr("disabled");
    $(this.botaoCancelar_lancto).removeAttr("disabled");
    $(this.comboConsultor).removeAttr("disabled");
}