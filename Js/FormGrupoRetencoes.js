//Projeto interrompido em 2018. Sem previsão de retomada.
//Menu (Banco de Dados): "CAD_MODULOS" e "CAD_TAREFAS" já possuem estrutura (FATURAMENTO_CADASTROS_GRUPO_RETENCOES).
//Engloba 5 Arquivos: "FormGrupoRetencoes.js", "FormGrupoRetencoes.aspx", "FormGrupoRetencoes.aspx.cs", "GrupoRetencao.cs" e "GrupoRetencoesDAO.cs".

$(function () {
    $(document).ready(function () {
        //Esconder e Mostrar os itens que dependem da escolha do Emitente.
        if ($('.ddlEmitente').val() == '0')
            $('.Visivel').hide();
        else
            $('.Visivel').show();
    });
});

function SetUniqueRadioButton(nameregex, current) {
    re = new RegExp(nameregex);
    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i]
        if (elm.type == 'radio') {
            if (re.test(elm.name)) {
                elm.checked = false;
            }
        }
    }
    current.checked = true;
}

//Serviços Jobs
function btnNovoGR_Click() {
    $('#Servicos_Jobs tbody').append('<tr>'
        + '<td class="line" style="width:375px"><select class="cboServico" style="width:350px" /></td>'
        + '<td class="line" style="width:375px"><select class="cboJob" style="width:350px" /></td>'
        + '<td class="line" style="width:200px"><input type="text" style="width:175px" maxlength="18" class="Monetaria txtValorServicoJob" /></td>'
        + '<td class="line"><input type="button" value="-" onclick="Remover_Servicos_Jobs(this)" class="btnMinus_Servicos_Jobs" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" onclick="btnNovoGR_Click()" /></td>'
        + '</tr>');
    let select_cboServico = $('#Servicos_Jobs .cboServico:first').html();
    let select_cboJob = $('#Servicos_Jobs .cboJob:first').html();
    $('.cboServico:last').html(select_cboServico);
    $('.cboJob:last').html(select_cboJob);
}

function Remover_Servicos_Jobs(Linha) {
    if ($('#Servicos_Jobs tbody tr').length > 1) {
        $(Linha).closest('tr').remove();
    }

    if ($(Linha).closest('tr').find('.tdADD').html != '') {
        $('#Servicos_Jobs tbody tr:last .tdADD').html('<input type="button" value="+" onclick="btnNovoGR_Click()" />');
    }

    Total();
    Total_Liquido();
    Vencimento_Parcelas();
    Vencimento_Total();
}

//Vencimento
function Adicionar_Vencimento() {
    $('.btnPlus_Vencimento').remove();
    $('#Vencimento tbody').append('<tr>'
        + '<td class="line" style="width:200px"><input type="text" style="width:175px" maxlength="10" class="date txtDataVencimento" /></td>'
        + '<td class="line" style="width:200px"><input type="text" style="width:175px" maxlength="18" class="Monetaria txtValorVencimento" /></td>'
        + '<td class="line"><input type="button" value="-" onclick="Remover_Vencimento(this)" class="btnMinus_Vencimento" /></td>'
        + '<td class="line tdADD_Vencimento"><input type="button" value="+" onclick="Adicionar_Vencimento()" class="btnPlus_Vencimento" /></td>'
        + '</tr>');

    $('.date').mask('99/99/9999'); //Data de Vencimento
    Vencimento_Parcelas();
    Vencimento_Total();
}

function Remover_Vencimento(Linha_Vencimento) {
    if ($('#Vencimento tbody tr').length > 1) {
        $(Linha_Vencimento).closest('tr').remove();
    }

    if ($(Linha_Vencimento).closest('tr').find('.tdADD_Vencimento').html != '') {
        $('#Vencimento tbody tr:last .tdADD_Vencimento').html('<input type="button" value="+" onclick="Adicionar_Vencimento()" class="btnPlus_Vencimento" />');
    }

    Vencimento_Parcelas();
    Vencimento_Total();
}

