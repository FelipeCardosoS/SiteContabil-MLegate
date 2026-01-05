<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormAnaliseJobs.aspx.cs" Inherits="FormAnaliseJobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(function () {
            $(document).on('change', '.comboDivisao', function (e) {
                var cod_divisao = $(this).val();
                carregalinhaNegocio(cod_divisao, this);                
            });
        });

        function carregalinhaNegocio(cod_divisao, cb) {
            $.ajax({
                type: "POST",
                url: "FormAnaliseJobs.aspx/loadlinhaNegocio",
                data: "{ cod_divisao: " + cod_divisao + " }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Carregando Linha de Negócio..</font>");
                },
                success: function (res) {
                    $(cb).closest('tr').find('.comboLinhaNegocio option').remove();
                    for (var i = 0; i < res.d.length; i++) {
                        $(cb).closest('tr').find('.comboLinhaNegocio').append('<option value="' + res.d[i].Cod_Linha_Negocio + '">' + res.d[i].Descricao + '</option>');
                    }
                },
                complete: function (result) {
                    $(".legendaForm").html("Informe os dados abaixo");
                },
                error: function (err) {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Erro fatal na sincronização.</font>");
                }
            });
        }

        function carregaJobs(){
            $.ajax({
                type: "POST",
                url: "FormAnaliseJobs.aspx/carregaJobsAnalise",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Carregando Jobs..</font>");
                },
                success: function (result) {
                    var row = $('tbody').html();

                    for(var x = 1; x < result.d.length; x++ ){
                        $('tbody tr:last-child').after(row);
                    }

                    for (var y = 0; y < result.d.length; y++) {
                        $("tbody tr:eq(" + y + ")").find(".cod_ref").html(result.d[y].cod_referencia);
                        $("tbody tr:eq(" + y + ")").find(".txtNome").val(result.d[y].nome);
                        $("tbody tr:eq(" + y + ")").find(".txtDescricao").val(result.d[y].descricao);
                        $("tbody tr:eq(" + y + ")").find(".comboCliente").val(result.d[y].cliente);
                        $("tbody tr:eq(" + y + ")").find(".comboLinhaNegocio").val(result.d[y].linhaNegocio);
                        $("tbody tr:eq(" + y + ")").find(".comboDivisao").val(result.d[y].divisao);
                    }
                },
                complete: function (result) {
                    $(".legendaForm").html("Informe os dados abaixo");
                },
                error: function (err) {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Erro fatal na sincronização.</font>");
                }
            });
        }

        function salva()
        {
            var Jobs = [];

            $("tbody tr").each(function () {
                var Job = {
                    nome: $(this).find(".txtNome").val(),
                    cod_referencia: $(this).find(".cod_ref").html(),
                    descricao: $(this).find(".txtDescricao").val(),
                    cliente: $(this).find(".comboCliente").val(),
                    linhaNegocio: $(this).find(".comboLinhaNegocio").val(),
                    divisao: $(this).find(".comboDivisao").val()
                }
                Jobs.push(Job);
            });
            Jobs = JSON.stringify({ 'Jobs': Jobs });

            $.ajax({
                type: "POST",
                url: "FormAnaliseJobs.aspx/salvar",
                data: Jobs,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Salvando Jobs..</font>");
                },
                success: function (result) {
                    if (result.d.length == 0){
                        alert('Job(s) salvos com sucesso!');
                        window.location.replace("Default.aspx");
                    } else {
                        var erros = "";
                        for (var y = 0; y < result.d.length; y++)
                        {
                            erros += result.d[y] + "\r";
                        }
                        alert(erros);
                    }
                },
                complete: function (result) {
                    $(".legendaForm").html("Informe os dados abaixo");
                },
                error: function (err) {
                    $(".legendaForm").html("Informe os dados abaixo - <font color=\"#FF0000\">Erro fatal não foi possível salvar os Jobs! - "+err.responseText+"</font>");
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <asp:Literal ID="ltMensagem" Visible="false" runat="server"></asp:Literal>
    <asp:Panel ID="Painel" Visible="false" runat="server">
        <div id="conteudo">
            <div class="legendaForm">Informe os dados abaixo</div>
            <fieldset>
                <table>
                    <thead>
                        <tr>
                            <td>Nome</td>
                            <td>Descrição</td>
                            <td>Cliente</td>
                            <td>Divisão</td>
                            <td>Linha de Negócio</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><span id="cod_ref" class="cod_ref" style="display:none;"></span><asp:TextBox ID="txtNome" CssClass="txtNome" runat="server"></asp:TextBox></td>
                            <td><asp:TextBox ID="txtDescricao" CssClass="txtDescricao" runat="server"></asp:TextBox></td>
                            <td><asp:DropDownList ID="comboCliente" CssClass="comboCliente" style="width: 220px;" runat="server"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="comboDivisao" runat="server" CssClass="comboDivisao" >
                                </asp:DropDownList>
                            </td>
                            <td><asp:DropDownList ID="comboLinhaNegocio" CssClass="comboLinhaNegocio" runat="server"></asp:DropDownList></td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
            <div id="areaBotoes" class="botoes" runat="server">
                <asp:Button ID="botaoSalvar" OnClientClick="salva();return false;" runat="server" Text="Salvar" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

