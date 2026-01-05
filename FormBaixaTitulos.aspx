<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal"  AutoEventWireup="true" CodeFile="FormBaixaTitulos.aspx.cs" Inherits="FormBaixaTitulos" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <script>
    $(document).ready(function () {
        $("#<%= Page.Form.ClientID %>").validate({
            rules: {
                <%= Txtpag.UniqueID %>:{
                    required: true,
                    min: 1,
                    max: 2000                    
                }
            },
            messages:{
                <%= Txtpag.UniqueID %>:{
                    required: "Campo obrigatório",
                    min: "Valor min. 1",
                    max: "Valor max. 2000"  
                    
                }
            },
            errorElement: "em"
        });
    });

    function verificaNumero(e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    }
    $(document).ready(function() {
        $("#<%= Txtpag.ClientID %>").keypress(verificaNumero);
    });


</script>
    <span style="display:none;"><asp:Button ID="botaoVencimento" runat="server" /></span>
    <fieldset id="form_baixa_titulo" class="form_baixa_titulo" runat="server">
        <legend>
            Títulos
        </legend>
        
        <div id="areaFiltro">
            <span class="linha">
                <label>
                    <span>Data da Baixa</span>
                    <asp:TextBox ID="textData" CssClass="inputData" Width="80px" runat="server"></asp:TextBox>
                </label>
                <label><span>Tipo</span>
                    <asp:DropDownList ID="comboTipo" runat="server">
                        <asp:ListItem Text="Todos" Value="T"></asp:ListItem>
                        <asp:ListItem Text="Contas à Pagar" Value="CP"></asp:ListItem>
                        <asp:ListItem Text="Contas à Receber" Value="CR"></asp:ListItem>
                    </asp:DropDownList>
                </label>
                <label><span>De</span>
                    <asp:TextBox ID="textPeriodoDe" Width="80px" runat="server"></asp:TextBox>
                </label>
                <label><span>Até</span>
                    <asp:TextBox ID="textPeriodoAte" Width="80px" runat="server"></asp:TextBox>
                </label>
            </span>
            <span class="linha">
                <label><span>Fornecedor/Cliente</span>
                    <asp:DropDownList ID="comboTerceiro" Width="280px" runat="server">
                    </asp:DropDownList>
                </label>
                <label><span>Itens por pag.</span>                    
                    <asp:TextBox ID="Txtpag" runat="server" MaxLength="4"></asp:TextBox>
                </label>
                <asp:Button ID="botaoFiltrar" OnClick="botaoFiltrar_Click" CssClass="botao" runat="server" Text="Filtrar" />
            </span>
        </div>
        
        <h3> - à Receber</h3>
        <div id="boxBairaaReceber" class="boxBaixa">
            <table id="gridLanctosAReceber" class="gridLanctosBaixa" cellpadding="0" cellspacing="0">
                <tr class="titulo">
                    <td>Lote</td>
                    <td>Data</td>
                    <td>Terceiro</td>
                    <td>Histórico</td>
                    <td class="valor">Total</td>
                    <td></td>
                </tr>
                <asp:Repeater ID="repeaterLanctosAReceber" OnItemDataBound="repeaterLanctosAReceber_ItemDataBound" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hiddenLotePai" Value='<%# DataBinder.Eval(Container.DataItem, "[LOTE_PAI]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenSeqLotePai" Value='<%# DataBinder.Eval(Container.DataItem, "[SEQ_LOTE_PAI]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenSeqBaixa" Value='<%# DataBinder.Eval(Container.DataItem, "[SEQ_BAIXA]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenModuloPai" Value='<%# DataBinder.Eval(Container.DataItem, "[MODULO_PAI]") %>' runat="server" />
                        <tr class="linha">
                            <td>
                                <asp:HyperLink ID="linkEditaLote" runat="server"><%# DataBinder.Eval(Container.DataItem, "[LOTE_PAI]")%></asp:HyperLink>
                            </td>
                            <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[data]")).ToString("dd/MM/yyyy") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "[DESC_TERCEIRO]") %></td>
                            <td><asp:Literal ID="historicoLiteral" runat="server" /></td>
                            <td class="valor"><%# String.Format("{0:0,0.00}", Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "[Creditos]"))) %></td>
                            <td class="check">
                                <asp:HiddenField ID="hdValor" Value='<%# DataBinder.Eval(Container.DataItem, "[Creditos]") %>' runat="server" />
                                <input type="checkbox" id="check" class="check" name="check" 
                                    value='<%# DataBinder.Eval(Container.DataItem, "[LOTE]") %>' title='<%# DataBinder.Eval(Container.DataItem, "[Creditos]") %>' runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="labelNenhumRegistro_CR" runat="server" Text="Nenhuma baixa encontrada." Visible="false"></asp:Label>
            </table>
            <div id="boxPaginacao_CR" class="boxPaginacao" runat="server">
                <asp:LinkButton ID="linkPrimeira_CR" OnClick="linkPrimeira_Click" Tipo="CR" runat="server"><<</asp:LinkButton>
                <asp:LinkButton ID="linkPaginacaoAnterior_CR" OnClick="linkPaginacaoAnterior_Click" Tipo="CR" CssClass="botaoPaginacaoAnterior" runat="server"> < </asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao1_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao2_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao3_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao4_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao5_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao6_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao7_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao8_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao9_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao10_CR" OnClick="linkPaginacao_Click" Tipo="CR" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacaoProximo_CR" OnClick="linkPaginacaoProximo_Click" Tipo="CR" CssClass="botaoPaginacaoProximo" runat="server"> > </asp:LinkButton>
                <asp:LinkButton ID="linkUltima_CR" OnClick="linkUltima_Click" Tipo="CR" runat="server">>></asp:LinkButton>
            </div>
        </div>
        <p class="totalizador">Total à receber: <span id="boxTotalAReceber">0</span></p>
        
        <h3> - à Pagar</h3>
        <div id="boxBaixaaPagar" class="boxBaixa">
            <table id="gridLanctosAPagar" class="gridLanctosBaixa" cellpadding="0" cellspacing="0">
                <tr class="titulo">
                    <td>Lote</td>
                    <td>Data</td>
                    <td>Terceiro</td>
                    <td>Histórico</td>
                    <td class="valor">Total</td>
                    <td></td>
                </tr>
                <asp:Repeater ID="repeaterLanctosAPagar" OnItemDataBound="repeaterLanctosAPagar_ItemDataBound" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hiddenLotePai" Value='<%# DataBinder.Eval(Container.DataItem, "[LOTE_PAI]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenSeqLotePai" Value='<%# DataBinder.Eval(Container.DataItem, "[SEQ_LOTE_PAI]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenSeqBaixa" Value='<%# DataBinder.Eval(Container.DataItem, "[SEQ_BAIXA]") %>' runat="server" />
                        <asp:HiddenField ID="hiddenModuloPai" Value='<%# DataBinder.Eval(Container.DataItem, "[MODULO_PAI]") %>' runat="server" />
                        <tr class="linha">
                            <td><asp:HyperLink ID="linkEditaLote" runat="server"><%# DataBinder.Eval(Container.DataItem, "[LOTE_PAI]")%></asp:HyperLink></td>
                            <td><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[data]")).ToString("dd/MM/yyyy") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "[DESC_TERCEIRO]") %></td>
                            <td><asp:Literal ID="historicoLiteral" runat="server" /></td>
                            <td class="valor"><%# String.Format("{0:0,0.00}", Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "[Debitos]"))) %></td>
                            <td class="check">
                                <asp:HiddenField ID="hdValor" Value='<%# DataBinder.Eval(Container.DataItem, "[Debitos]") %>' runat="server" />
                                <input type="checkbox" id="check" class="check" name="check" 
                                    value='<%# DataBinder.Eval(Container.DataItem, "[LOTE]") %>' title='<%# DataBinder.Eval(Container.DataItem, "[Debitos]") %>'  runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="labelNenhumRegistro_CP" runat="server" Text="Nenhuma baixa encontrada." Visible="false"></asp:Label>
            </table>
            <div id="boxPaginacao_CP" class="boxPaginacao" runat="server">
                <asp:LinkButton ID="linkPrimeira_CP" OnClick="linkPrimeira_Click" Tipo="CP" runat="server"><<</asp:LinkButton>
                <asp:LinkButton ID="linkPaginacaoAnterior_CP" OnClick="linkPaginacaoAnterior_Click" Tipo="CP" CssClass="botaoPaginacaoAnterior" runat="server"> < </asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao1_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao2_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao3_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao4_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao5_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao6_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao7_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao8_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao9_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacao10_CP" OnClick="linkPaginacao_Click" Tipo="CP" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="linkPaginacaoProximo_CP" OnClick="linkPaginacaoProximo_Click" Tipo="CP" CssClass="botaoPaginacaoProximo" runat="server"> > </asp:LinkButton>
                <asp:LinkButton ID="linkUltima_CP" OnClick="linkUltima_Click" Tipo="CP" runat="server">>></asp:LinkButton>
            </div>
        </div>
        <p class="totalizador">Total à pagar: <span id="boxTotalAPagar">0</span></p>
        
        <div id="boxTotalBaixa">
            <strong>Total Selecionado:</strong><span id="totalBaixa">0</span>
        </div>
    </fieldset>

    <fieldset id="formLancamento" class="formLancamento">
        <asp:HiddenField ID="H_SEQLOTE_LANCTO" runat="server" />
        <asp:HiddenField ID="H_SEQLOTE_LANCTO_MIN" runat="server" />
        <asp:HiddenField ID="H_SEQLOTE_LANCTO_MAX" runat="server" />
        <table style="width:988px;margin:0 auto 0 auto;" cellpadding="0" cellspacing="0">
            <tbody>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <label id="labelNumeroDocumento_lancto" runat="server">
                                            <span>Nº Doc</span>
                                            <asp:TextBox ID="textNumeroDocumento_lancto" Enabled="false" Width="80px" runat="server"></asp:TextBox>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>D/C</span>
                                            <asp:DropDownList ID="comboDebCred_lancto" Width="50px" runat="server">
                                                <asp:ListItem Text="N/D" Selected="True" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="D" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Conta</span>
                                            <asp:DropDownList ID="comboConta_lancto" Width="300px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label for="3432432423">
                                            <span><div>Job</div><asp:CheckBox ToolTip="" ID="checkJobAtivo"  Text="Ativos" CssClass="checkJobAtivo" runat="server" /></span>
                                            <asp:DropDownList ID="comboJob_lancto" Width="300px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Qtd</span>
                                            <asp:TextBox ID="textQtd_lancto" Enabled="false" Width="20px" runat="server"></asp:TextBox>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Valor Unit($)</span>
                                            <asp:TextBox ID="textValorUnit_lancto" Enabled="false" Width="70px" runat="server"></asp:TextBox>
                                        </label>
                                        <label>
                                            <span>Valor($)</span>
                                            <asp:TextBox ID="textValor_lancto" Width="70px" runat="server"></asp:TextBox>
                                        </label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <label>
                                            <span>Linha Negócio</span>
                                            <asp:DropDownList ID="comboLinhaNegocio_lancto" Width="120px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Divisão</span>
                                            <asp:DropDownList ID="comboDivisao_lancto" Width="120px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Cliente</span>
                                            <asp:DropDownList ID="comboCliente_lancto" Width="150px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Consultor</span>
                                            <asp:DropDownList ID="comboConsultor_lancto"  Width="120px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td>
                                        <label>
                                            <span>Gera Título?</span>
                                            <asp:DropDownList ID="comboGeraTitulo_lancto" Enabled="false" runat="server">
                                                <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="Não" Selected="True" Value="N"></asp:ListItem>
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                    <td id="celulaParcelasTitulo" style="display:none;">
                                        <label>
                                            <span>N°Parc.</span>
                                            <asp:TextBox ID="textParcelas_lancto" Width="25px" runat="server"></asp:TextBox>
                                            
                                            <input id="btVencimento_lancto" class="botaoCalendario" type="button" runat="server" />
                                        </label>
                                    </td>
                                    <td id="celulaFornecedorTitulo" style="display:none;">
                                        <label>
                                            <span>Terceiro</span>
                                            <asp:DropDownList ID="comboTerceiro_lancto" Width="250px" runat="server">
                                            </asp:DropDownList>
                                        </label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding:4px 0 4px 0;">
                        <asp:TextBox ID="textHistorico_lancto" Enabled="false" TextMode="MultiLine" Width="980px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id="boxLoading" style="display:none;">carregando...</span>
                        <input type="button" name="botaoCancelar_lancto" id="botaoCancelar_lancto" class="botao" value="Cancelar" runat="server" />
                        <input type="button" name="botaoSalvar_lancto" id="botaoSalvar_lancto" class="botao" value="Incluir" runat="server" />
                        
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <div id="boxTotalizador">
        <table>
            <tr>
                <td class="celulaLabelTotal">Totais</td>
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
            <td class="cotacao"></td>
            <td class="deletar"></td>
            <td class="parcelas" style="display:none;">N° Parc.</td>
            <td class="fornecedor" style="display:none;">Fornec.</td>
        </tr>
    </table>
    <div class="boxBotaoTerminei">
        
        <asp:Button ID="botaoSalvar" runat="server" Text="Concluir" />
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
            <fieldset id="formVencimento">
                
            </fieldset>
            <div>
                <input id="btSalvarVencimento" type="button" value="Salvar" runat="server" />
                <span style="display:none;"><asp:Button ID="botaoSalvarVencimento" runat="server" Text="Salvar" /></span>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
