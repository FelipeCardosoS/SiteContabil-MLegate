function iniciaSincronizacao() {
    sincronizaDivisao();
    sincronizaLinhasNegocio();
    sincronizaClientes();
    sincronizaJobLinhaNegocio();
}

function sincronizaDivisao() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/sincronizaDivisoes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function() {
            $("#box_sincronizacao_divisao").html("Sincronizando divisões...");
        },
        success: function(result) {
            if (result.length > 0) {
                var texto = "";

                for (var i = 0; i < result.length; i++) {
                    texto += "<p>" + result[i] + "</p>";
                }
                $("#box_sincronizacao_divisao").html(texto);
            } else {
                $("#box_sincronizacao_divisao").html("Sincronização realizada com sucesso.");
            }
        },
        complete: function(result) {
            $("#box_sincronizacao_divisao").html("Sincronização realizada com sucesso.")
        },
        error: function(err) {
            $("#box_sincronizacao_divisao").html("Erro fatal na sincronização.");
        }
    });
}

function sincronizaLinhasNegocio() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/sincronizaLinhasNegocio",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function() {
            $("#box_sincronizacao_linha_negocio").html("Sincronizando linhas de negócio...");
        },
        success: function(result) {
            if (result.length > 0) {
                var texto = "";

                for (var i = 0; i < result.length; i++) {
                    texto += "<p>" + result[i] + "</p>";
                }
                $("#box_sincronizacao_linha_negocio").html(texto);
            } else {
                $("#box_sincronizacao_linha_negocio").html("Sincronização realizada com sucesso.");
            }
        },
        complete: function(result) {
            $("#box_sincronizacao_linha_negocio").html("Sincronização realizada com sucesso.")
        },
        error: function(err) {
            $("#box_sincronizacao_linha_negocio").html("Erro fatal na sincronização.");
        }
    });
}

function sincronizaClientes() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/sincronizaClientes",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function() {
            $("#box_sincronizacao_cliente").html("Sincronizando clientes...");
        },
        success: function(result) {
            if (result.d.length > 0) {
                var texto = "";

                for (var i = 0; i < result.d.length; i++) {
                    texto += "<p>" + result.d[i] + "</p>";
                }
                $("#box_sincronizacao_cliente").html(texto);
            } else {
                $("#box_sincronizacao_cliente").html("Sincronização realizada com sucesso.");
            }
        },
        complete: function(result) {
            $("#box_sincronizacao_cliente").html("Sincronização realizada com sucesso.")
            sincronizaJob();
        },
        error: function(err) {
            $("#box_sincronizacao_cliente").html("Erro fatal na sincronização.");
        }
    });
}

function sincronizaJob() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/sincronizaJobs",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function() {
            $("#box_sincronizacao_job").html("Sincronizando jobs...");
        },
        success: function(result) {
            if (result.d.length > 0) {
                var texto = "";

                for (var i = 0; i < result.d.length; i++) {
                    texto += "<p>" + result.d[i] + "</p>";
                }
                $("#box_sincronizacao_job").html(texto);
            } else {
                $("#box_sincronizacao_job").html("Sincronização realizada com sucesso.");
            }
        },
        complete: function(result) {
            
        },
        error: function(err) {
            $("#box_sincronizacao_job").html("Erro fatal na sincronização.");
        }
    });
}

function sincronizaJobLinhaNegocio() {
    $.ajax({
        type: "POST",
        url: "Default.aspx/sincronizaJobLinhaNegocio",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#box_sincronizacao_joblinhanegocio").html("Analisando cadastro de Jobs de outras divisões...");
        },
        success: function (result) {
            if (result.d > 0) {
                if(confirm('Existem Jobs de outras divisões com consultores associados. Deseja atualizar a base do FC ?')) {
                    window.location.replace("FormAnaliseJobs.aspx");
                }
            } else {
                $("#box_sincronizacao_joblinhanegocio").html("Sincronização realizada com sucesso.");
            }
        },
        complete: function (result) {

        },
        error: function (err) {
            $("#box_sincronizacao_joblinhanegocio").html("Erro fatal na sincronização.");
        }
    });
}