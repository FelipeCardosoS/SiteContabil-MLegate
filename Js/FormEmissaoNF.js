$(function () {
    $(document).ready(function () {
        //Esconder ou Mostrar os itens que dependem da escolha do Emitente.
        if ($('.ddlEmitente').val() == '0')
            $('.Visivel').hide();
        else {
            $('.Visivel').show();
            $('.date').mask('99/99/9999'); //Data de Emissão.
            $('.dateCompetencia').mask('99/9999'); //Data de Competência.
        }

        //Serviços Jobs: Atualiza o valor do label "Total" quando o usuário voltar a página.
        let total = 0;

        $('.txtValorServicoJob').each(function () {
            if ($(this).val() != '') //Se o usuário voltar a página, o Valor estará preenchido com os dados anteriores e a página será recarregada.
                location.reload(); //Recarrega a tela.

            if ($(this).val() != '' && $(this).val() != ',' && $(this).val() != '.')
                total += parseFloat($(this).val().replaceAll('.', '').replace(',', '.'));
        });
        $('.lblTotal').html('Total: R$ ' + total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        Total_Liquido();

        //Vencimento: Atualiza o valor do label "Total" quando o usuário voltar a página.
        total = 0;

        $('.txtValorVencimento').each(function () {
            if ($(this).val() != '') //Se o usuário voltar a página, o Valor estará preenchido com os dados anteriores e a página será recarregada.
                location.reload(); //Recarrega a tela.

            if ($(this).val() != '' && $(this).val() != ',' && $(this).val() != '.')
                total += parseFloat($(this).val().replaceAll('.', '').replace(',', '.'));
        });
        $('.lblTotal_Vencimento').html('Total: R$ ' + total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

        //Observações: Limita o número máximo de quebras de linha (Enter), informado no atributo "attr('MaxLines')", o valor é atribuído no Front-End e o atributo 'MaxLines' não existe no contexto deste projeto, foi criado para atender esta necessidade.
        $('.MaxLines').keydown(function (event) {
            if (event.which == 13) { //Tabela ASCII (Enter)
                let numberOfLines = $(this).val().split('\n').length;

                if (numberOfLines >= $(this).attr('MaxLines'))
                    event.preventDefault();
            }
        });
    });

    //Job
    let temp = JSON.parse($('#div_Job input[type=hidden]').val());

    $('.cboJob').find('option').remove();
    $('.cboJob').append('<option value="0">Escolha</option>');
    $('.cboJobObs').find('option').remove();
    $('.cboJobObs').append('<option value="0"></option>');

    $(temp).each(function () {
        if ($(this)[0].status == 'A') { //Somente os Jobs Ativos estarão no combo.
            $('.cboJob').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].nome + '</option>');
            $('.cboJobObs').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].obsNF + '</option>');
        }
    });

    //Natureza da Operação
    $('.cboNaturezaOperacao').change(function () {
        let temp = JSON.parse($('#div_NaturezaOperacao input[type=hidden]').val());

        $(temp).each(function () {
            if ($('.cboNaturezaOperacao').val() == '0') {
                $('.txtDescricaoNO').val('');
                $('.txtNaturezaOperacao').val('');
                return false;
            }
            else if ($(this)[0].cod_natureza_operacao == $('.cboNaturezaOperacao').val()) {
                $('.txtDescricaoNO').val($(this)[0].descricao);
                $('.txtNaturezaOperacao').val($(this)[0].natureza_operacao);
                return false;
            }
        });
    });

    //Prestação de Serviço
    $('.cboPrestacaoServico').change(function () {
        let temp = JSON.parse($('#div_PrestacaoServico input[type=hidden]').val());

        $(temp).each(function () {
            if ($('.cboPrestacaoServico').val() == '0') {
                $('.txtDescricaoPS').val('');
                return false;
            }
            else if ($(this)[0].cod_prestacao_servico == $('.cboPrestacaoServico').val()) {
                $('.txtDescricaoPS').val($(this)[0].descricao);
                return false;
            }
        });
    });

    //Narrativa
    $(document).on('change', '.cboNarrativa', function () {
        let cboNarrativa = $(this).val();
        let txtDescricaoNarrativa = $(this).closest('tr').find('.txtDescricaoNarrativa');
        let temp = JSON.parse($('#div_Narrativa input[type=hidden]').val());

        $(temp).each(function () {
            if (cboNarrativa == '0') {
                txtDescricaoNarrativa.val('');
                return false;
            }
            else if ($(this)[0].cod_narrativa == cboNarrativa) {
                txtDescricaoNarrativa.val($(this)[0].descricao);
                return false;
            }
        });
    });

    //Serviços Jobs
    $(document).on('change', '.txtValorServicoJob', function () {
        Total();
        Total_Liquido();
        Vencimento_Parcelas();
        Vencimento_Total();
    });

    //Vencimento
    $(document).on('change', '.txtValorVencimento', function () {
        Vencimento_Total();
    });
});

