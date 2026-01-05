using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class FormEditCadTributos : BaseEditCadForm
{
    private Tributo tributo;
    private Emitente emitente;
    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbTributo = new DataTable("tbTributo");
    private DataTable tbEmitentes_Selecionados = new DataTable("tbEmitentes_Selecionados");

    public FormEditCadTributos()
        : base("FATURAMENTO_CADASTROS_TRIBUTOS")
    {
        tributo = new Tributo(_conn);
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
            Hdid.Text = Request.QueryString["id"].ToString();
        }
       
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
    }

    protected override void montaTela()
    {
        base.montaTela();
        addSubTitulo("Impostos Sobre Vendas", "FormGridTributos.aspx");

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

            tributo.Load_Sys_Tributos(ref tbTributo);

            ComboTributo.DataSource = tbTributo;
            ComboTributo.DataTextField = "Descricao";
            ComboTributo.DataValueField = "Cod_Tributo";
            ComboTributo.DataBind();

            if (!_cadastro)
            {
                tributo.cod_tributo = Convert.ToInt32(Request.QueryString["id"]);
                tributo.load();

                textNome.Text = tributo.nome;
                textAliquota.Text = tributo.aliquota;
                ckDestacado.Checked = tributo.Destacado;
                ComboTributo.SelectedValue = tributo.Cod_Tributos_Sys.ToString();

                tributo.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);

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

        tributo.nome = textNome.Text;
        tributo.aliquota = textAliquota.Text;
        tributo.Destacado = ckDestacado.Checked;
        tributo.Cod_Tributos_Sys = Convert.ToInt32(ComboTributo.SelectedValue);

        List<string> erros = new List<string>();
        if (_cadastro) //Novo Cadastro
        {
            erros = tributo.novo();

            if (erros.Count == 0)
            {
                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                        HtmlInputText TextServicos = (HtmlInputText)item.FindControl("TxtHide");

                        if (check.Checked == true)
                        {
                            tributo.cod_emitente = Convert.ToInt32(check.Value);
                            tributo.insert_Emitentes_Selecionados(TextServicos.Value);
                        }
                    }
                }
            }
        }
        else //Alterar Cadastro
        {
            tributo.cod_tributo = Convert.ToInt32(Request.QueryString["id"]);
            erros = tributo.alterar();

            if (erros.Count == 0)
            {
                //tributo.lista_Emitentes_Selecionados(ref tbEmitentes_Selecionados);
                //int COD_EMITENTE_ATUAL = 0;
                //int COD_EMITENTE_ANTERIOR = 0;

                foreach (RepeaterItem item in repeaterDados.Items)
                {
                    if (item.ItemType != ListItemType.Separator)
                    {
                        HtmlInputCheckBox check = (HtmlInputCheckBox)item.FindControl("check");
                        HtmlInputText TextServicos = (HtmlInputText)item.FindControl("TxtHide");
                        //COD_EMITENTE_ATUAL = Convert.ToInt32(check.Value);
                        //bool Controle = false;

                        if (check.Checked == true) //INSERT
                        {
                            //foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                            //{
                            //    COD_EMITENTE_ANTERIOR = Convert.ToInt32(row["COD_EMITENTE"]);

                            //    if (COD_EMITENTE_ATUAL == COD_EMITENTE_ANTERIOR)
                            //    {
                            //        Controle = true;
                            //        break;
                            //    }
                            //}
                            //if (Controle == false)
                            //{
                            //tributo.cod_emitente = COD_EMITENTE_ATUAL;
                            erros.AddRange(tributo.Valida_Emitente(TextServicos.Value));
                            if (erros.Count == 0)
                            {
                                tributo.cod_emitente = Convert.ToInt32(check.Value);
                                tributo.delete_Emitentes_Deselecionados();
                                tributo.insert_Emitentes_Selecionados(TextServicos.Value);
                            }                            
                            //}
                        }
                        else //DELETE
                        {
                            //foreach (DataRow row in tbEmitentes_Selecionados.Rows)
                            //{
                            //    COD_EMITENTE_ANTERIOR = Convert.ToInt32(row["COD_EMITENTE"]);

                            //    if (COD_EMITENTE_ATUAL == COD_EMITENTE_ANTERIOR)
                            //    {
                            //        Controle = true;
                            //        break;
                            //    }
                            //}
                            //if (Controle == true)
                            //{
                            //    tributo.cod_emitente = COD_EMITENTE_ATUAL;
                                tributo.cod_emitente = Convert.ToInt32(check.Value);
                                tributo.delete_Emitentes_Deselecionados();
                            //}
                        }
                    }
                }
            }
        }
        botaoSalvar.Enabled = true;

        if (erros.Count > 0)
            errosFormulario(erros);
        else
            Response.Redirect("FormGridTributos.aspx");
    }

    [WebMethod]
    public static string CarregaServicos(int Cod_Emitente)
    {
        Conexao _Conn = new Conexao();
        Servico _Servico = new Servico(_Conn);

        return new JavaScriptSerializer().Serialize(_Servico.List_Servico(_Conn, Cod_Emitente));
    }

    [WebMethod]
    public static string CarregaServicosEmitente(int Cod_Tributo, int Cod_Emitente)
    {
        Conexao _Conn = new Conexao();
        Servico _Servico = new Servico(_Conn);

        return new JavaScriptSerializer().Serialize(_Servico.List_ServicoEmitente(_Conn, Cod_Tributo, Cod_Emitente));
    }
}