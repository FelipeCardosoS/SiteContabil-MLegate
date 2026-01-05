using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadDespesas : BaseEditCadForm
{
	private Despesa despesa;
	private Despesa loadDespesa;
	public FormEditCadDespesas()
		: base("DESPESA")
	{
		despesa = new Despesa(_conn);
	}

	protected override void Page_PreLoad(object sender, EventArgs e)
	{
		base.Page_PreLoad(sender, e);

		if (_cadastro)
		{
			_codigoTarefa = "CAD";
			Title += "Cadastro de Despesa";
		}
		else
		{
			_codigoTarefa = "ALT";
			Title += "Edição de Despesa";
		}
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);

		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHddlTipoDespesa", "$('#" + ddlTipoDespesa.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);

		if (_cadastro)
			H_COD_DESPESA.Value = "0";
		else
		{

			if (!Page.IsPostBack)
			{
				loadDespesa = despesa.load(Convert.ToInt32(Request.QueryString["id"]));

				H_COD_DESPESA.Value = loadDespesa.CodDespesa.ToString();
				ddlTipoDespesa.SelectedValue = Convert.ToString(loadDespesa.CodTipoDespesa);
				ddlTipoValor.SelectedValue = loadDespesa.ValorLivre ? "1" : "0";
				chkAnexo.Checked = loadDespesa.AnexoObrigatorio;

				textDescricao.Text = loadDespesa.Descricao;

			}
		}
	}

	protected override void montaTela()
	{
		base.montaTela();
		addSubTitulo("Despesas", "FormGridDespesas.aspx");
		if (_cadastro)
		{
			subTitulo.Text = "Novo";
			botaoSalvar.Text = "Cadastrar";
		}
		else
		{
			subTitulo.Text = "Editar";
			botaoSalvar.Text = "Editar";
		}

		if (!Page.IsPostBack)
		{
			TipoDespesa tipoDespesa = new TipoDespesa(_conn);
			List<TipoDespesa> listaTipoDespesa = tipoDespesa.lista();

			ddlTipoDespesa.DataSource = listaTipoDespesa;
			ddlTipoDespesa.DataTextField = "Display_Descricao";
			ddlTipoDespesa.DataValueField = "CodTipoDespesa";
			ddlTipoDespesa.DataBind();

			ddlTipoDespesa.Items.Insert(0, new ListItem("Escolha", "0"));

		}
	}


	protected override void botaoSalvar_Click(object sender, EventArgs e)
	{
		loadDespesa = new Despesa();
		if (!_cadastro)
			loadDespesa.CodDespesa = Convert.ToInt32(H_COD_DESPESA.Value);

		loadDespesa.Descricao = textDescricao.Text;
		loadDespesa.CodTipoDespesa = Convert.ToInt32(ddlTipoDespesa.SelectedValue);
		loadDespesa.AnexoObrigatorio = chkAnexo.Checked;
		loadDespesa.ValorLivre = Convert.ToBoolean(Convert.ToInt32(ddlTipoValor.SelectedValue));

		List<string> erros = despesa.salva(loadDespesa);

		if (erros.Count == 0)
		{
			Response.Redirect("FormGridDespesas.aspx");
		}
		else
		{
			errosFormulario(erros);
		}
	}
}