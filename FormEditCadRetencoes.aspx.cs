using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormEditCadRetencoes : BaseEditCadForm
{
    private Retencao retencao;
    private Emitente emitente;
    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbRetencao = new DataTable("tbRetencao");
    private DataTable tbEmitentes_Selecionados = new DataTable("tbEmitentes_Selecionados");

    public FormEditCadRetencoes()
        :base("FATURAMENTO_CADASTROS_RETENCOES")
    {
        retencao = new Retencao(_conn);
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
        addSubTitulo("Retenções", "FormGridRetencoes.aspx");

        if (_cadastro)
            subTitulo.Text = "Novo";
        else
            subTitulo.Text = "Editar";

        if (!Page.IsPostBack)
        {
            dsDados.Tables.Add(tbEmitentes);
            repeaterDados.DataSource = dsDados;
            repeaterDados.DataMember = dsDados.Tables["tbEmitentes"].TableName;
            repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);

            tbEmitentes.Clear();
            emitente.lista_Emitentes(ref tbEmitentes);
            repeaterDados.DataBind();

            retencao.Load_Sys_Retencoes(ref tbRetencao);

            ComboRetencao.DataSource = tbRetencao;
            ComboRetencao.DataTextField = "Descricao";
            ComboRetencao.DataValueField = "Cod_Retencao";
            ComboRetencao.DataBind();

            if (!_cadastro)
            {
                retencao.cod_retencao = Convert.ToInt32(Request.QueryString["id"]);
                retencao.load();

                textNome.Text = retencao.nome;
                textAliquota.Text = retencao.aliquota;
                textApresentacao.Text = retencao.apresentacao;
                ComboRetencao.SelectedValue = retencao.Cod_Retencoes_Sys.ToString();

                retencao.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);

                foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                {
                    int COD_EMITENTE_SELECIONADOS = Convert.ToInt32(row["COD_EMITENTE"]);

                    foreach (RepeaterItem item in repeaterDados.Items)
                    {
                        if (item.ItemType != ListItemType.Separator)
                        {
                            HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                            int COD_EMITENTE = Convert.ToInt32(check.Value);

                            if (COD_EMITENTE_SELECIONADOS == COD_EMITENTE)
                            {
                                check.Checked = true;
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

        retencao.nome = textNome.Text;
        retencao.aliquota = textAliquota.Text;
        retencao.apresentacao = textApresentacao.Text;
        retencao.Cod_Retencoes_Sys = Convert.ToInt32(ComboRetencao.SelectedValue);

        List<string> erros = new List<string>();
        if (_cadastro) //Novo Cadastro
        {
            erros = retencao.novo();

            if (erros.Count == 0)
            {
                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");

                        if (check.Checked == true)
                        {
                            retencao.cod_emitente = Convert.ToInt32(check.Value);
                            retencao.insert_Emitentes_Selecionados();
                        }
                    }
                }
            }
        }
        else //Alterar Cadastro
        {
            retencao.cod_retencao = Convert.ToInt32(Request.QueryString["id"]);
            erros = retencao.alterar();

            if (erros.Count == 0)
            {
                retencao.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);
                int COD_EMITENTE_ATUAL = 0;
                int COD_EMITENTE_ANTERIOR = 0;

                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                        COD_EMITENTE_ATUAL = Convert.ToInt32(check.Value);
                        bool Controle = false;

                        if (check.Checked == true) //INSERT
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
                            if (Controle == false)
                            {
                                retencao.cod_emitente = COD_EMITENTE_ATUAL;
                                retencao.insert_Emitentes_Selecionados();
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
                                retencao.cod_emitente = COD_EMITENTE_ATUAL;
                                retencao.delete_Emitentes_Deselecionados();
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
            Response.Redirect("FormGridRetencoes.aspx");
    }
}