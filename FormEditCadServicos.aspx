<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadServicos.aspx.cs" Inherits="FormEditCadServicos" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados do Serviço</div>
    <fieldset>
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="300px" MaxLength="200" runat="server" ToolTip="Será exibido na Nota Fiscal." />
            
            <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
                ControlToValidate="textNome" ErrorMessage="Informe o Nome do Serviço." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
                TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha">
            <label>Código de Serviço (Prefeitura)</label>
            <asp:TextBox ID="textCodServicoPrefeitura" Width="160px" MaxLength="50" runat="server" ToolTip="Exibição para controle interno do usuário." />
            
            <asp:RequiredFieldValidator ID="validaCodServicoPrefeitura" runat="server" Display="None"
                ControlToValidate="textCodServicoPrefeitura" ErrorMessage="Informe o Código de Serviço (Prefeitura)." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaCodServicoPrefeitura" runat="server"
                TargetControlID="validaCodServicoPrefeitura" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha">
            <label>% Aproximada dos Impostos Sobre Vendas</label>
            <asp:TextBox ID="textImpostos" Width="160px" CssClass="Monetaria" runat="server" MaxLength="18" 
                ToolTip="O valor digitado, será automaticamente arredondado para 4 casas decimais pelo sistema. E será utilizado na Nota Fiscal, para calcular o Valor Aproximado dos Tributos." />
            
            <asp:RequiredFieldValidator ID="validaImpostos" runat="server" Display="None"
                ControlToValidate="textImpostos" ErrorMessage="Informe a % Aproximada dos Impostos Sobre Vendas da Empresa."></asp:RequiredFieldValidator>
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaImpostos" runat="server"
                TargetControlID="validaImpostos" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif"
                HighlightCssClass="validacaoErro" Width="425px" />
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