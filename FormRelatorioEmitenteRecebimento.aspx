<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioEmitenteRecebimento.aspx.cs" Inherits="FormRelatorioEmitenteRecebimento" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" runat="Server">
    <script type="text/javascript" src="Js/select2.min.js"></script>
    <style type="text/css">@import url(Css/select2.min.css);</style>

    <div style="width:1000px;">
        <div style="margin-left:7px;">
            <input type="text" id="tbxPage_IsPostBack" style="display:none;" runat="server" />
            <asp:ListBox ID="ddlEmitente" CssClass="ddlEmitente" SelectionMode="Multiple" Width="489px" runat="server" />
            <input type="text" id="tbxEmitente" class="tbxEmitente" style="display:none;" runat="server" />
            <label style="margin-left:10px;"></label>
            <asp:ListBox ID="ddlTomador" CssClass="ddlTomador" SelectionMode="Multiple" Width="489px" runat="server" />
            <input type="text" id="tbxTomador" class="tbxTomador" style="display:none;" runat="server" />
        </div>
        
        <div style="margin-top:10px;">
            <div style="text-align:center; width:215px; float:left; display:inline-table;">
                <div>Período Recebimento</div>
                <label style="color:blue; margin-left:10px;">De: </label>
                <input type="text" id="tbxDeRec" class="date" style="width:65px;" maxlength="10" runat="server" />
                <label style="color:blue; margin-left:10px;">Até: </label>
                <input type="text" id="tbxAteRec" class="date" style="width:65px;" maxlength="10" runat="server" />
            </div>
            
            <div style="text-align:center; width:215px; float:left; display:inline-table; margin-left:50px; margin-right:65px;">
                <div>Período Faturamento</div>
                <label style="color:blue; margin-left:10px;">De: </label>
                <input type="text" id="tbxDe" class="date" style="width:65px;" maxlength="10" runat="server" />
                <label style="color:blue; margin-left:10px;">Até: </label>
                <input type="text" id="tbxAte" class="date" style="width:65px;" maxlength="10" runat="server" />
            </div>
            
            <div>
                <select id="ddlOrdenacao" style="width:200px; margin-top:15px; margin-bottom:15px; margin-right:45px;" runat="server">
                    <option value="0">Ordenar por:</option>
                    <option value="NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS">Emitente</option>
                    <option value="DATA_RECEBIMENTO, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE">Data Recebimento</option>
                    <option value="NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, DATA_RECEBIMENTO">Data Faturamento</option>
                    <option value="NF.NUMERO_RPS, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, NF.DATA_EMISSAO_RPS">Número NF</option>
                    <option value="NOME_RAZAO_SOCIAL_CLIENTE">Grupo Econômico</option>
                    <option value="NF.NOME_RAZAO_SOCIAL_TOMADOR">Cliente (Nota Fiscal)</option>
                    <option value="VALOR_BRUTO, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, DATA_RECEBIMENTO">Valor Bruto</option>
                    <option value="APRESENTACAO, VALOR_RETENCOES, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS">Retenção</option>
                    <option value="VALOR_RETENCOES, APRESENTACAO, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS">Valor Retenção</option>
                    <option value="Parametro_ValorLiquido">Valor Líquido</option>
                    <option value="VALOR_RECEBIMENTO, DATA_RECEBIMENTO, NF.DATA_EMISSAO_RPS, NF.NUMERO_RPS, NF.NOME_RAZAO_SOCIAL_EMITENTE, NF.COD_EMITENTE">Valor Recebimento</option>
                </select>
                <input type="checkbox" id="cbxGrupoEconomico" style="width:50px; height:22px; margin-top:15px; margin-bottom:15px; text-align:center; vertical-align:middle;" runat="server" />Visualizar Grupo Econômico
            </div>
        </div>
    </div>
    
    <div id="div_Tomador">
        <asp:HiddenField ID="HF_Tomador" runat="server" />
    </div>
    
    <div style="height:475px; margin-top:10px; margin-left:7px;">
        <rsweb:ReportViewer ID="rptEmitenteRecebimento" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
            Width="100%" Height="100%" SizeToReportContent="True" OnReportRefresh="rptEmitenteRecebimento_ReportRefresh" runat="server">
            <LocalReport ReportPath="Relatorios\EmitenteRecebimento.rdlc" EnableHyperlinks="True">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="DataSourceEmitenteRecebimento" Name="dsEmitenteRecebimento" />
                    <rsweb:ReportDataSource DataSourceId="DataSourceTotalValorBrutoRecebimento" Name="dsTotalValorBrutoRecebimento" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>

        <asp:ObjectDataSource ID="DataSourceEmitenteRecebimento" runat="server" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.EmitenteRecebimentoTableAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="tbxPage_IsPostBack" Name="Page_IsPostBack" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxEmitente" Name="Emitentes_Selecionados" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxTomador" Name="Tomadores_Selecionados" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxDeRec" Name="De_Rec" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxAteRec" Name="Ate_Rec" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxDe" Name="De" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxAte" Name="Ate" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="ddlOrdenacao" Name="Ordenacao" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="DataSourceTotalValorBrutoRecebimento" runat="server" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="GetData" TypeName="RelatoriosDAOTableAdapters.TotalValorBrutoRecebimentoTableAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="tbxPage_IsPostBack" Name="Page_IsPostBack" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxEmitente" Name="Emitentes_Selecionados" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxTomador" Name="Tomadores_Selecionados" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxDeRec" Name="De_Rec" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxAteRec" Name="Ate_Rec" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxDe" Name="De" PropertyName="Value" Type="String" />
                <asp:ControlParameter ControlID="tbxAte" Name="Ate" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".ddlEmitente").select2({
                tags: true,
                allowClear: true,
                placeholder: "Emitentes"
            })

            $(".ddlTomador").select2({
                tags: true,
                allowClear: true,
                placeholder: "Clientes NF"
            })

            var temp = JSON.parse($('#div_Tomador input[type=hidden]').val());
            $(temp).each(function () {
                $('.ddlTomador').append('<option value="' + $(this)[0] + '">' + $(this)[1] + '</option>');
            });

            $('.date').mask('99/99/9999'); //Data: "De" e "Até"
        });

        var $eventSelect = $(".ddlEmitente");
        $eventSelect.on("change", function (e) {
            $('.tbxEmitente').val('');
            $('.tbxTomador').val('');

            $('.ddlEmitente option:selected').each(function () {
                $('.tbxEmitente').val($('.tbxEmitente').val() + ', ' + $(this).val());
            });

            $('.ddlTomador').find('option').remove();

            temp = JSON.parse($('#div_Tomador input[type=hidden]').val());
            $(temp).each(function () {
                $('.ddlTomador').append('<option value="' + $(this)[0] + '">' + $(this)[1] + '</option>');
            });

            if ($('.tbxEmitente').val() != '') {
                $('.ddlTomador').find('option').remove();

                temp = JSON.parse($('#div_Tomador input[type=hidden]').val());
                $(temp).each(function () {
                    var Emitentes_Selecionados = $('.tbxEmitente').val().substring(2).split(", ");

                    for (var i = 0; i < Emitentes_Selecionados.length; i++) {
                        if ($(this)[2].indexOf(Emitentes_Selecionados[i]) >= 0) {
                            $('.ddlTomador').append('<option value="' + $(this)[0] + '">' + $(this)[1] + '</option>');
                            break;
                        }
                    }
                });
            }
        });

        $eventSelect = $(".ddlTomador");
        $eventSelect.on("change", function (e) {
            $('.tbxTomador').val('');

            $('.ddlTomador option:selected').each(function () {
                $('.tbxTomador').val($('.tbxTomador').val() + ', ' + $(this).val());
            });
        });
    </script>
    <style>
        ul {
            padding-bottom: 10px !important;
        }
    </style>
</asp:Content>