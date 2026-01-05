<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridUsuarios.aspx.cs" Inherits="FormGridUsuarios" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Nome</span>
        <asp:TextBox ID="textNome" Width="220px" runat="server"></asp:TextBox>
    </label>
    <label><span>Perfil</span>
        <asp:DropDownList ID="comboPerfil" runat="server">
        </asp:DropDownList>
    </label>
</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome</td>
            <td>Login</td>
            <td>Perfil</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_USUARIO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_COMPLETO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[Login]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="funcoes line" width="1%">
                        <asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink>&nbsp;|&nbsp;<asp:HyperLink ID="linkAlterarSenha" runat="server">Editar&nbsp;Senha</asp:HyperLink>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>