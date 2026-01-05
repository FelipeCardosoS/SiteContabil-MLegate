<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadEmpresas.aspx.cs" Inherits="FormEditCadEmpresas" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
	<link href="Plugins/chosen.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Plugins/chosen.jquery.js"></script>
    <script type="text/javascript" src="Plugins/chosen.proto.js"></script>
	<script type="text/javascript" src="Js/select2.min.js"></script>
    <script type="text/javascript" src="Js/FormEditCadEmpresas.js"></script>
    <style type="text/css">@import url(Css/select2.min.css);</style>

 <div class="legendaForm">Classificação</div>
    <fieldset>
        <span class="linha">
            <asp:UpdatePanel ID="UpRadio" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                <ContentTemplate>
                    <label style="width:50px;"></label>
                    <asp:RadioButtonList CssClass="campoRadio" AutoPostBack="true" OnSelectedIndexChanged="radioTipo_SelectedIndexChanged" ID="radioTipo" runat="server">
                        <asp:ListItem Text="Grupo" Value="GRUPO"></asp:ListItem>
                        <asp:ListItem Text="Empresa" Value="EMPRESA"></asp:ListItem>
                        <asp:ListItem Text="Fornecedor" Value="FORNECEDOR"></asp:ListItem>
                        <asp:ListItem Text="Cliente" Value="CLIENTE"></asp:ListItem>
                        <asp:ListItem Text="Cliente/Fornecedor" Value="CLIENTE_FORNECEDOR"></asp:ListItem>
                        <asp:ListItem Text="Emitente" Value="EMITENTE"></asp:ListItem>
                    </asp:RadioButtonList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </span>
        <p></p>
    </fieldset>
    <div class="legendaForm">Tipo de Pessoa</div>
    <fieldset>
        <span class="linha">
            <label style="width:50px;"></label>
            <asp:RadioButtonList CssClass="campoRadio" ID="radioFisicaJuridica" runat="server">
                <asp:ListItem Text="Física" Selected="True" Value="FISICA"></asp:ListItem>
                <asp:ListItem Text="Jurídica" Value="JURIDICA"></asp:ListItem>
            </asp:RadioButtonList>
        </span>
        <p></p>
    </fieldset>
    <div class="legendaForm">Dados Cadastrais</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_EMPRESA" runat="server" />
        <span class="linha">
            <label id="labelComboEconomico">Grupo Econômico</label>
            <asp:DropDownList ID="comboGrupoEconomico" runat="server"></asp:DropDownList>
        </span>

        <span class="linha">
            <label id="labelComboFinanceiroEntrada">Grupo Financeiro - Entrada</label>
            <asp:DropDownList ID="comboGrupoFinanceiroEntrada" runat="server"></asp:DropDownList>
        </span>

        <span class="linha">
            <label id="labelComboFinanceiroSaida">Grupo Financeiro - Saída</label>
            <asp:DropDownList ID="comboGrupoFinanceiroSaida" runat="server"></asp:DropDownList>
        </span>

        <span class="linha">
            <label id="labelNomeRazaoSocial">Razão Social</label>
            <asp:TextBox ID="textNomeRazaoSocial" Width="220px" MaxLength="200" runat="server"></asp:TextBox>
        </span>

        <span id="linhaNomeFantasia" class="linha">
            <label>Nome Fantasia</label>
            <asp:TextBox ID="textNomeFantasia" Width="180px" MaxLength="85" runat="server"></asp:TextBox>
        </span>

        <asp:UpdatePanel ID="UpSincronizar" runat="server">
            <ContentTemplate>
                <span id="linhaSincronizar" visible="false" runat="server" class="linha">
                    <label>Sincronizar?</label>
                    <asp:CheckBox ID="sincronizarCheckBox" runat="server" />
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>

		<asp:UpdatePanel ID="UpExigeContribuicoes" runat="server">
			<ContentTemplate>
				<span id="linhaExigeContribuicoes" visible="false" runat="server" class="linha">
					<label>Exigir Sped Contribuições?</label>
					<asp:CheckBox ID="exigeContribuicoesCheckBox" runat="server" />
				</span>
			</ContentTemplate>
		</asp:UpdatePanel>

        <span class="linha">
            <label>Matrícula</label>
            <asp:TextBox ID="textMatricula" Text="0" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Consultor</label>
            <asp:DropDownList ID="comboConsultor" runat="server"></asp:DropDownList>
        </span>

        <span class="linha">
            <label>Timesheet</label>
            <asp:DropDownList ID="comboEmpresaTimesheet" runat="server"></asp:DropDownList>
        </span>

        <span class="linha">
            <label id="labelCnpjCpf"></label>
            <asp:TextBox ID="textCnpjCpf" runat="server"></asp:TextBox>
        </span>

        <span id="linhaIM" class="linha">
            <label>Inscrição Municipal</label>
            <asp:TextBox ID="textIM" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label id="labelIeRg"></label>
            <asp:TextBox ID="textIe" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>CEP</label>
            <asp:TextBox ID="textCep" Width="60px" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Endereço</label>
            <asp:TextBox ID="textEndereco" Width="300px" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Numero</label>
            <asp:TextBox ID="textNumero" Width="60px" MaxLength="100" runat="server"></asp:TextBox>
        </span>

           <span class="linha">
            <label>Complemento</label>
            <asp:TextBox ID="textComplemento" Width="200px" MaxLength="200" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Bairro</label>
            <asp:TextBox ID="textBairro" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Municipio</label>
            <asp:TextBox ID="textMunicipio" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>UF</label>
            <asp:TextBox ID="textUf" Width="20px" MaxLength="2" runat="server"></asp:TextBox>
        </span>

		<asp:UpdatePanel ID="UpPaisMunicipio" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
			<Triggers>
				 <asp:AsyncPostBackTrigger ControlID="comboCodigoPais" />
				<asp:AsyncPostBackTrigger ControlID="comboCodigoMunicipio" />
				<asp:AsyncPostBackTrigger ControlID="botaoSalvar" />
			</Triggers>

			<ContentTemplate>
				<span class="linha">
					<label>Código País</label>
					<div style="float:left">
						<asp:DropDownList ID="comboCodigoPais" AutoPostBack="True" OnSelectedIndexChanged="comboCodigoPais_SelectedIndexChanged" Width="300px" runat="server"></asp:DropDownList>
					</div>
					<asp:label id="validacaoCodigoPais" style="color:red; padding-left:10px; line-height:28px;" visible="false" runat="server">Informe o Código do País.</asp:label>
				</span>
				<span class="linha">
						<label>Código Município</label>
					<div style="float:left">
						<asp:DropDownList ID="comboCodigoMunicipio" Enabled="false"  AutoPostBack="True" OnSelectedIndexChanged="comboCodigoMunicipio_SelectedIndexChanged" Width="300px" runat="server"></asp:DropDownList>
					</div>
					<asp:label id="validacaoCodigoMunicipio" style="color:red;padding-left:10px; line-height:28px;" visible="false" runat="server">Informe o Código do Município.</asp:label>
				</span>
			</ContentTemplate>
		</asp:UpdatePanel>

        <span class="linha">
            <label>Nire</label>
            <asp:TextBox ID="textNire" runat="server"></asp:TextBox>
        </span>

        <span class="linha">
            <label>Telefone</label>
            <asp:TextBox ID="textTelefone" onkeyup="formataTelefoneCelular()" Width="90px" runat="server"></asp:TextBox>
        </span>

        <span class="linha" style="display:none">
            <label>Plano de Contas</label>
            <asp:DropDownList ID="comboPlanoConta" runat="server"></asp:DropDownList>
        </span>
    </fieldset>

    <asp:UpdatePanel ID="UpModelo" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="radioTipo" />
        </Triggers>
        <ContentTemplate>
            <div id="legendaModelos" class="legendaForm" visible="false" runat="server">Sugestão de Modelo</div>
            <fieldset>
                <asp:Panel ID="painelModelosFornecedor" runat="server">
                    <span class="linha">
                        <label>Modelos</label>
                        <asp:DropDownList ID="comboModeloFornecedor" runat="server">
                        </asp:DropDownList>
                    </span>
                </asp:Panel>

                <asp:Panel ID="painelModelosCliente" Visible="false" runat="server">
                    <span class="linha">
                        <label>Modelos</label>
                        <asp:DropDownList ID="comboModeloCliente" runat="server">
                        </asp:DropDownList>
                    </span>
                </asp:Panel>

                <asp:Panel ID="painelModelosClienteFornecedor" Visible="false" runat="server">
                    <span class="linha">
                        <label style="width:210px;">Modelos Contas à pagar</label>
                        <asp:DropDownList ID="comboModeloClienteFornecedor_CP" runat="server">
                        </asp:DropDownList>
                    </span>

                    <span class="linha">
                        <label style="width:210px;">Modelos Contas à receber</label>
                        <asp:DropDownList ID="comboModeloClienteFornecedor_CR" runat="server">
                        </asp:DropDownList>
                    </span>
                </asp:Panel>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:RequiredFieldValidator ID="validaClassificacao" runat="server" Display="None"
        ControlToValidate="radioTipo" ErrorMessage="Selecione uma Classificação."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaClassificacao" runat="server"
        TargetControlID="validaClassificacao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaNomeRazaoSocial" runat="server" Display="None"
        ControlToValidate="textNomeRazaoSocial" ErrorMessage="Informe a Razão Social/Nome."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNomeRazaoSocial" runat="server"
        TargetControlID="validaNomeRazaoSocial" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaCnpjCpf" runat="server" Display="None"
        ControlToValidate="textCnpjCpf" ErrorMessage="Informe o CNPJ/CPF."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaCnpjCpf" runat="server"
        TargetControlID="validaCnpjCpf" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
    <toolKit:FilteredTextBoxExtender ID="filtraIM" runat="server"
        FilterType="Numbers, Custom" ValidChars=".-" TargetControlID="textIM"></toolKit:FilteredTextBoxExtender>
    
    <asp:RequiredFieldValidator ID="validaIe" runat="server" Display="None"
        ControlToValidate="textIe" ErrorMessage="Informe a Inscrição Estadudal/RG."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaIe" runat="server"
        TargetControlID="validaIe" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
    <asp:RequiredFieldValidator ID="validaCep" runat="server" Display="None"
        ControlToValidate="textCep" ErrorMessage="Informe o CEP."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaCep" runat="server"
        TargetControlID="validaCep" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaEndereco" runat="server" Display="None"
        ControlToValidate="textEndereco" ErrorMessage="Informe o Endereço."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaEndereco" runat="server"
        TargetControlID="validaEndereco" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaBairro" runat="server" Display="None"
        ControlToValidate="textBairro" ErrorMessage="Informe o Bairro."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaBairro" runat="server"
        TargetControlID="validaBairro" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaMunicipio" runat="server" Display="None"
        ControlToValidate="textMunicipio" ErrorMessage="Informe o Município."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaMunicipio" runat="server"
        TargetControlID="validaMunicipio" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaUf" runat="server" Display="None"
        ControlToValidate="textUf" ErrorMessage="Informe a UF."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaUf" runat="server"
        TargetControlID="validaUf" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <toolKit:FilteredTextBoxExtender ID="filtraUf" runat="server"
        FilterType="UppercaseLetters" TargetControlID="textUf"></toolKit:FilteredTextBoxExtender>

    <asp:RequiredFieldValidator ID="validaTelefone" runat="server" Display="None"
        ControlToValidate="textTelefone" ErrorMessage="Informe o Telefone."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaTelefone" runat="server"
        TargetControlID="validaTelefone" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
</asp:Content>