<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridNaturezaOperacao.aspx.cs" Inherits="FormGridNaturezaOperacao" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label id="linhaNome"><span>Nome</span>
        <asp:TextBox ID="textNome" Width="200px" MaxLength="200" runat="server" />
    </label>

    <label id="linhaDescricao"><span>Descrição</span>
        <asp:TextBox ID="textDescricao" Width="275px" MaxLength="500" runat="server" />
    </label>
    
    <label id="linhaNaturezaOperacao"><span>Natureza da Operação (Prefeitura)</span>
        <asp:TextBox ID="textNaturezaOperacao" Width="180px" MaxLength="50" runat="server" />
    </label>
    
    <label><span>Emitente</span>
        <asp:DropDownList ID="comboEmitente" Width="270px" runat="server"></asp:DropDownList>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome</td>
            <td>Descrição</td>
            <td>Natureza da Operação (Prefeitura)</td>
            <td>Emitente</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%"><input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_NATUREZA_OPERACAO]") %>' runat="server" /></td>
                    <td class="line"><asp:Label ID="nome" Text='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' runat="server" /></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NATUREZA_OPERACAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[EMITENTE]") %></td>
                    <td class="funcoes line" width="1%"><p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>