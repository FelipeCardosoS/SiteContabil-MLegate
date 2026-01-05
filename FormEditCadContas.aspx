<%@ Page Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEditCadContas.aspx.cs" Inherits="FormEditCadContas" %>
<%@ MasterType VirtualPath="~/MasterEditCad.master" %>

<asp:Content ID="_areaHead" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_areaForm_checkBox").change(function () {
                if(<%=(checkBoxRetorno == true ? "true" : "false")%>)
                {
                    $(this).attr("checked","checked");
                    alert("O sistema exige que uma conta seja CTA!");
                }
            });
        });
    </script>
    <div class="legendaForm">Informe os dados da Conta Contábil</div>
    <fieldset>
        <asp:HiddenField ID="num_itens" Value="2" runat="server" />
        <span class="linha">
            <label>Grupo Contábil</label>
            <asp:DropDownList ID="comboGrupoContabil" Width="200px" runat="server" />
        </span>
        <span class="linha">
            <label>Código</label>
            <asp:TextBox ID="textCodigo" runat="server"></asp:TextBox>
        </span>
        <span class="linha">
            <label>Descricao</label>
            <asp:TextBox ID="textDescricao" Width="220px" runat="server"></asp:TextBox>
        </span>
        <p></p>
        <span class="linha">
            <label>Déb/Créd</label>
            
            <asp:RadioButtonList CssClass="campoRadio" ID="radioDebCred" runat="server">
                <asp:ListItem Text="Débito" Value="D"></asp:ListItem>
                <asp:ListItem Text="Crédito" Value="C"></asp:ListItem>
            </asp:RadioButtonList>
        </span>
        <p></p>

        <span class="linha">
            <label>Conta Sintética</label>
            <asp:DropDownList ID="comboContaSintetica" runat="server">
            </asp:DropDownList>
        </span>
        <p></p>
        <span class="linha">
            <label>Analítica</label>
            <asp:RadioButtonList  CssClass="campoRadio" ID="radioAnalitica" runat="server">
                <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                <asp:ListItem Text="Não" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
        </span>
        <p></p>
        <span class="linha">
            <label>Tipo</label>
            <asp:DropDownList ID="comboTipo" Width="200px" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha">
            <label>Classificação Conta</label>
            <asp:DropDownList ID="comboClassificacao" Width="200px" runat="server" />
        </span>
        <span class="linha" id="campoJobDefault">
            <label>Job Default</label>
            <asp:DropDownList ID="comboJobDefault" Width="280px" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha" id="Span1">
            <label>Tipo Imposto</label>
            <asp:DropDownList ID="comboTipoImposto" Width="200px" runat="server">
            </asp:DropDownList>
        </span>
        <span class="linha" id="Span2">
            <label>Gera Débito/Crédito</label>
            <asp:CheckBox ID="checkGeraCredito" runat="server" />
        </span>
        <span class="linha" id="Span3">
            <label>Retido</label>
            <asp:CheckBox ID="checkRetido" runat="server" />
        </span>
        <span class="linha" id="Span4">
            <label>Recibo</label>
            <asp:CheckBox ID="checkRecibo" runat="server" />
        </span>
        <span class="linha" id="Span5">
            <label>Conta Referencial Sped</label>
            <asp:DropDownList ID="contaRefDropDown" Width="280px" runat="server" />
        </span>
         <span class="linha" id="Span12">
            <label>Conta DRE Sped</label>
            <asp:DropDownList ID="Combo_Dre" Width="280px" runat="server" />
        </span>
        <span class="linha" id="Span13">
            <label>Conta Balanco SPED</label>
            <asp:DropDownList ID="ComboBalanco" Width="280px" runat="server" />
        </span>
        <span class="linha" id="Span10" style="display:none;">
            <label>Conta Referencial ECF</label>
            <asp:DropDownList ID="contaRefDropDownEcf" Width="280px" runat="server" />
        </span>
        <span class="linha" id="Span8">
            <label>CTA</label>
            <asp:CheckBox ID="checkBox" runat="server" />
        </span>
        <span class="linha" id="Span9">
            <label>Job</label>
            <asp:DropDownList ID="comboJob" Width="280px" runat="server" />
        </span>
        <span class="linha" id="Span6">
            <label>Moeda de Balanço</label>
            <asp:DropDownList ID="comboMoedaBalanco" Width="280px" runat="server" />
        </span>
        <span class="linha" id="Span7">
            <label>Moeda de Movimento</label>
            <asp:DropDownList ID="comboMoedaMovimento" Width="280px" runat="server" />
        </span>
    </fieldset>
    
    <asp:CustomValidator ID="validaGrupoContabil" runat="server" ClientValidationFunction="validaCombo"
        ControlToValidate="comboGrupoContabil" ErrorMessage="Escolha um Grupo Contábil." Display="None"></asp:CustomValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxValidaGrupoContabil" runat="server"
        TargetControlID="validaGrupoContabil" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaCodigo" runat="server" Display="None"
        ControlToValidate="textCodigo" ErrorMessage="Informe o Código da Conta."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaCodigo" runat="server"
        TargetControlID="validaCodigo" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <toolKit:FilteredTextBoxExtender ID="filtraCodigo" runat="server"
        FilterType="Numbers, Custom" ValidChars=".,-"  TargetControlID="textCodigo"></toolKit:FilteredTextBoxExtender>
        
    <asp:RequiredFieldValidator ID="validaDescricao" runat="server" Display="None"
        ControlToValidate="textDescricao" ErrorMessage="Informe a Descrição da Conta."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaDescricao" runat="server"
        TargetControlID="validaDescricao" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>

    <asp:RequiredFieldValidator ID="validaRadioDebCred" runat="server" Display="None"
        ControlToValidate="radioDebCred" ErrorMessage="Escolha uma opção."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaRadioDebCred" runat="server"
        TargetControlID="validaRadioDebCred" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
        
    <asp:RequiredFieldValidator ID="validaRadioAnalitica" runat="server" Display="None"
        ControlToValidate="radioAnalitica" ErrorMessage="Escolha uma opção."></asp:RequiredFieldValidator>
    
    <toolKit:ValidatorCalloutExtender ID="ajaxValidaRadioAnalitica" runat="server"
        TargetControlID="validaRadioAnalitica" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
       
    <asp:CustomValidator ID="validaTipo" runat="server" ClientValidationFunction="validaCombo"
        ControlToValidate="comboTipo" ErrorMessage="Escolha um Tipo." Display="None"></asp:CustomValidator>

    <toolKit:ValidatorCalloutExtender ID="ajaxvalidaTipo" runat="server"
        TargetControlID="validaTipo" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" 
        HighlightCssClass="validacaoErro"></toolKit:ValidatorCalloutExtender>
</asp:Content>