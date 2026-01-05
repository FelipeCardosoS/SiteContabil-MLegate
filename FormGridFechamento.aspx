<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridFechamento.aspx.cs" Inherits="FormGridFechamento" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Início</span>
        <asp:TextBox ID="textDataInicio" Width="80px" runat="server"></asp:TextBox>
    </label>
    <label><span>Término</span>
        <asp:TextBox ID="textDataTermino" Width="80px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Período</td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[PERIODO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[PERIODO]") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    
    <toolKit:ModalPopupExtender ID="popFechar" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" CancelControlID="closePop" 
        TargetControlID="botaoFechar" BackgroundCssClass="pop_background" PopupControlID="blocoPop">
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPop" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Abrir Período.</div>
            <asp:LinkButton ID="closePop" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <fieldset>
                <label style="width:80px;">Período: </label>
                <asp:TextBox ID="textPeriodo" runat="server"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="validaPeriodo" ValidationGroup="popFechamentoPeriodo" runat="server" Display="None"
                    ControlToValidate="textPeriodo" ErrorMessage="Informe o Período para realizar o fechamento."></asp:RequiredFieldValidator>
                
                <toolKit:ValidatorCalloutExtender ID="ajaxValidaPeriodo" runat="server"
                    TargetControlID="validaPeriodo" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                    HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
                    
                <asp:Button ID="botaoExecutaFechamento" OnClick="botaoExecutaFechamento_Click" ValidationGroup="popFechamentoPeriodo" runat="server" Text="Abrir" />
            </fieldset>
        </div>
    </asp:Panel>
</asp:Content>