<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormDiarioRelForm.aspx.cs" Inherits="FormDiarioRelForm" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label>Período Início:</label>
            <asp:TextBox ID="textPeriodoInicio" runat="server" Width="70px" />
            <label>Período Término:</label>
            <asp:TextBox ID="textPeriodoTermino" runat="server" Width="70px" />
        </span>
        <span class="linha">
            <label>Pag. Inicio:</label>
            <asp:TextBox ID="textPaginaInicio" runat="server" Width="50px" />
        </span>
        <span class="linha">
            <label>Total Pag:</label>
            <asp:TextBox ID="textTotalPaginas" runat="server" Width="50px" />
        </span>
    </fieldset>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="400px" Width="1000px" 
        onreportrefresh="ReportViewer1_ReportRefresh">
        <LocalReport ReportPath="Relatorios\diarioReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                    Name="RelatoriosDAO_diario" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        SelectMethod="executa" 
        TypeName="RelatoriosDAOTableAdapters.diarioTableAdapter" 
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="textPeriodoInicio" DefaultValue="01/01/1900" 
                Name="periodoInicio" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="textPeriodoTermino" DefaultValue="01/01/1900" 
                Name="periodoTermino" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="textPaginaInicio" DefaultValue="1" 
                Name="inicioPagina" PropertyName="Text" Type="Int32" />
            <asp:ControlParameter ControlID="textTotalPaginas" DefaultValue="1" 
                Name="totalPaginas" PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
</asp:Content>

