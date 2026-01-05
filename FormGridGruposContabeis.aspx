<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridGruposContabeis.aspx.cs" Inherits="FormGridGruposContabeis" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Descrição</span>
        <asp:TextBox ID="textDescricao" Width="220px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Descrição</td>
            <td>Regra Exibição</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_GRUPO_CONTABIL]") %>' runat="server" />
                        
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[REGRA_EXIBICAO]") %></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
