<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master"  StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioBaseImposto.aspx.cs" Inherits="FormBaseImpostoRelForm" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label style="width:auto;">De:</label>
            <asp:TextBox ID="txtDe" runat="server" Width="70px" />
            <label style="width:40px;">Até:</label>
            <asp:TextBox ID="txtAte" runat="server" Width="70px" />
        </span>
    </fieldset>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="972px" 
        onreportrefresh="ReportViewer1_ReportRefresh">
        <LocalReport ReportPath="Relatorios\BaseImposto.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="baseCalculoImposto" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="RelatoriosDAO.baseCalculoImpostoTableAdapter">
    </asp:ObjectDataSource>
</asp:Content>