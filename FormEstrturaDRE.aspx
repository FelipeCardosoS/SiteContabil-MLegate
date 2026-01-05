<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormEstrturaDRE.aspx.cs" Inherits="FormEstrturaDRE" %>

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
                <label class="control-label col-sm-3 ">Codigo DRE:</label>
                <div class="input-group col-sm-4">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                    <input id="txtcodigoDRE" required class="form-control" placeholder="Codigo DRE" />
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

        <table id="tableDRE" class="table table-striped table-hover">
            <thead>
                <tr>
                    <th>Codigo</th>
                    <th>Descrição</th>
                    <th>Analítica</th>
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
                    url: "FormEstrturaDRE.aspx/CarregaEstruturaDRE",
                    type: "POST",
                    data: "{ }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: beforeSendFunction("Aguarde carregando..."),
                    success: function (ret) {
                        $(ret.d).each(function () {
                            $('#tableDRE').append(linha(this.Cod_DRE, this.Descricao, this.Analitica, this.Nivel))
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

                var DRE = {
                    Cod_DRE: $('#txtcodigoDRE').val(),
                    Descricao: $('#txtdescricao').val(),
                    Analitica: $('#cbanalitica').val(),
                    Nivel: $('#txtnivel').val()
                }

                jsonDRE = JSON.stringify(DRE);
                var existe = false;
                $('table .cod_dre').each(function () {
                    if ($(this).html() == DRE.Cod_DRE) {
                        existe = true;
                    }
                });

                if (existe) {
                    $.alert('Esse código DRE já existe.');
                } else {
                    $.ajax({
                        url: "FormEstrturaDRE.aspx/Salva",
                        type: "POST",
                        data: "{ estruturadre:" + jsonDRE + " }",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: beforeSendFunction("Aguarde salvando..."),
                        success: function (ret) {
                            if (ret.d != "") {
                                $.alert(ret.d);
                            }
                            else {
                                divalert("Processo efetuado com sucesso.", "alert-success");

                                $('#tableDRE').append(linha(DRE.Cod_DRE, DRE.Descricao, $('#cbanalitica option:selected').text(), DRE.Nivel))
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
                var cod_dre = row.children(".cod_dre").html();

                if ($(this).is(".up")) {
                    $.ajax({
                        url: "FormEstrturaDRE.aspx/uP",
                        type: "POST",
                        data: "{ Cod_DRE:'" + cod_dre + "' }",
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
                        url: "FormEstrturaDRE.aspx/Down",
                        type: "POST",
                        data: "{ Cod_DRE:'" + cod_dre + "' }",
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
                var DRE = {
                    Cod_DRE: $('#txtcodigoDRE').val(),
                    Descricao: $('#txtdescricao').val(),
                    Analitica: $('#cbanalitica').val(),
                    Nivel: $('#txtnivel').val()
                }
                jsonDRE = JSON.stringify(DRE);
                $.ajax({
                    url: "FormEstrturaDRE.aspx/Editar",
                    type: "POST",
                    data: "{ estruturadre:" + jsonDRE + " }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: beforeSendFunction("Aguarde salvando..."),
                    success: function (ret) {
                        if (ret.d != "") {
                            $.alert(ret.d);
                        }
                        else {
                            divalert("Processo efetuado com sucesso.", "alert-success");

                            $('table .cod_dre').each(function () {
                                if ($(this).html() == DRE.Cod_DRE) {
                                    var row = $(this).closest('tr');
                                    var newrow = $(linha(DRE.Cod_DRE, DRE.Descricao, $('#cbanalitica option:selected').text(), DRE.Nivel));
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

            function editar(cod_dre, descricao, analitica, nivel) {
                $("#btnadd").hide();
                $("#btneditar").show();
                $("#btncancelar").show();

                $("#txtcodigoDRE").prop('disabled', true);

                $('#txtcodigoDRE').val(cod_dre);
                $('#txtdescricao').val(descricao);
                if (analitica = 'Sim') {
                    $('#cbanalitica').val('S');
                } else if (analitica = 'Não') {
                    $('#cbanalitica').val('N');
                }
                $('#txtnivel').val(nivel);

                $("html, body").animate({ scrollTop: 0 }, "slow");
            }

            function deletar(cod_dre, descricao) {
                $.confirm({
                    title: false,
                    content: 'O ' + descricao + ' será excluido.',
                    keyboardEnabled: true,
                    animation: 'left',
                    buttons: {
                        Ok: function () {
                            $.ajax({
                                url: "FormEstrturaDRE.aspx/Deletar",
                                type: "POST",
                                data: "{ Cod_DRE:'" + cod_dre + "' }",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: beforeSendFunction("Aguarde Deletando..."),
                                success: function (ret) {
                                    if (ret.d != "") {
                                        $.alert(ret.d);
                                    }
                                    else {
                                        divalert("Processo efetuado com sucesso.", "alert-success");

                                        $('table .cod_dre').each(function () {
                                            if ($(this).html() == cod_dre) {
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

                $("#txtcodigoDRE").prop('disabled', false);

                $('#txtcodigoDRE').val('');
                $('#txtdescricao').val('');
                $('#cbanalitica').val('S');
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

            function linha(Cod_DRE, Descricao, Analitica, Nivel) {
                return '<tr>'
                        + '<td class="cod_dre">' + Cod_DRE + '</td>'
                        + '<td>' + Descricao + '</td>'
                        + '<td>' + Analitica + '</td>'
                        + '<td>' + Nivel + '</td>'
                        + '<td><a href="javascript: editar(\'' + Cod_DRE + '\', \'' + Descricao + '\', \'' + Analitica + '\' , \'' + Nivel + '\')" class="btntr"><i title="Editar" class="fa fa-pencil-square-o fnteditar" aria-hidden="true"></i></a>'
                        + '<a href="javascript: deletar(\'' + Cod_DRE + '\', \'' + Descricao + '\')" class="btntr"><i title="Deletar" class="fa fa-times fntdeletar" aria-hidden="true"></i></a>'
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

