<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadFuncoesCliente.aspx.cs" Inherits="FormEditCadFuncoesCliente" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados da Função</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_FUNCAO" runat="server" />
        <span class="linha">
            <label>Descrição</label>
            <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
        </span>
    </fieldset>

    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição da Função."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
</asp:Content>
