<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridTipoDespesas.aspx.cs" Inherits="FormGridTipoDespesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" Runat="Server">
	<label id="linhaDescricao">
		<span>Descrição</span>
		<asp:TextBox ID="textDescricao" Width="200px" runat="server"></asp:TextBox>
	</label>
	<label id="linhaUnidade">
		<span>Unidade</span>
		<asp:TextBox ID="textUnidade" Width="200px" runat="server"></asp:TextBox>
	</label>

</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
	    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Descrição</td>
            <td>Unidade</td>
            <td>Valor Referência Atual</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_TIPO_DESPESA]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[UNIDADE]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[VALOR_REFERENCIA]") %></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

</asp:Content>

