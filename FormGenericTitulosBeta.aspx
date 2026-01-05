<%@ Page Language="C#" MasterPageFile="~/MasterFormBeta.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGenericTitulosBeta.aspx.cs" Inherits="FormGenericTitulos" %>

<%@ MasterType VirtualPath="~/MasterFormBeta.master" %>
<asp:Content ID="areaHead" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <link type="text/css" href="Css/w2ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="Js/w2ui.js"></script>
    <script type="text/javascript" src="Js/jquery.maskedinput.js"></script>
    <style>
        /* CSS WGRID */
        #tb_grid_toolbar_item_w2ui-reload, #tb_grid_toolbar_item_w2ui-search, #tb_grid_toolbar_item_w2ui-break0 {
            display: none !important;
        }

        input[readonly="readonly"] {
            color: #000 !important;
            font-size: 12px;
        }

        #cmbDiferenca, #cmbCredito, #cmbDebito {
            width: 80px;
            text-align: right;
        }
        /*************/

        .gridLancamentos tbody tr td {
            padding: 0px !important;
        }

        div#boxTotalizador {
            top: 0px !important;
        }

        fieldset.form_inclusao_titulo_beta {
            margin: 0;
        }

        fieldset.form_inclusao_titulo_beta {
            width: 998px;
            padding-bottom: 5px !important;
            border-top: 2px solid #252E57;
        }

            fieldset.form_inclusao_titulo_beta .linha_form {
                display: block;
                margin: 3px 0px 0px 0px;
                /*overflow: hidden;*/
                height: 45px;
            }

            fieldset.form_inclusao_titulo_beta label {
                display: table;
                padding: 0;
                width: auto;
                float: left;
                position: relative;
                margin: 0px 5px 0px 5px;
                left: 4px;
                height: 45px;
            }

                fieldset.form_inclusao_titulo_beta label span {
                    display: table;
                    padding: 0;
                    width: auto;
                    float: none;
                    position: relative;
                    top: 2px;
                }

                fieldset.form_inclusao_titulo_beta label input, fieldset.form_inclusao_titulo_beta label select {
                    display: inline-block;
                    padding: 0;
                    width: auto;
                    float: left;
                    position: relative;
                    top: 4px;
                    padding: 2px;
                    border: 1px solid #C6CFF7;
                }

                fieldset.form_inclusao_titulo_beta label select {
                    padding: 1px;
                }

                fieldset.form_inclusao_titulo_beta label.gera_credito {
                    top: 17px;
                }

                    fieldset.form_inclusao_titulo_beta label.gera_credito span {
                        color: red;
                        display: inline-block;
                    }

                    fieldset.form_inclusao_titulo_beta label.gera_credito input {
                        float: none;
                        border: 0px;
                    }

                fieldset.form_inclusao_titulo_beta label input.botaoCalendario {
                    background: transparent url(../../Imagens/icones/calendar.gif);
                    background-position: left;
                    background-repeat: no-repeat;
                    border: 0px;
                    width: 20px;
                    height: 20px;
                    position: relative;
                    left: 4px;
                    cursor: pointer;
                }

        fieldset label {
            text-align: left !important;
        }

        .btnRefresh {
            float: left;
            cursor: pointer;
            padding: 2px 7px !important;
            margin: 1px !important;
            float: right;
        }

        .table-head {
            margin: 0;
            padding: 0;
            border-right: 1px solid #c5c5c5;
            border-bottom: 1px solid #c5c5c5;
            color: #000;
            background-image: -webkit-linear-gradient(#f9f9f9,#e4e4e4);
            background-image: -moz-linear-gradient(#f9f9f9,#e4e4e4);
            background-image: -ms-linear-gradient(#f9f9f9,#e4e4e4);
            background-image: -o-linear-gradient(#f9f9f9,#e4e4e4);
            background-image: linear-gradient(#f9f9f9,#e4e4e4);
            filter: progid:dximagetransform.microsoft.gradient(startColorstr='#fff9f9f9', endColorstr='#ffe4e4e4', GradientType=0);
            cursor: default;
            overflow: hidden;
            line-height: 100%;
            box-sizing: border-box;
            display: table-cell;
            vertical-align: inherit;
            border-spacing: 0;
            border-collapse: collapse;
            font-family: Verdana,Arial,sans-serif;
            font-size: 11px;
        }

        .w2ui-col-header {
            padding: 7px 3px;
            white-space: nowrap;
            text-overflow: ellipsis;
            overflow: hidden;
            position: relative;
        }

        #boxLoad {
            float: left;
            margin-top: 7px;
            color: #FF0000;
            font-weight: bold;
            width: 300px;
            height: 20px;
            text-align: left;
        }
    </style>
    <span style="display: none;">
        <asp:Button ID="botaoVencimento" runat="server" /></span>
    <asp:Label ID="boxModelo" CssClass="boxModelo" runat="server" Visible="false" Text=""></asp:Label>
    <div id="box_etapas">
        <div id="etapa1" class="etapa">
            <span class="etapa_numero">1</span>
            <span class="etapa_texto">Folha de Lançamento Beta</span>
        </div>
        <div id="etapa2" class="etapa">
            <span class="etapa_numero">2</span>
            <span class="etapa_texto">Escolha do Tipo de Nota Fiscal</span>
        </div>
        <div id="etapa3" class="etapa">
            <span class="etapa_numero">3</span>
            <span class="etapa_texto">Nota Fiscal</span>
        </div>
        <div style="float: right;">
            <asp:Label ID="labelLote" CssClass="boxLote" runat="server" Visible="false" Text=""></asp:Label>
        </div>
        <div id="etapaMoedaInclusaoLancto">
            <div style="float: left;">
                <span class="etapaTipoMoeda">Tipo de Moeda</span>
                <span class="etapaComboMoeda">
                    <asp:DropDownList ID="comboEtapaMoeda" runat="server"></asp:DropDownList></span>
            </div>
            <div style="float: left; padding: 7px 9px;">
                <img src="Imagens/divisa.gif" />
            </div>
            <div style="float: left; padding: 10px 0 0 0;">
                <asp:Button ID="botaoCotacao" runat="server" Text="" CssClass="btnCotacao" OnClientClick="loadCotacao(1,0); return false;" />
                <asp:Button ID="botaoCotacaoAbre" runat="server" Style="display: none;" Text="" />
            </div>
        </div>
    </div>
    <div id="panelEtapa1">
        <fieldset id="form_inclusao_titulo" class="form_inclusao_titulo_beta" runat="server">
            <div class="linha_form">
                <label id="labelTerceiro" runat="server">
                    <span>
                        <asp:Literal ID="ltTerceiro" runat="server"></asp:Literal></span>
                    <div>
                        <input id="comboFornecedor" style="width: 400px;">
                        <span class="legend"></span></div>
                    <%--<asp:DropDownList ID="comboFornecedor" class="chosen" data-placeholder="Escolha o fornecedor..." tabindex="1" Width="480px" runat="server"></asp:DropDownList>--%>
                </label>
                <label id="labelData" runat="server">
                    <span>Data</span>
                    <asp:TextBox ID="textData" Width="80px" Enabled="false" runat="server"></asp:TextBox>
                </label>
                <label id="labelNumeroDocumento" runat="server">
                    <span>Nº Documento</span>
                    <asp:TextBox ID="textNumeroDocumento" Enabled="false" Width="80px" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="textNumeroDocumento_old" runat="server" />
                </label>
                <label id="labelModelo" runat="server" class="lado">
                    <span>Modelo</span>
                    <asp:DropDownList ID="comboModelo" class="chosen" data-placeholder="Escolha o modelo..." Enabled="false" TabIndex="1" Width="280px" runat="server">
                    </asp:DropDownList>
                </label>
                <label id="labelEtapaMoeda" style="float: right;" runat="server" visible="False">
                    <div style="float: left;">
                        <span class="etapaTipoMoeda">Tipo de Moeda</span>
                        <span class="etapaComboMoeda">
                            <asp:DropDownList ID="comboEtapaMoedaInclusaLancto" runat="server"></asp:DropDownList></span>
                    </div>
                    <div style="float: left; padding: 7px 9px;">
                        <img src="Imagens/divisa.gif" />
                    </div>
                    <div style="float: left; padding: 10px 0 0 0;">
                        <asp:Button ID="botaoCotacaoInclusaoLancto" runat="server" Text="" CssClass="btnCotacao" OnClientClick="loadCotacao(1,0); return false;" />
                        <asp:Button ID="botaoCotacaoInclusaoLanctoAbre" runat="server" Style="display: none;" Text="" />
                    </div>
                </label>
            </div>
            <div class="linha_form">
                <label id="labelConsultor" runat="server" class="lado">
                    <span>Consultor</span>
                    <asp:DropDownList ID="comboConsultor" Enabled="false" Width="280px" runat="server">
                    </asp:DropDownList>
                </label>
                <label id="labelQtd" runat="server">
                    <span>Qtde</span>
                    <asp:HiddenField ID="textQtd_old" runat="server" />
                    <asp:TextBox ID="textQtd" Enabled="false" Width="60px" runat="server"></asp:TextBox>
                </label>
                <label id="labelValorUnit" runat="server">
                    <span>Valor Unitário</span>
                    <asp:HiddenField ID="textValorUnit_old" runat="server" />
                    <asp:TextBox ID="textValorUnit" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelLiquido" runat="server">
                    <span>Valor Total</span>
                    <asp:TextBox ID="textValorTotal" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelValorBruto" runat="server">
                    <span>Valor Bruto</span>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:TextBox ID="textValorBruto" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelParcelas" runat="server" style="display: none;">
                    <span>N° Parcelas</span>
                    <asp:TextBox ID="textParcelas" Enabled="false" Width="90px" runat="server"></asp:TextBox>
                    <input id="btVencimento" class="botaoCalendario" disabled="disabled" type="button" runat="server" />
                </label>
                <%--<label id="labelNumeroDocumento_lancto" runat="server">
                    <span><asp:Label ID="textGrupoDoc" runat="server" Text="Nº Doc"></asp:Label></span>
                    <asp:TextBox ID="textNumeroDocumento_lancto" Width="80px" runat="server"></asp:TextBox>
                    <asp:TextBox ID="textGrupo" Visible="false" Width="80px" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="dataHidden" runat="server" />
                </label>--%>
            </div>
        </fieldset>
        <fieldset id="formLancamento" class="formLancamento" style="display: none;">
            <asp:HiddenField ID="H_SEQLOTE_LANCTO" runat="server" />
            <asp:HiddenField ID="H_SEQLOTE_LANCTO_MIN" runat="server" />
            <asp:HiddenField ID="H_SEQLOTE_LANCTO_MAX" runat="server" />
            <div class="linha_form">
                <label>
                    <span>D/C</span>
                    <asp:DropDownList ID="comboDebCred_lancto" Enabled="false" Width="50px" runat="server">
                        <asp:ListItem Text="N/D" Selected="True" Value="0"></asp:ListItem>
                        <asp:ListItem Text="D" Value="D"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                </label>
                <label>
                    <span>Conta</span>
                    <asp:DropDownList ID="comboConta_lancto" Enabled="false" Width="300px" runat="server">
                    </asp:DropDownList>
                </label>
                <label for="3426868">
                    <span>
                        <div>Job</div>
                        <asp:CheckBox ToolTip="" ID="checkJobAtivo" Text="Ativos" CssClass="checkJobAtivo" runat="server" /></span>
                    <asp:DropDownList ID="comboJob_lancto" Enabled="false" Width="300px" runat="server" />
                </label>
                <label>
                    <span>Qtd</span>
                    <asp:TextBox ID="textQtd_lancto" Enabled="false" Width="20px" runat="server"></asp:TextBox>
                </label>
                <label>
                    <span>Valor Unit($)</span>
                    <asp:TextBox ID="textValorUnit_lancto" Enabled="false" Width="70px" runat="server"></asp:TextBox>
                </label>
                <label>
                    <span>Valor($)</span>
                    <asp:TextBox ID="textValor_lancto" Enabled="false" Width="70px" runat="server"></asp:TextBox>
                </label>
            </div>
            <script>
                $(document).ready(function () {
                    $(".btnexp").toggle(function () {
                        $(this).css("background-position", "0px -10px");
                        $(".expand").show();
                        $(tela.comboCliente_lancto + "_chosen").css("width","120px").trigger("chosen:updated");
                    }, function () {
                        $(this).css("background-position", "0px 0px");
                        $(".expand").hide();
                    });
                });
            </script>
            <div class="linha_form">
                <div class="btnexp"></div>
                <div class="expand">
                    <label>
                        <span>Linha Negócio</span>
                        <asp:DropDownList ID="comboLinhaNegocio_lancto" Enabled="false" Width="120px" runat="server">
                        </asp:DropDownList>
                    </label>
                    <label>
                        <span>Divisão</span>
                        <asp:DropDownList ID="comboDivisao_lancto" Enabled="false" Width="120px" runat="server">
                        </asp:DropDownList>
                    </label>
                    <label>
                        <span>Cliente</span>
                        <asp:DropDownList ID="comboCliente_lancto" Enabled="false" Width="190px" runat="server">
                        </asp:DropDownList>
                    </label>
                </div>
                <label>
                    <span>Consultor</span>
                    <asp:DropDownList ID="comboConsultor_lancto" Width="120px" runat="server">
                    </asp:DropDownList>
                </label>
                <label>
                    <span>Gera Título</span>
                    <asp:DropDownList ID="comboGeraTitulo_lancto" Enabled="false" runat="server">
                        <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                        <asp:ListItem Text="Não" Selected="True" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </label>
                <div id="celulaParcelasTitulo" style="display: none;">
                    <label>
                        <span>N°Parc.</span>
                        <asp:TextBox ID="textParcelas_lancto" Enabled="false" Width="25px" runat="server"></asp:TextBox>
                        <input id="btVencimento_lancto" class="botaoCalendario" type="button" runat="server" />
                    </label>
                </div>
                <div id="celulaFornecedorTitulo" style="display: none;">
                    <label>
                        <span>Terceiro</span>
                        <asp:DropDownList ID="comboTerceiro_lancto" Enabled="false" Style="width: 250px !important;" runat="server"></asp:DropDownList>
                    </label>
                </div>
            </div>
            <div class="linha_form">
                <asp:TextBox ID="textHistorico_lancto" MaxLength="1000" Enabled="false" TextMode="MultiLine" Width="980px" runat="server"></asp:TextBox>
            </div>
            <div class="linha_form">
                <span id="boxLoadd" style="display: none;">Carregando...</span>
                <input type="button" disabled="disabled" name="botaoCancelar_lancto" id="botaoCancelar_lancto" class="botao" value="Cancelar" runat="server" />
                <input type="button" disabled="disabled" name="botaoSalvar_lancto" id="botaoSalvar_lancto" class="botao" value="Incluir" runat="server" />
            </div>
        </fieldset>
        <div id="boxLog" style="width: 100%; height: 600px;"></div>
        <asp:Literal ID="logLiteral" runat="server" />
        <script type="text/javascript">

            var cod_usuario = <%=HttpContext.Current.Session["usuario"] %>;

            var debcred = [{ id: '-', text: '-' }, { id: 'D', text: 'D' }, { id: 'C', text: 'C' }];
            var gera_titulo = [{ id: 'Não', text: 'Não' }, { id: 'Sim', text: 'Sim' }];
            var cmbTerceiro = [<%=comboTerceiro_lanctoBeta %>];
            var cmbConsultor = [<%=comboConsultor_lanctoBeta %>];
            var cmbConta = [<%=comboConta_lanctoBeta %>];
            var cmbJob = [<%=comboJob_lanctoBeta %>];

            var cmbCliente = [<%=comboTerceiro_lanctoBeta %>];
            var cmbLinhaNegocio = [<%=comboLinhaNegocio %>];
            var cmbDivisao = [<%=comboDivisao %>];

            var optMoeda = [<%=htmlMoeda %>];
            var htmlMoeda = "";

            var htmlCotacaoMoeda = "";
            var htmlParcelas = "";

            var linhaEdit = 0;

            var arrayParcelaDados = [];
            var arrayCotacaoDados = [];

            $(document).ready(function () {                
                switch (get('modulo')){
                    case 'C_INCLUSAO_LANCTO':
                        $("fieldset.form_inclusao_titulo_beta").css("height","57px");
                        break;
                    default:
                        $('#comboFornecedor').w2field('list', { items: cmbTerceiro, match: 'contains', selected: '0' }).change(changeFornecedor);
                        break;
                }                
                $("body").dblclick(function(event) {
                    if(event.target.id != "totalParcelas" && event.target.classList[0] != "parcValor" && event.target.classList[0] != "parcData" && event.target.classList[0] != "valorMoeda"){
                        if($(".w2ui-expanded-row").length){
                            offExpand(eventEdit, linhaEdit);
                        }
                    }
                });
            });

            function montaHtmlMoeda()
            {
                htmlMoeda = '<div id="gridMoeda"><table style="width:100%; border-bottom: 1px solid #CCC;">';

                for (var x = 0; x < optMoeda.length; x++)
                {
                    htmlMoeda += '<tr class="w2ui-' + ((x % 2) == 0 ? 'odd' : 'even') + '"><td align="right"> ' + optMoeda[x].text + ' </td><td> <input class="valorMoeda" type="text" value="0,00" onkeypress="return Onlynumbers(event);"><input type="hidden" value="' + optMoeda[x].id + '" class="codMoeda"></td></tr>';
                }
                htmlMoeda += '</table></div>';
            }

            function verificaUltimaColuna()
            {
                var record = w2ui['grid'].records[w2ui['grid'].records.length - 1];
            }

            function verificaNovaLinha()
            {
                var record = w2ui['grid'].records[w2ui['grid'].records.length - 1];

                if (record.conta != '0' || record.cliente != '0' || record.consultor != '0' || record.divisao != '0' || record.gera_titulo != 'Não' || record.historico != '' || record.job != '0' || record.linha_negocio != '0' || record.qtd != 0 || record.terceiro != '0' || record.valor != 0 || record.valor_unit != 0)
                {
                    w2ui['grid'].set(w2ui['grid'].records.length, { style:''});
                    w2ui['grid'].add({ recid: (w2ui['grid'].records.length + 1), grupo: '0', dc: '-', conta: '0', job: '0', cliente: '0', divisao: '0', linha_negocio: '0', qtd: '0', valor_unit: '0', valor: '0', historico: '', gera_titulo: 'Não', terceiro: '0', consultor: '0', style: "background-color: #FFF877;" });
                }
            }

            function atualizaSaldo()
            {
                var debito = 0;
                var credito = 0;
                var diferenca = 0;
                
                for (var index in w2ui['grid'].records)
                {
                    if (w2ui['grid'].records[index].dc == 'D')
                        debito += w2ui['grid'].records[index].valor;
                    else if (w2ui['grid'].records[index].dc == 'C')
                        credito += w2ui['grid'].records[index].valor;
                }

                diferenca = debito - credito;
                diferenca = (diferenca < 0 ? diferenca * (-1) : diferenca);

                $("#cmbDebito").val(debito.toFixed(2).toString().replace(".",","));
                $("#cmbCredito").val(credito.toFixed(2).toString().replace(".", ","));
                $("#cmbDiferenca").val(diferenca.toFixed(2).toString().replace(".", ","));
            }

            function atualizaGridParcelas(linha)
            {
                var parcelas = parseFloat($("#totalParcelas").val());
                var total = (parcelas * 25) + 25;
                var totalHeigthCotacao = (parseFloat(optMoeda.length) * 25) + 25;
                total = (totalHeigthCotacao > total ? totalHeigthCotacao : total);
                var htmlGrid = "";

                for (var x = 0; x < parcelas; x++)
                {
                    htmlGrid += '<tr class="w2ui-' + ((x % 2) == 0 ? 'even' : 'odd') + '"><td align="center" width="10%"> ' + (x + 1) + 'º </td><td width="45%"><input class="parcData" type="text" value=""></td><td width="45%"><input style="text-align:right;" class="parcValor" onkeypress="return Onlynumbers(event);" onchange="mudaSomatoria();" type="text" value="0,00"></td></tr>';
                }

                $("#gridParcelas").html('<table style="width:100%;border-bottom: 1px solid #CCC;"><tbody>'+htmlGrid+'</tbody></table>');
                $(".parcData").mask("99/99/9999").change(verificaData);
                $("#grid_grid_rec_" + linha + "_expanded").css("height", total+"px");
            }

            $(function () {
                montaHtmlMoeda();

                $('#boxLog').w2grid({
                    name: 'grid',
                    show: {
                        toolbar: true,
                        //selectColumn: true,
                        expandColumn: true,
                        toolbarDelete: true,
                        footer: true
                    },
                    reorderColumns: true,
                    columns: [
                        { field: 'recid', caption: 'Nº de Linhas', size: '100%', hidden: true },
                        { field: 'grupo', caption: 'Grupo', size: '55px', editable: { type: 'int', min: 0, max: 32756 } },
                        {
                            field: 'dc', caption: 'D/C', size: '41px', 
                            editable: { type: 'select', items: debcred },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in debcred) {
                                    if (debcred[p].id == this.getCellValue(index, col_index)) html = debcred[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'conta', caption: 'Conta', size: '175px', 
                            editable: { type: 'select', items: cmbConta },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbConta) {
                                    if (cmbConta[p].id == this.getCellValue(index, col_index)) html = cmbConta[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'job', caption: 'Job', size: '10%', 
                            editable: { type: 'select', items: cmbJob },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbJob) {
                                    if (cmbJob[p].id == this.getCellValue(index, col_index)) html = cmbJob[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'cliente', caption: 'Cliente', size: '89px', hidden: true, 
                            editable: { type: 'select', items: cmbCliente },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbCliente) {
                                    if (cmbCliente[p].id == this.getCellValue(index, col_index)) html = cmbCliente[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'divisao', caption: 'Divisão', size: '89px', hidden: true, 
                            editable: { type: 'select', items: cmbDivisao },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbDivisao) {
                                    if (cmbDivisao[p].id == this.getCellValue(index, col_index)) html = cmbDivisao[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'linha_negocio', caption: 'Linha Negócio', size: '89px', hidden: true, 
                            editable: { type: 'select', items: cmbLinhaNegocio },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbLinhaNegocio) {
                                    if (cmbLinhaNegocio[p].id == this.getCellValue(index, col_index)) html = cmbLinhaNegocio[p].text;
                                }
                                return html;
                            }
                        },
                        { field: 'qtd', caption: 'Qtd', size: '37px', render: 'money', editable: { type: 'money', "groupSymbol": " " } },
                        { field: 'valor_unit', caption: 'Valor Unit($)', size: '98px',  render: 'money', editable: { type: 'money', "groupSymbol": " " } },
                        { field: 'valor', caption: 'Valor($)', size: '69px', render: 'money', editable: { type: 'money', "groupSymbol": " " } },
                        { field: 'historico', caption: 'Histórico', size: '152px', editable: { type: 'text' } },
                        {
                            field: 'gera_titulo', caption: 'Gera Título', size: '89px', 
                            editable: { type: 'select', items: gera_titulo },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in gera_titulo) {
                                    if (gera_titulo[p].id == this.getCellValue(index, col_index)) html = gera_titulo[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'terceiro', caption: 'Terceiro', size: '150px', 
                            editable: { type: 'select', items: cmbTerceiro },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbTerceiro) {
                                    if (cmbTerceiro[p].id == this.getCellValue(index, col_index)) html = cmbTerceiro[p].text;
                                }
                                return html;
                            }
                        },
                        {
                            field: 'consultor', caption: 'Consultor', size: '150px', hidden: true,
                            editable: { type: 'select', items: cmbConsultor },
                            render: function (record, index, col_index) {
                                var html = '';
                                for (var p in cmbConsultor) {
                                    if (cmbConsultor[p].id == this.getCellValue(index, col_index)) html = cmbConsultor[p].text;
                                }
                                return html;
                            }
                        }
                    ],
                    records: [
                        { recid: 1, grupo: '0', dc: '-', conta: '0', job: '0', cliente: '0', divisao: '0', linha_negocio: '0', qtd: '0', valor_unit: '0', valor: '0', historico: '', gera_titulo: 'Não', terceiro: '0', consultor: '0', style: "background-color: #FFF877;" }
                    ],
                    onExpand: function (event) {
                        linhaEdit = event.recid;
                        eventEdit = event;
                        offExpand(event, event.recid);
                    },
                    onChange: function (event) {                        
                        var record = w2ui['grid'].get(event.recid);                        
                        var nomeColuna = retornaNomeColuna(event.column);

                        //if(w2ui['grid'].records[event.recid - 1] === undefined)
                        //{
                        //    w2ui['grid'].records[event.recid - 1] = { grupo: '0', dc: '-', conta: '0', job: '0', cliente: '0', divisao: '0', linha_negocio: '0', qtd: '0', valor_unit: '0', valor: '0', historico: '', gera_titulo: 'Não', terceiro: '0', consultor: '0' };
                        //}

                        switch (nomeColuna) {
                            case 'cliente':
                                break;
                            case 'consultor':
                                break;
                            case 'conta':
                                w2ui['grid'].get(event.recid).conta = event.value_new;
                                w2ui['grid'].get(event.recid).descConta = $("#" + event.input_id + " option:selected").text();
                                w2ui['grid'].save();
                                verificaNovaLinha();
                                break;
                            case 'dc':
                                w2ui['grid'].get(event.recid).dc = event.value_new;
                                w2ui['grid'].save();
                                atualizaSaldo();
                                break;
                            case 'divisao':
                                break;
                            case 'gera_titulo':
                                w2ui['grid'].get(event.recid).gera_titulo = event.value_new;
                                w2ui['grid'].save();
                                verificaNovaLinha();
                                if (w2ui['grid'].records[(event.recid)-1].gera_titulo == "Sim") {
                                    offExpand(event, event.recid); 
                                    if ($("#totalParcelas").val() == '0') {
                                        $("#totalParcelas").val('1'); 
                                        atualizaGridParcelas(event.recid);
                                    }                                    
                                }else {
                                    $("#totalParcelas").val('0');
                                    atualizaGridParcelas(event.recid);
                                }
                                break;
                            case 'grupo':
                                w2ui['grid'].get(event.recid).grupo = event.value_new;
                                w2ui['grid'].save();
                                break;
                            case 'historico':
                                w2ui['grid'].get(event.recid).historico = event.value_new;
                                w2ui['grid'].save();
                                verificaNovaLinha();
                                break;
                            case 'job':
                                w2ui['grid'].get(event.recid).job = event.value_new;
                                w2ui['grid'].get(event.recid).jobDesc = $("#" + event.input_id + " option:selected").text();
                                changeLinhaNegocioDivisaoCLiente(event.value_new, event.recid);
                                w2ui['grid'].save();
                                verificaNovaLinha();
                                break;
                            case 'linha_negocio':
                                break;
                            case 'qtd':
                                w2ui['grid'].get(event.recid).qtd = event.value_new;
                                w2ui['grid'].get(event.recid).valor = event.value_new * w2ui['grid'].records[event.recid - 1].valor_unit;
                                w2ui['grid'].save();
                                atualizaSaldo();
                                verificaNovaLinha();
                                break;
                            case 'terceiro':
                                w2ui['grid'].get(event.recid).terceiro = event.value_new;
                                w2ui['grid'].get(event.recid).terceiroDesc = $("#" + event.input_id + " option:selected").text();
                                w2ui['grid'].save();
                                verificaNovaLinha();
                                break;
                            case 'valor':
                                w2ui['grid'].get(event.recid).valor = event.value_new;
                                w2ui['grid'].get(event.recid).valor_unit = event.value_new / w2ui['grid'].records[event.recid - 1].qtd;
                                w2ui['grid'].save();
                                atualizaSaldo();
                                verificaNovaLinha();
                                break;
                            case 'valor_unit':
                                w2ui['grid'].get(event.recid).valor_unit = event.value_new;
                                w2ui['grid'].get(event.recid).valor = w2ui['grid'].records[event.recid - 1].qtd * event.value_new;
                                w2ui['grid'].save();
                                atualizaSaldo();
                                verificaNovaLinha();
                                break;
                        }

                        console.log(event);
                    },
                    onDelete: function(event) {
                        if(event.force == true) {
                            //var sel = this.getSelection();
                            //w2ui['grid'].records.splice((sel[0] - 1),1);
                        }
                        
                        atualizaSaldo();
                    },  
                    onError: function (event) {
                        console.log(event);
                    }
                });

                //.on({ type: 'delete', execute: 'after' }, function (target, info) {
                //    reorganizaIndices();
                //    verificaNovaLinha();
                //});
                //var g = w2ui['grid'].records.length,
                //w2ui['grid'].add({ recid: 13, grupo: '9', dc: 'C', conta: {text:'11111 - Caixa'}, job: 'Geral', cliente: 'Mlegate', divisao: 'Geral', linha_negocio: 'Geral', qtd: '1', valor_unit: '100', valor: '1000', historico: 'Teste ', gera_titulo: 'Sim', terceiro: 'Teste' }),
            });

            function changeFornecedor()
            {
                var existeLoteAnterior = false;
                var buscaLoteAnterior = false;
                var erroGetListaModelos = 0;

                $.ajax({
                    type: "POST",
                    url: "FormGenericTitulosBeta.aspx/loadTerceiro",
                    data: "{terceiro: " + returnComboValue("FORNECEDOR", $("#comboFornecedor").val()) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {
                        $("#boxLoad").show();
                        $("#boxLoad").html("Buscando Terceiro...");
                    },
                    success: function (r) {
                        var result = r.d;
                        if (tela.modulo == "CAP_INCLUSAO_TITULO")
                            modelo = result.sugestaoModeloCP;
                        else
                            modelo = result.sugestaoModeloCR;
                    },
                    error: OnFailed,
                    complete: function() {
                        var lote = _GET('lote');
                        $.ajax({
                            type: "POST",
                            url: "FormGenericTitulosBeta.aspx/totalLotesAnteriores",
                            data: "{modulo: '" + _GET('modulo') + "', codigoTerceiro: " + returnComboValue("FORNECEDOR", $("#comboFornecedor").val()) + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {
                                $("#boxLoad").html("Contando lotes anteriores do terceiro...");
                            },
                            success: function (r) {
                                var result = r.d;

                                if (result > 0 && lote == null) {
                                    existeLoteAnterior = true;
                                }
                            },
                            error: OnFailed,
                            complete: function () {
                                buscaLoteAnterior = false;
                                if (existeLoteAnterior && lote == null) {
                                    buscaLoteAnterior = window.confirm("Deseja carregar a ultima folha realizada criada para o fornecedor?");
                                }
                                var data = $("#ctl00_areaConteudo_textData").val();
                                if (data == "") {
                                    //data = $(tela.dataHidden).val(),
                                    var now = new Date();
                                    data = now.format("dd/MM/yyyy");
                                }
                                if (buscaLoteAnterior && lote == null) {
                                    $.ajax({
                                        type: "POST",
                                        url: "FormGenericTitulosBeta.aspx/carregaUltimoLote",
                                        data: "{modulo:'" + _GET('modulo') + "',  codigoTerceiro: " + returnComboValue("FORNECEDOR", $("#comboFornecedor").val()) + ", data: '" + data + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        beforeSend: function () {
                                            $("#boxLoad").html("Carregando a última folha de lançamento..");
                                        },
                                        success: function (r) {
                                            var result = r.d;
                                            $("#boxLoad").hide();
                                            if (result != null || result != undefined) {
                                                if (result.length > 0) {
                                                    montaGridLanctosBeta(result);
                                                    tela.calculaTotais(result);
                                                    tela.ativaFormTitulo();

                                                    $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                                                    $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                                                    $(tela.textQtd).val(number_format(result[0].qtd, 4, ',', '.'));
                                                    $(tela.textValorUnit).val(number_format(result[0].valorUnit, 2, ',', '.'));
                                                    $(tela.textValorBruto).val(number_format(result[0].valorBruto, 2, ',', '.'));
                                                    $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                                                    //$(tela.comboFornecedor).val(result[0].terceiro),
                                                    //$(tela.textData).val(result[0].dataLancamento_formatada),
                                                    $(tela.textParcelas).val(result[0].vencimentos.length);
                                                    tela.carregaModelo(result[0].terceiro, result[0].modelo);
                                                    $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                                                    $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                                                    var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                                                    newSeqLote++;
                                                    $(tela.hdSeqLote_lancto).val(newSeqLote);

                                                    $(tela.textQtd_lancto).val("1");
                                                    $(tela.textValorUnit_lancto).val("0");
                                                    $(tela.textValor_lancto).val("0");
                                                    $(tela.textParcelas_lancto).val("1");
                                                    $(tela.comboConsultor).val(result[0].codConsultor);
                                                }
                                                else {
                                                    //$(tela.textNumeroDocumento).val(""),
                                                    $(tela.textParcelas).val("1");
                                                    $(tela.textQtd).val("1");
                                                    $(tela.textValorTotal).val("0");
                                                    $(tela.textValorUnit).val("0");
                                                    $(tela.textValorBruto).val("0");
                                                    $(tela.hdSeqLote_lancto).val("2");
                                                    $(tela.hdSeqLoteMin_lancto).val("1");
                                                    $(tela.hdSeqLoteMax_lancto).val("1");
                                                    $(tela.textQtd_lancto).val("1");
                                                    $(tela.textValor_lancto).val("0");
                                                    $(tela.textValorUnit_lancto).val("0");
                                                    $(tela.textParcelas_lancto).val("1");
                                                    tela.desativaFormTitulo();
                                                    tela.desativaFormLancamento();
                                                }
                                            } else {
                                                //$(tela.textNumeroDocumento).val(""),
                                                $(tela.textParcelas).val("1");
                                                $(tela.textQtd).val("1");
                                                $(tela.textValorTotal).val("0");
                                                $(tela.textValorUnit).val("0");
                                                $(tela.hdSeqLote_lancto).val("2");
                                                $(tela.hdSeqLoteMin_lancto).val("1");
                                                $(tela.hdSeqLoteMax_lancto).val("1");
                                                $(tela.textQtd_lancto).val("1");
                                                $(tela.textValor_lancto).val("0");
                                                $(tela.textValorUnit_lancto).val("0");
                                                $(tela.textParcelas_lancto).val("1");
                                                tela.desativaFormTitulo();
                                                tela.desativaFormLancamento();
                                            }
                                        },
                                        error: OnFailed
                                    });
                                } else {
                                    $.ajax({
                                        type: "POST",
                                        url: "FormGenericTitulosBeta.aspx/getListaModelos",
                                        data: "{modulo: '" + tela.modulo + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        beforeSend: function () {
                                            $("#boxLoad").html("Pegando lista de modelos...");
                                        },
                                        success: function (r) {
                                            var result = r.d;
                                            if (result != null || result != undefined) {
                                                if (result.length > 0) {
                                                    $("#ctl00_areaConteudo_comboModelo").html("");
                                                    $("<option value='0'>Escolha</option>").appendTo("#ctl00_areaConteudo_comboModelo");

                                                    if (modelo != 0) {
                                                        var existeModelo = 0;
                                                        for (var i = 0; i < result.length; i++) {
                                                            if (result[i]["COD_MODELO"] == modelo)
                                                                existeModelo++;
                                                            if (result[i]["PADRAO_DEFAULT"] == "True") {
                                                                $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected' >" + result[i]["NOME"] + "</option>").appendTo("#ctl00_areaConteudo_comboModelo");
                                                                $("#ctl00_areaConteudo_comboModelo").append('<li class="active-result">' + result[i]["NOME"] + '</li>');
                                                            } else {
                                                                $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo("#ctl00_areaConteudo_comboModelo");
                                                            }
                                                        }

                                                        if (existeModelo > 0) {
                                                            $("#ctl00_areaConteudo_comboModelo").val(modelo);
                                                        }
                                                    }
                                                    else {
                                                        for (var i = 0; i < result.length; i++) {
                                                            if (result[i]["PADRAO_DEFAULT"] == "True") {
                                                                $("<option value='" + result[i]["COD_MODELO"] + "' selected='selected'>" + result[i]["NOME"] + "</option>").appendTo("#ctl00_areaConteudo_comboModelo");
                                                                modelo = result[i]["COD_MODELO"];
                                                            }
                                                            else {
                                                                $("<option value='" + result[i]["COD_MODELO"] + "'>" + result[i]["NOME"] + "</option>").appendTo("#ctl00_areaConteudo_comboModelo");
                                                            }

                                                        }
                                                    }
                                                }
                                                else {
                                                    erroGetListaModelos++;
                                                    alert("Não existe nenhum modelo para contas à pagar cadastrado no sistema.");
                                                    tela.desativaFormTitulo();
                                                    tela.desativaFormLancamento();
                                                }
                                            }
                                            else {
                                                erroGetListaModelos++;
                                            }
                                        },
                                        complete: function () {
                                            if (erroGetListaModelos == 0 && lote == null) {
                                                var comboModelo = $("#ctl00_areaConteudo_comboModelo").val();
                                                var comboFornecedor_dt = $('#comboFornecedor').data('selected').id;
                                                var comboFornecedor_opt = $(tela.prefixo + "comboFornecedor").find("option:selected").text();
                                                var textValorBruto = 0;
                                                if ($(tela.prefixo + "textValorBruto").val() != undefined &&  $(tela.prefixo + "textValorBruto").val() != null) {
                                                    textValorBruto = $(tela.prefixo + "textValorBruto").val().replace(",", ".");
                                                }                                                
                                                var cbetapmoeda = 0;
                                                if ( $(tela.prefixo + "comboEtapaMoeda").val() != undefined &&  $(tela.prefixo + "comboEtapaMoeda").val() != null) {
                                                    cbetapmoeda = $(tela.prefixo + "comboEtapaMoeda").val();
                                                }    
                                                $.ajax({
                                                    type: "POST",
                                                    url: "FormGenericTitulosBeta.aspx/loadLanctosModelo",
                                                    data: "{modulo:'" + tela.modulo + "', cod_modelo: " + comboModelo + ", terceiroEscolhido: " + comboFornecedor_dt + ", descTerceiroEscolhido: '" + comboFornecedor_opt + "', data: '" + data + "', valorbruto: '" + textValorBruto + "', codmoeda: '" + cbetapmoeda + "'}",
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    beforeSend: function () {
                                                        $("#boxLoad").html("Pegando lançamentos do modelo...");
                                                    },
                                                    success: function (r) {
                                                        var result = r.d;
                                                        $("#boxLoad").hide();
                                                        if (result != null && result != undefined) {
                                                            if (result.length > 0) {
                                                                montaGridLanctosBeta(result);

                                                                $(tela.hdSeqLoteMin_lancto).val(result[0].seqLote);
                                                                $(tela.hdSeqLoteMax_lancto).val(result[eval(result.length - 1)].seqLote);

                                                                var newSeqLote = eval(result[eval(result.length - 1)].seqLote);
                                                                newSeqLote++;

                                                                $(tela.hdSeqLote_lancto).val(newSeqLote);

                                                                tela.ativaFormTitulo();
                                                                $(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                                                                $(tela.textNumeroDocumento_lancto).val(result[0].numeroDocumento);
                                                                $(tela.textValorTotal).val(number_format(result[0].valor, 2, ',', '.'));
                                                                $(tela.textParcelas).val(result[0].vencimentos.length);
                                                                $(tela.comboConsultor).val(result[0].codConsultor);

                                                                if (tela.exigeModelo) {
                                                                    $(tela.comboConta_lancto).prop('disabled', 'true');
                                                                    $(tela.botaoSalvar_lancto).prop('disabled', 'true');
                                                                }
                                                            }
                                                            else {
                                                                alert("O modelo escolhido não possui lançamentos configurados.");
                                                                desativaFormTitulo();
                                                                desativaFormLancamento();
                                                            }
                                                        }
                                                        else {
                                                            alert("O modelo escolhido não possui lançamentos configurados.");
                                                            tela.desativaFormTitulo();
                                                            tela.desativaFormLancamento();
                                                        }
                                                    },
                                                    error: OnFailed
                                                });
                                            } else { $("#boxLoad").html(""); }
                                        },
                                        error: OnFailed
                                    });
                                }
                            }
                        });
                    }
                });
            }

            function carregaModelo(terceiro, modeloSelecionado) {

                var erroGetListaModelos = 0;
                var modelo = 0;
                var existeLoteAnterior = false;

                if(modeloSelecionado <= 0)
                {


                }
                else
                {
                    modelo = modeloSelecionado;
        
                    $.ajax({
                        type: "POST",
                        url: "FormGenericTitulos.aspx/getListaModelos",
                        data: "{modulo: '"+tela.modulo+"'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function()
                        {
                            $("#boxLoad").show();
                            $("#boxLoad").html("Buscando lista de modelos...");
                        },
                        success: function(r)
                        {
                            var result = r.d;
                            $("#boxLoad").hide();
                            if(result != null || result != undefined)
                            {
                                if(result.length > 0)
                                {
                                    $(tela.comboModelo).html("");
                                    $("<option value='0'>Escolha</option>").appendTo(tela.comboModelo);
                        
                                    if(modelo != 0)
                                    {
                                        for(var i=0;i<result.length;i++)
                                        {
                                            if(result[i]["COD_MODELO"] == modelo)
                                                $("<option value='"+result[i]["COD_MODELO"]+"' selected='selected'>"+result[i]["NOME"]+"</option>").appendTo(tela.comboModelo);
                                            else
                                                $("<option value='"+result[i]["COD_MODELO"]+"'>"+result[i]["NOME"]+"</option>").appendTo(tela.comboModelo);
                                        }
                                    }
                                    else
                                    {
                                        for(var i=0;i<result.length;i++)
                                        {
                                            if(result[i]["PADRAO_DEFAULT"] == "True")
                                            {
                                                $("<option value='"+result[i]["COD_MODELO"]+"' selected='selected'>"+result[i]["NOME"]+"</option>").appendTo(tela.comboModelo);
                                                modelo = result[i]["COD_MODELO"];
                                            }
                                            else
                                            {
                                                $("<option value='"+result[i]["COD_MODELO"]+"'>"+result[i]["NOME"]+"</option>").appendTo(tela.comboModelo);
                                            }
                            
                                        }
                                    }
                                }
                                else
                                {
                                    alert("Não existe nenhum modelo para contas à pagar cadastrado no sistema.");
                        
                                    tela.desativaFormTitulo();
                                    tela.desativaFormLancamento();
                                }
                            }
                        },
                        error: OnFailed
                    });
                }
            
                destravaCamposCabecalho();
            }

            function alteraModelo(modelo)
            {
                
                var realiza = false;

                realiza = window.confirm("Alterando o modelo você irá perder todos os lançamentos criados, deseja continuar?");
    
                if(realiza)
                {
                    var arrayTerceiro = cmbTerceiro.filter(function(obj){ return obj.text == $(this.comboFornecedor).val(); });

                    if(arrayTerceiro[0] !== undefined){
                        var terceiro = arrayTerceiro[0].id;
                        var descTerceiro = arrayTerceiro[0].text;
                    } else {
                        var terceiro = 0;
                        var descTerceiro = '';
                    }
        
                    $.ajax({
                        type: "POST",
                        url: "FormGenericTitulos.aspx/loadLanctosModelo",
                        data: "{modulo:'" + tela.modulo + "', cod_modelo: " + modelo + ", terceiroEscolhido: " + terceiro + ", descTerceiroEscolhido: '" + descTerceiro + "',data:'" + $(tela.textData).val() + "', valorbruto:0 , codmoeda:0}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function()
                        {
                            $("#boxLoad").show();
                            $("#boxLoad").html("Alterando modelo..");
                        },
                        success: function(r)
                        {
                            var result = r.d;
                            $("#boxLoad").hide();
                            if(result != null && result != undefined)
                            {
                                if(result.length > 0)
                                {
                                    montaGridLanctosBeta(result);

                                    //$(tela.textNumeroDocumento).val(result[0].numeroDocumento);
                                    //$(tela.textValorTotal).val(number_format(result[0].valor,2,',','.'));
                                    //$(tela.comboConsultor).val(result[0].codConsultor);
                                }
                                else
                                {
                                    alert("O modelo escolhido não possui lançamentos configurados.");
                                }
                            }
                            else
                            {
                                alert("O modelo escolhido não possui lançamentos configurados.");
                            }
                        },
                        error: OnFailed
                    });     
                }
            }

            function salvaFolha(input) {
                $(input).prop('disabled', 'true');
                atualizaSaldo();
                verificaNovaLinha();
                //Verifico se o grid está aberto
                if($(".w2ui-expanded-row").length){
                    offExpand(eventEdit, linhaEdit);
                }
                var val = valida();
                if (val == '') {
                    //Monta Folha
                    var str = JSON.stringify(sincronizaLancto());
                
                    $.ajax({
                        type: "POST",
                        async: false,
                        url: "FormGenericTitulosBeta.aspx/salvaLanctoBeta",
                        data: "{modulo:'" + tela.modulo + "',listLancto: " + str + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (){
                            $(input).val("Processando...");
                            $(input).prop('disabled', 'true');
                        },
                        success: function (r) {
                            result = r.d;
                        },
                        complete: function (a) {
                            if (result != undefined) {
                                if (result.length == 0) {
                                    alert("Folha salva com sucesso.");
                                    document.location.reload();
                                }
                                else {
                                    var texto = "";
                                    for (var i = 0; i < result.length; i++) {
                                        texto += result[i] + "\n";
                                    }

                                    alert(texto);
                                }
                                $(input).val("Próximo");
                                $(input).removeAttr("disabled");
                            }
                        },
                        error: OnFailed
                    });

                    var result;
                    if (tela.modulo != "MODELO_LANCAMENTO") {
                        if (tela.executaEtapa()) {
                            if (tela.etapaAtual == 1) {
                                if (!verificaGeraCredito()) {
                                    tela.salvaTela("MIN");
                                    return false;
                                }
                            }
                            if (tela.etapaAtual < 3) {
                                tela.etapaAtual++;
                            }
                            tela.exibeEtapa();
                        }
                    } else {
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: "FormGenericTitulos.aspx/salvaLanctoBeta",
                            data: "{modulo:'" + tela.modulo + "',listLancto: '" + $(tela.textData).val() + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {
                                $(input).val("Processando...");
                                $(input).prop('disabled', 'true');
                            },
                            success: function (r) {

                                result = r.d;

                            },
                            complete: function (a) {
                                if (result != undefined) {
                                    if (result.length == 0) {
                                        alert("Folha salva com sucesso.");
                                        document.location.reload();
                                    }
                                    else {
                                        var texto = "";
                                        for (var i = 0; i < result.length; i++) {
                                            texto += result[i] + "\n";
                                        }

                                        alert(texto);
                                    }
                                    $(input).val("Próximo");
                                    $(input).removeAttr("disabled");
                                }
                            },
                            error: OnFailed
                        });
                    }
                }else {
                    alert(val);
                }
                $(input).removeAttr("disabled");
            }

            function valida() {
                var total = w2ui['grid'].total - 1;
                for (var x = 0 ; x < total; x++)
                {
                    if (w2ui['grid'].records[x].gera_titulo == "Sim") {
                        var vencimentos = w2ui['grid'].records[x].SVencimento;
                        if (vencimentos != undefined && vencimentos != null) {
                            if (vencimentos.length <= 0) {
                                return 'Todos os Lançamentos que gera título, tem que ter pelo menos uma parcela.';
                            }else {
                                s_vlr = 0;
                                $(vencimentos).each(function () {
                                    s_vlr += this.valor;
                                });

                                if (s_vlr != w2ui['grid'].records[x].valor) {
                                    return 'O valor da soma das parcelas é diferente do total.';
                                }
                            }  
                        }else{
                            return 'Todos os Lançamentos que gera título, tem que ter pelo menos uma parcela.';
                        }
                    } else {
                        if (vencimentos != undefined && vencimentos != null) {
                            if (vencimentos.length > 0) {
                                return 'Todos os Lançamentos que não gera título, não pode ter parcela.';
                            }
                        }
                    }
                }
                return '';
            }

            function changeCmbFornecedor()
            {

            }

            function mudaSomatoria()
            {
                var valor = 0;
                var number = 0;

                for (var x = 0; x < $('.parcValor').length; x++)
                {
                    if ($('.parcValor:eq(' + x + ')').val() != '') {

                        var v = $('.parcValor:eq(' + x + ')').val();
                        var n = v.toString().indexOf(",");

                        if (n != -1) {
                            v = v.substring(0, n) + v.substring(n, (n + 3));
                            $('.parcValor:eq(' + x + ')').val(v);
                        }

                        number = parseFloat($('.parcValor:eq(' + x + ')').val().replace(",", "."));
                        valor += number;
                        $('.parcValor:eq(' + x + ')').val(number.toFixed(2).replace(".", ","));
                    }
                }

                $('#somaParcelas').html(parseFloat(valor).toFixed(2).toString().replace(".", ","));

                var valorTotal = parseFloat($('#valorTotal').html().replace(",","."));
                var parcTotal = parseFloat(valor);

                if (parcTotal == valorTotal) {
                    $("#difParcelas").html('');
                } else if (parcTotal < valorTotal) {
                    $("#difParcelas").html(' ( +' + (valorTotal - parcTotal).toFixed(2).replace(".", ",") + ')').css("color", "#0600FF");
                } else {
                    $("#difParcelas").html(' ( -' + (parcTotal - valorTotal).toFixed(2).replace(".", ",") + ')').css("color", "#FF0000");
                }
            }

            function montaGridLanctosBeta(lista) {

                w2ui['grid'].clear();

                for (var i = 0; i < lista.length; i++) {

                    w2ui['grid'].add({ recid: (w2ui['grid'].records.length + 1), grupo: lista[i].valorGrupo, dc: lista[i].debCred, conta: lista[i].conta, job: lista[i].job, cliente: lista[i].cliente, divisao: lista[i].divisao, linha_negocio: lista[i].linhaNegocio, qtd: lista[i].qtd, valor_unit: lista[i].valorUnit, valor: lista[i].valor, historico: lista[i].historico, gera_titulo: (lista[i].titulo == true ? "Sim" : "Não"), terceiro: lista[i].terceiro, consultor: lista[i].codConsultor });
                    
                }

                verificaNovaLinha();
                atualizaSaldo();
            }

            function offExpand(event, recid){                
                if ($('#totalParcelas').length != 0) {
                    if (verificaCamposData()) {
                        if (verificaSaldoParcelas()) {
                            w2ui['grid'].records[linhaEdit - 1].cotacao = [];
                            w2ui['grid'].records[linhaEdit - 1].SVencimento = [];

                            for (var x = 0; x < $('.parcData').length; x++) {
                                if ($('.parcData:eq(' + x + ')').val() != "" && parseFloat($('.parcValor:eq(' + x + ')').val().replace(",", ".")) != 0 ) {
                                    var classParcela = new SVencimento();
                                    classParcela.data = $('.parcData:eq(' + x + ')').val();
                                    classParcela.valor = parseFloat($('.parcValor:eq(' + x + ')').val().replace(",", "."));
                                    w2ui['grid'].records[linhaEdit - 1].SVencimento.push(classParcela);
                                }
                            }
                            for (var x = 0; x < $('.valorMoeda').length; x++) {
                                if (parseFloat($('.valorMoeda:eq(' + x + ')').val().replace(",", ".")) != 0) {
                                    var classCotacao = new cotacaoDados();
                                    classCotacao.id = $('.codMoeda:eq(' + x + ')').val();
                                    classCotacao.valor = parseFloat($('.valorMoeda:eq(' + x + ')').val().replace(",", "."));
                                    w2ui['grid'].records[linhaEdit - 1].cotacao.push(classCotacao);
                                }
                            }
                            w2ui['grid'].toggle(linhaEdit, event);
                        } else {
                            alert('A soma das parcelas não batem!');
                            return false;
                        }                        
                    } else {
                        alert('Para salvar as parcelas, todos os campos de "Data" devem ser preenchidos!');
                        return false;
                    }
                } else {
                    if(recid !== undefined){
                        w2ui['grid'].toggle(recid, event);
                        $('#' + event.box_id).html('<table style="width:100%;"><tr><td class="table-head"><div style="float:left;width:360px;" class="w2ui-col-header">Parcelas - Valor <span id="valorTotal">' + w2ui['grid'].records[recid - 1].valor.toFixed(2).replace(".", ",") + '</span> - Soma <span id="somaParcelas">0,00</span> <span id="difParcelas" style="font-weight: bold;"></span> </div><input id="btnRefresh" class="btnRefresh" type="button" onclick="atualizaGridParcelas(' + event.recid + ');return false;" value="Atualizar"><input id="totalParcelas" style="float:right;width:30px;" name="parcelas" type="text" value="0" maxlength="2" onkeypress="return Onlynumbers(event);"></td><td class="table-head"><div class="w2ui-col-header">Cotação</div></td></tr><tr><td><div id="gridParcelas"></div></td><td style="float:left;">' + htmlMoeda + '</td></tr></table>').animate({ 'height': 100 }, 100, function () {

                            if (w2ui['grid'].records[recid - 1].cotacao !== undefined && w2ui['grid'].records[recid - 1].cotacao.length > 0) {
                                for (var x = 0; x < w2ui['grid'].records[linhaEdit - 1].cotacao.length; x++) {
                                    $('.valorMoeda:eq(' + x + ')').val(w2ui['grid'].records[linhaEdit - 1].cotacao[x].valor.toFixed(2).replace(".", ","));
                                }
                            }
                            if (w2ui['grid'].records[recid - 1].SVencimento !== undefined && w2ui['grid'].records[recid - 1].SVencimento.length > 0) {
                                $("#totalParcelas").val(w2ui['grid'].records[recid - 1].SVencimento.length);
                                atualizaGridParcelas(recid);
                                for (var x = 0; x < w2ui['grid'].records[recid - 1].SVencimento.length; x++)
                                {
                                    $('.parcData:eq(' + x + ')').val(w2ui['grid'].records[recid - 1].SVencimento[x].data);
                                    $('.parcValor:eq(' + x + ')').val(w2ui['grid'].records[recid - 1].SVencimento[x].valor.toFixed(2).replace(".", ","));
                                }
                            }
                            mudaSomatoria();
                        });
                    }
                }
                if (event.stopPropagation) {
                    event.stopPropagation();
                }  else  {
                    event.cancelBubble = true;
                }
                return true;
            }

            function retornaNomeColuna(nIndex)
            {
                for( var index in w2ui['grid'].columns )
                {
                    if (index == nIndex)
                    {
                        return w2ui['grid'].columns[index].field;
                    }
                }
            }

            function verificaData()
            {
                for (var x = 0; x < $('.parcData').length; x++)
                {
                    if ($('.parcData:eq(' + x + ')').val() != '')
                    {
                        var patternData = /^(((0[1-9]|[12][0-9]|3[01])([-.\/])(0[13578]|10|12)([-.\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-.\/])(0[469]|11)([-.\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-.\/])(02)([-.\/])(\d{4}))|((29)(\.|-|\/)(02)([-.\/])([02468][048]00))|((29)([-.\/])(02)([-.\/])([13579][26]00))|((29)([-.\/])(02)([-.\/])([0-9][0-9][0][48]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][2468][048]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][13579][26])))$/;
                        if (!patternData.test($('.parcData:eq(' + x + ')').val())) {
                            alert("Data inválida, digite no formato Dia/Mês/Ano!");
                            $('.parcData:eq(' + x + ')').val('');
                            $('.parcData:eq(' + x + ')').focus();
                            break;
                        }
                    }
                }
            }

            function verificaCamposData()
            {
                var ver = false;

                for (var x = 0; x < $('.parcData').length; x++) {
                    if ($('.parcData:eq(' + x + ')').val() != '' || $('.parcValor:eq(' + x + ')').val() != '0,00') {
                        ver = true;
                    }
                }

                if (ver)
                {
                    for (var x = 0; x < $('.parcData').length; x++)
                    {
                        if ($('.parcData:eq(' + x + ')').val() == '')
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            function returnComboValue(tipoCombo, valor)
            {
                switch (tipoCombo) {
                    case "FORNECEDOR":
                        for (var x = 0; x < cmbTerceiro.length; x++) {
                            if (cmbTerceiro[x].text == valor) {
                                return cmbTerceiro[x].id;
                            }
                        }
                        break;
                    default:
                        alert("Nenhum valor encontrado!");
                        break;
                }
            }

            function verificaSaldoParcelas()
            {
                //Se está ero não tem porque verificar as parcelas
                if ($("#totalParcelas").val() == "0") return true;

                //Verifica se tem saldo na diferença das parcela
                if ($("#difParcelas").html() != "") return false;

                return true;
            }

            function SVencimento()
            {
                this.data = "";
                this.valor = 0;
            }

            function cotacaoDados()
            {
                this.id = 0;
                this.valor = 0;
            }

            function destravaCamposCabecalho()
            {
                $("#ctl00_areaConteudo_textData").prop("disabled", false);
                $("#ctl00_areaConteudo_textNumeroDocumento").prop("disabled", false);
                $("#ctl00_areaConteudo_comboModelo").prop("disabled", false);
                $("#ctl00_areaConteudo_comboConsultor").prop("disabled", false);
                $("#ctl00_areaConteudo_textQtd").prop("disabled", false);
                $("#ctl00_areaConteudo_textValorUnit").prop("disabled", false);
                $("#ctl00_areaConteudo_textValorBruto").prop("disabled", false);
                $("#ctl00_areaConteudo_textValorTotal").prop("disabled", false);
                $("#ctl00_areaConteudo_textParcelas").prop("disabled", false);
                $("#ctl00_areaConteudo_textNumeroDocumento_lancto").prop("disabled", false);               
            }

            function changeLinhaNegocioDivisaoCLiente(job, numero_linha)
            {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "FormGenericTitulosBeta.aspx/loadJob",
                    data: "{job: " + job + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        var result = r.d;
                        w2ui['grid'].records[numero_linha - 1].linha_negocio = eval(result[1]);
                        w2ui['grid'].records[numero_linha - 1].linha_negocioDesc = result[2];
                        w2ui['grid'].records[numero_linha - 1].divisao = eval(result[3]);
                        w2ui['grid'].records[numero_linha - 1].divisaoDesc = result[4];
                        w2ui['grid'].records[numero_linha - 1].cliente = eval(result[5]);
                        w2ui['grid'].records[numero_linha - 1].clienteDesc = result[6];
                        w2ui['grid'].refresh();
                    },
                    complete: function () {

                    }
                });
            }

            function sincronizaLancto()
            {
                var listLancto = [];
                var total = w2ui['grid'].total - 1;
                var dataLancto = trataData($("#ctl00_areaConteudo_textData").val());

                for (var x = 0 ; x < total; x++)
                {
                    var gera_titulo;
                    var pendente;

                    if (w2ui['grid'].records[x].gera_titulo == "Sim") {
                        gera_titulo = true;
                        pendente = true;
                    } else {
                        gera_titulo = false;
                        pendente = false;
                    }

                    switch (tela.modulo){
                        case 'C_INCLUSAO_LANCTO':
                            valorBrutoTratado = 0;
                            break;
                        default:
                            valorBrutoTratado = $("#ctl00_areaConteudo_textValorBruto").val();
                            valorBrutoTratado = valorBrutoTratado.replace('.','');
                            valorBrutoTratado = valorBrutoTratado.replace(',','');
                            valorBrutoTratado = eval(valorBrutoTratado);
                            break;
                    }

                    var select_cmbDivisao_Desc = cmbDivisao.filter(function(obj) { return obj.id == w2ui['grid'].records[x].divisao; });
                    var select_cmbLinhaNegocio_Desc = cmbLinhaNegocio.filter(function(obj) { return obj.id == w2ui['grid'].records[x].linha_negocio; });
                    var select_cmbCliente_Desc = cmbCliente.filter(function(obj) { return obj.id == w2ui['grid'].records[x].cliente; });
                    var select_cmbJob_Desc = cmbJob.filter(function(obj) { return obj.id == w2ui['grid'].records[x].job; });
                    var select_cmbConta_Desc = cmbConta.filter(function(obj) { return obj.id == w2ui['grid'].records[x].conta; });

                    var SLancamento = {
                        seqLote: (x + 1),
                        debCred: w2ui['grid'].records[x].dc,
                        dataLancamento: dataLancto,
                        conta: w2ui['grid'].records[x].conta,
                        descConta: select_cmbConta_Desc[0].text,
                        job: w2ui['grid'].records[x].job,
                        descJob: select_cmbJob_Desc[0].text,
                        linhaNegocio: w2ui['grid'].records[x].linha_negocio,
                        descLinhaNegocio: select_cmbLinhaNegocio_Desc[0].text,
                        divisao: w2ui['grid'].records[x].divisao,
                        descDivisao: select_cmbDivisao_Desc[0].text,
                        cliente: w2ui['grid'].records[x].cliente,
                        descCliente: select_cmbCliente_Desc[0].text,
                        qtd: w2ui['grid'].records[x].qtd,
                        valorUnit: w2ui['grid'].records[x].valor_unit,
                        valorBruto: valorBrutoTratado,
                        valor: w2ui['grid'].records[x].valor,
                        historico: w2ui['grid'].records[x].historico,
                        titulo: gera_titulo,
                        pendente: pendente,
                        vencimentos: w2ui['grid'].records[x].SVencimento,
                        terceiro: w2ui['grid'].records[x].terceiro,
                        descTerceiro: w2ui['grid'].records[x].terceiroDesc,
                        modelo: $("#ctl00_areaConteudo_comboModelo").val(),
                        strData: w2ui['grid'].records[x].data,
                        //modulo: "",
                        numeroDocumento: $("#ctl00_areaConteudo_textNumeroDocumento").val(),
                        //codOrigem: "",
                        //seqOrigem: "",
                        //efetivado: "",
                        //tipoLancto: "",
                        usuario: cod_usuario,
                        descConsultor: w2ui['grid'].records[x].descConsultor,
                        codConsultor: w2ui['grid'].records[x].codConsultor,
                        //codPlanilha: "",
                        valorGrupo: eval(w2ui['grid'].records[x].grupo),
                        codMoeda: w2ui['grid'].records[x].codMoeda
                    };
                    listLancto.push(SLancamento);
                }

                return listLancto;
            }

            function reorganizaIndices()
            {
                for(var x = 0; x < w2ui['grid'].records.length; x++)
                {
                    w2ui['grid'].records[x].recid = (x + 1);
                }
            }

            function trataData(data)
            {
                var dataMod = data.substring(6,10) + '-' + data.substring(3,5) + '-' + data.substring(0,2) + ' 00:00:00';
                return dataMod;
            }

            function alteraValorLancto(valor)
            {
                var qtd = w2ui['grid'].records[0].qtd;

                w2ui['grid'].records[0].valor_unit = eval(valor.replace(',','.'));
                w2ui['grid'].records[0].valor = qtd * eval(valor.replace(',','.'));
                $("#ctl00_areaConteudo_textValorTotal").val(w2ui['grid'].records[0].valor.toFixed(2).replace('.',','));
                w2ui['grid'].refresh();
                w2ui['grid'].save();
            }

            function alteraQtdLancto(valor)
            {
                w2ui['grid'].records[0].qtd = eval(valor.replace(',','.'));
                $("#ctl00_areaConteudo_textQtd").val(w2ui['grid'].records[0].qtd.toFixed(4).replace('.',','));

                if(w2ui['grid'].records[0].qtd != 0 && eval($("#ctl00_areaConteudo_textValorTotal").val().replace(',','.')) != 0)
                {
                    var valor = (eval($("#ctl00_areaConteudo_textValorTotal").val().replace(',','.')) / w2ui['grid'].records[0].qtd).toFixed(2).replace('.',',');
                    $("#ctl00_areaConteudo_textValorUnit").val(valor);
                    w2ui['grid'].records[0].valor_unit = eval(valor.replace(',','.'));
                }

                if(w2ui['grid'].records[0].qtd == 0 && eval($("#ctl00_areaConteudo_textValorTotal").val().replace(',','.')) == 0)
                {
                    $("#ctl00_areaConteudo_textValorTotal").val($("#ctl00_areaConteudo_textValorUnit").val());
                    w2ui['grid'].records[0].valor = eval($("#ctl00_areaConteudo_textValorUnit").val().replace(',','.'));
                }

                w2ui['grid'].refresh();
                w2ui['grid'].save();
            }

            function alteraValorTotalLancto(valor)
            {
                w2ui['grid'].records[0].valor = eval(valor.replace(',','.'));
                
                if(w2ui['grid'].records[0].qtd != 0)
                {
                    w2ui['grid'].records[0].valor_unit = eval(valor.replace(',','.')) / w2ui['grid'].records[0].qtd;

                    var numero = w2ui['grid'].records[0].valor_unit;
                    $("#ctl00_areaConteudo_textValorUnit").val(numero.toFixed(2).replace('.',','));
                }
                else 
                {
                    $("#ctl00_areaConteudo_textValorUnit").val(valor);
                    w2ui['grid'].records[0].valor_unit = eval(valor.replace(',','.'));
                }
                
                w2ui['grid'].refresh();
                w2ui['grid'].save();
            }

            function get(paramName) {
                var sURL = window.document.URL.toString();
                if (sURL.indexOf("?") > 0) {
                    var arrParams = sURL.split("?");
                    var arrURLParams = arrParams[1].split("&");
                    var arrParamNames = new Array(arrURLParams.length);
                    var arrParamValues = new Array(arrURLParams.length);

                    var i = 0;
                    for (i = 0; i < arrURLParams.length; i++) {
                        var sParam = arrURLParams[i].split("=");
                        arrParamNames[i] = sParam[0];
                        if (sParam[1] != "")
                            arrParamValues[i] = unescape(sParam[1]);
                        else
                            arrParamValues[i] = "No Value";
                    }

                    for (i = 0; i < arrURLParams.length; i++) {
                        if (arrParamNames[i] == paramName) {
                            //alert("Parameter:" + arrParamValues[i]);
                            return arrParamValues[i];
                        }
                    }
                    return "No Parameters Found";
                }
            }
        </script>
    </div>
    <div id="panelEtapa2">
        <h3>
            <p>Na folha de lançamento encontramos algumas contas que geram crédito ou despesa.</p>
            <p>Para continuar o processo e digitar a Nota Fiscal escolha seu Tipo e clique em "Próximo" ou então clique em "Concluído" para terminar o processo e salvar a folha.</p>

        </h3>
        <center>
            <asp:Button ID="botaoConcluido" CssClass="botao" runat="server" Text="Concluído" />
        </center>
        <fieldset id="formTipoNotaFiscal">
            <label>
                <span>Tipo da Nota Fiscal</span>
                <asp:DropDownList ID="comboTipoNotaFiscal" Width="250px" runat="server">
                    <asp:ListItem Text="Escolha" Value="0" />
                    <asp:ListItem Text="Produto" Value="P" />
                    <asp:ListItem Text="Serviço" Value="S" />
                </asp:DropDownList>
            </label>
        </fieldset>
    </div>
    <div id="panelEtapa3">
        <asp:HiddenField ID="H_ORDEM" runat="server" />
        <asp:HiddenField ID="H_ORDEM_MIN" runat="server" />
        <asp:HiddenField ID="H_ORDEM_MAX" runat="server" />
        <asp:HiddenField ID="H_PRODUTO_SERVICO" runat="server" />
        <fieldset id="formNotaFiscal" class="form_inclusao_titulo">
            <div class="linha_form">
                <span><strong>Fornecedor/Cliente:</strong> <font id="areaFornecedorClienteNota"></font></span>
            </div>
            <div class="linha_form">
                <label>
                    <span>Nº Nota</span>
                    <asp:HiddenField ID="numeroNotaOldHidden" Value="0" runat="server" />
                    <asp:TextBox ID="textNumeroNota" Width="70px" runat="server" />
                </label>
                <label>
                    <span>Nº Eletronica</span>
                    <asp:TextBox ID="textNumeroEletronica" Width="70px" runat="server" />
                </label>
                <label>
                    <span>Entrada/Saída</span>
                    <asp:DropDownList ID="comboEntradaSaidaNota" Width="100px" runat="server">
                        <asp:ListItem Text="Escolha" Value="0" />
                        <asp:ListItem Text="Entrada" Value="E" />
                        <asp:ListItem Text="Saída" Value="S" />
                    </asp:DropDownList>
                </label>
                <label>
                    <span>Valor</span>
                    <asp:TextBox ID="textValorNota" Width="70px" runat="server" />
                </label>
                <label>
                    <span>Icms</span>
                    <asp:TextBox ID="textIcmsNota" Width="60px" runat="server" />
                </label>
                <label>
                    <span>Descontos</span>
                    <asp:TextBox ID="textDescontoNota" Width="60px" runat="server" />
                </label>
                <label id="labelFreteNota">
                    <span>Frete</span>
                    <asp:TextBox ID="textFreteNota" Width="60px" runat="server" />
                </label>
                <label id="labelIpiNota">
                    <span>Ipi</span>
                    <asp:TextBox ID="textIpiNota" Width="60px" runat="server" />
                </label>
                <label>
                    <span>Base de Imp.</span>
                    <asp:TextBox ID="textBaseImpNota" Width="80px" runat="server" />
                </label>
            </div>
            <div id="camposServico" class="linha_form">
                <label>
                    <span>Csll</span>
                    <asp:TextBox ID="textCsllNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>IR</span>
                    <asp:TextBox ID="textIrNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Iss</span>
                    <asp:TextBox ID="textIssNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Pis Retido</span>
                    <asp:TextBox ID="textPisRetidoNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Cofins Retido</span>
                    <asp:TextBox ID="textCofinsRetidoNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Csll Retido</span>
                    <asp:TextBox ID="textCsllRetidoNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Ir Retido</span>
                    <asp:TextBox ID="textIrRetidoNota" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Iss Retido</span>
                    <asp:TextBox ID="textIssRetidoNota" Width="80px" runat="server" />
                </label>
            </div>
        </fieldset>
        <fieldset id="formItemNota" class="formLancamento">
            <div class="linha_form">
                <label id="label2" runat="server" class="lado">
                    <span>Produto</span>
                    <asp:DropDownList ID="comboProdutoItem" Width="250px" runat="server">
                    </asp:DropDownList>
                </label>
                <label id="labelCfopItem">
                    <span>Cfop</span>
                    <asp:TextBox ID="textCfopItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Qtde</span>
                    <asp:TextBox ID="textQtdeItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Valor Total</span>
                    <asp:TextBox ID="textValorTotalItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Base de Imp.</span>
                    <asp:TextBox ID="textBaseImpItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Alíquota Icms</span>
                    <asp:TextBox ID="textIcmsItem" Width="80px" runat="server" />
                </label>
                <label id="labelIpiItem">
                    <span>Alíquota Ipi</span>
                    <asp:TextBox ID="textIpiItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Alíquota Pis</span>
                    <asp:TextBox ID="textPisItem" Width="80px" runat="server" />
                </label>
                <label>
                    <span>Alíquota Cofins</span>
                    <asp:TextBox ID="textCofinsItem" Width="80px" runat="server" />
                </label>
            </div>
            <div class="linha_form">
                <span id="boxLoadingItem" style="display: none;">carregando...</span>
                <input type="button" name="botaoCancelar_item" id="botaoCancelar_item" class="botao" value="Cancelar" runat="server" />
                <input type="button" name="botaoSalvar_item" id="botaoSalvar_item" class="botao" value="Incluir" runat="server" />
            </div>
        </fieldset>
        <table id="gridItensNotaFiscal" cellpadding="0" cellspacing="0" class="gridLancamentos">
            <tr class="cab">
                <td>Ordem</td>
                <td>Produto</td>
                <td class="valor">Qtde</td>
                <td class="valor">Valor Total</td>
                <td class="valor">BaseImp</td>
                <td class="valor">Aliq Icms</td>
                <td class="valor">Aliq Pis</td>
                <td class="valor">Aliq Cofins</td>
                <td class="deletar"></td>
            </tr>
        </table>
    </div>
    <div class="boxBotaoTerminei">
        <asp:Button ID="botaoVoltar" CssClass="botao voltar" runat="server" Text="Anterior" />
        <asp:Button ID="botaoSalvar" CssClass="botao" runat="server" Text="Próximo" />
    </div>

    <toolKit:ModalPopupExtender ID="popVencimento" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll"
        OkControlID="botaoSalvarVencimento" CancelControlID="closePopVencimento" TargetControlID="botaoVencimento"
        BackgroundCssClass="pop_background" PopupControlID="blocoPopVencimento">
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPopVencimento" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Datas de Vencimento</div>
            <asp:LinkButton ID="closePopVencimento" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <div style="overflow: scroll; height: 300px;">
                <fieldset id="formVencimento" style="width: 375px;">
                </fieldset>
            </div>
            <div style="width: 100%; text-align: center;">
                <input id="btSalvarVencimento" type="button" class="botao_vencimentos" value="Salvar" runat="server" />
                <span style="display: none;">
                    <asp:Button ID="botaoSalvarVencimento" runat="server" Text="Salvar" /></span>
            </div>
        </div>
    </asp:Panel>


    <toolKit:ModalPopupExtender ID="popBaixas" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll"
        CancelControlID="closePopBaixas" TargetControlID="botaoBaixas"
        BackgroundCssClass="pop_background" PopupControlID="blocoPopBaixas">
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPopBaixas" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Datas de Vencimento</div>
            <asp:LinkButton ID="closePopBaixas" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <div style="overflow: scroll; height: 300px;">
                <div id="dadosBaixa">
                </div>
            </div>
        </div>
    </asp:Panel>

    <toolKit:ModalPopupExtender ID="cotacao" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll"
        OkControlID="botaoSalvarCotacao" CancelControlID="closeCotacao" TargetControlID="botaoCotacaoAbre"
        BackgroundCssClass="pop_background" PopupControlID="blocoCotacao">
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoCotacao" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Cotações</div>
            <asp:LinkButton ID="closeCotacao" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudoCotacao">
            <div class="gridCotacao">
                <table class="tblCotacao"></table>
            </div>
            <div style="width: 100%; text-align: center;">
                <asp:Button ID="botaoSalvarCotacao" runat="server" Text="Salvar" OnClientClick="javascript:salvaCotacao();" />
            </div>
        </div>
    </asp:Panel>
    <span style="display: none;">
        <asp:Button ID="botaoBaixas" runat="server" Text="" /></span>
</asp:Content>
