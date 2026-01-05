using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormConfirmacaoContabilizacao : BaseForm
{
    private Contabilizacao_Funcoes _Contabilizacao_Funcoes;

    public FormConfirmacaoContabilizacao()
        : base("DEFAULT")
    {
        _Contabilizacao_Funcoes = new Contabilizacao_Funcoes(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Confirmação de Contabilização";
        subTitulo.Text = "Confirmação de Contabilização";

        int cod_faturamento_nf = Convert.ToInt32(Request.QueryString["id"]);

        if (cod_faturamento_nf != 0)
        {
            string erros = _Contabilizacao_Funcoes.Contabilizar(cod_faturamento_nf);

            if (string.IsNullOrEmpty(erros))
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Contabilização realizada com sucesso.');location.href='FormRelatorioEmissaoNF.aspx?id=" + cod_faturamento_nf + "';", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Foi encontrado um ou mais erros durante a Contabilização.\\nConforme segue:\\n\\n" + erros + "');location.href='FormRelatorioEmissaoNF.aspx?id=" + cod_faturamento_nf + "';", true);
        }
    }
}