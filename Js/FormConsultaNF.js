function function_botaoGerarArquivo() {
    if (confirma_GerarArq()) {
        if ($("input[type=checkbox]").is(':checked')) {            
            return valida();
        } else {
            alert('Nenhum item foi selecionado.');
            return false;
        }
    } else {
        return false;
    }
}

function confirma_GerarArq() {
    var conf = window.confirm("Será gerado o arquivo de texto dos itens selecionados, para ser anexado ao site da Prefeitura. Deseja prosseguir?");
    return conf;
}

function valida() {
    var txtemitente = '';
    var valid = true;
    $('input:checkbox:checked:enabled').each(function () {
        if (txtemitente == '') {
            txtemitente = $(this).closest('tr').find('.emitente').text();
        } else {
            if ($(this).closest('tr').find('.emitente').text() != txtemitente) {
                alert('É permitido selecionar apenas um Emitente por vez.');
                valid = false;
            }            
        } 
        if ($(this).closest('tr').find('.status').text() == "Gerada") {
            alert('Selecione somente Notas Fiscais com o status diferente de "Gerada".');
            valid = false;
        }
        else if ($(this).closest('tr').find('.situacao').text() == "CANCELADA") {
            alert('Não selecione Notas Fiscais canceladas.');
            valid = false;
        }
    });
    return valid;
}