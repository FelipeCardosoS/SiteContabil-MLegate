/*TODO:
 * Criar uma forma mais "limpa" de implementar os controles das tabelas dinâmicas. Há muita repetição de código. No momento, uma versão funciona é a prioridade.
 * Criar os Controles da Tabela de Despesa
 */ 


var prefix = "ctl00_areaForm_";
var listaGestores = [];
var listaConsultores = [];
var listaDespesas = [];
var regexNumbers = new RegExp('([^0-9])+', 'g');
const regexData = "([0-9])+";

var htmlGestores = "";
var comboContas = '';
var comboTerceiros = '';
var comboDespesas = '<option value="0"> Escolha... </option>';


var headerConsultor = '<thead>'
    + '<tr class="titulo">'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal"><button type="button" onclick="Toggle_Detalhes_Consultor(this)">Detalhes</button></th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Consultor</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Aprovador Horas</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Aprovador RDV</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Avisar Aprovador?</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Status - <a class="txtStatus"> (-) </a></th>'
    + '<th></th>'
    + '<th></th>'
    + '</tr>'
    + '</thead>';

var tableStatusConsultor = '<table class="tableStatusConsultor">'
    + '<thead>'
    + '<tr class="titulo">'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Inicio</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Fim</th>'
    + '</tr>'
    + '</thead>'
    + '<tbody>'
    + '<tr>'
    + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataInicio data"/></td>'
    + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataFim data"/></td>'
    + '</tr>'
    + '</tbody>'
    + '</table>';

var tableHeaderDespesa = '<thead>'
    + '<tr class="titulo">'
    + '<th style = "color: blue; border: 1px solid #B0C4DE; font-weight: normal"> Tipo Despesa</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Cobrar do Cliente</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal"><a>Limite Diário</a> <button type="button" class="btnToggleValorDespesa" onclick="toggleValorDespesa(this)">Mostrar</button></th>'
    + '</tr>'
    + '</thead>';

var tableValorDespesa = '<table class="tableLinhaValorDespesa" style="display:none">'
	+ '<thead>'
	+ '<tr class="titulo">' 
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Inicio</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Fim</th>'
    + '<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Valor Limite</th>'
    + '<th></th>'
    + '<th></th>'
    + '</tr>'
    + '</thead>'
    + '<tbody>'
    + '</tbody>'
    + '</table>';


$(document).ready(function () {
    Extrai_Gestores();
    Extrai_Despesas();
    Prepara_Tabela_Gestores();

    Prepara_Tabela_Consultores();

    Prepara_Tabela_Despesas();

    Atualiza_Combos();
    toggleGestores();

    $(".menu thead tr th").click(mudaTela);

    $("#ctl00_botaoSalvar").on("click",function (e) {
        Salva_Job().then(function (result) {
            if (result) {
                $("#ctl00_botaoSalvarHidden").click();
            }
        });
    });
    $("#ctl00_botaoSalvar")[0].onclick = "";

});

async function Salva_Job() {
    try {
        let resultadoGestores = await Salva_Gestores();
        let resultadoConsultores = await  Salva_Consultores();
        let resultadoDespesas = await  Salva_Despesas();

        if (!resultadoGestores || !resultadoConsultores || !resultadoDespesas)
            return false;

        return true;

    }
    catch (ex) {
        alert("Erro durante o processamento da página:\n" + ex);
        return false;
	}

}

function Atualiza_Combos() {
    $('#tableGestores select').chosen({ search_contains: true, no_results_text: "Nenhum resultado encontrado!", width: "100%" });
    $('#tableConsultores select').chosen({ search_contains: true, no_results_text: "Nenhum resultado encontrado!", width: "100%" });
    $('#tableDespesas select').chosen({ search_contains: true, no_results_text: "Nenhum resultado encontrado!", width: "100%" });
}

function Evento_Salvar() {
    let validacaoTabela = Salva_Gestores();
    let validacaoControles = Page_ClientValidate();
    return validacaoControles && validacaoTabela;
}

