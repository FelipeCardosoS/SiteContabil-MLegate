<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridEncerramento.aspx.cs" Inherits="FormGridEncerramento" %>

<asp:Content ID="Content2" ContentPlaceHolderID="areaFiltro" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="areaGrid" Runat="Server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Período</td>
            <td>Lote</td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[cod_encerramento]") %>' runat="server" />
                    </td>
                    <td class="line"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[PERIODO]")).ToString("dd/MM/yyyy") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[LOTE]") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

