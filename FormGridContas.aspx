<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridContas.aspx.cs" Inherits="FormGridContas" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <label><span>Grupo</span>
        <asp:DropDownList ID="comboGrupo" Width="150px" runat="server">
        </asp:DropDownList>
    </label>
    <label><span>Código</span>
        <asp:TextBox ID="textCodigo" Width="70px" runat="server"></asp:TextBox>
    </label>
    <label><span>Descrição</span>
        <asp:TextBox ID="textDescricao" Width="180px" runat="server"></asp:TextBox>
    </label>
    <label><span>D/C</span>
        <asp:DropDownList ID="comboDebCred" Width="50px" runat="server">
            <asp:ListItem Value="0" Text="ND"></asp:ListItem>
            <asp:ListItem Value="D" Text="D"></asp:ListItem>
            <asp:ListItem Value="C" Text="C"></asp:ListItem>
        </asp:DropDownList>
    </label>
    <label><span>Analítica</span>
        <asp:DropDownList ID="comboAnalitica" Width="60px" runat="server">
            <asp:ListItem Value="0" Text="ND"></asp:ListItem>
            <asp:ListItem Value="1" Text="Sim"></asp:ListItem>
            <asp:ListItem Value="0" Text="Não"></asp:ListItem>
        </asp:DropDownList>
    </label>
    <label><span>Tipo</span>
        <asp:DropDownList ID="comboTipo" Width="150px" runat="server">
        </asp:DropDownList>
    </label>
</asp:Content>
<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td></td>
            <td>Código</td>
            <td>Descrição</td>
            <td>Grupo</td>
            <td>Analit/Sint</td>
            <td>Tipo</td>
            <td></td>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="line" width="1%">
                        <input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_CONTA]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[COD_CONTA]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[DESCRICAO]") %></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[descricao_grupo_contabil]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[descricao_analitica_sintetica]")%></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[descricao_tipo_conta]")%></td>
                    <td class="funcoes line" width="1%">
                        <p><asp:HyperLink ID="linkAlterar" runat="server">Editar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <toolKit:ModalPopupExtender ID="popImportar" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" CancelControlID="closePopImportar" 
        TargetControlID="botaoImportar" BackgroundCssClass="pop_background" PopupControlID="blocoPopImportar">
    </toolKit:ModalPopupExtender>
    <asp:Panel ID="blocoPopImportar" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Importar Contas.</div>
            <asp:LinkButton ID="closePopImportar" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <fieldset>
                <label style="width:80px;">Planilha: </label>
                <asp:FileUpload ID="arquivo" runat="server" />
                
                <asp:RequiredFieldValidator ID="validaArquivo" ValidationGroup="popImportacao" runat="server" Display="None"
                    ControlToValidate="arquivo" ErrorMessage="Informe o arquivo para realizar a importação."></asp:RequiredFieldValidator>
                
                <toolKit:ValidatorCalloutExtender ID="ajaxValidaArquivo" runat="server"
                    TargetControlID="validaArquivo" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                    HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
                    
                <asp:Button ID="botaoIniciar" OnClick="botaoIniciar_Click" ValidationGroup="popImportacao" runat="server" Text="Iniciar" />
            </fieldset>
        </div>
    </asp:Panel>

</asp:Content>