<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridEmpresas.aspx.cs" Inherits="FormGridEmpresas" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Tipo</span>
        <asp:DropDownList ID="comboTipo" runat="server">
            <asp:ListItem Text="Escolha" Value="0"></asp:ListItem>
            <asp:ListItem Text="Grupo" Value="GRUPO"></asp:ListItem>
            <asp:ListItem Text="Empresa" Value="EMPRESA"></asp:ListItem>
            <asp:ListItem Text="Fornecedor" Value="FORNECEDOR"></asp:ListItem>
            <asp:ListItem Text="Cliente" Value="CLIENTE"></asp:ListItem>
            <asp:ListItem Text="Cliente/Fornecedor" Value="CLIENTE_FORNECEDOR"></asp:ListItem>
            <asp:ListItem Text="Emitente" Value="EMITENTE"></asp:ListItem>
        </asp:DropDownList>
    </label>
    
    <label><span>Física/Jurídica</span>
        <asp:DropDownList ID="comboFisicaJuridica" runat="server">
            <asp:ListItem Text="Escolha" Value="0"></asp:ListItem>
            <asp:ListItem Text="Física" Value="FISICA"></asp:ListItem>
            <asp:ListItem Text="Jurídica" Value="JURIDICA"></asp:ListItem>
        </asp:DropDownList>
    </label>


    <label>
        <span>Grupo Financeiro</span>
        <asp:DropDownList ID="comboGrupoFinanceiro" runat="server" AutoPostBack ="true">
            <%--            <asp:ListItem Text="Escolha" Value="0"></asp:ListItem>--%>
        </asp:DropDownList>
    </label>



    <label>
        <span>Grupo Econômico</span>
        <asp:DropDownList ID="comboGrupoEconomico" runat="server" AutoPostBack ="true">
            <%--            <asp:ListItem Text="Escolha" Value="0"></asp:ListItem>--%>
        </asp:DropDownList>
    </label>

    <label id="linhaNomeFantasia"><span>Nome Fantasia</span>
        <asp:TextBox ID="textNomeFantasia" Width="200px" runat="server"></asp:TextBox>
    </label>
    
    <label id="linhaNomeRazaoSocial"><span id="labelNomeRazaoSocial">Razão Social</span>
        <asp:TextBox ID="textNomeRazaoSocial" Width="200px" runat="server"></asp:TextBox>
    </label>
    <label id="linhaCnpjCpf"><span id="labelCnpjCpf"></span>
        <asp:TextBox ID="textCnpjCpf" Width="160px" runat="server"></asp:TextBox>
    </label>
</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Nome Fantasia</td>
            <td>Razão Social</td>
            <td>CNPJ/CPF</td>
            <td>Telefone</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <asp:HiddenField ID="hdFisicaJuridica" Value='<%# DataBinder.Eval(Container.DataItem, "[FISICA_JURIDICA]") %>' runat="server" />
                        <input type="checkbox" id="check" class="check" name="check" 
                            value='<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_FANTASIA]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL]") %></td>
                    <td class="line"><asp:Literal ID="lCnpjCpf" Text='<%# DataBinder.Eval(Container.DataItem, "[CNPJ_CPF]") %>' runat="server"></asp:Literal></td>
                    <td class="line"><asp:Literal ID="lTelefone" Text='<%# DataBinder.Eval(Container.DataItem, "[TELEFONE]") %>' runat="server"></asp:Literal></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>