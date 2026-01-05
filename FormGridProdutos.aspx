<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridProdutos.aspx.cs" Inherits="FormGridProdutos" %>

<asp:Content ID="areaFiltro" ContentPlaceHolderID="areaFiltro" Runat="Server">
    <label><span>Descrição</span>
        <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
    </label>
    <label><span>Gera Crédito</span>
        <asp:DropDownList ID="comboCredito" Width="80px" runat="server">
            <asp:ListItem Value="" Text="Nenhum"></asp:ListItem>
            <asp:ListItem Value="True" Text="Sim"></asp:ListItem>
            <asp:ListItem Value="False" Text="Não"></asp:ListItem>
        </asp:DropDownList>
    </label>
</asp:Content>
<asp:Content ID="areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Descrição</td>
            <td>Gera Crédito</td>
            <td>Sigla</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_PRODUTO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[GERA_CREDITO]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[SIGLA]")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" NavigateUrl='<%# "FormEditCadProdutos.aspx?id=" + DataBinder.Eval(Container.DataItem, "[COD_PRODUTO]")  %>' runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

