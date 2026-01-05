<%@ Page Language="C#" MasterPageFile="~/MasterForm.master" StylesheetTheme="principal" AutoEventWireup="true" CodeFile="FormImportContato.aspx.cs" Inherits="FormImportContato" %>

<asp:Content ID="areaForm" ContentPlaceHolderID="areaConteudo" Runat="Server">
    <div class="legendaForm">Informe os dados para importação</div>
    <fieldset class="aki">

        
        <span class="linha">
        <label>Job</label>
        <asp:DropDownList runat="server" ID='DDLjob'></asp:DropDownList>
        </span>

        <span class="linha">
        <label>Dinheiro/Cheque</label>
        <asp:DropDownList runat="server" ID='DDLcontadinheiro'></asp:DropDownList>
        </span>

        <span class="linha">
        <label>Cartão</label>
        <asp:DropDownList runat="server" ID='DDLcontacartao'></asp:DropDownList>
        </span>

        <span class="linha">
        <label>Receita</label>
        <asp:DropDownList runat="server" ID='DDLcontareceita'></asp:DropDownList>
        </span>

        <span class="linha">
         <label> Historico</label>
        <asp:TextBox ID="txthistorico" runat="server" Height="100px" TextMode="MultiLine" 
            Width="400px"></asp:TextBox>
         </span>

        <span class="linha">
        <label>Arquivo</label>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        </span>

        <span class="linha">
        <label></label>
        <asp:Button ID="btnsalvar" runat="server" Text="Salvar" onclick="Button1_Click" />
        </span>

         <span class="linha">
         <label></label>
        <asp:TextBox ID="txterros" runat="server" Height="100px" TextMode="MultiLine" 
            Width="400px"></asp:TextBox>
        </span>

    </fieldset>
    
</asp:Content>
