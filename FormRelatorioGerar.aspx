<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormRelatorioGerar.aspx.cs" Inherits="FormRelatorioGerar" %>
<%@ MasterType VirtualPath="~/MasterForm.master" %>

<asp:Content ID="_areaConteudo" ContentPlaceHolderID="areaConteudo" runat="server">
    <asp:HiddenField ID="hiddenRelatorio" runat="server" />
    <asp:Panel ID="painelRazao"  runat="server">
        <fieldset>
            <legend>Filtrando por:</legend>
            <span class="linha">
                <label>Periodo de: </label>
                <asp:TextBox ID="textPeriodoDeRAZAO" Width="70px" runat="server"></asp:TextBox>
                <label>Periodo até: </label>
                <asp:TextBox ID="textPeriodoAteRAZAO" Width="70px" runat="server"></asp:TextBox>
            </span>
            <span class="linha">
                <label>Tipo de Moeda (razao): </label>
                <asp:DropDownList ID="comboMoedaRazao" runat="server"></asp:DropDownList> <label></label><asp:CheckBox ID="checkBoxRazao" style="text-align:left;width:190px;" Text="Relatório&nbsp;com&nbsp;duas&nbsp;moedas!" runat="server" />
            </span>
            <script type="text/javascript">
            //<![CDATA[
                $(document).ready(function () {
                    if(<%=consolidaEmpresa %>){
                        var htmlBotaoUm = '<div><label>Selecione a(s) Empresa(s)<br /></label><button type="button"></button></div>';
                        $('h1').after(htmlBotaoUm);

                        $('button').text('Expandir').click(function () {
                        if ($(this).text() == 'Expandir') {
                            $(this).text('Ocultar');
                            $('#Div2').show();
                        } else {
                            $(this).text('Expandir');
                            $('#Div2').hide();
                        }
                        });
                    }
                });
                // ]]>	
            </script>
<h1></h1>
<div id="Div2">
    <table id="table3" class='grid_dados' cellpadding="0" cellspacing="0">
        <tr class='titulo'>
            <td>Selecionar todas Empresa</td>
            <td><input type="checkbox" id="Checkbox2"  /></td>
        </tr>
        <asp:Repeater ID="Repeater2" runat="server">
              <ItemTemplate>
                <tr class='linha'>
                    <td><%# Eval("NOME_RAZAO_SOCIAL") %></td>
                    <td class="check"> <input type="checkbox" grid="1" id="chk" name="check" value='<%# Eval("COD_EMPRESA") %>' runat="server" /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
            <script type="text/javascript">
            //<![CDATA[
                $(document).ready(function () {
                    $('#Div2').hide();
                });
                // ]]>	
            </script>
            <script type="text/javascript">
                $(document).ready(function () {

                    $('input[type=checkbox]').each(function () {
                        if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                        {
                            $(this).attr("checked", true);
                        }
                    });

                    $("#Checkbox2").click(function () {
                            var chkBox = $(this).is(":checked");
                            $("#table3 input[type='checkbox']").attr("checked", chkBox);

                            $('input[type=checkbox]').each(function () {
                                if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                                {
                                    $(this).attr("checked", true);
                                }
                            });
                    });

                    $('input[type=checkbox]').click(function (){
                        $('input[type=checkbox]').each(function () {
                            if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                            {
                                $(this).attr("checked", true);
                            }
                        });
                    });
                });
            </script>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <span class="linha">
                        <label>Conta de: </label>
                        <asp:DropDownList ID="comboContaDeRAZAO" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Conta até: </label>
                        <asp:DropDownList ID="comboContaAteRAZAO" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Divisão de: </label>
                        <asp:DropDownList ID="comboDivisaoDeRAZAO" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Divisão até: </label>
                        <asp:DropDownList ID="comboDivisaoAteRAZAO" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Linha Negócio de: </label>
                        <asp:DropDownList ID="comboLinhaNegocioDeRAZAO" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Linha Negócio até: </label>
                        <asp:DropDownList ID="comboLinhaNegocioAteRAZAO" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Cliente de: </label>
                        <asp:DropDownList ID="comboClienteDeRAZAO" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Cliente até: </label>
                        <asp:DropDownList ID="comboClienteAteRAZAO" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Job de: </label>
                        <asp:DropDownList ID="comboJobDeRAZAO" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Job até: </label>
                        <asp:DropDownList ID="comboJobAteRAZAO" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Terceiro de: </label>
                        <asp:DropDownList ID="comboTerDeRAZAO" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Terceiro até: </label>
                        <asp:DropDownList ID="comboTerAteRAZAO" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </asp:Panel>

    <%--Balancete aki--%>

    <asp:Panel ID="painelBalancete"  runat="server">
        <fieldset>
            <legend>Filtrando por:</legend>
            <span class="linha">
                <label>Periodo: </label>
                <asp:TextBox ID="textPeriodo" Width="50px" runat="server"></asp:TextBox>
            </span>
            <span class="linha">
                <%--Tipo de Moeda - Balancete--%>
                <label>Tipo de Moeda (balancete): </label>
                <asp:DropDownList ID="comboMoedaBalancete" runat="server"></asp:DropDownList> 
                
                <label></label><asp:CheckBox ID="checkBoxBalancete" style="text-align:left;width:190px;" Text="Relatório&nbsp;com&nbsp;duas&nbsp;moedas!" runat="server" />
            </span>
			<span class="linha">
                
            </span>
