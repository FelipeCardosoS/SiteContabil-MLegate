var prefix = "ctl00_areaForm_";
var listaPeriodos = [];
const regexData = "([-0-9])+";

$(document).ready(function () {
    Extrai_Periodos();
    Prepara_Tabela_Periodos();
    Atualiza_Combos();

    $("input[id^=" + "ctl00_areaForm_radioTipo" + "]").change(function () {
        radioTipoChanged();
    });

    $("#ctl00_botaoSalvar").on("click", function (e) {
        if (Salva_Periodos())
            $("#ctl00_botaoSalvarHidden").click();

        });
    $("#ctl00_botaoSalvar")[0].onclick = "";

});

function radioTipoChanged() {
    let selecionado = $("input[id^=" + "ctl00_areaForm_radioTipo" + "]:checked")
    if (selecionado.length != 0 && selecionado.val() == "1")
        $(".periodos").css("display", "");
    else
        $(".periodos").css("display", "none");
}

function Salva_Periodos() {
    let listaPeriodosNovo = [];
    let listaPeriodosDeletar = [];
    let erros = [];
    let contPeriodo = 0
    let contPeriodoInicioAberto = 0;
    let contPeriodoFimAberto = 0;

    let radioTipo = $("input[id^=" + "ctl00_areaForm_radioTipo" + "]:checked");
    if (radioTipo.length != 0 && radioTipo.val() == "0") {
        $("#tablePeriodos tr:not(.deletar).linhaPeriodo").each(function () {
            let codPeriodo = Number($(this).find("td .codPeriodo").val())
            if(!isNaN(codPeriodo))
                listaPeriodosDeletar.push(codPeriodo);
        });
        $("#" + prefix + 'H_PERIODOS_DELETAR').val(JSON.stringify(listaPeriodosDeletar.filter((x, i) => listaPeriodosDeletar.indexOf(x) === i) ));
        return true;
	}

    $("#tablePeriodos tr:not(.deletar).linhaPeriodo").each(function () {
        contPeriodo += 1;
        let sucesso = true;
        let preenchido = false;

        let codPeriodo = $(this).find(".codPeriodo").val();
        let dataInicio = $(this).find("td .txtDataInicio").val();
        let dataFim = $(this).find("td .txtDataFim").val();
        let valorReferencia = $(this).find("td .txtValorReferencia").val();

        let errosLinha = [];

        if (dataInicio == undefined || dataInicio == "") {
            errosLinha.push("Informe a Data Início");
            sucesso = false;
        }
        else {
            preenchido = true;
            let parseDataInicio = dataInicio.split("/");
            let objDataInicio = new Date(parseDataInicio[2], parseDataInicio[1] - 1, parseDataInicio[0]);

            if (parseDataInicio.length != 3 || objDataInicio == "Invalid Date" || objDataInicio.getDate() != parseDataInicio[0] || objDataInicio.getMonth() != parseDataInicio[1] - 1 || objDataInicio.getFullYear() != parseDataInicio[2]) {
                errosLinha.push("Data Inicio Inválida");
                sucesso = false;
            }
            else {
                dataInicio = objDataInicio;
            }
        }

        if (dataFim == undefined || dataFim == "") {
            dataFim = new Date("9999-12-31T00:00:00");
            contPeriodoFimAberto += 1;
		}
        else {
            preenchido = true;
            let parseDataFim = dataFim.split("/");
            let objDataFim = new Date(parseDataFim[2], parseDataFim[1] - 1, parseDataFim[0]);

            if (parseDataFim.length != 3 || objDataFim == "Invalid Date" || objDataFim.getDate() != parseDataFim[0] || objDataFim.getMonth() != parseDataFim[1] - 1 || objDataFim.getFullYear() != parseDataFim[2]) {
                errosLinha.push("Data Fim Inválida");
                sucesso = false;
            }
            else {
                dataFim = objDataFim;
            }
        }

        if (dataInicio instanceof Date && dataFim instanceof Date) {
            if (dataFim < dataInicio) {
                errosLinha.push("A Data Fim deve ser posterior ao Inicio");
                sucesso = false;
            }
        }

        if (valorReferencia == undefined || valorReferencia == "") {
            errosLinha.push("Informe o Valor Referência");
            sucesso = false;
        }
        else {
            preenchido = true;
            valorReferencia = Number(valorReferencia.replace(",", "."));
            if (isNaN(valorReferencia) || valorReferencia <= 0) {
                errosLinha.push("Valor Referência deve ser maior que 0");
            }
        }

        if (sucesso) {
            listaPeriodosNovo.push({
                CodTipoDespesaPeriodo: codPeriodo,
                DataInicio: dataInicio,
                DataFim: dataFim,
                ValorReferencia: valorReferencia
            });
        }
        else if (preenchido) {
            erros.push("Período " + contPeriodo + " :\n");
            erros.push(...errosLinha);
        }

    });
    if (erros.length == 0 && listaPeriodosNovo.length == 0)
        erros.push("Informe ao menos 1 Período");

    if (contPeriodoFimAberto > 1) {
        erros.push("Há mais de uma Data Fim Aberta nos Períodos");
    }

   // if (listaPeriodosNovo.length != 0) {
   //     listaPeriodosNovo.sort(function (a, b) { return a.dataInicio - b.dataInicio });
   //     let i = 0;
   //     for (i = 0; i < listaPeriodosNovo.length - 1; i++) {
   //         if (listaPeriodosNovo[i].DataFim.getFullYear() == 9999)
   //             break;
   //         if (listaPeriodosNovo[i].DataInicio > listaPeriodosNovo[i].DataFim || listaPeriodosNovo[i].DataFim >= listaPeriodosNovo[i + 1].DataInicio)
   //             break;
   //         else {
   //             let novoPeriodo = new Date(listaPeriodosNovo[i].DataFim.getFullYear(), listaPeriodosNovo[i].DataFim.getMonth() + 1, 1);
   //             if (novoPeriodo.toString() != listaPeriodosNovo[i + 1].DataInicio.toString())
   //                 break;
			//}
   //     }

   //     if ((i != listaPeriodosNovo.length - 1) || (listaPeriodosNovo[i].DataFim != undefined && listaPeriodosNovo[i].DataFim != "" && listaPeriodosNovo[i].DataInicio > listaPeriodosNovo[i].DataFim))
   //         erros.push("Há conflitos de Períodos.");
   // }


    if (erros.length > 0) {
        alert(erros.join("\n"));
        return false;
    }

    $("#tablePeriodos tr.deletar.linhaPeriodo").each(function () {
        let codPeriodo = Number($(this).find(".codPeriodo").val());
        if (codPeriodo != undefined && !isNaN(codPeriodo))
            listaPeriodosDeletar.push(codPeriodo);
    });
    
    $("#" + prefix + 'H_PERIODOS').val(JSON.stringify(listaPeriodosNovo));
    $("#" + prefix + 'H_PERIODOS_DELETAR').val(JSON.stringify(listaPeriodosDeletar.filter((x, i) => listaPeriodosDeletar.indexOf(x) === i)));

    return true;
}

