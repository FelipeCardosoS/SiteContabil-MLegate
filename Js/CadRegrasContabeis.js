var prefix = "ctl00_areaForm_";
var listaContas = [];
var listaTerceiros = [];
var listaRegras = [];
var regexNumbers = new RegExp('([^0-9])+', 'g');

var comboContas = '';
var comboTerceiros = '';

$(document).ready(function () {
    Extrai_Contas();
    Extrai_Terceiros();
    Extrai_Regras();
    Prepara_Tabela_Regras();
    Atualiza_Combos();
    Atualiza_Triggers();
    //$("#ctl00_botaoSalvar").click(function () {
    //    return Salva_Regras();
    //});

    $("#ctl00_botaoSalvar").on("click", function (e) {
        if (Salva_Regras())
            $("#ctl00_botaoSalvarHidden").click();
    });
    $("#ctl00_botaoSalvar")[0].onclick = "";
});


function Atualiza_Combos() {
    $('.cboConta').chosen({ search_contains: true, no_results_text: "Nenhum resultado encontrado!" });
    $('.cboTipoValor').chosen({ width: "100%", search_contains: true, no_results_text: "Nenhum resultado encontrado!" });
    $('.cboTerceiro').chosen({ width: "100%", search_contains: true, no_results_text: "Nenhum resultado encontrado!" });
}

function Atualiza_Triggers() {
    $('.cboFinalidade').change(function (event) { Change_Finalidade(event) });
    $('.txtValor').keypress(function (event) { return Onlynumbers(event) });
    $('.txtValor').on("paste", function (event) { return Paste_Event(event) });
    $('.txtDiaVencimento').keypress(function (event) { return Onlynumbers(event) });
    $('.txtDiaVencimento').on("paste", function (event) { return Paste_Event(event) });
}

function Paste_Event(e) {
    let text = e.originalEvent.clipboardData.getData("Text");

    e.stopPropagation();
    e.preventDefault();

    e.target.value = text.replaceAll(regexNumbers, '');

}

function Evento_Salvar() {
    let validacaoTabela = Salva_Regras();
    let validacaoControles = Page_ClientValidate();
    return validacaoControles && validacaoTabela;
}

function Extrai_Contas() {
    let jsonListaContas = $("#" + prefix + 'hdContas').val();

    if (jsonListaContas == undefined || jsonListaContas == "")
        return;

    listaContas = JSON.parse(jsonListaContas);

    comboContas += '<option value=""> <option/>';
    listaContas.forEach(function (conta) {
        comboContas += '<option value="' + conta.codigo + '"> ' + conta.descricao + ' <option/> ';
    });
}

function Extrai_Terceiros() {
    let jsonListaTerceiros = $("#" + prefix + 'hdTerceiros').val();

    if (jsonListaTerceiros == undefined || jsonListaTerceiros == "")
        return;

    listaTerceiros = JSON.parse(jsonListaTerceiros);

    comboTerceiros += '<option value=""> <option/>';
    listaTerceiros.forEach(function (terceiro) {
        comboTerceiros += '<option value="' + terceiro.codigo + '"> ' + terceiro.nome + ' <option/> ';
    });

}

function Extrai_Regras() {
    let jsonListaRegras = $("#" + prefix + 'hdRegras').val();

    if (jsonListaRegras == undefined || jsonListaRegras == "")
        return;

    listaRegras = JSON.parse(jsonListaRegras);
}

function Prepara_Tabela_Regras() {
    Inicializa_Tabela();
    listaRegras.forEach(function (regra) {
        if (regra == undefined) return;
        else if (!regra.TipoRegra.endsWith("DESCONTO")) return;

        Adiciona_Linha_Regra(regra);
    });
}

function Inicializa_Tabela() {
    $("#tableRegras tbody .tipoRegra").val(0);
}

function Click_Default(event) {
    let valor = event.target.checked;
    let linha = event.target.parentElement.parentElement;
    $(linha).find("td .cboTerceiro").prop("disabled", !valor);
    
    if (!valor)
    {
        $(linha).find("td .cboTerceiro").val("");
    }

    $(linha).find("td .cboTerceiro").trigger("chosen:updated")
}   

