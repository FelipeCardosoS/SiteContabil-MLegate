<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridConsultaNF.aspx.cs" Inherits="FormGridConsultaNF" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">
    <script type="text/javascript" src="Js/FormConsultaNF.js"></script>
    <label><span>Emitente</span>
        <select id="ddlEmitente" style="width:250px" runat="server" />
    </label>

    <label><span>Tomador</span>
        <select id="ddlTomador" style="width:250px" runat="server" />
    </label>
    
    <label><span>Nº NF</span>
        <asp:TextBox ID="tbxNumeroNF" Width="47px" MaxLength="9" runat="server" />
    </label>

    <toolKit:FilteredTextBoxExtender ID="ftbeNumeroNF" TargetControlID="tbxNumeroNF" FilterType="Numbers" runat="server" />

    <label><span>Nº RPS</span>
        <asp:TextBox ID="tbxNumeroRPS" Width="47px" MaxLength="9" runat="server" />
    </label>

    <toolKit:FilteredTextBoxExtender ID="ftbeNumeroRPS" TargetControlID="tbxNumeroRPS" FilterType="Numbers" runat="server" />

    <label><span style="margin-left:65px;">Período de Emissão</span>
        <label style="color:blue; margin-left:10px;">De:</label>
        <input type="text" id="tbxDe" class="date" style="width:65px;" maxlength="10" runat="server" />
        <label style="color:blue; margin-left:10px;">Até:</label>
        <input type="text" id="tbxAte" class="date" style="width:65px;" maxlength="10" runat="server" />
    </label>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.date').mask('99/99/9999'); //Data: "De" e "Até"
        });
    </script>

    <label><span>Valor Total</span>
        <input type="text" id="tbxValorTotal" class="Monetaria" style="width:77px" maxlength="18" runat="server" />
    </label>
    
    <label><span>Lote</span>
        <asp:TextBox ID="tbxLote" Width="61px" MaxLength="10" runat="server" />
    </label>

    <toolKit:FilteredTextBoxExtender ID="ftbeLote" TargetControlID="tbxLote" FilterType="Numbers" runat="server" />
    
    <label><span>Situação NF</span>
        <select id="ddlSituacaoNF" style="width:100px" runat="server">
            <option value="0">Escolha</option>
            <option value="T">Válida</option>
            <option value="C">Cancelada</option>
        </select>
    </label>

    <label><span>Status</span>
        <select id="ddlStatus" style="width:100px" runat="server">
            <option value="0">Escolha</option>
            <option value="G">Gerada</option>
            <option value="A">Aceita</option>
            <option value="R">Rejeitada</option>
            <option value="C">Cancelada</option>
        </select>
    </label>

<label style="float: right; margin-right: 10px;">
        <asp:Button ID="btnAcaoRoboV2" runat="server" 
                    Text="Enviar NFSe V2 (Reforma)" 
                    CausesValidation="false" 
                    UseSubmitBehavior="false" 
                    ValidationGroup="GrupoRobo"
                    BackColor="#FFCC00" Font-Bold="true" Height="24px" BorderStyle="Solid" BorderWidth="1px" />
    </label>
    </asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">
    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td/>
            <td>Emitente</td>
            <td>Tomador</td>
            <td>Nº NF</td>
            <td>Nº RPS</td>
            <td>Data de Emissão</td>
            <td>Valor Total</td>
            <td>Lote</td>
            <td>Situação NF</td>
            <td>Status</td>
            <td/>
        </tr>
        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%"><input type="checkbox" id="check" class="check" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_FATURAMENTO_NF]") %>' runat="server" /></td>
                    <td class="line emitente"><asp:Label ID="emitente" Text='<%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_EMITENTE]") %>' runat="server" /></td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_TOMADOR]") %></td>
                    <td class="line"><asp:Label ID="numero_nf" Text='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_NF]") %>' runat="server" /></td>
                    <td class="line"><asp:Label ID="numero_rps" Text='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_RPS]") %>' runat="server" /></td>
                    <td class="line"><asp:Label ID="dt_emissao" Text='<%# DataBinder.Eval(Container.DataItem, "[DATA_EMISSAO_RPS]").ToString().Substring(0,10) %>' runat="server" /></td>
                    <td class="line valor"><%# String.Format("{0:f2}", DataBinder.Eval(Container.DataItem, "[VALOR_SERVICOS]")) %></td>
                    <td class="line funcoes"><p><asp:HyperLink ID="linkLote" runat="server"><asp:Label ID="lote" Text='<%# DataBinder.Eval(Container.DataItem, "[LOTE]") %>' runat="server" /></asp:HyperLink></p></td>
                    <td class="line situacao"><asp:Label ID="situacao_nf" Text='<%# DataBinder.Eval(Container.DataItem, "[SITUACAO_NF]") %>' runat="server" /></td>
                    <td class="line status"><asp:Label ID="status" Text='<%# DataBinder.Eval(Container.DataItem, "[STATUS]") %>' runat="server" /></td>
                    <td class="line funcoes" width="1%"><p><asp:HyperLink ID="linkConsultar" runat="server">Consultar</asp:HyperLink></p></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <toolKit:ModalPopupExtender ID="mpeImportarCSV" TargetControlID="botaoImportarArquivo" PopupControlID="pciImportarCSV" CancelControlID="cciImportarCSV" BackgroundCssClass="pop_background" RepositionMode="RepositionOnWindowResizeAndScroll" runat="server" />
    <asp:Panel ID="pciImportarCSV" CssClass="pop" runat="server">
        <div class="top">
            <div class="popTitulo">Importar Arquivo CSV</div>
            <asp:LinkButton ID="cciImportarCSV" runat="server">X</asp:LinkButton>
        </div>
        <div class="conteudo">
            <fieldset>
                <asp:FileUpload ID="fulImportarCSV" runat="server" />
                <asp:RequiredFieldValidator ID="rfvImportarCSV" ControlToValidate="fulImportarCSV" ValidationGroup="vgpImportarCSV" ErrorMessage="Selecione um arquivo.csv" Display="None" runat="server" />
                <toolKit:ValidatorCalloutExtender ID="vceImportarCSV" TargetControlID="rfvImportarCSV" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
                <asp:Button ID="btnImportarCSV" OnClick="btnImportarCSV_Click" ValidationGroup="vgpImportarCSV" Text="Importar" runat="server" />
            </fieldset>
        </div>
    </asp:Panel>
</asp:Content>