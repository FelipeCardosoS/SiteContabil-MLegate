using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormEditCadPrestacaoServicos : BaseEditCadForm
{
    private PrestacaoServico prestacao_servico;
    private Emitente emitente;
    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbEmitentes_Selecionados = new DataTable("tbEmitentes_Selecionados");

    public FormEditCadPrestacaoServicos()
        :base("FATURAMENTO_CADASTROS_PRESTACAO_SERVICOS")
    {
        prestacao_servico = new PrestacaoServico(_conn);
        emitente = new Emitente(_conn);
    }

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
    }

    protected override void montaTela()
    {
        base.montaTela();
        addSubTitulo("Prestação de Serviços", "FormGridPrestacaoServicos.aspx");

        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {
            textDescricao.Attributes.Add("maxlength", textDescricao.MaxLength.ToString());

            dsDados.Tables.Add(tbEmitentes);
            repeaterDados.DataSource = dsDados;
            repeaterDados.DataMember = dsDados.Tables["tbEmitentes"].TableName;
            repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

            tbEmitentes.Clear();
            emitente.lista_Emitentes(ref tbEmitentes);
            repeaterDados.DataBind();

            if (!_cadastro)
            {
                prestacao_servico.cod_prestacao_servico = Convert.ToInt32(Request.QueryString["id"]);
                prestacao_servico.load();

                textNome.Text = prestacao_servico.nome;
                textDescricao.Text = prestacao_servico.descricao;

                prestacao_servico.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);

                foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                {
                    int COD_EMITENTE_SELECIONADOS = Convert.ToInt32(row["COD_EMITENTE"]);
                    bool PADRAO_SELECIONADOS;

                    if (row["PADRAO"] == DBNull.Value)
                        PADRAO_SELECIONADOS = false;
                    else
                        PADRAO_SELECIONADOS = Convert.ToBoolean(row["PADRAO"]);

                    foreach (RepeaterItem item in repeaterDados.Items)
                    {
                        if (item.ItemType != ListItemType.Separator)
                        {
                            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                            HtmlInputCheckBox check_padrao = (HtmlInputCheckBox)item.FindControl("check_padrao");

                            int COD_EMITENTE = Convert.ToInt32(check.Value);

                            if (COD_EMITENTE_SELECIONADOS == COD_EMITENTE)
                            {
                                check.Checked = true;

                                if (PADRAO_SELECIONADOS == true)
                                {
                                    check_padrao.Checked = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    protected override void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e) { }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        botaoSalvar.Enabled = false;

        prestacao_servico.nome = textNome.Text;
        prestacao_servico.descricao = textDescricao.Text;

        List<string> erros = new List<string>();
        if (_cadastro) //Novo Cadastro
        {
            erros = prestacao_servico.novo();

            if (erros.Count == 0)
            {
                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");

                        if (check.Checked == true)
                        {
                            HtmlInputCheckBox check_padrao = (HtmlInputCheckBox)item.FindControl("check_padrao");

                            prestacao_servico.cod_emitente = Convert.ToInt32(check.Value);
                            prestacao_servico.padrao = check_padrao.Checked;
                            prestacao_servico.insert_Emitentes_Selecionados();
                        }
                    }
                }
            }
        }
        else //Alterar Cadastro
        {
            prestacao_servico.cod_prestacao_servico = Convert.ToInt32(Request.QueryString["id"]);
            erros = prestacao_servico.alterar();

            if (erros.Count == 0)
            {
                prestacao_servico.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);
                int COD_EMITENTE_ATUAL;
                int COD_EMITENTE_ANTERIOR;
                bool PADRAO_ATUAL;
                bool PADRAO_ANTERIOR;

                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                        HtmlInputCheckBox check_padrao = (HtmlInputCheckBox)item.FindControl("check_padrao");

                        COD_EMITENTE_ATUAL = Convert.ToInt32(check.Value);
                        PADRAO_ATUAL = check_padrao.Checked;

                        bool Controle = false;
                        bool Update_Padrao = true;

                        if (check.Checked == true) //INSERT
                        {
                            foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                            {
                                COD_EMITENTE_ANTERIOR = Convert.ToInt32(row["COD_EMITENTE"]);

                                if (row["PADRAO"] == DBNull.Value)
                                    PADRAO_ANTERIOR = false;
                                else
                                    PADRAO_ANTERIOR = Convert.ToBoolean(row["PADRAO"]);

                                if (COD_EMITENTE_ATUAL == COD_EMITENTE_ANTERIOR)
                                {
                                    Controle = true;

                                    if (PADRAO_ATUAL == PADRAO_ANTERIOR)
                                    {
                                        Update_Padrao = false;
                                    }
                                    else
                                    {
                                        Update_Padrao = true;
                                    }
                                    break;
                                }
                            }
                            if (Controle == false)
                            {
                                prestacao_servico.cod_emitente = COD_EMITENTE_ATUAL;
                                prestacao_servico.padrao = PADRAO_ATUAL;
                                prestacao_servico.insert_Emitentes_Selecionados();
                                Update_Padrao = false;
                            }

                            if (Update_Padrao == true)
                            {
                                prestacao_servico.cod_emitente = COD_EMITENTE_ATUAL;
                                prestacao_servico.padrao = PADRAO_ATUAL;
                                prestacao_servico.update_Emitentes_Padrao();
                            }
                        }
                        else //DELETE
                        {
                            foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                            {
                                COD_EMITENTE_ANTERIOR = Convert.ToInt32(row["COD_EMITENTE"]);

                                if (COD_EMITENTE_ATUAL == COD_EMITENTE_ANTERIOR)
                                {
                                    Controle = true;
                                    break;
                                }
                            }
                            if (Controle == true)
                            {
                                prestacao_servico.cod_emitente = COD_EMITENTE_ATUAL;
                                prestacao_servico.delete_Emitentes_Deselecionados();
                            }
                        }
                    }
                }
            }
        }
        botaoSalvar.Enabled = true;

        if (erros.Count > 0)
            errosFormulario(erros);
        else
            Response.Redirect("FormGridPrestacaoServicos.aspx");
    }
}