function Extrai_Periodos() {
    let JSONlistaPeriodos = $("#" + prefix + 'H_PERIODOS').val();

    listaPeriodos = JSON.parse(JSONlistaPeriodos);
}

function Prepara_Tabela_Periodos() {
    $('#tablePeriodos tbody').html("");

    listaPeriodos.forEach(function (periodo) {
        Adiciona_Linha_Periodo(periodo);
    });
    if ($('#tablePeriodos tbody tr').length == 0)
        Adiciona_Linha_Periodo();

    radioTipoChanged();
}

function Adiciona_Linha_Periodo(periodo) {
    let numLinhas = $('#tablePeriodos tbody tr:not(.deletar)').length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = $('#tablePeriodos tbody');
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = $('#tablePeriodos tbody tr:not(.deletar):last');
    }
    else {
        linhaReferenciaAtual = $($('#tablePeriodos tbody tr:not(.deletar)')[linha - 1]);
    }

    let codigoLinha = '<tr class="linhaPeriodo">'
        +'<td class="line codPeriodo"></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataInicio data"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataFim data"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="5" class="txtValorReferencia valor"/></td>'
        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Periodo(this)" class="btnMinus" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Periodo()" class="btnPlus" /></td>'
        + '<td class="line" style="display:none"><input type="checkbox" class="checkDeletar"></td>';
        + '</tr>';

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next("tr");
    }

    Atualiza_Combos();
    Atualiza_Campos_Data();
    Atualiza_Campos_Valor();

    if (periodo != undefined)
        Preenche_Linha_Periodo(linhaNova, periodo)
    else {
        $(linhaNova).find(".codPeriodo").val("0");
        $(linhaNova).find('.txtDataInicio').val("");
        $(linhaNova).find('.txtDataFim').val("");
        $(linhaNova).find('.txtValorReferencia').val("");
    }

    Atualiza_Controles();
    //Atualiza_Triggers();


    return linhaNova;


}

