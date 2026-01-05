<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorios.aspx.cs" Inherits="FormRelatorios" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <fieldset>
        <label></label>
        <asp:ListBox ID="listRelatorios" Width="500px" Height="180px" runat="server">
        </asp:ListBox>
    </fieldset>
    <div class="botoes">
        <asp:Button ID="botaoVaiRelatorio" OnClick="botaoVaiRelatorio_Click" runat="server" Text="Ir" />
    </div>
</asp:Content>