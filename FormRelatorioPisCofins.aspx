<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioPisCofins.aspx.cs" Inherits="FormRelatorioPisCofins" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>

<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label>Código da Conta: </label>
            <asp:DropDownList ID="ddlCodigoConta" runat="server">
            </asp:DropDownList>
            <label>Periodo de: </label>
            <asp:TextBox ID="txtPeriodoDe" runat="server" Width="70px"></asp:TextBox>
            <label>Periodo até: </label>
            <asp:TextBox ID="txtPeriodoAte" runat="server" Width="70px"></asp:TextBox>
        </span>
    </fieldset>

    <div style="height: 475px">
        <rsweb:ReportViewer ID="rptPisCofins" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1000px" 
            Height="475px" onreportrefresh="rptPisCofins_ReportRefresh1">
            <LocalReport ReportPath="Relatorios\PisCofins.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="DataSetPisCofins" Name="PisCofins" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="DataSetPisCofins" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="executar" 
            TypeName="RelatoriosDAOTableAdapters.PisCofinsTableAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlCodigoConta" DefaultValue="0" 
                    Name="COD_CONTA" PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="txtPeriodoDe" DefaultValue="01/01/1900" 
                    Name="De" PropertyName="Text" Type="DateTime" />
                <asp:ControlParameter ControlID="txtPeriodoAte" DefaultValue="01/01/1900" 
                    Name="Ate" PropertyName="Text" Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>