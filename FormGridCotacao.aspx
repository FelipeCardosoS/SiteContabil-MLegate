<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridCotacao.aspx.cs" Inherits="FormGridCotacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="areaFiltro" Runat="Server">
    <label>
        <span>Moeda</span>
        <asp:DropDownList ID="comboMoeda" runat="server"></asp:DropDownList>
    </label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="areaGrid" Runat="Server">
        <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Código</td>
            <td>Descrição</td>
            <td>Valor</td>
            <td>Data</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_COTACAO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[COD_COTACAO]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[VALOR]")%></td>
                    <td class="line"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[DATA]")).ToString("dd/MM/yyyy")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" NavigateUrl='<%# "FormEditCadCotacao.aspx?id=" + DataBinder.Eval(Container.DataItem, "[COD_COTACAO]")  %>' runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
</asp:Content>

