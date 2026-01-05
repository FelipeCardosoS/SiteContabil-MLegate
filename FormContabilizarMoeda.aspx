<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormContabilizarMoeda.aspx.cs" Inherits="FormContabilizarMoeda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_areaConteudo_txtData").mask("99/9999");

            if (_GET('error') == '0') {
                alert('Período contabilizado com sucesso!');
            }
            else if (_GET('error') == '1')
            {
                alert(_GET('msg'));
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <fieldset>
        <legend>Informe os dados abaixo:</legend>
        <span class="linha">
            <label>Moeda</label>
            <asp:DropDownList ID="comboMoeda" runat="server"></asp:DropDownList>
        </span>
        <span class="linha">
            <label>Período</label>
            <asp:TextBox ID="txtData" runat="server" Width="80px" />
         </span>
        <span class="linha">
            <label></label>
            <asp:Button ID="btnContabilizar" runat="server" Text="Contabilizar" 
            onclick="btnContabilizar_Click" />
         </span>
    </fieldset>
</asp:Content>

