<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadGruposContabeis.aspx.cs" Inherits="FormEditCadGruposContabeis" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados da Grupo Contábil</div>
    <fieldset>
        <asp:HiddenField ID="H_COD_GRUPO_CONTABIL" runat="server" />
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Regra Exibição</label>
            <asp:DropDownList ID="comboRegraExibicao" runat="server">
                <asp:ListItem Text="Escolha" Value = "" />
                <asp:ListItem Text="Balanço" Value = "B" />
                <asp:ListItem Text="Demonstração de Resultado" Value = "R" />
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Conta Totalizar</label>
            <asp:TextBox ID="textConta" runat="server"></asp:TextBox>
        </span>
    </fieldset>

    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição da Conta."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
</asp:Content>