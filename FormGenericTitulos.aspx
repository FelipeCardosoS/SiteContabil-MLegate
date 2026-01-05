<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGenericTitulos.aspx.cs" Inherits="FormGenericTitulos" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="areaHead" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <%--<style type="text/css">@import url(Css/loading.css);</style>--%>
    <style>
        fieldset.form_inclusao_titulo .linha_form {
            overflow: inherit !important;
        }
        table.gridLancamentos tr.linha td, table.gridLancamentos tr.linhaMarcada td, table.gridLancamentos tr.cab td {
            font-size: 11px !important;
        }
        fieldset.formLancamento label {
            font-size: 12px !important;
        }
    </style>
    <span style="display:none;"><asp:Button ID="botaoVencimento" runat="server" /></span>
    <asp:Label ID="boxModelo" CssClass="boxModelo" runat="server" Visible="false" Text=""></asp:Label>
    <div id="box_etapas">
        <div id="etapa1" class="etapa">
            <span class="etapa_numero">1</span>
            <span class="etapa_texto">Folha de Lançamento</span>
        </div>
        <div id="etapa2" class="etapa">
            <span class="etapa_numero">2</span>
            <span class="etapa_texto">Escolha do Tipo de Nota Fiscal</span>
        </div>
        <div id="etapa3" class="etapa">
            <span class="etapa_numero">3</span>
            <span class="etapa_texto">Nota Fiscal</span>
        </div>
        <div style="float:right;">
            <asp:Label ID="labelLote" CssClass="boxLote" runat="server" Visible="false" Text=""></asp:Label>
        </div>
        <div id="etapaMoedaInclusaoLancto">
            <div style="float:left;">
                <span class="etapaTipoMoeda">Tipo de Moeda</span>
                <span class="etapaComboMoeda"><asp:DropDownList ID="comboEtapaMoeda" runat="server"></asp:DropDownList></span>
            </div>
            <div style="float:left;padding:7px 9px;">
                <img src="Imagens/divisa.gif" />
            </div>
            <div style="float:left;padding: 10px 0 0 0;">
                <asp:Button ID="botaoCotacao" runat="server" Text="" CssClass="btnCotacao" OnClientClick="loadCotacao(1,0); return false;" />
                <asp:Button ID="botaoCotacaoAbre" runat="server" style="display:none;" Text=""/>
            </div>
        </div>
    </div>
    <div id="panelEtapa1">
        <fieldset id="form_inclusao_titulo" class="form_inclusao_titulo" runat="server">
            <div class="linha_form">
                <label id="labelTerceiro" runat="server">
                    <span><asp:Literal ID="ltTerceiro" runat="server"></asp:Literal></span>
                    <asp:DropDownList ID="comboFornecedor" class="chosen" data-placeholder="Escolha o fornecedor..." tabindex="1" Width="480px" runat="server"></asp:DropDownList>
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
                    <asp:DropDownList ID="comboModelo" class="chosen" data-placeholder="Escolha o modelo..." Enabled="false" tabindex="1" Width="280px" runat="server">
                    </asp:DropDownList>
                </label>
                <label id="labelEtapaMoeda" style="float:right;" runat="server" visible="False">
                    <div style="float:left;">
                        <span class="etapaTipoMoeda">Tipo de Moeda</span>
                        <span class="etapaComboMoeda"><asp:DropDownList ID="comboEtapaMoedaInclusaLancto" runat="server"></asp:DropDownList></span>
                    </div>
                    <div style="float:left;padding:7px 9px;">
                        <img src="Imagens/divisa.gif" />
                    </div>
                    <div style="float:left;padding: 10px 0 0 0;">
                        <asp:Button ID="botaoCotacaoInclusaoLancto" runat="server" Text="" CssClass="btnCotacao" OnClientClick="loadCotacao(1,0); return false;" />
                        <asp:Button ID="botaoCotacaoInclusaoLanctoAbre" runat="server" style="display:none;" Text=""/>
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
                    <asp:TextBox ID="textQtd" Enabled="false" Width="40px" runat="server"></asp:TextBox>
                </label>
                <label id="labelValorUnit" runat="server">
                    <span>Valor Unitário</span>
                    <asp:HiddenField ID="textValorUnit_old" runat="server" />
                    <asp:TextBox ID="textValorUnit" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelValorBruto" runat="server">
                    <span>Valor Bruto</span>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:TextBox ID="textValorBruto" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelLiquido" runat="server">
                    <span>Valor Total</span>
                    <asp:TextBox ID="textValorTotal" Enabled="false" Width="120px" runat="server"></asp:TextBox>
                </label>
                <label id="labelParcelas" runat="server">
                    <span>N° Parcelas</span>
                    <asp:TextBox ID="textParcelas"  Enabled="false" Width="90px" runat="server"></asp:TextBox>
                    <input id="btVencimento" class="botaoCalendario" disabled="disabled" type="button" runat="server" />
                </label>
            </div>
        </fieldset>
        <fieldset id="formLancamento" class="formLancamento">
            <asp:HiddenField ID="H_SEQLOTE_LANCTO" runat="server" />
            <asp:HiddenField ID="H_SEQLOTE_LANCTO_MIN" runat="server" />
            <asp:HiddenField ID="H_SEQLOTE_LANCTO_MAX" runat="server" />
            <div class="linha_form">
                <label id="labelNumeroDocumento_lancto" runat="server">
                    <span><asp:Label ID="textGrupoDoc" runat="server" Text="Nº Doc"></asp:Label></span>
                    <asp:TextBox ID="textNumeroDocumento_lancto" Width="80px" runat="server"></asp:TextBox>
                    <asp:TextBox ID="textGrupo" Visible="false" Width="80px" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="dataHidden" runat="server" />
                </label>
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
                    <span><div>Job</div><asp:CheckBox ToolTip="" ID="checkJobAtivo"   Text="Ativos" CssClass="checkJobAtivo" runat="server" /></span>
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
                    //$(".btnexp").toggle(function () {
                    //        $(this).css("background-position", "0px -10px");
                    //        $(".expand").show();
                    //        $(tela.comboCliente_lancto + "_chosen").css("width","120px").trigger("chosen:updated");
                    //    }, function () {
                    //        $(this).css("background-position", "0px 0px");
                    //        $(".expand").hide();
                    //});
                    $(".btnexp").click(function () {
                        if ($(".expand").is(":visible")) {
                            $(this).css("background-position", "0px 0px");
                             $(".expand").hide();
                        } else {
                            $(this).css("background-position", "0px -10px");
                            $(".expand").show();
                            $(tela.comboCliente_lancto + "_chosen").css("width", "120px").trigger("chosen:updated");
                        }
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
                <div id="celulaParcelasTitulo" style="display:none;">
                    <label>
                        <span>N°Parc.</span>
                        <asp:TextBox ID="textParcelas_lancto" Enabled="false" Width="25px" runat="server"></asp:TextBox>
                        <input id="btVencimento_lancto" class="botaoCalendario" type="button" runat="server" />
                    </label>
                </div>
                <div id="celulaFornecedorTitulo" style="display:none;">
                    <label>
                        <span>Terceiro</span>
                        <asp:DropDownList ID="comboTerceiro_lancto" Enabled="false" style="width:250px !important;" runat="server"></asp:DropDownList>
                    </label>
                </div>
            </div>
            <div class="linha_form">
                <asp:TextBox ID="textHistorico_lancto" MaxLength="3000" Enabled="false" TextMode="MultiLine" Width="980px" runat="server"></asp:TextBox>
            </div>
            <div class="linha_form">
                <span id="boxLoading" style="display:none;">carregando...</span>
                <input type="button" disabled="disabled" name="botaoCancelar_lancto" id="botaoCancelar_lancto" class="botao" value="Cancelar" runat="server" />
                <input type="button" disabled="disabled" name="botaoSalvar_lancto" id="botaoSalvar_lancto" class="botao" value="Incluir" runat="server" />
            </div>
        </fieldset>
        <div id="boxTotalizador">
            <table>
                <tr>
                    <td class="celulaLabelTotal">Totais:</td>
                    <td class="celulaDebito">Débito: <span id="totalDebito"></span></td>
                    <td class="celulaCredito">Crédito: <span id="totalCredito"></span></td>
                    <td class="celulaDiferenca">Diferença: <span id="totalDiferenca"></span></td>
                    <td class="celulaInverter"><a class="botaoInverter" onclick="inverterCoresTabela()">Deixar a Tabela Escura</a></td>
                </tr>
            </table>
        </div>
        <table id="gridLancamentos" cellpadding="0" cellspacing="0" class="gridLancamentos">
            <tr class="cab">
                <td id="linhaSeqLote" style="display:none;">Seq Lote</td>
                <td class="debCred">Grupo&nbsp;</td>
                <td class="debCred">D/C</td>
                <td class="conta">Conta</td>
                <td class="job">Job</td>
                <td class="linhaNegocio" style="display:none;">Linha Negócio</td>
                <td class="divisao"  style="display:none;">Divisão</td>
                <td class="valor">Valor($)</td>
                <td class="historico">Histórico</td>
                <td class="titulo">Tit</td>
                <td class="historico">Terceiro</td>
                <td class="historico"></td>
                <td class="cotacao"></td>
                <td class="deletar"></td>
                <td class="parcelas" style="display:none;">N° Parc.</td>
                <td class="fornecedor" style="display:none;">Fornec.</td>
            </tr>
        </table>
        <div class="boxLog">
            <asp:Literal ID="logLiteral" runat="server" />
        </div>
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
                <span id="boxLoadingItem" style="display:none;">carregando...</span>
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
        BackgroundCssClass="pop_background" PopupControlID="blocoPopVencimento" >
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPopVencimento" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Datas de Vencimento</div>
            <asp:LinkButton ID="closePopVencimento" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <div style="overflow:scroll; height:300px;">
                <fieldset id="formVencimento" style="width:375px;">
                    
                </fieldset>
            </div>
            <div style="width:100%;text-align:center;">
                <input id="btSalvarVencimento" type="button" class="botao_vencimentos" value="Salvar" runat="server" />
                <span style="display:none;"><asp:Button ID="botaoSalvarVencimento" runat="server" Text="Salvar" /></span>
            </div>
        </div>
    </asp:Panel>
    
    
    <toolKit:ModalPopupExtender ID="popBaixas" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" 
        CancelControlID="closePopBaixas" TargetControlID="botaoBaixas" 
        BackgroundCssClass="pop_background" PopupControlID="blocoPopBaixas" >
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPopBaixas" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Datas de Vencimento</div>
            <asp:LinkButton ID="closePopBaixas" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <div style=" overflow:scroll; height:300px;">
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
            <div style="width:100%;text-align:center;">
                <asp:Button ID="botaoSalvarCotacao" runat="server" Text="Salvar" OnClientClick="javascript:salvaCotacao();" />
            </div>
        </div>
    </asp:Panel>
    <span style="display:none;"><asp:Button ID="botaoBaixas" runat="server" Text="" /></span>
</asp:Content>