function selecionaAte(combo, irmao) {
    __doPostBack(combo, irmao);
}

function abreRelatorioRazao(url) {
    if (url != null) {
        if (url != "") {
            var confirmacao = window.confirm("Você deseja abrir o Razão Contábil?");
            if (confirmacao) {
                popupRazao(url + "&tipo=CONTABIL", 0, 0);
            } else {
                popupRazao(url + "&tipo=DETALHADO", 0, 0);
            }
        }
    }
}

function ativaCombosFilhos(combo, nivel, prefixo) {
    var max = 4;
    if ($(combo).val() == "0") {
        if (nivel < 4) {
            for (var i = nivel; i <= max; i++) {
                if (i != nivel) {
                    var idFilho = "#ctl00_areaConteudo_comboDetalhamento" + i + prefixo;

                    $(idFilho).attr("disabled", "disabled");
                    $(idFilho).val("0");
                }
            }
        }
    } else {
        var idFilho = "#ctl00_areaConteudo_comboDetalhamento" + (nivel + 1) + prefixo;
        var opcoes = $(idFilho + " option");

        $(idFilho).removeAttr("disabled");
        $(idFilho).val("0");


        for (var i = 0; i < opcoes.length; i++) {
            var existe = false;
            for (var x = nivel; x >= 1; x--) {
                var pai = "#ctl00_areaConteudo_comboDetalhamento" + x + prefixo;
                if ($(pai).val() == $(opcoes[i]).val()) {
                    existe = true;
                    break;
                }
            }

            if (existe) {
                $(opcoes[i]).attr("disabled", "disabled");
            } else {
                $(opcoes[i]).removeAttr("disabled");
            }
        }

        for (var i = (nivel + 2); i <= max; i++) {
            if (i != nivel) {
                var idFilho = "#ctl00_areaConteudo_comboDetalhamento" + i + prefixo;

                $(idFilho).attr("disabled", "disabled");
                $(idFilho).val("0");
            }
        }
    }
}

function exibeJobDefault(valor, input) {
    if ($(input).is(':checked')) {
        if (valor == 0) {
            $("#campoJobDefault").hide();
        }
        else {
            $("#campoJobDefault").show();
        }
    }
    else {
        $("#campoJobDefault").show();
    }
}

function bloqueiaCombo(ele, combo) {
    if ($(ele).val() != "0") {
        $("#" + combo).attr("disabled", "disabled");
    }
    else {
        $("#" + combo).attr("disabled", "");
    }
}

function textCarregando_Combos(comboLinhaNegocio, comboDivisao, comboCliente) {
    $("#" + comboLinhaNegocio).html("");
    $("#" + comboDivisao).html("");
    $("#" + comboCliente).html("");

    $("<option value=''>Carregando...</option>").appendTo("#" + comboLinhaNegocio);
    $("<option value=''>Carregando...</option>").appendTo("#" + comboDivisao);
    $("<option value=''>Carregando...</option>").appendTo("#" + comboCliente);
}

function exibeVencimentosTitulo(valor) {
    if (valor == "S") {
        $("#celulaParcelasTitulo").show();
        $("#celulaFornecedorTitulo").show();
        $(tela.comboTerceiro_lancto).removeAttr("disabled").trigger("chosen:updated");
    } else {
        $("#celulaParcelasTitulo").hide();
        $("#celulaFornecedorTitulo").hide();
        $(tela.comboTerceiro_lancto).attr("disabled", "disabled").trigger("chosen:updated");
    }
}

function exibeFiltro() {
    if ($("#areaFiltro").css("display") == "none") {
        $("#areaFiltro").show();
    } else {
        $("#areaFiltro").hide();
    }
}

function alteraFisicaJuridica(valor, textNomeRazao, textCnpjCpf) {
    var inputs = 2;

    for (var i = 0; i < inputs; i++) {
        if ($("input[id='" + valor + "_" + i + "'][type='radio']").prop("checked")) {
            if ($("input[id='" + valor + "_" + i + "'][type='radio']").prop("value") == "FISICA") {

                $("#labelNomeRazaoSocial").text("Nome");
                $("#labelCnpjCpf").text("CPF");
                $("#labelIeRg").text("RG");

                $("#linhaNomeFantasia").hide();
                $("#linhaIM").hide();
                //$("#linhaImpostos").hide();

                $("#" + textCnpjCpf).unmask();
                $("#" + textCnpjCpf).mask("999.999.999-99");
            }
            else {
                $("#labelNomeRazaoSocial").text("Razão Social");
                $("#labelCnpjCpf").text("CNPJ");
                $("#labelIeRg").text("Inscrição Estadual");

                $("#linhaNomeFantasia").show();
                $("#linhaIM").show();

                $("#" + textCnpjCpf).unmask();
                $("#" + textCnpjCpf).mask("99.999.999/9999-99");
            }
        }
    }
}

function alteraFisicaJuridicaCombo(valor, textNomeRazao, textCnpjCpf) {
    var inputs = 2;

    if (valor == "FISICA") {

        $("#labelNomeRazaoSocial").text("Nome");
        $("#labelCnpjCpf").text("CPF");

        $("#linhaNomeFantasia").hide();

        $("#linhaNomeRazaoSocial").show();
        $("#linhaCnpjCpf").show();

        $("#" + textCnpjCpf).unmask();
        $("#" + textCnpjCpf).mask("999.999.999-99");
    }
    else if (valor == "JURIDICA") {

        $("#labelNomeRazaoSocial").text("Razão Social");
        $("#labelCnpjCpf").text("CNPJ");

        $("#linhaNomeFantasia").show();
        $("#linhaNomeRazaoSocial").show();
        $("#linhaCnpjCpf").show();

        $("#" + textCnpjCpf).unmask();
        $("#" + textCnpjCpf).mask("99.999.999/9999-99");
    }
    else {
        $("#linhaNomeFantasia").hide();
        $("#linhaNomeRazaoSocial").hide();
        $("#linhaCnpjCpf").hide();
    }
}

function alteraTipo(radioTipo, radioFisicaJuridica) {
    var inputs = 6;

    for (var i = 0; i < inputs; i++) {
        if ($("input[id='" + radioTipo + "_" + i + "'][type='radio']").prop("checked")) {
            if ($("input[id='" + radioTipo + "_" + i + "'][type='radio']").prop("value") == "EMITENTE") {

                $("#linhaNomeFantasia").show();
                $("#linhaIM").show();
                //$("#linhaImpostos").show();

                $("input[id='" + radioFisicaJuridica + "_1'][type='radio']").prop("checked", true);
                $("input[id='" + radioFisicaJuridica + "_0'][type='radio']").prop("disabled", true);
            }
            else {
                //$("#linhaImpostos").hide();
                $("input[id='" + radioFisicaJuridica + "_0'][type='radio']").prop("disabled", false);
            }
        }
    }
}

function checkboxPadrao(check) {
    $(check).closest('tr').find('.Emitentes').prop('checked', true);
}

function checkboxEmitentes(check) {
    $(check).closest('tr').find('.Padrao').prop('checked', false);
}

$(function () {
    $(document).on('keypress', '.Monetaria', function (event) {
        var Valor = $(this).val();
        var Index_Virgula = Valor.indexOf(',')
        var Tabela_ASCII = event.keyCode;

        if (Index_Virgula !== -1 && Tabela_ASCII == 44)
            return false

        if (Tabela_ASCII != 44 && (Tabela_ASCII < 48 || Tabela_ASCII > 57))
            return false;

        //Tabela ASCII - Decimal
        //44 = ,
        //48 = 0
        //57 = 9
    });
});