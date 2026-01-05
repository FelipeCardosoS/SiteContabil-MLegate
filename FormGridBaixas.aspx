<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridBaixas.aspx.cs" Inherits="FormGridBaixas" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Baixa</span>
        <asp:TextBox ID="textBaixa" Width="60px" runat="server" />
    </label>
    <label><span>Inicio</span>
        <asp:TextBox ID="textDataInicio" Width="80px" runat="server" />
    </label>
    <label><span>Término</span>
        <asp:TextBox ID="textDataTermino" Width="80px" runat="server" />
    </label>
    <label><span>Terceiros</span>
        <asp:DropDownList ID="comboTerceiro" Width="220px" runat="server">
        </asp:DropDownList>     
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Baixa</td>
            <td>Data</td>
            <td>Banco</td>
            <td>Terceiros</td>
            <td>Valor($)</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[SEQ_BAIXA]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[SEQ_BAIXA]") %></td>
                    <td class="line"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[data]")).ToString("dd/MM/yyyy") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[Terceiros]") %></td>
                    <td class="line"><%# String.Format("{0:0,0.00}", Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "[VALOR]"))) %></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkDetalhe" runat="server">Detalhe</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
