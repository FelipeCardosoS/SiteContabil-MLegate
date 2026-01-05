function limpaNumero(numero) {
    if (numero != "") {
        var existe = true;
        while (existe) {
            numero = numero.replace(".", "");
            var posicao = numero.indexOf(".");
            if (posicao >= 0)
                existe = true;
            else
                existe = false;
        }

        numero = numero.replace(",", ".");
        return Number(numero);
    } else {
        return 0;
    }
}

function _GET(name) {
    var url = window.location.search.replace("?", "");
    var itens = url.split("&");

    for (n in itens) {
        if (itens[n].match(name)) {
            return decodeURIComponent(itens[n].replace(name + "=", ""));
        }
    }
    return null;
}

function Onlynumbers(e) {
    var tecla = new Number();
    if (window.event) {
        tecla = e.keyCode;
    }
    else if (e.which) {
        tecla = e.which;
    }
    else {
        return true;
    }

    if ((tecla >= "48") && (tecla <= "57")) {
        return true;
    } else {
        if (tecla == 46 || tecla == 44) {
            return true;
        } else {
            return false;
        }
    }
}

function OnlynumbersFiltrado(e) {
    var tecla = new Number();
    if (window.event) {
        tecla = e.keyCode;
    }
    else if (e.which) {
        tecla = e.which;
    }
    else {
        return true;
    }
    if ((tecla >= "48") && (tecla <= "57")) {
        return true;
    } else {

        return false;
    }
}

function verificaNulo(input) {
    if ($(input).val() == "")
        $(input).val("0");
}


function number_format(number, decimals, dec_point, thousands_sep) {
    var n = number, c = isNaN(decimals = Math.abs(decimals)) ? 2 : decimals;
    var d = dec_point == undefined ? "," : dec_point;
    var t = thousands_sep == undefined ? "." : thousands_sep, s = n < 0 ? "-" : "";
    var i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
}

function formataValor(campo) {
    campo.value = filtraCampo(campo);
    vr = campo.value;
    tam = vr.length;

    if (tam <= 2) {
        campo.value = vr;
    }
    if ((tam > 2) && (tam <= 5)) {
        campo.value = vr.substr(0, tam - 2) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 6) && (tam <= 8)) {
        campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 9) && (tam <= 11)) {
        campo.value = vr.substr(0, tam - 8) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 12) && (tam <= 14)) {
        campo.value = vr.substr(0, tam - 11) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 15) && (tam <= 18)) {
        campo.value = vr.substr(0, tam - 14) + '.' + vr.substr(tam - 14, 3) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }

}

function formataNumerico(campo) {

    campo.value = filtraCampo(campo);
    vr = campo.value;
    tam = vr.length;
}

// limpa todos os caracteres especiais do campo solicitado
function filtraCampo(campo) {
    var s = "";
    var cp = "";
    vr = campo.value;
    tam = vr.length;
    for (i = 0; i < tam ; i++) {
        if (vr.substring(i, i + 1) != "/" && vr.substring(i, i + 1) != "-" && vr.substring(i, i + 1) != "." && vr.substring(i, i + 1) != ",") {
            s = s + vr.substring(i, i + 1);
        }
    }
    campo.value = s;
    return cp = campo.value
}


function horizontal(idMenu) {

    var navItems = document.getElementById("ulMenu").getElementsByTagName("li");

    for (var i = 0; i < navItems.length; i++) {
        if (navItems[i].className == "submenu") {
            if (navItems[i].getElementsByTagName('ul')[0] != null) {
                navItems[i].onmouseover = function () {
                    this.getElementsByTagName('ul')[0].style.display = "block";
                    //this.getElementsByTagName('a')[0].style.color="#DE5D29";
                }
                navItems[i].onmouseout = function () {
                    this.getElementsByTagName('ul')[0].style.display = "none";
                    //this.getElementsByTagName('a')[0].style.color="#fff";
                }
            }
        }
    }
}

function marcaTodos() {
    //$("input[type='checkbox']").attr("checked", "checked");
    $("input[type='checkbox']").prop("checked", true)
}

function desmarcaTodos() {
    //$("input[type='checkbox']").removeAttr("checked", "");
    $("input[type='checkbox']").prop("checked", false);
}

function Verifica_Selecionados(botao) {
    if (Confirmar(botao)) {
        if ($("input[type=checkbox]").is(':checked')) {
            return true;
        } else {
            alert("Nenhum item foi selecionado.");
            return false;
        }
    } else {
        return false;
    }
}

