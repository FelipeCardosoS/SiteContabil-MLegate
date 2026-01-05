<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioEmissaoNF.aspx.cs" Inherits="FormRelatorioEmissaoNF" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <div style="height: 475px">
        <rsweb:ReportViewer ID="rptEmissaoNF" runat="server" Font-Names="Verdana" Font-Size="8pt"
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" Height="100%" SizeToReportContent="True">
            <LocalReport ReportPath="Relatorios\EmissaoNF.rdlc" EnableHyperlinks="True">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF" Name="dsFATURAMENTO_NF" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_RETENCOES" Name="dsFATURAMENTO_NF_RETENCOES" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_SERVICOS_JOBS" Name="dsFATURAMENTO_NF_SERVICOS_JOBS" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_DATA_VENCIMENTO" Name="dsFATURAMENTO_NF_DATA_VENCIMENTO" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_NARRATIVAS" Name="dsFATURAMENTO_NF_NARRATIVAS" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOS" Name="dsFATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOS" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceFATURAMENTO_NF_ESPACAMENTO" Name="dsFATURAMENTO_NF_ESPACAMENTO" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NFTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_RETENCOES" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_RETENCOESTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_SERVICOS_JOBS" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_SERVICOS_JOBSTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_DATA_VENCIMENTO" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_DATA_VENCIMENTOTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_NARRATIVAS" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_NARRATIVASTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOS" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="Impostos" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_SERVICOS_JOBS_IMPOSTOSTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataSourceFATURAMENTO_NF_ESPACAMENTO" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="Espacamento" TypeName="RelatoriosDAOTableAdapters.FATURAMENTO_NF_ESPACAMENTOTableAdapter"></asp:ObjectDataSource>
    </div>
</asp:Content>