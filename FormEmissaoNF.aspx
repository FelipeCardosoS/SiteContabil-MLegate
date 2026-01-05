<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEmissaoNF.aspx.cs" Inherits="FormEmissaoNF" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <script type="text/javascript" src="Js/FormEmissaoNF.js"></script>
    <fieldset>
        <div id="div_NaturezaOperacao">
            <asp:HiddenField ID="HF_NaturezaOperacao" runat="server" />
        </div>
        <div id="div_PrestacaoServico">
            <asp:HiddenField ID="HF_PrestacaoServico" runat="server" />
        </div>
        <div id="div_Job">
            <asp:HiddenField ID="HF_Job" runat="server" />
        </div>
        <div id="div_Narrativa">
            <asp:HiddenField ID="HF_Narrativa" runat="server" />
        </div>
        <span class="linha">
            <label style="color:blue;">Emitente: </label>
            <asp:DropDownList ID="ddlEmitente" Width="858px" OnSelectedIndexChanged="ddlEmitente_Changed" AutoPostBack="true" CssClass="ddlEmitente" runat="server"></asp:DropDownList>
        </span>
        <div class="Visivel">
            <span class="linha">
                <label style="color:blue;">Tomador: </label>
                <asp:DropDownList ID="ddlCliente" Width="858px" OnSelectedIndexChanged="ddlCliente_Changed" AutoPostBack="true" CssClass="ddlCliente" runat="server"></asp:DropDownList>
            </span>
            <span class="linha">
                <label style="color:blue;">Data de Emissão: </label>
                <asp:TextBox ID="textDataEmissaoRPS" Width="80px" MaxLength="10" CssClass="date txtDataEmissaoRPS" runat="server" />
                <asp:RequiredFieldValidator ID="validaDataEmissaoRPS" ControlToValidate="textDataEmissaoRPS" ErrorMessage="Informe a Data de Emissão da Nota Fiscal." Display="None" runat="server" />
                <toolKit:ValidatorCalloutExtender ID="ajaxValidaDataEmissaoRPS" TargetControlID="validaDataEmissaoRPS" CssClass="customCalloutStyle" HighlightCssClass="validacaoErro" WarningIconImageUrl="Imagens/pixel.gif" runat="server" />
                <label style="color:blue; width:222px;">Última Nota Fiscal Emitida Em: </label>
                <asp:TextBox ID="textData_RPS" Width="80px" MaxLength="10" CssClass="txtDataRPS" Enabled="False" runat="server" />
                <label style="color:blue; margin-left:252px;">Data de Competência: </label>
                <asp:TextBox ID="textDataCompetencia" Width="50px" MaxLength="7" CssClass="dateCompetencia txtDataCompetencia" onblur="ObservacoesNF_Changed()" onchange="ObservacoesNF_Changed()" runat="server" />
            </span>
            <span class="linha">
                <label style="color:blue;">Natureza da Operação: </label>
                <asp:DropDownList ID="comboNaturezaOperacao" Width="180px" CssClass="cboNaturezaOperacao" runat="server"></asp:DropDownList>
                <label style="color:blue;">Descrição: </label>
                <asp:TextBox ID="textDescricao_NO" Width="340px" CssClass="txtDescricaoNO" Enabled="false" runat="server"></asp:TextBox>
                <label style="color:blue;">Código da Prefeitura: </label>
                <asp:TextBox ID="textNaturezaOperacao" Width="40px" CssClass="txtNaturezaOperacao" Enabled="false" runat="server" />
            </span>
            <span class="linha">
                <label style="color:blue;">Prestação de Serviço: </label>
                <asp:DropDownList ID="comboPrestacaoServico" Width="180px" CssClass="cboPrestacaoServico" runat="server"></asp:DropDownList>
                <label style="color:blue;">Descrição: </label>
                <asp:TextBox ID="textDescricao_PS" Width="340px" CssClass="txtDescricaoPS" Enabled="false" runat="server"></asp:TextBox>
            </span>

            <span class="linha" style="margin-top:10px; display:block;">
                <fieldset style="border: 1px solid #B0C4DE; padding: 5px; width: 845px;">
                    <legend style="color:blue; font-weight:bold; font-size: 11px;">Tributação Federal (Reforma Tributária - EC 132/2023)</legend>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100px;">
                                <label style="color:blue;">IBS (0,1%):</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAliquotaIBS" runat="server" Width="40px" Enabled="false" Text="0,10" style="text-align:center; background-color:#f0f0f0;"></asp:TextBox> %
                                &nbsp; 
                                <label style="color:blue;">Valor:</label> R$ 
                                <asp:TextBox ID="txtValorIBS" runat="server" Width="80px" Enabled="false" style="text-align:right; background-color:#f0f0f0;"></asp:TextBox>
                            </td>
                            <td style="width: 100px;">
                                <label style="color:blue;">CBS (0,9%):</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAliquotaCBS" runat="server" Width="40px" Enabled="false" Text="0,90" style="text-align:center; background-color:#f0f0f0;"></asp:TextBox> %
                                &nbsp; 
                                <label style="color:blue;">Valor:</label> R$ 
                                <asp:TextBox ID="txtValorCBS" runat="server" Width="80px" Enabled="false" style="text-align:right; background-color:#f0f0f0;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </span>
            <table id="Retencao" cellpadding="0" cellspacing="0" style="margin-top:12px; border:1px solid #B0C4DE;">
                <thead>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE;" colspan="4">Retenções</th>
                    </tr>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;" colspan="2">Nome</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">%</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">Modo de Apresentação</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="repeaterDados" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="line"><input type="checkbox" id="cod_retencao" class="cod_retencao" onclick="Retencao_Checked_Changed()" style="width:40px; height:16px;" value='<%# DataBinder.Eval(Container.DataItem, "[COD_RETENCAO]") %>' runat="server" /></td>
                                <td class="line"><input type="text" id="nome_retencao" class="nome_retencao" style="width:615px;" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[NOME]") %>' runat="server" /></td>
                                <td class="line"><input type="text" id="aliquota_retencao" class="aliquota_retencao" style="width:100px; text-align:center;" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[ALIQUOTA]") %>' runat="server" /></td>
                                <td class="line"><input type="text" id="apresentacao_retencao" class="apresentacao_retencao" style="width:200px; text-align:center;" disabled="disabled" value='<%# DataBinder.Eval(Container.DataItem, "[APRESENTACAO]") %>' runat="server" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div>
                <label id="lblJobsInativos" style="color:blue; margin-left:813px; margin-right:-6px;">Mostrar Jobs Inativos: </label>
                <input type="checkbox" id="cbxJobsInativos" class="cbxJobsInativos" onclick="Jobs_Inativos_Checked_Changed()" style="width:22px; height:22px; margin-top:0px;" />
            </div>
            <table id="Servicos_Jobs" cellpadding="0" cellspacing="0" style="margin-top:30px; border:1px solid #B0C4DE;">
                <thead>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">Serviços</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">Jobs</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;" colspan="3">Valor Bruto</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="line" style="width:375px;"><select id="comboServico" class="cboServico" style="width:350px;" runat="server" /></td>
                        <td class="line" style="width:375px;"><select id="comboJob" class="cboJob" onchange="cboJob_Changed(this)" style="width:350px;" runat="server" /><select class="cboJobObs" style="width:350px; display:none;" disabled="disabled" /></td>
                        <td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="18" class="Monetaria txtValorServicoJob" onchange="Valor_Changed(this, 'txtValorServicoJob')" onpaste="Formata_Valor_Copiado(event)" /></td>
                        <td class="line tdADD"><input type="button" value="+" class="btnPlus_Servicos_Jobs" onclick="Adicionar_Servicos_Jobs()" /></td>
                        <td class="line"><input type="button" value="-" class="btnMinus_Servicos_Jobs" onclick="Remover_Servicos_Jobs(this)" /></td>
                    </tr>
                </tbody>
            </table>
            <label id="lblTotal" class="lblTotal" style="color:blue; margin-left:755px; clear:both; text-align:left!important; width:200px!important;">Total: R$ 0,00</label>
            <label id="lblTotal_Liquido" class="lblTotal_Liquido" style="color:blue; margin-left:755px; clear:both; text-align:left!important; width:200px!important;">Total Líquido: R$ 0,00</label>
            <span class="linha"></span>
            <table id="Vencimento" cellpadding="0" cellspacing="0" style="border:1px solid #B0C4DE;">
                <thead>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">Data de Vencimento</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;" colspan="3">Valor</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="10" class="date txtDataVencimento" /></td>
                        <td class="line" style="width:200px;"><input type="text" style="width:175px;" maxlength="18" class="Monetaria txtValorVencimento" onchange="Valor_Changed(this, 'txtValorVencimento')" onpaste="Formata_Valor_Copiado(event)" /></td>
                        <td class="line tdADD_Vencimento"><input type="button" value="+" class="btnPlus_Vencimento" onclick="Adicionar_Vencimento()" /></td>
                        <td class="line"><input type="button" value="-" class="btnMinus_Vencimento" onclick="Remover_Vencimento(this)" /></td>
                    </tr>
                </tbody>
            </table>
            <label id="lblTotal_Vencimento" class="lblTotal_Vencimento" style="color:blue; margin-left:203px; clear:both; text-align:left!important; width:200px!important;">Total: R$ 0,00</label>
            <span class="linha"></span>
            <table id="Narrativa" cellpadding="0" cellspacing="0" style="border:1px solid #B0C4DE;">
                <thead>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE;" colspan="4">Narrativa</th>
                    </tr>
                    <tr class="titulo">
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;">Nome</th>
                        <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal;" colspan="3">Descrição</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="line" style="width:375px;"><select id="comboNarrativa" class="cboNarrativa" style="width:350px;" runat="server" /></td>
                        <td class="line" style="width:575px;"><input type="text" id="textDescricao_Narrativa" class="txtDescricaoNarrativa" style="width:550px;" disabled="disabled" runat="server" /></td>
                        <td class="line tdADD_Narrativa"><input type="button" value="+" class="btnPlus_Narrativa" onclick="Adicionar_Narrativa()" /></td>
                        <td class="line"><input type="button" value="-" class="btnMinus_Narrativa" onclick="Remover_Narrativa(this)" /></td>
                    </tr>
                </tbody>
            </table>
            <span class="linha" style="margin-top:30px;">
                <label style="color:blue;">Observações: </label>
                <asp:TextBox ID="textObservacoes" TextMode="MultiLine" Width="830px" Height="100px" CssClass="txtObservacoes MaxLines" onchange="ObservacoesNF_Changed()" MaxLines="4" MaxLength="400" runat="server" />
                <label class="lblCaracteres_Obs">Caracteres: </label>
                <div id="boxFiltro" style="margin-top:15px; margin-bottom:15px;"></div>
                <label style="color:blue;">Observações da Nota Fiscal: </label>
                <asp:TextBox TextMode="MultiLine" Width="830px" Height="100px" CssClass="txtObservacoesNF" Enabled="False" runat="server" />
                <label class="lblCaracteres_Obs_NF">Caracteres: </label>
            </span>
        </div>
    </fieldset>
    <div class="botoes Visivel">
        <input type="button" value="Gerar Nota Fiscal" id="btnGerarNF" class="btnGerarNF" onclick="btnGerarNF_Click()" runat="server" />
    </div>
</asp:Content>