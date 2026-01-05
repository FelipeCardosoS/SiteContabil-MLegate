$(function () {
    $(document).ready(function () {
        //Esconder e Mostrar os itens que dependem da escolha do Emitente.
        if ($('.ddlEmitente').val() == '0')
            $('.Visivel').hide();
        else
            $('.Visivel').show();

        //Habilita os campos abaixo, quando carregar a parametrização realizada pelo usuário
        $('#Servicos tbody tr').each(function () {
            var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
            var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();

            if (cod_conta_debito != '0') {
                $(this).closest('tr').find('.ddlBrutoLiquidoDebito').prop('disabled', false);
                $(this).closest('tr').find('.cbxGeraTituloDebito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoDebito').prop('disabled', false);
            }

            if (cod_conta_credito != '0') {
                $(this).closest('tr').find('.ddlBrutoLiquidoCredito').prop('disabled', false);
                $(this).closest('tr').find('.cbxGeraTituloCredito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoCredito').prop('disabled', false);
            }
        });

        $('#Retencoes tbody tr').each(function () {
            var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
            var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();
            var gera_titulo_debito = $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked');
            var gera_titulo_credito = $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked');

            if (cod_conta_debito != '0') {
                $(this).closest('tr').find('.cbxGeraTituloDebito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoDebito').prop('disabled', false);

                if (gera_titulo_debito == true)
                    $(this).closest('tr').find('.ddlTerceiroDebito').prop('disabled', false);
            }

            if (cod_conta_credito != '0') {
                $(this).closest('tr').find('.cbxGeraTituloCredito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoCredito').prop('disabled', false);

                if (gera_titulo_credito == true)
                    $(this).closest('tr').find('.ddlTerceiroCredito').prop('disabled', false);
            }
        });

        $('#Tributos tbody tr').each(function () {
            var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
            var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();
            var gera_titulo_debito = $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked');
            var gera_titulo_credito = $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked');

            if (cod_conta_debito != '0') {
                $(this).closest('tr').find('.cbxGeraTituloDebito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoDebito').prop('disabled', false);

                if (gera_titulo_debito == true)
                    $(this).closest('tr').find('.ddlTerceiroDebito').prop('disabled', false);
            }

            if (cod_conta_credito != '0') {
                $(this).closest('tr').find('.cbxGeraTituloCredito').prop('disabled', false);
                $(this).closest('tr').find('.tbxHistoricoCredito').prop('disabled', false);

                if (gera_titulo_credito == true)
                    $(this).closest('tr').find('.ddlTerceiroCredito').prop('disabled', false);
            }
        });
    });

    $('.ddlContaDebito').change(function () {
        if ($(this).val() != '0') {
            $(this).closest('tr').find('.ddlBrutoLiquidoDebito').prop('disabled', false);
            $(this).closest('tr').find('.cbxGeraTituloDebito').prop('disabled', false);
            $(this).closest('tr').find('.tbxHistoricoDebito').prop('disabled', false);
        }
        else {
            $(this).closest('tr').find('.ddlBrutoLiquidoDebito').prop('disabled', true);
            $(this).closest('tr').find('.cbxGeraTituloDebito').prop('disabled', true);
            $(this).closest('tr').find('.tbxHistoricoDebito').prop('disabled', true);
            $(this).closest('tr').find('.ddlTerceiroDebito').prop('disabled', true);

            $(this).closest('tr').find('.ddlBrutoLiquidoDebito').val('0');
            $(this).closest('tr').find('.cbxGeraTituloDebito').prop('checked', false);
            $(this).closest('tr').find('.tbxHistoricoDebito').val('');
            $(this).closest('tr').find('.ddlTerceiroDebito').val('0');
        }
    });

    $('.ddlContaCredito').change(function () {
        if ($(this).val() != '0') {
            $(this).closest('tr').find('.ddlBrutoLiquidoCredito').prop('disabled', false);
            $(this).closest('tr').find('.cbxGeraTituloCredito').prop('disabled', false);
            $(this).closest('tr').find('.tbxHistoricoCredito').prop('disabled', false);
        }
        else {
            $(this).closest('tr').find('.ddlBrutoLiquidoCredito').prop('disabled', true);
            $(this).closest('tr').find('.cbxGeraTituloCredito').prop('disabled', true);
            $(this).closest('tr').find('.tbxHistoricoCredito').prop('disabled', true);
            $(this).closest('tr').find('.ddlTerceiroCredito').prop('disabled', true);

            $(this).closest('tr').find('.ddlBrutoLiquidoCredito').val('0');
            $(this).closest('tr').find('.cbxGeraTituloCredito').prop('checked', false);
            $(this).closest('tr').find('.tbxHistoricoCredito').val('');
            $(this).closest('tr').find('.ddlTerceiroCredito').val('0');
        }
    });
});

