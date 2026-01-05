<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadUsuarios.aspx.cs" Inherits="FormEditCadUsuarios" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados do Usuário</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_USUARIO" runat="server" />
        <span class="linha">
            <label>Nome Completo</label>
            <asp:TextBox ID="textNome" Width="220px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Perfil de Acesso</label>
            <asp:DropDownList ID="comboPerfil" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Login</label>
            <asp:TextBox ID="textLogin" runat="server"></asp:TextBox>
        </span>
        <span id="campoSenha" runat="server" class="linha">
            <label>Senha</label>
            <asp:TextBox ID="textSenha" TextMode="Password" runat="server"></asp:TextBox>
        </span>
        <span id="campoConfirmacao" runat="server" class="linha">
            <label>Confirmação</label>
            <asp:TextBox ID="textConfirmacao" TextMode="Password" runat="server"></asp:TextBox>
        </span>
    </fieldset>
    <asp:RequiredFieldValidator ID="validaNomeCompleto" runat="server" Display="None"
        ControlToValidate="textNome" ErrorMessage="Informe o nome do Usuário."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNomeCompleto" runat="server"
        TargetControlID="validaNomeCompleto" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaLogin" runat="server" Display="None"
        ControlToValidate="textLogin" ErrorMessage="Informe o Login de acesso do Usuário."></asp:RequiredFieldValidator>
     
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaLogin" runat="server"
        TargetControlID="validaLogin" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
     
    <asp:RequiredFieldValidator ID="validaSenha" runat="server" Display="None"
        ControlToValidate="textSenha" ErrorMessage="Informe a Senha de acesso do Usuário."></asp:RequiredFieldValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxValidaSenha" runat="server"
        TargetControlID="validaSenha" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaConfirmacao" runat="server" Display="None"
        ControlToValidate="textConfirmacao" ErrorMessage="Confirme a Senha de acesso do Usuário."></asp:RequiredFieldValidator>
 
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaConfirmacao" runat="server"
        TargetControlID="validaConfirmacao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>      
  
    <asp:CompareValidator ID="comparaSenha" runat="server" Display="None"
        ControlToValidate="textConfirmacao" ControlToCompare="textSenha" Operator="Equal" 
        ErrorMessage="Confirmação da Senha está incorreta."></asp:CompareValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxComparaSenha" runat="server"
        TargetControlID="comparaSenha" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
    <asp:CustomValidator ID="validaComboPerfil" runat="server" ClientValidationFunction="validaCombo"
        ControlToValidate="comboPerfil" ErrorMessage="Escolha um Perfil de Acesso." Display="None"></asp:CustomValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxValidacomboPerfil" runat="server"
        TargetControlID="validacomboPerfil" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
</asp:Content>