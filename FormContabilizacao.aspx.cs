using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormContabilizacao : BaseForm
{
    private readonly Emitente emitente;
    private readonly Servico servico;
    private readonly Retencao retencao;
    private readonly Tributo tributo;
    private readonly ContaContabil conta;
    private readonly Empresa empresa; //tbTerceiros
    private readonly Contabilizacao_Funcoes contabilização_funcoes;

    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbServicos = new DataTable("tbServicos");
    private DataTable tbRetencoes = new DataTable("tbRetencoes");
    private DataTable tbTributos = new DataTable("tbTributos");
    private DataTable tbContas = new DataTable("tbContas");
    private DataTable tbTerceiros = new DataTable("tbTerceiros");
    private DataTable tbContabilizacaoServicos = new DataTable("tbContabilizacaoServicos");
    private DataTable tbContabilizacaoRetencoes = new DataTable("tbContabilizacaoRetencoes");
    private DataTable tbContabilizacaoTributos = new DataTable("tbContabilizacaoTributos");
    private DataTable tbContabilizacaoGlobal = new DataTable("tbContabilizacaoGlobal");

    public FormContabilizacao()
        : base("FATURAMENTO_CADASTROS_CONTABILIZACAO")
    {
        emitente = new Emitente(_conn);
        servico = new Servico(_conn);
        retencao = new Retencao(_conn);
        tributo = new Tributo(_conn);
        conta = new ContaContabil(_conn);
        empresa = new Empresa(_conn); //Terceiros
        contabilização_funcoes = new Contabilizacao_Funcoes(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Parametrização Contábil";
        subTitulo.Text = "Parametrização Contábil";

        if (!Page.IsPostBack)
        {
            tbxHistorico.Attributes.Add("maxlength", tbxHistorico.MaxLength.ToString());

            //Emitentes
            emitente.lista_Emitentes(ref tbEmitentes);
            ddlEmitente.DataSource = tbEmitentes;
            ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlEmitente.DataValueField = "COD_EMPRESA";
            ddlEmitente.DataBind();
            ddlEmitente.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void verificaTarefas()
    {
        bool aceitaSalvar = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "SALVAR")
                aceitaSalvar = true;
        }

        if (!aceitaSalvar)
        {
            ddlEmitente.Enabled = false;
            btnSalvar.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Você precisa de permissão para realizar a Parametrização Contábil.');", true);
        }
    }

    protected void ddlEmitente_Changed(object sender, EventArgs e)
    {
        int cod_emitente = Convert.ToInt32(ddlEmitente.SelectedValue);

        if (cod_emitente != 0)
        {
            string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

            if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('A sessão expirou. Faça login novamente.');", true);
            }
            else
            {
                //Carrega as listas de Contas Analíticas e Empresas (Terceiros)
                conta.listaContasAnaliticas(ref tbContas);
                empresa.lista_Empresas(ref tbTerceiros);

                //Carrega FATURAMENTO_CONTABILIZACAO_: SERVICOS, RETENCOES, TRIBUTOS e GLOBAL
                contabilização_funcoes.Select_Servicos(ref tbContabilizacaoServicos, cod_emitente, Convert.ToInt32(cod_empresa));
                contabilização_funcoes.Select_Retencoes(ref tbContabilizacaoRetencoes, cod_emitente, Convert.ToInt32(cod_empresa));
                contabilização_funcoes.Select_Tributos(ref tbContabilizacaoTributos, cod_emitente, Convert.ToInt32(cod_empresa));
                contabilização_funcoes.Select_Global(ref tbContabilizacaoGlobal, cod_emitente, Convert.ToInt32(cod_empresa));

                //Serviços
                servico.lista_Servicos(ref tbServicos, cod_emitente);
                repeaterServicos.DataSource = tbServicos;
                repeaterServicos.DataMember = tbServicos.TableName;
                repeaterServicos.ItemDataBound += new RepeaterItemEventHandler(repeaterServicos_ItemDataBound);
                repeaterServicos.DataBind();

                //Retenções
                retencao.lista_Retencoes(ref tbRetencoes, cod_emitente);
                repeaterRetencoes.DataSource = tbRetencoes;
                repeaterRetencoes.DataMember = tbRetencoes.TableName;
                repeaterRetencoes.ItemDataBound += new RepeaterItemEventHandler(repeaterRetencoes_ItemDataBound);
                repeaterRetencoes.DataBind();

                //Tributos
                tributo.lista_Tributos(ref tbTributos, cod_emitente);
                repeaterTributos.DataSource = tbTributos;
                repeaterTributos.DataMember = tbTributos.TableName;
                repeaterTributos.ItemDataBound += new RepeaterItemEventHandler(repeaterTributos_ItemDataBound);
                repeaterTributos.DataBind();

                //Global
                //Contas A Receber - Conta Diferença
                ddlContaDiferenca.DataSource = tbContas;
                ddlContaDiferenca.DataTextField = "DESCRICAO";
                ddlContaDiferenca.DataValueField = "COD_CONTA";
                ddlContaDiferenca.DataBind();
                ddlContaDiferenca.Items.Insert(0, new ListItem("Escolha", "0"));

                //Combo Débito/Crédito
                ddlDebitoCredito.Items.Insert(0, new ListItem("Escolha", "0"));
                ddlDebitoCredito.Items.Insert(1, new ListItem("Débito", "DEBITO"));
                ddlDebitoCredito.Items.Insert(2, new ListItem("Crédito", "CREDITO"));

                //Limpa checkbox para não herdar as parametrizações do Emitente anterior
                cbxGeraTitulo.Checked = false;
                cbxInsert.Checked = false;
                cbxUpdate.Checked = false;

                if (tbContabilizacaoGlobal.Rows.Count > 0)
                {
                    if (tbContabilizacaoGlobal.Rows[0]["COD_CONTA_DIFERENCA"] != DBNull.Value)
                        ddlContaDiferenca.SelectedValue = tbContabilizacaoGlobal.Rows[0]["COD_CONTA_DIFERENCA"].ToString();

                    if (tbContabilizacaoGlobal.Rows[0]["DEBITO_CREDITO"] != DBNull.Value)
                        ddlDebitoCredito.SelectedValue = tbContabilizacaoGlobal.Rows[0]["DEBITO_CREDITO"].ToString();

                    if (tbContabilizacaoGlobal.Rows[0]["HISTORICO"] != DBNull.Value)
                        tbxHistorico.Text = tbContabilizacaoGlobal.Rows[0]["HISTORICO"].ToString();

                    if (tbContabilizacaoGlobal.Rows[0]["GERA_TITULO"] != DBNull.Value)
                        cbxGeraTitulo.Checked = Convert.ToBoolean(tbContabilizacaoGlobal.Rows[0]["GERA_TITULO"]);

                    cbxUpdate.Checked = true;
                }

                if (cbxUpdate.Checked == false)
                {
                    cbxInsert.Checked = true;
                }
            }
        }
        else
            Response.Redirect("FormContabilizacao.aspx");
    }

    //Repeater Serviços
    protected virtual void repeaterServicos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            //Contas
            DropDownList ddlContaDebito = (DropDownList)item.FindControl("ddlContaDebito");
            DropDownList ddlContaCredito = (DropDownList)item.FindControl("ddlContaCredito");

            if (ddlContaDebito != null && ddlContaCredito != null)
            {
                ddlContaDebito.DataSource = tbContas;
                ddlContaDebito.DataTextField = "DESCRICAO";
                ddlContaDebito.DataValueField = "COD_CONTA";
                ddlContaDebito.DataBind();
                ddlContaDebito.Items.Insert(0, new ListItem("Escolha", "0"));

                ddlContaCredito.DataSource = tbContas;
                ddlContaCredito.DataTextField = "DESCRICAO";
                ddlContaCredito.DataValueField = "COD_CONTA";
                ddlContaCredito.DataBind();
                ddlContaCredito.Items.Insert(0, new ListItem("Escolha", "0"));
            }

            //Valores
            DropDownList ddlBrutoLiquidoDebito = (DropDownList)item.FindControl("ddlBrutoLiquidoDebito");
            DropDownList ddlBrutoLiquidoCredito = (DropDownList)item.FindControl("ddlBrutoLiquidoCredito");

            if (ddlBrutoLiquidoDebito != null && ddlBrutoLiquidoCredito != null)
            {
                ddlBrutoLiquidoDebito.Items.Insert(0, new ListItem("Escolha", "0"));
                ddlBrutoLiquidoDebito.Items.Insert(1, new ListItem("Bruto", "BRUTO"));
                ddlBrutoLiquidoDebito.Items.Insert(2, new ListItem("Líquido", "LIQUIDO"));

                ddlBrutoLiquidoCredito.Items.Insert(0, new ListItem("Escolha", "0"));
                ddlBrutoLiquidoCredito.Items.Insert(1, new ListItem("Bruto", "BRUTO"));
                ddlBrutoLiquidoCredito.Items.Insert(2, new ListItem("Líquido", "LIQUIDO"));
            }

            //Atualiza Dados
            HtmlInputCheckBox cbxInsert = (HtmlInputCheckBox)item.FindControl("cbxInsert");
            HtmlInputCheckBox cbxUpdate = (HtmlInputCheckBox)item.FindControl("cbxUpdate");
            HtmlInputHidden cod_servico = (HtmlInputHidden)item.FindControl("cod_servico");
            HtmlInputCheckBox cbxGeraTituloDebito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloDebito");
            HtmlInputCheckBox cbxGeraTituloCredito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloCredito");
            TextBox tbxHistoricoDebito = (TextBox)item.FindControl("tbxHistoricoDebito");
            TextBox tbxHistoricoCredito = (TextBox)item.FindControl("tbxHistoricoCredito");

            tbxHistoricoDebito.Attributes.Add("maxlength", tbxHistoricoDebito.MaxLength.ToString());
            tbxHistoricoCredito.Attributes.Add("maxlength", tbxHistoricoCredito.MaxLength.ToString());

            foreach (DataRow row in tbContabilizacaoServicos.Rows)
            {
                if (row["COD_SERVICO"] != DBNull.Value && Convert.ToInt32(row["COD_SERVICO"]) == Convert.ToInt32(cod_servico.Value))
                {
                    //Débito
                    if (row["COD_CONTA_DEBITO"] != DBNull.Value)
                        ddlContaDebito.SelectedValue = row["COD_CONTA_DEBITO"].ToString();

                    if (row["BRUTO_LIQUIDO_DEBITO"] != DBNull.Value)
                        ddlBrutoLiquidoDebito.SelectedValue = row["BRUTO_LIQUIDO_DEBITO"].ToString();

                    if (row["GERA_TITULO_DEBITO"] != DBNull.Value)
                        cbxGeraTituloDebito.Checked = Convert.ToBoolean(row["GERA_TITULO_DEBITO"]);

                    if (row["HISTORICO_DEBITO"] != DBNull.Value)
                        tbxHistoricoDebito.Text = row["HISTORICO_DEBITO"].ToString();
                    
                    //Crédito
                    if (row["COD_CONTA_CREDITO"] != DBNull.Value)
                        ddlContaCredito.SelectedValue = row["COD_CONTA_CREDITO"].ToString();

                    if (row["BRUTO_LIQUIDO_CREDITO"] != DBNull.Value)
                        ddlBrutoLiquidoCredito.SelectedValue = row["BRUTO_LIQUIDO_CREDITO"].ToString();

                    if (row["GERA_TITULO_CREDITO"] != DBNull.Value)
                        cbxGeraTituloCredito.Checked = Convert.ToBoolean(row["GERA_TITULO_CREDITO"]);

                    if (row["HISTORICO_CREDITO"] != DBNull.Value)
                        tbxHistoricoCredito.Text = row["HISTORICO_CREDITO"].ToString();

                    cbxUpdate.Checked = true;
                    break;
                }
            }

            if (cbxUpdate.Checked == false)
            {
                cbxInsert.Checked = true;
            }
        }
    }

    //Repeater Retenções
    protected virtual void repeaterRetencoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            //Contas
            DropDownList ddlContaDebito = (DropDownList)item.FindControl("ddlContaDebito");
            DropDownList ddlContaCredito = (DropDownList)item.FindControl("ddlContaCredito");

            if (ddlContaDebito != null && ddlContaCredito != null)
            {
                ddlContaDebito.DataSource = tbContas;
                ddlContaDebito.DataTextField = "DESCRICAO";
                ddlContaDebito.DataValueField = "COD_CONTA";
                ddlContaDebito.DataBind();
                ddlContaDebito.Items.Insert(0, new ListItem("Escolha", "0"));

                ddlContaCredito.DataSource = tbContas;
                ddlContaCredito.DataTextField = "DESCRICAO";
                ddlContaCredito.DataValueField = "COD_CONTA";
                ddlContaCredito.DataBind();
                ddlContaCredito.Items.Insert(0, new ListItem("Escolha", "0"));
            }

            //Terceiros
            DropDownList ddlTerceiroDebito = (DropDownList)item.FindControl("ddlTerceiroDebito");
            DropDownList ddlTerceiroCredito = (DropDownList)item.FindControl("ddlTerceiroCredito");

            if (ddlTerceiroDebito != null && ddlTerceiroCredito != null)
            {
                ddlTerceiroDebito.DataSource = tbTerceiros;
                ddlTerceiroDebito.DataTextField = "NOME_RAZAO_SOCIAL";
                ddlTerceiroDebito.DataValueField = "COD_EMPRESA";
                ddlTerceiroDebito.DataBind();
                ddlTerceiroDebito.Items.Insert(0, new ListItem("Escolha", "0"));

                ddlTerceiroCredito.DataSource = tbTerceiros;
                ddlTerceiroCredito.DataTextField = "NOME_RAZAO_SOCIAL";
                ddlTerceiroCredito.DataValueField = "COD_EMPRESA";
                ddlTerceiroCredito.DataBind();
                ddlTerceiroCredito.Items.Insert(0, new ListItem("Escolha", "0"));
            }

            //Atualiza Dados
            HtmlInputCheckBox cbxInsert = (HtmlInputCheckBox)item.FindControl("cbxInsert");
            HtmlInputCheckBox cbxUpdate = (HtmlInputCheckBox)item.FindControl("cbxUpdate");
            HtmlInputHidden cod_retencao = (HtmlInputHidden)item.FindControl("cod_retencao");
            HtmlInputCheckBox cbxGeraTituloDebito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloDebito");
            HtmlInputCheckBox cbxGeraTituloCredito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloCredito");
            TextBox tbxHistoricoDebito = (TextBox)item.FindControl("tbxHistoricoDebito");
            TextBox tbxHistoricoCredito = (TextBox)item.FindControl("tbxHistoricoCredito");

            tbxHistoricoDebito.Attributes.Add("maxlength", tbxHistoricoDebito.MaxLength.ToString());
            tbxHistoricoCredito.Attributes.Add("maxlength", tbxHistoricoCredito.MaxLength.ToString());

            foreach (DataRow row in tbContabilizacaoRetencoes.Rows)
            {
                if (row["COD_RETENCAO"] != DBNull.Value && Convert.ToInt32(row["COD_RETENCAO"]) == Convert.ToInt32(cod_retencao.Value))
                {
                    //Débito
                    if (row["COD_CONTA_DEBITO"] != DBNull.Value)
                        ddlContaDebito.SelectedValue = row["COD_CONTA_DEBITO"].ToString();

                    if (row["GERA_TITULO_DEBITO"] != DBNull.Value)
                        cbxGeraTituloDebito.Checked = Convert.ToBoolean(row["GERA_TITULO_DEBITO"]);

                    if (row["COD_TERCEIRO_DEBITO"] != DBNull.Value)
                        ddlTerceiroDebito.SelectedValue = row["COD_TERCEIRO_DEBITO"].ToString();

                    if (row["HISTORICO_DEBITO"] != DBNull.Value)
                        tbxHistoricoDebito.Text = row["HISTORICO_DEBITO"].ToString();

                    //Crédito
                    if (row["COD_CONTA_CREDITO"] != DBNull.Value)
                        ddlContaCredito.SelectedValue = row["COD_CONTA_CREDITO"].ToString();

                    if (row["GERA_TITULO_CREDITO"] != DBNull.Value)
                        cbxGeraTituloCredito.Checked = Convert.ToBoolean(row["GERA_TITULO_CREDITO"]);

                    if (row["COD_TERCEIRO_CREDITO"] != DBNull.Value)
                        ddlTerceiroCredito.SelectedValue = row["COD_TERCEIRO_CREDITO"].ToString();

                    if (row["HISTORICO_CREDITO"] != DBNull.Value)
                        tbxHistoricoCredito.Text = row["HISTORICO_CREDITO"].ToString();

                    cbxUpdate.Checked = true;
                    break;
                }
            }

            if (cbxUpdate.Checked == false)
            {
                cbxInsert.Checked = true;
            }
        }
    }

    //Repeater Tributos
    protected virtual void repeaterTributos_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Separator)
        {
            RepeaterItem item = e.Item;

            //Contas
            DropDownList ddlContaDebito = (DropDownList)item.FindControl("ddlContaDebito");
            DropDownList ddlContaCredito = (DropDownList)item.FindControl("ddlContaCredito");

            if (ddlContaDebito != null && ddlContaCredito != null)
            {
                ddlContaDebito.DataSource = tbContas;
                ddlContaDebito.DataTextField = "DESCRICAO";
                ddlContaDebito.DataValueField = "COD_CONTA";
                ddlContaDebito.DataBind();
                ddlContaDebito.Items.Insert(0, new ListItem("Escolha", "0"));

                ddlContaCredito.DataSource = tbContas;
                ddlContaCredito.DataTextField = "DESCRICAO";
                ddlContaCredito.DataValueField = "COD_CONTA";
                ddlContaCredito.DataBind();
                ddlContaCredito.Items.Insert(0, new ListItem("Escolha", "0"));
            }

            //Terceiros
            DropDownList ddlTerceiroDebito = (DropDownList)item.FindControl("ddlTerceiroDebito");
            DropDownList ddlTerceiroCredito = (DropDownList)item.FindControl("ddlTerceiroCredito");

            if (ddlTerceiroDebito != null && ddlTerceiroCredito != null)
            {
                ddlTerceiroDebito.DataSource = tbTerceiros;
                ddlTerceiroDebito.DataTextField = "NOME_RAZAO_SOCIAL";
                ddlTerceiroDebito.DataValueField = "COD_EMPRESA";
                ddlTerceiroDebito.DataBind();
                ddlTerceiroDebito.Items.Insert(0, new ListItem("Escolha", "0"));

                ddlTerceiroCredito.DataSource = tbTerceiros;
                ddlTerceiroCredito.DataTextField = "NOME_RAZAO_SOCIAL";
                ddlTerceiroCredito.DataValueField = "COD_EMPRESA";
                ddlTerceiroCredito.DataBind();
                ddlTerceiroCredito.Items.Insert(0, new ListItem("Escolha", "0"));
            }

            //Atualiza Dados
            HtmlInputCheckBox cbxInsert = (HtmlInputCheckBox)item.FindControl("cbxInsert");
            HtmlInputCheckBox cbxUpdate = (HtmlInputCheckBox)item.FindControl("cbxUpdate");
            HtmlInputHidden cod_tributo = (HtmlInputHidden)item.FindControl("cod_tributo");
            HtmlInputCheckBox cbxGeraTituloDebito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloDebito");
            HtmlInputCheckBox cbxGeraTituloCredito = (HtmlInputCheckBox)item.FindControl("cbxGeraTituloCredito");
            TextBox tbxHistoricoDebito = (TextBox)item.FindControl("tbxHistoricoDebito");
            TextBox tbxHistoricoCredito = (TextBox)item.FindControl("tbxHistoricoCredito");

            tbxHistoricoDebito.Attributes.Add("maxlength", tbxHistoricoDebito.MaxLength.ToString());
            tbxHistoricoCredito.Attributes.Add("maxlength", tbxHistoricoCredito.MaxLength.ToString());

            foreach (DataRow row in tbContabilizacaoTributos.Rows)
            {
                if (row["COD_TRIBUTO"] != DBNull.Value && Convert.ToInt32(row["COD_TRIBUTO"]) == Convert.ToInt32(cod_tributo.Value))
                {
                    //Débito
                    if (row["COD_CONTA_DEBITO"] != DBNull.Value)
                        ddlContaDebito.SelectedValue = row["COD_CONTA_DEBITO"].ToString();

                    if (row["GERA_TITULO_DEBITO"] != DBNull.Value)
                        cbxGeraTituloDebito.Checked = Convert.ToBoolean(row["GERA_TITULO_DEBITO"]);

                    if (row["COD_TERCEIRO_DEBITO"] != DBNull.Value)
                        ddlTerceiroDebito.SelectedValue = row["COD_TERCEIRO_DEBITO"].ToString();

                    if (row["HISTORICO_DEBITO"] != DBNull.Value)
                        tbxHistoricoDebito.Text = row["HISTORICO_DEBITO"].ToString();

                    //Crédito
                    if (row["COD_CONTA_CREDITO"] != DBNull.Value)
                        ddlContaCredito.SelectedValue = row["COD_CONTA_CREDITO"].ToString();

                    if (row["GERA_TITULO_CREDITO"] != DBNull.Value)
                        cbxGeraTituloCredito.Checked = Convert.ToBoolean(row["GERA_TITULO_CREDITO"]);

                    if (row["COD_TERCEIRO_CREDITO"] != DBNull.Value)
                        ddlTerceiroCredito.SelectedValue = row["COD_TERCEIRO_CREDITO"].ToString();

                    if (row["HISTORICO_CREDITO"] != DBNull.Value)
                        tbxHistoricoCredito.Text = row["HISTORICO_CREDITO"].ToString();

                    cbxUpdate.Checked = true;
                    break;
                }
            }

            if (cbxUpdate.Checked == false)
            {
                cbxInsert.Checked = true;
            }
        }
    }

    [WebMethod]
    public static string btnSalvar_Click(Contabilizacao contabilizacao)
    {
        Conexao _conn = new Conexao();
        Contabilizacao_Funcoes _Contabilizacao_Funcoes = new Contabilizacao_Funcoes(_conn);

        string Mensagem_Sucesso_Erro = _Contabilizacao_Funcoes.Validar(contabilizacao.cod_conta_diferenca, contabilizacao.debito_credito, contabilizacao.historico, 
            contabilizacao.List_Servico, contabilizacao.List_Retencao, contabilizacao.List_Tributo);

        if (string.IsNullOrEmpty(Mensagem_Sucesso_Erro))
        {
            _Contabilizacao_Funcoes.Salvar(contabilizacao.cod_emitente, contabilizacao.cod_conta_diferenca, contabilizacao.debito_credito, contabilizacao.historico,
                contabilizacao.gera_titulo, contabilizacao.insert, contabilizacao.update, 
                contabilizacao.List_Servico, contabilizacao.List_Retencao, contabilizacao.List_Tributo);

            Mensagem_Sucesso_Erro = "reload";
        }
        return Mensagem_Sucesso_Erro;
    }
}