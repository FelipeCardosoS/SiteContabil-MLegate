<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormDiarioDetalhadoRelForm.aspx.cs" Inherits="FormDiarioDetalhadoRelForm" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label>Período Início:</label>
            <asp:TextBox ID="textPeriodoInicio" runat="server" Width="70px" />
            <label>Período Término:</label>
            <asp:TextBox ID="textPeriodoTermino" runat="server" Width="70px" />
        </span>
        <span class="linha">
            <label>Detalhamento:</label>
            <asp:DropDownList ID="detalhamentoDropDownList" runat="server" Width="200px">
                <asp:ListItem Text="Job" Value="JOB" />
                <asp:ListItem Text="Linha de Negócio" Value="LINHA_NEGOCIO" />
                <asp:ListItem Text="Divisão" Value="DIVISAO" />
                <asp:ListItem Text="Cliente" Value="CLIENTE" />
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Pag. Inicio:</label>
            <asp:TextBox ID="textPaginaInicio" runat="server" Width="50px" />
        </span>
        <span class="linha">
            <label>Total Pag:
        </label>
            &nbsp;<asp:TextBox ID="textTotalPaginas" runat="server" Width="50px" />
        </span>
        
    </fieldset>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="400px" Width="1000px" onreportrefresh="ReportViewer1_ReportRefresh">
        <LocalReport ReportPath="Relatorios\diarioDetalhadoReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="RelatoriosDAO_diarioDetalhado" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="executa" 
        TypeName="RelatoriosDAOTableAdapters.diarioDetalhadoTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="textPeriodoInicio" DefaultValue="01/01/1900" 
                Name="periodoInicio" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="textPeriodoTermino" DefaultValue="01/01/1900" 
                Name="periodoTermino" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="textPaginaInicio" DefaultValue="01" 
                Name="inicioPagina" PropertyName="Text" Type="Int32" />
            <asp:ControlParameter ControlID="textTotalPaginas" DefaultValue="500" 
                Name="totalPaginas" PropertyName="Text" Type="Int32" />
            <asp:ControlParameter ControlID="detalhamentoDropDownList" 
                DefaultValue="&quot;JOB&quot;" Name="detalhamento" PropertyName="SelectedValue" 
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

