<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" AutoEventWireup="true" StylesheetTheme="principal" CodeFile="FormEditCadClassificacaoConta.aspx.cs" Inherits="FormEditCadClassificacaoConta" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Classificação de Conta</div>
    <fieldset>
        <span class="linha">
            <label>Código:</label>
            <asp:TextBox ID="codigoText" Width="100px" runat="server" />

            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                ControlToValidate="codigoText" ErrorMessage="Informe o Nome." />
            
            <toolKit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                TargetControlID="RequiredFieldValidator1" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha">
            <label>Descrição:</label>
            <asp:TextBox ID="descricaoText" Width="200px" runat="server" />
            
            <asp:RequiredFieldValidator ID="descricaoValidator" runat="server" Display="None"
                ControlToValidate="descricaoText" ErrorMessage="Informe o Nome." />
            
            <toolKit:ValidatorCalloutExtender ID="nomeAjaxValidator" runat="server"
                TargetControlID="descricaoValidator" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
    </fieldset>
</asp:Content>

