<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGerarSpedContabil.aspx.cs" Inherits="FormGerarSpedContabil" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <style>
        .msgSuc, .msgError, .msgAlert
        {
            border: 1px solid #009900;
            background: #CBFFD5;
            padding: 10px;
            margin-bottom: 10px;
            width: 50%;    
        }
        
        .msgError 
        {
            border: 1px solid #990000;
            background: #FFB8B8;
        }
        
        .msgAlert
        {
            border: 1px solid #FFD600;
            background: #FCFFB8;
        }
    </style>
    <fieldset>
        <%--<legend>Upload do DRE </legend>
        <asp:Literal ID="msgAlertExist" runat="server">
            <center class="msgAlert">Já existe uma planilha(DRE) para está empresa, caso você faça o Upload novamente a nova planilha irá sobrescrever a antiga!</center>
        </asp:Literal>
        <center class="msgAlert">
            Baixe o modelo de DRE: <a href="Dre/Modelo.xls">DOWNLOAD</a>
        </center>
        <asp:Literal ID="ltUpload" runat="server" />
        <span class="linha">
            <label>Arquivo:</label>
            <asp:FileUpload ID="FileUpload" runat="server" />
        </span>
        <span class="linha">
            <label></label>
            <asp:Button ID="btnFile" CssClass="botaoLado" runat="server" 
            Text="Enviar Arquivo" onclick="btnFile_Click" />
        </span>--%>
        <legend>Informe os dados para gerar o Sped Contábil </legend>
        <span class="linha">
            <label>Início</label>
            <asp:TextBox ID="textInicio" runat="server" Width="80px" />
         </span>
         <span class="linha">
            <label>Término</label>
            <asp:TextBox ID="textTermino" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Demonstrações Anuais</label>
            <asp:RadioButton ID="DAnuais" GroupName="RadionDemonstracao" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Demonstrações Trimestrais</label>
            <asp:RadioButton ID="DTrime" GroupName="RadionDemonstracao" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Demonstrações Mensais</label>
            <asp:RadioButton ID="DMensal" GroupName="RadionDemonstracao" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Lucro Presumido</label>
            <asp:CheckBox ID="checkLucroPresumido" runat="server" />
        </span>
        <span class="linha">
            <label>Aberto por Centro de Custo</label>
            <asp:CheckBox ID="checkDetCentroCusto" runat="server" />
        </span>
        <span class="linha">
            <label></label>
            <asp:Button ID="botaoGerar" CssClass="botaoLado" runat="server" 
            Text="Gerar SpedContabil" onclick="botaoGerar_Click" />
        </span>
        <center>
            <asp:Literal ID="textoLiteral" runat="server" />
        </center>
    </fieldset>
</asp:Content>

