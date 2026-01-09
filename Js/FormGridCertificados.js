// =============================
// Certificados - JS (mínimo e estável)
// =============================

window.CERT_UI = window.CERT_UI || {
    active: false,
    confirmed: false,
    allowOnePostback: false,
    lastSubmitterId: null
};

(function () {
    // Se jQuery não existe ainda, tenta depois (legado)
    if (!window.jQuery) { setTimeout(arguments.callee, 50); return; }
    var $ = window.jQuery;

    // Libera UM postback para o botão de salvar certificado
    $(document).on("click", "#btnSalvarCertificado", function () {
        window.CERT_UI.lastSubmitterId = "btnSalvarCertificado";
        window.CERT_UI.allowOnePostback = true;
    });

    // Bloqueia submit enquanto modal principal está aberto (evita fluxo legado)
    $(document).on("submit", "form", function (e) {
        if (window.CERT_UI.active && !window.CERT_UI.confirmed) {

            // exceção: permitir postback do upload
            if (window.CERT_UI.allowOnePostback && window.CERT_UI.lastSubmitterId === "btnSalvarCertificado") {
                window.CERT_UI.allowOnePostback = false;
                window.CERT_UI.lastSubmitterId = null;
                return true;
            }

            e.preventDefault();
            e.stopImmediatePropagation();
            return false;
        }
        return true;
    });

    // Bloqueia __doPostBack enquanto modal principal está aberto
    if (typeof window.__doPostBack === "function" && !window.__doPostBack.__certWrapped) {
        var _old = window.__doPostBack;

        window.__doPostBack = function (eventTarget, eventArgument) {
            if (window.CERT_UI.active && !window.CERT_UI.confirmed) {

                // exceção: permitir postback do upload
                if (window.CERT_UI.allowOnePostback && window.CERT_UI.lastSubmitterId === "btnSalvarCertificado") {
                    window.CERT_UI.allowOnePostback = false;
                    window.CERT_UI.lastSubmitterId = null;
                    return _old(eventTarget, eventArgument);
                }

                return; // bloqueia qualquer outro postback
            }

            return _old(eventTarget, eventArgument);
        };

        window.__doPostBack.__certWrapped = true;
    }

    // Após salvar, voltar para lista automaticamente
    $(function () {
        if ($("#hdfVoltarListaAposUpload").val() === "1") {
            try { $find("mpeUploadCertBehavior").hide(); } catch (e) { }
            try { $find("mpeCertificadosBehavior").show(); } catch (e) { }

            $("#hdfVoltarListaAposUpload").val("");

            // mantém travado até confirmar certificado
            window.CERT_UI.active = true;
            window.CERT_UI.confirmed = false;

            ativarSelecaoTabelaCertificados();
        }
    });

})();

// ---------- Seleção de notas ----------
function getNotasSelecionadas() {
    var $checks = $("#tabelaGrid input[type=checkbox].nf-check:checked");
    var notas = [];

    $checks.each(function () {
        var $cb = $(this);
        var $tr = $cb.closest("tr");

        notas.push({
            cod: $cb.attr("data-cod") || $cb.val(),
            emitente: $cb.attr("data-emitente") || $.trim($tr.find("td").eq(1).text()),
            tomador: $cb.attr("data-tomador") || $.trim($tr.find("td").eq(2).text()),
            nf: $cb.attr("data-nf") || $.trim($tr.find("td").eq(3).text()),
            rps: $cb.attr("data-rps") || $.trim($tr.find("td").eq(4).text()),
            emissao: $cb.attr("data-emissao") || $.trim($tr.find("td").eq(5).text()),
            valor: $cb.attr("data-valor") || $.trim($tr.find("td").eq(6).text())
        });
    });

    return notas;
}

function renderNotasSelecionadas(notas) {
    var $tb = $("#tblNotasSelecionadas tbody");
    $tb.empty();

    for (var i = 0; i < notas.length; i++) {
        var n = notas[i];
        $tb.append(
            "<tr>" +
            "<td>" + (n.emitente || "") + "</td>" +
            "<td>" + (n.tomador || "") + "</td>" +
            "<td>" + (n.nf || "") + "</td>" +
            "<td>" + (n.rps || "") + "</td>" +
            "<td>" + (n.emissao || "") + "</td>" +
            "<td style='text-align:right;'>" + (n.valor || "") + "</td>" +
            "</tr>"
        );
    }
}

function ativarSelecaoTabelaCertificados() {
    $("#tblCertificados .cert-row").off("click").on("click", function () {
        var $row = $(this);
        $("#tblCertificados .cert-row").removeClass("cert-row-selected");
        $row.addClass("cert-row-selected");
        $row.find('input[type="radio"][name="rbCert"]').prop("checked", true);
        $("#spnAvisosCert").text("");
    });
}

// ---------- Fluxo modal ----------
function abrirModalCertificados() {
    var notas = getNotasSelecionadas();

    if (notas.length === 0) {
        alert("Selecione ao menos 1 nota fiscal para enviar.");
        return false;
    }

    window.CERT_UI.active = true;
    window.CERT_UI.confirmed = false;

    $("#spnQtdSelecionadas").text(notas.length);
    renderNotasSelecionadas(notas);
    ativarSelecaoTabelaCertificados();

    try { $find("mpeCertificadosBehavior").show(); }
    catch (e) { $("#pciCertificados").show(); }

    return false; // MUITO importante para impedir postback
}

function fecharModalCertificados() {
    try { $find("mpeCertificadosBehavior").hide(); } catch (e) { $("#pciCertificados").hide(); }
    window.CERT_UI.active = false;
    window.CERT_UI.confirmed = false;
    return false;
}

// Upload modal
function abrirModalUploadCert() {
    try { $find("mpeCertificadosBehavior").hide(); } catch (e) { }
    try { $find("mpeUploadCertBehavior").show(); } catch (e) { $("#pciUploadCert").show(); }
    return false;
}

function voltarParaListaCertificados() {
    try { $find("mpeUploadCertBehavior").hide(); } catch (e) { $("#pciUploadCert").hide(); }
    try { $find("mpeCertificadosBehavior").show(); } catch (e) { $("#pciCertificados").show(); }

    window.CERT_UI.active = true;
    window.CERT_UI.confirmed = false;

    ativarSelecaoTabelaCertificados();
    return false;
}

// Confirmar certificado (por enquanto só fecha e guarda o ID)
function usarCertificadoSelecionado() {
    var certId = $('input[name="rbCert"]:checked').val();
    if (!certId) {
        $("#spnAvisosCert").text("Selecione um certificado antes de continuar.");
        return false;
    }

    $("#hdfCertSelecionado").val(certId);
    window.CERT_UI.confirmed = true;
    window.CERT_UI.active = false;

    try { $find("mpeCertificadosBehavior").hide(); } catch (e) { $("#pciCertificados").hide(); }

    // NÃO disparar fluxo legado agora. Vamos focar no upload primeiro.
    return false;
}
