<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadPerfisAcesso2.aspx.cs" Inherits="FormEditCadPerfisAcesso2" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <div class="legendaForm">Escolha os módulos e tarefas compatíveis com o Perfil</div>
    <p></p>
    <asp:UpdatePanel ID="UpdatePanel1" RenderMode="Inline" runat="server">
        <ContentTemplate>
            <table style="width:798px;float:left;">
                <tbody>
                    <tr>
                        <td>
                            <div>
                                <asp:TreeView ID="treeView" ShowExpandCollapse="true" NodeStyle-ForeColor="#000066" SelectedNodeStyle-ForeColor="Red"
                                    OnSelectedNodeChanged="treeView_SelectedNodeChanged" runat="server">
                                </asp:TreeView>
                            </div>
                        </td>
                        <td valign="middle" align="center">
                            <asp:Panel ID="painelTarefas" Visible="false" runat="server">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <p>Disponíveis</p>
                                                <asp:ListBox ID="lisTarefasDisponiveis" SelectionMode="Multiple" Width="160px" Height="200px" runat="server"></asp:ListBox>
                                            </td>
                                            <td>
                                                <p><asp:Button ID="botaoColoca" OnClick="botaoColoca_Click" runat="server" Text=">" /></p>
                                                <p><asp:Button ID="botaoRetira" OnClick="botaoRetira_Click" runat="server" Text="<" /></p>
                                            </td>
                                            <td>
                                                <p>Permitidos</p>
                                                <asp:ListBox ID="listTarefasSelecionadas" SelectionMode="Multiple" Width="160px" Height="200px" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>