function Remove_Linha_Periodo(linha) {
    $(linha).closest("tr").find("td .checkDeletar").prop("checked", true);
    $(linha).closest("tr").addClass("deletar");

    Atualiza_Controles();
}

//function Remove_Linha_Periodo(Linha) {
//    if ($('#tablePeriodos tbody tr').length > 1) {
//        $(Linha).closest('tr').remove();
//    }
//    else {
//        Limpa_Linha_Periodo(Linha);
//    }

//    Atualiza_Controles();
//}

function Limpa_Linha_Periodo(linha) {
    linha = $(linha).closest('tr')
    linha.find("td .txtDataInicio").val();
    linha.find("td .txtDataFim").val();
    linha.find("td .txtValorReferencia").val();
}

function Preenche_Linha_Periodo(linha, periodo) {
    let dataInicio = new Date(periodo.DataInicio);
    let dataFim = new Date(periodo.DataFim);

    dataInicio = dataInicio.getFullYear() == 1900 ? "" : dataInicio.getDate().toString().padStart(2, "0") + "/" + (dataInicio.getMonth() + 1).toString().padStart(2,"0") + "/" + dataInicio.getFullYear();
    dataFim = dataFim.getFullYear() == 9999 ? "" : dataFim.getDate().toString().padStart(2, "0") + "/" + (dataFim.getMonth() + 1).toString().padStart(2, "0")  + "/" + dataFim.getFullYear();

    linha.find(".codPeriodo").val(periodo.CodTipoDespesaPeriodo);
    linha.find('.txtDataInicio').val(dataInicio);
    linha.find('.txtDataFim').val(dataFim);
    linha.find('.txtValorReferencia').val(periodo.ValorReferencia);

    linha.find('select').trigger('chosen:updated');
}


function Atualiza_Controles() {
    if ($('#tablePeriodos tbody tr:not(.deletar)').length == 0)
        Adiciona_Linha_Periodo();

    if ($('#tablePeriodos tbody tr:not(.deletar)').length == 1)
        $('#tablePeriodos tbody tr:not(.deletar) .tdREMOVE').first().css("display", "none");
    else
        $('#tablePeriodos tbody tr:not(.deletar) .tdREMOVE').css("display", "");
}

function Atualiza_Campos_Data() {
    $(".data").mask("99/99/9999", { placeholder: "__/__/____", reverse: true });
}


function Atualiza_Combos() {
    $('#tablePeriodos select').chosen({ search_contains: true, no_results_text: "Nenhum resultado encontrado!", width: "100%" });
}


function Atualiza_Campos_Valor() {
    $(".valor").mask("#0,00", { placeholder: "_,__", reverse: true });
}