function cbxGeraTituloDebito_Click(checkbox) {
    if ($(checkbox).prop('checked')) {
        $(checkbox).closest('tr').find('.ddlTerceiroDebito').prop('disabled', false);
    }
    else {
        $(checkbox).closest('tr').find('.ddlTerceiroDebito').prop('disabled', true);
        $(checkbox).closest('tr').find('.ddlTerceiroDebito').val('0');
    }
}

function cbxGeraTituloCredito_Click(checkbox) {
    if ($(checkbox).prop('checked')) {
        $(checkbox).closest('tr').find('.ddlTerceiroCredito').prop('disabled', false);
    }
    else {
        $(checkbox).closest('tr').find('.ddlTerceiroCredito').prop('disabled', true);
        $(checkbox).closest('tr').find('.ddlTerceiroCredito').val('0');
    }
}

function btnSalvar_Click() {
    $('.btnSalvar').prop("disabled", true);
    var erros = [];

    //Serviços
    $('#Servicos tbody tr').each(function () {
        var nome_servico = $(this).closest('tr').find('td .tbxNomeServico').val();
        var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
        var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();
        var bruto_liquido_debito = $(this).closest('tr').find('td .ddlBrutoLiquidoDebito').val();
        var bruto_liquido_credito = $(this).closest('tr').find('td .ddlBrutoLiquidoCredito').val();
        var historico_debito = $(this).closest('tr').find('td .tbxHistoricoDebito').val();
        var historico_credito = $(this).closest('tr').find('td .tbxHistoricoCredito').val();
        
        //Débito e Crédito
        if (cod_conta_debito == '0' && cod_conta_credito == '0') {
            erros.push('Serviço ' + nome_servico + ': Selecione uma Conta de Débito e/ou Crédito. Respectivamente, selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.');
            return false;
        }

        if (cod_conta_debito != '0' && cod_conta_credito != '0' && bruto_liquido_debito == '0' && bruto_liquido_credito == '0' && historico_debito.trim() == '' && historico_credito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Débito e Crédito): Respectivamente, selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.');
            return false;
        }

        if (cod_conta_debito != '0' && cod_conta_credito != '0' && bruto_liquido_debito == '0' && bruto_liquido_credito == '0' && historico_debito.trim() != '' && historico_credito.trim() != '') {
            erros.push('Serviço ' + nome_servico + ' (Débito e Crédito): Respectivamente, selecione Valor Bruto ou Líquido.');
            return false;
        }

        if (cod_conta_debito != '0' && cod_conta_credito != '0' && bruto_liquido_debito != '0' && bruto_liquido_credito != '0' && historico_debito.trim() == '' && historico_credito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.');
            return false;
        }

        //Débito
        if (cod_conta_debito != '0' && bruto_liquido_debito == '0' && historico_debito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Débito): Selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.');
            return false;
        }

        if (cod_conta_debito != '0' && bruto_liquido_debito == '0' && historico_debito.trim() != '') {
            erros.push('Serviço ' + nome_servico + ' (Débito): Selecione Valor Bruto ou Líquido.');
            return false;
        }

        if (cod_conta_debito != '0' && bruto_liquido_debito != '0' && historico_debito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Débito): Informe o Histórico do lançamento.');
            return false;
        }

        //A função "$('.ddlContaDebito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
        var gera_titulo_debito = $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked');
        
        if (cod_conta_debito == '0' && (bruto_liquido_debito != '0' || gera_titulo_debito == true || historico_debito.trim() != '')) {
            erros.push('Serviço ' + nome_servico + ': Selecione uma Conta de Débito.');
            return false;
        }

        //Crédito
        if (cod_conta_credito != '0' && bruto_liquido_credito == '0' && historico_credito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Crédito): Selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.');
            return false;
        }

        if (cod_conta_credito != '0' && bruto_liquido_credito == '0' && historico_credito.trim() != '') {
            erros.push('Serviço ' + nome_servico + ' (Crédito): Selecione Valor Bruto ou Líquido.');
            return false;
        }

        if (cod_conta_credito != '0' && bruto_liquido_credito != '0' && historico_credito.trim() == '') {
            erros.push('Serviço ' + nome_servico + ' (Crédito): Informe o Histórico do lançamento.');
            return false;
        }

        //A função "$('.ddlContaCredito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
        var gera_titulo_credito = $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked');

        if (cod_conta_credito == '0' && (bruto_liquido_credito != '0' || gera_titulo_credito == true || historico_credito.trim() != '')) {
            erros.push('Serviço ' + nome_servico + ': Selecione uma Conta de Crédito.');
            return false;
        }
    });

    //Retenções
    if (erros.length == 0) {
        $('#Retencoes tbody tr').each(function () {
            var nome_retencao = $(this).closest('tr').find('td .tbxNomeRetencao').val();
            var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
            var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();
            var gera_titulo_debito = $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked');
            var gera_titulo_credito = $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked');
            var cod_terceiro_debito = parseInt($(this).closest('tr').find('td .ddlTerceiroDebito').val());
            var cod_terceiro_credito = parseInt($(this).closest('tr').find('td .ddlTerceiroCredito').val());
            var historico_debito = $(this).closest('tr').find('td .tbxHistoricoDebito').val();
            var historico_credito = $(this).closest('tr').find('td .tbxHistoricoCredito').val();

            //Débito e Crédito
            if (cod_conta_debito == '0' && cod_conta_credito == '0') {
                erros.push('Retenção ' + nome_retencao + ': Selecione uma Conta de Débito e/ou Crédito. Respectivamente, informe o Histórico do lançamento.');
                return false;
            }

            if (cod_conta_debito != '0' && cod_conta_credito != '0' && historico_debito.trim() == '' && historico_credito.trim() == '') {
                erros.push('Retenção ' + nome_retencao + ' (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_debito == true && gera_titulo_credito == true && cod_terceiro_debito == 0 && cod_terceiro_credito == 0) {
                erros.push('Retenção ' + nome_retencao + ' (Débito e Crédito): Respectivamente, selecione um Terceiro.');
                return false;
            }

            //Débito
            if (cod_conta_debito != '0' && historico_debito.trim() == '') {
                erros.push('Retenção ' + nome_retencao + ' (Débito): Informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_debito == true && cod_terceiro_debito == 0) {
                erros.push('Retenção ' + nome_retencao + ' (Débito): Selecione um Terceiro.');
                return false;
            }

            //A função "cbxGeraTituloDebito_Click", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (gera_titulo_debito == false && cod_terceiro_debito != 0) {
                erros.push('Retenção ' + nome_retencao + ' (Débito): Selecione Gera Título, pois um Terceiro foi selecionado.');
                return false;
            }

            //A função "$('.ddlContaDebito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (cod_conta_debito == '0' && (gera_titulo_debito == true || cod_terceiro_debito != 0 || historico_debito.trim() != '')) {
                erros.push('Retenção ' + nome_retencao + ': Selecione uma Conta de Débito.');
                return false;
            }

            //Crédito
            if (cod_conta_credito != '0' && historico_credito.trim() == '') {
                erros.push('Retenção ' + nome_retencao + ' (Crédito): Informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_credito == true && cod_terceiro_credito == 0) {
                erros.push('Retenção ' + nome_retencao + ' (Crédito): Selecione um Terceiro.');
                return false;
            }

            //A função "cbxGeraTituloCredito_Click", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (gera_titulo_credito == false && cod_terceiro_credito != 0) {
                erros.push('Retenção ' + nome_retencao + ' (Crédito): Selecione Gera Título, pois um Terceiro foi selecionado.');
                return false;
            }

            //A função "$('.ddlContaCredito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (cod_conta_credito == '0' && (gera_titulo_credito == true || cod_terceiro_credito != 0 || historico_credito.trim() != '')) {
                erros.push('Retenção ' + nome_retencao + ': Selecione uma Conta de Crédito.');
                return false;
            }
        });
    }

    //Tributos
    if (erros.length == 0) {
        $('#Tributos tbody tr').each(function () {
            var nome_tributo = $(this).closest('tr').find('td .tbxNomeTributo').val();
            var cod_conta_debito = $(this).closest('tr').find('td .ddlContaDebito').val();
            var cod_conta_credito = $(this).closest('tr').find('td .ddlContaCredito').val();
            var gera_titulo_debito = $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked');
            var gera_titulo_credito = $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked');
            var cod_terceiro_debito = parseInt($(this).closest('tr').find('td .ddlTerceiroDebito').val());
            var cod_terceiro_credito = parseInt($(this).closest('tr').find('td .ddlTerceiroCredito').val());
            var historico_debito = $(this).closest('tr').find('td .tbxHistoricoDebito').val();
            var historico_credito = $(this).closest('tr').find('td .tbxHistoricoCredito').val();

            //Débito e Crédito
            if (cod_conta_debito == '0' && cod_conta_credito == '0') {
                erros.push('Tributo ' + nome_tributo + ': Selecione uma Conta de Débito e/ou Crédito. Respectivamente, informe o Histórico do lançamento.');
                return false;
            }

            if (cod_conta_debito != '0' && cod_conta_credito != '0' && historico_debito.trim() == '' && historico_credito.trim() == '') {
                erros.push('Tributo ' + nome_tributo + ' (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_debito == true && gera_titulo_credito == true && cod_terceiro_debito == 0 && cod_terceiro_credito == 0) {
                erros.push('Tributo ' + nome_tributo + ' (Débito e Crédito): Respectivamente, selecione um Terceiro.');
                return false;
            }

            //Débito
            if (cod_conta_debito != '0' && historico_debito.trim() == '') {
                erros.push('Tributo ' + nome_tributo + ' (Débito): Informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_debito == true && cod_terceiro_debito == 0) {
                erros.push('Tributo ' + nome_tributo + ' (Débito): Selecione um Terceiro.');
                return false;
            }

            //A função "cbxGeraTituloDebito_Click", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (gera_titulo_debito == false && cod_terceiro_debito != 0) {
                erros.push('Tributo ' + nome_tributo + ' (Débito): Selecione Gera Título, pois um Terceiro foi selecionado.');
                return false;
            }

            //A função "$('.ddlContaDebito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (cod_conta_debito == '0' && (gera_titulo_debito == true || cod_terceiro_debito != 0 || historico_debito.trim() != '')) {
                erros.push('Tributo ' + nome_tributo + ': Selecione uma Conta de Débito.');
                return false;
            }

            //Crédito
            if (cod_conta_credito != '0' && historico_credito.trim() == '') {
                erros.push('Tributo ' + nome_tributo + ' (Crédito): Informe o Histórico do lançamento.');
                return false;
            }

            if (gera_titulo_credito == true && cod_terceiro_credito == 0) {
                erros.push('Tributo ' + nome_tributo + ' (Crédito): Selecione um Terceiro.');
                return false;
            }

            //A função "cbxGeraTituloCredito_Click", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (gera_titulo_credito == false && cod_terceiro_credito != 0) {
                erros.push('Tributo ' + nome_tributo + ' (Crédito): Selecione Gera Título, pois um Terceiro foi selecionado.');
                return false;
            }

            //A função "$('.ddlContaCredito').change", já impossibilita entrar nesta validação. Porém, se a função não funcionar, esta validação irá travar.
            if (cod_conta_credito == '0' && (gera_titulo_credito == true || cod_terceiro_credito != 0 || historico_credito.trim() != '')) {
                erros.push('Tributo ' + nome_tributo + ': Selecione uma Conta de Crédito.');
                return false;
            }
        });
    }

    //Contas A Receber - Conta Diferença
    if (erros.length == 0) {
        var _cod_conta_diferenca = $('.ddlContaDiferenca').val();
        var _debito_credito = $('.ddlDebitoCredito').val();
        var _historico = $('.tbxHistorico').val();

        if (_cod_conta_diferenca == '0')
            erros.push('Selecione uma Conta A Receber.');

        if (_debito_credito == '0')
            erros.push('Contas A Receber: Selecione Débito ou Crédito.');

        if (_historico.trim() == '')
            erros.push('Contas A Receber: Informe o Histórico do lançamento.');
    }

    //Validação OK
    if (erros.length == 0) {
        var _cod_emitente = parseInt($('.ddlEmitente').val());
        var _gera_titulo = $('.cbxGeraTitulo').prop('checked');
        var _insert = $('.cbxInsertContaDiferenca').prop('checked');
        var _update = $('.cbxUpdateContaDiferenca').prop('checked');

        var _List_Servico = [];
        var _List_Retencao = [];
        var _List_Tributo = [];

        $('#Servicos tbody tr').each(function () {
            var _Servico = {
                insert: $(this).closest('tr').find('td .cbxInsert').prop('checked'),
                update: $(this).closest('tr').find('td .cbxUpdate').prop('checked'),
                cod_servico: parseInt($(this).closest('tr').find('td .cod_servico').val()),
                nome_servico: $(this).closest('tr').find('td .tbxNomeServico').val(),

                cod_conta_debito: $(this).closest('tr').find('td .ddlContaDebito').val(),
                bruto_liquido_debito: $(this).closest('tr').find('td .ddlBrutoLiquidoDebito').val(),
                gera_titulo_debito: $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked'),
                historico_debito: $(this).closest('tr').find('td .tbxHistoricoDebito').val(),

                cod_conta_credito: $(this).closest('tr').find('td .ddlContaCredito').val(),
                bruto_liquido_credito: $(this).closest('tr').find('td .ddlBrutoLiquidoCredito').val(),
                gera_titulo_credito: $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked'),
                historico_credito: $(this).closest('tr').find('td .tbxHistoricoCredito').val()
            }
            _List_Servico.push(_Servico);
        });

        $('#Retencoes tbody tr').each(function () {
            var _Retencao = {
                insert: $(this).closest('tr').find('td .cbxInsert').prop('checked'),
                update: $(this).closest('tr').find('td .cbxUpdate').prop('checked'),
                cod_retencao: parseInt($(this).closest('tr').find('td .cod_retencao').val()),
                nome_retencao: $(this).closest('tr').find('td .tbxNomeRetencao').val(),

                cod_conta_debito: $(this).closest('tr').find('td .ddlContaDebito').val(),
                gera_titulo_debito: $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked'),
                cod_terceiro_debito: parseInt($(this).closest('tr').find('td .ddlTerceiroDebito').val()),
                historico_debito: $(this).closest('tr').find('td .tbxHistoricoDebito').val(),

                cod_conta_credito: $(this).closest('tr').find('td .ddlContaCredito').val(),
                gera_titulo_credito: $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked'),
                cod_terceiro_credito: parseInt($(this).closest('tr').find('td .ddlTerceiroCredito').val()),
                historico_credito: $(this).closest('tr').find('td .tbxHistoricoCredito').val()
            }
            _List_Retencao.push(_Retencao);
        });

        $('#Tributos tbody tr').each(function () {
            var _Tributo = {
                insert: $(this).closest('tr').find('td .cbxInsert').prop('checked'),
                update: $(this).closest('tr').find('td .cbxUpdate').prop('checked'),
                cod_tributo: parseInt($(this).closest('tr').find('td .cod_tributo').val()),
                nome_tributo: $(this).closest('tr').find('td .tbxNomeTributo').val(),

                cod_conta_debito: $(this).closest('tr').find('td .ddlContaDebito').val(),
                gera_titulo_debito: $(this).closest('tr').find('td .cbxGeraTituloDebito').prop('checked'),
                cod_terceiro_debito: parseInt($(this).closest('tr').find('td .ddlTerceiroDebito').val()),
                historico_debito: $(this).closest('tr').find('td .tbxHistoricoDebito').val(),

                cod_conta_credito: $(this).closest('tr').find('td .ddlContaCredito').val(),
                gera_titulo_credito: $(this).closest('tr').find('td .cbxGeraTituloCredito').prop('checked'),
                cod_terceiro_credito: parseInt($(this).closest('tr').find('td .ddlTerceiroCredito').val()),
                historico_credito: $(this).closest('tr').find('td .tbxHistoricoCredito').val()
            }
            _List_Tributo.push(_Tributo);
        });

        var Contabilizacao = {
            cod_emitente: _cod_emitente,
            cod_conta_diferenca: _cod_conta_diferenca,
            debito_credito: _debito_credito,
            historico: _historico,
            gera_titulo: _gera_titulo,
            insert: _insert,
            update: _update,
            List_Servico: _List_Servico,
            List_Retencao: _List_Retencao,
            List_Tributo: _List_Tributo
        }

        var str = JSON.stringify(Contabilizacao);

        $.ajax({
            type: "POST",
            url: "../FormContabilizacao.aspx/btnSalvar_Click",
            data: "{contabilizacao: " + str + "}",
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