function Extrai_Gestores() {
    //Dados são recebidos em Base64
    //.NET não gosta muito que código HTML arbitrário seja enviado pelo backend
    //Converter esse HTML em Base64 evita que ele cause problemas no Postback
    let b64HTMLGestores = atob($("#" + prefix + 'hdGestores').val());

    //JS não gosta de Base64 de uma string UTF-8, então temos mais esse problema para resolver :)
    htmlGestores = decodeURIComponent(b64HTMLGestores.split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
}

function Extrai_Despesas() {
    listaDespesas = JSON.parse($("#" + prefix + 'hdDespesas').val());

    listaDespesas.forEach(function (tipoDespesa) {
        comboDespesas += "<option value = '" + tipoDespesa.CodDespesa + "'>" + tipoDespesa.Descricao + "</option>";
    });
}

function Extrai_Gestores_Job() {
    listaGestores = JSON.parse($("#" + prefix + 'hdGestorJob').val());
}

function Extrai_Consultores_Job() {
    listaConsultores = JSON.parse($("#" + prefix + 'hdConsultorJob').val());
}

function Extrai_Despesas_Job() {
    listaDespesas = JSON.parse($("#" + prefix + 'hdDespesaJob').val());
}

function Prepara_Tabela_Gestores() {
    $('#tableGestores tbody').html("");

    Extrai_Gestores_Job();
    Carrega_Gestores();
    if ($('#tableGestores tbody tr').length == 0)
        Adiciona_Linha_Regra();

}



function Prepara_Tabela_Consultores() {
    $('#tableConsultores tbody').html("");
    Extrai_Consultores_Job();
    Carrega_Consultores();
    if ($('#tableConsultores tbody tr').length == 0)
        Adiciona_Linha_Consultor();
}


function Prepara_Tabela_Despesas() {
    $('#tableDespesas tbody').html("");

    Extrai_Despesas_Job();
    Carrega_Despesas();
    if ($('#tableDespesas tbody tr').length == 0)
        Adiciona_Linha_Despesas();
}


function Carrega_Gestores() {
    if (listaGestores == undefined || listaGestores == "")
        return;

    listaGestores.forEach(function (gestor) {
        Adiciona_Linha_Regra(gestor);
    });

}

function Carrega_Consultores() {
    if (listaConsultores == undefined || listaConsultores == "")
        return;

    listaConsultores.forEach(function (consultor) {
        Adiciona_Linha_Consultor(consultor);
    });

}

function Carrega_Despesas() {
    if (listaDespesas == undefined || listaDespesas == "")
        return;

    listaDespesas.forEach(function (despesa) {
        Adiciona_Linha_Despesas(despesa);
    });

}

function Adiciona_Linha_Regra(gestorJob) {
    let numLinhas = $('#tableGestores tbody tr').length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = $('#tableGestores tbody');
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = $('#tableGestores tbody tr:last');
    }
    else {
        linhaReferenciaAtual = $($('#tableGestores tbody tr')[linha - 1]);
    }

    let codigoLinha = '<tr>'
        + '<td class="codGestorJob" style="display:none"></td>'
        + '<td class="line"><select class="cboGestores">' + htmlGestores + '</select></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataInicio data"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataFim data"/></td>'
        + '<td class="line" style="text-align:center"> <a class="txtStatus">-</a></td>'

        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Gestor(this)" class="btnMinus_Banco" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Gestor(undefined, this.rowIndex)" class="btnPlus_Banco" /></td>'
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

    if (gestorJob != undefined)
        Preenche_Linha_Gestor(linhaNova, gestorJob)
    else {
        $(linhaNova).find('.cboGestores').val("");
        $(linhaNova).find('.txtDataInicio').val("");
        $(linhaNova).find('.txtDataFim').val("");
        $(linhaNova).find('.txtDataFim').val("");
        $(linhaNova).find('.codGestorJob').val("0");
    }

    Atualiza_Controles();
    Atualiza_Triggers();

    return linhaNova;
}

function Adiciona_Linha_Consultor(consultor) {
    let numLinhas = $('#tableConsultores .linhaConsultor').length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = $('#tableConsultores tbody');
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = $('#tableConsultores .linhaConsultor:last');
    }
    else {
        linhaReferenciaAtual = $($('#tableConsultores .linhaConsultor')[linha]);
    }

    let codigoLinha = '<tr class="linhaConsultor">'
        + '<td class="codConsultorJob"></td>'
        + '<td colspan=3>'
        + '<table class="tableLinhaConsultor">'
        + headerConsultor
        + '<tbody>'
        + '<tr>'
        + '<td></td>'
        + '<td class="line"><select class="cboConsultor">' + htmlGestores + '</select></td>'
        + '<td class="line"><select class="cboAprovadorHora">' + htmlGestores + '</select></td>'
        + '<td class="line"><select class="cboAprovadorRDV">' + htmlGestores + '</select></td>'
        + '<td style="width:2%;text-align:center;"><input type="checkbox" class="checkEmail"> </td>'
        + '<td style="text-align:center">'
        + tableStatusConsultor
        + '</td>'
        + '</tr>'
        + '<tr class="linhaDetalhe" style="display: none;">'
        + '<td></td>'
        + '<td class="line"> <label style="display: flex;">Email</label> <input type="email" class="txtEmail" /> </td>'
        + '<td class="line"> <label style="display: flex;">Contato</label> <input type="text" class="txtContato" /> </td>'
        + '<td class="line"> <label style="display: flex;">Telefone</label> <input type="text" class="txtTelefone" /></td>'
        + '</tr>'
        + '<tr class="linhaDetalhe" style="display: none;">'
        + '<td></td>'
        + '<td class="line"> <label style="display: flex;">Taxa</label> <input type="text" class="txtTaxa" /> </td>'
        + '<td class="line"> <label style="display: flex;">Custo Intra-Divisão</label> <input type="text" class="txtCustoIntraDivisao" /></td>'
        + '</tr>'
        + '</tbody>'
        + '</table>'
        + '</td>'
        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Consultor(this)" class="btnMinus_Banco" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Consultor(undefined, this.rowIndex)" class="btnPlus_Banco" /></td>'
        + '</tr>'

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next(".linhaConsultor");
    }

    Atualiza_Combos();


    if (consultor != undefined) {
        Preenche_Linha_Consultor(linhaNova, consultor);
    }
    else {
        $(linhaNova).find('.cboConsultor').val("");
        $(linhaNova).find('.codConsultorJob').val("0");
    }

    Atualiza_Controles();
    Atualiza_Campos_Data();
    Atualiza_Campos_Valor();

    return linhaNova;
}

