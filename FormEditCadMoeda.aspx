<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadMoeda.aspx.cs" Inherits="FormEditCadMoeda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados da Moeda.</div>
    <fieldset>
        <span class="linha">
            <label>Descrição</label>
            <asp:TextBox ID="txtDescricao" Width="220px" runat="server" />
        </span>
    </fieldset>
</asp:Content>

