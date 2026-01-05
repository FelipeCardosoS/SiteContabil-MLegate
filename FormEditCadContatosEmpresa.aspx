<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadContatosEmpresa.aspx.cs" Inherits="FormEditCadContatosEmpresa" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados do Contato</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_CONTATO" runat="server" />
        <span class="linha">
            <label>Empresa</label>
            <asp:DropDownList ID="comboEmpresa" Width="220px" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Função</label>
            <asp:DropDownList ID="comboFuncao" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Nome Completo</label>
            <asp:TextBox ID="textNomeCompleto" Width="270px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>CEP</label>
            <asp:TextBox ID="textCep" Width="80px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Endereço</label>
            <asp:TextBox ID="textEndereco" Width="180px" runat="server"></asp:TextBox>
            <label class="lado">N</label>
            <asp:TextBox ID="textNumero" Width="30px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Bairro</label>
            <asp:TextBox ID="textBairro" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Cidade</label>
            <asp:TextBox ID="textCidade" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Estado</label>
            <asp:TextBox ID="textEstado" MaxLength="2" Width="30px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Telefone</label>
            <asp:TextBox ID="textTelefone" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Email</label>
            <asp:TextBox ID="textEmail" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Enviar?</label>
            <asp:RadioButtonList CssClass="campoRadio" ID="radioEnviar" runat="server">
                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                <asp:ListItem Text="Não" Selected="True" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </span>
    </fieldset>
    
    <asp:CustomValidator ID="validaComboFuncao" runat="server" ClientValidationFunction="validaCombo"
        ControlToValidate="comboFuncao" ErrorMessage="Escolha uma Função." Display="None"></asp:CustomValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxValidaComboFuncao" runat="server"
        TargetControlID="validaComboFuncao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaNomeCompleto" runat="server" Display="None"
        ControlToValidate="textNomeCompleto" ErrorMessage="Informe o Nome Completo."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNomeCompleto" runat="server"
        TargetControlID="validaNomeCompleto" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
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
        
    <asp:RequiredFieldValidator ID="validaNumero" runat="server" Display="None"
        ControlToValidate="textNumero" ErrorMessage="Informe o Número."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNumero" runat="server"
        TargetControlID="validaNumero" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaBairro" runat="server" Display="None"
        ControlToValidate="textBairro" ErrorMessage="Informe o Bairro."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaBairro" runat="server"
        TargetControlID="validaBairro" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaCidade" runat="server" Display="None"
        ControlToValidate="textCidade" ErrorMessage="Informe a Cidade."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaCidade" runat="server"
        TargetControlID="validaCidade" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaEstado" runat="server" Display="None"
        ControlToValidate="textEstado" ErrorMessage="Informe o Estado."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaEstado" runat="server"
        TargetControlID="validaEstado" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaTelefone" runat="server" Display="None"
        ControlToValidate="textTelefone" ErrorMessage="Informe o Telefone."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaTelefone" runat="server"
        TargetControlID="validaTelefone" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaEmail" runat="server" Display="None"
        ControlToValidate="textEmail" ErrorMessage="Informe o Email."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaEmail" runat="server"
        TargetControlID="validaEmail" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <toolKit:FilteredTextBoxExtender ID="filtraNumero" runat="server"
        FilterType="Numbers"  TargetControlID="textNumero"></toolKit:FilteredTextBoxExtender>
    
    <toolKit:FilteredTextBoxExtender ID="filtraEstado" runat="server"
        FilterType="UppercaseLetters"  TargetControlID="textEstado"></toolKit:FilteredTextBoxExtender>
</asp:Content>