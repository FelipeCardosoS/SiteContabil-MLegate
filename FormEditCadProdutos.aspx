<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadProdutos.aspx.cs" Inherits="FormEditCadProdutos" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados do produto.</div>
    <fieldset>
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="220px" runat="server" />
        </span>
        <span class="linha">
            <label>Gera Crédito</label>
            <asp:CheckBox ID="creditoCheckBox" runat="server" />
        </span>
        <span class="linha">
            <label>Unidades</label>
            <asp:DropDownList ID="comboUnidades" runat="server" Width="200px" />
        </span>
    </fieldset>
</asp:Content>

