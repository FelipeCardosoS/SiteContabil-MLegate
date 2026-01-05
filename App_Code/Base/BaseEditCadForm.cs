using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public class BaseEditCadForm : BaseForm
{
    protected bool _cadastro;
    protected string _codigoTarefa;
    protected Button botaoSalvar;
    protected Button botaoSalvarHidden;
    protected HtmlContainerControl areaBotoes;
    protected Label labelStatus;
    protected DataSet dsDados = new DataSet("dsDados");

    public BaseEditCadForm(string modulo) : base(modulo)
    {

    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (Request.QueryString["id"] != null)
        {
            _codigoTarefa = "ALT";
            this._cadastro = false;
        }
        else
        {
            _codigoTarefa = "CAD";
            this._cadastro = true;
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {

        base.Page_Load(sender, e);
    }

    protected override void montaTela()
    {
        base.montaTela();

        botaoSalvar = (Button)Master.FindControl("botaoSalvar");
        botaoSalvarHidden = (Button)Master.FindControl("botaoSalvarHidden");
        areaBotoes = (HtmlContainerControl)Master.FindControl("areaBotoes");
        labelStatus = (Label)Master.FindControl("labelStatus");

        botaoSalvar.Click += botaoSalvar_Click;
        botaoSalvarHidden.Click += botaoSalvar_Click;
    }

    protected virtual void botaoSalvar_Click(object sender, EventArgs e)
    {

    }

    protected virtual void limpaCampos() { }

    protected override void verificaTarefas()
    {
        bool status = false;
        for (int i = 0; i < this._tarefas.Count; i++)
        {
            if (this._tarefas[i].tarefa.ToString() == this._codigoTarefa)
            {
                status = true;
                break;
            }
        }

        if (!status)
        {
            if (this._codigoTarefa == "ALT_SENHA")
                status = true;
        }

        if (!status)
            Response.Redirect("AcessoNegado.aspx");
    }

    protected virtual void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        throw new NotImplementedException();
    }
}