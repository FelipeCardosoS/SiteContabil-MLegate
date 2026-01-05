<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadDespesas.aspx.cs" Inherits="FormEditCadDespesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="Server">
	<link href="Plugins/chosen.min.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="Plugins/chosen.jquery.js"></script>
	<script type="text/javascript" src="Plugins/chosen.proto.js"></script>
	<script type="text/javascript" src="Js/select2.min.js"></script>
	<style type="text/css">
		@import url(Css/select2.min.css);
	</style>

	<fieldset>
		<asp:HiddenField ID="H_COD_DESPESA" runat="server" />
		<span class="linha">
			<label id="lblDescricao">Descrição</label>
			<asp:TextBox ID="textDescricao" Width="220px" MaxLength="200" runat="server"></asp:TextBox>
		</span>

		<span class="linha">
			<label>Tipo de Despesa</label>
			<asp:DropDownList ID="ddlTipoDespesa" Width="308px" runat="server" />
			<asp:CustomValidator ID="cvTipoDespesa" ControlToValidate="ddlTipoDespesa" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
			<toolKit:ValidatorCalloutExtender ID="vceTipoDespesa" TargetControlID="cvTipoDespesa" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
		</span>

		<span class="linha">
			<label>Obrigar Anexo RDV?</label>
			<asp:CheckBox ID="chkAnexo" runat="server"></asp:CheckBox>
		</span>

		<span class="linha">
			<label>Tipo Valor</label>
			<asp:DropDownList ID="ddlTipoValor" runat="server">
				<asp:ListItem Text="Valor Fixo" Value="0"></asp:ListItem>
				<asp:ListItem Text="Valor Livre" Value="1"></asp:ListItem>
			</asp:DropDownList>
		</span>
		<asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
			ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição."></asp:RequiredFieldValidator>
		<toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
			TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif"
			HighlightCssClass="validacaoErro">
		</toolKit:ValidatorCalloutExtender>
	</fieldset>

</asp:Content>