function Adiciona_Linha_Regra(regra) {
    let numLinhas = $('#tableRegras tbody tr:not(.deletar)').length;
    let linhaReferenciaAtual = undefined;
    let linha = event == undefined ? -1 : event.target.parentElement.parentElement.rowIndex;
    
    if (numLinhas == 0) {
        linhaReferenciaAtual = $('#tableRegras tbody');
    }
    else if (linha == -1 || linha >= numLinhas) {
        linhaReferenciaAtual = $('#tableRegras tbody tr:not(.deletar):last');
    }
    else {
        linhaReferenciaAtual = $($('#tableRegras tbody tr:not(.deletar)')[linha-1]);
	}
    let codigoLinha = '<tr>'
        + '<td class="line tipoRegra" style="display:none"></td>'
        + '<td class="line codRegraContabil" style="display:none"></td>'
        + '<td class="line nome"></td>'
        + '<td class="line"><select class="cboConta"/></td>'
        + '<td class="line"><input type="text" class="txtValor" style="width:60px"/></td>'
        + '<td class="line"><select class="cboTipoValor"><option value="">Escolha...</option><option value="%">%</option><option value="R$">R$</option></select></td>'
        + '<td class="line"><input type="text" maxlength="2" class="txtDiaVencimento" style="width:30px"/></td>'
        + '<td class="line"><select class="cboTerceiro"/></td>'
        + '<td class="line"><input type="text" class="txtHistorico" style="width:60px" /></td>'
        + '<td class="line" style="text-align:center"><input type="checkbox" class="checkTitulo" onclick="Click_Default(event)"/></td>'
        + '<td class="line" ' + (numLinhas < 2 ? 'style="display:none"' : "") + '><input type="button" value="-" onclick="Remove_Linha(this)" class="btnMinus_Banco" /></td>'
        + '<td class="line tdADD" ' + (numLinhas < 2 ? 'style="display:none"' : "") + '><input type="button" value="+" onclick="Adiciona_Linha_Regra(undefined, this.rowIndex)" class="btnPlus_Banco" /></td>'
        + '</tr>';

    if (numLinhas == 0) {
        linhaReferenciaAtual.html(codigoLinha);
        linhaNova = linhaReferenciaAtual.children();
    }
    else {
        linhaReferenciaAtual.after(codigoLinha);
        linhaNova = linhaReferenciaAtual.next("tr");
	}


    $(linhaNova).find('.cboConta').html(comboContas);
    $(linhaNova).find('.cboTerceiro').html(comboTerceiros);
    $(linhaNova).find('.codRegraContabil').val(0);
    $(linhaNova).find('.cboTerceiro').prop("disabled", true);

    Atualiza_Combos();
    Atualiza_Triggers();

    if (regra != undefined)
        Preenche_Linha(linhaNova, regra)
    else {
        $(linhaNova).find('.cboConta').val("");
        $(linhaNova).find('.cboTerceiro').val("");
	}
    Atualiza_Nomes();

    return linhaNova;
}
function Atualiza_Nomes() {
    let nomes = $('#tableRegras tbody tr:not(.deletar) .nome');

    if (nomes.length <= 2)
        return;

    nomes.each(function (index) {
        if (index <= 1) return;
        $(this).html('Outro ' + String(index - 1));
	})
}

function Preenche_Linha(linha, regra) {
    //let regraZerada = regra.CodRegraContabil == 0;
    linha.find('.nome').html(regra.Nome);
    linha.find('.cboConta').val(regra.CodConta);
    linha.find('.txtValor').val(regra.Valor);
    linha.find('.cboTipoValor').val(regra.TipoValor);
    linha.find('.txtDiaVencimento').val(regra.DiaVencimento);
    linha.find('.cboTerceiro').val(regra.CodTerceiro);
    linha.find('.txtHistorico').val(regra.Historico);
    linha.find('.checkTitulo').prop('checked', regra.Titulo);
    linha.find('.tipoRegra').html(regra.TipoRegra);
    linha.find('.codRegraContabil').val(regra.CodRegraContabil);

    linha.find('select').trigger('chosen:updated');
    linha.find('.checkTitulo').change();
}

function Remove_Linha(Linha) {
    let index = event.target.parentElement.parentElement.rowIndex;

    if ($('#tableRegras tbody tr:not(.deletar)').length > 3) {
        $(Linha).closest('tr').addClass("deletar");
    }

    Atualiza_Nomes();
}

