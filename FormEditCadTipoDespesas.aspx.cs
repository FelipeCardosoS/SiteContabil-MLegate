using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
public partial class FormEditCadTipoDespesas : BaseEditCadForm
{
	private TipoDespesa tipoDespesa;
	private TipoDespesa tipoDespesaLoad;

    public FormEditCadTipoDespesas()
    : base("TIPO_DESPESA")
    {
        tipoDespesa = new TipoDespesa(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Tipo de Despesa";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Tipo de Despesa";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (_cadastro)
		{
            H_COD_TIPO_DESPESA.Value = "0";
            H_PERIODOS.Value = "";
        }
        else
        {
            if (!Page.IsPostBack)
            {
                tipoDespesaLoad = tipoDespesa.load(Convert.ToInt32(Request.QueryString["id"]));

                H_COD_TIPO_DESPESA.Value = tipoDespesaLoad.CodTipoDespesa.ToString();
				H_PERIODOS.Value = JsonConvert.SerializeObject(tipoDespesaLoad.ListaPeriodos);
				textDescricao.Text = tipoDespesaLoad.Descricao;
                textUnidade.Text = tipoDespesaLoad.Unidade;
                radioTipo.SelectedValue = Convert.ToInt32(tipoDespesaLoad.TipoQuantitativo).ToString();
            
            }
        }

    }

    protected override void montaTela()
    {
        base.montaTela();

        botaoSalvar.Click -= botaoSalvar_Click;
        botaoSalvar.UseSubmitBehavior = false;
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        tipoDespesaLoad = new TipoDespesa();
        if (!_cadastro)
            tipoDespesaLoad.CodTipoDespesa = Convert.ToInt32(H_COD_TIPO_DESPESA.Value);

        tipoDespesaLoad.Descricao = textDescricao.Text;
        tipoDespesaLoad.Unidade = textUnidade.Text;
        tipoDespesaLoad.TipoQuantitativo = radioTipo.SelectedValue.Equals("1");
        tipoDespesaLoad.ListaPeriodos = JsonConvert.DeserializeObject<List<TipoDespesaPeriodo>>(H_PERIODOS.Value);

        List<int> listaDeletar = JsonConvert.DeserializeObject<List<int>>(H_PERIODOS_DELETAR.Value);

        List<string> erros = tipoDespesa.salva(tipoDespesaLoad, listaDeletar);

        if (erros.Count == 0)
        {
            Response.Redirect("FormGridTipoDespesas.aspx");
        }
        else
        {
            errosFormulario(erros);
        }
    }

}