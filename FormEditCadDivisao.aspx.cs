using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Configuration;

public partial class FormEditCadDivisao : BaseEditCadForm
{
    public bool sincronizaTimesheet;

    public FormEditCadDivisao() : base("DIVISAO") { }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Modelo";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Modelo";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Page.IsPostBack)
            sincronizaTimesheet = Convert.ToBoolean(ConfigurationManager.AppSettings["sincronizaTimesheet"]);
    }

    protected override void montaTela()
    {
        base.montaTela();
        addSubTitulo("Divisões", "FormGridDivisoes.aspx");
        
        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {
            if (!_cadastro)
            {
                Divisao divisao = new Divisao(_conn);
                divisao.codigo = Convert.ToInt32(Request.QueryString["id"]);
                divisao.load();

                nomeTextBox.Text = divisao.descricao;
                checkSincroniza.Checked = divisao.sincronizaBool;
            }
            else
                checkSincroniza.Checked = true;
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        List<string> erros;

        Divisao divisao = new Divisao(_conn);
        divisao.descricao = nomeTextBox.Text;
        divisao.sincronizaBool = checkSincroniza.Checked;
        
        if (_cadastro)
            erros = divisao.novo();
        else
        {
            divisao.codigo = Convert.ToInt32(Request.QueryString["id"]);
            divisao.sincronizaBool = checkSincroniza.Checked;
            erros = divisao.alterar();
        }

        if (erros.Count > 0)
            errosFormulario(erros);
        else
            Response.Redirect("FormGridDivisoes.aspx");
    }
}