//Narrativa
function Adicionar_Narrativa() {
    $('.btnPlus_Narrativa').remove();
    $('#Narrativa tbody').append('<tr>'
        + '<td class="line" style="width:375px"><select class="cboNarrativa" style="width:350px" /></td>'
        + '<td class="line" style="width:575px"><input type="text" class="txtDescricaoNarrativa" style="width:550px" disabled="disabled" /></td>'
        + '<td class="line"><input type="button" value="-" onclick="Remover_Narrativa(this)" class="btnMinus_Narrativa" /></td>'
        + '<td class="line tdADD_Narrativa"><input type="button" value="+" onclick="Adicionar_Narrativa()" class="btnPlus_Narrativa" /></td>'
        + '</tr>');
    let select_cboNarrativa = $('#Narrativa .cboNarrativa:first').html();
    $('.cboNarrativa:last').html(select_cboNarrativa).val('0');
}

function Remover_Narrativa(Linha_Narrativa) {
    if ($('#Narrativa tbody tr').length > 1) {
        $(Linha_Narrativa).closest('tr').remove();
    }

    if ($(Linha_Narrativa).closest('tr').find('.tdADD_Narrativa').html != '') {
        $('#Narrativa tbody tr:last .tdADD_Narrativa').html('<input type="button" value="+" onclick="Adicionar_Narrativa()" class="btnPlus_Narrativa" />');
    }
}

