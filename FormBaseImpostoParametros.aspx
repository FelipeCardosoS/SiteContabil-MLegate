<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormBaseImpostoParametros.aspx.cs" Inherits="FormBaseImpostoParametros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        fieldset 
        {
            float:left !important;
            margin:0px !important;
            padding:0px !important;
            width:480px !important;
        }
        select
        {
            margin-top: 4px;
            width:250px;    
        }
        #Soma 
        {
            width:30px !important;
            margin-left:10px;
        }
        #Subtrai
        {
            width:30px !important;
        }
    </style>
    <script src="Js/FormBaseImpostoParametros.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
            <fieldset id="quadro">
                <legend id="legenda">Informe a(s) conta(s):</legend>
                <span id="optionContasMenu" class="linha option1">
                    <label>Conta: </label>
                    <select id="Contas" name="Contas" class="optionContas1"><option value="">Carregando...</option></select> <input type="button" id="Soma" value=" + "> <input type="button" id="Subtrai" onclick="removeOption('option1')" value=" - " />
                </span>
            </fieldset>
            <fieldset>
                <legend></legend>
                <span class="linha">
                    <label>IR na fonte: </label>
                    <select id="IRFonte" name="IRFonte"><option value="">Carregando...</option></select>
                </span>
                <span class="linha">
                    <label>CSL: </label>
                    <select id="CSL" name="CSL"><option value="">Carregando...</option></select>
                </span>
                <span class="linha">
                    <label>Pis: </label>
                    <select id="PIS" name="PIS"><option value="">Carregando...</option></select>
                </span>
                <span class="linha">
                    <label>Cofins: </label>
                    <select id="COFINS" name="COFINS"><option value="">Carregando...</option></select>
                </span>
                <span class="linha">
                    <label>ISS: </label>
                    <select id="ISS" name="ISS"><option value="">Carregando...</option></select>
                </span>
                <span class="linha">
                    <label>Valor Líquido: </label>
                    <select id="ValorLiquido" name="ValorLiquido"><option value="">Carregando...</option></select>
                </span>
            </fieldset>
            <div class="botoes">
                <input id="btnSalva" value="Salvar Parametros" type="button">
            </div>
</asp:Content>

