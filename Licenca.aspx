<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="auth" CodeFile="Licenca.aspx.cs" Inherits="Licenca" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Licenças - Sistema Contábil</title>
    <style type="text/css">@import url(Css/Basico.css);</style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bloco">
            <div id="areaDescricao">
                <h1>Licenças</h1>
                <p>
                    Para utilizar o sistema, informe a chave de licença
                    para o mês vigente. Caso não possua por favor entre
                    em contato com o Administrador.
                </p>
            </div>
            <div id="areaForm">
                <fieldset>
                    <span class="linha">
                        <label>Empresa </label>
                        <asp:DropDownList ID="comboEmpresa" Width="152px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Mês </label>
                        <asp:TextBox ID="textMes" MaxLength="2" runat="server" Width="142px"></asp:TextBox>
                    </span>
                    <span class="linha">
                        <label>Ano </label>
                        <asp:TextBox ID="textAno" MaxLength="4" runat="server" Width="142px"></asp:TextBox>
                        
                    </span>
                    <span class="linha">
                        <label>Chave </label>
                        <asp:TextBox ID="textChave" runat="server" Width="142px"></asp:TextBox>
                        
                    </span>
                    <asp:Button ID="botaoSalvar" CssClass="botao direita" OnClick="botaoSalvar_Click" runat="server" Text="Salvar" />
                </fieldset>
            </div>
        </div>
    </form>
</body>
</html>