function Limpa_Linha(linha)
{
    linha.find('.nome').val("");
    linha.find('.cboConta').val("");
    linha.find('.txtValor').val("");
    linha.find('.cboTipoValor').val("");
    linha.find('.txtDiaVencimento').val("");
    linha.find('.cboTerceiro').val("");
    linha.find('.txtHistorico').val("");
    linha.find('.checkTitulo').prop('checked', false);
    linha.find('.tipoRegra').val("OUTROS_DESCONTO");

    linha.find('select').trigger('chosen:updated');
    linha.find('.checkTitulo').change();
}

function Salva_Regras() {
    let listaRegrasNovas = [];
    let listaRegrasDeletar = [];
    let erros = [];

    $('#tableRegras tbody tr:not(.deletar)').each(function () {
        let sucesso = true;
        let preenchido = false;

        let codRegraContabil = parseInt($(this).find('.codRegraContabil').val());
        let nome = $(this).find('.nome').html();
        let conta = $(this).find('td .cboConta').val();
        let valor = parseInt($(this).find('td .txtValor').val());
        let tipoValor = $(this).find('td .cboTipoValor').val();
        let diaVencimento = parseInt($(this).find('td .txtDiaVencimento').val());
        let terceiro = parseInt($(this).find('td .cboTerceiro').val());
        let historico = $(this).find('td .txtHistorico').val();
        let titulo = $(this).find('td .checkTitulo').prop('checked');
        let tipoRegra = $(this).find('.tipoRegra').html();

        let errosLinha = [];
        let nomeDefault = !nome.startsWith("Outro");
        
        if (conta == "" || conta == "0") {
            errosLinha.push("Insira o Campo Conta");
            sucesso = false;
        }
        else {
            preenchido = true;
		}

        if (valor == "" || isNaN(valor)) {
            valor = 0;
            errosLinha.push("Defina o Valor do Desconto");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (tipoValor == "") {
            errosLinha.push("Informe o Tipo do Valor");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (diaVencimento == 0 || isNaN(diaVencimento)) {
            diaVencimento = 0;
            errosLinha.push("Informe o Dia do Vencimento");
            sucesso = false;
        }
        else if (diaVencimento > 31) {
            errosLinha.push("Dia do Vencimento Inválido");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (titulo && (isNaN(terceiro) || terceiro == 0)) {
            terceiro = 0;
            errosLinha.push("Informe o Terceiro");
            sucesso = false;
            preenchido = true;
        }
        else if (!titulo)
            terceiro = 0;

        if (historico == "") {
            errosLinha.push("Informe o Histórico");
            sucesso = false;
        }
        else {
            preenchido = true;
        }

        if (preenchido && !sucesso) {
            erros.push.apply(erros, errosLinha);
            return false;
        }

        else if (!preenchido) {
            //Linha Vazia
            if (codRegraContabil != 0)
                listaRegrasDeletar.push(codRegraContabil);
            return true;
		}

        listaRegrasNovas.push({
            CodRegraContabil: codRegraContabil,
            TipoRegra: tipoRegra,
            Nome: nome,
            CodConta: conta,
            Valor: valor,
            TipoValor: tipoValor,
            DiaVencimento: diaVencimento,
            CodTerceiro: terceiro,
            Historico: historico,
            Titulo: titulo
        });
    });

    if (erros.length > 0) {
        alert(erros.join("\n"));
        return false;
    }

    $('#tableRegras tbody tr.deletar').each(function () {
        let codRegraContabil = Number($(this).find('.codRegraContabil').val());
        if (codRegraContabil != undefined && !isNaN(codRegraContabil) && codRegraContabil != 0)
            listaRegrasDeletar.push(codRegraContabil);
    });


    $("#" + prefix + 'hdRegrasNovo').val(JSON.stringify(listaRegrasNovas));
    $("#" + prefix + 'hdRegrasDeletar').val(JSON.stringify(listaRegrasDeletar.filter((x, i) => listaRegrasDeletar.indexOf(x) === i)));

    return true;
}
function ajaxSalvaRegras(listaRegrasNovas) {
    return $.ajax({
        type: "POST",
        url: "FormEditCadRegrasContabeis.aspx/salvaRegras",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{regras:'" + JSON.stringify(listaRegrasNovas) + "'}",
        error: OnFailed
    });
}
function OnFailed(ajax, msg) {
    var msgUrl = "Erro ao chamar o método: " + this.url + " \r\n ";
    var msgDados = "Dados para chamar o método: " + this.data + " \r\n ";
    alert(msgUrl + msgDados + ajax.responseText.replace('\'', ''));
}