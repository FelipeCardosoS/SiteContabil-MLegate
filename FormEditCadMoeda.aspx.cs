using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadMoeda : BaseEditCadForm
{
    int codMoeda;
    moedaDAL moedaDAL;

    public FormEditCadMoeda()
        : base("MOEDA")
    {
        moedaDAL = new moedaDAL(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);
        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Moeda";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Moeda";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
        }
        else
        {
            botaoSalvar.Text = "Alterar";
            if (!Page.IsPostBack)
            {
                codMoeda = Convert.ToInt32(Request.QueryString["id"]);
                SMoeda tipo = moedaDAL.load(codMoeda);
                if (tipo != null)
                {
                    txtDescricao.Text = tipo.descricao;
                }
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (txtDescricao.Text != "")
        {
            if (_cadastro)
            {
                if (moedaDAL.novo(txtDescricao.Text))
                {
                    Response.Redirect("FormGridMoeda.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Já existe Moeda com esse nome!');", true);
                }
            }
            else
            {
                codMoeda = 0;
                int.TryParse(Request.QueryString["id"], out codMoeda);

                if (moedaDAL.editar(codMoeda, txtDescricao.Text))
                {
                    Response.Redirect("FormGridMoeda.aspx");
                }
                else 
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Já existe Moeda com esse nome!');", true);
                }
            }
        }
        else 
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Campo vazio.');", true);
        }
    }


}