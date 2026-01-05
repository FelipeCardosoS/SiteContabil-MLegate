using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormRelatorioEmitenteRecebimento : BaseForm
{
    private Emitente _Emitente;
    private EmissaoNF_Funcoes _EmissaoNF_Funcoes;

    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbTomadores = new DataTable("tbTomadores");

    public FormRelatorioEmitenteRecebimento()
        : base("DEFAULT")
    {
        _Emitente = new Emitente(_conn);
        _EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Recebimento das Empresas Emitentes";

        if (!Page.IsPostBack)
        {
            //Emitentes
            _Emitente.lista_Emitentes(ref tbEmitentes);
            ddlEmitente.DataSource = tbEmitentes;
            ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlEmitente.DataValueField = "COD_EMPRESA";
            ddlEmitente.DataBind();

            //Tomadores
            _EmissaoNF_Funcoes.lista_Tomadores(ref tbTomadores);
            ddlTomador.DataSource = tbTomadores;
            ddlTomador.DataTextField = "NOME_RAZAO_SOCIAL_TOMADOR";
            ddlTomador.DataValueField = "COD_TOMADOR";
            ddlTomador.DataBind();

            List<string> List_Coluna = new List<string>();
            List<List<string>> List_Linha = new List<List<string>>();

            foreach (DataRow item in tbTomadores.Rows)
            {
                List_Coluna = new List<string>();
                List_Coluna.Add(item[0].ToString());
                List_Coluna.Add(item[1].ToString());
                List_Coluna.Add(item[2].ToString());

                List_Linha.Add(List_Coluna);
            }
            HF_Tomador.Value = new JavaScriptSerializer().Serialize(List_Linha);
        }
        else
            tbxPage_IsPostBack.Value = "true";
    }

    protected void rptEmitenteRecebimento_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        //Validação do Período: Data Inicial e Data Final.
        List<string> Mensagem_Sucesso_Erro = new List<string>();
        string Mensagem_Erro;

        if (tbxDeRec.Value != "")
        {
            Mensagem_Erro = _EmissaoNF_Funcoes.Verifica_Data(tbxDeRec.Value, "Período Inicial do Recebimento");
            if (Mensagem_Erro != "") //Se o Período Inicial do Recebimento estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"
        }

        if (tbxAteRec.Value != "")
        {
            Mensagem_Erro = _EmissaoNF_Funcoes.Verifica_Data(tbxAteRec.Value, "Período Final do Recebimento");
            if (Mensagem_Erro != "") //Se o Período Final do Recebimento estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"
        }

        if (Mensagem_Sucesso_Erro.Count == 0 && tbxDeRec.Value != "" && tbxAteRec.Value != "" && Convert.ToDateTime(tbxDeRec.Value) > Convert.ToDateTime(tbxAteRec.Value))
            Mensagem_Sucesso_Erro.Add("Período Recebimento: Informe uma Data Final igual ou posterior a Data Inicial.");

        if (tbxDe.Value != "")
        {
            Mensagem_Erro = _EmissaoNF_Funcoes.Verifica_Data(tbxDe.Value, "Período Inicial do Faturamento");
            if (Mensagem_Erro != "") //Se o Período Inicial do Faturamento estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"
        }

        if (tbxAte.Value != "")
        {
            Mensagem_Erro = _EmissaoNF_Funcoes.Verifica_Data(tbxAte.Value, "Período Final do Faturamento");
            if (Mensagem_Erro != "") //Se o Período Final do Faturamento estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"
        }

        if (Mensagem_Sucesso_Erro.Count == 0 && tbxDe.Value != "" && tbxAte.Value != "" && Convert.ToDateTime(tbxDe.Value) > Convert.ToDateTime(tbxAte.Value))
            Mensagem_Sucesso_Erro.Add("Período Faturamento: Informe uma Data Final igual ou posterior a Data Inicial.");

        if (Mensagem_Sucesso_Erro.Count >= 1)
        {
            errosFormulario(Mensagem_Sucesso_Erro);
            tbxDeRec.Value = "";
            tbxAteRec.Value = "";
            tbxDe.Value = "";
            tbxAte.Value = "";
            tbxPage_IsPostBack.Value = "";
        }
        else //Validação OK
        {
            string Selecionados = "";
            string Filtros = "";
            int Contador = 0;

            foreach (ListItem item in ddlEmitente.Items)
            {
                if (item.Selected == true)
                {
                    Selecionados += ", " + item;
                    Contador++;
                }
            }

            if (Contador == 1)
                Filtros += "Emitente: " + Selecionados.Substring(2);
            else if (Contador >= 2)
                Filtros += "Emitentes: " + Selecionados.Substring(2);

            Selecionados = "";
            Contador = 0;

            foreach (ListItem item in ddlTomador.Items)
            {
                if (item.Selected == true)
                {
                    Selecionados += ", " + item;
                    Contador++;
                }
            }

            if (Contador == 1)
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Tomador: " + Selecionados.Substring(2);
            }
            else if (Contador >= 2)
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Tomadores: " + Selecionados.Substring(2);
            }

            if (tbxDeRec.Value != "" && tbxAteRec.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Recebimento: de " + tbxDeRec.Value + " até " + tbxAteRec.Value;
            }
            else if (tbxDeRec.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Recebimento: a partir de " + tbxDeRec.Value;
            }
            else if (tbxAteRec.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Recebimento: até a data " + tbxAteRec.Value;
            }

            if (tbxDe.Value != "" && tbxAte.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Faturamento: de " + tbxDe.Value + " até " + tbxAte.Value;
            }
            else if (tbxDe.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Faturamento: a partir de " + tbxDe.Value;
            }
            else if (tbxAte.Value != "")
            {
                if (Filtros != "")
                {
                    Filtros += "\n\n";
                }
                Filtros += "Período Faturamento: até a data " + tbxAte.Value;
            }

            ReportParameter[] parametro = new ReportParameter[3];
            parametro[0] = new ReportParameter("Filtros", Filtros);
            parametro[1] = new ReportParameter("GrupoEconomico", cbxGrupoEconomico.Checked.ToString());
            parametro[2] = new ReportParameter("ValorLiquido", ddlOrdenacao.Value);
            rptEmitenteRecebimento.LocalReport.SetParameters(parametro);
        }
    }
}