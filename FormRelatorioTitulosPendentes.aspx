<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioTitulosPendentes.aspx.cs" Inherits="FormRelatorioTitulosPendentes" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
    
<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label style="width:80px">Período:</label>
            <asp:TextBox ID="textPeriodo" runat="server" Width="70px" />
            <label>Vencido à mais de:</label>
            <asp:TextBox ID="textVencimento" runat="server" Width="50px" TextMode="Number" min="0"/>
            <label style="width:80px">Agrupador:</label>
            <asp:DropDownList ID="comboAgrupador" runat="server">
                <asp:ListItem Text="Vencimento" Value="VENCIMENTO" />
                <asp:ListItem Text="Terceiro" Value="TERCEIRO" />
                <asp:ListItem Text="Emitente" Value="EMITENTE" />
            </asp:DropDownList>
        </span>
        <asp:UpdatePanel ID="UpConta" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
            <Triggers>
				 <asp:AsyncPostBackTrigger ControlID="textContaDe" />
				 <asp:AsyncPostBackTrigger ControlID="textContaAte" />
			</Triggers>
            <ContentTemplate>
				<span class="linha">
					<label style="width: 80px">Conta de:</label>
					<asp:DropDownList ID="textContaDe" OnSelectedIndexChanged="textContaDe_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="280px" />
					<label style="width: 40px">até:</label>
					<asp:DropDownList ID="textContaAte" OnSelectedIndexChanged="textContaAte_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="280px" />
				</span>
            </ContentTemplate>
        </asp:UpdatePanel>
		<span class="linha">
			<asp:UpdatePanel ID="UpDivisao" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="textDivisaoDe" />
                </Triggers>
				<ContentTemplate>
					<label style="width: 80px">Divisão de:</label>
					<asp:DropDownList ID="textDivisaoDe" OnSelectedIndexChanged="selecionaAte" runat="server" Width="140px" />
					<label style="width: 40px">até:</label>
					<asp:DropDownList ID="textDivisaoAte" Enabled="false" runat="server" Width="140px" />
				</ContentTemplate>
			</asp:UpdatePanel>
			<asp:UpdatePanel ID="UpLinhaNegocio" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="textLinhaNegocioDe" />
				</Triggers>
				<ContentTemplate>
					<label style="width: 130px;">Linha Negócio de:</label>
					<asp:DropDownList ID="textLinhaNegocioDe" OnSelectedIndexChanged="selecionaAte" runat="server" Width="140px" />
					<label style="width: 40px;">até:</label>
					<asp:DropDownList ID="textLinhaNegocioAte" Enabled="false" runat="server" Width="140px" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</span>
        <span class="linha">
			<asp:UpdatePanel ID="UpCliente" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="textClienteDe" />
				</Triggers>
				<ContentTemplate>
					<label style="width: 80px">Cliente de:</label>
					<asp:DropDownList ID="textClienteDe" OnSelectedIndexChanged="selecionaAte" runat="server" Width="140px" />
					<label style="width: 40px">até:</label>
					<asp:DropDownList ID="textClienteAte" Enabled="false" runat="server" Width="140px" />
				</ContentTemplate>
			</asp:UpdatePanel>
			<asp:UpdatePanel ID="UpJob" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="textJobDe" />
				</Triggers>
				<ContentTemplate>
					<label style="width: 80px">Job de:</label>
					<asp:DropDownList ID="textJobDe" OnSelectedIndexChanged="selecionaAte" runat="server" Width="140px" />
					<label style="width: 40px">até:</label>
					<asp:DropDownList ID="textJobAte" Enabled="false" runat="server" Width="140px" />
				</ContentTemplate>
			</asp:UpdatePanel>
        </span>
        <span class="linha">
			<asp:UpdatePanel ID="UpTerceiro" ChildrenAsTriggers="false" UpdateMode="Conditional" RenderMode="Inline" runat="server">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="textTerceiroDe" />
				</Triggers>
				<ContentTemplate>
					<label style="width: 80px">Terceiro de:</label>
					<asp:DropDownList ID="textTerceiroDe" OnSelectedIndexChanged="selecionaAte" runat="server" Width="280px" />
					<label style="width: 40px;">até:</label>
					<asp:DropDownList ID="textTerceiroAte" Enabled="false" runat="server" Width="280px" />
				</ContentTemplate>
			</asp:UpdatePanel>
        </span>
    </fieldset>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" 
    Width="100%" Font-Names="Verdana" Font-Size="8pt" onreportrefresh="ReportViewer1_ReportRefresh">
        <LocalReport ReportPath="Relatorios\titulosPendentesReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="RelatoriosDAO_TitulosPendentes" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="executar" 
        TypeName="RelatoriosDAOTableAdapters.TitulosPendentesTableAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="textPeriodo" DefaultValue="01/01/1900" 
                Name="periodo" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="textVencimento" DefaultValue="0" 
                Name="diasVencimento" PropertyName="Text" Type="Int32" />
            <asp:ControlParameter ControlID="textContaDe" DefaultValue="" Name="contaDe" 
                PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="textContaAte" Name="contaAte" 
                PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="textDivisaoDe" DefaultValue="0" 
                Name="divisaoDe" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textDivisaoAte" DefaultValue="0" 
                Name="divisaoAte" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textLinhaNegocioDe" DefaultValue="0" 
                Name="linhaNegocioDe" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textLinhaNegocioAte" DefaultValue="0" 
                Name="linhaNegocioAte" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textClienteDe" DefaultValue="0" 
                Name="clienteDe" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textClienteAte" DefaultValue="0" 
                Name="clienteAte" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textJobDe" DefaultValue="0" Name="jobDe" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textJobAte" DefaultValue="0" Name="jobAte" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textTerceiroDe" DefaultValue="" 
                Name="terceiroDe" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="textTerceiroAte" Name="terceiroAte" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="comboAgrupador" Name="ordenacao" 
                PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