<script type="text/javascript">
//<![CDATA[
    $(document).ready(function () {
        if(<%=consolidaEmpresa %>){
            var htmlBotaoUm = '<div><label>Selecione a(s) Empresa(s)<br /></label><button type="button"></button></div>';

            $('h1').after(htmlBotaoUm);

            $('button').text('Expandir').click(function () {
                if ($(this).text() == 'Expandir') {
                    $(this).text('Ocultar');
                    $('#tudo').show();
                } else {
                    $(this).text('Expandir');
                    $('#tudo').hide();
                }
            });

            $('#ctl00_areaConteudo_textPeriodo').blur(function(){
                $("#ctl00_areaConteudo_txtAteEncerramento").val($(this).val());
            });
            
            $('#ctl00_areaConteudo_checkContasEncerramento').click(function(){
                if($('#ctl00_areaConteudo_txtAteEncerramento').prop('disabled')){
                    $('#ctl00_areaConteudo_txtAteEncerramento').removeAttr('disabled');
                } else {
                    $('#ctl00_areaConteudo_txtAteEncerramento').attr('disabled','disabled');
                }
            });

            $("#ctl00_areaConteudo_txtAteEncerramento").blur(function(){
                if($(this).val() != ""){
                    var splitDt = $("#ctl00_areaConteudo_textPeriodo").val().split('/');
                    var dt = new Date(splitDt[1],splitDt[0],1);
                
                    var splitDtEnc = $("#ctl00_areaConteudo_txtAteEncerramento").val().split('/');
                    var dtEnc = new Date(splitDtEnc[1],splitDtEnc[0],1);

                    if(dtEnc > dt){
                        alert('A data de encerramento não pode ser maior que a data do período!');
                        $("#ctl00_areaConteudo_txtAteEncerramento").focus();
                    }
                }
            });
        }
    });
    // ]]>	
</script>
<h1></h1>
<div id="tudo">
    <table id="table1" class='grid_dados' cellpadding="0" cellspacing="0">
            <tr class='titulo'>
                <td>Selecionar todas Empresa</td>
                <td><input type="checkbox" id="cbSelectAll"  /></td>
            </tr>
        <asp:Repeater ID="gvempresas" runat="server">
              <ItemTemplate>
                <tr class='linha'>
                    <td><%# Eval("NOME_RAZAO_SOCIAL") %></td>
                    <td class="check"> <input type="checkbox" grid="1" id="chk" name="check" value='<%# Eval("COD_EMPRESA") %>' runat="server" /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
</div>

<script type="text/javascript">
//<![CDATA[
    $(document).ready(function () {
                $('#tudo').hide();
    });
    // ]]>	
</script>
<script type="text/javascript">
    $(document).ready(function () {
    
        $('input[type=checkbox]').each(function () {
            if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
            {
                $(this).attr("checked", true);
            }
        });

        $("#cbSelectAll").click(function () {
                var chkBox = $(this).is(":checked");
                $("#table1 input[type='checkbox']").attr("checked", chkBox);

                $('input[type=checkbox]').each(function () {
                    if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                    {
                        $(this).attr("checked", true);
                    }
                });
        });

        $('input[type=checkbox]').click(function (){
            $('input[type=checkbox]').each(function () {
                if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                {
                    $(this).attr("checked", true);
                }
            });
        });
    });
</script>

















<span class="linha">

