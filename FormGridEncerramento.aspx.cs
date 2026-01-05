using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class FormGridEncerramento : BaseGridForm
{
    public FormGridEncerramento()
        : base("ENCERRAMENTO_PERIODO_GRID") 
    {
        
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        subTitulo.Text = "Encerramentos";
        botaoNovo.NavigateUrl = "FormEncerraPeriodo.aspx";
    }

    protected override void montaGrid()
    {
        EncerramentoPeriodo encerramentoPeriodo = new EncerramentoPeriodo(_conn);
        totalRegistros = encerramentoPeriodo.totalRegistros();
        DataTable tb = new DataTable("d");
        encerramentoPeriodo.listaPaginada(ref tb, paginaAtual, ordenacao);
        repeaterDados.DataSource = tb;
        repeaterDados.DataBind();
        base.montaGrid();
    }

    protected override void botaoDeletar_Click(object sender, EventArgs e)
    {
        List<int> selecionados = new List<int>();
        List<string> erros = new List<string>();
        foreach (RepeaterItem item in repeaterDados.Items)
        {
            if (item.ItemType != ListItemType.Separator)
            {
                HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                if (check.Checked)
                {
                    selecionados.Add(Convert.ToInt32(check.Value));
                }
            }
        }
        EncerramentoPeriodo encerramento = new EncerramentoPeriodo(_conn);
        for (int i = 0; i < selecionados.Count; i++)
        {
            if (!encerramento.deletaEncerramento(selecionados[i]))
            {
                erros.Add("Erro na tentativa de remover o encerramento: "+selecionados[i]+"");
            }
        }
        if (erros.Count > 0)
        {
            errosFormulario(erros);
            return;
        }
        montaGrid();
    }
}
