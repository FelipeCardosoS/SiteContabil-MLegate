<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadLinhasNegocio.aspx.cs" Inherits="FormEditCadLinhasNegocio" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados da Linha de Negócio</div>
    <fieldset>
        <span class="linha">
            <label>Descrição:</label>
            <asp:TextBox ID="nomeTextBox" Width="200px" runat="server" />
            
            <asp:RequiredFieldValidator ID="nomeValidator" runat="server" Display="None"
                ControlToValidate="nomeTextBox" ErrorMessage="Informe o Nome." />
            
            <toolKit:ValidatorCalloutExtender ID="nomeAjaxValidator" runat="server"
                TargetControlID="nomeValidator" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
    </fieldset>
</asp:Content>

