using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadRegrasContabeis : BaseEditCadForm
{
	DataTable tbContas;
	DataTable tbTerceiros;
	List<RegraContabil> listaRegras;
	ContaContabil conta;
	Empresa empresa;
	RegraContabil regraContabil;
	public FormEditCadRegrasContabeis()
	  : base("REGRAS_CONTABEIS")
	{
		conta = new ContaContabil(_conn);
		empresa = new Empresa(_conn);
		regraContabil = new RegraContabil(_conn);
		listaRegras = new List<RegraContabil>();

		tbContas = new DataTable();
		tbTerceiros = new DataTable();
	}

	protected override void montaTela()
	{
		base.montaTela();

		if (_cadastro)
			subTitulo.Text = "Novo";
		else
			subTitulo.Text = "Editar";

		botaoSalvar.Click -= botaoSalvar_Click;
		botaoSalvar.UseSubmitBehavior = false;
	}

	protected override void Page_Load(object sender, EventArgs e)
	{
		base.Page_Load(sender, e);
		int codEmpresa = Convert.ToInt32(Request.QueryString["id"]);

		if (Page.IsPostBack) return;

		conta.listaContasAnaliticas(ref tbContas);
		empresa.listaFornecedoresClientes(ref tbTerceiros);

		ddlContaDespesa.DataSource = tbContas;
		ddlContaDespesa.DataTextField = "DESCRICAO";
		ddlContaDespesa.DataValueField = "COD_CONTA";
		ddlContaDespesa.DataBind();
		ddlContaDespesa.Items.Insert(0, new ListItem("Escolha", ""));

		ddlContaFornecedor.DataSource = tbContas;
		ddlContaFornecedor.DataTextField = "DESCRICAO";
		ddlContaFornecedor.DataValueField = "COD_CONTA";
		ddlContaFornecedor.DataBind();
		ddlContaFornecedor.Items.Insert(0, new ListItem("Escolha", ""));

		List<ContaContabil> listaContas = new List<ContaContabil>();
		foreach(DataRow linhaConta in tbContas.Rows)
		{
			ContaContabil rConta = new ContaContabil(_conn);
			rConta.codigo = Convert.ToString(linhaConta["COD_CONTA"]);
			rConta.descricao = Convert.ToString(linhaConta["DESCRICAO"]);
			listaContas.Add(rConta);
		}
		listaContas.Insert(0, new ContaContabil(_conn)
		{
			codigo = "0",
			descricao = "Escolha..."
		});
		List<Empresa> listaTerceiros = new List<Empresa>();
		foreach (DataRow linhaTerceiro in tbTerceiros.Rows)
		{
			Empresa terceiro = new Empresa(_conn);
			terceiro.codigo = Convert.ToInt32(linhaTerceiro["COD_EMPRESA"]);
			terceiro.nome = Convert.ToString(linhaTerceiro["NOME_RAZAO_SOCIAL"]);

			listaTerceiros.Add(terceiro);
		}
		listaTerceiros.Insert(0, new Empresa(_conn)
		{
			codigo = 0,
			nome = "Escolha..."
		});
		hdContas.Value = new JavaScriptSerializer().Serialize(listaContas);
		hdTerceiros.Value = new JavaScriptSerializer().Serialize(listaTerceiros);

		listaRegras = regraContabil.lista(codEmpresa);

		InicializaRegras();
	}

	private void InicializaRegras()
	{
		if(listaRegras.Where(o => o.TipoRegra.StartsWith("IR")).Count() == 0)
		{
			listaRegras.Insert(0, new RegraContabil
			{
				Nome = "Imposto Renda Fonte",
				TipoRegra = "IR_DESCONTO"
			});
		}

		if (listaRegras.Where(o => o.TipoRegra.StartsWith("PCC")).Count() == 0)
		{
			listaRegras.Insert(1, new RegraContabil
			{
				Nome = "PIS/COFINS/CSLL",
				TipoRegra = "PCC_DESCONTO"
			});
		}

		if (listaRegras.Where(o => o.TipoRegra.StartsWith("OUTROS")).Count() == 0)
		{
			listaRegras.Add(new RegraContabil
			{
				Nome = "Outro 1",
				TipoRegra = "OUTROS_DESCONTO"
			});
		}

		
		ddlContaDespesa.SelectedValue = Convert.ToString(listaRegras.Where(o => o.TipoRegra.Equals("DESPESA_CONTA")).Select(o => o.CodConta).FirstOrDefault());
		ddlContaFornecedor.SelectedValue = Convert.ToString(listaRegras.Where(o => o.TipoRegra.Equals("FORNECEDOR_CONTA")).Select(o => o.CodConta).FirstOrDefault());

		hdRegras.Value = new JavaScriptSerializer().Serialize(listaRegras);
	}
	protected override void Page_PreLoad(object sender, EventArgs e)
	{
		base.Page_PreLoad(sender, e);
	}

	[WebMethod]
	public static void salvaRegras(string regras) {
		HttpContext.Current.Session["ss_regras_contabeis"] = regras; 
	}

	protected override void botaoSalvar_Click(object sender, EventArgs e)
	{
		base.botaoSalvar_Click(sender, e);
		salvar();

	}

	private void salvar()
	{
		listaRegras = JsonConvert.DeserializeObject<List<RegraContabil>>(hdRegrasNovo.Value);
		List<int> listaDeletar = JsonConvert.DeserializeObject<List<int>>(hdRegrasDeletar.Value);
		List<RegraContabil> listaAtual = regraContabil.lista(Convert.ToInt32(Request.QueryString["id"]));

		if (listaDeletar == null)
			listaDeletar = new List<int>();

		RegraContabil contaDespesa = listaAtual.Where(o => o.TipoRegra.Equals("DESPESA_CONTA")).FirstOrDefault();
		RegraContabil contaFornecedor = listaAtual.Where(o => o.TipoRegra.Equals("FORNECEDOR_CONTA")).FirstOrDefault();

		if (!Convert.ToString(ddlContaDespesa.SelectedValue).Equals(""))
		{
			if (contaDespesa == null)
				listaRegras.Add(new RegraContabil
				{
					Nome = "Conta Despesa",
					TipoRegra = "DESPESA_CONTA",
					CodConta = Convert.ToString(ddlContaDespesa.SelectedValue)
				});
			else
			{
				contaDespesa.CodConta = Convert.ToString(ddlContaDespesa.SelectedValue);
				listaRegras.Add(contaDespesa);
			}
		}
		else if(contaDespesa != null)
		{
			listaDeletar.Add(contaDespesa.CodRegraContabil);
		}

		if (!Convert.ToString(ddlContaFornecedor.SelectedValue).Equals(""))
		{
			if(contaFornecedor == null)
			{
				listaRegras.Add(new RegraContabil
				{
					Nome = "Conta Fornecedor",
					TipoRegra = "FORNECEDOR_CONTA",
					CodConta = Convert.ToString(ddlContaFornecedor.SelectedValue)
				});
			}
			else
			{
				contaFornecedor.CodConta = Convert.ToString(ddlContaFornecedor.SelectedValue);
				listaRegras.Add(contaFornecedor);
			}

		}
		else if (contaFornecedor != null)
		{
			listaDeletar.Add(contaFornecedor.CodRegraContabil);
		}

		List<string> erros = regraContabil.salva(listaRegras, listaDeletar, Convert.ToInt32(Request.QueryString["id"]));

		if (erros.Count > 0)
			errosFormulario(erros);
		else
			Response.Redirect("FormEditCadRegrasContabeis.aspx.cs");

	}

	private bool validar()
	{
		List<string> erros = new List<string>();

		erros = regraContabil.valida(listaRegras);

		if(erros.Count() > 0)
		{
			ScriptManager.RegisterStartupScript(this, this.GetType(), "postBackErros", "alert('" + string.Join(Environment.NewLine, erros) +"');", true);
			return false;
		}

		listaRegras.RemoveAll(o => string.IsNullOrEmpty(o.TipoRegra));

		return true;
	}
}