<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridJobs.aspx.cs" Inherits="FormGridJobs" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label>
        <span>Cliente</span>
        <asp:DropDownList ID="ddlCliente" Width="450px" runat="server"></asp:DropDownList>
    </label>

    <label>
        <span>Nome</span>
        <asp:TextBox ID="tbxNome" Width="150px" runat="server"></asp:TextBox>
    </label>

    <label>
        <span>Descrição</span>
        <asp:TextBox ID="tbxDescricao" Width="250px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Cliente</td>
            <td>Nome</td>
            <td>Descrição</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line check" width="1%"><input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_JOB]") %>' runat="server" /></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line funcoes" width="1%"><p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>