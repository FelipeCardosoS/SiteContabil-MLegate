<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadNaturezaOperacao.aspx.cs" Inherits="FormEditCadNaturezaOperacao" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" Runat="Server">
    <div class="legendaForm">Informe os dados da Natureza da Operação (será exibido no cabeçalho da Nota Fiscal)</div>
    <fieldset>
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="400px" MaxLength="200" runat="server" ToolTip="Exibição para controle interno do usuário." />
            
            <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
                ControlToValidate="textNome" ErrorMessage="Informe o Nome da Natureza da Operação." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
                TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha margin_bottom">
            <label>Descrição</label>
            <asp:TextBox ID="textDescricao" TextMode="MultiLine" Width="400px" Height="100px" MaxLength="500" runat="server" ToolTip="Será exibido na Nota Fiscal." />

            <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
                ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição da Natureza da Operação." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
                TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha margin_top">
            <label>Natureza da Operação (Prefeitura)</label>
            <asp:TextBox ID="textNaturezaOperacao" Width="400px" MaxLength="50" runat="server" ToolTip="Exibição para controle interno do usuário." />
            
            <asp:RequiredFieldValidator ID="validaNaturezaOperacao" runat="server" Display="None"
                ControlToValidate="textNaturezaOperacao" ErrorMessage="Informe a Natureza da Operação (Prefeitura)." />
            
            <toolKit:ValidatorCalloutExtender ID="ajaxValidaNaturezaOperacao" runat="server"
                TargetControlID="validaNaturezaOperacao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
                HighlightCssClass="validacaoErro" />
        </span>
        <div class="legendaForm margin_top margin_bottom">Emitentes</div>
            <asp:Label id="labelPadrao" Text="Padrão" class="margin_left_label" runat="server"/>
            <asp:Label id="labelEmitentes" Text="Emitentes" class="margin_left_padrao_label" runat="server"/>
        <table class="margin_top">
            <thead></thead>
            <tbody>
                <asp:Repeater ID="repeaterDados" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><input type="checkbox" id="check_padrao" class="margin_left Padrao" onclick="checkboxPadrao(this)" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %>' runat="server" /></td>
                            <td><input type="checkbox" id="check" class="margin_left_padrao Emitentes" onclick="checkboxEmitentes(this)" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %>' runat="server" /></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL]") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </fieldset>
</asp:Content>