<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadCotacao.aspx.cs" Inherits="FormEditCadCotacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="areaForm" Runat="Server">
    <div style="height: 200px;">
        <div class="legendaForm">Cadastro por Importação</div>
        <fieldset>
            <span class="linha">
                <label>Moeda </label>
                <asp:DropDownList ID="comboMoedaBC" runat="server"></asp:DropDownList>
            </span>
            <span class="linha">
                <label>Arquivo </label>
                <asp:FileUpload ID="fileBC" runat="server" />
            </span>
            <span class="linha">
                <label></label>
                ( Acesse o <a href="http://www4.bcb.gov.br/pec/taxas/port/ptaxnpesq.asp?id=txcotacao" target="_blank">site</a> do Banco Central para gerar o arquivo! )
            </span>
        </fieldset>
        <div id="_areaBotoesBC" class="botoes">
            <asp:Button ID="btnSubmit" runat="server" Text="Importar Documento" OnClick="btnSubmit_Click" />
        </div>
    </div>
    <div class="legendaForm">Cadastro por Dia/Cotação</div>
    <fieldset>
        <span class="linha">
            <label>Moeda </label>
            <asp:DropDownList ID="comboMoeda" runat="server"></asp:DropDownList>
        </span>
        <span class="linha">
            <label>Valor </label>
            <asp:TextBox ID="txtValor" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Data </label>
            <asp:TextBox ID="dtData" runat="server"></asp:TextBox>
        </span>
    </fieldset>
</asp:Content>

