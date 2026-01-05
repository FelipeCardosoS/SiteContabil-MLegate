<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" CodeFile="FormGridModelos.aspx.cs" Inherits="FormGridModelos" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Nome</span>
        <asp:TextBox ID="textNome" Width="200px" runat="server"></asp:TextBox>
    </label>
    <label><span>Tipo</span>
        <asp:DropDownList ID="comboTipo" Width="140px" runat="server">
            <asp:ListItem Text="Escolha" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Contas à Pagar" Value="CP"></asp:ListItem>
            <asp:ListItem Text="Contas à Receber" Value="CR"></asp:ListItem>
            <asp:ListItem Text="Contabilidade" Value="C"></asp:ListItem>
        </asp:DropDownList>
    </label>
    <label><span>Default?</span>
        <asp:DropDownList ID="comboDefault" runat="server">
            <asp:ListItem Text="Escolha" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Sim" Value="True"></asp:ListItem>
            <asp:ListItem Text="False" Value="False"></asp:ListItem>
        </asp:DropDownList>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Descrição</td>
            <td>Tipo</td>
            <td>Default?</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line">
                        <asp:HiddenField ID="hiddenTipo" Value='<%# DataBinder.Eval(Container.DataItem, "[TIPO]") %>' runat="server" />
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_MODELO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO_TIPO]") %></td>
                    <td class="line">
                        <asp:Literal ID="lDefault" Text='<%# DataBinder.Eval(Container.DataItem, "[PADRAO_DEFAULT]") %>' runat="server"></asp:Literal>
                    </td>
                    <td class="funcoes line" width="10%">
                        <asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink>&nbsp;|&nbsp;<asp:HyperLink ID="linkLanctos" runat="server">Lanctos</asp:HyperLink>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>