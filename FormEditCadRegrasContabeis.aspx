<%@ Page Title="" Language="C#" MasterPageFile="~/MasterEditCad.master" StylesheetTheme="principal"  AutoEventWireup="true" 
    CodeFile="FormEditCadRegrasContabeis.aspx.cs" Inherits="FormEditCadRegrasContabeis" %>

<%@ MasterType VirtualPath="~/MasterEditCad.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Plugins/chosen.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Plugins/chosen.jquery.js"></script>
    <script type="text/javascript" src="Plugins/chosen.proto.js"></script>
	<script type="text/javascript" src="Js/select2.min.js"></script>
    <script type="text/javascript" src="Js/CadRegrasContabeis.js"></script>
    <script type="text/javascript" src="Js/validacao.js"></script>
    <script type="text/javascript" src="Js/principal.js"></script>   

</asp:Content>
<asp:Content ID="_areaForm" ContentPlaceHolderID="areaForm" runat="Server">
    <div class="legendaForm">Regras Contábeis</div>
    <fieldset>
		<asp:HiddenField ID="hdContas" runat="server" />
		<asp:HiddenField ID="hdTerceiros" runat="server" />
		<asp:HiddenField ID="hdRegras" runat="server" />
		<asp:HiddenField ID="hdRegrasNovo" runat="server" />
		<asp:HiddenField ID="hdRegrasDeletar" runat="server" />
        <span class="linha">
            <label>Conta de Despesa</label>
            <asp:DropDownList ID="ddlContaDespesa" Width="180px" runat="server"></asp:DropDownList>
        </span>
        <span class="linha">
            <label>Conta de Fornecedor</label>
            <asp:DropDownList ID="ddlContaFornecedor" Width="180px" runat="server"></asp:DropDownList>
        </span>
    </fieldset>

    <div class="legendaForm">Contas de Descontos</div>
     <fieldset>
         <asp:UpdatePanel ID="UpRegras" ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
                <div id="grid">
                    
                   <table id="tableRegras" cellpadding="0" cellspacing="0" style="margin-top:30px; border:1px solid #B0C4DE" class="Visivel">
                            <thead>
                                <tr class="titulo">
                                    <th style="display:none"></th>
                                    <th style="display:none"></th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal"></th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Conta</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Valor</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">%/R$</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Dia Vencimento</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Terceiro</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Histórico</th>
                                    <th style="color:blue; border:1px solid #B0C4DE; font-weight:normal">Gera Título</th>
                                    <th></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>

                            </tbody>
                        </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </fieldset>
    </asp:Content>


