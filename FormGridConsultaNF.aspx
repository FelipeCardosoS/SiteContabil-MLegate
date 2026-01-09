<%@ Page Language="C#" MasterPageFile="~/MasterGrid.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormGridConsultaNF.aspx.cs" Inherits="FormGridConsultaNF" %>
<%@ MasterType VirtualPath="~/MasterGrid.master" %>

<asp:Content ID="_areaFiltro" ContentPlaceHolderID="areaFiltro" runat="server">

    <!-- CSS do modal de certificados -->
    <link rel="stylesheet" type="text/css" href="CSS/FormGridCertificados.css" />

    <!-- JS legado da tela -->
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

    <asp:HiddenField ID="hdfBtnAcaoRoboV2Uid" ClientIDMode="Static" runat="server" />

    <label style="float: right; margin-right: 10px;">
       <asp:Button ID="btnAcaoRoboV2" ClientIDMode="Static" runat="server"
    Text="Enviar NFSe V2 (Reforma)"
    CausesValidation="false"
    UseSubmitBehavior="false"
    OnClientClick="return abrirModalCertificados();"
    BackColor="#FFCC00" Font-Bold="true" Height="24px" BorderStyle="Solid" BorderWidth="1px" />


        <span id="lblCertEscolhido" style="margin-left:10px; color:#0b5394; font-weight:bold;"></span>
    </label>

</asp:Content>

