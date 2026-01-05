<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" AutoEventWireup="true" StylesheetTheme="principal" CodeFile="FormGridClassificacaoConta.aspx.cs" Inherits="FormGridClassificacaoConta" %>

<asp:Content ID="areaHead" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaFiltro" ContentPlaceHolderID="areaFiltro" Runat="Server">
    <label><span>Codigo</span>
        <asp:TextBox ID="codigoText" Width="80px" runat="server"></asp:TextBox>
    </label>
    <label><span>Descrição</span>
        <asp:TextBox ID="descricaoText" Width="180px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
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
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_CLASSIFICACAO]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[COD_CLASSIFICACAO]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" NavigateUrl='<%# "FormEditCadClassificacaoConta.aspx?id=" + DataBinder.Eval(Container.DataItem, "[COD_CLASSIFICACAO]")  %>' runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

