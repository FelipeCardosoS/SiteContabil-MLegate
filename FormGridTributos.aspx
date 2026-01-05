<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridTributos.aspx.cs" Inherits="FormGridTributos" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label id="linhaNome"><span>Nome</span>
        <asp:TextBox ID="textNome" Width="250px" MaxLength="200" runat="server" />
    </label>

    <label id="linhaAliquota"><span>Alíquota</span>
        <asp:TextBox ID="textAliquota" Width="100px" CssClass="Monetaria" MaxLength="18" runat="server" />
    </label>
    
    <label><span>Emitente</span>
        <asp:DropDownList ID="comboEmitente" Width="350px" runat="server">
        </asp:DropDownList>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome</td>
            <td>Alíquota</td>
            <td>Emitente</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%"><input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_TRIBUTO]") %>' runat="server" /></td>
                    <td class="line"><asp:Label ID="nome" Text='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' runat="server" /></td>
                    <td class="line valor"><%# DataBinder.Eval(Container.DataItem, "[ALIQUOTA]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[EMITENTE]") %></td>
                    <td class="funcoes line" width="1%"><p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>