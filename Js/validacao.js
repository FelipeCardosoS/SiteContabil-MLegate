function validaCombo(source, args) {
	if(args.Value != ""){
		if (args.Value == "0")
			args.IsValid = false;
		else
			args.IsValid = true;
	}else{
		args.IsValid = false;
	}
}

function validaRadioDebCred(source, args) {
    var PrefixoCampos = "ctl00_areaForm_";
    var max = $("#"+PrefixoCampos+"num_itens").val();
    alert(max);
    args.IsValid = false;
    
    var selecionados = 0;
    for (var i = 0; i < max; i++) {
        if(document.enquete.elements[i].type == "radio"){
            if (document.getElementById(PrefixoCampos + "radioDebCred_"+i).checked) {
                selecionados++;
            }
        }
    }

    if (selecionados == 0) {
        args.IsValid = false;
    } else {
        args.IsValid = true;
    }
}

function validaListCheck(source, args) {
    var PrefixoCampos = "ctl00_areaForm_";
    var max = $("#"+PrefixoCampos+"num_itens").val();
    var selecionados = 0;

    for (var i = 0; i < max; i++) {
        if (document.getElementById(PrefixoCampos + "checkNegocio_"+i).checked) {
            selecionados++;
        }
    }

    if (selecionados == 0) {
        args.IsValid = false;
    } else {
        args.IsValid = true;
    }
}