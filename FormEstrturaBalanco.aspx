<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEstrturaBalanco.aspx.cs" Inherits="FormEstrturaBalanco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Js/maskMoney.js"></script>
    <script type="text/javascript" src="Js/jquery.confirm.min.js"></script>
    <script type="text/javascript" src="Js/toggleLoading.jquery.js"></script>
    <script type="text/javascript" src="Js/bootstrap-toggle.min.js"></script>
    <style type="text/css"> @import url(Css/bootstrap-toggle.min.css); </style>
    <style type="text/css"> @import url(Css/bootstrap.min.css); </style>
    <style type="text/css"> @import url(Css/font-awesome.min.css </style>
    <style type="text/css"> @import url(Css/fonts-googleapis.css); </style>
    <style type="text/css"> @import url(Css/jquery-confirm.css); </style>
    <style type="text/css"> @import url(Css/loading.css); </style>
</asp:Content>


<asp:Content ID="areaConteudo" ContentPlaceHolderID="areaConteudo" runat="Server">
    <fieldset>
        <legend>Preencha os campos abaixo:</legend>
        <div class="col-md-6 column">
            <div class="form-group row">
                <label class="control-label col-sm-3 ">Codigo Balanço:</label>
                <div class="input-group col-sm-4">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                    <input id="txtcodigobalanco" required class="form-control" placeholder="Codigo Balanço" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-sm-3 ">Descrição:</label>
                <div class="input-group col-sm-8">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                    <input id="txtdescricao" required class="form-control" placeholder="Descrição" />
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-sm-3 ">Analítica:</label>
                <div class="input-group col-sm-4">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                    <select class="form-control" id="cbanalitica">
                        <option value="S">Sim</option>
                        <option value="N">Não</option>
                    </select>
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-sm-3 ">Tipo:</label>
                <div class="input-group col-sm-4">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                    <select class="form-control" id="cbtipo">
                        <option value="A">Ativo</option>
                        <option value="P">Passivo</option>
                    </select>
                </div>
            </div>
            <div class="form-group row">
                <label class="control-label col-sm-3 ">Nível:</label>
                <div class="input-group col-sm-4">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                    <input id="txtnivel" required class="form-control" placeholder="Nível" />
                </div>
            </div>
            <div class="form-group row">
                <div class="input-group col-sm-12">
                    <div class="col-sm-3">
                        <input id="btnadd" type="submit" value="Adicionar" class="form-control"/>
                    </div>
                    <div class="col-sm-3">
                        <input id="btneditar" type="button" value="Editar" class="form-control" style="display:none;" onclick="funceditar()"/>
                    </div>
                    <div class="col-sm-3">
                        <input id="btncancelar" type="button" value="Cancelar" class="form-control" style="display:none;" onclick="limpar()"/>
                    </div>
                </div>
            </div>
        </div>

        <div class="row rowotimizar rowerros col-sm-12">            
        </div>

        <table id="tableBalanco" class="table table-striped table-hover">
            <thead>
                <tr>
                    <th>Codigo</th>
                    <th>Descrição</th>
                    <th>Analítica</th>
                    <th>Tipo</th>
                    <th>Nivel</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>                
            </tbody>
        </table>

        <script>
            $(document).ready(function () {
                $.ajax({
                    url: "FormEstrturaBalanco.aspx/CarregaEstruturaBalanco",
                    type: "POST",
                    data: "{ }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: beforeSendFunction("Aguarde carregando..."),
                    success: function (ret) {
                        $(ret.d).each(function () {
                            $('#tableBalanco').append(linha(this.Cod_Balanco, this.Descricao, this.Analitica, this.Ativo_Passivo, this.Nivel))
                        });
                    },
                    complete: function () {
                        aguarde();
                    },
                    error: errorfunction
                });                                
            });

            var $contactForm = $('form');
            $contactForm.on('submit', function (ev) {
                ev.preventDefault();

                var Balanco = {
                    Cod_Balanco: $('#txtcodigobalanco').val(),
                    Descricao: $('#txtdescricao').val(),
                    Analitica: $('#cbanalitica').val(),
                    Ativo_Passivo: $('#cbtipo').val(),
                    Nivel: $('#txtnivel').val()
                }

                jsonBalanco = JSON.stringify(Balanco);
                var existe = false;
                $('table .cod_balanco').each(function () {
                    if ($(this).html() == Balanco.Cod_Balanco) {
                        existe = true;
                    }
                });

                if (existe) {
                    $.alert('Esse código Balanço já existe.');
                } else {
                    $.ajax({
                        url: "FormEstrturaBalanco.aspx/Salva",
                        type: "POST",
                        data: "{ estruturabalanco:" + jsonBalanco + " }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: beforeSendFunction("Aguarde salvando..."),
                        success: function (ret) {
                            if (ret.d != "") {
                                $.alert(ret.d);
                            }
                            else {
                                divalert("Processo efetuado com sucesso.", "alert-success");

                                $('#tableBalanco').append(linha(Balanco.Cod_Balanco, Balanco.Descricao, $('#cbanalitica option:selected').text(), $('#cbtipo option:selected').text(), Balanco.Nivel))
                            }                            
                        },
                        complete: function () {
                            aguarde();
                        },
                        error: errorfunction
                    });
                }
            });       

            $('table tbody').on('click', '.up,.down', function () {
                var row = $(this).parents("tr:first");
                var cod_balanco = row.children(".cod_balanco").html();

                if ($(this).is(".up")) {
                    $.ajax({
                        url: "FormEstrturaBalanco.aspx/uP",
                        type: "POST",
                        data: "{ Cod_Balanco: '" + cod_balanco + "' }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: beforeSendFunction("Aguarde salvando..."),
                        success: function (ret) {
                            if (ret.d != "") {
                                $.alert(ret.d);
                            }
                            else {
                                row.insertBefore(row.prev());
                            }
                        },
                        complete: function () {
                            aguarde();
                        },
                        error: errorfunction
                    });                   
                } else {
                    $.ajax({
                        url: "FormEstrturaBalanco.aspx/Down",
                        type: "POST",
                        data: "{ Cod_Balanco: '" + cod_balanco + "' }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: beforeSendFunction("Aguarde salvando..."),
                        success: function (ret) {
                            if (ret.d != "") {
                                $.alert(ret.d);
                            }
                            else {
                                row.insertAfter(row.next());
                            }
                        },
                        complete: function () {
                            aguarde();
                        },
                        error: errorfunction
                    });
                }
            });

            function funceditar() {
                var Balanco = {
                    Cod_Balanco: $('#txtcodigobalanco').val(),
                    Descricao: $('#txtdescricao').val(),
                    Analitica: $('#cbanalitica').val(),
                    Ativo_Passivo: $('#cbtipo').val(),
                    Nivel: $('#txtnivel').val()
                }
                jsonBalanco = JSON.stringify(Balanco);
                $.ajax({
                    url: "FormEstrturaBalanco.aspx/Editar",
                    type: "POST",
                    data: "{ estruturabalanco:" + jsonBalanco + " }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: beforeSendFunction("Aguarde salvando..."),
                    success: function (ret) {
                        if (ret.d != "") {
                            $.alert(ret.d);
                        }
                        else {
                            divalert("Processo efetuado com sucesso.", "alert-success");

                            $('table .cod_balanco').each(function () {
                                if ($(this).html() == Balanco.Cod_Balanco) {
                                    var row = $(this).closest('tr');
                                    var newrow = $(linha(Balanco.Cod_Balanco, Balanco.Descricao, $('#cbanalitica option:selected').text(), $('#cbanalitica option:selected').text(), Balanco.Nivel)); 
                                    newrow.insertBefore(row);
                                    row.remove();
                                    return false;
                                }
                            });

                            limpar();
                        }
                    },
                    complete: function () {
                        aguarde();
                    },
                    error: errorfunction
                });
            }                    
            
            function editar(cod_balanco, descricao, analitica, tipo, nivel) {
                $("#btnadd").hide();
                $("#btneditar").show();
                $("#btncancelar").show();

                $("#txtcodigobalanco").prop('disabled', true);

                $('#txtcodigobalanco').val(cod_balanco);
                $('#txtdescricao').val(descricao);
                if (analitica = 'Sim') {
                    $('#cbanalitica').val('S');
                } else if (analitica = 'Não') {
                    $('#cbanalitica').val('N');
                }
                if (tipo = 'Ativo') {
                    $('#cbtipo').val('A');
                } else if (analitica = 'Passivo') {
                    $('#cbtipo').val('P');
                }
                $('#txtnivel').val(nivel);

                $("html, body").animate({ scrollTop: 0 }, "slow");
            }

            function deletar(cod_balanco, descricao) {
                $.confirm({
                    title: false,
                    content: 'O ' + descricao + ' será excluido.',
                    keyboardEnabled: true,
                    animation: 'left',
                    buttons: {
                        Ok: function () {
                            $.ajax({
                                url: "FormEstrturaBalanco.aspx/Deletar",
                                type: "POST",
                                data: "{ Cod_Balanco:'" + cod_balanco + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: beforeSendFunction("Aguarde Deletando..."),
                                success: function (ret) {
                                    if (ret.d != "") {
                                        $.alert(ret.d);
                                    }
                                    else {
                                        divalert("Processo efetuado com sucesso.", "alert-success");

                                        $('table .cod_balanco').each(function () {
                                            if ($(this).html() == cod_balanco) {
                                                $(this).closest('tr').remove();
                                                return false;
                                            }
                                        });
                                    }                                    
                                },
                                complete: function () {
                                    aguarde();
                                },
                                error: errorfunction
                            });
                        },
                        Cancelar: function () {
                            
                        }
                    }
                });
            }

            function limpar() {
                $("#btnadd").show();
                $("#btneditar").hide();
                $("#btncancelar").hide();

                $("#txtcodigobalanco").prop('disabled', false);

                $('#txtcodigobalanco').val('');
                $('#txtdescricao').val('');
                $('#cbanalitica').val('S');
                $('#cbtipo').val('A');
                $('#txtnivel').val('');
            }

            function beforeSendFunction(txt) {
                aguarde(txt);
            }
            function errorfunction() {
                $.alert('Erro atualize a página e tente novamente.');
            }

            function aguarde(txt) {
                $('#conteudo').toggleLoading({
                    addClass: "my-class",
                    addText: txt
                });
            }

            function linha(Cod_Balanco, Descricao, Analitica, Ativo_Passivo, Nivel) {
                return '<tr>'
                        + '<td class="cod_balanco">' + Cod_Balanco + '</td>'
                        + '<td>' + Descricao + '</td>'
                        + '<td>' + Analitica + '</td>'
                        + '<td>' + Ativo_Passivo + '</td>'
                        + '<td>' + Nivel + '</td>'
                        + '<td><a href="javascript: editar(\'' + Cod_Balanco + '\', \'' + Descricao + '\', \'' + Analitica + '\' , \'' + Ativo_Passivo + '\' , \'' + Nivel + '\')" class="btntr"><i title="Editar" class="fa fa-pencil-square-o fnteditar" aria-hidden="true"></i></a>'
                        + '<a href="javascript: deletar(\'' + Cod_Balanco + '\', \'' + Descricao + '\')" class="btntr"><i title="Deletar" class="fa fa-times fntdeletar" aria-hidden="true"></i></a>'
                        + '<a href="javascript: up()" class="btntr up"><i title="Subir" class="fa fa-arrow-up fntup" aria-hidden="true"></i></a>'
                        + '<a href="javascript: down()" class="btntr down"><i title="Descer" class="fa fa-arrow-down fntdown" aria-hidden="true"></i></a></td>'
                        + '</tr>';
            }

            function divalert(texto, tipo) {
                $('.rowerros').append('<div class="col-md-11 divalert column alert ' + tipo + '" >' + texto + '</div>');
                $('.divalert:last').delay(6000).slideUp(400);
            }
           
        </script>

    </fieldset>
</asp:Content>
