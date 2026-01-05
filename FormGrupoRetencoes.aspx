<%--Projeto interrompido em 2018. Sem previsão de retomada.--%>
<%--Menu (Banco de Dados): "CAD_MODULOS" e "CAD_TAREFAS" já possuem estrutura (FATURAMENTO_CADASTROS_GRUPO_RETENCOES).--%>
<%--Engloba 5 Arquivos: "FormGrupoRetencoes.js", "FormGrupoRetencoes.aspx", "FormGrupoRetencoes.aspx.cs", "GrupoRetencao.cs" e "GrupoRetencoesDAO.cs".--%>

<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGrupoRetencoes.aspx.cs" Inherits="FormGrupoRetencoes" %>

<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <script type="text/javascript" src="Js/FormGrupoRetencoes.js"></script>
    <fieldset>
        <span class="linha">
            <label style="color: blue;">Emitente: </label>
            <asp:DropDownList ID="ddlEmitente" CssClass="ddlEmitente" OnSelectedIndexChanged="ddlEmitente_Changed" AutoPostBack="true" Width="708px" runat="server"></asp:DropDownList>
        </span>
        <table id="tblGrupoRetencoes" cellpadding="0" cellspacing="0" style="margin-left:80px; margin-top:12px; border:1px solid #B0C4DE; width:77%" class="Visivel">
            <thead>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE;" colspan="2">Grupo de Retenções</th>
                    <th style="border:1px solid #B0C4DE;">
                        <input type="button" value="Novo" onclick="btnNovoGR_Click()" style="margin-left:5px;" />
                        <input type="button" value="Deletar" onclick="btnDeletarGR_Click()" style="margin-left:5px;" />
                    </th>
                </tr>
                <tr class="titulo">
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight: normal" colspan="2">Nome</th>
                    <th style="color:blue; border:1px solid #B0C4DE; font-weight: normal">Ativo</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptrGrupoRetencoes" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="line"><asp:RadioButton ID="rbtnCodGR" CssClass="rbtnCodGR" OnCheckedChanged="rbtnCodGR_CheckedChanged" AutoPostBack="true" GroupName="GrupoRetencoes" Value='<%# DataBinder.Eval(Container.DataItem, "[COD_GRUPO_RETENCAO]") %>' runat="server" /></td>
                            <td class="line"><input type="text" id="tbxNomeGR" class="tbxNomeGR" style="width:615px; border-color:transparent;" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' runat="server" /></td>
                            <td class="line"><input type="checkbox" id="cbxAtivoGR" class="cbxAtivoGR" style="width:80px; height:16px; margin-left:10px;" runat="server" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <%--        <table id="Retencao" cellpadding="0" cellspacing="0" style="margin-top: 12px; border: 1px solid #B0C4DE" class="Visivel">
            <thead>
                <tr class="titulo">
                    <th style="color: blue; border: 1px solid #B0C4DE" colspan="4">Retenções</th>
                </tr>
                <tr class="titulo">
                    <th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal" colspan="2">Nome</th>
                    <th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">%</th>
                    <th style="color: blue; border: 1px solid #B0C4DE; font-weight: normal">Modo de Apresentação</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="repeaterDados" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="line">
                                <input type="checkbox" id="cod_retencao" class="cod_retencao" onclick="Retencao_Checked_Changed()" style="width: 40px" value='<%# DataBinder.Eval(Container.DataItem, "[COD_RETENCAO]") %>' runat="server" /></td>
                            <td class="line">
                                <input type="text" id="nome_retencao" class="nome_retencao" style="width: 615px" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' runat="server" /></td>
                            <td class="line">
                                <input type="text" id="aliquota_retencao" class="aliquota_retencao" style="width: 100px; text-align: center" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[ALIQUOTA]") %>' runat="server" /></td>
                            <td class="line">
                                <input type="text" id="apresentacao_retencao" class="apresentacao_retencao" style="width: 200px; text-align: center" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[APRESENTACAO]") %>' runat="server" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>--%>
    </fieldset>
    <div class="botoes">
        <input type="button" id="btnSalvar" value="Salvar" onclick="btnSalvar_Click()" class="btnSalvar Visivel" runat="server" />
    </div>
</asp:Content>