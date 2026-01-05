<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadTributos.aspx.cs" Inherits="FormEditCadTributos" %>

<%@ MasterType VirtualPath="~/MasterEditCad.master" %>


<asp:Content ID="areaForm" ContentPlaceHolderID="areaForm" runat="Server">
    <script type="text/javascript" src="Js/select2.min.js"></script>
    <style type="text/css">
        @import url(Css/select2.min.css);
    </style>
    <div class="legendaForm">Informe os dados dos Impostos Sobre Vendas</div>
    <asp:TextBox ID="Hdid" CssClass="Hdid" runat="server" />
    <fieldset>
        <span class="linha">
            <label>Nome</label>
            <asp:TextBox ID="textNome" Width="250px" MaxLength="200" runat="server" ToolTip="Exibição para controle interno do usuário." />

            <asp:RequiredFieldValidator ID="validaNome" runat="server" Display="None"
                ControlToValidate="textNome" ErrorMessage="Informe o Nome do Tributo." />

            <toolKit:ValidatorCalloutExtender ID="ajaxValidaNome" runat="server"
                TargetControlID="validaNome" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif"
                HighlightCssClass="validacaoErro" Width="210px" />
        </span>
        <span class="linha">
            <label>Alíquota</label>
            <asp:TextBox ID="textAliquota" Width="100px" CssClass="Monetaria" MaxLength="18" runat="server" 
                ToolTip="O valor digitado, será automaticamente arredondado para 4 casas decimais pelo sistema." />

            <asp:RequiredFieldValidator ID="validaAliquota" runat="server" Display="None"
                ControlToValidate="textAliquota" ErrorMessage="Informe a Alíquota do Tributo." />

            <toolKit:ValidatorCalloutExtender ID="ajaxValidaAliquota" runat="server"
                TargetControlID="validaAliquota" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif"
                HighlightCssClass="validacaoErro" Width="225px" />
        </span>
        <span class="linha">
            <label>Tributo</label>
            <asp:DropDownList ID="ComboTributo" Width="288px" AutoPostBack="false" CssClass="Visivel cboTributo" runat="server"></asp:DropDownList>
        </span>
        <span class="linha">
            <label>Destacado</label>
            <input type="checkbox" id="ckDestacado" class="" name="check" checked="checked" runat="server" />
        </span>
        <div class="legendaForm margin_top margin_bottom">Emitentes/Serviços</div>
        <table class="margin_top">
            <thead></thead>
            <tbody>
                <asp:Repeater ID="repeaterDados" runat="server">
                    <ItemTemplate>
                        <tr style="height: auto;">
                            <td>
                                <input type="checkbox" id="check" class="ckemitente margin_left" name="check" value='<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %>' runat="server" /></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL]") %></td>
                            <td>
                                <select id="cbservicos" class="cbservicos cbservicos_<%# DataBinder.Eval(Container.DataItem, "[COD_EMPRESA]") %> form-control" multiple="multiple" style="width: 400px;">
                                    <%--<option>Carregando...</option>--%>
                                </select>
                                <input type="text" id="TxtHide" class="TxtHide" runat="server"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </fieldset>
    <style>
        .Hdid {
            display:none;
        }
        .TxtHide {
            display:none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            cbservicoload();

            $(".cbservicos").select2();
            load();
        });

        function cbservicoload() {
            $('.cbservicos').each(function () {
                var cod_emitente = $(this).closest('tr').find('.ckemitente').val();
                $.ajax({
                    type: "POST",
                    async: false,
                    cache: false,
                    url: "../FormEditCadTributos.aspx/CarregaServicos",
                    data: "{Cod_Emitente: " + cod_emitente + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (ret) {
                        Carregarcbservico(ret.d, cod_emitente);
                    }
                });
            });
        }

        function Carregarcbservico(ret, cod_emitente) {
            jsn = jQuery.parseJSON(ret);
            $('.cbservicos_' + cod_emitente).find('option').remove();
            $(jsn).each(function () {
                $('.cbservicos_' + cod_emitente).append('<option value="' + this.cod_servico + '">' + this.nome + '</option>');
            });
        }

        function load() {
            $('.cbservicos').each(function () {
                var cod_tributo = $('.Hdid').val();
                var cod_emitente = $(this).closest('tr').find('.ckemitente').val();
                $.ajax({
                    type: "POST",
                    async: false,
                    cache: false,
                    url: "../FormEditCadTributos.aspx/CarregaServicosEmitente",
                    data: "{ Cod_Tributo: " + cod_tributo + ", Cod_Emitente: " + cod_emitente + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (ret) {
                        CarregarcbservicoEmitente(ret.d, cod_emitente);
                    }
                });
            });
        }

        function CarregarcbservicoEmitente(ret, cod_emitente) {
            jsn = jQuery.parseJSON(ret);
            $('.cbservicos_' + cod_emitente).val(jsn).trigger("change");
        }

        $(".cbservicos").on("change", function (e) {
            $(this).closest('tr').find('.TxtHide').val($(this).select2("val"));
        });
    </script>
</asp:Content>

