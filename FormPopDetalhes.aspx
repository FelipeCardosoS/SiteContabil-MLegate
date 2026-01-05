<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="popup" CodeFile="FormPopDetalhes.aspx.cs" Inherits="FormPopDetalhes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" width="1500px" runat="server">
    <div>
    <table  class="grid_dados" width="1500px" cellpadding="0" cellspacing="">
        <tr>
            <td class="grid_area_titulo">
                <table id="tabelaGrid"  width="1500px" cellpadding="0" cellspacing="0">
                    <tr class="titulo">
                        <td>Nunero do Contrato</td> 
                        <td>Cliente</td>
                        <td>Fantasia</td>
                        <td>IE/RG</td>
                        <td>Valor Total</td>
                        <td>Data</td>
                        <td>Status</td>
                    </tr>
            </td>
        </tr>
            <tr>
                <td>
                    <div class="grid_area_dados">
                            <asp:Repeater  ID="GVdetalhes" runat="server">
                                <ItemTemplate>
                                    <tr class="linha">
                                    <td><span><%# Eval("numero_contrato") %></span></td>  
                                    <td><span><%# Eval("cliente") %></span></td>
                                    <td><span><%# Eval("nome_fantasia") %></span></td>
                                    <td><span><%# Eval("ie_rg") %></span></td>
                                    <td><span><%# Eval("valor_total") %></span></td>
                                    <td><span><%# Eval("data") %></spansd></td>
                                    <td><span><%# Convert.ToBoolean(Eval("ok")) == false ? "Erro" : "Importado"%></span></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </td>
            </tr>
    </form>
</body>
</html>
