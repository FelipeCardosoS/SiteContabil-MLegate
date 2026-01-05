<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGerarSped.aspx.cs" Inherits="FormGerarSped" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <legend>Informe os dados para gerar o Sped Pis/Cofins</legend>
        <span class="linha">
            <label>Início</label>
            <asp:TextBox ID="textInicio" runat="server" Width="80px" />
         </span>
         <span class="linha">
            <label>Término</label>
            <asp:TextBox ID="textTermino" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Lucro Real</label>
            <asp:CheckBox ID="checkLucroReal" runat="server" />
        </span>
        <span class="linha" style="padding-bottom:30px;">
            <label>Indicador da Composição da Receita</label>
            <table>
                <tr>
                    <td><asp:RadioButton ID="checkDocumentoFiscal" runat="server" GroupName="1" /></td>
                    <td><span>Documento Fiscal</span></td>
                </tr>
                <tr>
                    <td><asp:RadioButton ID="checkCliente" runat="server" GroupName="1"  /></td>
                    <td><span>Cliente</span></td>
                </tr>
            </table>
        </span>
        <span class="linha">
            <label></label>
            <asp:Button ID="botaoGerar" OnClick="botaoGerar_Click" CssClass="botaoLado" runat="server" Text="Gerar" />
        </span>
        <center>
            <asp:Literal ID="textoLiteral" runat="server" />
        </center>
    </fieldset>
</asp:Content>