function Confirmar(botao) {
    var conf = '';

    if (botao == 'botaoContabilizar') {
        conf = window.confirm("Os itens selecionados serão Contabilizados. Deseja prosseguir?");
    }
    else if (botao == 'botaoDeletar') {
        conf = window.confirm("Tem certeza de que deseja realizar esta ação?");
    }
    
    return conf;
}

function popup(url, nwidth, nheight) {
    var width = nwidth;
    var height = nheight;
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    var params = 'width=' + width + ', height=' + height;
    params += ', top=' + top + ', left=' + left;
    params += ', directories=no';
    params += ', location=no';
    params += ', menubar=no';
    params += ', resizable=yes';
    params += ', scrollbars=yes';
    params += ', status=no';
    params += ', toolbar=no';
    newwin = window.open(url, '', params);
    if (window.focus) {
        newwin.focus();
    }
}

function popupf(url) {
    var width = 1000;
    var height = 400;
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    var params = 'width=' + width + ', height=' + height;
    params += ', top=' + top + ', left=' + left;
    params += ', directories=no';
    params += ', location=no';
    params += ', menubar=no';
    params += ', resizable=yes';
    params += ', scrollbars=yes';
    params += ', status=no';
    params += ', toolbar=no';
    newwin = window.open(url, '', params);
    if (window.focus) {
        newwin.focus();
    }
}
function popupRazao(url, nwidth, nheight) {
    var width = 998;
    var height = 600;
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    var params = 'width=' + width + ', height=' + height;
    params += ', top=' + top + ', left=' + left;
    params += ', directories=no';
    params += ', location=no';
    params += ', menubar=no';
    params += ', resizable=no';
    params += ', scrollbars=yes';
    params += ', status=no';
    params += ', toolbar=no';
    newwin = window.open(url, '', params);
    if (window.focus) {
        newwin.focus();
    }
}

function Converte_Data_EUA(ddMMyyyy, Campo) {
    if (ddMMyyyy != '') {
        var Data = ddMMyyyy.split("/");

        if (parseInt(Data[0]) < 1)
            return Campo + ': Informe um Dia Válido.'

        if (parseInt(Data[1]) < 1)
            return Campo + ': Informe um Mês Válido.'

        if (parseInt(Data[2]) < 2000)
            return Campo + ': Informe um Ano Válido.'

        switch (Data[1]) {
            case '01':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Janeiro possui até 31 dias.'
            case '02':
                if (Data[0] <= 29)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Fevereiro possui até 29 dias.'
            case '03':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Março possui até 31 dias.'
            case '04':
                if (Data[0] <= 30)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Abril possui até 30 dias.'
            case '05':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Maio possui até 31 dias.'
            case '06':
                if (Data[0] <= 30)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Junho possui até 30 dias.'
            case '07':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Julho possui até 31 dias.'
            case '08':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Agosto possui até 31 dias.'
            case '09':
                if (Data[0] <= 30)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Setembro possui até 30 dias.'
            case '10':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Outubro possui até 31 dias.'
            case '11':
                if (Data[0] <= 30)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Novembro possui até 30 dias.'
            case '12':
                if (Data[0] <= 31)
                    return Data[1] + "/" + Data[0] + "/" + Data[2];
                else
                    return Campo + ': Informe um Dia Válido. Dezembro possui até 31 dias.'
            default:
                return Campo + ': Informe um Mês entre 01 e 12 (Janeiro a Dezembro).'
        }
    }
    else
        return ''
}

function Mensagem_Sucesso_Erro(Mensagem) {
    if (Mensagem.d != '') {
        if (Mensagem.d.substring(0, 1) == '/') {
            if (Mensagem.d.substring(1, 30) == 'FormConfirmacaoContabilizacao')
                alert('Operação realizada com sucesso.\n\nContabilizando A Nota Fiscal.');
            else
                alert('Operação realizada com sucesso.');

            location.href = '..' + Mensagem.d; //Redireciona para o Form indicado na variável "Mensagem".
        }
        else if (Mensagem.d == 'reload') {
            alert('Operação realizada com sucesso.');
            location.reload(); //Recarrega a tela.
        }
        else {
            alert(Mensagem.d); //Mensagem de Erro.
            if (Mensagem.d.indexOf('recarregada') >= 0)
                location.reload(); //Recarrega a tela.
        }
    }
    else
        alert('Operação realizada com sucesso.');
}

function Mensagem_Erro() {
    alert('Erro ao realizar a tarefa!');
}