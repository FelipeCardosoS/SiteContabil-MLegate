using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadGruposEconomicos : BaseEditCadForm
{
    private GrupoEconomico grupoEconomico;
    public FormEditCadGruposEconomicos()
    : base("GRUPO_ECONOMICO")
    {
        grupoEconomico = new GrupoEconomico(_conn);
    }
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);


        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            H_COD_GRUPO_ECONOMICO.Value = "0";
        }
        else
        {
            botaoSalvar.Text = "Alterar";

            if (!Page.IsPostBack)
            {
                grupoEconomico.codigoGrupoEconomico = Convert.ToInt32(Request.QueryString["id"]);
                grupoEconomico.load();

                H_COD_GRUPO_ECONOMICO.Value = grupoEconomico.codigoGrupoEconomico.ToString();
                textDescricao.Text = grupoEconomico.descricao;
            }
        }
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Grupo Econômico";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Grupo Econômico";
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {

            grupoEconomico.descricao = textDescricao.Text;

            List<string> erros = grupoEconomico.novo();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposEconomicos.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            grupoEconomico.codigoGrupoEconomico = Convert.ToInt32(H_COD_GRUPO_ECONOMICO.Value);
            grupoEconomico.descricao = textDescricao.Text;

            List<string> erros = grupoEconomico.alterar();
            if (erros.Count == 0)
            {
                Response.Redirect("FormGridGruposEconomicos.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }


}