</span>
            <span class="linha">
                <label style="display:none;">Considerar lançamentos de Encerramento? </label>
                <asp:CheckBox style="display:none;" ID="checkContasEncerramento" runat="server" Checked="false" />
                <label>Considerar Lançamentos de Encerramentos até: </label>
                <asp:TextBox ID="txtAteEncerramento" Width="50px" runat="server"></asp:TextBox>

				<label style="width:216px"></label><asp:CheckBox ID="checkBoxContasZeradas" style="text-align:left;width:190px;" Text="Mostrar Contas Zeradas" runat="server" />
            </span>
            <script>
                $(document).ready(function(){
                    $("#"+<%=checkContasEncerramento.ClientID %>).click(function(){
                        if($("#<%=txtAteEncerramento.ClientID %>").is(":disabled")){
                            $("#<%=txtAteEncerramento.ClientID %>").removeAttr("disabled");
                        } else {
                            $("#<%=txtAteEncerramento.ClientID %>").attr("disabled","disabled");
                        }
                    });
                });
            </script>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <span class="linha">
                        <label>Divisão de: </label>
                        <asp:DropDownList ID="comboDivisaoDe" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Divisão até: </label>
                        <asp:DropDownList ID="comboDivisaoAte" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Linha Negócio de: </label>
                        <asp:DropDownList ID="comboLinhaNegocioDe" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Linha Negócio até: </label>
                        <asp:DropDownList ID="comboLinhaNegocioAte" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Cliente de: </label>
                        <asp:DropDownList ID="comboClienteDe" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Cliente até: </label>
                        <asp:DropDownList ID="comboClienteAte" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Job de: </label>
                        <asp:DropDownList ID="comboJobDe" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Job até: </label>
                        <asp:DropDownList ID="comboJobAte" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <asp:Panel id="divDetalhe" runat="server">
        <fieldset id="fieldDetalhe" runat="server">
            <legend>Detalhado por:</legend>
            <span class="linha">
                <label>1º</label>
                <asp:DropDownList ID="comboDetalhamento1" Width="220px" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>2º</label>
                <asp:DropDownList ID="comboDetalhamento2" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>3º</label>
                <asp:DropDownList ID="comboDetalhamento3" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>4º</label>
                <asp:DropDownList ID="comboDetalhamento4" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>5º</label>
                <asp:DropDownList ID="comboDetalhamento5" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
        </fieldset>
            </asp:Panel>
    </asp:Panel>

<%--    DRE--%>
    <asp:Panel ID="painelDRE"  runat="server">
        <fieldset>
            <legend>Filtrando por:</legend>
            <span class="linha">
                <label>Periodo de: </label>
                <asp:TextBox ID="textPeriodoDeDRE" Width="50px" runat="server"></asp:TextBox>
                <label>Periodo até: </label>
                <asp:TextBox ID="textPeriodoAteDRE" Width="50px" runat="server"></asp:TextBox>
            </span>
            <span class="linha">
                <label>Tipo de Moeda (dre): </label>
                <asp:DropDownList ID="comboMoedaDRE" runat="server"></asp:DropDownList>
            </span>
<script type="text/javascript">
//<![CDATA[
    $(document).ready(function () {
        if(<%=consolidaEmpresa %>){
            var htmlBotaoUm = '<div><label>Selecione a(s) Empresa(s)<br /></label><button type="button"></button></div>';

            $('h1').after(htmlBotaoUm);

            $('button').text('Expandir').click(function () {
                if ($(this).text() == 'Expandir') {
                    $(this).text('Ocultar');
                    $('#Div1').show();
                } else {
                    $(this).text('Expandir');
                    $('#Div1').hide();
                }
            });
        }
    });
    // ]]>	
</script>

<h1></h1>
<div id="Div1">
    <table id="table2" class='grid_dados' cellpadding="0" cellspacing="0">
            <tr class='titulo'>
                
                <td>Selecionar todas Empresa</td>
                <td><input type="checkbox" id="Checkbox1"  /></td>
            </tr>
        <asp:Repeater ID="Repeater1" runat="server">
              <ItemTemplate>
                <tr class='linha'>
                    <td><%# Eval("NOME_RAZAO_SOCIAL") %></td>
                    <td class="check"> <input type="checkbox" grid="1" id="chk" name="check" value='<%# Eval("COD_EMPRESA") %>' runat="server" /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>


</div>

<script type="text/javascript">
//<![CDATA[
    $(document).ready(function () {
        $('#Div1').hide();
    });
    // ]]>	