function btnSalvar_Click() {
    $('.btnSalvar').prop("disabled", true);

    var erros = [];
    var Contador = 0;
    var Repetido = 0;
    var List_Count = 0;
    var Tabela_Length = $('#Servicos_Jobs tbody tr').length;
    var Vencimento_OK = true;

    //var _cod_tomador = parseInt($('.cboCliente').val());
    var _data_emissao_rps = Converte_Data_EUA($('.txtDataEmissaoRPS').val(), 'Data de Emissão');
    var _data_rps = Converte_Data_EUA($('.txtDataRPS').val());
    var _cod_natureza_operacao = parseInt($('.cboNaturezaOperacao').val());
    var _cod_prestacao_servico = parseInt($('.cboPrestacaoServico').val());
    var _valor_total_liquido = $('.lblTotal_Liquido').text().replace(/\./g, '').substring(18);
    var _valor_total_vencimento = $('.lblTotal_Vencimento').text().replace(/\./g, '').substring(10);

    if (_cod_tomador == 0)
        erros.push('Selecione um Tomador.');

    if (_data_emissao_rps == '')
        erros.push('Informe a Data de Emissão da Nota Fiscal.');
    else if (_data_emissao_rps.length > 10) //Se a Data de Emissão estiver no formato errado, terá mais de 10 caracteres
        erros.push(_data_emissao_rps); //Mensagem de erro atribuída pela função "Converte_Data_EUA" do "uteis.js"
    else if (parseInt(_data_emissao_rps.substr(6) + _data_emissao_rps.substr(0, 2) + _data_emissao_rps.substr(3, 2)) < parseInt(_data_rps.substr(6) + _data_rps.substr(0, 2) + _data_rps.substr(3, 2)))
        erros.push('Informe uma Data de Emissão igual ou posterior a Última Nota Fiscal Emitida.');

    if (_cod_natureza_operacao == 0)
        erros.push('Selecione uma Natureza da Operação.');

    if (_cod_prestacao_servico == 0)
        erros.push('Selecione uma Prestação de Serviço.');

    //Serviço e Job
    $('#Servicos_Jobs tbody tr').each(function () {
        var cboServico = parseInt($(this).closest('tr').find('td .cboServico').val());
        var cboJob = parseInt($(this).closest('tr').find('td .cboJob').val());
        var txtValorServicoJob = $(this).closest('tr').find('td .txtValorServicoJob').val();

        if (cboServico == 0) {
            erros.push('Selecione um Serviço.');
            Contador += 1;
        }

        if (cboJob == 0) {
            erros.push('Selecione um Job.');
            Contador += 1;
        }

        if (txtValorServicoJob == '') {
            erros.push('Informe o Valor do Serviço.');
            Contador += 1;
        }

        if (Contador > 0)
            return false;

        $('#Servicos_Jobs tbody tr').each(function () {
            var cboServico2 = parseInt($(this).closest('tr').find('td .cboServico').val());
            var cboJob2 = parseInt($(this).closest('tr').find('td .cboJob').val());

            if (cboServico == cboServico2 && cboJob == cboJob2)
                Repetido += 1;
        });

        List_Count += 1;
        if (List_Count == Tabela_Length && Repetido != Tabela_Length)
            erros.push('Informe Serviços e Jobs diferentes.');
    });

    //Vencimento
    Contador = 0;
    Repetido = 0;
    List_Count = 0;
    Tabela_Length = $('#Vencimento tbody tr').length;

    $('#Vencimento tbody tr').each(function () {
        var txtDataVencimento = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val(), 'Data de Vencimento');
        var txtValorVencimento = $(this).closest('tr').find('td .txtValorVencimento').val();

        if (txtDataVencimento == '' && txtValorVencimento == '') {
            erros.push('Informe a Data e o Valor do Vencimento.');
            Contador += 1;
            Vencimento_OK = false;
        }

        if (txtDataVencimento == '' && txtValorVencimento != '') {
            erros.push('Informe a Data de Vencimento.');
            Contador += 1;
            Vencimento_OK = false;
        }

        if (txtDataVencimento != '' && txtValorVencimento == '') {
            erros.push('Informe o Valor do Vencimento.');
            Contador += 1;
            Vencimento_OK = false;
        }

        if (txtDataVencimento.length > 10) { //Se a Data de Vencimento estiver no formato errado, terá mais de 10 caracteres
            erros.push(txtDataVencimento); //Mensagem de erro atribuída pela função "Converte_Data_EUA" do "uteis.js"
            Contador += 1;
            Vencimento_OK = false;
        }

        if (txtDataVencimento != '' && parseInt(txtDataVencimento.substr(6) + txtDataVencimento.substr(0, 2) + txtDataVencimento.substr(3, 2)) < parseInt(_data_emissao_rps.substr(6) + _data_emissao_rps.substr(0, 2) + _data_emissao_rps.substr(3, 2))) {
            erros.push('Informe uma Data de Vencimento igual ou posterior a Data de Emissão.');
            Contador += 1;
            Vencimento_OK = false;
        }

        if (Contador > 0)
            return false;

        if (txtDataVencimento != '' && txtValorVencimento != '') {
            $('#Vencimento tbody tr').each(function () {
                var txtDataVencimento2 = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val());

                if (txtDataVencimento == txtDataVencimento2)
                    Repetido += 1;
            });
        }
        else
            Repetido += 1;

        List_Count += 1;
        if (List_Count == Tabela_Length && Repetido != Tabela_Length) {
            erros.push('Informe Datas de Vencimento diferentes.');
            Vencimento_OK = false;
        }
    });

    if (Vencimento_OK == true) {
        if (_valor_total_liquido != _valor_total_vencimento)
            erros.push('O Valor Total dos Vencimentos deve ser igual ao Total Líquido.');
    }

    //Narrativa
    Repetido = 0;
    List_Count = 0;
    Tabela_Length = $('#Narrativa tbody tr').length;

    $('#Narrativa tbody tr').each(function () {
        var cboNarrativa = parseInt($(this).closest('tr').find('td .cboNarrativa').val());

        if (cboNarrativa == 0) {
            erros.push('Selecione uma Narrativa.');
            return false;
        }

        $('#Narrativa tbody tr').each(function () {
            var cboNarrativa2 = parseInt($(this).closest('tr').find('td .cboNarrativa').val());

            if (cboNarrativa == cboNarrativa2)
                Repetido += 1;
        });

        List_Count += 1;
        if (List_Count == Tabela_Length && Repetido != Tabela_Length)
            erros.push('Informe Narrativas diferentes.');
    });

    if (erros.length == 0) {
        var _cod_emitente = parseInt($('.ddlEmitente').val());
        var _nome_natureza_operacao = $('.cboNaturezaOperacao :selected').text();
        var _descricao_natureza_operacao = $('.txtDescricaoNO').val();
        var _natureza_operacao = $('.txtNaturezaOperacao').val();
        var _nome_prestacao_servico = $('.cboPrestacaoServico :selected').text();
        var _descricao_prestacao_servico = $('.txtDescricaoPS').val();
        var _valor_total_servico_job = $('.lblTotal').text().replace(/\./g, '').substring(10);
        var _observacoes = $('.txtObservacoes').val();

        var _List_Retencao = [];
        var _List_Servico_Job = [];
        var _List_Vencimento = [];
        var _List_Narrativa = [];

        $('#Retencao tbody tr input[type=checkbox]:checked').each(function () {
            var _Retencao = {
                Cod_GR: parseInt($(this).closest('tr').find('td .rbtnCodGR').val()),
                Nome_GR: $(this).closest('tr').find('td .tbxNomeGR').val(),
                Ativo_GR: $(this).closest('tr').find('td .cbxAtivoGR').prop('checked')
            }
            _List_Retencao.push(_Retencao);
        });

        $('#Servicos_Jobs tbody tr').each(function () {
            var _Servico_Job = {
                cod_servico: parseInt($(this).closest('tr').find('td .cboServico').val()),
                cod_job: parseInt($(this).closest('tr').find('td .cboJob').val()),
                valor_servico_job: $(this).closest('tr').find('td .txtValorServicoJob').val()
            }
            _List_Servico_Job.push(_Servico_Job);
        });

        $('#Vencimento tbody tr').each(function () {
            var txtDataVencimento = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val());
            var txtValorVencimento = $(this).closest('tr').find('td .txtValorVencimento').val();

            if (txtDataVencimento != '' && txtValorVencimento != '') {
                var _Vencimento = {
                    data_vencimento: txtDataVencimento,
                    valor_vencimento: txtValorVencimento
                }
                _List_Vencimento.push(_Vencimento);
            }
        });

        $('#Narrativa tbody tr').each(function () {
            var _Narrativa = {
                cod_narrativa: parseInt($(this).closest('tr').find('td .cboNarrativa').val()),
                nome_narrativa: $(this).closest('tr').find('td .cboNarrativa :selected').text(),
                descricao_narrativa: $(this).closest('tr').find('td .txtDescricaoNarrativa').val()
            }
            _List_Narrativa.push(_Narrativa);
        });

        var EmissaoNF = {
            cod_emitente: _cod_emitente,
            cod_tomador: _cod_tomador,
            data_emissao_rps: _data_emissao_rps,
            data_rps: _data_rps,
            cod_natureza_operacao: _cod_natureza_operacao,
            nome_natureza_operacao: _nome_natureza_operacao,
            descricao_natureza_operacao: _descricao_natureza_operacao,
            natureza_operacao: _natureza_operacao,
            cod_prestacao_servico: _cod_prestacao_servico,
            nome_prestacao_servico: _nome_prestacao_servico,
            descricao_prestacao_servico: _descricao_prestacao_servico,
            valor_total_servico_job: _valor_total_servico_job,
            valor_total_liquido: _valor_total_liquido,
            valor_total_vencimento: _valor_total_vencimento,
            observacoes: _observacoes,
            List_Retencao: _List_Retencao,
            List_Servico_Job: _List_Servico_Job,
            List_Vencimento: _List_Vencimento,
            List_Narrativa: _List_Narrativa
        }

        var str = JSON.stringify(EmissaoNF);

        $.ajax({
            type: "POST",
            url: "../FormGrupoRetencoes.aspx/btnSalvar_Click",
            data: "{emissao_nf: " + str + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: Mensagem_Sucesso_Erro,
            error: Mensagem_Erro
        });
    }
    else {
        var mensagem_erros = '';

        for (var item in erros) {
            mensagem_erros += erros[item] + '\n';
        }
        alert(mensagem_erros);
    }
    $('.btnSalvar').prop('disabled', false);
}