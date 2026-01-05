using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadEmpresas : BaseEditCadForm
{
    private Empresa empresa;
    private Modelo modelo;
    private DataTable tbModelosFornecedor = new DataTable("tbModelosFornecedor");
    private DataTable tbModelosCliente = new DataTable("tbModelosCliente");
    private DataTable tbModelosFornecedor_CP = new DataTable("tbModelosFornecedor_CP");
    private DataTable tbModelosFornecedor_CR = new DataTable("tbModelosFornecedor_CR");
	private DataTable tbCodigoMunicipioMain = new DataTable("tbCodigoMunicipioMain");
	private DataTable tbCodigoMunicipioExt = new DataTable("tbCodigoMunicipioExt");
	private DataSet dsConsultoresTimesheet = new DataSet("dsConsultores");
    private DataSet dsEmpresasTimesheet = new DataSet("dsEmpresasTimesheet");
	
	public FormEditCadEmpresas()
        : base("EMPRESA")
    {
        empresa = new Empresa(_conn);
        modelo = new Modelo(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);

		if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Empresa";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Empresa";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        comboConsultor.Attributes.Add("onChange", "bloqueiaCombo(this, '" + comboEmpresaTimesheet.ClientID + "');");
        comboEmpresaTimesheet.Attributes.Add("onChange", "bloqueiaCombo(this, '" + comboConsultor.ClientID + "');");

		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboCodigoPais", "$('#" + comboCodigoPais.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);
		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboCodigoMunicipio", "$('#" + comboCodigoMunicipio.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);
		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboPlanoConta", "$('#" + comboPlanoConta.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);
		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboModeloFornecedor", "$('#" + comboModeloFornecedor.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);
		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboModeloClienteFornecedor_CP", "$('#" + comboModeloClienteFornecedor_CP.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);
		ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboModeloClienteFornecedor_CR", "$('#" + comboModeloClienteFornecedor_CR.ClientID + "').chosen({search_contains : true, no_results_text: \"Nenhum resultado encontrado!\"});", true);

		if (_cadastro)
            H_COD_EMPRESA.Value = "0";
        else
        {

			if (!Page.IsPostBack)
            {
                empresa.codigo = Convert.ToInt32(Request.QueryString["id"]);
                empresa.load();

                H_COD_EMPRESA.Value = empresa.codigo.ToString();
                radioTipo.SelectedValue = empresa.tipo;
                sincronizarCheckBox.Checked = empresa.sincronizar;

                if (empresa.fisicaJuridica == ETipoEmpresa.FISICA)
                    radioFisicaJuridica.SelectedValue = "FISICA";
                else
                    radioFisicaJuridica.SelectedValue = "JURIDICA";

                textNomeRazaoSocial.Text = empresa.nome;
                textNomeFantasia.Text = empresa.nomeFantasia;
                textCnpjCpf.Text = empresa.cnpjCpf;
                textEndereco.Text = empresa.endereco;
                textNumero.Text = empresa.numero;
                textComplemento.Text = empresa.complemento;
                textBairro.Text = empresa.bairro;
                textCep.Text = empresa.cep;
                textMunicipio.Text = empresa.municipio;
                textTelefone.Text = empresa.telefone;
                textIe.Text = empresa.ieRg;
                textIM.Text = empresa.im;
                textUf.Text = empresa.uf;
				comboCodigoPais.SelectedValue = comboCodigoPais.DataSource == null ? "0" : ((DataTable)comboCodigoPais.DataSource).AsEnumerable().Where(o => o["ID_PAIS"].ToString().Equals(empresa.codigoPais) ^ o["COD_PAIS"].ToString().TrimStart('0').Equals(empresa.codigoPais)).Select(o => o["ID_PAIS"].ToString()).FirstOrDefault();
				comboCodigoPais_SelectedIndexChanged(comboCodigoPais, EventArgs.Empty);
				comboCodigoMunicipio.SelectedValue = comboCodigoMunicipio.DataSource == null ? "0": ((DataTable)comboCodigoMunicipio.DataSource).AsEnumerable().Where(o => o["ID_MUNICIPIO"].ToString().Equals(empresa.codigoMunicipio) ^ o["COD_MUNICIPIO"].ToString().TrimStart('0').Equals(empresa.codigoMunicipio)).Select(o => o["ID_MUNICIPIO"].ToString()).FirstOrDefault();
				validaCodigoPaisMunicipio();
                comboGrupoFinanceiroEntrada.SelectedValue = Convert.ToString(empresa.grupoFinanceiroEntrada);
                comboGrupoFinanceiroSaida.SelectedValue = Convert.ToString(empresa.grupoFinanceiroSaida);
                comboGrupoEconomico.SelectedValue = Convert.ToString(empresa.codigoGrupoEconomico);
				textNire.Text = empresa.nire;
                textMatricula.Text = empresa.consultor.ToString();

                if (empresa.tipo == "GRUPO")
                {
                    comboPlanoConta.SelectedValue = empresa.cod_empresa_plano_contas.ToString();
                    exigeContribuicoesCheckBox.Checked = empresa.exigeContribuicoes;
                }
                else
                    comboPlanoConta.Enabled = false;

                if (empresa.consultor > 0)
                {
                    comboConsultor.SelectedValue = empresa.consultor.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bloqueia_combo",
                        "bloqueiaCombo($(\"#" + comboConsultor.ClientID + "\"), '" + comboEmpresaTimesheet.ClientID + "');", true);
                }
                else
                {
                    comboEmpresaTimesheet.SelectedValue = empresa.empresaTimesheet.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "bloqueia_combo",
                        "bloqueiaCombo($(\"#" + comboEmpresaTimesheet.ClientID + "\"), '" + comboConsultor.ClientID + "');", true);
                }

                radioTipo.Enabled = false;
                radioTipo_SelectedIndexChanged(radioTipo, EventArgs.Empty);

                switch (empresa.tipo)
                {
                    case "FORNECEDOR":
                        comboModeloFornecedor.SelectedValue = empresa.sugestaoModeloCP.ToString();
                        break;
                    case "CLIENTE":
                        comboModeloCliente.SelectedValue = empresa.sugestaoModeloCR.ToString();
                        break;
                    case "CLIENTE_FORNECEDOR":
                        comboModeloClienteFornecedor_CP.SelectedValue = empresa.sugestaoModeloCP.ToString();
                        comboModeloClienteFornecedor_CR.SelectedValue = empresa.sugestaoModeloCR.ToString();
                        break;
                }
            }
        }

        string strMascaras = "$(\"#" + textCep.ClientID + "\").unmask();";
        strMascaras += "$(\"#" + textCep.ClientID + "\").mask(\"99999-999\");";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "mascaras",
            strMascaras, true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alteraFisicaJuridica",
            "alteraFisicaJuridica('" + radioFisicaJuridica.ClientID + "','" + textNomeRazaoSocial.ClientID + "','" + textCnpjCpf.ClientID + "');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alteraTipo",
            "alteraTipo('" + radioTipo.ClientID + "','" + radioFisicaJuridica.ClientID + "');", true);
    }
   
    protected override void montaTela()
    {
        base.montaTela();
        addSubTitulo("Empresas", "FormGridEmpresas.aspx");
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
            radioFisicaJuridica.Attributes.Add("onclick", "alteraFisicaJuridica('" + radioFisicaJuridica.ClientID + "','" + textNomeRazaoSocial.ClientID + "','" + textCnpjCpf.ClientID + "');");
            radioTipo.Attributes.Add("onclick", "alteraTipo('" + radioTipo.ClientID + "','" + radioFisicaJuridica.ClientID + "');");
            Empresa emp = new Empresa(_conn);
            emp.codigo = Convert.ToInt32(Session["empresa"]);
            emp.load();

            if (emp.nome == "Mlegate")
            {
                empresa.listaEmpresasTimesheet(ref dsEmpresasTimesheet);
                empresa.listaConsultoresTimesheet(ref dsConsultoresTimesheet);
            }

            if (emp.nome == "Mlegate")
            {
                comboConsultor.DataSource = dsConsultoresTimesheet.Tables[0];
                comboConsultor.DataTextField = "NomeConsultor";
                comboConsultor.DataValueField = "CodConsultor";
                comboConsultor.DataBind();
            }
            comboConsultor.Items.Insert(0, new ListItem("Escolha", "0"));
            if (emp.nome == "Mlegate")
            {
                comboEmpresaTimesheet.DataSource = dsEmpresasTimesheet.Tables[0];
                comboEmpresaTimesheet.DataTextField = "NomeCliente";
                comboEmpresaTimesheet.DataValueField = "CodCliente";
                comboEmpresaTimesheet.DataBind();
            }
            comboEmpresaTimesheet.Items.Insert(0, new ListItem("Escolha", "0"));

            GrupoFinanceiroDAO grupoFinanceiroDAO = new GrupoFinanceiroDAO(_conn);
            List<GrupoFinanceiro> listaGruposFinanceiros = grupoFinanceiroDAO.list();

            comboGrupoFinanceiroEntrada.DataSource = listaGruposFinanceiros;
            comboGrupoFinanceiroEntrada.DataTextField = "nome";
            comboGrupoFinanceiroEntrada.DataValueField = "nome";
            comboGrupoFinanceiroEntrada.DataBind();
            comboGrupoFinanceiroEntrada.Items.Insert(0, new ListItem("Nenhum", "0"));
            
            comboGrupoFinanceiroSaida.DataSource = listaGruposFinanceiros;
            comboGrupoFinanceiroSaida.DataTextField = "nome";
            comboGrupoFinanceiroSaida.DataValueField = "nome";
            comboGrupoFinanceiroSaida.DataBind();
            comboGrupoFinanceiroSaida.Items.Insert(0, new ListItem("Nenhum", "0"));

            GrupoEconomicoDAO grupoEconomicoDAO = new GrupoEconomicoDAO(_conn);
            comboGrupoEconomico.DataSource = grupoEconomicoDAO.list();
            comboGrupoEconomico.DataTextField = "descricao";
            comboGrupoEconomico.DataValueField = "codigoGrupoEconomico";
            comboGrupoEconomico.DataBind();
            comboGrupoEconomico.Items.Insert(0, new ListItem("Nenhum", "0"));

            paisDAO paisDAO = new paisDAO(_conn);
			comboCodigoPais.DataSource = paisDAO.lista();
			comboCodigoPais.DataTextField = "DESC_PAIS";
			comboCodigoPais.DataValueField = "ID_PAIS";
			comboCodigoPais.DataBind();
			comboCodigoPais.Items.Insert(0, new ListItem("Escolha", "0"));

			comboCodigoMunicipio.Items.Insert(0, new ListItem("Escolha", "0"));
			comboCodigoMunicipio.Enabled = false;

			empresasDAO empresaDAO = new empresasDAO(_conn);
            comboPlanoConta.DataSource = empresaDAO.carregaEmpresa();
            comboPlanoConta.DataTextField = "NOME_RAZAO_SOCIAL";
            comboPlanoConta.DataValueField = "COD_EMPRESA";
            comboPlanoConta.DataBind();
            comboPlanoConta.Items.Insert(0, new ListItem("Escolha", "0"));

            modelo.listaTipoCP(ref tbModelosFornecedor);
            comboModeloFornecedor.DataSource = tbModelosFornecedor;
            comboModeloFornecedor.DataTextField = "NOME";
            comboModeloFornecedor.DataValueField = "COD_MODELO";
            comboModeloFornecedor.DataBind();
            comboModeloFornecedor.Items.Insert(0, new ListItem("Escolha", "0"));

            modelo.listaTipoCR(ref tbModelosCliente);
            comboModeloCliente.DataSource = tbModelosCliente;
            comboModeloCliente.DataTextField = "NOME";
            comboModeloCliente.DataValueField = "COD_MODELO";
            comboModeloCliente.DataBind();
            comboModeloCliente.Items.Insert(0, new ListItem("Escolha", "0"));

            modelo.listaTipoCP(ref tbModelosFornecedor_CP);
            comboModeloClienteFornecedor_CP.DataSource = tbModelosFornecedor_CP;
            comboModeloClienteFornecedor_CP.DataTextField = "NOME";
            comboModeloClienteFornecedor_CP.DataValueField = "COD_MODELO";
            comboModeloClienteFornecedor_CP.DataBind();
            comboModeloClienteFornecedor_CP.Items.Insert(0, new ListItem("Escolha", "0"));

            modelo.listaTipoCR(ref tbModelosFornecedor_CR);
            comboModeloClienteFornecedor_CR.DataSource = tbModelosFornecedor_CR;
            comboModeloClienteFornecedor_CR.DataTextField = "NOME";
            comboModeloClienteFornecedor_CR.DataValueField = "COD_MODELO";
            comboModeloClienteFornecedor_CR.DataBind();
            comboModeloClienteFornecedor_CR.Items.Insert(0, new ListItem("Escolha", "0"));
        }
        ClientScript.RegisterStartupScript(GetType(), "text", "formataTelefoneCelular();", true);
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
		if (!validaCodigoPaisMunicipio()) return;

		if (_cadastro)
        {
            empresa.sincronizar = sincronizarCheckBox.Checked;
            empresa.nome = textNomeRazaoSocial.Text;
            empresa.nomeFantasia = textNomeFantasia.Text;
            empresa.cnpjCpf = limpaString(textCnpjCpf.Text);
            empresa.endereco = textEndereco.Text;
            empresa.numero = textNumero.Text;
            empresa.complemento = textComplemento.Text;
            empresa.bairro = textBairro.Text;
            empresa.cep = limpaString(textCep.Text);
            empresa.municipio = textMunicipio.Text;
            empresa.telefone = limpaString(textTelefone.Text);
            empresa.ieRg = textIe.Text;
            empresa.im = textIM.Text;
            empresa.uf = textUf.Text;
            empresa.tipo = radioTipo.SelectedValue;
            empresa.codigoPais = comboCodigoPais.SelectedValue;
			empresa.codigoMunicipio = comboCodigoMunicipio.SelectedValue;
			empresa.nire = textNire.Text;
            empresa.cod_empresa_plano_contas = Convert.ToInt32(comboPlanoConta.SelectedValue);
            empresa.consultor = String.IsNullOrEmpty(textMatricula.Text) ? 0 : Convert.ToInt32(textMatricula.Text);
            empresa.grupoFinanceiroEntrada = String.IsNullOrEmpty(comboGrupoFinanceiroEntrada.SelectedValue) ? null : Convert.ToString(comboGrupoFinanceiroEntrada.SelectedValue);
            empresa.grupoFinanceiroSaida = String.IsNullOrEmpty(comboGrupoFinanceiroSaida.SelectedValue) ? null : Convert.ToString(comboGrupoFinanceiroSaida.SelectedValue);
            empresa.codigoGrupoEconomico = String.IsNullOrEmpty(comboGrupoEconomico.SelectedValue) ? 0 : Convert.ToInt32(comboGrupoEconomico.SelectedValue);

            if (radioTipo.SelectedValue == "FORNECEDOR")
            {
                empresa.sugestaoModeloCP = Convert.ToInt32(comboModeloFornecedor.SelectedValue);
            }
            else if (radioTipo.SelectedValue == "CLIENTE")
            {
                empresa.sugestaoModeloCR = Convert.ToInt32(comboModeloCliente.SelectedValue);
            }
            else if (radioTipo.SelectedValue == "CLIENTE_FORNECEDOR")
            {
                empresa.sugestaoModeloCP = Convert.ToInt32(comboModeloClienteFornecedor_CP.SelectedValue);
                empresa.sugestaoModeloCR = Convert.ToInt32(comboModeloClienteFornecedor_CR.SelectedValue);
            }
            else
            {
                empresa.sugestaoModeloCP = 0;
                empresa.sugestaoModeloCR = 0;
                empresa.exigeContribuicoes = exigeContribuicoesCheckBox.Checked;
            }

            if (radioFisicaJuridica.SelectedValue == "FISICA")
                empresa.fisicaJuridica = ETipoEmpresa.FISICA;
            else
                empresa.fisicaJuridica = ETipoEmpresa.JURIDICA;

            if (comboConsultor.SelectedValue != "0")
            {
                empresa.empresaTimesheet = 0;
            }
            else
            {
                empresa.empresaTimesheet = Convert.ToInt32(comboEmpresaTimesheet.SelectedValue);
            }

            List<string> erros = empresa.validar("novo");

            if (erros.Count == 0)
            {
                Response.Redirect("FormGridEmpresas.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
        else
        {
            empresa.codigo = Convert.ToInt32(H_COD_EMPRESA.Value);
            empresa.sincronizar = sincronizarCheckBox.Checked;
            empresa.nome = textNomeRazaoSocial.Text;
            empresa.nomeFantasia = textNomeFantasia.Text;
            empresa.cnpjCpf = limpaString(textCnpjCpf.Text);
            empresa.endereco = textEndereco.Text;
            empresa.numero = textNumero.Text;
            empresa.complemento = textComplemento.Text;
            empresa.bairro = textBairro.Text;
            empresa.cep = limpaString(textCep.Text);
            empresa.municipio = textMunicipio.Text;
            empresa.telefone = limpaString(textTelefone.Text);
            empresa.ieRg = textIe.Text;
            empresa.im = textIM.Text;
            empresa.uf = textUf.Text;
            empresa.tipo = radioTipo.SelectedValue;
			empresa.codigoPais = comboCodigoPais.SelectedValue;
			empresa.codigoMunicipio = comboCodigoMunicipio.SelectedValue;
			empresa.nire = textNire.Text;
            empresa.consultor = String.IsNullOrEmpty(textMatricula.Text) ? 0 : Convert.ToInt32(textMatricula.Text);
            empresa.codigoGrupoEconomico = String.IsNullOrEmpty(comboGrupoEconomico.SelectedValue) ? 0 : Convert.ToInt32(comboGrupoEconomico.SelectedValue);
            empresa.grupoFinanceiroEntrada = String.IsNullOrEmpty(comboGrupoFinanceiroEntrada.SelectedValue) ? null : Convert.ToString(comboGrupoFinanceiroEntrada.SelectedValue);
            empresa.grupoFinanceiroSaida = String.IsNullOrEmpty(comboGrupoFinanceiroSaida.SelectedValue) ? null : Convert.ToString(comboGrupoFinanceiroSaida.SelectedValue);

            if (radioTipo.SelectedValue == "FORNECEDOR")
            {
                empresa.sugestaoModeloCP = Convert.ToInt32(comboModeloFornecedor.SelectedValue);
            }
            else if (radioTipo.SelectedValue == "CLIENTE")
            {
                empresa.sugestaoModeloCR = Convert.ToInt32(comboModeloCliente.SelectedValue);
            }
            else if (radioTipo.SelectedValue == "CLIENTE_FORNECEDOR")
            {
                empresa.sugestaoModeloCP = Convert.ToInt32(comboModeloClienteFornecedor_CP.SelectedValue);
                empresa.sugestaoModeloCR = Convert.ToInt32(comboModeloClienteFornecedor_CR.SelectedValue);
            }
            else
            {
                empresa.sugestaoModeloCP = 0;
                empresa.sugestaoModeloCR = 0;
                empresa.exigeContribuicoes = exigeContribuicoesCheckBox.Checked;
            }

            if (radioFisicaJuridica.SelectedValue == "FISICA")
                empresa.fisicaJuridica = ETipoEmpresa.FISICA;
            else
                empresa.fisicaJuridica = ETipoEmpresa.JURIDICA;

            if (comboConsultor.SelectedValue != "0")
            {
                empresa.empresaTimesheet = 0;
            }
            else
            {
                empresa.empresaTimesheet = Convert.ToInt32(comboEmpresaTimesheet.SelectedValue);
            }

            List<string> erros = empresa.validar("alterar");

            if (erros.Count == 0)
            {
                Response.Redirect("FormGridEmpresas.aspx");
            }
            else
            {
                errosFormulario(erros);
            }
        }
    }

    protected void radioTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (radioTipo.SelectedValue)
        {
            case "FORNECEDOR":
                legendaModelos.Visible = true;
                painelModelosFornecedor.Visible = true;
                painelModelosCliente.Visible = false;
                painelModelosClienteFornecedor.Visible = false;
                linhaSincronizar.Visible = false;
                sincronizarCheckBox.Checked = false;
                linhaExigeContribuicoes.Visible = false;
                break;
            case "CLIENTE":
                legendaModelos.Visible = true;
                painelModelosFornecedor.Visible = false;
                painelModelosCliente.Visible = true;
                painelModelosClienteFornecedor.Visible = false;
                linhaSincronizar.Visible = false;
                sincronizarCheckBox.Checked = false;
                linhaExigeContribuicoes.Visible = false;
                break;
            case "CLIENTE_FORNECEDOR":
                legendaModelos.Visible = true;
                painelModelosFornecedor.Visible = false;
                painelModelosCliente.Visible = false;
                painelModelosClienteFornecedor.Visible = true;
                linhaSincronizar.Visible = false;
                sincronizarCheckBox.Checked = false;
                linhaExigeContribuicoes.Visible = false;
                break;
            case "EMITENTE":
                legendaModelos.Visible = false;
                painelModelosFornecedor.Visible = false;
                painelModelosCliente.Visible = false;
                painelModelosClienteFornecedor.Visible = false;
                linhaSincronizar.Visible = false;
                sincronizarCheckBox.Checked = false;
                linhaExigeContribuicoes.Visible = false;
                break;
            case "GRUPO":
                legendaModelos.Visible = false;
                painelModelosFornecedor.Visible = false;
                painelModelosCliente.Visible = false;
                painelModelosClienteFornecedor.Visible = false;
                linhaSincronizar.Visible = true;
                linhaExigeContribuicoes.Visible = true;
                break;
            default:
                legendaModelos.Visible = false;
                painelModelosFornecedor.Visible = false;
                painelModelosCliente.Visible = false;
                painelModelosClienteFornecedor.Visible = false;
                linhaSincronizar.Visible = true;
                linhaExigeContribuicoes.Visible = false;
                break;
        }
    }
	protected void comboCodigoPais_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboCodigoPais.SelectedValue.Equals("0"))
		{
			comboCodigoMunicipio.SelectedValue = "0";
			comboCodigoMunicipio.Enabled = false;
		}

		else
		{
			comboCodigoMunicipio.Items.Clear();

			if (comboCodigoPais.SelectedItem.Text.Split(" - ".ToCharArray())[0].TrimStart('0').Equals("1058"))
			{

				if (ViewState["tbcodigoMunicipioMain"] != null)
					comboCodigoMunicipio.DataSource = ViewState["tbcodigoMunicipioMain"];

				else
				{
					municipioDAO municipioDAO = new municipioDAO(_conn);
					tbCodigoMunicipioMain = municipioDAO.lista();
					comboCodigoMunicipio.DataSource = tbCodigoMunicipioMain;
					var registroExt = tbCodigoMunicipioMain.Rows[tbCodigoMunicipioMain.Rows.Count - 1];
					tbCodigoMunicipioExt = tbCodigoMunicipioMain.Clone();
					tbCodigoMunicipioExt.ImportRow(registroExt);
					tbCodigoMunicipioMain.Rows.RemoveAt(tbCodigoMunicipioMain.Rows.Count - 1);
					ViewState.Add("tbcodigoMunicipioMain", tbCodigoMunicipioMain);
					ViewState.Add("tbcodigoMunicipioExt", tbCodigoMunicipioExt);
				}

				comboCodigoMunicipio.Enabled = true;
				comboCodigoMunicipio.DataTextField = "DESC_MUNICIPIO";
				comboCodigoMunicipio.DataValueField = "ID_MUNICIPIO";
				comboCodigoMunicipio.DataBind();
				comboCodigoMunicipio.Items.Insert(0, new ListItem("Escolha", "0"));
				comboCodigoMunicipio.SelectedValue = "0";
            }
			else
			{
				if (ViewState["tbcodigoMunicipioExt"] != null)
					comboCodigoMunicipio.DataSource = ViewState["tbcodigoMunicipioExt"];
				else
				{
					municipioDAO municipioDAO = new municipioDAO(_conn);
					comboCodigoMunicipio.DataSource = municipioDAO.lista_externo();
					tbCodigoMunicipioExt = ((DataTable)comboCodigoMunicipio.DataSource);
					ViewState.Add("tbcodigoMunicipioExt", tbCodigoMunicipioExt);
				}
				comboCodigoMunicipio.Enabled = false;
				comboCodigoMunicipio.DataTextField = "DESC_MUNICIPIO";
				comboCodigoMunicipio.DataValueField = "ID_MUNICIPIO";
				comboCodigoMunicipio.DataBind();
				comboCodigoMunicipio.Items.Insert(0, new ListItem("Escolha", "0"));
				comboCodigoMunicipio.SelectedIndex = 1;
			}
		}
		validaCodigoPaisMunicipio();
	}

	private bool validaCodigoPaisMunicipio()
	{
		bool comboCodigoValido = true;

		if (comboCodigoPais.SelectedIndex == 0)
		{
			validacaoCodigoPais.Visible = true;
			comboCodigoValido = false;
		}

		else validacaoCodigoPais.Visible = false;

		if (comboCodigoMunicipio.SelectedIndex == 0)
		{
			validacaoCodigoMunicipio.Visible = true;
			comboCodigoValido = false;
		}
		else validacaoCodigoMunicipio.Visible = false;

		return comboCodigoValido;
	}

	protected void comboCodigoMunicipio_SelectedIndexChanged(object sender, EventArgs e)
	{
		validaCodigoPaisMunicipio();
	}
}