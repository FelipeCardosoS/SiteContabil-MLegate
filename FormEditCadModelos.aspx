<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" CodeFile="FormEditCadModelos.aspx.cs" Inherits="FormEditCadModelos" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados do Modelo</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_MODELO" runat="server" />
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="180px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Tipo</label>
            <asp:RadioButtonList ID="radioTipo" CssClass="campoRadio" runat="server">
                <asp:ListItem Text="Contas à pagar" Value="CP"></asp:ListItem>
                <asp:ListItem Text="Contas à receber" Value="CR"></asp:ListItem>
                <asp:ListItem Text="Contabilidade" Value="C"></asp:ListItem>
            </asp:RadioButtonList>
        </span>
        <span class="linha">
            <label>Padrão Default</label>
            <asp:CheckBox ID="checkDefault" runat="server" />
        </span>
        <span class="linha">
            <label>Observações</label>
            <asp:TextBox ID="textObservacao" TextMode="MultiLine" Width="400px" Height="100px" runat="server"></asp:TextBox>
        </span>
        <p></p>
    </fieldset>

    <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
        ControlToValidate="textNome" ErrorMessage="Informe o Nome do Modelo."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
        TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaTipo" runat="server" Display="None"
        ControlToValidate="textNome" ErrorMessage="Escolha o tipo do Modelo."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaTipo" runat="server"
        TargetControlID="validaTipo" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
   
</asp:Content>