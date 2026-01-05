var cont = 1;
var optionConta = "";
var optionsAll = "";

function removeOption(option) {
    if ($("fieldset #optionContasMenu").size() > 1) {
        $("." + option).fadeOut("normal", function () { 
            $("." + option).remove();
        });
    }
}

$(document).ready(function () {
    $("#btnSalva").click(function () {
        optionConta = "";
        $("select#Contas").each(function () {
            if ($(this).val() != 0) {
                var wordFind = optionConta.indexOf($(this).val());
                if (wordFind == -1) {
                    optionConta += "''" + $(this).val() + "'',";
                }
            }
        });
        optionConta = (optionConta.length > 0 ? optionConta.substring(0, (optionConta.length - 1)) : optionConta);

        if (optionConta == "") {
            alert("Informe pelo menos uma conta!");
        } else {
            $.ajax({
                type: "POST",
                url: "FormBaseImpostoParametros.aspx/atualizaParametros",
                data: "{ 'contas' : \"" + optionConta + "\" , 'irnafonte' : '" + $("select#IRFonte").val() + "', 'csl':'" + $("select#CSL").val() + "', 'pis':'" + $("select#PIS").val() + "', 'cofins':'" + $("select#COFINS").val() + "', 'iss':'" + $("select#ISS").val() + "', 'valorliquido':'" + $("select#ValorLiquido").val() + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var result = r.d;
                    if (result == "true") {
                        alert('Parâmetros atualizados com sucesso!');
                    }
                    else {
                        alert('Não foi possível atualizar os parâmetros!');
                    }
                }
            });
        }
        return false;
    });

    $("fieldset #optionContasMenu").find("#Soma").live("click", function () {
        cont++;
        var post = $("fieldset #optionContasMenu:first").clone();
        var novoNome = "option" + cont;
        var novoSelect = "optionContas" + cont;
        var divCompleta = post[0].outerHTML;
        divCompleta = divCompleta.replace(/option[0-9]{1,2}/g, novoNome);
        divCompleta = divCompleta.replace(/optionContas[0-9]{1,2}/g, novoSelect);
        $("fieldset #optionContasMenu:last").after(divCompleta);
        $(".optionContas" + cont + " option").prop("selected", false);
    });

    $("fieldset #optionContasMenu").find("#Subtrai").live("click", function () {
        alert($(this).live().size());
    });

    $.ajax({
        type: "POST",
        url: "FormBaseImpostoParametros.aspx/retornaContas",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var result = r.d;
            var option = "<option value='0'>Selecione...</option>";
            for (var x = 0; x < result.length; x++) {
                option += "<option value='" + result[x].codigo + "'>" + result[x].codigo + " - " + result[x].valor + "</option>";
            }

            optionsAll = option;

            $("select#Contas").each(function () {
                $(this).html(option);
            });

            $("select#IRFonte").html(option);
            $("select#CSL").html(option);
            $("select#PIS").html(option);
            $("select#COFINS").html(option);
            $("select#ISS").html(option);
            $("select#COFINS").html(option);
            $("select#ValorLiquido").html(option);

            $.ajax({
                type: "POST",
                url: "FormBaseImpostoParametros.aspx/carregaValores",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var result = r.d;

                    var removeContas = result[0].COD_CONTAS.replace(/\'/g, "");
                    var splitContas = removeContas.split(",");
                    
                    for (var x = 1; x < splitContas.length; x++) {
                        cont++;
                        var post = $("fieldset #optionContasMenu:first").clone();
                        var novoNome = "option" + cont;
                        var novoSelect = "optionContas" + cont;
                        var divCompleta = post[0].outerHTML;
                        divCompleta = divCompleta.replace(/option[0-9]{1,2}/g, novoNome);
                        divCompleta = divCompleta.replace(/optionContas[0-9]{1,2}/g, novoSelect);
                        $("fieldset #optionContasMenu:last").after(divCompleta);
                    };

                    $("select#Contas").each(function (index, v) {
                        var keyConta = splitContas.shift();
                        $(".optionContas" + (index + 1) + " option[value='" + keyConta + "'").attr('selected', true);
                    });

                    $("select#IRFonte option[value='" + result[0].IR_NA_FONTE + "'").attr('selected', true);
                    $("select#CSL option[value='" + result[0].CSL + "'").attr('selected', true);
                    $("select#PIS option[value='" + result[0].PIS + "'").attr('selected', true);
                    $("select#ISS option[value='" + result[0].ISS + "'").attr('selected', true);
                    $("select#COFINS option[value='" + result[0].COFINS + "'").attr('selected', true);
                    $("select#ValorLiquido option[value='" + result[0].VALOR_LIQUIDO + "'").attr('selected', true);

                }
            });
        }
    });

});