<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" AutoEventWireup="true" StylesheetTheme="principal" CodeFile="FormGridMoeda.aspx.cs" Inherits="FormGridMoeda" %>

<asp:Content ID="Content2" ContentPlaceHolderID="areaFiltro" Runat="Server">
    <label>
        <span>Descrição</span>
        <asp:TextBox ID="txtDescricao" Width="180px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="areaGrid" Runat="Server">
        <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Código</td>
            <td>Descrição</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_MOEDA]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[COD_MOEDA]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" NavigateUrl='<%# "FormEditCadMoeda.aspx?id=" + DataBinder.Eval(Container.DataItem, "[COD_MOEDA]")  %>' runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

