<%@ Page Language="C#" StylesheetTheme="auth" AutoEventWireup="true" CodeFile="Auth.aspx.cs" Inherits="Auth" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">
 <meta content="text/html; charset=utf-8">
    <title>Autenticação - Sistema Contábil</title>
    <style type="text/css">@import url(Css/Basico.css);</style>
    <script type="text/javascript" src="Js/Jquery.js"></script>
    <script type="text/javascript" src="Js/w2ui.js"></script>
    <script src='https://www.google.com/recaptcha/api.js'></script> <%--Comentar esta linha para publicar na G5--%>
</head>
<body>
    <div id="topo_auth">
        <a href="http://www.mlegate.com.br/">
            <img src="Imagens/site/logotipo.png" class="img_lgmlegate" />
        </a>
        <img src="Imagens/site/morison.png" class="img_morison" />
    </div>
    <div id="loginbox">
        <form id="form_principal" runat="server">
            <div class="rowerros"></div>
            <div id="bloco">
                <div id="areaDescricao">
                    <h1>Autenticação</h1>
                    <p>
                        Informe seus dados nos campos ao lado para
                   ter acesso ao nosso sistema.
                    </p>
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <div id="areaForm">
                    <fieldset>
                        <span class="linha">
                            <label>Login </label>
                            <asp:TextBox CssClass="textLogin" ID="textLogin" runat="server" AutoPostBack="true"
                                OnTextChanged="textLogin_TextChanged"></asp:TextBox>
                        </span>
                        <span class="linha">
                            <label>Senha </label>
                            <asp:TextBox CssClass="textSenha" ID="textSenha" TextMode="Password" runat="server" Width="82px"></asp:TextBox>
                        </span>
                        <span class="linha">
                            <label>Empresa </label>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList CssClass="comboEmpresa" ID="comboEmpresa" Width="290" runat="server">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="textLogin" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </span>
                        <span class="linha">
                            <div class="g-recaptcha" style="margin-left: 70px;" data-sitekey="<%= recaptcha %>"></div> <%--Comentar esta linha para publicar na G5--%>
                        </span>
                        <span class="linha">
                            <label></label>
                            <input type="submit" name="botaoLogar" value="Logar" id="botaoLogar" class="botao" />
                        </span>
                    </fieldset>
                </div>
            </div>
        </form>
    </div>
    <script>

        $(document).on('submit', '#form_principal', function (event) {
            event.preventDefault();

            var v = grecaptcha.getResponse(); //Comentar esta linha para publicar na G5

            if (v.length == 0) //Comentar esta linha para publicar na G5
                alertshow("Confirme que você não é um robo."); //Comentar esta linha para publicar na G5

            if (v.length != 0) { //Comentar esta linha para publicar na G5
                var login = $(".textLogin").val();
                var senha = $(".textSenha").val();
                var empresa = $(".comboEmpresa").val();
                var url = "Auth.aspx/Autentica";
                $.ajax({
                    url: url,
                    type: "POST",
                    data: "{ login:'" + login + "', senha:'" + senha + "', empresa:" + empresa + " }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: successFunction,
                    error: function (err) {
                        alertshow('Erro atualize a pagina e tente novamente');
                    }
                });
            } //Comentar esta linha para publicar na G5
        });

    function successFunction(msg) {
        msg = msg.d;
        if (msg.substring(0, 1) == "/")
            window.location.href = '..' + msg; //Comentar esta linha para publicar na G5
            //window.location.href = msg.substring(1); //Descomentar esta linha para publicar na G5
        else
            alertshow(msg);
    }

    function alertshow(texto) {
        $('.rowerros').append('<div class="alertbox">' + texto + '</div>');
        $('.alertbox:last').delay(6000).slideUp(400);
    }

    </script>
</body>
</html>