</script>





<script type="text/javascript">
    $(document).ready(function () {
        
        $('input[type=checkbox]').each(function () {
            if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
            {
                $(this).attr("checked", true);
            }
        });

        $("#Checkbox1").click(function () {
                var chkBox = $(this).is(":checked");
                $("#table2 input[type='checkbox']").attr("checked", chkBox);

                $('input[type=checkbox]').each(function () {
                    if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                    {
                        $(this).attr("checked", true);
                    }
                });
        });

        $('input[type=checkbox]').click(function (){
            $('input[type=checkbox]').each(function () {
                if($(this).val() == <%=HttpContext.Current.Session["empresa"] %>)
                {
                    $(this).attr("checked", true);
                }
            });
        });
    });
</script>









            <span class="linha">
                <label>Considerar lançamentos de Encerramento? </label>
                <asp:CheckBox ID="checkContasEncerramentoDRE" runat="server" Checked="false" />
            </span>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <span class="linha">
                        <label>Divisão de: </label>
                        <asp:DropDownList ID="comboDivisaoDeDRE" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Divisão até: </label>
                        <asp:DropDownList ID="comboDivisaoAteDRE"  Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Linha Negócio de: </label>
                        <asp:DropDownList ID="comboLinhaNegocioDeDRE" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Linha Negócio até: </label>
                        <asp:DropDownList ID="comboLinhaNegocioAteDRE" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Cliente de: </label>
                        <asp:DropDownList ID="comboClienteDeDRE" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Cliente até: </label>
                        <asp:DropDownList ID="comboClienteAteDRE" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Job de: </label>
                        <asp:DropDownList ID="comboJobDeDRE" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Job até: </label>
                        <asp:DropDownList ID="comboJobAteDRE" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <legend>Detalhado por:</legend>
            <span class="linha">
                <label>1º</label>
                <asp:DropDownList ID="comboDetalhamento1DRE" Width="220px" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>2º</label>
                <asp:DropDownList ID="comboDetalhamento2DRE" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>3º</label>
                <asp:DropDownList ID="comboDetalhamento3DRE" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>4º</label>
                <asp:DropDownList ID="comboDetalhamento4DRE" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>5º</label>
                <asp:DropDownList ID="comboDetalhamento5DRE" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Terceiro"></asp:ListItem>
                </asp:DropDownList>
            </span>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="painelMovFinanc"  runat="server">
        <fieldset>
            <legend>Filtrando por:</legend>
            <span class="linha">
                <label>Periodo de: </label>
                <asp:TextBox ID="textPeriodoDeMov" Width="80px" runat="server"></asp:TextBox>
                <label>Periodo até: </label>
                <asp:TextBox ID="textPeriodoAteMov" Width="80px" runat="server"></asp:TextBox>
            </span>
			<span class="linha" <%=(Request.QueryString["rel"].ToString().Equals("MOV_BANCARIA") ? "" : "style=\"display:none\"")%>>
                <label>Agrupar por:</label>
				<asp:DropDownList ID="ddlAgrupar" Visible="false" runat="server">
				</asp:DropDownList>
			</span>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <span class="linha">
                        <label>Conta de: </label>
                        <asp:DropDownList ID="comboContaDeMov" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Conta até: </label>
                        <asp:DropDownList ID="comboContaAteMov" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <asp:Panel ID="filtrosMov" runat="server">
                    <span class="linha">
                        <label>Divisão de: </label>
                        <asp:DropDownList ID="comboDivisaoDeMov" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Divisão até: </label>
                        <asp:DropDownList ID="comboDivisaoAteMov" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Linha Negócio de: </label>
                        <asp:DropDownList ID="comboLinhaNegocioDeMov" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Linha Negócio até: </label>
                        <asp:DropDownList ID="comboLinhaNegocioAteMov" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Cliente de: </label>
                        <asp:DropDownList ID="comboClienteDeMov" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Cliente até: </label>
                        <asp:DropDownList ID="comboClienteAteMov" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Job de: </label>
                        <asp:DropDownList ID="comboJobDeMov" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Job até: </label>
                        <asp:DropDownList ID="comboJobAteMov" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                        </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset  <%=(Request.QueryString["rel"].ToString().Equals("MOV_BANCARIA") ? "style=\"display:none\"" : "")%>>
            <legend>Detalhado por:</legend>
            <span class="linha">
                <label>1º</label>
                <asp:DropDownList ID="comboDetalhamento1Mov" Width="220px" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>2º</label>
                <asp:DropDownList ID="comboDetalhamento2Mov" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>3º</label>
                <asp:DropDownList ID="comboDetalhamento3Mov" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <span class="linha">
                <label>4º</label>
                <asp:DropDownList ID="comboDetalhamento4Mov" Width="220px" Enabled="false" runat="server">
                    <asp:ListItem Value="0" Text="Escolha" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="DIVISAO" Text="Divisão"></asp:ListItem>
                    <asp:ListItem Value="LINHA_NEGOCIO" Text="Linha de Negócio"></asp:ListItem>
                    <asp:ListItem Value="CLIENTE" Text="Cliente"></asp:ListItem>
                    <asp:ListItem Value="JOB" Text="Job"></asp:ListItem>
                </asp:DropDownList>
            </span>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="painelTitPend"  runat="server">
        <fieldset>
            <legend>Filtrando por:</legend>
            <span class="linha">
                <label>Data: </label>
                <asp:TextBox ID="textPeriodoTitPend" Width="65px" runat="server"></asp:TextBox>
            </span>
            <span class="linha">
                <label>Vencidos à mais de: </label>
                <asp:TextBox ID="textDiasVencimento" Width="65px" runat="server"></asp:TextBox>
                <span>dias</span>
            </span>
            <span class="linha">
                <label>Tipo</label>
                <asp:DropDownList ID="comboCPCR" Width="220px" runat="server">
                    <asp:ListItem Text="Todos" Selected="True" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Contas à Pagar" Value="CAP_INCLUSAO_TITULO"></asp:ListItem>
                    <asp:ListItem Text="Contas à Receber" Value="CAR_INCLUSAO_TITULO"></asp:ListItem>
                </asp:DropDownList>
            </span>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <span class="linha">
                        <label>Conta de: </label>
                        <asp:DropDownList ID="comboContaDeTitPend" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Conta até: </label>
                        <asp:DropDownList ID="comboContaAteTitPend" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Divisão de: </label>
                        <asp:DropDownList ID="comboDivisaoDeTitPend" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Divisão até: </label>
                        <asp:DropDownList ID="comboDivisaoAteTitPend" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Linha Negócio de: </label>
                        <asp:DropDownList ID="comboLinhaNegocioDeTitPend" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Linha Negócio até: </label>
                        <asp:DropDownList ID="comboLinhaNegocioAteTitPend" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Cliente de: </label>
                        <asp:DropDownList ID="comboClienteDeTitPend" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Cliente até: </label>
                        <asp:DropDownList ID="comboClienteAteTitPend" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    <span class="linha">
                        <label>Job de: </label>
                        <asp:DropDownList ID="comboJobDeTitPend" OnSelectedIndexChanged="selecionaAte" Width="220px" runat="server">
                        </asp:DropDownList>
                        <label>Job até: </label>
                        <asp:DropDownList ID="comboJobAteTitPend" Enabled="false" Width="220px" runat="server">
                        </asp:DropDownList>
                    </span>
                    
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset>
            <legend>Ordenando por: </legend>
            <span class="linha">
                <label>Escolha</label>
                <asp:DropDownList ID="comboOrdenacaoTitPend" Width="220px" runat="server">
                    <asp:ListItem Value="DATA" Text="Data de Vencimento"></asp:ListItem>
                    <asp:ListItem Value="TERCEIRO" Text="Cliente/Fornecedor"></asp:ListItem>
                    <asp:ListItem Value="CONTA" Text="Conta Contábil"></asp:ListItem>
                </asp:DropDownList>
            </span>
        </fieldset>
    </asp:Panel>
    <div class="botoes">
        <asp:Button ID="botaoVaiRelatoriosintetico" Visible="false" PostBackUrl="~/FormRelatorioGeradosintetico.aspx" UseSubmitBehavior="true" runat="server" Text="Gerar Relatório Sintético" />
        <asp:Button ID="botaoVaiRelatorio" PostBackUrl="~/FormRelatorioGerado.aspx" UseSubmitBehavior="true" runat="server" Text="Gerar Relatório Analítico" />
        <asp:Button ID="botaoVaiRelatorioLayout2" Visible="false" PostBackUrl="~/FormRelatorioGerado.aspx" UseSubmitBehavior="true" runat="server" Text="Gerar Relatório Analítico - Layout 2" OnClick="botaoVaiRelatorio_Click" />
    </div>
</asp:Content>
