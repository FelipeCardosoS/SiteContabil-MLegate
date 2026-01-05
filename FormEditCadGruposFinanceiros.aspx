<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadGruposFinanceiros.aspx.cs" Inherits="FormEditCadGruposFinanceiros" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados da Grupo Financeiro</div>
    <fieldset>
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="180px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
        </span>
    </fieldset>

    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição do Grupo Financeiro."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
        ControlToValidate="textNome" ErrorMessage="Informe a Nome do Grupo Financeiro."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
        TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
</asp:Content>