function Adiciona_Linha_Status_Consultor(tabela, dataInicio, dataFim) {
    let numLinhas = tabela.find("tbody tr").length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = tabela.find("tbody");
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = tabela.find("tr:last");
    }
    else {
        linhaReferenciaAtual = $(tabela.find("tr")[linha]);
    }

    let codigoLinha = '<tr>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataInicio dataConsultor"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataFim dataConsultor"/></td>'
        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Status_Consultor(this)" class="btnMinus_Banco" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Status_Consultor($(this).parents(\'.tableStatusConsultor\'))" class="btnPlus_Banco" /></td>'
        + '</tr>';

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next("tr");
    }


    if (dataInicio != undefined) {
        linhaNova.find(".txtDataInicio").val(dataInicio);
    }

    if (dataFim != undefined) {
        linhaNova.find(".txtDataFim").val(dataFim);
    }

    Atualiza_Triggers();
}


function Adiciona_Linha_Despesas(despesa) {
    let numLinhas = $('#tableDespesas tr:not(.deletar).linhaDespesa').length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = $('#tableDespesas tbody');
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = $('#tableDespesas tr:not(.deletar).linhaDespesa:last');
    }
    else {
        linhaReferenciaAtual = $($('#tableDespesas tr:not(.deletar).linhaDespesa')[linha]);
    }
    let codigoLinha = '<tr class="linhaDespesa">'
        + '<td class="codDespesaJob"></td>'
        + '<td class="line"><select class="cboDespesa">' + comboDespesas + '</select></td>'
        + '<td style="width:2%;text-align:center;"><input type="checkbox" class="checkCobrarCliente"> </td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="5" class="txtValorLimite valor"/></td>'
        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Despesas(this)" class="btnMinus_Banco" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Despesas(undefined)" class="btnPlus_Banco" /></td>'
        + '</tr>'

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next(".linhaDespesa");
    }

    Atualiza_Combos();


    if (despesa != undefined) {
        Preenche_Linha_Despesa(linhaNova, despesa);
    }
    else {
        $(linhaNova).find('.cboDespesa').val("");
        $(linhaNova).find('.codDespesaJob').val("0");
    }

    Atualiza_Controles();
    Atualiza_Campos_Data();
    Atualiza_Campos_Valor();

    return linhaNova;
}

function Adiciona_Linha_Valor_Despesa(tabela, valorDespesa) {
    let numLinhas = tabela.find("tbody tr").length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;

    if (numLinhas == 0) {
        linhaReferenciaAtual = tabela.find("tbody");
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = tabela.find("tr:last");
    }
    else {
        linhaReferenciaAtual = $(tabela.find("tr")[linha]);
    }



    let codigoLinha = '<tr>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataInicio data"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="10" class="txtDataFim data"/></td>'
        + '<td class="line" style="width:fit-content"><input type="text" size="5" class="txtValorLimite valor"/></td>'
        + '<td class="line tdREMOVE"><input type="button" value="-" onclick="Remove_Linha_Valor_Despesa(this)" class="btnMinus" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="Adiciona_Linha_Valor_Despesa($(this).parents(\'.tableLinhaValorDespesa\'))" class="btnPlus" /></td>'
        + '</tr>';

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next("tr");
    }

    if (valorDespesa != undefined) {
        Preenche_Linha_Despesa(linhaNova, despesa);
    }
    else {
    }

    Atualiza_Controles();
    Atualiza_Campos_Data();
    Atualiza_Campos_Valor();

}



