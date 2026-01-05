<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditSenhaUsuarios.aspx.cs" Inherits="FormEditSenhaUsuarios" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm" runat="server" id="legendaSenha">Informe a Atual e a Nova senha do Usuário</div>
     <fieldset>
        <asp:HiddenField ID="H_COD_USUARIO" runat="server" />
        <span class="linha" id="fieldAtual" runat="server">
            <label>Atual</label>
            <asp:TextBox ID="textAtual" TextMode="Password" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Senha</label>
            <asp:TextBox ID="textSenha" TextMode="Password" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Confirmação</label>
            <asp:TextBox ID="textConfirmacao" TextMode="Password" runat="server"></asp:TextBox>
        </span>
    </fieldset>
    
        
    <asp:RequiredFieldValidator ID="validaAtual" runat="server" Display="None"
        ControlToValidate="textAtual" ErrorMessage="Informe a senha Atual do Usuário."></asp:RequiredFieldValidator>
     
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaAtual" runat="server"
        TargetControlID="validaAtual" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
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
</asp:Content>