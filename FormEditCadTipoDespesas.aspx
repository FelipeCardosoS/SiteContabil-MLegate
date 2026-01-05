<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadTipoDespesas.aspx.cs" Inherits="FormEditCadTipoDespesas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
		<link href="Plugins/chosen.min.css" rel="stylesheet" type="text/css" />
		<link href="Css/FormEditCadTipoDespesas.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Plugins/chosen.jquery.js"></script>
    <script type="text/javascript" src="Plugins/chosen.proto.js"></script>
	<script type="text/javascript" src="Js/select2.min.js"></script>
    <script type="text/javascript" src="Js/FormEditCadTipoDespesas.js"></script>
    <style type="text/css">@import url(Css/select2.min.css);</style>

    <fieldset>
		<asp:HiddenField ID="H_COD_TIPO_DESPESA" runat="server" />
		<span class="linha">
			<label>Descrição</label>
			<asp:TextBox ID="textDescricao" Text="" runat="server"></asp:TextBox>
		</span>

		<span class="linha">
			<label>Unidade</label>
			<asp:TextBox ID="textUnidade" Text="" MaxLength="5" runat="server"></asp:TextBox>
		</span>
		<span class="linha">
			<label>Tipo</label>
			<asp:RadioButtonList ID="radioTipo" runat="server">
				<asp:ListItem Text="Quantitativo" Value="1"></asp:ListItem>
				<asp:ListItem Text="Valor" Value="0"></asp:ListItem>
			</asp:RadioButtonList>
		</span>
    </fieldset>
	<div class="periodos">
		<div class="legendaForm">Períodos</div>
		<fieldset>
			<asp:HiddenField ID="H_PERIODOS" runat="server" />
			<asp:HiddenField ID="H_PERIODOS_DELETAR" runat="server" />
			<div id="grid">
				<table id="tablePeriodos" cellpadding="0" cellspacing="0" style="margin-top: 30px; border: 1px solid #B0C4DE" class="Visivel">
					<thead>
						<tr class="titulo">
							<th></th>
							<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Início</th>
							<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Fim</th>
							<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Valor Referência</th>
							<th></th>
							<th></th>
							<th></th>
						</tr>
					</thead>
					<tbody>
					</tbody>
				</table>
			</div>

		</fieldset>
	</div>
</asp:Content>