function Atualiza_Triggers() {
    $(".data").change(function () {
        changeStatus(this);
    });

    $(".dataConsultor").change(function () {
        changeStatusConsultor(this);
    });

    $(".data").trigger("change");
    $(".dataConsultor").trigger("change");
}

function changeStatus(e) {
    let linha = $(e).parents("tr");

    let dataInicioPreenchida = linha.find(".txtDataInicio").val() != "";
    let dataFimPreenchida = linha.find(".txtDataFim").val() != "";

    if (dataInicioPreenchida && dataFimPreenchida)
        linha.find(".txtStatus").html("Inativo");
    else if (dataInicioPreenchida && !dataFimPreenchida)
        linha.find(".txtStatus").html("Ativo");
    else
        linha.find(".txtStatus").html("-");

}

function changeStatusConsultor(e) {
    let linha = $(e).parents(".linhaConsultor");

    let dataInicioPreenchida = linha.find(".txtDataInicio").val() != "";
    let dataFimPreenchida = linha.find(".txtDataFim").val() != "";

    if (dataInicioPreenchida && dataFimPreenchida)
        linha.find(".txtStatus").html("(Inativo)");
    else if (dataInicioPreenchida && !dataFimPreenchida)
        linha.find(".txtStatus").html("(Ativo)");
    else
        linha.find(".txtStatus").html("(-)");
}

function Atualiza_Controles() {
    if ($('#tableGestores tbody tr').length == 1)
        $('#tableGestores tbody tr .tdREMOVE').first().css("display", "none");
    else
        $('#tableGestores tbody tr .tdREMOVE').css("display", "");

    if ($('#tableConsultores tbody tr.linhaConsultor').length == 1)
        $('#tableConsultores tbody tr.linhaConsultor .tdREMOVE').first().css("display", "none");
    else
        $('#tableConsultores tbody tr.linhaConsultor .tdREMOVE').css("display", "");

    if ($('#tableDespesas tbody tr.linhaDespesa').length == 1)
        $('#tableDespesas tbody tr.linhaDespesa .tdREMOVE').first().css("display", "none");
    else
        $('#tableDespesas tbody tr.linhaDespesa .tdREMOVE').css("display", "");
}

function Atualiza_Campos_Data() {
    $(".data").mask("99/99/9999", {placeholder: "__/__/____", reverse:true});
}

function Atualiza_Campos_Valor() {
    $(".valor").mask("#0,00", { placeholder: "_,__", reverse:true });
}

function Preenche_Linha_Gestor(linha, gestorJob) {
    let dataInicio = new Date(gestorJob.DataInicio);
    let dataFim = new Date(gestorJob.DataFim);

    dataInicio = dataInicio.getFullYear() == 1900 ? "" : dataInicio.getDate().toString().padStart(2, "0") + "/" + (dataInicio.getMonth() + 1).toString().padStart(2, "0") + "/" + dataInicio.getFullYear();
    dataFim = dataFim.getFullYear() == 9999 ? "" : dataFim.getDate().toString().padStart(2, "0") + "/" + (dataFim.getMonth() + 1).toString().padStart(2, "0") + "/" + dataFim.getFullYear();

    linha.find('.cboGestores').val(gestorJob.CodGestor);
    linha.find('.txtDataInicio').val(dataInicio);
    linha.find('.txtDataFim').val(dataFim);
    linha.find('.codGestorJob').val(gestorJob.CodGestorJob);

    linha.find('select').trigger('chosen:updated');
}

function Preenche_Linha_Consultor(linha, consultorJob) {
    let dataInicio = new Date(consultorJob.DataInicio);
    let dataFim = new Date(consultorJob.DataFim);

    dataInicio = dataInicio.getFullYear() == 1900 ? "" : dataInicio.getDate().toString().padStart(2, "0") + "/" + (dataInicio.getMonth() + 1).toString().padStart(2, "0") + "/" + dataInicio.getFullYear();
    dataFim = dataFim.getFullYear() == 9999 ? "" : dataFim.getDate().toString().padStart(2, "0") + "/" + (dataFim.getMonth() + 1).toString().padStart(2, "0") + "/" + dataFim.getFullYear();

    linha.find('.cboConsultor').val(consultorJob.CodConsultor);
    linha.find('.cboAprovadorHora').val(consultorJob.CodAprovador);
    linha.find('.cboAprovadorRDV').val(consultorJob.CodAprovadorRDV);
    linha.find('.checkEmail').prop("checked", consultorJob.EnviarEmailAprovador);
    linha.find('.txtContato').val(consultorJob.ContatoAlocacao);
    linha.find('.txtEmail').val(consultorJob.EmailContato);
    linha.find('.txtTelefone').val(consultorJob.TelefoneContato);
    linha.find('.txtTaxa').val(consultorJob.TaxaConsultor.toString().replace(".", ","));
    linha.find('.txtCustoIntraDivisao').val(consultorJob.CustoIntraDivisao.toString().replace(".", ","));

    linha.find('.txtDataInicio').val(dataInicio);
    linha.find('.txtDataFim').val(dataFim);

    linha.find('.codConsultorJob').val(consultorJob.CodConsultorJob);


    linha.find('select').trigger('chosen:updated');
}

