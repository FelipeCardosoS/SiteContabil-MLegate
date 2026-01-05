<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadUnidades.aspx.cs" Inherits="FormEditCadUnidades" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados da Unidade.</div>
    <fieldset>
        <span class="linha">
            <label>Sigla</label>
            <asp:TextBox ID="textSigla" Width="80px" runat="server" />
        </span>
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="220px" runat="server" />
        </span>
    </fieldset>
</asp:Content>

