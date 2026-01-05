<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormContabilizacao.aspx.cs" Inherits="FormContabilizacao" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <script type="text/javascript" src="Js/FormContabilizacao.js"></script>
    <fieldset>
        <span class="linha">
            <label style="color:blue">Emitente: </label>
            <asp:DropDownList ID="ddlEmitente" Width="858px" OnSelectedIndexChanged="ddlEmitente_Changed" AutoPostBack="true" CssClass="ddlEmitente" runat="server"></asp:DropDownList>
        </span>
        <table id="Servicos" cellpadding="0" cellspacing="0" style="margin-top:12px; border:1px solid #B0C4DE" class="Visivel">
            <thead>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE;" colspan="2" rowspan="2">Serviços</th>
                    <th style="color:blue; border:1px solid #B0C4DE;" colspan="4">Débito</th>
                    <th style="color:blue; border:1px solid #B0C4DE;" colspan="4">Crédito</th>
                </tr>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Valor</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>

                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Valor</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="repeaterServicos" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="display:none"><input type="checkbox" id="cbxInsert" class="cbxInsert" runat="server" /></td>
                            <td style="display:none"><input type="checkbox" id="cbxUpdate" class="cbxUpdate" runat="server" /></td>
                            <td style="display:none"><input type="hidden" id="cod_servico" class="cod_servico" value='<%# DataBinder.Eval(Container.DataItem, "[COD_SERVICO]") %>' runat="server" /></td>
                            <td class="line" colspan="2"><input type="text" id="tbxNomeServico" class="tbxNomeServico" style="width:175px" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' disabled="disabled" runat="server" /></td>
                            
                            <td class="line"><asp:DropDownList ID="ddlContaDebito" CssClass="ddlContaDebito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:DropDownList ID="ddlBrutoLiquidoDebito" CssClass="ddlBrutoLiquidoDebito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloDebito" class="cbxGeraTituloDebito" style="width:55px; height:16px;" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoDebito" CssClass="tbxHistoricoDebito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>

                            <td class="line"><asp:DropDownList ID="ddlContaCredito" CssClass="ddlContaCredito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:DropDownList ID="ddlBrutoLiquidoCredito" CssClass="ddlBrutoLiquidoCredito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloCredito" class="cbxGeraTituloCredito" style="width:55px; height:16px;" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoCredito" CssClass="tbxHistoricoCredito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table id="Retencoes" cellpadding="0" cellspacing="0" style="margin-top:30px; border:1px solid #B0C4DE" class="Visivel">
            <thead>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="2" rowspan="2">Retenções</th>
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="4">Débito</th>
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="4">Crédito</th>
                </tr>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Terceiro</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>

                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Terceiro</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="repeaterRetencoes" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="display:none"><input type="checkbox" id="cbxInsert" class="cbxInsert" runat="server" /></td>
                            <td style="display:none"><input type="checkbox" id="cbxUpdate" class="cbxUpdate" runat="server" /></td>
                            <td style="display:none"><input type="hidden" id="cod_retencao" class="cod_retencao" value='<%# DataBinder.Eval(Container.DataItem, "[COD_RETENCAO]") %>' runat="server" /></td>
                            <td class="line" colspan="2"><input type="text" id="tbxNomeRetencao" class="tbxNomeRetencao" style="width:175px" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' disabled="disabled" runat="server" /></td>
                            
                            <td class="line"><asp:DropDownList ID="ddlContaDebito" CssClass="ddlContaDebito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloDebito" class="cbxGeraTituloDebito" style="width:55px; height:16px;" onclick="cbxGeraTituloDebito_Click(this)" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:DropDownList ID="ddlTerceiroDebito" CssClass="ddlTerceiroDebito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoDebito" CssClass="tbxHistoricoDebito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>

                            <td class="line"><asp:DropDownList ID="ddlContaCredito" CssClass="ddlContaCredito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloCredito" class="cbxGeraTituloCredito" style="width:55px; height:16px;" onclick="cbxGeraTituloCredito_Click(this)" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:DropDownList ID="ddlTerceiroCredito" CssClass="ddlTerceiroCredito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoCredito" CssClass="tbxHistoricoCredito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table id="Tributos" cellpadding="0" cellspacing="0" style="margin-top:30px; border:1px solid #B0C4DE" class="Visivel">
            <thead>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="2" rowspan="2">Impostos Sobre Vendas</th>
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="4">Débito</th>
                    <th style="color:blue; border:1px solid #B0C4DE" colspan="4">Crédito</th>
                </tr>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Terceiro</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>

                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Terceiro</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="repeaterTributos" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="display:none"><input type="checkbox" id="cbxInsert" class="cbxInsert" runat="server" /></td>
                            <td style="display:none"><input type="checkbox" id="cbxUpdate" class="cbxUpdate" runat="server" /></td>
                            <td style="display:none"><input type="hidden" id="cod_tributo" class="cod_tributo" value='<%# DataBinder.Eval(Container.DataItem, "[COD_TRIBUTO]") %>' runat="server" /></td>
                            <td class="line" colspan="2"><input type="text" id="tbxNomeTributo" class="tbxNomeTributo" style="width:175px" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' disabled="disabled" runat="server" /></td>
                            
                            <td class="line"><asp:DropDownList ID="ddlContaDebito" CssClass="ddlContaDebito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloDebito" class="cbxGeraTituloDebito" style="width:55px; height:16px;" onclick="cbxGeraTituloDebito_Click(this)" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:DropDownList ID="ddlTerceiroDebito" CssClass="ddlTerceiroDebito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoDebito" CssClass="tbxHistoricoDebito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>

                            <td class="line"><asp:DropDownList ID="ddlContaCredito" CssClass="ddlContaCredito" Width="125px" runat="server"></asp:DropDownList></td>
                            <td class="line"><input type="checkbox" id="cbxGeraTituloCredito" class="cbxGeraTituloCredito" style="width:55px; height:16px;" onclick="cbxGeraTituloCredito_Click(this)" disabled="disabled" runat="server" /></td>
                            <td class="line"><asp:DropDownList ID="ddlTerceiroCredito" CssClass="ddlTerceiroCredito" Width="80px" Enabled="False" runat="server"></asp:DropDownList></td>
                            <td class="line"><asp:TextBox ID="tbxHistoricoCredito" CssClass="tbxHistoricoCredito" Width="150px" Height="20px" TextMode="MultiLine" MaxLength="500" Enabled="False" runat="server"></asp:TextBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <span class="linha Visivel" style="margin-top:40px">
            <label style="color:blue; margin-left:90px">Contas A Receber</label>
            <label style="color:blue; margin-left:155px">Débito/Crédito</label>
            <label style="color:blue; margin-left:-25px">Gera Título</label>
            <label style="color:blue; margin-left:115px">Histórico</label>
            <span class="linha">
                <asp:DropDownList ID="ddlContaDiferenca" CssClass="ddlContaDiferenca" Width="350px" Style="margin-top:-15px" runat="server" />
                <asp:DropDownList ID="ddlDebitoCredito" CssClass="ddlDebitoCredito" Width="100px" Style="margin-left:80px; margin-top:-15px" runat="server" />
                <input type="checkbox" id="cbxGeraTitulo" class="cbxGeraTitulo" style="width:55px; height:16px; margin-left:42px; margin-top:-14px;" runat="server"/>
                <input type="checkbox" id="cbxInsert" class="cbxInsertContaDiferenca" style="display:none" runat="server"/>
                <input type="checkbox" id="cbxUpdate" class="cbxUpdateContaDiferenca" style="display:none" runat="server"/>
                <asp:TextBox ID="tbxHistorico" CssClass="tbxHistorico" Width="350px" Height="20px" TextMode="MultiLine" MaxLength="500" Style="margin-left:57px; margin-top:-20px" runat="server" />
            </span>
        </span>
    </fieldset>
    <div class="botoes">
        <input type="button" id="btnSalvar" value="Salvar" onclick="btnSalvar_Click()" class="btnSalvar Visivel" runat="server" />
    </div>
</asp:Content>