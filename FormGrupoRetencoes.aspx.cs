//Projeto interrompido em 2018. Sem previsão de retomada.
//Menu (Banco de Dados): "CAD_MODULOS" e "CAD_TAREFAS" já possuem estrutura (FATURAMENTO_CADASTROS_GRUPO_RETENCOES).
//Engloba 5 Arquivos: "FormGrupoRetencoes.js", "FormGrupoRetencoes.aspx", "FormGrupoRetencoes.aspx.cs", "GrupoRetencao.cs" e "GrupoRetencoesDAO.cs".

using System;
using System.Data;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FormGrupoRetencoes : BaseForm
{
    private Emitente Emitente;
    private GrupoRetencao GrupoRetencao;
    private Retencao Retencao;

    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbGrupoRetencoes = new DataTable("tbGrupoRetencoes");
    private DataTable tbRetencoes = new DataTable("tbRetencoes");

    public FormGrupoRetencoes()
        : base("FATURAMENTO_CADASTROS_GRUPO_RETENCOES")
    {
        Emitente = new Emitente(_conn);
        GrupoRetencao = new GrupoRetencao(_conn);
        Retencao = new Retencao(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Grupo de Retenções";
        subTitulo.Text = "Grupo de Retenções";

        if (!Page.IsPostBack)
        {
            Emitente.lista_Emitentes(ref tbEmitentes);
            ddlEmitente.DataSource = tbEmitentes;
            ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlEmitente.DataValueField = "COD_EMPRESA";
            ddlEmitente.DataBind();
            ddlEmitente.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void verificaTarefas()
    {
        bool aceitaGerar_NF = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "SALVAR")
                aceitaGerar_NF = true;
        }

        if (!aceitaGerar_NF)
        {
            ddlEmitente.Enabled = false;
            btnSalvar.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Você precisa de permissão para Emitir Nota Fiscal.');", true);
        }
    }

    protected void ddlEmitente_Changed(object sender, EventArgs e)
    {
        int cod_emitente = Convert.ToInt32(ddlEmitente.SelectedValue);

        if (cod_emitente != 0)
        {
            GrupoRetencao.lista_Grupo_Retencoes(ref tbGrupoRetencoes, cod_emitente);
            rptrGrupoRetencoes.DataSource = tbGrupoRetencoes;
            rptrGrupoRetencoes.DataMember = tbGrupoRetencoes.TableName;
            rptrGrupoRetencoes.ItemDataBound += new RepeaterItemEventHandler(rptrGrupoRetencoes_ItemDataBound);
            rptrGrupoRetencoes.DataBind();
        }
        else
            Response.Redirect("FormGrupoRetencoes.aspx");
    }

    protected virtual void rptrGrupoRetencoes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        RadioButton rbtnCodGR = (RadioButton)e.Item.FindControl("rbtnCodGR");
        HtmlInputCheckBox cbxAtivoGR = (HtmlInputCheckBox)e.Item.FindControl("cbxAtivoGR");

        string script = "SetUniqueRadioButton('rptrGrupoRetencoes.*GrupoRetencoes',this)";
        rbtnCodGR.Attributes.Add("onclick", script);

        cbxAtivoGR.Checked = true;
        //rbtnCodGR.Attributes.Add("value", "1");
    }

    protected void rbtnCodGR_CheckedChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptrGrupoRetencoes.Items)
        {
            RadioButton teste = (RadioButton)item.FindControl("rbtnCodGR");
            if (teste.Checked)
            {
                string aaaa = teste.Attributes["value"].ToString();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('" + aaaa + "');", true);
            }
        }

    }

    [WebMethod]
    public static string btnSalvar_Click(EmissaoNF emissao_nf)
    {
        Conexao _conn = new Conexao();
        EmissaoNF_Funcoes _EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);

        string Mensagem_Sucesso_Erro = _EmissaoNF_Funcoes.Validar(emissao_nf.cod_emitente, emissao_nf.cod_tomador,
            emissao_nf.data_emissao_rps, emissao_nf.data_rps, emissao_nf.data_competencia,
            emissao_nf.cod_natureza_operacao, emissao_nf.cod_prestacao_servico, emissao_nf.valor_total_liquido, emissao_nf.valor_total_vencimento,
            emissao_nf.List_Servico_Job, emissao_nf.List_Vencimento, emissao_nf.List_Narrativa);

        if (string.IsNullOrEmpty(Mensagem_Sucesso_Erro)) //Se For Sucesso
        {
            _EmissaoNF_Funcoes.Salvar(
                emissao_nf.cod_emitente,
                emissao_nf.cod_tomador,
                emissao_nf.data_emissao_rps,
                emissao_nf.data_competencia,
                emissao_nf.cod_natureza_operacao,
                emissao_nf.nome_natureza_operacao,
                emissao_nf.descricao_natureza_operacao,
                emissao_nf.natureza_operacao,
                emissao_nf.cod_prestacao_servico,
                emissao_nf.nome_prestacao_servico,
                emissao_nf.descricao_prestacao_servico,
                emissao_nf.valor_total_servico_job,
                emissao_nf.observacoes,

                // --- INÍCIO DO AJUSTE (4 Zeros para os campos da Reforma) ---
                0, // Valor IBS
                0, // Aliquota IBS
                0, // Valor CBS
                0, // Aliquota CBS
                   // --- FIM DO AJUSTE ---

                emissao_nf.List_Retencao,
                emissao_nf.List_Servico_Job,
                emissao_nf.List_Vencimento,
                emissao_nf.List_Narrativa
            );

            Mensagem_Sucesso_Erro = "/FormConfirmacaoContabilizacao.aspx?id=" + _EmissaoNF_Funcoes.cod_faturamento_nf;
        }
        return Mensagem_Sucesso_Erro;
    }
}