function Retencao_Checked_Changed() {
    Total_Liquido();
    Vencimento_Parcelas();
    Vencimento_Total();
}

function Jobs_Inativos_Checked_Changed() {
    let temp = JSON.parse($('#div_Job input[type=hidden]').val());
    let Jobs_Inativos = $('.cbxJobsInativos').prop('checked');

    $('.cboJob').find('option').remove();
    $('.cboJob').append('<option value="0">Escolha</option>');
    $('.cboJobObs').find('option').remove();
    $('.cboJobObs').append('<option value="0"></option>');

    if (Jobs_Inativos == true) { //Todos os Jobs estarão no combo (Ativos e Inativos).
        $(temp).each(function () {
            $('.cboJob').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].nome + '</option>');
            $('.cboJobObs').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].obsNF + '</option>');
        });
    }
    else {
        $(temp).each(function () {
            if ($(this)[0].status == 'A') { //Somente os Jobs Ativos estarão no combo.
                $('.cboJob').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].nome + '</option>');
                $('.cboJobObs').append('<option value="' + $(this)[0].codigo + '">' + $(this)[0].obsNF + '</option>');
            }
        });
    }
}

function cboJob_Changed(Linha) {
    $(Linha).closest('tr').find('.cboJobObs').val($(Linha).closest('tr').find('.cboJob').val());

    if ($(Linha).closest('tr').find('.cboJob').val() == 0 || $(Linha).closest('tr').find('.cboJobObs :selected').text() == '')
        $(Linha).closest('tr').find('.cboJobObs').hide();
    else
        $(Linha).closest('tr').find('.cboJobObs').show();

    ObservacoesNF_Changed();
}

