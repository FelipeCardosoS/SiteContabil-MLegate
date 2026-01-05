<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridDelImportacao.aspx.cs" Inherits="FormGridDelImportacao" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
  
    <label><span>Data</span>
        <asp:TextBox ID="textData" Width="80px" runat="server" />
    </label>
    
</asp:Content>
<asp:Content ID="areaGrid" ContentPlaceHolderID="areaGrid" Runat="Server">
    
    
     <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            
            <td width="10%">Codigo</td>
            <td width="45%">Nome</td>
            <td width="30%">Data</td>
            <td width="15%">Valor Total</td>
            <td></td>
            <td></td>
        </tr>
        <asp:Repeater ID="GVimportacao" OnItemDataBound="repeaterDados_ItemDataBound"  runat="server">
            <ItemTemplate>
                <tr class="linha">
                    
                    <td class="line" width="1%"><%# Eval("COD_PLANILHA")%></td>    
                    <td class="line" width="45%"><%# Eval("NOME_PLANILHA")%></td>    
                    <td class="line" width="30%"><%# Eval("DATA")%></td>    
                    <td class="line" width="15%"><%# Eval("Valor_Total")%></td>
                    <td class="line">
                        <asp:LinkButton ID="linkDeletar" CodPlanilha='<%# Eval("COD_PLANILHA")%>' OnClick="linkDeletar_Click" runat="server">Deletar</asp:LinkButton>
                    </td>
                    <td class="line" width="1%">
                        <asp:HyperLink ID="linkdetalhes" CodPlanilha='<%# Eval("COD_PLANILHA")%>'  runat="server">Detalhes</asp:HyperLink>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>



</asp:Content>

