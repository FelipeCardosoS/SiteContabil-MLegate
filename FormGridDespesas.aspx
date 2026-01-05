<%@ Page Title="" Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridDespesas.aspx.cs" Inherits="FormGridCadDespesas" %>

<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="Server">
	<label>
		<span>Tipo Despesa</span>
		<asp:DropDownList ID="ddlTipoDespesa" runat="server">
		</asp:DropDownList>
	</label>
	<label>
		<span>Descrição</span>
		<asp:TextBox ID="textDescricao" Width="200px" runat="server"></asp:TextBox>
	</label>
</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="Server">
	<table id="tabelaGrid" cellpadding="0" cellspacing="0">
		<tr class="titulo">
			<td></td>
			<td>Descrição</td>
			<td>Tipo Despesa</td>
			<td></td>
		</tr>
		<asp:Repeater ID="repeaterDados" runat="server">
			<ItemTemplate>
				<tr class="linha">
					<td class="check line" width="1%">
						<input type="checkbox" id="check" class="check" name="check"
							value='<%# DataBinder.Eval(Container.DataItem, "[COD_DESPESA]") %>' runat="server" />
					</td>
					<td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
					<td class="line"><%# DataBinder.Eval(Container.DataItem, "[TIPO_DESPESA]") %></td>
					<td class="funcoes line" width="1%">
						<p>
							<asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
					</td>
				</tr>
			</ItemTemplate>
		</asp:Repeater>
	</table>
</asp:Content>

