<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormDadosResponsavel.aspx.cs" Inherits="FormDadosResponsavel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <legend>Preencha os campos abaixo:</legend>
        <span class="linha">
            <label>Nome Completo</label>
            <asp:TextBox ID="textNome" runat="server" Width="240px" />
            <asp:RequiredFieldValidator ID="nomeValidator" runat="server" Display="None" ControlToValidate="textNome" ErrorMessage="Informe o Nome Completo." />
            <toolKit:ValidatorCalloutExtender ID="nomeAjaxValidator" runat="server" TargetControlID="nomeValidator" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha">
            <label>CPF</label>
            <asp:TextBox ID="textCpf" runat="server" Width="120px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" ControlToValidate="textCpf" ErrorMessage="Informe o CPF." />
            <toolKit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1" CssClass="customCalloutStyle" WarningIconImageUrl="Imagens/pixel.gif" HighlightCssClass="validacaoErro" />
        </span>
        <span class="linha">
            <label>CRC</label>
            <asp:TextBox ID="textCrc" runat="server" Width="120px" />
        </span>
        <span class="linha">
            <label>CNPJ Escritório</label>
            <asp:TextBox ID="textCnpjEscritorio" runat="server" Width="160px" />
        </span>
        <span class="linha">
            <label>Endereço</label>
            <asp:TextBox ID="textEndereco" runat="server" Width="260px" />
            <label style="width:30px;">Nº</label>
            <asp:TextBox ID="textNumero" runat="server" Width="50px" />
        </span>
        <span class="linha">
            <label>Complemento</label>
            <asp:TextBox ID="textComplemento" runat="server" Width="180px" />
        </span>
        <span class="linha">
            <label>CEP</label>
            <asp:TextBox ID="textCep" runat="server" Width="100px" />
        </span>
        <span class="linha">
            <label>Bairro</label>
            <asp:TextBox ID="textBairro" runat="server" Width="180px" />
        </span>
        <span class="linha">
            <label>Telefone</label>
            <asp:TextBox ID="textTelefone" runat="server" Width="90px" />
        </span>
        <span class="linha">
            <label>Celular</label>
            <asp:TextBox ID="textCelular" runat="server" Width="90px" />
        </span>
        <span class="linha">
            <label>E-mail</label>
            <asp:TextBox ID="textEmail" runat="server" Width="140px" />
        </span>
        <span class="linha">
            <label>Qualificação do Assinante</label>
            <asp:TextBox ID="textIdentQualif" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Código de Qualificação</label>
            <asp:TextBox ID="textCodAssi" runat="server" Width="80px" MaxLength="3" />
        </span>
        <span class="linha">
            <label>Código Município (IBGE)</label>
            <asp:TextBox ID="textCodigoMunicipio" runat="server" Width="80px" MaxLength="9" />
        </span>
        <toolKit:FilteredTextBoxExtender ID="ftbeCodigoMunicipio" TargetControlID="textCodigoMunicipio" FilterType="Numbers" runat="server" />
        <span class="linha">
            <label>UF CRC</label>
            <asp:TextBox ID="textUfCrc" MaxLength="2" runat="server" Width="20px" />
        </span>
        <toolKit:FilteredTextBoxExtender ID="ftbeUfCrc" runat="server" FilterType="UppercaseLetters"  TargetControlID="textUfCrc" />
        <span class="linha">
            <label>Número Sequencial (UF/Ano/Número)</label>
            <asp:TextBox ID="textUFSequencial" runat="server" Width="80px" />
        </span>
        <span class="linha">
            <label>Data de Validade (CRC)</label>
            <asp:TextBox ID="dtValidadeCrc" MaxLength="10" runat="server" Width="80px" />
        </span>
        <div class="botoes">
            <asp:Button ID="botaoSalvar" OnClick="botaoSalvar_Click" runat="server" Text="Salvar" />
        </div>
    </fieldset>
</asp:Content>