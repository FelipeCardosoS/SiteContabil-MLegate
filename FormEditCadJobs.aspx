<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadJobs.aspx.cs" Inherits="FormEditCadJobs" %>

<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<link href="Plugins/chosen.min.css" rel="stylesheet" type="text/css" />
	<link href="Css/CadJobs.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="Plugins/chosen.jquery.js"></script>
	<script type="text/javascript" src="Plugins/chosen.proto.js"></script>
	<script type="text/javascript" src="Js/select2.min.js"></script>
	<script type="text/javascript" src="Js/CadJobs.js"></script>
	<script type="text/javascript" src="Js/validacao.js"></script>
	<script type="text/javascript" src="Js/principal.js"></script>

</asp:Content>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" runat="Server">
	<div class="legendaForm">Informe os dados do Job</div>
	<table class="menu">
		<thead>
			<tr class="titulo">
				<th class="job ativo">Job
				</th>

				<th class="consultor">Consultor
				</th>
				<th class="despesa">Despesa
				</th>
				<th class="fatura">Fatura
				</th>
			</tr>
		</thead>
	</table>

	<div class="job">
		<fieldset>
			<asp:HiddenField ID="hdGestores" runat="server" />
			<asp:HiddenField ID="hdDespesas" runat="server" />
			<asp:HiddenField ID="hdGestorJob" runat="server" />
			<asp:HiddenField ID="hdConsultorJob" runat="server" />
			<asp:HiddenField ID="hdDespesaJob" runat="server" />
			<asp:HiddenField ID="hdGestorJobDeletar" runat="server" />
			<asp:HiddenField ID="hdConsultorJobDeletar" runat="server" />
			<asp:HiddenField ID="hdDespesaJobDeletar" runat="server" />

			<span class="linha">
				<label>Cliente</label>
				<asp:DropDownList ID="ddlCliente" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvCliente" ControlToValidate="ddlCliente" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceCliente" TargetControlID="cvCliente" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>
			<span class="linha">
				<label>Linha de Negócio</label>
				<asp:DropDownList ID="ddlLinhaNegocio" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvLinhaNegocio" ControlToValidate="ddlLinhaNegocio" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceLinhaNegocio" TargetControlID="cvLinhaNegocio" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>
			<span class="linha">
				<label>Divisão</label>
				<asp:DropDownList ID="ddlDivisao" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvDivisao" ControlToValidate="ddlDivisao" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceDivisao" TargetControlID="cvDivisao" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>
			<span class="linha">
				<label>Nome</label>
				<asp:TextBox ID="tbxNome" Width="300px" runat="server" />
				<asp:RequiredFieldValidator ID="rfvNome" ControlToValidate="tbxNome" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceNome" TargetControlID="rfvNome" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>
			<span class="linha">
				<label>Status</label>
				<asp:DropDownList ID="ddlStatus" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvStatus" ControlToValidate="ddlStatus" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceStatus" TargetControlID="cvStatus" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>

			<span class="linha">
				<label>Status Timesheet</label>
				<asp:DropDownList ID="ddlStatusTimesheet" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvStatusTimesheet" ControlToValidate="ddlStatusTimesheet" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceStatusTimesheet" TargetControlID="cvStatusTimesheet" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>

			<span class="linha">
				<label>Projeto</label>
				<asp:DropDownList ID="ddlProjeto" Width="308px" runat="server" />
				<asp:CustomValidator ID="cvProjeto" ControlToValidate="ddlProjeto" ClientValidationFunction="validaCombo" ErrorMessage="Campo Obrigatório." Display="None" runat="server" />
				<toolKit:ValidatorCalloutExtender ID="vceProjeto" TargetControlID="cvProjeto" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
			</span>

			<span class="linha">
				<label>Gestores</label>
				<button type="button" class="btnGestores" onclick="toggleGestores()">Esconder</button>
			</span>

			<div class="divGestores">
				<asp:UpdatePanel ID="UpRegras" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
					<ContentTemplate>


						<div id="grid">
							<table id="tableGestores" cellpadding="0" cellspacing="0" style="margin-top: 30px; border: 1px solid #B0C4DE" class="Visivel">
								<thead>
									<tr class="titulo">
										<th style="display:none"></th>
										<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Gestores</th>
										<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Início</th>
										<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Data Fim</th>
										<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Status</th>
										<th></th>
										<th></th>
									</tr>
								</thead>
								<tbody>
								</tbody>
							</table>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>



			<span <% if (!MedicalPrime) { Response.Write("style=\"display:none;\""); } %>>
				<span class="linha">
					<label>Médico</label>
					<asp:DropDownList ID="ddlMedico" Width="308px" runat="server" />
				</span>
				<span class="linha">
					<label>Equipe</label>
					<asp:DropDownList ID="ddlEquipe" Width="308px" runat="server" />
				</span>
				<span class="linha">
					<label>Vendedor</label>
					<asp:DropDownList ID="ddlVendedor" Width="308px" runat="server" />
				</span>
				<span class="linha">
					<label>Paciente</label>
					<asp:TextBox ID="tbxPaciente" Width="300px" runat="server" />
				</span>
				<span class="linha">
					<label>Convênio</label>
					<asp:TextBox ID="tbxConvenio" Width="300px" runat="server" />
				</span>
			</span>
			<span class="linha">
				<label>Descrição</label>
				<asp:TextBox ID="tbxDescricao" TextMode="MultiLine" Width="298px" Height="50px" runat="server" />
			</span>
			<span class="linha" style="margin-top: 2%">
				<label>Observações da Nota Fiscal</label>
				<asp:TextBox ID="tbxObsNF" TextMode="MultiLine" Width="298px" Height="50px" CssClass="MaxLines" MaxLines="4" MaxLength="400" runat="server" />
			</span>
		</fieldset>
	</div>
	<div class="consultor" style="display: none">

		<table id="tableConsultores" cellpadding="0" cellspacing="0" style="margin-top: 30px; border: 1px solid #B0C4DE;" class="Visivel">
			<%--			<thead>
				<tr class="titulo">
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Consultor</th>
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Aprovador Horas</th>
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Aprovador RDV</th>
					<th></th>
					<th></th>
				</tr>
			</thead>--%>
			<tbody>
			</tbody>
		</table>
	</div>

	
	<div class="despesa" style="display:none">
		<table id="tableDespesas" cellpadding="0" cellspacing="0" style="margin-top: 30px; border: 1px solid #B0C4DE;" class="Visivel">
			<thead>
				<tr class="titulo">
					<th></th>
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Tipo Despesa</th>
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Cobrar do Cliente</th>
					<th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Limite Diário</th>
				</tr>
			</thead>
			<tbody>

			</tbody>

		</table>

	</div>



	<script type="text/javascript">
		$(document).ready(function () {
			//Observações: Limita o número máximo de quebras de linha (Enter), informado no atributo "attr('MaxLines')", o valor é atribuído no Front-End e o atributo 'MaxLines' não existe no contexto deste projeto, foi criado para atender esta necessidade.
			$('.MaxLines').keydown(function (event) {
				if (event.which == 13) { //Tabela ASCII (Enter)
					var numberOfLines = $(this).val().split('\n').length;
					if (numberOfLines >= $(this).attr('MaxLines')) {
						event.preventDefault();
					}
				}
			});
		});
	</script>
</asp:Content>
