<%@ Page Language="C#" MasterPageFile="~/MasterRelatorio.master" StylesheetTheme="relatorio" AutoEventWireup="true" CodeFile="FormRelatorioGeradosintetico.aspx.cs" Inherits="FormRelatorioGerado" %>
<%@ MasterType VirtualPath="~/MasterRelatorio.master" %>
<%@ PreviousPageType VirtualPath="~/FormRelatorioGerar.aspx"  %>
<asp:Content ID="Head1" ContentPlaceHolderID="head" runat="server">
    <style media="print" type="text/css" >
        table
        {
	        width:900px;
	        border-left:5px solid #fff;
	        border-right:5px solid #fff;
        }
        table tr td
        {
	        padding:4px 0 4px 0;
	        font-size:9px;
        }
        
        table tr td.valores
        {
        	padding:2px 0 5px 0;
	        text-align:right;
        }
        table tr td.valores a
        {
	        color:#000;
	        text-decoration:underline;
        }
        
        .nivel00
        {
	        background-color:Blue;
        }
        
        table tr.titulo td
        {
	        padding:2px 0 5px 0;
	        font-weight:bold;
        }
        table tr.linha td
        {
	        border-bottom:1px dashed #ecc;
        }

        table tr.nivel0 td
        {
	        color:Blue;
        }
        
        table tr.nivel00 td
        {
	        color:Blue;
        }
        table tr.nivel1 td
        {
	        color:Green;
        }
        table tr.nivel2 td
        {
	        color:Maroon;
        }
        table tr.nivel3 td
        {
	        color:Navy;
        }
        table tr.nivel4 td
        {
	        color:Orange;
        }

        table tr.nivel1 td a
        {
	        color:Green;
	        text-decoration:underline;
        }
        table tr.nivel2 td a
        {
	        color:Maroon;
	        text-decoration:underline;
        }
        table tr.nivel3 td a
        {
	        color:Navy;
	        text-decoration:underline;
        }
        table tr.nivel4 td a
        {
	        color:Orange;
	        text-decoration:underline;
        }

        table#dre tr td.valores
        {
	        width:190px;
        }
    </style>
</asp:Content>
<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">

    <asp:Literal ID="literalRelatorio" runat="server">
        
    </asp:Literal>
</asp:Content>
