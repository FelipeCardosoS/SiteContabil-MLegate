<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadRetencoes.aspx.cs" Inherits="FormEditCadRetencoes" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados da Retenção</div>
    <fieldset>
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="250px" MaxLength="200" runat="server" ToolTip="Exibição para controle interno do usuário." />
            
            <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
                ControlToValidate="textNome" ErrorMessage="Informe o Nome da Retenção." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
                TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" Width="225px" />
        </span>
        <span class="linha">
            <label>Alíquota</label>
            <asp:TextBox ID="textAliquota" Width="100px" CssClass="Monetaria" MaxLength="18" runat="server" 
                ToolTip="O valor digitado, será automaticamente arredondado para 4 casas decimais pelo sistema." />
            
            <asp:RequiredFieldValidator ID="validaAliquota" runat="server" Display="None"
                ControlToValidate="textAliquota" ErrorMessage="Informe a Alíquota da Retenção." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaAliquota" runat="server"
                TargetControlID="validaAliquota" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" Width="240px" />
        </span>
        <span class="linha">
            <label>Modo de Apresentação</label>
            <asp:TextBox ID="textApresentacao" Width="200px" MaxLength="200" runat="server" ToolTip="Será exibido na Nota Fiscal." />
            
            <asp:RequiredFieldValidator ID="validaApresentacao" runat="server" Display="None"
                ControlToValidate="textApresentacao" ErrorMessage="Informe o Modo de Apresentação que será exibido na Nota Fiscal." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaApresentacao" runat="server"
                TargetControlID="validaApresentacao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" Width="250px" />
        </span>

        <span class="linha">
            <label>Retenção</label>
            <asp:DropDownList ID="ComboRetencao" Width="288px" AutoPostBack="false" CssClass="Visivel cboRetencao" runat="server"></asp:DropDownList>     
        </span>

        <div class="legendaForm margin_top margin_bottom">Emitentes</div>
        <table class="margin_top">
            <thead></thead>
            <tbody>
                <asp:Repeater ID="repeaterDados" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><input type="checkbox" id="check" class="margin_left" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %>' runat="server" /></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL]") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </fieldset>
</asp:Content>