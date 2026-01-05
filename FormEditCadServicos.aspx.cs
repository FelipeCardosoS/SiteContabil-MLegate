using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class FormEditCadServicos : BaseEditCadForm
{
    private Servico servico;
    private Emitente emitente;
    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbEmitentes_Selecionados = new DataTable("tbEmitentes_Selecionados");

    public FormEditCadServicos()
        :base("FATURAMENTO_CADASTROS_SERVICOS")
    {
        servico = new Servico(_conn);
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
        addSubTitulo("Serviços", "FormGridServicos.aspx");

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

            if (!_cadastro)
            {
                servico.cod_servico = Convert.ToInt32(Request.QueryString["id"]);
                servico.load();

                textNome.Text = servico.nome;
                textCodServicoPrefeitura.Text = servico.cod_servico_prefeitura;
                textImpostos.Text = servico.impostos;

                servico.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);

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

        servico.nome = textNome.Text;
        servico.cod_servico_prefeitura = textCodServicoPrefeitura.Text;
        servico.impostos = textImpostos.Text.Replace(".", ",");

        List<string> erros = new List<string>();
        if (_cadastro) //Novo Cadastro
        {
            erros = servico.novo();

            if (erros.Count == 0)
            {
                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");

                        if (check.Checked == true)
                        {
                            servico.cod_emitente = Convert.ToInt32(check.Value);
                            servico.insert_Emitentes_Selecionados();
                        }
                    }
                }
            }
        }
        else //Alterar Cadastro
        {
            servico.cod_servico = Convert.ToInt32(Request.QueryString["id"]);
            erros = servico.alterar();

            if (erros.Count == 0)
            {
                servico.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);
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
                                servico.cod_emitente = COD_EMITENTE_ATUAL;
                                servico.insert_Emitentes_Selecionados();
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
                                servico.cod_emitente = COD_EMITENTE_ATUAL;
                                servico.delete_Emitentes_Deselecionados();
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
            Response.Redirect("FormGridServicos.aspx");
    }
}