function Preenche_Linha_Despesa(linha, despesa) {
    linha.find(".cboDespesa").val(despesa.CodDespesa);
    linha.find(".checkCobrarCliente").prop("checked", despesa.CobrarCliente);
    linha.find(".txtValorLimite").val(despesa.ValorLimite);
    linha.find(".codDespesaJob").val(despesa.CodDespesaJob);
}


function Remove_Linha_Gestor(linha) {
    if ($('#tableGestores tbody tr:not(.deletar)').length > 1) {
        $(linha).closest("tr").addClass("deletar");
    }

    Atualiza_Controles();
}

function Remove_Linha_Consultor(linha) {
    if ($('#tableConsultores tbody tr:not(.deletar).linhaConsultor').length > 1) {
        $(linha).closest('.linhaConsultor').addClass("deletar");
    }

    Atualiza_Controles();
}


function Remove_Linha_Despesas(linha) {
    if ($('#tableDespesas tbody tr:not(.deletar).linhaDespesa').length > 1) {
        $(linha).closest('.linhaDespesa').addClass("deletar");
    }

    Atualiza_Controles();
}

function Limpa_Linha_Gestor(linha) {
    $(linha).find('.cboGestores').val("");

    $(linha).find('select').trigger('chosen:updated');
}

function Limpa_Linha_Consultor(linha) {
    linha.find('.cboConsultor').val("");
    linha.find('.cboAprovador').val("");
    linha.find('.cboAprovadorRDV').val("");
    linha.find('.checkEmail').prop("checked", false);
    linha.find('.txtContato').val("");
    linha.find('.txtEmail').val("");
    linha.find('.txtTelefone').val("");
    linha.find('.txtTaxa').val("");
    linha.find('.txtCustoIntraDivisao').val("");

    linha.find('.txtDataInicio').val("");
    linha.find('.txtDataFim').val("");

    linha.find('.codConsultorJob').val("");


    $(linha).find('select').trigger('chosen:updated');
}

function Limpa_Linha_Despesa(linha) {
    linha.find(".cboDespesa").val("");
    linha.find(".checkCobrarCliente").prop("checked", false);
    linha.find(".txtValorLimite").val(despesa.ValorLimite);
    linha.find(".codDespesaJob").val(despesa.CodDespesaJob);

    $(linha).find('select').trigger('chosen:updated');
}

function Limpa_Linha_Valor_Despesa(linha) {
    $(linha).find('.cboDespesa').val("");

    $(linha).find('select').trigger('chosen:updated');
}



