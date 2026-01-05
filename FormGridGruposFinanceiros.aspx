<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" AutoEventWireup="true" CodeFile="FormGridGruposFinanceiros.aspx.cs" Inherits="FormGridGrupoFinanceiro" StylesheetTheme="principal" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" Runat="Server">
        <label id="linhaDescricao"><span id="labelDescricao">Descrição</span>
        <asp:TextBox ID="textDescricao" Width="160px" runat="server"></asp:TextBox>
    </label>
</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
<table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome</td>
            <td>Descrição</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[GRUPO_FINANCEIRO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[GRUPO_FINANCEIRO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