function Valor_Changed(Linha, Campo) {
    let Valor;

    if (Campo == 'txtValorServicoJob') {
        Valor = $(Linha).closest('tr').find('.txtValorServicoJob').val();

        if (Valor != '' && Valor != ',')
            $(Linha).closest('tr').find('.txtValorServicoJob').val(parseFloat(Valor.replaceAll('.', '').replace(',', '.')).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    }
    else if (Campo == 'txtValorVencimento') {
        Valor = $(Linha).closest('tr').find('.txtValorVencimento').val();

        if (Valor != '' && Valor != ',')
            $(Linha).closest('tr').find('.txtValorVencimento').val(parseFloat(Valor.replaceAll('.', '').replace(',', '.')).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    }
}

function Formata_Valor_Copiado(event) {
    let Valor = event.originalEvent == undefined ? event.clipboardData.getData('Text') : event.originalEvent.clipboardData.getData('Text');
    //Apaga todos os caracteres que não forem números ou vírgula. ([^0-9,])+)
    //Caso haja mais de uma vírgula, apaga tudo. (.*,+.*,+.*)
    let RegEx = new RegExp('([^0-9,])+|.*,+.*,+.*', 'g');

    event.preventDefault();
    event.target.value = Valor.replaceAll(RegEx, '');
    $(event.target).trigger('change');
}

function Adicionar_Servicos_Jobs() {
    $('.btnPlus_Servicos_Jobs').remove();

    $('#Servicos_Jobs tbody').append('<tr>'
        + '<td class="line" style="width:375px;"><select class="cboServico" style="width:350px;" /></td>'
        + '<td class="line" style="width:375px;"><select class="cboJob" onchange="cboJob_Changed(this)" style="width:350px;" /><select class="cboJobObs" style="width:350px; display:none;" disabled="disabled" /></td>'
        + '<td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="18" class="Monetaria txtValorServicoJob" onchange="Valor_Changed(this, \'txtValorServicoJob\')" onpaste="Formata_Valor_Copiado(event)" /></td>'
        + '<td class="line tdADD"><input type="button" value="+" class="btnPlus_Servicos_Jobs" onclick="Adicionar_Servicos_Jobs()" /></td>'
        + '<td class="line"><input type="button" value="-" class="btnMinus_Servicos_Jobs" onclick="Remover_Servicos_Jobs(this)" /></td>'
        + '</tr>');

    let select_cboServico = $('#Servicos_Jobs .cboServico:first').html();
    let select_cboJob = $('#Servicos_Jobs .cboJob:first').html();
    let select_cboJobObs = $('#Servicos_Jobs .cboJobObs:first').html();

    $('.cboServico:last').html(select_cboServico);
    $('.cboJob:last').html(select_cboJob);
    $('.cboJobObs:last').html(select_cboJobObs);
}

function Remover_Servicos_Jobs(Linha) {
    if ($('#Servicos_Jobs tbody tr').length > 1)
        $(Linha).closest('tr').remove();

    if ($(Linha).closest('tr').find('.tdADD').html() != '')
        $('#Servicos_Jobs tbody tr:last .tdADD').html('<input type="button" value="+" class="btnPlus_Servicos_Jobs" onclick="Adicionar_Servicos_Jobs()" />');

    Total();
    Total_Liquido();
    Vencimento_Parcelas();
    Vencimento_Total();
    ObservacoesNF_Changed();
}

//Serviços Jobs: Atualiza o valor do label "Total"
function Total() {
    let total = 0;

    $('.txtValorServicoJob').each(function () {
        if ($(this).val() != '' && $(this).val() != ',' && $(this).val() != '.')
            total += parseFloat($(this).val().replaceAll('.', '').replace(',', '.'));
    });

    $('.lblTotal').html('Total: R$ ' + total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));

    Calcular_Reforma_Tributaria(total);
}

//Serviços Jobs: Atualiza o valor do label "Total Líquido"
function Total_Liquido() {
    let total_aliquota_retencao = 0;
    let valor_total_servico_job = $('.lblTotal').text().replaceAll('.', '').substring(10);
    let valor_total_liquido = 0;

    $('#Retencao tbody tr input[type=checkbox]:checked').each(function () {
        total_aliquota_retencao += parseFloat($(this).closest('tr').find('td .aliquota_retencao').val().replaceAll('.', '').replace(',', '.'));
    });

    valor_total_liquido = parseFloat(valor_total_servico_job.replaceAll('.', '').replace(',', '.')) - ((parseFloat(valor_total_servico_job.replaceAll('.', '').replace(',', '.')) * total_aliquota_retencao) / 100);

    $('.lblTotal_Liquido').html('Total Líquido: R$ ' + valor_total_liquido.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}

function Adicionar_Vencimento() {
    $('.btnPlus_Vencimento').remove();

    $('#Vencimento tbody').append('<tr>'
        + '<td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="10" class="date txtDataVencimento" /></td>'
        + '<td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="18" class="Monetaria txtValorVencimento" onchange="Valor_Changed(this, \'txtValorVencimento\')" onpaste="Formata_Valor_Copiado(event)" /></td>'
        + '<td class="line tdADD_Vencimento"><input type="button" value="+" class="btnPlus_Vencimento" onclick="Adicionar_Vencimento()" /></td>'
        + '<td class="line"><input type="button" value="-" class="btnMinus_Vencimento" onclick="Remover_Vencimento(this)" /></td>'
        + '</tr>');

    $('.date').mask('99/99/9999'); //Data de Vencimento.
    Vencimento_Parcelas();
    Vencimento_Total();
}

function Remover_Vencimento(Linha) {
    if ($('#Vencimento tbody tr').length > 1)
        $(Linha).closest('tr').remove();

    if ($(Linha).closest('tr').find('.tdADD_Vencimento').html() != '')
        $('#Vencimento tbody tr:last .tdADD_Vencimento').html('<input type="button" value="+" class="btnPlus_Vencimento" onclick="Adicionar_Vencimento()" />');

    Vencimento_Parcelas();
    Vencimento_Total();
}

//Calcula o valor das parcelas de acordo com o Total Líquido.
function Vencimento_Parcelas() {
    let Numero_Parcelas = 0;

    $('.txtValorVencimento').each(function () {
        Numero_Parcelas += 1;
    });

    let Contador_Parcelas = 0;
    let Total_Liquido = parseFloat($('.lblTotal_Liquido').text().replaceAll('.', '').substring(18).replaceAll('.', '').replace(',', '.'));
    let Valor_Parcelas = (Total_Liquido / Numero_Parcelas).toFixed(2);

    $('.txtValorVencimento').each(function () {
        Contador_Parcelas += 1;

        if (Numero_Parcelas == 1 || Numero_Parcelas != Contador_Parcelas) //Contém apenas 1 parcela OU parcelas com o mesmo valor.
            $(this).val(parseFloat(Valor_Parcelas).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        else { //Última parcela: acerta os centavos do arredondamento.
            let Diferenca_Parcelas = (Total_Liquido - (parseFloat(Valor_Parcelas) * Numero_Parcelas)).toFixed(2);

            if (Diferenca_Parcelas < 0)
                $(this).val((parseFloat(Diferenca_Parcelas) + parseFloat(Valor_Parcelas)).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            else
                $(this).val((parseFloat(Valor_Parcelas) + parseFloat(Diferenca_Parcelas)).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        }
    });
}

//Atualiza o valor do label "Total"
function Vencimento_Total() {
    let total = 0;

    $('.txtValorVencimento').each(function () {
        if ($(this).val() != '' && $(this).val() != ',' && $(this).val() != '.')
            total += parseFloat($(this).val().replaceAll('.', '').replace(',', '.'));
    });

    $('.lblTotal_Vencimento').html('Total: R$ ' + total.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
}

function Adicionar_Narrativa() {
    $('.btnPlus_Narrativa').remove();

    $('#Narrativa tbody').append('<tr>'
        + '<td class="line" style="width:375px;"><select class="cboNarrativa" style="width:350px;" /></td>'
        + '<td class="line" style="width:575px;"><input type="text" class="txtDescricaoNarrativa" style="width:550px;" disabled="disabled" /></td>'
        + '<td class="line tdADD_Narrativa"><input type="button" value="+" class="btnPlus_Narrativa" onclick="Adicionar_Narrativa()" /></td>'
        + '<td class="line"><input type="button" value="-" class="btnMinus_Narrativa" onclick="Remover_Narrativa(this)" /></td>'
        + '</tr>');

    let select_cboNarrativa = $('#Narrativa .cboNarrativa:first').html();
    $('.cboNarrativa:last').html(select_cboNarrativa).val('0');
}

function Remover_Narrativa(Linha) {
    if ($('#Narrativa tbody tr').length > 1)
        $(Linha).closest('tr').remove();

    if ($(Linha).closest('tr').find('.tdADD_Narrativa').html() != '')
        $('#Narrativa tbody tr:last .tdADD_Narrativa').html('<input type="button" value="+" class="btnPlus_Narrativa" onclick="Adicionar_Narrativa()" />');
}

function ObservacoesNF_Changed() {
    $('.dateCompetencia').mask('99/9999'); //Data de Competência.

    let _observacoes_nf_jobs = '';
    let _observacoes = $('.txtObservacoes').val();
    let _data_competencia = $('.txtDataCompetencia').val();

    $('#Servicos_Jobs tbody tr').each(function () {
        if ($(this).closest('tr').find('td .cboJobObs :selected').text() != '')
            _observacoes_nf_jobs += $(this).closest('tr').find('td .cboJobObs :selected').text() + ' / ';
    });

    $('.lblCaracteres_Obs').text('Caracteres: ' + _observacoes.length);

    if (_observacoes_nf_jobs != '') {
        _observacoes_nf_jobs = _observacoes_nf_jobs.substr(0, _observacoes_nf_jobs.length - 3);

        if (_observacoes != '') {
            _observacoes_nf_jobs += ' / ' + _observacoes;

            if (_data_competencia != '')
                _observacoes_nf_jobs += ' / Competência: ' + _data_competencia;
        }
        else if (_data_competencia != '')
            _observacoes_nf_jobs += ' / Competência: ' + _data_competencia;
    }
    else if (_observacoes != '') {
        _observacoes_nf_jobs = _observacoes;

        if (_data_competencia != '')
            _observacoes_nf_jobs += ' / Competência: ' + _data_competencia;
    }
    else if (_data_competencia != '') {
        _observacoes_nf_jobs = 'Competência: ' + _data_competencia;
    }

    $('.txtObservacoesNF').val(_observacoes_nf_jobs);
    $('.lblCaracteres_Obs_NF').text('Caracteres: ' + _observacoes_nf_jobs.length);
}

function btnGerarNF_Click() {
    $('.btnGerarNF').prop('disabled', true);

    let erros = [];
    let Contador = 0;
    let Diferente = 0;
    let Repetido = 0;
    let List_Count = 0;
    let Tabela_Length = $('#Servicos_Jobs tbody tr').length;
    let Vencimento_OK = true;

    let _cod_tomador = parseInt($('.ddlCliente').val());
    let _data_emissao_rps = Converte_Data_EUA($('.txtDataEmissaoRPS').val(), 'Data de Emissão');
    let _data_rps = Converte_Data_EUA($('.txtDataRPS').val());
    let _data_competencia = $('.txtDataCompetencia').val();
    let _cod_natureza_operacao = parseInt($('.cboNaturezaOperacao').val());
    let _cod_prestacao_servico = parseInt($('.cboPrestacaoServico').val());
    let _valor_total_liquido = $('.lblTotal_Liquido').text().replaceAll('.', '').substring(18);
    let _valor_total_vencimento = $('.lblTotal_Vencimento').text().replaceAll('.', '').substring(10);

    if (_cod_tomador == 0)
        erros.push('Selecione um Tomador.');

    if (_data_emissao_rps == '')
        erros.push('Informe a Data de Emissão da Nota Fiscal.');
    else if (_data_emissao_rps.length > 10) //Se a Data de Emissão estiver no formato errado, terá mais de 10 caracteres.
        erros.push(_data_emissao_rps); //Mensagem de erro atribuída pelo método "Converte_Data_EUA" do "uteis.js".
    else if (parseInt(_data_emissao_rps.substr(6) + _data_emissao_rps.substr(0, 2) + _data_emissao_rps.substr(3, 2)) < parseInt(_data_rps.substr(6) + _data_rps.substr(0, 2) + _data_rps.substr(3, 2)))
        erros.push('Informe uma Data de Emissão igual ou posterior a Última Nota Fiscal Emitida.');

    if (_data_competencia != '') {
        _data_competencia = '01/' + _data_competencia;
        _data_competencia = Converte_Data_EUA(_data_competencia, 'Data de Competência');

        if (_data_competencia.length > 10) //Se a Data de Competência estiver no formato errado, terá mais de 10 caracteres.
            erros.push(_data_competencia); //Mensagem de erro atribuída pelo método "Converte_Data_EUA" do "uteis.js".
    }

    if (_cod_natureza_operacao == 0)
        erros.push('Selecione uma Natureza da Operação.');

    if (_cod_prestacao_servico == 0)
        erros.push('Selecione uma Prestação de Serviço.');

    //Serviço e Job
    $('#Servicos_Jobs tbody tr').each(function () {
        let cboServico = parseInt($(this).closest('tr').find('td .cboServico').val());
        let cboJob = parseInt($(this).closest('tr').find('td .cboJob').val());
        let txtValorServicoJob = $(this).closest('tr').find('td .txtValorServicoJob').val().replaceAll('.', '');

        if (cboServico == 0) {
            erros.push('Selecione um Serviço.');
            Contador += 1;
        }

        if (cboJob == 0) {
            erros.push('Selecione um Job.');
            Contador += 1;
        }

        try {
            if (txtValorServicoJob == '' || txtValorServicoJob == ',' || parseFloat(txtValorServicoJob.replace(',', '.')).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) == '0,00') {
                erros.push('Informe o Valor do Serviço.');
                Contador += 1;
            }
        } catch {
            erros.push('Valor do Serviço Inválido.');
            Contador += 1;
        }

        if (Contador > 0)
            return false;

        $('#Servicos_Jobs tbody tr').each(function () {
            let cboServico2 = parseInt($(this).closest('tr').find('td .cboServico').val());
            let cboJob2 = parseInt($(this).closest('tr').find('td .cboJob').val());

            if (cboServico != cboServico2)
                Diferente += 1;

            if (cboJob == cboJob2)
                Repetido += 1;
        });

        List_Count += 1;

        if (List_Count == Tabela_Length && Diferente != 0)
            erros.push('Informe apenas um tipo de Serviço.');

        if (List_Count == Tabela_Length && Repetido != Tabela_Length)
            erros.push('Informe Jobs diferentes.');
    });

    //Vencimento
    Contador = 0;
    Repetido = 0;
    List_Count = 0;
    Tabela_Length = $('#Vencimento tbody tr').length;

    $('#Vencimento tbody tr').each(function () {
        let txtDataVencimento = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val(), 'Data de Vencimento');
        let txtValorVencimento = $(this).closest('tr').find('td .txtValorVencimento').val();

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

        if (txtDataVencimento.length > 10) { //Se a Data de Vencimento estiver no formato errado, terá mais de 10 caracteres.
            erros.push(txtDataVencimento); //Mensagem de erro atribuída pelo método "Converte_Data_EUA" do "uteis.js".
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
                let txtDataVencimento2 = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val());

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
        let cboNarrativa = parseInt($(this).closest('tr').find('td .cboNarrativa').val());

        if (cboNarrativa == 0) {
            erros.push('Selecione uma Narrativa.');
            return false;
        }

        $('#Narrativa tbody tr').each(function () {
            let cboNarrativa2 = parseInt($(this).closest('tr').find('td .cboNarrativa').val());

            if (cboNarrativa == cboNarrativa2)
                Repetido += 1;
        });

        List_Count += 1;
        if (List_Count == Tabela_Length && Repetido != Tabela_Length)
            erros.push('Informe Narrativas diferentes.');
    });

    if (erros.length == 0) {
        if (_data_competencia != '' && parseInt(_data_emissao_rps.substr(6) + _data_emissao_rps.substr(0, 2)) != parseInt(_data_competencia.substr(6) + _data_competencia.substr(0, 2))) {
            let result = confirm('As datas de Emissão e Competência estão em períodos diferentes, deseja continuar?');

            if (result == false) {
                $('.btnGerarNF').prop('disabled', false);
                return;
            }
        }

        let _cod_emitente = parseInt($('.ddlEmitente').val());
        let _nome_natureza_operacao = $('.cboNaturezaOperacao :selected').text();
        let _descricao_natureza_operacao = $('.txtDescricaoNO').val();
        let _natureza_operacao = $('.txtNaturezaOperacao').val();
        let _nome_prestacao_servico = $('.cboPrestacaoServico :selected').text();
        let _descricao_prestacao_servico = $('.txtDescricaoPS').val();
        let _valor_total_servico_job = $('.lblTotal').text().replaceAll('.', '').substring(10);
        let _observacoes = $('.txtObservacoesNF').val();

        let _List_Retencao = [];
        let _List_Servico_Job = [];
        let _List_Vencimento = [];
        let _List_Narrativa = [];

        $('#Retencao tbody tr input[type=checkbox]:checked').each(function () {
            let _Retencao = {
                cod_retencao: parseInt($(this).closest('tr').find('td .cod_retencao').val()),
                nome_retencao: $(this).closest('tr').find('td .nome_retencao').val(),
                aliquota_retencao: $(this).closest('tr').find('td .aliquota_retencao').val(),
                apresentacao_retencao: $(this).closest('tr').find('td .apresentacao_retencao').val()
            }
            _List_Retencao.push(_Retencao);
        });

        $('#Servicos_Jobs tbody tr').each(function () {
            let _Servico_Job = {
                cod_servico: parseInt($(this).closest('tr').find('td .cboServico').val()),
                cod_job: parseInt($(this).closest('tr').find('td .cboJob').val()),
                valor_servico_job: $(this).closest('tr').find('td .txtValorServicoJob').val().replaceAll('.', '')
            }
            _List_Servico_Job.push(_Servico_Job);
        });

        $('#Vencimento tbody tr').each(function () {
            let txtDataVencimento = Converte_Data_EUA($(this).closest('tr').find('td .txtDataVencimento').val());
            let txtValorVencimento = $(this).closest('tr').find('td .txtValorVencimento').val().replaceAll('.', '');

            if (txtDataVencimento != '' && txtValorVencimento != '') {
                let _Vencimento = {
                    data_vencimento: txtDataVencimento,
                    valor_vencimento: txtValorVencimento
                }
                _List_Vencimento.push(_Vencimento);
            }
        });

        $('#Narrativa tbody tr').each(function () {
            let _Narrativa = {
                cod_narrativa: parseInt($(this).closest('tr').find('td .cboNarrativa').val()),
                nome_narrativa: $(this).closest('tr').find('td .cboNarrativa :selected').text(),
                descricao_narrativa: $(this).closest('tr').find('td .txtDescricaoNarrativa').val()
            }
            _List_Narrativa.push(_Narrativa);
        });

        if (_observacoes.length > 400) {
            let result = confirm('O campo "Observações" excedeu o limite máximo de 400 caracteres, deseja continuar?');

            if (result == false) {
                $('.btnGerarNF').prop('disabled', false);
                return;
            }
        }

        alert('Emitindo Nota Fiscal. Processando Informações.');

        let EmissaoNF = {
            cod_emitente: _cod_emitente,
            cod_tomador: _cod_tomador,
            data_emissao_rps: _data_emissao_rps,
            data_rps: _data_rps,
            data_competencia: _data_competencia,
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

        let str = JSON.stringify(EmissaoNF);

        $.ajax({
            type: "POST",
            url: "../FormEmissaoNF.aspx/btnGerarNF_Click",
            data: "{emissao_nf: " + str + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: Mensagem_Sucesso_Erro,
            error: Mensagem_Erro
        });
    }
    else {
        let mensagem_erros = '';

        for (let item in erros) {
            mensagem_erros += erros[item] + '\n';
        }
        alert(mensagem_erros);
    }
    $('.btnGerarNF').prop('disabled', false);

    // --- FUNÇÃO NOVA: Calcula IBS e CBS em Tempo Real ---
    function Calcular_Reforma_Tributaria(valorTotal) {
        // Regra de Teste 2026: IBS 0.1% e CBS 0.9%
        // Converto para Float seguro para evitar erros de arredondamento JS
        let valIBS = valorTotal * 0.001;
        let valCBS = valorTotal * 0.009;

        // Atualiza os campos na tela (requer que você tenha adicionado as classes CSS no ASPX conforme conversamos)
        // Se não achar os campos pela classe, tente pelo ID direto (ex: ctl00_areaConteudo_txtValorIBS)
        // Mas o ideal é manter as classes .txtValorIBS e .txtValorCBS que combinamos.

        if ($('.txtValorIBS').length > 0) {
            $('.txtValorIBS').val(valIBS.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
            $('.txtValorCBS').val(valCBS.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        }
    }

}