<asp:Content ID="_areaGrid" ContentPlaceHolderID="areaGrid" runat="server">

    <table id="tabelaGrid" cellpadding="0" cellspacing="0">
        <tr class="titulo">
            <td />
            <td>Emitente</td>
            <td>Tomador</td>
            <td>Nº NF</td>
            <td>Nº RPS</td>
            <td>Data de Emissão</td>
            <td>Valor Total</td>
            <td>Lote</td>
            <td>Situação NF</td>
            <td>Status</td>
            <td />
        </tr>

        <asp:Repeater ID="repeaterDados" runat="server">
            <ItemTemplate>
                <tr class="linha">
                    <td class="check line" width="1%">
                        <input type="checkbox"
                               id="check"
                               runat="server"
                               class="check nf-check"
                               value='<%# DataBinder.Eval(Container.DataItem, "[COD_FATURAMENTO_NF]") %>'
                               data-cod='<%# DataBinder.Eval(Container.DataItem, "[COD_FATURAMENTO_NF]") %>'
                               data-emitente='<%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_EMITENTE]") %>'
                               data-tomador='<%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_TOMADOR]") %>'
                               data-nf='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_NF]") %>'
                               data-rps='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_RPS]") %>'
                               data-emissao='<%# DataBinder.Eval(Container.DataItem, "[DATA_EMISSAO_RPS]").ToString().Substring(0,10) %>'
                               data-valor='<%# String.Format("{0:f2}", DataBinder.Eval(Container.DataItem, "[VALOR_SERVICOS]")) %>' />
                    </td>
                    <td class="line emitente">
                        <asp:Label ID="emitente" Text='<%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_EMITENTE]") %>' runat="server" />
                    </td>
                    <td class="line"><%# DataBinder.Eval(Container.DataItem, "[NOME_RAZAO_SOCIAL_TOMADOR]") %></td>
                    <td class="line">
                        <asp:Label ID="numero_nf" Text='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_NF]") %>' runat="server" />
                    </td>
                    <td class="line">
                        <asp:Label ID="numero_rps" Text='<%# DataBinder.Eval(Container.DataItem, "[NUMERO_RPS]") %>' runat="server" />
                    </td>
                    <td class="line">
                        <asp:Label ID="dt_emissao" Text='<%# DataBinder.Eval(Container.DataItem, "[DATA_EMISSAO_RPS]").ToString().Substring(0,10) %>' runat="server" />
                    </td>
                    <td class="line valor"><%# String.Format("{0:f2}", DataBinder.Eval(Container.DataItem, "[VALOR_SERVICOS]")) %></td>
                    <td class="line funcoes">
                        <p>
                            <asp:HyperLink ID="linkLote" runat="server">
                                <asp:Label ID="lote" Text='<%# DataBinder.Eval(Container.DataItem, "[LOTE]") %>' runat="server" />
                            </asp:HyperLink>
                        </p>
                    </td>
                    <td class="line situacao">
                        <asp:Label ID="situacao_nf" Text='<%# DataBinder.Eval(Container.DataItem, "[SITUACAO_NF]") %>' runat="server" />
                    </td>
                    <td class="line status">
                        <asp:Label ID="status" Text='<%# DataBinder.Eval(Container.DataItem, "[STATUS]") %>' runat="server" />
                    </td>
                    <td class="line funcoes" width="1%">
                        <p><asp:HyperLink ID="linkConsultar" runat="server">Consultar</asp:HyperLink></p>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <!-- =========================
         MODAL CSV (existente)
         ========================= -->
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

    <!-- =========================
         MODAL CERTIFICADOS
         ========================= -->
    <asp:Button ID="btnAbrirCertModal" ClientIDMode="Static" runat="server" Style="display:none;" CausesValidation="false" UseSubmitBehavior="false" />
    <asp:Button ID="btnAbrirUploadModal" ClientIDMode="Static" runat="server" Style="display:none;" CausesValidation="false" UseSubmitBehavior="false" />
    <asp:HiddenField ID="hdfCertSelecionado" ClientIDMode="Static" runat="server" />

    <toolKit:ModalPopupExtender
        ID="mpeCertificados"
        runat="server"
        TargetControlID="btnAbrirCertModal"
        PopupControlID="pciCertificados"
        CancelControlID="cciCertificados"
        BackgroundCssClass="pop_background"
        RepositionMode="RepositionOnWindowResizeAndScroll"
        BehaviorID="mpeCertificadosBehavior" />

    <asp:Panel ID="pciCertificados" runat="server" CssClass="pop modal-cert" Style="display:none;">
        <div class="top">
            <div class="popTitulo">
                Selecionar Certificado
                <span class="subtitulo">— NFs selecionadas: <span id="spnQtdSelecionadas">0</span></span>
            </div>
            <asp:LinkButton ID="cciCertificados" ClientIDMode="Static" runat="server" CssClass="popClose">X</asp:LinkButton>
        </div>

        <div class="conteudo">
            <div class="box">
                <div class="box-title">Notas selecionadas</div>
                <table class="mini-table" id="tblNotasSelecionadas">
                    <thead>
                        <tr>
                            <th>Emitente</th>
                            <th>Tomador</th>
                            <th>Nº NF</th>
                            <th>Nº RPS</th>
                            <th>Emissão</th>
                            <th>Valor</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>

            <div class="cert-toolbar">
                <input type="button" class="btn-sec" value="Upload novo certificado" onclick="abrirModalUploadCert(); return false;" />
                <span id="spnAvisosCert" class="cert-avisos"></span>
            </div>

            <div class="box">
                <div class="box-title">Certificados disponíveis</div>
                <table class="cert-table" id="tblCertificados">
                    <thead>
                        <tr>
                            <th style="width:30px;"></th>
                            <th>Alias / Identificação</th>
                            <th>Validade</th>
                            <th>Status</th>
                            <th>Arquivo</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptCertificados" runat="server">
                            <ItemTemplate>
                                <tr class="cert-row" data-id="<%# Eval("CertId") %>">
                                    <td><input type="radio" name="rbCert" value="<%# Eval("CertId") %>" /></td>
                                    <td><%# Eval("Alias") %></td>
                                    <td><%# Eval("ValidadeFimFmt") %></td>
                                    <td><span class="badge <%# Eval("BadgeClass") %>"><%# Eval("StatusTexto") %></span></td>
                                    <td><%# Eval("ArquivoNome") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

            <div class="cert-actions">
                <input type="button" class="btn-prim" value="Confirmar e continuar" onclick="usarCertificadoSelecionado(); return false;" />
                <input type="button" class="btn-sec" value="Cancelar" onclick="fecharModalCertificados(); return false;" />
            </div>
        </div>
    </asp:Panel>

    <!-- =========================
         MODAL UPLOAD
         ========================= -->
    <toolKit:ModalPopupExtender
        ID="mpeUploadCert"
        runat="server"
        TargetControlID="btnAbrirUploadModal"
        PopupControlID="pciUploadCert"
        CancelControlID="cciUploadCert"
        BackgroundCssClass="pop_background"
        RepositionMode="RepositionOnWindowResizeAndScroll"
        BehaviorID="mpeUploadCertBehavior" />

    <asp:Panel ID="pciUploadCert" runat="server" CssClass="pop modal-upload" Style="display:none;">
        <div class="top">
            <div class="popTitulo">Upload de novo certificado</div>
            <asp:LinkButton ID="cciUploadCert" ClientIDMode="Static" runat="server" CssClass="popClose">X</asp:LinkButton>
        </div>

        <div class="conteudo">
            <div class="upload-grid">
                <label>Alias / Identificação
                    <asp:TextBox ID="txtAliasCert" ClientIDMode="Static" runat="server" MaxLength="120" />
                </label>

                <label>Arquivo (.pfx / .p12)
                    <asp:FileUpload ID="fuCertificado" ClientIDMode="Static" runat="server" />
                </label>

                <label>Senha do certificado
                    <asp:TextBox ID="txtSenhaCert" ClientIDMode="Static" runat="server" TextMode="Password" MaxLength="200" />
                </label>
            </div>

            <div class="cert-actions">
                <asp:Button ID="btnSalvarCertificado"
                     ClientIDMode="Static"
                     runat="server"
                     CssClass="btn-prim"
                     Text="Salvar (Banco)"
                     CausesValidation="false"
                     UseSubmitBehavior="false"
                     OnClientClick="
                         window.CERT_UI = window.CERT_UI || {};
                         window.CERT_UI.lastSubmitterId = 'btnSalvarCertificado';
                         window.CERT_UI.allowOnePostback = true;
                         this.disabled = true;
                         this.value = 'Salvando...';
                     "
                     OnClick="btnSalvarCertificado_Click" />



                <input type="button" class="btn-sec" value="Voltar" onclick="voltarParaListaCertificados(); return false;" />
            </div>

            <asp:Label ID="lblUploadErro" runat="server"
                Style="display:block; margin-top:8px; color:#a61c00; font-weight:bold;"></asp:Label>

            <asp:HiddenField ID="hdfVoltarListaAposUpload" ClientIDMode="Static" runat="server" />
        </div>
    </asp:Panel>

    <!-- JS novo (modal certificados/upload) - no FINAL para garantir jQuery e $find -->
    <script type="text/javascript" src="JS/FormGridCertificados.js"></script>

</asp:Content>
