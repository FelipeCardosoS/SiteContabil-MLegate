<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridTiposImposto.aspx.cs" Inherits="FormGridTiposImposto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaFiltro" ContentPlaceHolderID="areaFiltro" Runat="Server">
    <label><span>Tipo Imposto</span>
        <asp:TextBox ID="textTipoImposto" Width="80px" runat="server"></asp:TextBox>
    </label>
    <label><span>Descrição</span>
        <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
    </label>
</asp:Content>
<asp:Content ID="areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Tipo</td>
            <td>Descrição</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[tipo_imposto]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[tipo_imposto]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" NavigateUrl='<%# "FormEditCadTiposImposto.aspx?id=" + DataBinder.Eval(Container.DataItem, "[tipo_imposto]")  %>' runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

