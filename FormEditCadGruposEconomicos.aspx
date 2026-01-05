<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" AutoEventWireup="true" CodeFile="FormEditCadGruposEconomicos.aspx.cs" Inherits="FormEditCadGruposEconomicos" StylesheetTheme="principal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="Server">
    <div class="legendaForm">Informe os dados do Grupo Econômico</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_GRUPO_ECONOMICO" runat="server" />
        <span class="linha">
            <label>Descrição</label>
            <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
        </span>
    </fieldset>
    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição da Conta."></asp:RequiredFieldValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif"
        HighlightCssClass="validacaoErro">
    </toolKit:ValidatorCalloutExtender>
</asp:Content>