async function Salva_Gestores() {
    let listaGestoresNovo = [];
    let listaGestoresDeletar = [];
    let erros = [];
    let contGestor = 0;
    $('#tableGestores tbody tr:not(.deletar)').each(function () {
        contGestor = contGestor + 1;
        let sucesso = true;
        let preenchido = false;

        let codGestorJob = Number($(this).find('.codGestorJob').val());
        let codGestor = $(this).find('td .cboGestores').val();
        let dataInicio = $(this).find('td .txtDataInicio').val();
        let dataFim = $(this).find('td .txtDataFim').val();

        let errosLinha = [];

        if (codGestor == undefined || codGestor == "" || codGestor == "0") {
            errosLinha.push("Informe o Gestor");
            sucesso = false;
        }
        //else if (listaGestores.find(codGestor) == undefined) {
        //    errosLinha.push("Gestor Inválido");
        //    sucesso = false;
        //    preenchido = true;
        //}
        else {
            preenchido = true;
		}

        if (dataInicio == undefined || dataInicio == "") {
            errosLinha.push("Informe a Data Inicio do Gestor");
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

        if (dataFim != undefined && dataFim != "") {
            preenchido = true;
            let parseDataFim = dataFim.split("/");
            let objDataFim = new Date(parseDataFim[2], parseDataFim[1] - 1, parseDataFim[0]);

            if (parseDataFim.length != 3 || objDataFim == "Invalid Date" || objDataFim.getDate() != parseDataFim[0] || objDataFim.getMonth() != parseDataFim[0] - 1 || objDataFim.getFullYear() != parseDataFim[2]) {
                errosLinha.push("Data Fim Inválida");
                sucesso = false;
            }
            else {
                dataFim = objDataFim;
			}
        }
        else {
            dataFim = new Date("9999-12-31T00:00:00");
        }

        if (dataInicio instanceof Date && dataFim instanceof Date) {
            if (dataFim < dataInicio) {
                errosLinha.push("A Data Fim do Gestor deve ser posterior ao Inicio");
                sucesso = false;
			}
		}

        if (sucesso) {
            listaGestoresNovo.push(
                {
                    CodGestorJob: codGestorJob,
                    CodGestor: codGestor,
                    DataInicio: dataInicio,
                    DataFim: dataFim
                }
            );
        }
        else if (preenchido) {
            erros.push("Gestor " + contGestor + ":\n");
            erros.push(...errosLinha);
		}
    });

    if (erros.length > 0) {
        alert(erros.join("\n"));
        return false;
    }

    $('#tableGestores tbody tr.deletar').each(function () {
        let codGestorJob = Number($(this).find('.codGestorJob').val());
        if (codGestorJob != undefined && !isNaN(codGestorJob) && codGestorJob != 0)
            listaGestoresDeletar.push(codGestorJob);

    });

    $("#" + prefix + 'hdGestorJob').val(JSON.stringify(listaGestoresNovo));
    $("#" + prefix + 'hdGestorJobDeletar').val(JSON.stringify(listaGestoresDeletar.filter((x, i) => listaGestoresDeletar.indexOf(x) === i)));

    return true;
}

async function Salva_Consultores() {
    let listaConsultoresNovo = [];
    let listaConsultoresDeletar = [];
    let erros = [];
    let contConsultor = 0
    $('#tableConsultores tr:not(.deletar).linhaConsultor').each(function () {
        contConsultor = contConsultor + 1;
        let sucesso = true;
        let preenchido = false;

        let codConsultorJob = $(this).find('.codConsultorJob').val();
        let codConsultor = $(this).find('td .cboConsultor').val();
        let codAprovador = $(this).find('td .cboAprovadorHora').val();
        let codAprovadorRDV = $(this).find('td .cboAprovadorRDV').val();
        let checkEmail = $(this).find('td .checkEmail').is(":checked");
        let txtContato = $(this).find('td .txtContato').val();
        let txtEmail = $(this).find('td .txtEmail').val();
        let txtTelefone = $(this).find('td .txtTelefone').val();
        let txtTaxa = $(this).find('td .txtTaxa').val();
        let txtCustoIntradivisao = $(this).find('td .txtCustoIntraDivisao').val();
        let dataInicio = $(this).find('td .txtDataInicio').val();
        let dataFim = $(this).find('td .txtDataFim').val();

        let errosLinha = [];

        if (codConsultorJob == undefined || codConsultorJob == "")
            codConsultorJob = "0";

        if (codConsultor == undefined || codConsultor == "" || codConsultor == "0") {
            errosLinha.push("Informe o Consultor");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (codAprovador == undefined || codAprovador == "" || codAprovador == "0") {
            errosLinha.push("Informe o Aprovador");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (codAprovadorRDV == undefined || codAprovadorRDV == "" || codAprovadorRDV == "0") {
            errosLinha.push("Informe o Aprovador RDV");
            sucesso = false;
        }
        else{
            preenchido = true;
        }

        if (codConsultor == undefined || codConsultor == "" || codConsultor == "0") {
            if (codAprovador == codConsultor)
                errosLinha.push("O Consultor não pode ser o próprio Aprovador");
            if (codAprovadorRDV == codConsultor)
                errosLinha.push("O Consultor não pode ser o próprio Aprovador RDV");
        }

        if (checkEmail)
            preenchido = true;

        checkEmail = checkEmail;

        if (txtContato != undefined && txtContato != "")
            preenchido = true;

        if (txtEmail != undefined && txtEmail != "")
            preenchido = true;

        if (txtTelefone != undefined && txtTelefone != "")
            preenchido = true;

        if (txtTaxa != undefined && txtTaxa != "") {
            preenchido = true;
            txtTaxa = Number(txtTaxa.replace(",", "."))

            if (isNaN(txtTaxa) || txtTaxa < 0) {
                errosLinha.push("Taxa Consultor Inválida");
                sucesso = false;
            }
        }
        else {
            txtTaxa = Number("0");
		}
        if (txtCustoIntradivisao != undefined && txtCustoIntradivisao != "") {
            preenchido = true;
            txtCustoIntradivisao = Number(txtCustoIntradivisao.replace(",", "."))

            if (isNaN(txtCustoIntradivisao) || txtCustoIntradivisao < 0) {
                errosLinha.push("Valor do Custo Intra Divisão Inválido");
                sucesso = false;
            }
        }
        else {
            txtCustoIntradivisao = Number("0");
		}

        if (dataInicio == undefined || dataInicio == "") {
            errosLinha.push("Informe a Data Inicio do Consultor");
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

        if (dataFim != undefined && dataFim != "") {
            preenchido = true;
            let parseDataFim = dataFim.split("/");
            let objDataFim = new Date(dataFim[2], dataFim[1] - 1, dataFim[0]);

            if (parseDataFim.length != 3 || objDataFim == "Invalid Date" || objDataFim.getDate() != parseDataFim[0] || objDataFim.getMonth() != parseDataFim[1] - 1 || objDataFim.getFullYear() != parseDataFim[2]) {
                errosLinha.push("Data Fim Inválida");
                sucesso = false;
            }
            else {
                dataFim = objDataFim;
            }
        }
        else {
            dataFim = new Date("9999-12-31T00:00:00");
		}

        if (dataInicio instanceof Date && dataFim instanceof Date) {
            if (dataFim < dataInicio) {
                errosLinha.push("A Data Fim do Consultor deve ser posterior ao Inicio");
                sucesso = false;
            }
        }

        if (sucesso) {
            listaConsultoresNovo.push(
                {
                    CodConsultorJob: codConsultorJob,
                    CodConsultor : codConsultor,
                    CodAprovador : codAprovador,
                    CodAprovadorRDV : codAprovadorRDV,
                    EnviarEmailAprovador : checkEmail,
                    ContatoAlocacao : txtContato,
                    TelefoneContato : txtTelefone,
                    EmailContato : txtEmail,
                    TaxaConsultor : txtTaxa,
                    CustoIntraDivisao : txtCustoIntradivisao,
                    DataInicio : dataInicio,
                    DataFim : dataFim
                }
            );
        }
		else if (preenchido) {
			erros.push("Consultor " + contConsultor + " :\n")
			erros.push(...errosLinha);
		}
    });

    if (erros.length > 0) {
        alert(erros.join("\n"));
        return false;
    }

    $('#tableConsultores tbody tr.deletar').each(function () {
        let codConsultorJob = Number($(this).find('.codConsultorJob').val());
        if (codConsultorJob != undefined && !isNaN(codConsultorJob) && codConsultorJob != 0)
            listaConsultoresDeletar.push(codConsultorJob);

    });

    $("#" + prefix + 'hdConsultorJob').val(JSON.stringify(listaConsultoresNovo));
    $("#" + prefix + 'hdConsultorJobDeletar').val(JSON.stringify(listaConsultoresDeletar.filter((x, i) => listaConsultoresDeletar.indexOf(x) === i)));


    return true;
}

async function Salva_Despesas() {
    let listaDespesasNovo = [];
    let listaDespesasDeletar = [];
    let erros = [];
    let contDespesa = 0

    $("#tableDespesas tr:not(.deletar).linhaDespesa").each(function () {
        contDespesa += 1;
        let sucesso = true;
        let preenchido = false;

        let codDespesaJob = $(this).find(".codDespesaJob").val();
        let codDespesa = $(this).find("td .cboDespesa").val();
        let checkCobrarCliente = $(this).find("td .checkCobrarCliente").is(":checked");
        let valorLimite = $(this).find("td .txtValorLimite").val();

        let errosLinha = [];

        if (codDespesaJob == undefined || codDespesaJob == "")
            codDespesaJob = "0";

        if (codDespesa == undefined || codDespesa == "") {
            errosLinha.push("Informe o Tipo de Despesa");
            sucesso = false;
        }
        else
            preenchido = true;

        if (checkCobrarCliente)
            preenchido = true;

        checkCobrarCliente = checkCobrarCliente;

        if (valorLimite == undefined || valorLimite == "") {
            errosLinha.push("Informe o Valor Limite");
            sucesso = false;
        }
        else {
            valorLimite = Number(valorLimite.replace(",", "."));
            if (isNaN(valorLimite) || valorLimite <= 0) {
                errosLinha.push("Valor Limite deve ser maior que 0");
            }
        }

        if (sucesso) {
            listaDespesasNovo.push({
                CodDespesaJob: codDespesaJob,
                CodDespesa : codDespesa,
                CobrarCliente: checkCobrarCliente,
                ValorLimite: valorLimite
            });
        }
        else if (preenchido) {
            erros.push("Despesa " + contDespesa + " :\n");
            erros.push(...errosLinha);
		}

    });


    if (erros.length > 0) {
        alert(erros.join("\n"));
        return false;
    }

    $('#tableDespesas tbody tr.deletar').each(function () {
        let codDespesaJob = Number($(this).find('.codDespesaJob').val());
        if (codDespesaJob != undefined && !isNaN(codDespesaJob) && codDespesaJob != 0)
            listaDespesasDeletar.push(codDespesaJob);

    });

    $("#" + prefix + 'hdDespesaJob').val(JSON.stringify(listaDespesasNovo));
    $("#" + prefix + 'hdDespesaJobDeletar').val(JSON.stringify(listaDespesasDeletar.filter((x, i) => listaDespesasDeletar.indexOf(x) === i)));



    return true;
}

function validaPeriodo(dataInicio, dataFim, erros) {
    if (!erros instanceof Array)
        return;

    if (dataInicio == undefined || dataInicio == "") {
        erros.push("Informe a Data Inicio");
        sucesso = false;
    }
    else {
        preenchido = true;
        let parseDataInicio = dataInicio.split("/");
        let objDataInicio = new Date(dataInicio[2], dataInicio[1] - 1, dataInicio[0]);

        if (parseDataInicio.length != 3 || objDataInicio == "Invalid Date" || objDataInicio.getDate() != parseDataInicio[0] || objDataInicio.getMonth() != parseDataInicio[1] - 1 || objDataInicio.getFullYear() != parseDataInicio[2]) {
            erros.push("Data Inicio Inválida");
        }
        else {
            dataInicio = objDataInicio;
        }
    }

    if (dataFim != undefined && dataFim != "") {
        preenchido = true;
        let parseDataFim = dataFim.split("/");
        let objDataFim = new Date(dataFim[2], dataFim[1] - 1, dataFim[0]);

        if (parseDataFim.length != 3 || objDataFim == "Invalid Date" || objDataFim.getDate() != parseDataFim[0] || objDataFim.getMonth() != parseDataFim[1] - 1 || objDataFim.getFullYear() != parseDataFim[2]) {
            erros.push("Data Inicio Inválida");
        }
        else {
            dataFim = objDataFim;
        }
    }

    if (dataInicio instanceof Date && dataFim instanceof Date) {
        if (dataFim < dataInicio) {
            erros.push("A Data Fim deve ser posterior ao Inicio");
        }
    }


}

function toggleStatusConsultor(e) {
    $(e).closest("tr").find(".tableStatusConsultor").toggle();
}

function toggleValorDespesa(e) {
    $(e).closest("tbody tr td").find(".tableLinhaValorDespesa").toggle();
}


function toggleGestores() {
    let statusTabela = $(".divGestores").css("display") == "none" ? true : false;

    $(".divGestores").css("display", statusTabela ? "" : "none");
    $(".btnGestores").html(statusTabela ? "Esconder" : "Mostrar");
}

function ajaxGetGestoresJob() {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/getGestoresJob",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == undefined || r.d == "")
                return;
            listaGestores = JSON.parse(r.d);
        },
        error: OnFailed
    });
}

async function ajaxSalvaGestoresJob(listaGestoresNovo) {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/salvaGestoresJob",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{gestoresJob: '" + JSON.stringify(listaGestoresNovo) + "'}",
        error: OnFailed
    });
}

