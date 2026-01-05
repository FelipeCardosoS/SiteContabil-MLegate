<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridLanctosContasPagar.aspx.cs" Inherits="FormGridLanctosContasPagar" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="areaHead" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <script type="text/javascript" src="Js/jquery_tools.js"></script>
    <label><span>Lote</span>
        <asp:TextBox ID="textLote" Width="60px" runat="server"></asp:TextBox>     
    </label>

    <label><span>Data Início</span>
        <asp:TextBox ID="textDataInicio" Width="80px" runat="server"></asp:TextBox>     
    </label>
    <label><span>Data Término</span>
        <asp:TextBox ID="textDataTermino" Width="80px" runat="server"></asp:TextBox>     
    </label>
    <label>
        <span>N° Documento</span>
        <asp:TextBox ID="textDocumento" Width="80px" runat="server"></asp:TextBox>
    </label>
    <label><span>Conta</span>
        <asp:DropDownList ID="comboConta" Width="220px" runat="server">
        </asp:DropDownList>     
    </label>
    <label><span>Job</span>
        <asp:DropDownList ID="comboJob" Width="220px" runat="server">
        </asp:DropDownList>     
    </label>
    <label><span>Terceiros</span>
        <asp:DropDownList ID="comboTerceiro" Width="220px" runat="server">
        </asp:DropDownList>     
    </label>
</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Lote</td>
            <td>Data</td>
            <td>N° Doc.</td>
            <td>Conta</td>
            <td>Job</td>
            <td>Terceiros</td>
            <td>Histórico</td>
            <td>Status</td>
            <td class="valor">Valor</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[LOTE]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[LOTE]") %></td>
                    <td class="line"><%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "[data]")).ToString("dd/MM/yyyy") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[numero_documento]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[desc_conta]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[desc_job]") %></td>
                    <td class="line">
                        <asp:HyperLink ID="linkTerceiros" runat="server" />
                        <div id="tipTerceiros" runat="server" class="tooltip">
                            <%# DataBinder.Eval(Container.DataItem, "[terceiros]") %>
                        </div>
                    </td>
                    <td class="line">
                        <asp:HyperLink ID="linkHistorico" runat="server" />
                        <div id="tip" runat="server" class="tooltip">
                            <%# DataBinder.Eval(Container.DataItem, "[historico]") %>
                        </div>
                    </td>
                    <td class="line">
                        <span style="font-size:10px;"><asp:Literal ID="literalStatusBaixa" runat="server" /></span>
                    </td>
                    <td class="valor line"><%# String.Format("{0:0,0.00}", Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "[Debitos]"))) %></td>
                    <td class="funcoes line" width="1%">
                        <asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>