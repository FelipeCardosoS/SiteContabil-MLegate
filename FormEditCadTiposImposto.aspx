<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadTiposImposto.aspx.cs" Inherits="FormEditCadTiposImposto" %>

<asp:Content ID="areaHead" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados do Tipo de Imposto.</div>
    <fieldset>
        <span class="linha">
            <label>Tipo</label>
            <asp:TextBox ID="textTipoImposto" Width="80px" runat="server" />
        </span>
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="220px" runat="server" />
        </span>
    </fieldset>
    <div class="legendaForm">Lista das aliquotas desse tipo de imposto.</div>
    <div id="bloco_aliquotas">
        <fieldset style="position:relative;width:300px;top:5px;left:5px;float:left;">
            <span class="linha">
                <label>Cumulativa</label>
                <asp:CheckBox ID="checkCumulativo" runat="server" />
            </span>
            <span class="linha">
                <label>Aliquota</label>
                <asp:TextBox ID="textAliquota" Width="60px" runat="server" />
            </span>
            <span class="linha">
                <label>Aliquota Retenção</label>
                <asp:TextBox ID="textAliquotaRetencao" Width="60px" runat="server" />
            </span>
            <div class="botoes">
                <asp:Button ID="botaoCancelar" OnClick="botaoCancelar_Click" runat="server" Text="Cancelar" />
                <asp:Button ID="botaoInserir" OnClick="botaoInserir_Click" runat="server" Text="Inserir" />
                
            </div>
        </fieldset>
        <table id="tabelaGrid" style="position:relative; width:350px;top:5px;left:20px;float:left;" cellpadding="0" cellspacing="0">
            <tr class="titulo">
                <td>Cumulativo</td>
                <td>Aliquota</td>
                <td>Aliquota Retenção</td>
                <td></td>
            </tr>
            <asp:Repeater ID="repeaterDados" runat="server">
                <ItemTemplate>
                    <tr class="linha">
                        <td><asp:CheckBox ID="checkCc" runat="server" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "cumulativo") %>' /></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "aliquota") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "aliquotaRetencao")%></td>
                        <td class="funcoes">
                            <asp:LinkButton ID="linkDeletar" tipoImposto='<%# DataBinder.Eval(Container.DataItem, "tipoImposto") %>' 
                                cumulativo='<%# DataBinder.Eval(Container.DataItem, "cumulativo") %>' OnClick="botaoDeletar_Click" Text="Remover" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

    </div>
</asp:Content>

