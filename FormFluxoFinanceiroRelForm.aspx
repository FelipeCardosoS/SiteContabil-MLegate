<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormFluxoFinanceiroRelForm.aspx.cs" Inherits="FormFluxoFinanceiroRelForm" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="400px" Width="1000px">
        <LocalReport ReportPath="Relatorios\fluxoFinanceiroReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="RelatoriosDAO_SaldoFluxoFinanceiro" />
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                    Name="RelatoriosDAO_FluxoFinanceiro" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
    TypeName="RelatoriosDAOTableAdapters.FluxoFinanceiroTableAdapter">
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
    TypeName="RelatoriosDAOTableAdapters.SaldoFluxoFinanceiroTableAdapter">
</asp:ObjectDataSource>
</asp:Content>

