<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridContatosEmpresa.aspx.cs" Inherits="FormGridContatosEmpresa" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Nome Completo</span>
        <asp:TextBox ID="textNomeCompleto" Width="250px" runat="server"></asp:TextBox>
    </label>
    <label><span>Empresa</span>
        <asp:DropDownList ID="comboEmpresa" Width="250px" runat="server">
        </asp:DropDownList>
    </label>
    <label><span>E-mail</span>
        <asp:TextBox ID="textEmail" Width="180px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome</td>
            <td>Cep</td>
            <td>Telefone</td>
            <td>E-mail</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_CONTATO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_COMPLETO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[CEP]") %></td>
                    <td class="line"><asp:Literal ID="lTelefone" Text='<%# DataBinder.Eval(Container.DataItem, "[TELEFONE]") %>' runat="server"></asp:Literal></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[EMAIL]") %></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>