function ajaxGetConsultoresJob() {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/getConsultoresJob",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == undefined || r.d == "")
                return;
            listaConsultores = JSON.parse(r.d);
        },
        error: OnFailed
    });
}

async function ajaxSalvaConsultoresJob(listaConsultoresNovo) {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/salvaConsultoresJob",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{consultoresJob: '" + JSON.stringify(listaConsultoresNovo) + "'}",
        error: OnFailed
    });
}

function ajaxGetDespesasJob() {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/getDespesasJob",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == undefined || r.d == "")
                return;
            listaDespesas = JSON.parse(r.d);
        },
        error: OnFailed
    });
}

async function ajaxSalvaDespesasJob(listaDespesasNovo) {
    return $.ajax({
        type: "POST",
        url: "FormEditCadJobs.aspx/salvaDespesasJob",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{despesasJob: '" + JSON.stringify(listaDespesasNovo) + "'}",
        error: OnFailed
    });
}
function OnFailed(ajax, msg) {
    var msgUrl = "Erro ao chamar o método: " + this.url + " \r\n ";
    var msgDados = "Dados para chamar o método: " + this.data + " \r\n ";
    alert(msgUrl + msgDados + ajax.responseText.replace('\'', ''));
}

function mudaTela() {
    if ($(this).hasClass("ativo"))
        return;

    let telaAtual = $("th.ativo");
    telaAtual.removeClass("ativo");

    $("div." + telaAtual.attr("class")).css("display", "none")
    $("div." + $(this).attr("class")).css("display", "");

    $(this).addClass("ativo");
}

function Toggle_Detalhes_Consultor(element) {
    $(element).closest("table").find(".linhaDetalhe").toggle();
}
