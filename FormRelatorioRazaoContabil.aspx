<%@ Page Title="" Language="C#" MasterPageFile="~/MasterRelatorio.master" StylesheetTheme="relatorio" AutoEventWireup="true" CodeFile="FormRelatorioRazaoContabil.aspx.cs" Inherits="FormRelatorioRazaoContabil" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="200px" 
    Width="100%" Font-Names="Verdana" Font-Size="8pt" ShowToolBar="False">
        <LocalReport ReportPath="Relatorios\HonorariosReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="RelatoriosDAO_RazaoContabil" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="executar" 
        TypeName="RelatoriosDAOTableAdapters.RazaoContabilTableAdapter">
        <SelectParameters>
            <asp:QueryStringParameter Name="contaDe" QueryStringField="contaDe" 
                Type="String" />
            <asp:QueryStringParameter Name="contaAte" QueryStringField="contaAte" 
                Type="String" />
            <asp:QueryStringParameter Name="periodoAnterior" 
                QueryStringField="periodoAnterior" Type="DateTime" />
            <asp:QueryStringParameter Name="periodoDe" QueryStringField="periodoDe" 
                Type="DateTime" />
            <asp:QueryStringParameter Name="periodoAte" QueryStringField="periodoAte" 
                Type="DateTime" />
            <asp:Parameter DefaultValue="0" Name="divisaoDe" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="divisaoAte" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="linhaNegocioDe" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="linhaNegocioAte" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="clienteDe" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="clienteAte" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="jobDe" Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="jobAte" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

