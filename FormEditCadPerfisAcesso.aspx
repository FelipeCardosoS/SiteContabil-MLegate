<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadPerfisAcesso.aspx.cs" Inherits="FormEditCadPerfisAcesso" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Informe os dados do Perfil e escolha os módulos que ele terá permissão</div>
    <table>
        <tbody>
            <tr>
                <td valign="top">
                    <fieldset style="width:450px">
                        <asp:HiddenField ID="H_COD_PERFIL" runat="server" />
                        <span class="linha">
                            <label>Código</label>
                            <asp:TextBox ID="textCodigo" Visible="false" runat="server"></asp:TextBox>
                            <asp:TextBox ID="textCodigoPerfil" runat="server"></asp:TextBox>
                        </span>
                        <span class="linha">
                            <label>Descrição</label>
                            <asp:TextBox ID="textDescricao" Width="200px" runat="server"></asp:TextBox>
                        </span>
                        <span class="linha">
                            <label>Obriga Modelo</label>
                            <asp:DropDownList ID="comboModelo" runat="server">
                                <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </fieldset>
                </td>
                <td>
                    <asp:TreeView ID="treeView" ShowCheckBoxes="All" 
                        ShowExpandCollapse="true" NodeStyle-ForeColor="#000066" runat="server">
                    </asp:TreeView>
                </td>
            </tr>
        </tbody>
    </table>

    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe o nome do Usuário."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
    
    <toolKit:FilteredTextBoxExtender ID="filtraCodigo" runat="server"
        FilterType="UppercaseLetters"  TargetControlID="textCodigo"></toolKit:FilteredTextBoxExtender>
</asp:Content>