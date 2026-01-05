function verificaLancamentos() {
    var status = true;
    $.ajax({
        type: "POST",
        async:false,
        url: "Default.aspx/getSessionLanctos",
        data: "{modulo:'" + tela.modulo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: OnBeforeSend("Montando tela..."),
        success: function(result) {
            if (result != null) {
                if(result.length > 0){
                    status = window.confirm("Você deseja sair e perder os lançamentos?")
                }
            }
        },
        complete: function(a) {
            return status;   
        }
    });
    
}












