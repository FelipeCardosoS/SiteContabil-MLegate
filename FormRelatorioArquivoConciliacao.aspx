<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioArquivoConciliacao.aspx.cs" Inherits="FormRelatorioArquivoConciliacao" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" runat="Server">
    <br />
    <label style="font-weight: bold; margin-left: 10px;">Informe um período válido para realizar a extração de dados.</label>
    <br />
    <br />
    <br />
    <label style="font-weight: bold; margin-left: 10px; color: blue;">De: </label>
    <input type="date" id="dateDe" min="2000-01-01" max="9999-12-31" required="true" style="font-weight: bold; color: red; width: 212px;" runat="server" />
    <label style="font-weight: bold; margin-left: 10px; color: blue;">Até: </label>
    <input type="date" id="dateAte" min="2000-01-01" max="9999-12-31" required="true" style="font-weight: bold; color: red; width: 212px;" runat="server" />
    
    <div class="botoes">
        <asp:Button ID="btnGerar" OnClick="btnGerar_Click" Text="Gerar Arquivo CSV" runat="server" />
    </div>
</asp:Content>