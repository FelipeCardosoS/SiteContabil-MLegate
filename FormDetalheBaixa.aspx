<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" CodeFile="FormDetalheBaixa.aspx.cs" Inherits="FormDetalheBaixa" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <asp:Label ID="labelBaixa" CssClass="boxLote" runat="server" Text=""></asp:Label>
    <asp:Literal ID="lGrid" runat="server"></asp:Literal>
</asp:Content>
