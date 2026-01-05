using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class FormRelatorios : BaseForm
{
    public FormRelatorios()
        : base("RELATORIO")
    {

    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        subTitulo.Text = "Relatórios";
        Title += "Relatórios";
    }

    protected override void montaTela()
    {
        base.montaTela();

        if (!Page.IsPostBack)
        {
            for (int i = 0; i < _tarefas.Count; i++)
            {
                listRelatorios.Items.Add(new ListItem(_tarefas[i].descricao, _tarefas[i].tarefa.ToString()));
            }

        }
    }

    protected void botaoVaiRelatorio_Click(object sender, EventArgs e)
    {
        if (listRelatorios.SelectedIndex >= 0)
        {
            Response.Redirect("FormRelatorioGerar.aspx?rel=" + listRelatorios.SelectedValue);
        }
        else
        {
            List<string> erros = new List<string>();
            erros.Add("Nenhum relatório foi escolhido.");
            errosFormulario(erros);
        }
    }
}
