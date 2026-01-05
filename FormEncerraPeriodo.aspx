<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEncerraPeriodo.aspx.cs" Inherits="FormEncerraPeriodo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <span class="linha">
            <label>Início</label>
            <asp:TextBox ID="inicioTextBox" runat="server" Width="60px" />
            <label>Término</label>
            <asp:TextBox ID="terminoTextBox" runat="server" Width="60px" />
        </span>
        <span class="linha">
            <label>Conta/Apuração</label>
            <asp:DropDownList ID="contaApuracaoDropDownList" Width="280px" runat="server" />
        </span>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <span class="linha">
                    <label>Histórico</label>
                    <asp:TextBox ID="txtHistorico" Text="Encerramento do Período" Width="700px" runat="server"></asp:TextBox>
                </span>
                <span class="linha">
                    <label>Job</label>
                    <asp:DropDownList ID="jobDropDownList" AutoPostBack="true" OnSelectedIndexChanged="jobDropDownList_SelectedIndexChanged" 
                        Width="280px" runat="server" />
                </span>
                <span class="linha">
                    <label>Linha Negócio</label>
                    <asp:DropDownList ID="linhaNegocioDropDownList" Enabled="false" Width="280px" runat="server" />
                </span>
                <span class="linha">
                    <label>Divisão</label>
                    <asp:DropDownList ID="divisaoDropDownList" Enabled="false" Width="280px" runat="server" />
                </span>
                <span class="linha">
                    <label>Cliente</label>
                    <asp:DropDownList ID="clienteDropDownList" Enabled="false" Width="280px" runat="server" />
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <div class="botoes">
        <asp:Button ID="encerrarButton" runat="server" 
            OnClick="encerrarButton_Click" Text="Ok" />
    </div>
</asp:Content>

