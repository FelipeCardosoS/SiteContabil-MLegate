function Contabilidade(modulo)
{
    this.prefixo = "#ctl00_areaConteudo_";
    this.prefixo_s = "ctl00_areaConteudo_";
    this.modulo = modulo;
    this.iniciaTela = iniciaTelaCONTABILIDADE;
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
    this.montaGridLanctos = montaGridLanctosCONTABILIDADE;
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
    this.salvaFolha = salvaFolhaContabilidade;
}
Contabilidade.prototype = new Form;

function salvaFolhaContabilidade(input) {
    var result;
    $.ajax({
        type: "POST",
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
        complete: function(a){
            if (result != undefined) {
                if (result.length == 0) {
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

function iniciaTelaCONTABILIDADE() {
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

                    $(tela.comboModelo).val(result[0].modelo);

                    $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                    $(tela.textData).val(result[0].dataLancamento_formatada);
                    $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                    $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                    var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                    newSeqLote++;
                    $(tela.hdSeqLote_lancto).val(newSeqLote);

                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                }
                else {
                    $(tela.textNumeroDocumento).val("");
                    $(tela.hdSeqLote_lancto).val("1");
                    $(tela.hdSeqLoteMin_lancto).val("1");
                    $(tela.hdSeqLoteMax_lancto).val("1");

                    $(tela.textQtd_lancto).val("1");
                    $(tela.textValorUnit_lancto).val("0");
                    $(tela.textValor_lancto).val("0");
                    $(tela.textParcelas_lancto).val("1");
                    tela.ativaFormLancamento();
                }
            }
            else {
                $(tela.textNumeroDocumento).val("");
                $(tela.hdSeqLote_lancto).val("1");
                $(tela.hdSeqLoteMin_lancto).val("1");
                $(tela.hdSeqLoteMax_lancto).val("1");

                $(tela.textQtd_lancto).val("1");
                $(tela.textValorUnit_lancto).val("0");
                $(tela.textValor_lancto).val("0");
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

function montaGridLanctosCONTABILIDADE(lista)
{
    $("#gridLancamentos tr[class='linha']").remove();
    $("#gridLancamentos tr[class='linhaMarcada']").remove();
    $("#gridLancamentos tr[class='linhaSelecionada']").remove();
    
    for(var i=0;i<lista.length;i++) {
            var totalBaixas = 0;
            $.ajax({
                type: "POST",
                async: false,
                url: "FormGenericTitulos.aspx/getTotalBaixas",
                data: "{lote:" + lista[i].lote + ",seqLote:" + lista[i].seqLote + ",modulo:'" + tela.modulo + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(result) {
                    totalBaixas = result.d;
                },
                complete: function() {
                    var linha = "<tr class='linha' onmouseover=\"mudaCor(this);\" onmouseout=\"voltaCor(this);\">";
                    linha += "<td id='linhaSeqLote' style=\"display:none;\">";
                    linha += lista[i].seqLote;
                    linha += "</td>";
                    linha += "<td>" + lista[i].valorGrupo + "</td>";
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
                    if (totalBaixas > 0) {
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
                    
                    linha += "<a href=\"javascript:tela.deletaLancto("+lista[i].seqLote+");\"><img src='Imagens/icones/cross.jpg' alt='' style='border:0px;' /></a>";

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
