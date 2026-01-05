using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;

public partial class FormRelatorioGerado : Permissao
{
    Relatorios relatorio;
    DataTable tb = new DataTable("tbRelatorio");
    DataTable tb2 = new DataTable("tbRelatorio2"); // 2º relatório - Relatório Comparativo
    Empresa empresa;
    
    private int[] detalheMovFinanc = new int[4];
    string log = "";

    public FormRelatorioGerado()
        : base("RELATORIO")
    {
        relatorio = new Relatorios(_conn);
        empresa = new Empresa(_conn);
    }

    protected void Page_Preload(object sender, EventArgs e)
    {
    }

    protected  void Page_Load(object sender, EventArgs e)
    {
        if (PreviousPage != null)
        {
            
            HiddenField hiddenRelatorio = PreviousPage.hdRelatorio;
            if (hiddenRelatorio != null)
            {
                string empresas;
                TextBox textPeriodo;
                TextBox textAteEncerramento;
                CheckBox checkContasEncerramento;
                CheckBox checkContasEncerramentoDRE;
                CheckBox checkDuasMoedas;
				CheckBox checkContasZeradas;
                TextBox textDiasVencimento;
                DropDownList comboCPCR;
                DropDownList comboOrdenacao;
                DropDownList comboContaDe;
                DropDownList comboContaAte;
                DropDownList comboDivisaoDe;
                DropDownList comboDivisaoAte;
                DropDownList comboLinhaNegocioDe;
                DropDownList comboLinhaNegocioAte;
                DropDownList comboClienteDe;
                DropDownList comboClienteAte;
                DropDownList comboJobDe;
                DropDownList comboJobAte;
                DropDownList comboTerDe;
                DropDownList comboTerAte;
                DropDownList comboMoeda;
                DropDownList comboGrupo;

                DropDownList comboDetalhamento1;
                DropDownList comboDetalhamento2;
                DropDownList comboDetalhamento3;
                DropDownList comboDetalhamento4;
                DropDownList comboDetalhamento5;

                string contaDe = null;
                string contaAte = null;
                string tipo = null;
                Nullable<int> tipoMoeda = null;
                Nullable<int> divisaoDe = null;
                Nullable<int> divisaoAte = null;
                Nullable<int> linhaNegocioDe = null;
                Nullable<int> linhaNegocioAte = null;
                Nullable<int> clienteDe = null;
                Nullable<int> clienteAte = null;
                Nullable<int> jobDe = null;
                Nullable<int> jobAte = null;
                Nullable<int> terDe = null;
                Nullable<int> terAte = null;
                string grupo = null;

                string detalhamento1 = null;
                string detalhamento2 = null;
                string detalhamento3 = null;
                string detalhamento4 = null;
                string detalhamento5 = null;
                int maxDetalhe = 0;
                List<string> erros = new List<string>();
                string filtros = "";
				int maxNivel = 0;

                switch (hiddenRelatorio.Value)
                {
                    #region Relatório Balancete
                    case "BALANCETE":

                        textPeriodo = PreviousPage.txtPeriodo;
                        checkContasEncerramento = PreviousPage.checkContasEncerr;
                        checkDuasMoedas = PreviousPage.checkBoxDuasMoedas;
						checkContasZeradas = PreviousPage.checkContasZeradas;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;
                        comboMoeda = PreviousPage.cmbMoeda;

                        textAteEncerramento = PreviousPage.textAteEncerramento;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;
                        comboDetalhamento5 = PreviousPage.cmbDetalhamento5;

                        empresas = PreviousPage.empresas_selecionadas;

                        tipoMoeda = null;
                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;

                        if (comboMoeda != null)
                        {
                            if (comboMoeda.SelectedValue == "0")
                                tipoMoeda = null;
                            else
                                tipoMoeda = Convert.ToInt32(comboMoeda.SelectedValue);
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

                        if (comboDetalhamento1 != null)
                        {
                            if (comboDetalhamento1.SelectedValue == "0")
                                detalhamento1 = null;
                            else
                            {
                                detalhamento1 = comboDetalhamento1.SelectedValue;

                                if (comboDetalhamento2 != null)
                                {
                                    if (comboDetalhamento2.SelectedValue == "0")
                                        detalhamento2 = null;
                                    else
                                    {
                                        detalhamento2 = comboDetalhamento2.SelectedValue;

                                        if (comboDetalhamento3 != null)
                                        {
                                            if (comboDetalhamento3.SelectedValue == "0")
                                                detalhamento3 = null;
                                            else
                                            {
                                                detalhamento3 = comboDetalhamento3.SelectedValue;

                                                if (comboDetalhamento4 != null)
                                                {
                                                    if (comboDetalhamento4.SelectedValue == "0")
                                                        detalhamento4 = null;
                                                    else
                                                    {
                                                        detalhamento4 = comboDetalhamento4.SelectedValue;

                                                        if (comboDetalhamento5 != null)
                                                        {
                                                            if (comboDetalhamento5.SelectedValue == "0")
                                                                detalhamento5 = null;
                                                            else
                                                            {
                                                                detalhamento5 = comboDetalhamento5.SelectedValue;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.balancete(tipoMoeda, Convert.ToDateTime(textPeriodo.Text),
                                    divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                                    jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb, checkContasEncerramento.Checked, empresas, Convert.ToString(textAteEncerramento.Text), checkContasZeradas.Checked);

                        if (checkDuasMoedas.Checked)
                        {
                            erros = relatorio.balancete(0, Convert.ToDateTime(textPeriodo.Text),
                                    divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                                    jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb2, checkContasEncerramento.Checked, empresas, Convert.ToString(textAteEncerramento.Text), checkContasZeradas.Checked);

                            var moedaPadrao = tb2.AsEnumerable();
                            var moedaSelecionada = tb.AsEnumerable();

                            string codDetalhe1 = "codDetalhe1", codDetalhe2 = "codDetalhe2", codDetalhe3 = "codDetalhe3", codDetalhe4 = "codDetalhe4", codDetalhe5 = "codDetalhe5";
                            Decimal saldo_ini_moeda = 0, debito_moeda = 0, credito_moeda = 0, saldo_fim_moeda = 0;

                            var novoDtLeft = from a in moedaPadrao.DefaultIfEmpty()
                                            join c in moedaSelecionada.DefaultIfEmpty()
                                            on new { 
                                                COD_CONTA = a == null ? "" : a.Field<string>("COD_CONTA"),
                                                codDetalhe1 = detalhamento1 == null ? 0 : a.Field<int>("codDetalhe1"),
                                                codDetalhe2 = detalhamento2 == null ? 0 : a.Field<int>("codDetalhe2"),
                                                codDetalhe3 = detalhamento3 == null ? 0 : a.Field<int>("codDetalhe3"),
                                                codDetalhe4 = detalhamento4 == null ? 0 : a.Field<int>("codDetalhe4"),
                                                codDetalhe5 = detalhamento5 == null ? 0 : a.Field<int>("codDetalhe5")
                                            } 
                                            equals new 
                                            {
                                                COD_CONTA = c == null ? "" : c.Field<string>("COD_CONTA"),
                                                codDetalhe1 = detalhamento1 == null ? 0 : c.Field<int>("codDetalhe1"),
                                                codDetalhe2 = detalhamento2 == null ? 0 : c.Field<int>("codDetalhe2"),
                                                codDetalhe3 = detalhamento3 == null ? 0 : c.Field<int>("codDetalhe3"),
                                                codDetalhe4 = detalhamento4 == null ? 0 : c.Field<int>("codDetalhe4"),
                                                codDetalhe5 = detalhamento5 == null ? 0 : c.Field<int>("codDetalhe5")
                                            }
                                            into dtTemp
                                            from c in dtTemp.DefaultIfEmpty()
                                            select new {
                                                COD_CONTA = a == null ? c.Field<string>("COD_CONTA") : a.Field<string>("COD_CONTA"),
                                                DESCRICAO = a == null ? c.Field<string>("DESCRICAO") : a.Field<string>("DESCRICAO"),
                                                saldo_ini = a == null ? 0 : a.Field<Decimal>("saldo_ini"),
                                                debito = a == null ? 0 : a.Field<Decimal>("debito"),
                                                credito = a == null ? 0 : a.Field<Decimal>("credito"),
                                                saldo_fim = a == null ? 0 : a.Field<Decimal>("saldo_fim"),
                                                saldo_ini_moeda = c != null ? c.Field<Decimal>("saldo_ini") : 0,
                                                debito_moeda = c != null ? c.Field<Decimal>("debito") : 0,
                                                credito_moeda = c != null ? c.Field<Decimal>("credito") : 0,
                                                saldo_fim_moeda = c != null ? (c.Field<Decimal>("saldo_ini") + c.Field<Decimal>("debito") - c.Field<Decimal>("credito")) : 0,
                                                codDetalhe1 = detalhamento1 == null ? 0 : a == null ? c.Field<int>("codDetalhe1") : a.Field<int>("codDetalhe1"),
                                                codDetalhe2 = detalhamento2 == null ? 0 : a == null ? c.Field<int>("codDetalhe2") : a.Field<int>("codDetalhe2"),
                                                codDetalhe3 = detalhamento3 == null ? 0 : a == null ? c.Field<int>("codDetalhe3") : a.Field<int>("codDetalhe3"),
                                                codDetalhe4 = detalhamento4 == null ? 0 : a == null ? c.Field<int>("codDetalhe4") : a.Field<int>("codDetalhe4"),
                                                codDetalhe5 = detalhamento5 == null ? 0 : a == null ? c.Field<int>("codDetalhe5") : a.Field<int>("codDetalhe5"),
                                                descDetalhe1 = detalhamento1 == null ? "" : a == null ? c.Field<string>("descDetalhe1") : a.Field<string>("descDetalhe1"),
                                                descDetalhe2 = detalhamento2 == null ? "" : a == null ? c.Field<string>("descDetalhe2") : a.Field<string>("descDetalhe2"),
                                                descDetalhe3 = detalhamento3 == null ? "" : a == null ? c.Field<string>("descDetalhe3") : a.Field<string>("descDetalhe3"),
                                                descDetalhe4 = detalhamento4 == null ? "" : a == null ? c.Field<string>("descDetalhe4") : a.Field<string>("descDetalhe4"),
                                                descDetalhe5 = detalhamento5 == null ? "" : a == null ? c.Field<string>("descDetalhe5") : a.Field<string>("descDetalhe5"),
                                                analitica = a.Field<int>("analitica")
                                            };

                            var novoDtRight = from a in moedaSelecionada.DefaultIfEmpty()
                                                join c in moedaPadrao.DefaultIfEmpty()
                                                on new
                                                {
                                                    COD_CONTA = a == null ? "" : a.Field<string>("COD_CONTA"),
                                                    codDetalhe1 = detalhamento1 == null ? 0 : a.Field<int>("codDetalhe1"),
                                                    codDetalhe2 = detalhamento2 == null ? 0 : a.Field<int>("codDetalhe2"),
                                                    codDetalhe3 = detalhamento3 == null ? 0 : a.Field<int>("codDetalhe3"),
                                                    codDetalhe4 = detalhamento4 == null ? 0 : a.Field<int>("codDetalhe4"),
                                                    codDetalhe5 = detalhamento5 == null ? 0 : a.Field<int>("codDetalhe5")
                                                }
                                                equals new
                                                {
                                                    COD_CONTA = c == null ? "" : c.Field<string>("COD_CONTA"),
                                                    codDetalhe1 = detalhamento1 == null ? 0 : c.Field<int>("codDetalhe1"),
                                                    codDetalhe2 = detalhamento2 == null ? 0 : c.Field<int>("codDetalhe2"),
                                                    codDetalhe3 = detalhamento3 == null ? 0 : c.Field<int>("codDetalhe3"),
                                                    codDetalhe4 = detalhamento4 == null ? 0 : c.Field<int>("codDetalhe4"),
                                                    codDetalhe5 = detalhamento5 == null ? 0 : c.Field<int>("codDetalhe5")
                                                }
                                                into dtTemp
                                                from c in dtTemp.DefaultIfEmpty()
                                                select new
                                                {
                                                    COD_CONTA = a == null ? c.Field<string>("COD_CONTA") : a.Field<string>("COD_CONTA"),
                                                    DESCRICAO = a == null ? c.Field<string>("DESCRICAO") : a.Field<string>("DESCRICAO"),
                                                    saldo_ini = c == null ? 0 : c.Field<Decimal>("saldo_ini"),
                                                    debito = c == null ? 0 : c.Field<Decimal>("debito"),
                                                    credito = c == null ? 0 : c.Field<Decimal>("credito"),
                                                    saldo_fim = c == null ? 0 : c.Field<Decimal>("saldo_fim"),
                                                    saldo_ini_moeda = a != null ? a.Field<Decimal>("saldo_ini") : 0,
                                                    debito_moeda = a != null ? a.Field<Decimal>("debito") : 0,
                                                    credito_moeda = a != null ? a.Field<Decimal>("credito") : 0,
                                                    saldo_fim_moeda = a != null ? (a.Field<Decimal>("saldo_ini") + a.Field<Decimal>("debito") - a.Field<Decimal>("credito")) : 0,
                                                    codDetalhe1 = detalhamento1 == null ? 0 : a == null ? c.Field<int>("codDetalhe1") : a.Field<int>("codDetalhe1"),
                                                    codDetalhe2 = detalhamento2 == null ? 0 : a == null ? c.Field<int>("codDetalhe2") : a.Field<int>("codDetalhe2"),
                                                    codDetalhe3 = detalhamento3 == null ? 0 : a == null ? c.Field<int>("codDetalhe3") : a.Field<int>("codDetalhe3"),
                                                    codDetalhe4 = detalhamento4 == null ? 0 : a == null ? c.Field<int>("codDetalhe4") : a.Field<int>("codDetalhe4"),
                                                    codDetalhe5 = detalhamento5 == null ? 0 : a == null ? c.Field<int>("codDetalhe5") : a.Field<int>("codDetalhe5"),
                                                    descDetalhe1 = detalhamento1 == null ? "" : a == null ?  c.Field<string>("descDetalhe1") : a.Field<string>("descDetalhe1"),
                                                    descDetalhe2 = detalhamento2 == null ? "" : a == null ?  c.Field<string>("descDetalhe2") : a.Field<string>("descDetalhe2"),
                                                    descDetalhe3 = detalhamento3 == null ? "" : a == null ?  c.Field<string>("descDetalhe3") : a.Field<string>("descDetalhe3"),
                                                    descDetalhe4 = detalhamento4 == null ? "" : a == null ?  c.Field<string>("descDetalhe4") : a.Field<string>("descDetalhe4"),
                                                    descDetalhe5 = detalhamento5 == null ? "" : a == null ?  c.Field<string>("descDetalhe5") : a.Field<string>("descDetalhe5"),
                                                    analitica = a.Field<int>("analitica")
                                                };

                            tb = novoDtLeft.Union(novoDtRight).OrderBy(r => r.COD_CONTA).CopyToDataTable();
                        }

                        if (erros.Count == 0)
                        {
							//Page Header
                            Title += " - Balancete Contábil";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Balancete Contábil";
                            filtros = "";
                            empresasDAO empr = new empresasDAO(_conn);
                            Master.literalNomeEmpresa.Text = empr.retornanomesempresas(empresas);
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodo.Text + "</p>";

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                    filtros += "&divisaoDe=" + divisaoDe + "&divisaoAte=" + divisaoAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                    filtros += "&linhaNegocioDe=" + linhaNegocioDe + "&linhaNegocioAte=" + linhaNegocioAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                    filtros += "&clienteDe=" + clienteDe + "&clienteAte=" + clienteAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                    filtros += "&jobDe=" + jobDe + "&jobAte=" + jobAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

							//Table
                            string html = "<table cellpadding=\"0\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td style=\"width:50px;\">Código</td>\n";
                            html += "<td style=\"width:160px;\">Conta</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Anterior</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Débito</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Crédito</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Atual</td>\n";
                            if(checkDuasMoedas.Checked)
                            {
                                html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Anterior<br />Moeda</td>\n";
                                html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Débito<br />Moeda</td>\n";
                                html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Crédito<br />Moeda</td>\n";
                                html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Atual<br />Moeda</td>\n";
                            }
                            html += "</tr>\n";
							
						
                            string codContaAnt = "";
                            int codDetalhe1Ant = 0;
                            int codDetalhe2Ant = 0;
                            int codDetalhe3Ant = 0;
                            int codDetalhe4Ant = 0;

                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                DataRow row = tb.Rows[i];
                                if (row["cod_conta"].ToString().Equals(codContaAnt))
                                {
                                    if (row["codDetalhe1"].ToString() != "0")
                                    {
                                        if (Convert.ToInt32(row["codDetalhe1"]) == codDetalhe1Ant)
                                        {
                                            if (row["codDetalhe2"].ToString() != "0")
                                            {
                                                if (Convert.ToInt32(row["codDetalhe2"]) == codDetalhe2Ant)
                                                {
                                                    if (row["codDetalhe3"].ToString() != "0")
                                                    {
                                                        if (Convert.ToInt32(row["codDetalhe3"]) == codDetalhe3Ant)
                                                        {
                                                            if (row["codDetalhe4"].ToString() != "0")
                                                            {
                                                                if (Convert.ToInt32(row["codDetalhe4"]) == codDetalhe4Ant)
                                                                {
                                                                    if (row["codDetalhe5"].ToString() != "0")
                                                                    {
                                                                        html += "<tr class=\"linha nivel5\">\n";
                                                                        html += "<td></td>\n";
                                                                        html += "<td class=\"descricao\">" + tb.Rows[i]["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                                        if (Convert.ToInt32(row["analitica"]) == 1)
                                                                        {
                                                                            string parametros = "";
                                                                            
                                                                            if (row["codDetalhe1"] != null)
                                                                            {
                                                                                parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                if (row["codDetalhe2"] != null)
                                                                                {
                                                                                    parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                    if (row["codDetalhe3"] != null)
                                                                                    {
                                                                                        parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                        if (row["codDetalhe4"] != null)
                                                                                        {
                                                                                            parametros += "&detalhe4=" + row["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                            if (row["codDetalhe5"] != null)
                                                                                            {
                                                                                                parametros += "&detalhe5=" + row["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString()))-decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                                                        }
                                                                        else
                                                                        {
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                                        }
                                                                        if (checkDuasMoedas.Checked)
                                                                        {
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                            if (Convert.ToInt32(row["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "";

                                                                                if (row["codDetalhe1"] != null)
                                                                                {
                                                                                    parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                    if (row["codDetalhe2"] != null)
                                                                                    {
                                                                                        parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                        if (row["codDetalhe3"] != null)
                                                                                        {
                                                                                            parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                            if (row["codDetalhe4"] != null)
                                                                                            {
                                                                                                parametros += "&detalhe4=" + row["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                                if (row["codDetalhe5"] != null)
                                                                                                {
                                                                                                    parametros += "&detalhe5=" + row["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }

                                                                                html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                                                            }
                                                                            else
                                                                            {
                                                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                            }
                                                                        }

                                                                        html += "</tr>\n";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    html += "<tr class=\"linha nivel4\">\n";
                                                                    html += "<td></td>\n";
                                                                    html += "<td class=\"descricao\">" + tb.Rows[i]["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                                    if (Convert.ToInt32(row["analitica"]) == 1)
                                                                    {
                                                                        string parametros = "";
                                                                        if (row["codDetalhe1"] != null)
                                                                        {
                                                                            parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                            if (row["codDetalhe2"] != null)
                                                                            {
                                                                                parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                if (row["codDetalhe3"] != null)
                                                                                {
                                                                                    parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                    if (row["codDetalhe4"] != null)
                                                                                    {
                                                                                        parametros += "&detalhe4=" + row["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }

                                                                        html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                                                    }
                                                                    else
                                                                    {
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                                    }
                                                                    if (checkDuasMoedas.Checked)
                                                                    {
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                        if (Convert.ToInt32(row["analitica"]) == 1)
                                                                        {
                                                                            string parametros = "";
                                                                            if (row["codDetalhe1"] != null)
                                                                            {
                                                                                parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                if (row["codDetalhe2"] != null)
                                                                                {
                                                                                    parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                    if (row["codDetalhe3"] != null)
                                                                                    {
                                                                                        parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                        if (row["codDetalhe4"] != null)
                                                                                        {
                                                                                            parametros += "&detalhe4=" + row["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                                                        }
                                                                        else
                                                                        {
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                        }
                                                                    }
                                                                    html += "</tr>\n";
                                                                }

                                                                codDetalhe4Ant = Convert.ToInt32(row["codDetalhe4"]);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            html += "<tr class=\"linha nivel3\">\n";
                                                            html += "<td></td>\n";
                                                            html += "<td class=\"descricao\">" + tb.Rows[i]["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                            if (Convert.ToInt32(row["analitica"]) == 1)
                                                            {
                                                                string parametros = "";
                                                                if (row["codDetalhe1"] != null)
                                                                {
                                                                    parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                    if (row["codDetalhe2"] != null)
                                                                    {
                                                                        parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                        if (row["codDetalhe3"] != null)
                                                                        {
                                                                            parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                        }
                                                                    }
                                                                }

                                                                html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                                            }
                                                            else
                                                            {
                                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                            }
                                                            if (checkDuasMoedas.Checked)
                                                            {
                                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                if (Convert.ToInt32(row["analitica"]) == 1)
                                                                {
                                                                    string parametros = "";
                                                                    if (row["codDetalhe1"] != null)
                                                                    {
                                                                        parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                        if (row["codDetalhe2"] != null)
                                                                        {
                                                                            parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                            if (row["codDetalhe3"] != null)
                                                                            {
                                                                                parametros += "&detalhe3=" + row["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                            }
                                                                        }
                                                                    }

                                                                    html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                                                }
                                                                else
                                                                {
                                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                                }
                                                            }
                                                            html += "</tr>\n";

                                                            codDetalhe4Ant = 0;
                                                        }
                                                        codDetalhe3Ant = Convert.ToInt32(row["codDetalhe3"]);
                                                    }

                                                }
                                                else
                                                {
                                                    html += "<tr class=\"linha nivel2\">\n";
                                                    html += "<td></td>\n";
                                                    html += "<td class=\"descricao\">" + tb.Rows[i]["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                    if (Convert.ToInt32(row["analitica"]) == 1)
                                                    {
                                                        string parametros = "";
                                                        if (row["codDetalhe1"] != null)
                                                        {
                                                            parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                            if (row["codDetalhe2"] != null)
                                                            {
                                                                parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                            }
                                                        }

                                                        html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                                    }
                                                    else
                                                    {
                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                                    }

                                                    if (checkDuasMoedas.Checked)
                                                    {
                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                        if (Convert.ToInt32(row["analitica"]) == 1)
                                                        {
                                                            string parametros = "";
                                                            if (row["codDetalhe1"] != null)
                                                            {
                                                                parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                if (row["codDetalhe2"] != null)
                                                                {
                                                                    parametros += "&detalhe2=" + row["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                }
                                                            }

                                                            html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                                        }
                                                        else
                                                        {
                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                        }
                                                    }
                                                    html += "</tr>\n";

                                                    codDetalhe3Ant = 0;
                                                    codDetalhe4Ant = 0;
                                                }
                                                codDetalhe2Ant = Convert.ToInt32(row["codDetalhe2"]);
                                            }
                                        }
                                        else
                                        {
                                            html += "<tr class=\"linha nivel1\">\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"descricao\">" + tb.Rows[i]["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                            if (Convert.ToInt32(row["analitica"]) == 1)
                                            {
                                                string parametros = "";
                                                if (row["codDetalhe1"] != null)
                                                {
                                                    parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                }

                                                html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                            }
                                            else
                                            {
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                            }
                                            if (checkDuasMoedas.Checked)
                                            {
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                if (Convert.ToInt32(row["analitica"]) == 1)
                                                {
                                                    string parametros = "";
                                                    if (row["codDetalhe1"] != null)
                                                    {
                                                        parametros += "&detalhe1=" + row["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                    }

                                                    html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                                }
                                                else
                                                {
                                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                                }
                                            }
                                            html += "</tr>\n";

                                            codDetalhe2Ant = 0;
                                            codDetalhe3Ant = 0;
                                            codDetalhe4Ant = 0;
                                        }

                                        codDetalhe1Ant = Convert.ToInt32(row["codDetalhe1"]);
                                    }
                                }
                                else
                                {
                                    html += "<tr class=\"linha nivel0\">\n";
                                    html += "<td>" + tb.Rows[i]["cod_conta"] + "</td>\n";
                                    html += "<td class=\"descricao\">" + tb.Rows[i]["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini"].ToString())) + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito"].ToString())) + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                    if (Convert.ToInt32(row["analitica"]) == 1)
                                    {
                                        html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                    }
                                    else
                                    {
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
                                    }
                                    if (checkDuasMoedas.Checked)
                                    {
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString())) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                        if (Convert.ToInt32(row["analitica"]) == 1)
                                        {
                                            html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + "&moeda=" + tipoMoeda + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</a></td>\n";
                                        }
                                        else
                                        {
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini_moeda"].ToString()) + decimal.Parse(tb.Rows[i]["debito_moeda"].ToString())) - decimal.Parse(tb.Rows[i]["credito_moeda"].ToString())) + "</td>\n";
                                        }
                                    }         
                                    html += "</tr>\n";

                                    codDetalhe1Ant = 0;
                                    codDetalhe2Ant = 0;
                                    codDetalhe3Ant = 0;
                                    codDetalhe4Ant = 0;
                                }

                                codContaAnt = tb.Rows[i]["cod_conta"].ToString();
                            }

                            html += "</table>\n";

                            literalRelatorio.Text = html;
                        }
                        else
                        {
                            errosFormulario(erros);
                        }
                        break;
                    #endregion
                    #region Relatório DRE
                    case "DRE":

                        TextBox textPeriodoDe = PreviousPage.txtPeriodoDe;
                        TextBox textPeriodoAte = PreviousPage.txtPeriodoAte;
                        checkContasEncerramentoDRE = PreviousPage.checkContasEncerrDRE;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;
                        comboMoeda = PreviousPage.cmbMoeda;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;
                        comboDetalhamento5 = PreviousPage.cmbDetalhamento5;

                        empresas = PreviousPage.empresas_selecionadas2;

                        tipoMoeda = null;
                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;
                        detalhamento5 = null;

                        if (comboMoeda != null)
                        {
                            if (comboMoeda.SelectedValue == "0")
                                tipoMoeda = null;
                            else
                                tipoMoeda = Convert.ToInt32(comboMoeda.SelectedValue);
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

                        if (comboDetalhamento1 != null)
                        {
                            if (comboDetalhamento1.SelectedValue == "0")
                                detalhamento1 = null;
                            else
                            {
                                detalhamento1 = comboDetalhamento1.SelectedValue;

                                if (comboDetalhamento2 != null)
                                {
                                    if (comboDetalhamento2.SelectedValue == "0")
                                        detalhamento2 = null;
                                    else
                                    {
                                        detalhamento2 = comboDetalhamento2.SelectedValue;

                                        if (comboDetalhamento3 != null)
                                        {
                                            if (comboDetalhamento3.SelectedValue == "0")
                                                detalhamento3 = null;
                                            else
                                            {
                                                detalhamento3 = comboDetalhamento3.SelectedValue;

                                                if (comboDetalhamento4 != null)
                                                {
                                                    if (comboDetalhamento4.SelectedValue == "0")
                                                        detalhamento4 = null;
                                                    else
                                                    {
                                                        detalhamento4 = comboDetalhamento4.SelectedValue;

                                                        if (comboDetalhamento5 != null)
                                                        {
                                                            if (comboDetalhamento5.SelectedValue == "0")
                                                                detalhamento5 = null;
                                                            else
                                                            {
                                                                detalhamento5 = comboDetalhamento5.SelectedValue;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        
                        erros = relatorio.dre(tipoMoeda, Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text),
                            divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                            jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb, checkContasEncerramentoDRE.Checked, empresas);
                        
                            
                        if (erros.Count == 0)
                        {
                            Title += " - DRE";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "DRE";
                            empresasDAO empr = new empresasDAO(_conn);
                            Master.literalNomeEmpresa.Text = empr.retornanomesempresas(empresas);
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                    filtros += "&divisaoDe=" + divisaoDe + "&divisaoAte=" + divisaoAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                    filtros += "&linhaNegocioDe=" + linhaNegocioDe + "&linhaNegocioAte=" + linhaNegocioAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                    filtros += "&clienteDe=" + clienteDe + "&clienteAte=" + clienteAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                {
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                    filtros += "&jobDe=" + jobDe + "&jobAte=" + jobAte;
                                }
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            DateTime periodoDe = Convert.ToDateTime(textPeriodoDe.Text);
                            DateTime periodoAte = Convert.ToDateTime(textPeriodoAte.Text);


                            List<DateTime> periodos = new List<DateTime>();

                            while (periodoDe <= periodoAte)
                            {
                                periodos.Add(periodoDe);

                                periodoDe = periodoDe.AddMonths(1);
                            }

                            DataRow rowAnt = null;
                            List<SValoresDRE> vlrs = new List<SValoresDRE>();
                            int nivel = 0;
                            string codContaAnt = "";
                            int codDetalhe1Ant = 0;
                            int codDetalhe2Ant = 0;
                            int codDetalhe3Ant = 0;
                            int codDetalhe4Ant = 0;
                            int codDetalhe5Ant = 0;

                            StringBuilder htmlx = new StringBuilder();

                            htmlx.Append("<table id=\"dre\" cellpadding=\"0\" cellspacing=\"0\">\n");
                            htmlx.Append("<tr class=\"titulo\">\n");
                            htmlx.Append("<td>Código</td>\n");
                            htmlx.Append( "<td>Conta</td>\n");
                            htmlx.Append( "<td class=\"valores\">Acumulado</td>");
                            for (int i = 0; i < periodos.Count; i++)
                            {
                                htmlx.Append("<td class=\"valores\">" + periodos[i].ToString("MM/yyyy") + "</td>\n");
                            }
                            htmlx.Append("</tr>\n");


                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                DataRow row = tb.Rows[i];


                                if (row["cod_conta"].ToString().Equals(codContaAnt))
                                {
                                    if (tb.Columns.Contains("codDetalhe1"))
                                    {
                                        if (row["codDetalhe1"].ToString() != "0")
                                        {
                                            if (Convert.ToInt32(row["codDetalhe1"]) == codDetalhe1Ant)
                                            {
                                                if (tb.Columns.Contains("codDetalhe2"))
                                                {
                                                    if (row["codDetalhe2"].ToString() != "0")
                                                    {
                                                        if (Convert.ToInt32(row["codDetalhe2"]) == codDetalhe2Ant)
                                                        {
                                                            if (tb.Columns.Contains("codDetalhe3"))
                                                            {
                                                                if (row["codDetalhe3"].ToString() != "0")
                                                                {
                                                                    if (Convert.ToInt32(row["codDetalhe3"]) == codDetalhe3Ant)
                                                                    {
                                                                        if (tb.Columns.Contains("codDetalhe4"))
                                                                        {
                                                                            if (row["codDetalhe4"].ToString() != "0")
                                                                            {
                                                                                if (Convert.ToInt32(row["codDetalhe4"]) == codDetalhe4Ant)
                                                                                {
                                                                                    if (tb.Columns.Contains("codDetalhe5"))
                                                                                    {
                                                                                        if (row["codDetalhe5"].ToString() != "0")
                                                                                        {
                                                                                            if (Convert.ToInt32(row["codDetalhe5"]) == codDetalhe5Ant)
                                                                                            {
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                    {
                                                                                                        SValoresDRE temp = vlrs[x];
                                                                                                        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                                        vlrs[x] = temp;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (rowAnt != null)
                                                                                                {
                                                                                                    decimal acumulado = 0;
                                                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                                                    {
                                                                                                        acumulado += vlrs[x].valor;
                                                                                                    }

                                                                                                    if (nivel != 5)
                                                                                                    {
                                                                                                        if (nivel == 4)
                                                                                                        {

                                                                                                            htmlx.Append("<tr class=\"linha nivel4\">\n");
                                                                                                            htmlx.Append("<td></td>\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                                            {
                                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                                {
                                                                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                                }
                                                                                                            }

                                                                                                            htmlx.Append("</tr>\n");
                                                                                                        }
                                                                                                        else if (nivel == 3)
                                                                                                        {
                                                                                                            htmlx.Append("<tr class=\"linha nivel3\">\n");
                                                                                                            htmlx.Append("<td></td>\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                                            {

                                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                                {
                                                                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                                }
                                                                                                            }

                                                                                                            htmlx.Append("</tr>\n");
                                                                                                        }
                                                                                                        else if (nivel == 2)
                                                                                                        {
                                                                                                            htmlx.Append("<tr class=\"linha nivel2\">\n");
                                                                                                            htmlx.Append("<td></td>\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                                            {
                                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                                {
                                                                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                                }
                                                                                                            }

                                                                                                            htmlx.Append("</tr>\n");
                                                                                                        }
                                                                                                        else if (nivel == 1)
                                                                                                        {
                                                                                                            htmlx.Append("<tr class=\"linha nivel1\">\n");
                                                                                                            htmlx.Append("<td></td>\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                                            {
                                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                                {
                                                                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                                }
                                                                                                            }

                                                                                                            htmlx.Append("</tr>\n");
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            htmlx.Append("<tr class=\"linha nivel0\">\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                                                                                            htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                                            {
                                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                                {
                                                                                                                    string parametros = "";
                                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                                }
                                                                                                            }

                                                                                                            htmlx.Append("</tr>\n");
                                                                                                        }

                                                                                                        vlrs = new List<SValoresDRE>();
                                                                                                        for (int x = 0; x < periodos.Count; x++)
                                                                                                        {
                                                                                                            if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                                vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                                            else
                                                                                                                vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<tr class=\"linha nivel5\">\n");
                                                                                                        htmlx.Append("<td></td>\n");
                                                                                                        htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                                                        {
                                                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                            {
                                                                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                            }
                                                                                                        }
                                                                                                        htmlx.Append("</tr>\n");

                                                                                                        //for (int x = 0; x < vlrs.Count; x++)
                                                                                                        //{
                                                                                                        //    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                        //    {
                                                                                                        //        SValoresDRE temp = vlrs[x];
                                                                                                        //        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                                        //        vlrs[x] = temp;
                                                                                                        //    }
                                                                                                        //}

                                                                                                        vlrs = new List<SValoresDRE>();
                                                                                                        for (int x = 0; x < periodos.Count; x++)
                                                                                                        {
                                                                                                            if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                                vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                                            else
                                                                                                                vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                                        }
                                                                                                    }

                                                                                                    rowAnt = null;
                                                                                                }

                                                                                                nivel = 5;
                                                                                            }

                                                                                            codDetalhe5Ant = Convert.ToInt32(row["codDetalhe5"]);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                            {
                                                                                                if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                {
                                                                                                    SValoresDRE temp = vlrs[x];
                                                                                                    temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                                    vlrs[x] = temp;
                                                                                                }
                                                                                            }

                                                                                            codDetalhe4Ant = Convert.ToInt32(row["codDetalhe4"]);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                                        {
                                                                                            if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                            {
                                                                                                SValoresDRE temp = vlrs[x];
                                                                                                temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                                vlrs[x] = temp;
                                                                                            }
                                                                                        }

                                                                                        codDetalhe4Ant = Convert.ToInt32(row["codDetalhe4"]);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (rowAnt != null)
                                                                                    {
                                                                                        decimal acumulado = 0;
                                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                                        {
                                                                                            acumulado += vlrs[x].valor;
                                                                                        }

                                                                                        if (nivel != 4)
                                                                                        {
                                                                                            if (nivel == 5)
                                                                                            {
                                                                                                htmlx.Append("<tr class=\"linha nivel5\">\n");
                                                                                                htmlx.Append("<td></td>\n");
                                                                                                htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                    {
                                                                                                        string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                    }
                                                                                                }

                                                                                                htmlx.Append("</tr>\n");
                                                                                            }
                                                                                            else if (nivel == 3)
                                                                                            {
                                                                                                htmlx.Append("<tr class=\"linha nivel3\">\n");
                                                                                                htmlx.Append("<td></td>\n");
                                                                                                htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                    {
                                                                                                        string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                    }
                                                                                                }

                                                                                                htmlx.Append("</tr>\n");
                                                                                            }
                                                                                            else if (nivel == 2)
                                                                                            {
                                                                                                htmlx.Append("<tr class=\"linha nivel2\">\n");
                                                                                                htmlx.Append("<td></td>\n");
                                                                                                htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                    {
                                                                                                        string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                    }
                                                                                                }

                                                                                                htmlx.Append("</tr>\n");
                                                                                            }
                                                                                            else if (nivel == 1)
                                                                                            {
                                                                                                htmlx.Append("<tr class=\"linha nivel1\">\n");
                                                                                                htmlx.Append("<td></td>\n");
                                                                                                htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                    {
                                                                                                        string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                    }
                                                                                                }

                                                                                                htmlx.Append("</tr>\n");
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                htmlx.Append("<tr class=\"linha nivel0\">\n");
                                                                                                htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                                                                                htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                                {
                                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                    {
                                                                                                        string parametros = "";
                                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                    }
                                                                                                }

                                                                                                htmlx.Append("</tr>\n");
                                                                                            }

                                                                                            vlrs = new List<SValoresDRE>();
                                                                                            for (int x = 0; x < periodos.Count; x++)
                                                                                            {
                                                                                                if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                    vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                                else
                                                                                                    vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            htmlx.Append("<tr class=\"linha nivel4\">\n");
                                                                                            htmlx.Append("<td></td>\n");
                                                                                            htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                                            {
                                                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                                {
                                                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                                }
                                                                                            }
                                                                                            htmlx.Append("</tr>\n");

                                                                                            //for (int x = 0; x < vlrs.Count; x++)
                                                                                            //{
                                                                                            //    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                            //    {
                                                                                            //        SValoresDRE temp = vlrs[x];
                                                                                            //        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                            //        vlrs[x] = temp;
                                                                                            //    }
                                                                                            //}

                                                                                            vlrs = new List<SValoresDRE>();
                                                                                            for (int x = 0; x < periodos.Count; x++)
                                                                                            {
                                                                                                if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                                    vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                                else
                                                                                                    vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                            }
                                                                                        }

                                                                                        rowAnt = null;
                                                                                    }

                                                                                    nivel = 4;

                                                                                }

                                                                                codDetalhe4Ant = Convert.ToInt32(row["codDetalhe4"]);
                                                                            }
                                                                            else
                                                                            {
                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                {
                                                                                    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                    {
                                                                                        SValoresDRE temp = vlrs[x];
                                                                                        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                        vlrs[x] = temp;
                                                                                    }
                                                                                }

                                                                                codDetalhe3Ant = Convert.ToInt32(row["codDetalhe3"]);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                            {
                                                                                if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                {
                                                                                    SValoresDRE temp = vlrs[x];
                                                                                    temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                    vlrs[x] = temp;
                                                                                }
                                                                            }

                                                                            codDetalhe3Ant = Convert.ToInt32(row["codDetalhe3"]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                        if (rowAnt != null)
                                                                        {
                                                                            decimal acumulado = 0;
                                                                            for (int x = 0; x < vlrs.Count; x++)
                                                                            {
                                                                                acumulado += vlrs[x].valor;
                                                                            }

                                                                            if (nivel != 3)
                                                                            {
                                                                                if (nivel == 5)
                                                                                {
                                                                                    htmlx.Append("<tr class=\"linha nivel5\">\n");
                                                                                    htmlx.Append("<td></td>\n");
                                                                                    htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                                    {
                                                                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                        {
                                                                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                        }
                                                                                    }

                                                                                    htmlx.Append("</tr>\n");
                                                                                }
                                                                                else if (nivel == 4)
                                                                                {
                                                                                    htmlx.Append("<tr class=\"linha nivel4\">\n");
                                                                                    htmlx.Append("<td></td>\n");
                                                                                    htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                                    {
                                                                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                        {
                                                                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                        }
                                                                                    }

                                                                                    htmlx.Append("</tr>\n");
                                                                                }
                                                                                if (nivel == 2)
                                                                                {
                                                                                    htmlx.Append("<tr class=\"linha nivel2\">\n");
                                                                                    htmlx.Append("<td></td>\n");
                                                                                    htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                                    {
                                                                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                        {
                                                                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                        }
                                                                                    }

                                                                                    htmlx.Append("</tr>\n");
                                                                                }
                                                                                else if (nivel == 1)
                                                                                {
                                                                                    htmlx.Append("<tr class=\"linha nivel1\">\n");
                                                                                    htmlx.Append("<td></td>\n");
                                                                                    htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                                    {
                                                                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                        {
                                                                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                        }
                                                                                    }

                                                                                    htmlx.Append("</tr>\n");
                                                                                }
                                                                                //else
                                                                                //{ // Diferente nivel 5
                                                                                //    htmlx.Append("<tr class=\"linha nivel0\">\n");
                                                                                //    htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                                                                //    htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                //    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                //    for (int x = 0; x < vlrs.Count; x++)
                                                                                //    {
                                                                                //        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                //        {
                                                                                //            string parametros = "";
                                                                                //            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                //        }
                                                                                //        else
                                                                                //        {
                                                                                //            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                //        }
                                                                                //    }

                                                                                //    htmlx.Append("</tr>\n");
                                                                                //}

                                                                                vlrs = new List<SValoresDRE>();
                                                                                for (int x = 0; x < periodos.Count; x++)
                                                                                {
                                                                                    if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                        vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                    else
                                                                                        vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                }
                                                                            }  // Nivel 3 else
                                                                            else
                                                                            {
                                                                                htmlx.Append("<tr class=\"linha nivel3\">\n");
                                                                                htmlx.Append("<td></td>\n");
                                                                                htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                                {
                                                                                    if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                                    {
                                                                                        string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                        htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                                    }
                                                                                }
                                                                                htmlx.Append("</tr>\n");

                                                                                //for (int x = 0; x < vlrs.Count; x++)
                                                                                //{
                                                                                //    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                //    {
                                                                                //        SValoresDRE temp = vlrs[x];
                                                                                //        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                                //        vlrs[x] = temp;
                                                                                //    }
                                                                                //}

                                                                                vlrs = new List<SValoresDRE>();
                                                                                for (int x = 0; x < periodos.Count; x++)
                                                                                {
                                                                                    if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                                        vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                                    else
                                                                                        vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                                }
                                                                            }


                                                                            rowAnt = null;
                                                                        }

                                                                        nivel = 3;
                                                                    }

                                                                    codDetalhe3Ant = Convert.ToInt32(row["codDetalhe3"]);
                                                                }
                                                                else
                                                                {
                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                    {
                                                                        if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                        {
                                                                            SValoresDRE temp = vlrs[x];
                                                                            temp.valor = Convert.ToDecimal(row["valor"]);
                                                                            vlrs[x] = temp;
                                                                        }
                                                                    }

                                                                    codDetalhe2Ant = Convert.ToInt32(row["codDetalhe2"]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                {
                                                                    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                    {
                                                                        SValoresDRE temp = vlrs[x];
                                                                        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                        vlrs[x] = temp;
                                                                    }
                                                                }

                                                                codDetalhe2Ant = Convert.ToInt32(row["codDetalhe2"]);
                                                            }
                                                        }
                                                        else
                                                        {

                                                            if (rowAnt != null)
                                                            {
                                                                decimal acumulado = 0;
                                                                for (int x = 0; x < vlrs.Count; x++)
                                                                {
                                                                    acumulado += vlrs[x].valor;
                                                                }

                                                                if (nivel != 2)
                                                                {
                                                                    if (nivel == 5)
                                                                    {
                                                                        htmlx.Append("<tr class=\"linha nivel5\">\n");
                                                                        htmlx.Append("<td></td>\n");
                                                                        htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                        {
                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                            }
                                                                            else
                                                                            {
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                            }
                                                                        }

                                                                        htmlx.Append("</tr>\n");
                                                                    }
                                                                    else if (nivel == 4)
                                                                    {
                                                                        htmlx.Append("<tr class=\"linha nivel4\">\n");
                                                                        htmlx.Append("<td></td>\n");
                                                                        htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                        {
                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                            }
                                                                            else
                                                                            {
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                            }
                                                                        }

                                                                        htmlx.Append("</tr>\n");
                                                                    }
                                                                    else if (nivel == 3)
                                                                    {
                                                                        htmlx.Append("<tr class=\"linha nivel3\">\n");
                                                                        htmlx.Append("<td></td>\n");
                                                                        htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                        {
                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                            }
                                                                            else
                                                                            {
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                            }
                                                                        }

                                                                        htmlx.Append("</tr>\n");
                                                                    }
                                                                    else if (nivel == 1)
                                                                    {
                                                                        htmlx.Append("<tr class=\"linha nivel1\">\n");
                                                                        htmlx.Append("<td></td>\n");
                                                                        htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                        {
                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                            }
                                                                            else
                                                                            {
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                            }
                                                                        }

                                                                        htmlx.Append("</tr>\n");
                                                                    }
                                                                    else
                                                                    {
                                                                        htmlx.Append("<tr class=\"linha nivel0\">\n");
                                                                        htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                                                        htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                        for (int x = 0; x < vlrs.Count; x++)
                                                                        {
                                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                            {
                                                                                string parametros = "";
                                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                            }
                                                                            else
                                                                            {
                                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                            }
                                                                        }

                                                                        htmlx.Append("</tr>\n");
                                                                    }

                                                                    vlrs = new List<SValoresDRE>();
                                                                    for (int x = 0; x < periodos.Count; x++)
                                                                    {
                                                                        if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                            vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                        else
                                                                            vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<tr class=\"linha nivel2\">\n");
                                                                    htmlx.Append("<td></td>\n");
                                                                    htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                                    for (int x = 0; x < vlrs.Count; x++)
                                                                    {
                                                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                        {
                                                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                        }
                                                                        else
                                                                        {
                                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                        }
                                                                    }
                                                                    htmlx.Append("</tr>\n");

                                                                    //for (int x = 0; x < vlrs.Count; x++)
                                                                    //{
                                                                    //    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                    //    {
                                                                    //        SValoresDRE temp = vlrs[x];
                                                                    //        temp.valor = Convert.ToDecimal(row["valor"]);
                                                                    //        vlrs[x] = temp;
                                                                    //    }
                                                                    //}

                                                                    vlrs = new List<SValoresDRE>();
                                                                    for (int x = 0; x < periodos.Count; x++)
                                                                    {
                                                                        if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                            vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                                        else
                                                                            vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                                    }
                                                                }

                                                                rowAnt = null;
                                                            }

                                                            nivel = 2;
                                                        }

                                                        codDetalhe2Ant = Convert.ToInt32(row["codDetalhe2"]);
                                                    }
                                                    else
                                                    {

                                                        for (int x = 0; x < vlrs.Count; x++)
                                                        {
                                                            if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                            {
                                                                SValoresDRE temp = vlrs[x];
                                                                temp.valor = Convert.ToDecimal(row["valor"]);
                                                                vlrs[x] = temp;
                                                            }
                                                        }

                                                        codDetalhe1Ant = Convert.ToInt32(row["codDetalhe1"]);
                                                    }
                                                }
                                                else
                                                {
                                                    for (int x = 0; x < vlrs.Count; x++)
                                                    {
                                                        if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                        {
                                                            SValoresDRE temp = vlrs[x];
                                                            temp.valor = Convert.ToDecimal(row["valor"]);
                                                            vlrs[x] = temp;
                                                        }
                                                    }

                                                    codDetalhe1Ant = Convert.ToInt32(row["codDetalhe1"]);
                                                }
                                            }
                                            else
                                            {

                                                if (rowAnt != null)
                                                {
                                                    decimal acumulado = 0;
                                                    for (int x = 0; x < vlrs.Count; x++)
                                                    {
                                                        acumulado += vlrs[x].valor;
                                                    }

                                                    if (nivel != 1)
                                                    {
                                                        if (nivel == 5)
                                                        {
                                                            htmlx.Append("<tr class=\"linha nivel5\">\n");
                                                            htmlx.Append("<td></td>\n");
                                                            htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                            for (int x = 0; x < vlrs.Count; x++)
                                                            {
                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                {
                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                }
                                                            }

                                                            htmlx.Append("</tr>\n");
                                                        }
                                                        else if (nivel == 4)
                                                        {
                                                            htmlx.Append("<tr class=\"linha nivel4\">\n");
                                                            htmlx.Append("<td></td>\n");
                                                            htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                            for (int x = 0; x < vlrs.Count; x++)
                                                            {
                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                {
                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                }
                                                            }

                                                            htmlx.Append("</tr>\n");
                                                        }
                                                        else if (nivel == 3)
                                                        {
                                                            htmlx.Append("<tr class=\"linha nivel3\">\n");
                                                            htmlx.Append("<td></td>\n");
                                                            htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                            for (int x = 0; x < vlrs.Count; x++)
                                                            {
                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                {
                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                }
                                                            }

                                                            htmlx.Append("</tr>\n");
                                                        }
                                                        else if (nivel == 2)
                                                        {
                                                            htmlx.Append("<tr class=\"linha nivel2\">\n");
                                                            htmlx.Append("<td></td>\n");
                                                            htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                            for (int x = 0; x < vlrs.Count; x++)
                                                            {
                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                {
                                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                }
                                                            }

                                                            htmlx.Append("</tr>\n");
                                                        }
                                                        else
                                                        {
                                                            htmlx.Append("<tr class=\"linha nivel0\">\n");
                                                            htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                                            htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                            for (int x = 0; x < vlrs.Count; x++)
                                                            {
                                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                                {
                                                                    string parametros = "";
                                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                                }
                                                                else
                                                                {
                                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                                }
                                                            }

                                                            htmlx.Append("</tr>\n");
                                                        }

                                                        vlrs = new List<SValoresDRE>();
                                                        for (int x = 0; x < periodos.Count; x++)
                                                        {
                                                            if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                            else
                                                                vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        htmlx.Append("<tr class=\"linha nivel1\">\n");
                                                        htmlx.Append("<td></td>\n");
                                                        htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                                        htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                                        for (int x = 0; x < vlrs.Count; x++)
                                                        {
                                                            if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                            {
                                                                string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                                htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                            }
                                                            else
                                                            {
                                                                htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                            }
                                                        }
                                                        htmlx.Append("</tr>\n");

                                                        //vlrs = new List<SValoresDRE>();
                                                        //for (int x = 0; x < vlrs.Count; x++)
                                                        //{
                                                        //    if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                        //    {
                                                        //        SValoresDRE temp = vlrs[x];
                                                        //        temp.valor = Convert.ToDecimal(row["valor"]);
                                                        //        vlrs[x] = temp;
                                                        //    }
                                                        //}

                                                        vlrs = new List<SValoresDRE>();
                                                        for (int x = 0; x < periodos.Count; x++)
                                                        {
                                                            if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                                                vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                                            else
                                                                vlrs.Add(new SValoresDRE(periodos[x], 0));
                                                        }
                                                    }

                                                    rowAnt = null;
                                                }

                                                nivel = 1;
                                            }

                                            codDetalhe1Ant = Convert.ToInt32(row["codDetalhe1"]);
                                        }
                                        else
                                        {
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                                {
                                                    SValoresDRE temp = vlrs[x];
                                                    temp.valor = Convert.ToDecimal(row["valor"]);
                                                    vlrs[x] = temp;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int x = 0; x < vlrs.Count; x++)
                                        {
                                            if (vlrs[x].periodo.ToString("yyyyMM") == row["ano_mes"].ToString())
                                            {
                                                SValoresDRE temp = vlrs[x];
                                                temp.valor = Convert.ToDecimal(row["valor"]);
                                                vlrs[x] = temp;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (rowAnt != null)
                                    {
                                        decimal acumulado = 0;
                                        for (int x = 0; x < vlrs.Count; x++)
                                        {
                                            acumulado += vlrs[x].valor;
                                        }

                                        if (nivel == 5)
                                        {
                                            htmlx.Append("<tr class=\"linha nivel5\">\n");
                                            htmlx.Append("<td></td>\n");
                                            htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }
                                        else if (nivel == 4)
                                        {
                                            htmlx.Append("<tr class=\"linha nivel4\">\n");
                                            htmlx.Append("<td></td>\n");
                                            htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }
                                        else if (nivel == 3)
                                        {
                                            htmlx.Append("<tr class=\"linha nivel3\">\n");
                                            htmlx.Append("<td></td>\n");
                                            htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }
                                        else if (nivel == 2)
                                        {
                                            htmlx.Append("<tr class=\"linha nivel2\">\n");
                                            htmlx.Append("<td></td>\n");
                                            htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }
                                        else if (nivel == 1)
                                        {
                                            htmlx.Append("<tr class=\"linha nivel1\">\n");
                                            htmlx.Append("<td></td>\n");
                                            htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<tr class=\"linha nivel0\">\n");
                                            htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                            htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>");
                                            for (int x = 0; x < vlrs.Count; x++)
                                            {
                                                if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                                {
                                                    string parametros = "";
                                                    htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                                }
                                                else
                                                {
                                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                                }
                                            }

                                            htmlx.Append("</tr>\n");
                                        }

                                        codDetalhe1Ant = 0;
                                        codDetalhe2Ant = 0;
                                        codDetalhe3Ant = 0;
                                        codDetalhe4Ant = 0;
                                        codDetalhe5Ant = 0;
                                        nivel = 0;
                                        rowAnt = null;
                                    }

                                    vlrs = new List<SValoresDRE>();
                                    for (int x = 0; x < periodos.Count; x++)
                                    {
                                        if (periodos[x].ToString("yyyyMM") == row["ano_mes"].ToString())
                                            vlrs.Add(new SValoresDRE(periodos[x], Convert.ToDecimal(row["valor"])));
                                        else
                                            vlrs.Add(new SValoresDRE(periodos[x], 0));
                                    }

                                    codDetalhe1Ant = 0;
                                    codDetalhe2Ant = 0;
                                    codDetalhe3Ant = 0;
                                    codDetalhe4Ant = 0;
                                    codDetalhe5Ant = 0;
                                }

                                codContaAnt = tb.Rows[i]["cod_conta"].ToString();
                                rowAnt = row;
                            }

                            if (rowAnt != null)
                            {
                                decimal acumulado = 0;
                                for (int x = 0; x < vlrs.Count; x++)
                                {
                                    acumulado += vlrs[x].valor;
                                }

                                if (nivel == 5)
                                {
                                    htmlx.Append("<tr class=\"linha nivel5\">\n");
                                    htmlx.Append("<td></td>\n");
                                    htmlx.Append("<td>" + rowAnt["descDetalhe5"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4 + "&detalhe5=" + rowAnt["codDetalhe5"] + "&detalhe5Cod=" + detalhamento5;
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }
                                else if (nivel == 4)
                                {
                                    htmlx.Append("<tr class=\"linha nivel4\">\n");
                                    htmlx.Append("<td></td>\n");
                                    htmlx.Append("<td>" + rowAnt["descDetalhe4"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3 + "&detalhe4=" + rowAnt["codDetalhe4"] + "&detalhe4Cod=" + detalhamento4;
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }
                                else if (nivel == 3)
                                {
                                    htmlx.Append("<tr class=\"linha nivel3\">\n");
                                    htmlx.Append("<td></td>\n");
                                    htmlx.Append("<td>" + rowAnt["descDetalhe3"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2 + "&detalhe3=" + rowAnt["codDetalhe3"] + "&detalhe3Cod=" + detalhamento3;
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }
                                else if (nivel == 2)
                                {
                                    htmlx.Append("<tr class=\"linha nivel2\">\n");
                                    htmlx.Append("<td></td>\n");
                                    htmlx.Append("<td>" + rowAnt["descDetalhe2"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1 + "&detalhe2=" + rowAnt["codDetalhe2"] + "&detalhe2Cod=" + detalhamento2;
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }
                                else if (nivel == 1)
                                {
                                    htmlx.Append("<tr class=\"linha nivel1\">\n");
                                    htmlx.Append("<td></td>\n");
                                    htmlx.Append("<td>" + rowAnt["descDetalhe1"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "&detalhe1=" + rowAnt["codDetalhe1"] + "&detalhe1Cod=" + detalhamento1;
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }
                                else
                                {
                                    htmlx.Append("<tr class=\"linha nivel0\">\n");
                                    htmlx.Append("<td>" + rowAnt["cod_conta"] + "</td>\n");
                                    htmlx.Append("<td>" + rowAnt["descricao"].ToString().Replace(" ", "&nbsp;") + "</td>\n");
                                    htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(acumulado.ToString())) + "</td>\n");
                                    for (int x = 0; x < vlrs.Count; x++)
                                    {
                                        if (Convert.ToInt32(rowAnt["analitica"]) == 1)
                                        {
                                            string parametros = "";
                                            htmlx.Append("<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + rowAnt["cod_conta"] + "&contaAte=" + rowAnt["cod_conta"] + "&periodo=" + vlrs[x].periodo.ToString("MM/yyyy") + filtros + parametros + "');\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</a></td>\n");
                                        }
                                        else
                                        {
                                            htmlx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", decimal.Parse(vlrs[x].valor.ToString())) + "</td>\n");
                                        }
                                    }

                                    htmlx.Append("</tr>\n");
                                }

                                codDetalhe1Ant = 0;
                                codDetalhe2Ant = 0;
                                codDetalhe3Ant = 0;
                                codDetalhe4Ant = 0;
                                nivel = 0;
                                rowAnt = null;
                            }

                            htmlx.Append("</table>\n");

                            literalRelatorio.Text = htmlx.ToString();

                        }
                        else
                        {
                            errosFormulario(erros);
                        }

                        break;
					#endregion
					#region Relatório de Movimentação Financeira
					case "MOV_FINANC2":

						textPeriodoDe = PreviousPage.txtPeriodoDe;
						textPeriodoAte = PreviousPage.txtPeriodoAte;

						comboContaDe = PreviousPage.cmbContaDe;
						comboContaAte = PreviousPage.cmbContaAte;
						comboDivisaoDe = PreviousPage.cmbDivisaoDe;
						comboDivisaoAte = PreviousPage.cmbDivisaoAte;
						comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
						comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
						comboClienteDe = PreviousPage.cmbClienteDe;
						comboClienteAte = PreviousPage.cmbClienteAte;
						comboJobDe = PreviousPage.cmbJobDe;
						comboJobAte = PreviousPage.cmbJobAte;

						comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
						comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
						comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
						comboDetalhamento4 = PreviousPage.cmbDetalhamento4;

						divisaoDe = null;
						divisaoAte = null;
						linhaNegocioDe = null;
						linhaNegocioAte = null;
						clienteDe = null;
						clienteAte = null;
						jobDe = null;
						jobAte = null;

						detalhamento1 = null;
						detalhamento2 = null;
						detalhamento3 = null;
						detalhamento4 = null;

						if (comboContaDe != null)
						{
							if (comboContaDe.SelectedValue == "0")
								contaDe = null;
							else
								contaDe = comboContaDe.SelectedValue;
						}

						if (comboContaAte != null)
						{
							if (comboContaAte.SelectedValue == "0")
								contaAte = null;
							else
								contaAte = comboContaAte.SelectedValue;
						}

						if (comboDivisaoDe != null)
						{
							if (comboDivisaoDe.SelectedValue == "0")
								divisaoDe = null;
							else
								divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
						}

						if (comboDivisaoAte != null)
						{
							if (comboDivisaoAte.SelectedValue == "0")
								divisaoAte = null;
							else
								divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
						}

						if (comboLinhaNegocioDe != null)
						{
							if (comboLinhaNegocioDe.SelectedValue == "0")
								linhaNegocioDe = null;
							else
								linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
						}

						if (comboLinhaNegocioAte != null)
						{
							if (comboLinhaNegocioAte.SelectedValue == "0")
								linhaNegocioAte = null;
							else
								linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
						}

						if (comboClienteDe != null)
						{
							if (comboClienteDe.SelectedValue == "0")
								clienteDe = null;
							else
								clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
						}

						if (comboClienteAte != null)
						{
							if (comboClienteAte.SelectedValue == "0")
								clienteAte = null;
							else
								clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
						}

						if (comboJobDe != null)
						{
							if (comboJobDe.SelectedValue == "0")
								jobDe = null;
							else
								jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
						}

						if (comboJobAte != null)
						{
							if (comboJobAte.SelectedValue == "0")
								jobAte = null;
							else
								jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
						}

						if (comboDetalhamento1 != null)
						{
							if (comboDetalhamento1.SelectedValue == "0")
								detalhamento1 = null;
							else
							{
								detalhamento1 = comboDetalhamento1.SelectedValue;

								if (comboDetalhamento2 != null)
								{
									if (comboDetalhamento2.SelectedValue == "0")
										detalhamento2 = null;
									else
									{
										detalhamento2 = comboDetalhamento2.SelectedValue;

										if (comboDetalhamento3 != null)
										{
											if (comboDetalhamento3.SelectedValue == "0")
												detalhamento3 = null;
											else
											{
												detalhamento3 = comboDetalhamento3.SelectedValue;

												if (comboDetalhamento4 != null)
												{
													if (comboDetalhamento4.SelectedValue == "0")
														detalhamento4 = null;
													else
													{
														detalhamento4 = comboDetalhamento4.SelectedValue;
													}
												}
											}
										}
									}
								}
							}
						}

						Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

						erros = relatorio.movimentacaoFinanceira(Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text), contaDe, contaAte,
							divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
							jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, ref tb);

                        if (PreviousPage.tipoRelatorio == 2) return;
						if (erros.Count == 0)
						{
							Title += " - Movimento Financeiro";
							empresa.codigo = Convert.ToInt32(Session["empresa"]);
							empresa.load();
							Master.literalNomeRelatorio.Text = "Movimentação Financeira";
							Master.literalNomeEmpresa.Text = empresa.nome;
							string detalhes = "";
							detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

							if (comboContaDe.SelectedValue != "0")
							{
								detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
								if (comboContaAte.SelectedValue != "0")
									detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
								else
									detalhes += "</p>";
							}

							if (comboDivisaoDe.SelectedValue != "0")
							{
								detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
								if (comboDivisaoAte.SelectedValue != "0")
									detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
								else
									detalhes += "</p>";
							}

							if (comboLinhaNegocioDe.SelectedValue != "0")
							{
								detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
								if (comboLinhaNegocioAte.SelectedValue != "0")
									detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
								else
									detalhes += "</p>";
							}

							if (comboClienteDe.SelectedValue != "0")
							{
								detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
								if (comboClienteAte.SelectedValue != "0")
									detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
								else
									detalhes += "</p>";
							}

							if (comboJobDe.SelectedValue != "0")
							{
								detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
								if (comboJobAte.SelectedValue != "0")
									detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
								else
									detalhes += "</p>";
							}

							Master.literalDetalhes.Text = detalhes;

							DateTime dataInicio = Convert.ToDateTime(textPeriodoDe.Text);
							DateTime dataFinal = Convert.ToDateTime(textPeriodoAte.Text);
							string codContaAnt = "";
							string TerceiroAnterior = "";
							DataRow row = null;
							DataRow rowAnt = null;
							decimal valor = 0;
							decimal totalEntradaConta = 0;
							decimal totalSaidaConta = 0;
							decimal totalSaldoInicial = 0;
							decimal totalSaldoFinal = 0;
							decimal totalMovimentoEntrada = 0;
							decimal totalValorTerceiro = 0;
							int numeroTerceiro = 0;
							decimal totalMovimentoSaida = 0;

							for (int h = 0; h < detalheMovFinanc.Length; h++)
							{
								detalheMovFinanc[h] = 0;
							}

							string html = "<table cellpadding=\"0\" id=\"mov_financeira\" cellspacing=\"0\">\n";
							html += "<tr class=\"titulo\">\n";
							html += "<td>Data</td>\n";
							html += "<td>Terceiro</td>\n";
							html += "<td>Job</td>\n";
							html += "<td>Histórico</td>\n";
							html += "<td class=\"valores\">Entrada</td>\n";
							html += "<td class=\"valores\">Saída</td>\n";
							html += "</tr>\n";

							for (int i = 0; i < tb.Rows.Count; i++)
							{
								row = tb.Rows[i];
								if (TerceiroAnterior == "")
								{
									TerceiroAnterior = row["nome_razao_social"].ToString();
									totalValorTerceiro = 0;
									numeroTerceiro = 0;
								}
								if (TerceiroAnterior != row["nome_razao_social"].ToString())
								{
									if (numeroTerceiro > 1)
									{
										html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
										if (totalValorTerceiro < 0)
										{
											html += "<td colspan=\"5\">       Subtotal de " + TerceiroAnterior + " </td>\n";
											totalValorTerceiro = totalValorTerceiro * -1;
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
										}
										else
										{
											html += "<td colspan=\"4\">       Subtotal de " + TerceiroAnterior + " </td>\n";
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
											html += "<td class=\"valores\">" + "" + "</td>\n";
										}

										html += "</tr>\n";
									}
									totalValorTerceiro = 0;
									numeroTerceiro = 0;
									TerceiroAnterior = row["nome_razao_social"].ToString();
								}

								totalValorTerceiro += Convert.ToDecimal(row["valor"]);
								numeroTerceiro++;

								if (row["cod_conta"].ToString().Equals(codContaAnt))
								{
									verificaNivel(hiddenRelatorio.Value, tb.Columns, 1, row, codContaAnt,
										ref html, ref valor, ref totalEntradaConta, ref totalSaidaConta);
									if (Convert.ToDecimal(row["valor"]) > 0)
									{
										totalMovimentoEntrada += Convert.ToDecimal(row["valor"]);
									}
									else
									{
										totalMovimentoSaida += Convert.ToDecimal(row["valor"]);
									}
								}
								else
								{
									if (rowAnt != null)
									{
										html += "<tr class=\"totalizador\">\n";
										html += "<td colspan=\"4\"></td>\n";
										html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
										html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
										html += "</tr>\n";

										decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

										if (TotalLiqui < 0)
										{
											totalEntradaConta = 0;
											totalSaidaConta = TotalLiqui * -1;
										}
										else
										{
											totalEntradaConta = TotalLiqui;
											totalSaidaConta = 0;
										}

										html += "<tr bgcolor=\"#87CEEB\">\n";
										html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
										html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
										html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
										html += "</tr>\n";

										html += "<tr class=\"fim nivel0\">\n";
										html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + rowAnt["descricao"] + " (Saldo Final) </td>\n";
										html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
										html += "</tr>\n";

										totalSaldoFinal += valor;

										valor = 0;
										totalEntradaConta = 0;
										totalSaidaConta = 0;


										if (row["descr"].ToString() == "Saldo Inicial")
										{
											totalSaldoInicial += Convert.ToDecimal(row["valor"]);

											html += "<tr class=\"inicio nivel0\">\n";
											html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
											html += "</tr>\n";
										}
										else
										{
											totalSaldoInicial += Convert.ToDecimal(row["valor"]);

											html += "<tr class=\"inicio nivel0\">\n";
											html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
											html += "</tr>\n";


											html += "<tr class=\"linha nivel0\">\n";
											html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
											html += "<td>" + row["nome_razao_social"] + "</td>\n";
											html += "<td>" + row["descr"] + "</td>\n";
											html += "<td>" + row["historico"] + "</td>\n";

											string inicioLink = "";
											string terminoLink = "";
											if (Convert.ToInt32(row["lote_pai"]) > 0)
											{
												inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
												terminoLink = "</a>";
											}

											if (Convert.ToDecimal(row["valor"]) > 0)
											{
												html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
												html += "<td class=\"valores\"></td>\n";
											}
											else
											{
												html += "<td class=\"valores\"></td>\n";
												html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
											}
											html += "</tr>\n";
										}
									}
									else
									{
										if (row["descr"].ToString() == "Saldo Inicial")
										{
											totalSaldoInicial += Convert.ToDecimal(row["valor"]);

											html += "<tr class=\"inicio nivel0\">\n";
											html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
											html += "</tr>\n";
										}
										else
										{


											totalSaldoInicial += Convert.ToDecimal(row["valor"]);

											decimal ValorRel = 0;
											if (row["descr"].ToString() == "Saldo Inicial")
											{
												ValorRel = Convert.ToDecimal(row["valor"]);
											}

											html += "<tr class=\"inicio nivel0\">\n";
											html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
											html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", ValorRel) + "</td>\n";
											html += "</tr>\n";


											html += "<tr class=\"linha nivel0\">\n";
											html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
											html += "<td>" + row["nome_razao_social"] + "</td>\n";
											html += "<td>" + row["descr"] + "</td>\n";
											html += "<td>" + row["historico"] + "</td>\n";

											string inicioLink = "";
											string terminoLink = "";
											if (Convert.ToInt32(row["lote_pai"]) > 0)
											{
												inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
												terminoLink = "</a>";
											}

											if (Convert.ToDecimal(row["valor"]) > 0)
											{
												html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
												html += "<td class=\"valores\"></td>\n";
											}
											else
											{
												html += "<td class=\"valores\"></td>\n";
												html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
											}
											html += "</tr>\n";
										}
									}

									valor += Convert.ToDecimal(row["valor"]);
									for (int h = 0; h < detalheMovFinanc.Length; h++)
									{
										detalheMovFinanc[h] = 0;
									}
								}
								rowAnt = row;
								codContaAnt = row["cod_conta"].ToString();
							}

							if (rowAnt != null)
							{
								html += "<tr class=\"totalizador\">\n";
								html += "<td colspan=\"4\"></td>\n";
								html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
								html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
								html += "</tr>\n";

								decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

								if (TotalLiqui < 0)
								{
									totalEntradaConta = 0;
									totalSaidaConta = TotalLiqui * -1;
								}
								else
								{
									totalEntradaConta = TotalLiqui;
									totalSaidaConta = 0;
								}

								html += "<tr bgcolor=\"#87CEEB\">\n";
								html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
								html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
								html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
								html += "</tr>\n";


								html += "<tr class=\"fim nivel0\">\n";
								html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + rowAnt["descricao"] + " (Saldo Final)</td>\n";
								html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
								html += "</tr>\n";

								totalSaldoFinal += valor;

								valor = 0;
							}

							html += "<tr class=\"resumo\">\n";
							html += "<td colspan=\"3\"></td>\n";
							html += "<td>Saldo Inicial Total</td>\n";
							html += "<td class=\"valores\"></td>\n";
							html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoInicial) + "</td>\n";
							html += "</tr>\n";

							html += "<tr class=\"resumo\">\n";
							html += "<td colspan=\"3\"></td>\n";
							html += "<td>Movimento Total</td>\n";
							html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoEntrada) + "</td>\n";
							html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoSaida) + "</td>\n";
							html += "</tr>\n";

							html += "<tr class=\"resumo\">\n";
							html += "<td colspan=\"3\"></td>\n";
							html += "<td>Saldo Final Total</td>\n";
							html += "<td class=\"valores\"></td>\n";
							html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoFinal) + "</td>\n";
							html += "</tr>\n";
							html += "</table>\n";
							literalRelatorio.Text = html;
						}
						else
						{
							errosFormulario(erros);
						}
						break;
                    #endregion
                    #region Relatório de Movimentação Bancária
                    case "MOV_BANCARIA":
                        textPeriodoDe = PreviousPage.txtPeriodoDe;
                        textPeriodoAte = PreviousPage.txtPeriodoAte;

                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;
						comboGrupo = PreviousPage.cmbGrupo;

						divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;
                        grupo = null;
                                    
                        if (comboContaDe != null)
                        {
                            if (comboContaDe.SelectedValue == "0")
                                contaDe = null;
                            else
                                contaDe = comboContaDe.SelectedValue;
                        }

                        if (comboContaAte != null)
                        {
                            if (comboContaAte.SelectedValue == "0")
                                contaAte = null;
                            else
                                contaAte = comboContaAte.SelectedValue;
                        }

                        grupo = comboGrupo.SelectedValue;

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.movimentacaoBancaria(Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text), contaDe, contaAte, grupo, ref tb);
                        bool multiPeriodo = DateTime.Compare(Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text)) != 0;

                        if (erros.Count == 0)
                        {
                            Title += " - Movimento Bancário";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Movimentação Bancário";
                            Master.literalNomeEmpresa.Text = empresa.nome;
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

                            if (comboContaDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
                                if (comboContaAte.SelectedValue != "0")
                                    detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            string grupoAnterior = "";
                            string contaAnterior = "";
                            decimal saldoGrupo = 0;
                            decimal saldoTotal = 0;
                            DateTime? dataAnterior = null;

                            string html = "<table cellpadding=\"0\" id=\"mov_financeira\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td>Grupo</td>\n";
                            html += "<td>Data</td>\n";
                            html += "<td>Histórico</td>\n";
                            html += "<td>Núm. Documento</td>\n";
                            html += "<td>Terceiro</td>\n";
                            html += "<td class=\"valores\">Entrada</td>\n";
                            html += "<td class=\"valores\">Saída</td>\n";
                            html += "<td class=\"valores\">Saldo</td>\n";
                            html += "</tr>\n";

                            foreach(DataRow linha in tb.Rows)
							{
                                if(Convert.ToString(linha["cod_conta"]) != contaAnterior)
								{
                                    finalizaGrupo_MovBancaria(ref html, ref grupoAnterior, ref saldoGrupo, saldoTotal, dataAnterior, multiPeriodo);
                                    inicializaConta_MovBancaria(ref html, ref saldoGrupo, ref saldoTotal, ref contaAnterior, ref grupoAnterior, linha);

                                    continue;
                                }

                                if(grupoAnterior != Convert.ToString(linha["grupo"]) || (dataAnterior != null && DateTime.Compare(dataAnterior.Value, Convert.ToDateTime(linha["data"])) != 0))
								{
                                    finalizaGrupo_MovBancaria(ref html, ref grupoAnterior, ref saldoGrupo, saldoTotal, dataAnterior, multiPeriodo);
                                    geraLinha_MovBancaria(ref html, ref saldoGrupo, ref saldoTotal, ref grupoAnterior, linha, ref dataAnterior, true);
                                }
								else
                                    geraLinha_MovBancaria(ref html, ref saldoGrupo, ref saldoTotal, ref grupoAnterior, linha, ref dataAnterior);

                            }

                            finalizaGrupo_MovBancaria(ref html, ref grupoAnterior, ref saldoGrupo, saldoTotal, dataAnterior, multiPeriodo);

                            var tbSaldoAnterior = tb.AsEnumerable().Where(o => Convert.ToString(o["historico"]).Equals("Saldo Inicial", StringComparison.CurrentCultureIgnoreCase));
                            var tbMovimentacao = tb.AsEnumerable().Where(o => !Convert.ToString(o["historico"]).Equals("Saldo Inicial", StringComparison.CurrentCultureIgnoreCase));

                            decimal somaSaldoAnterior = tbSaldoAnterior.Sum(o => Convert.ToDecimal(o["entradas"]) - Convert.ToDecimal(o["saidas"]));
                            decimal somaEntradas = tbMovimentacao.Sum(o => Convert.ToDecimal(o["entradas"]));
                            decimal somaSaidas = tbMovimentacao.Sum(o => Convert.ToDecimal(o["saidas"]));

                            html += "<tr class=\"inicio nivel0\", style=\"height:15px\">";
                            html += "<td colspan=\"9\" bgcolor=\"#AAAAAA\"></td>";
                            html += "</tr>";

                            html += "<tr class=\"inicio nivel0\">";
                            html += "<td colspan='9'> Conta: Resumo </td>";
                            html += "</tr>";
                            
                            html += "<tr class=\"resumo nivel0\">";
                            html += "<td colspan=4></td>";
                            html += "<td colspan=3>Saldo Anterior:</td>";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", somaSaldoAnterior) + "</td>";
                            html += "</tr>";

                            html += "<tr class=\"resumo nivel0\">";
                            html += "<td colspan=4></td>";
                            html += "<td colspan=3>Total Entradas:</td>";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", somaEntradas) + "</td>";
                            html += "</tr>";

                            html += "<tr class=\"resumo nivel0\">";
                            html += "<td colspan=4></td>";
                            html += "<td colspan=3>Total Saídas:</td>";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", somaSaidas) + "</td>";
                            html += "</tr>";

                            html += "<tr class=\"resumo nivel0\">";
                            html += "<td colspan=4></td>";
                            html += "<td colspan=3>Saldo Atual:</td>";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (somaSaldoAnterior + somaEntradas - somaSaidas)) + "</td>";
                            html += "</tr>";

                            literalRelatorio.Text = html;

                        }
                        else
                        {
                            errosFormulario(erros);
                        }
                        break;

                    #endregion
                    #region Relatório de Movimentação Financeira Rubens
                    case "MOV_FINANC":

                        textPeriodoDe = PreviousPage.txtPeriodoDe;
                        textPeriodoAte = PreviousPage.txtPeriodoAte;

                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;

                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;
                        maxDetalhe = 0;

                        if (comboContaDe != null)
                        {
                            if (comboContaDe.SelectedValue == "0")
                                contaDe = null;
                            else
                                contaDe = comboContaDe.SelectedValue;
                        }

                        if (comboContaAte != null)
                        {
                            if (comboContaAte.SelectedValue == "0")
                                contaAte = null;
                            else
                                contaAte = comboContaAte.SelectedValue;
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

                        if (comboDetalhamento1 != null)
                        {
                            if (comboDetalhamento1.SelectedValue == "0")
                                detalhamento1 = null;
                            else
                            {
                                detalhamento1 = comboDetalhamento1.SelectedValue;
                                maxDetalhe = 1;

                                if (comboDetalhamento2 != null)
                                {
                                    if (comboDetalhamento2.SelectedValue == "0")
                                        detalhamento2 = null;
                                    else
                                    {
                                        detalhamento2 = comboDetalhamento2.SelectedValue;
                                        maxDetalhe = 2;

                                        if (comboDetalhamento3 != null)
                                        {
                                            if (comboDetalhamento3.SelectedValue == "0")
                                                detalhamento3 = null;
                                            else
                                            {
                                                detalhamento3 = comboDetalhamento3.SelectedValue;
                                                maxDetalhe = 3;

                                                if (comboDetalhamento4 != null)
                                                {
                                                    if (comboDetalhamento4.SelectedValue == "0")
                                                        detalhamento4 = null;
                                                    else
                                                    {
                                                        detalhamento4 = comboDetalhamento4.SelectedValue;
                                                        maxDetalhe = 4;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.movimentacaoFinanceira(Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text), contaDe, contaAte,
                            divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                            jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, ref tb, PreviousPage.tipoRelatorio);

                        if (erros.Count == 0)
                        {
                            if (PreviousPage.tipoRelatorio == 2)
                            {
                                movimentacao_financeira_layout2(tb, maxDetalhe);
                                return;
                            }

                            Title += " - Movimento Financeiro";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Movimentação Financeira";
                            Master.literalNomeEmpresa.Text = empresa.nome;
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

                            if (comboContaDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
                                if (comboContaAte.SelectedValue != "0")
                                    detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            DateTime dataInicio = Convert.ToDateTime(textPeriodoDe.Text);
                            DateTime dataFinal = Convert.ToDateTime(textPeriodoAte.Text);
                            string codContaAnt = "";
                            string TerceiroAnterior = "";
                            DataRow row = null;
                            DataRow rowAnt = null;
                            decimal valor = 0;
                            decimal totalEntradaConta = 0;
                            decimal totalSaidaConta = 0;
                            decimal totalSaldoInicial = 0;
                            decimal totalSaldoFinal = 0;
                            decimal totalMovimentoEntrada = 0;
                            decimal totalValorTerceiro = 0;
                            decimal ValorAnterior = 0;
                            int numeroTerceiro = 0;
                            decimal totalMovimentoSaida = 0;

                            for (int h = 0; h < detalheMovFinanc.Length; h++)
                            {
                                detalheMovFinanc[h] = 0;
                            }

                            string html = "<table cellpadding=\"0\" id=\"mov_financeira\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td>Data</td>\n";
                            html += "<td>Terceiro</td>\n";
                            html += "<td>Job</td>\n";
                            html += "<td>Histórico</td>\n";
                            html += "<td class=\"valores\">Entrada</td>\n";
                            html += "<td class=\"valores\">Saída</td>\n";
                            html += "</tr>\n";

                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                row = tb.Rows[i];
                                if (TerceiroAnterior == "")
                                {
                                    TerceiroAnterior = row["nome_razao_social"].ToString();
                                    totalValorTerceiro = 0;
                                    numeroTerceiro = 0;
                                }
                                if (TerceiroAnterior != row["nome_razao_social"].ToString())
                                {
                                    if (numeroTerceiro > 1)
                                    {
                                        html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
                                        if (totalValorTerceiro < 0)
                                        {
                                            html += "<td colspan=\"5\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                            totalValorTerceiro = totalValorTerceiro * -1;
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                        }
                                        else
                                        {
                                            html += "<td colspan=\"4\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                            html += "<td class=\"valores\">" + "" + "</td>\n";
                                        }

                                        html += "</tr>\n";
                                    }
                                    totalValorTerceiro = 0;
                                    numeroTerceiro = 0;
                                    TerceiroAnterior = row["nome_razao_social"].ToString();
                                }

                                //totalValorTerceiro += Convert.ToDecimal(row["valor"]);
                                if (!tb.Columns.Contains("codDetalhe1") || Convert.ToInt32(row["codDetalhe1"]) == 0)
                                {
                                    totalValorTerceiro += Convert.ToDecimal(row["valor"]);
                                }

                                numeroTerceiro++;

                                if (row["cod_conta"].ToString().Equals(codContaAnt))
                                {
                                    verificaNivel(hiddenRelatorio.Value, tb.Columns, 1, row, codContaAnt,
                                        ref html, ref valor, ref totalEntradaConta, ref totalSaidaConta);
                                    if (Convert.ToDecimal(row["valor"]) > 0)
                                    {
                                        totalMovimentoEntrada += Convert.ToDecimal(row["valor"]);
                                    }
                                    else
                                    {
                                        totalMovimentoSaida += Convert.ToDecimal(row["valor"]);
                                    }
                                    if (numeroTerceiro > 1)
                                    {
                                        //if (ValorAnterior > 0)
                                        //{
                                        //    totalEntradaConta -= ValorAnterior;
                                        //    totalMovimentoEntrada -= ValorAnterior;
                                        //}
                                        //else
                                        //{
                                        //    totalSaidaConta -= ValorAnterior;
                                        //    totalMovimentoSaida -= ValorAnterior;
                                        //}
                                        //if (totalValorTerceiro > 0)
                                        //{
                                        //    totalEntradaConta += totalValorTerceiro;
                                        //    totalMovimentoEntrada += totalValorTerceiro;
                                        //}
                                        //else
                                        //{
                                        //    totalSaidaConta += totalValorTerceiro;
                                        //    totalMovimentoSaida += totalValorTerceiro;
                                        //}
                                    }
                                    ValorAnterior = totalValorTerceiro;
                                }
                                else
                                {
                                    if (rowAnt != null)
                                    {
                                        html += "<tr class=\"totalizador\">\n";
                                        html += "<td colspan=\"4\"></td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                        html += "</tr>\n";

                                        decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

                                        if (TotalLiqui < 0)
                                        {
                                            totalEntradaConta = 0;
                                            totalSaidaConta = TotalLiqui * -1;
                                        }
                                        else
                                        {
                                            totalEntradaConta = TotalLiqui;
                                            totalSaidaConta = 0;
                                        }

                                        html += "<tr bgcolor=\"#87CEEB\">\n";
                                        html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                        html += "</tr>\n";

                                        html += "<tr class=\"fim nivel0\">\n";
                                        html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + rowAnt["descricao"] + " (Saldo Final) </td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
                                        html += "</tr>\n";

                                        totalSaldoFinal += valor;

                                        valor = 0;
                                        totalEntradaConta = 0;
                                        totalSaidaConta = 0;


                                        if (row["descr"].ToString() == "Saldo Inicial")
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";


                                            html += "<tr class=\"linha nivel0\">\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["descr"] + "</td>\n";
                                            html += "<td>" + row["historico"] + "</td>\n";

                                            string inicioLink = "";
                                            string terminoLink = "";
                                            if (Convert.ToInt32(row["lote_pai"]) > 0)
                                            {
                                                inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
                                                terminoLink = "</a>";
                                            }

                                            if (Convert.ToDecimal(row["valor"]) > 0)
                                            {
                                                html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
                                                html += "<td class=\"valores\"></td>\n";
                                            }
                                            else
                                            {
                                                html += "<td class=\"valores\"></td>\n";
                                                html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
                                            }
                                            html += "</tr>\n";
                                        }
                                    }
                                    else
                                    {
                                        if (row["descr"].ToString() == "Saldo Inicial")
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {


                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            decimal ValorRel = 0;
                                            if (row["descr"].ToString() == "Saldo Inicial")
                                            {
                                                ValorRel = Convert.ToDecimal(row["valor"]);
                                            }

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", ValorRel) + "</td>\n";
                                            html += "</tr>\n";


                                            html += "<tr class=\"linha nivel0\">\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["descr"] + "</td>\n";
                                            html += "<td>" + row["historico"] + "</td>\n";

                                            string inicioLink = "";
                                            string terminoLink = "";
                                            if (Convert.ToInt32(row["lote_pai"]) > 0)
                                            {
                                                inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
                                                terminoLink = "</a>";
                                            }

                                            if (Convert.ToDecimal(row["valor"]) > 0)
                                            {
                                                html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
                                                html += "<td class=\"valores\"></td>\n";
                                            }
                                            else
                                            {
                                                html += "<td class=\"valores\"></td>\n";
                                                html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
                                            }
                                            html += "</tr>\n";
                                        }
                                    }

                                    valor += Convert.ToDecimal(row["valor"]);
                                    for (int h = 0; h < detalheMovFinanc.Length; h++)
                                    {
                                        detalheMovFinanc[h] = 0;
                                    }
                                }
                                rowAnt = row;
                                codContaAnt = row["cod_conta"].ToString();
                            }

                            if (numeroTerceiro > 1)
                            {
                                ValorAnterior = Convert.ToDecimal(rowAnt["valor"]);

                                //if (ValorAnterior > 0)
                                //{
                                //    totalEntradaConta -= ValorAnterior;
                                //    totalMovimentoEntrada -= ValorAnterior;
                                //}
                                //else
                                //{
                                //    totalSaidaConta -= ValorAnterior;
                                //    totalMovimentoSaida -= ValorAnterior;
                                //}

                                html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
                                if (totalValorTerceiro < 0)
                                {
                                    html += "<td colspan=\"5\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                    totalValorTerceiro = totalValorTerceiro * -1;
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                }
                                else
                                {
                                    html += "<td colspan=\"4\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                    html += "<td class=\"valores\">" + "" + "</td>\n";
                                }

                                html += "</tr>\n";
                            }

                            if (rowAnt != null)
                            {
                                html += "<tr class=\"totalizador\">\n";
                                html += "<td colspan=\"4\"></td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                html += "</tr>\n";

                                decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

                                if (TotalLiqui < 0)
                                {
                                    totalEntradaConta = 0;
                                    totalSaidaConta = TotalLiqui * -1;
                                }
                                else
                                {
                                    totalEntradaConta = TotalLiqui;
                                    totalSaidaConta = 0;
                                }

                                html += "<tr bgcolor=\"#87CEEB\">\n";
                                html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                html += "</tr>\n";


                                html += "<tr class=\"fim nivel0\">\n";
                                html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + rowAnt["descricao"] + " (Saldo Final)</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
                                html += "</tr>\n";

                                totalSaldoFinal += valor;

                                valor = 0;
                            }

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Saldo Inicial Total</td>\n";
                            html += "<td class=\"valores\"></td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoInicial) + "</td>\n";
                            html += "</tr>\n";

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Movimento Total</td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoEntrada) + "</td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoSaida) + "</td>\n";
                            html += "</tr>\n";

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Saldo Final Total</td>\n";
                            html += "<td class=\"valores\"></td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoFinal) + "</td>\n";
                            html += "</tr>\n";
                            html += "</table>\n";
                            literalRelatorio.Text = html;
                        }
                        else
                        {
                            errosFormulario(erros);
                        }
                        break;
                    #endregion
                    #region Relatório de Movimentação Financeira - Beta
                    case "MOV_FINANC_BETA":

                        textPeriodoDe = PreviousPage.txtPeriodoDe;
                        textPeriodoAte = PreviousPage.txtPeriodoAte;

                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;

                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;

                        if (comboContaDe != null)
                        {
                            if (comboContaDe.SelectedValue == "0")
                                contaDe = null;
                            else
                                contaDe = comboContaDe.SelectedValue;
                        }

                        if (comboContaAte != null)
                        {
                            if (comboContaAte.SelectedValue == "0")
                                contaAte = null;
                            else
                                contaAte = comboContaAte.SelectedValue;
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

						bool job = false;
                        if (comboDetalhamento1 != null)
                        {
                            if (comboDetalhamento1.SelectedValue == "0")
                                detalhamento1 = null;
                            else
                            {
								maxNivel++;
                                detalhamento1 = comboDetalhamento1.SelectedValue;
								if (detalhamento1.Equals("JOB")) job = true;
                                if (comboDetalhamento2 != null)
                                {
                                    if (comboDetalhamento2.SelectedValue == "0")
                                        detalhamento2 = null;
                                    else
                                    {
										maxNivel++;
										detalhamento2 = comboDetalhamento2.SelectedValue;
										if (detalhamento2.Equals("JOB")) job = true;
										if (comboDetalhamento3 != null)
                                        {
                                            if (comboDetalhamento3.SelectedValue == "0")
                                                detalhamento3 = null;
                                            else
                                            {
												maxNivel++;
												detalhamento3 = comboDetalhamento3.SelectedValue;
												if (detalhamento3.Equals("JOB")) job = true;
												if (comboDetalhamento4 != null)
                                                {
                                                    if (comboDetalhamento4.SelectedValue == "0")
                                                        detalhamento4 = null;
                                                    else
                                                    {
														maxNivel++;
														detalhamento4 = comboDetalhamento4.SelectedValue;
														if (detalhamento4.Equals("JOB")) job = true;
													}
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
						if (!job)
							maxNivel++;

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.movimentacaoFinanceiraBeta(Convert.ToDateTime(textPeriodoDe.Text), Convert.ToDateTime(textPeriodoAte.Text), contaDe, contaAte,
                            divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                            jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, ref tb);
                        if (erros.Count == 0)
                        {
                            Title += " - Movimento Financeiro";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Movimentação Financeira";
                            Master.literalNomeEmpresa.Text = empresa.nome;
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

                            if (comboContaDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
                                if (comboContaAte.SelectedValue != "0")
                                    detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            DateTime dataInicio = Convert.ToDateTime(textPeriodoDe.Text);
                            DateTime dataFinal = Convert.ToDateTime(textPeriodoAte.Text);
                            string codContaAnt = "";
                            string TerceiroAnterior = "";
                            DataRow row = null;
                            DataRow rowAnt = null;
                            decimal valor = 0;
                            decimal totalEntradaConta = 0;
                            decimal totalSaidaConta = 0;
                            decimal totalSaldoInicial = 0;
                            decimal totalSaldoFinal = 0;
                            decimal totalMovimentoEntrada = 0;
                            decimal totalValorTerceiro = 0;
                            int numeroTerceiro = 0;
                            decimal totalMovimentoSaida = 0;
							decimal valorNivelAtual = 0;
							int nivelAtual = -1;

                            for (int h = 0; h < detalheMovFinanc.Length; h++)
                            {
                                detalheMovFinanc[h] = 0;
                            }

                            string html = "<table cellpadding=\"0\" id=\"mov_financeira\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td>Data</td>\n";
                            html += "<td>Terceiro</td>\n";
                            html += "<td>Job</td>\n";
                            html += "<td>Histórico</td>\n";
                            html += "<td class=\"valores\">Entrada</td>\n";
                            html += "<td class=\"valores\">Saída</td>\n";
                            html += "</tr>\n";

                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                row = tb.Rows[i];
                                if (TerceiroAnterior == "")
                                {
                                    TerceiroAnterior = row["nome_razao_social"].ToString();
                                    totalValorTerceiro = 0;
                                    numeroTerceiro = 0;
                                }
                                if (TerceiroAnterior != row["nome_razao_social"].ToString())
                                {
									if(rowAnt != null && nivelAtual != -1)
									{
										rowAnt["valor"] = valorNivelAtual;
										geraLinhaBeta(hiddenRelatorio.Value, nivelAtual, rowAnt, ref html, maxNivel);
									}

                                    if (numeroTerceiro > 1)
                                    {
                                        html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
                                        if (totalValorTerceiro < 0)
                                        {
                                            html += "<td colspan=\"5\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                            totalValorTerceiro = totalValorTerceiro * -1;
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                        }
                                        else
                                        {
                                            html += "<td colspan=\"4\">       Subtotal de " + TerceiroAnterior + " </td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalValorTerceiro) + "</td>\n";
                                            html += "<td class=\"valores\">" + "" + "</td>\n";
                                        }
                                        
                                        html += "</tr>\n";
                                    }
                                    totalValorTerceiro = 0;
                                    numeroTerceiro = 0;
                                    TerceiroAnterior = row["nome_razao_social"].ToString();
                                }

                                totalValorTerceiro += Convert.ToDecimal(row["valor"]);
                                numeroTerceiro++;

                                if (row["cod_conta"].ToString().Equals(codContaAnt))
                                {
                                    verificaNivelBeta(hiddenRelatorio.Value, tb.Columns, 1, row, codContaAnt, 
                                        ref html, ref valor, ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, rowAnt);
                                    if (Convert.ToDecimal(row["valor"]) > 0)
                                    {
                                        totalMovimentoEntrada += Convert.ToDecimal(row["valor"]);
                                    }
                                    else
                                    {
                                        totalMovimentoSaida += Convert.ToDecimal(row["valor"]);
                                    }
                                }
                                else
                                {
                                    if (rowAnt != null)
                                    {
										if (nivelAtual != -1)
										{
											rowAnt["valor"] = valorNivelAtual;
											geraLinhaBeta(hiddenRelatorio.Value, nivelAtual, rowAnt, ref html, maxNivel);
										}

										html += "<tr class=\"totalizador\">\n";
                                        html += "<td colspan=\"4\"></td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                        html += "</tr>\n";

                                        decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

                                        if (TotalLiqui < 0)
                                        {
                                            totalEntradaConta = 0;
                                            totalSaidaConta = TotalLiqui * -1;
                                        }
                                        else
                                        {
                                            totalEntradaConta = TotalLiqui;
                                            totalSaidaConta = 0;
                                        }

                                        html += "<tr bgcolor=\"#87CEEB\">\n";
                                        html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                        html += "</tr>\n";

                                        html += "<tr class=\"fim nivel0\">\n";
                                        html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + rowAnt["cod_conta"] + " - " + rowAnt["descricao"] + " (Saldo Final) </td>\n";
                                        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
                                        html += "</tr>\n";
                                        
                                        totalSaldoFinal += valor;

                                        valor = 0;
                                        totalEntradaConta = 0;
                                        totalSaidaConta = 0;


                                        if (row["descr"].ToString() == "Saldo Inicial")
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["cod_conta"] + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";

											valor += Convert.ToDecimal(row["valor"]);

										}
										else
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["cod_conta"] + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";


											verificaNivelBeta(hiddenRelatorio.Value, tb.Columns, 1, row, codContaAnt,
										ref html, ref valor, ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, rowAnt);

											//if (Convert.ToDecimal(row["valor"]) > 0)
											//{
											//	totalMovimentoEntrada += Convert.ToDecimal(row["valor"]);
											//}
											//else
											//{
											//	totalMovimentoSaida += Convert.ToDecimal(row["valor"]);
											//}

											// Bloco acima removido, pois calcula o valor inicial de terceiros na movimentacao geral, causando inconsistencia na totalizacao final.
											//--Gabriel

											//html += "<tr class=\"linha nivel0\">\n";
											//html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
											//html += "<td>" + row["nome_razao_social"] + "</td>\n";
											//html += "<td> </td>\n";
											//html += "<td> </td>\n";


											//html += "<td>" + row["descr"] + "</td>\n";
											//html += "<td>" + row["historico"] + "</td>\n";

											//string inicioLink = "";
											//string terminoLink = "";
											//if (Convert.ToInt32(row["lote_pai"]) > 0)
											//{
											//	inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
											//	terminoLink = "</a>";
											//}

											//if (Convert.ToDecimal(row["valor"]) > 0)
											//{
											//	html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
											//	html += "<td class=\"valores\"></td>\n";
											//}
											//else
											//{
											//	html += "<td class=\"valores\"></td>\n";
											//	html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
											//}
											//html += "</tr>\n";
										}
									}
                                    else
                                    {
                                        if (row["descr"].ToString() == "Saldo Inicial")
                                        {
                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["cod_conta"] + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {
                                            

                                            totalSaldoInicial += Convert.ToDecimal(row["valor"]);

                                            decimal ValorRel = 0;
                                            if (row["descr"].ToString() == "Saldo Inicial")
                                            {
                                                ValorRel = Convert.ToDecimal(row["valor"]);
                                            }

                                            html += "<tr class=\"inicio nivel0\">\n";
                                            html += "<td colspan=\"5\">" + dataInicio.ToString("dd/MM/yyyy") + " - " + row["cod_conta"] + " - " + row["descricao"] + " (Saldo Inicial)</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", ValorRel) + "</td>\n";
                                            html += "</tr>\n";


           //                                 html += "<tr class=\"linha nivel0\">\n";
           //                                 html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
           //                                 html += "<td>" + row["nome_razao_social"] + "</td>\n";
											//html += "<td> </td>\n";
											//html += "<td> </td>\n";
											//html += "<td>" + row["descr"] + "</td>\n";
											//html += "<td>" + row["historico"] + "</td>\n";

											verificaNivelBeta(hiddenRelatorio.Value, tb.Columns, 1, row, codContaAnt,
										ref html, ref valor, ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, rowAnt);
											
											//if (Convert.ToDecimal(row["valor"]) > 0)
											//{
											//	totalMovimentoEntrada += Convert.ToDecimal(row["valor"]);
											//}
											//else
											//{
											//	totalMovimentoSaida += Convert.ToDecimal(row["valor"]);
											//}
											
											// Bloco acima removido, pois calcula o valor inicial de terceiros na movimentacao geral, causando inconsistencia na totalizacao final.
											//--Gabriel

											rowAnt = row;
											codContaAnt = row["cod_conta"].ToString();
											continue;

											//string inicioLink = "";
           //                                 string terminoLink = "";
           //                                 if (Convert.ToInt32(row["lote_pai"]) > 0)
           //                                 {
           //                                     inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
           //                                     terminoLink = "</a>";
           //                                 }

           //                                 if (Convert.ToDecimal(row["valor"]) > 0)
           //                                 {
           //                                     html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
           //                                     html += "<td class=\"valores\"></td>\n";
           //                                 }
           //                                 else
           //                                 {
           //                                     html += "<td class=\"valores\"></td>\n";
           //                                     html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
           //                                 }
           //                                 html += "</tr>\n";

											//valor += Convert.ToDecimal(row["valor"]);

										}
									}

                                    //valor += Convert.ToDecimal(row["valor"]);
                                    for (int h = 0; h < detalheMovFinanc.Length; h++)
                                    {
                                        detalheMovFinanc[h] = 0;
                                    }
                                }
                                rowAnt = row;
                                codContaAnt = row["cod_conta"].ToString();
                            }

                            if (rowAnt != null)
                            {
								if (nivelAtual != -1)
								{
									rowAnt["valor"] = valorNivelAtual;
									geraLinhaBeta(hiddenRelatorio.Value, nivelAtual, rowAnt, ref html, maxNivel);
								}

								html += "<tr class=\"totalizador\">\n";
                                html += "<td colspan=\"4\"></td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                html += "</tr>\n";

                                decimal TotalLiqui = totalEntradaConta + totalSaidaConta;

                                if (TotalLiqui < 0)
                                {
                                    totalEntradaConta = 0;
                                    totalSaidaConta = TotalLiqui * -1;
                                }
                                else
                                {
                                    totalEntradaConta = TotalLiqui;
                                    totalSaidaConta = 0;
                                }

                                html += "<tr bgcolor=\"#87CEEB\">\n";
                                html += "<td colspan=\"4\"> Movimento Liquido </td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalEntradaConta) + "</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaidaConta) + "</td>\n";
                                html += "</tr>\n";


                                html += "<tr class=\"fim nivel0\">\n";
                                html += "<td colspan=\"5\">" + dataFinal.ToString("dd/MM/yyyy") + " - " + row["cod_conta"] + " - " + rowAnt["descricao"] + " (Saldo Final)</td>\n";
                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n";
                                html += "</tr>\n";

                                totalSaldoFinal += valor;

                                valor = 0;
                            }

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Saldo Inicial Total</td>\n";
                            html += "<td class=\"valores\"></td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoInicial) + "</td>\n";
                            html += "</tr>\n";

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Movimento Total</td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoEntrada) + "</td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalMovimentoSaida) + "</td>\n";
                            html += "</tr>\n";

                            html += "<tr class=\"resumo\">\n";
                            html += "<td colspan=\"3\"></td>\n";
                            html += "<td>Saldo Final Total</td>\n";
                            html += "<td class=\"valores\"></td>\n";
                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalSaldoFinal) + "</td>\n";
                            html += "</tr>\n";
                            html += "</table>\n";
                            literalRelatorio.Text = html;
                        }
                        else
                        {
                            errosFormulario(erros);
                        }
                        break;
                    #endregion
                    #region Relatório de Títulos Pendentes
                    case "TITULO_PENDENTE":

                        textDiasVencimento = PreviousPage.txtDiasVencimento;
                        textPeriodo = PreviousPage.txtPeriodo;
                        comboCPCR = PreviousPage.cmbCPCR;
                        comboOrdenacao = PreviousPage.cmbOrdenacao;
                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;

                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;

                        tipo = null;
                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;


                        if (comboCPCR != null)
                        {
                            if (comboCPCR.SelectedValue == "0")
                                tipo = null;
                            else
                                tipo = comboCPCR.SelectedValue;
                        }

                        if (comboContaDe != null)
                        {
                            if (comboContaDe.SelectedValue == "0")
                                contaDe = null;
                            else
                                contaDe = comboContaDe.SelectedValue;
                        }

                        if (comboContaAte != null)
                        {
                            if (comboContaAte.SelectedValue == "0")
                                contaAte = null;
                            else
                                contaAte = comboContaAte.SelectedValue;
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.titulosPendentes(Convert.ToDateTime(textPeriodo.Text), Convert.ToInt32(textDiasVencimento.Text), tipo,
                            contaDe,contaAte,divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, comboOrdenacao.SelectedValue, ref tb);

                        if (erros.Count == 0)
                        {
                            Title += " - Títulos Pendentes";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Títulos Pendentes";
                            Master.literalNomeEmpresa.Text = empresa.nome;
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodo.Text + "</p>";

                            if (comboContaDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
                                if (comboContaAte.SelectedValue != "0")
                                    detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            string html = "<table cellpadding=\"0\" id=\"titulos_pendentes\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td>Cliente/Fornecedor</td>\n";
                            html += "<td>Job</td>\n";
                            html += "<td>Data Lançamento</td>";
                            html += "<td>Nº Títulos</td>\n";
                            html += "<td>Vencimento</td>\n";
                            html += "<td class=\"valores\">Valor</td>\n";
                            html += "<td  class=\"valores\">Status</td>\n";
                            html += "</tr>\n";

                            string dataVencimentoAnt = "";
                            string codTerceiroAnt = "";
                            string moduloAnt = "";
                            string codContaAnt = "";
                            DataRow row = null;
                            DataRow rowAnt = null;
                            decimal valor = 0;
                            decimal totalModulo = 0;
                            decimal totalRel = 0;

                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                row = tb.Rows[i];
                                string urlDrillDown = "";
                                if (Convert.ToDouble(row["seq_baixa"]) == 0)
                                {
                                    if (Convert.ToDouble(row["lote_pai"]) == 0)
                                    {
                                        urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo_original"].ToString() + "&lote=" + row["lote"].ToString();
                                    }
                                    else
                                    {
                                        urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo_original"].ToString() + "&lote=" + row["lote_pai"].ToString();
                                    }
                                }
                                else
                                {
                                    urlDrillDown = "FormDetalheBaixa.aspx?id=" + row["seq_baixa"].ToString();
                                }

                                #region Agrupamento por Data
                                if (comboOrdenacao.SelectedValue == "DATA")
                                {
                                    if (row["modulo"].ToString() == moduloAnt)
                                    {
                                        if (Convert.ToDateTime(row["data"]).ToString("MMyyyy").Equals(Convert.ToDateTime(dataVencimentoAnt).ToString("MMyyyy")))
                                        {
                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\""+urlDrillDown+"\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }
                                        else
                                        {
                                            if (rowAnt != null)
                                            {
                                                html += "<tr class=\"fim nivel0\">\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td colspan=\"2\" align=\"right\">Total de " + Convert.ToDateTime(rowAnt["data"]).ToString("MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                                html += "<td></td>";
                                                html += "</tr>\n";
                                                valor = 0;


                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }
                                            else
                                            {
                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }

                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                    else
                                    {
                                        if (rowAnt != null)
                                        {
                                            html += "<tr class=\"fim nivel0\">\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td colspan=\"2\" align=\"right\">Total de " + Convert.ToDateTime(rowAnt["data"]).ToString("MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            html += "<tr class=\"fim modulo\">\n";
                                            html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                            html += "<td></td>";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            valor = 0;
                                            totalModulo = 0;


                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {
                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                        }

                                        valor += Convert.ToDecimal(row["valor"]);
                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                }
                                #endregion
                                #region Agrupamento por Terceiro
                                else if (comboOrdenacao.SelectedValue == "TERCEIRO")
                                {
                                    if (row["modulo"].ToString() == moduloAnt)
                                    {
                                        if (row["cod_terceiro"].ToString().Equals(codTerceiroAnt))
                                        {
                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }
                                        else
                                        {
                                            if (rowAnt != null)
                                            {
                                                html += "<tr class=\"fim nivel0\">\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["nome_razao_social"] + "</td>\n";
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                                html += "<td></td>";
                                                html += "</tr>\n";
                                                valor = 0;


                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }
                                            else
                                            {
                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }

                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                    else
                                    {
                                        if (rowAnt != null)
                                        {
                                            html += "<tr class=\"fim nivel0\">\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["nome_razao_social"] + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            html += "<tr class=\"fim modulo\">\n";
                                            html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                            html += "<td></td>";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            valor = 0;
                                            totalModulo = 0;


                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {
                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                        }

                                        valor += Convert.ToDecimal(row["valor"]);
                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                }
                                #endregion
                                #region Agrupamento por Conta
                                else if (comboOrdenacao.SelectedValue == "CONTA")
                                {
                                    if (row["modulo"].ToString() == moduloAnt)
                                    {
                                        if (row["cod_conta"].ToString().Equals(codContaAnt))
                                        {
                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }
                                        else
                                        {
                                            if (rowAnt != null)
                                            {
                                                html += "<tr class=\"fim nivel0\">\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td></td>\n";
                                                html += "<td colspan=\"2\" align=\"right\">Total de "  + rowAnt["desc_conta"] + "</td>\n";
                                                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                                html += "<td></td>";
                                                html += "</tr>\n";
                                                valor = 0;


                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }
                                            else
                                            {
                                                html += "<tr class=\"nivel0\">\n";
                                                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                                html += "<td>" + row["desc_job"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td>" + row["lote"] + "</td>\n";
                                                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                                html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                                html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                                html += "</tr>\n";
                                            }

                                            valor += Convert.ToDecimal(row["valor"]);
                                        }

                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                    else
                                    {
                                        if (rowAnt != null)
                                        {
                                            html += "<tr class=\"fim nivel0\">\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["desc_conta"] + "</td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            html += "<tr class=\"fim modulo\">\n";
                                            html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                            html += "<td></td>";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                            html += "<td></td>";
                                            html += "</tr>\n";

                                            valor = 0;
                                            totalModulo = 0;


                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";
                                        }
                                        else
                                        {
                                            html += "<tr class=\"inicio modulo\">\n";
                                            html += "<td>" + row["modulo"] + "</td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td></td>\n";
                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</td>\n";
                                            html += "<td class=\"valores\"></td>\n";
                                            html += "</tr>\n";

                                            html += "<tr class=\"nivel0\">\n";
                                            html += "<td>" + row["nome_razao_social"] + "</td>\n";
                                            html += "<td>" + row["desc_job"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data_lancamento"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td>" + row["lote"] + "</td>\n";
                                            html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                                            html += "<td class=\"valores\"><a href=\"" + urlDrillDown + "\" target=\"_blank\" >" + String.Format("{0:0,0.00}", (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"]))) + "</a></td>\n";
                                            html += "<td class=\"valores\">" + row["status"] + "</td>\n";
                                            html += "</tr>\n";

                                        }

                                        valor += Convert.ToDecimal(row["valor"]);
                                        totalModulo += Convert.ToDecimal(row["valor"]);
                                    }
                                }
                                #endregion
                                else { }

                                rowAnt = row;
                                codContaAnt = row["cod_conta"].ToString();
                                moduloAnt = row["modulo"].ToString();
                                dataVencimentoAnt = row["data"].ToString();
                            }

                            if (comboOrdenacao.SelectedValue == "DATA")
                            {
                                if (rowAnt != null)
                                {
                                    html += "<tr class=\"fim nivel0\">\n";
                                    html += "<td></td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td colspan=\"2\" align=\"right\">Total de " + Convert.ToDateTime(rowAnt["data"]).ToString("MM/yyyy") + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    html += "<tr class=\"fim modulo\">\n";
                                    html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    valor = 0;
                                    totalModulo = 0;
                                }
                                else
                                {

                                }
                            }
                            else if (comboOrdenacao.SelectedValue == "TERCEIRO")
                            {
                                if (rowAnt != null)
                                {
                                    html += "<tr class=\"fim nivel0\">\n";
                                    html += "<td></td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["nome_razao_social"] + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    html += "<tr class=\"fim modulo\">\n";
                                    html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    valor = 0;
                                    totalModulo = 0;
                                }
                                else
                                {

                                }
                            }
                            else if (comboOrdenacao.SelectedValue == "CONTA")
                            {
                                if (rowAnt != null)
                                {
                                    html += "<tr class=\"fim nivel0\">\n";
                                    html += "<td></td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["desc_conta"] + "</td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor < 0 ? valor * -1 : valor)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    html += "<tr class=\"fim modulo\">\n";
                                    html += "<td>Total de " + rowAnt["modulo"] + "</td>";
                                    html += "<td></td>";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td></td>\n";
                                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (totalModulo < 0 ? totalModulo * -1 : totalModulo)) + "</td>\n";
                                    html += "<td></td>";
                                    html += "</tr>\n";

                                    valor = 0;
                                    totalModulo = 0;
                                }
                                else
                                {

                                }
                            }
                            else { }

                            html += "</table>\n";

                            literalRelatorio.Text = html;
                        }
                        else
                        {
                            errosFormulario(erros);
                        }
                        break;
                    #endregion
                    #region Razao
                    case "RAZAO":
                        textPeriodoDe = PreviousPage.txtPeriodoDe;
                        textPeriodoAte = PreviousPage.txtPeriodoAte;
                        checkDuasMoedas = PreviousPage.checkBoxDuasMoedas;

                        comboContaDe = PreviousPage.cmbContaDe;
                        comboContaAte = PreviousPage.cmbContaAte;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;
                        comboTerDe = PreviousPage.cmbTerDe;
                        comboTerAte = PreviousPage.cmbTerAte;
                        comboMoeda = PreviousPage.cmbMoeda;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;
                        comboDetalhamento5 = PreviousPage.cmbDetalhamento5;

                        empresas = PreviousPage.empresas_selecionadas3;

                        detalhamento1 = null;
                        detalhamento2 = null;
                        detalhamento3 = null;
                        detalhamento4 = null;
                        detalhamento5 = null;

                        int codMoeda = 0;

                        if (comboMoeda != null)
                        {
                            if (comboMoeda.SelectedValue == "0")
                                codMoeda = 0;
                            else
                                codMoeda = Convert.ToInt32(comboMoeda.SelectedValue);
                        }

                        if (comboContaDe != null)
                        {
                            if (comboContaDe.SelectedValue == "0")
                                contaDe = null;
                            else
                                contaDe = comboContaDe.SelectedValue;
                        }

                        if (comboContaAte != null)
                        {
                            if (comboContaAte.SelectedValue == "0")
                                contaAte = null;
                            else
                                contaAte = comboContaAte.SelectedValue;
                        }

                        if (comboDivisaoDe != null)
                        {
                            if (comboDivisaoDe.SelectedValue == "0")
                                divisaoDe = null;
                            else
                                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
                        }

                        if (comboDivisaoAte != null)
                        {
                            if (comboDivisaoAte.SelectedValue == "0")
                                divisaoAte = null;
                            else
                                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
                        }

                        if (comboLinhaNegocioDe != null)
                        {
                            if (comboLinhaNegocioDe.SelectedValue == "0")
                                linhaNegocioDe = null;
                            else
                                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
                        }

                        if (comboLinhaNegocioAte != null)
                        {
                            if (comboLinhaNegocioAte.SelectedValue == "0")
                                linhaNegocioAte = null;
                            else
                                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
                        }

                        if (comboClienteDe != null)
                        {
                            if (comboClienteDe.SelectedValue == "0")
                                clienteDe = null;
                            else
                                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
                        }

                        if (comboClienteAte != null)
                        {
                            if (comboClienteAte.SelectedValue == "0")
                                clienteAte = null;
                            else
                                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
                        }

                        if (comboJobDe != null)
                        {
                            if (comboJobDe.SelectedValue == "0")
                                jobDe = null;
                            else
                                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
                        }

                        if (comboJobAte != null)
                        {
                            if (comboJobAte.SelectedValue == "0")
                                jobAte = null;
                            else
                                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
                        }

                        if (comboTerDe != null)
                        {
                            if (comboTerDe.SelectedValue == "0")
                                terDe = null;
                            else
                                terDe = Convert.ToInt32(comboTerDe.SelectedValue);
                        }

                        if (comboTerAte != null)
                        {
                            if (comboTerAte.SelectedValue == "0")
                                terAte = null;
                            else
                                terAte = Convert.ToInt32(comboTerAte.SelectedValue);
                        }

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        erros = relatorio.razaoContabil(codMoeda, contaDe, contaAte, Convert.ToDateTime(textPeriodoDe.Text),
                            Convert.ToDateTime(textPeriodoAte.Text), divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte,
                            clienteDe, clienteAte, jobDe, jobAte, terDe, terAte, ref tb, empresas);

                        if(checkDuasMoedas.Checked)
                        {
                            erros = relatorio.razaoContabil(0, contaDe, contaAte, Convert.ToDateTime(textPeriodoDe.Text),
                                Convert.ToDateTime(textPeriodoAte.Text), divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte,
                                clienteDe, clienteAte, jobDe, jobAte, terDe, terAte, ref tb2, empresas);

                            Decimal valor_moeda = 0;

                            if (tb2.Rows.Count == 0)
                            {
                                if (tb.Rows.Count == 0)
                                {
                                    tb = null;
                                }
                                else
                                {
                                    var newDt = from a in tb.AsEnumerable().DefaultIfEmpty()
                                                 select new
                                                 {
                                                     cod_conta = a.Field<string>("cod_conta"),
                                                     lote_baixa = a.Field<string>("lote_baixa"),
                                                     numero_documento = a.Field<string>("numero_documento"),
                                                     deb_cred = a.Field<string>("deb_cred"),
                                                     descricao = a.Field<string>("descricao"),
                                                     data = a.Field<DateTime>("data"),
                                                     historico = a.Field<string>("historico"),
                                                     nome_razao_social = a.Field<string>("nome_razao_social"),
                                                     divisao = a.Field<string>("divisao"),
                                                     job = a.Field<string>("job"),
                                                     valor = 0,
                                                     valor_moeda = a.Field<Decimal>("valor"),
                                                     terceiro = a.Field<string>("terceiro"),
                                                     modulo = a.Field<string>("modulo")
                                                 };

                                    tb = newDt.CopyToDataTable();
                                }
                            }
                            else if (tb.Rows.Count == 0)
                            {
                                var newDT = from a in tb2.AsEnumerable().DefaultIfEmpty()
                                             select new
                                             {
                                                 cod_conta = a.Field<string>("cod_conta"),
                                                 lote_baixa = a.Field<string>("lote_baixa"),
                                                 numero_documento = a.Field<string>("numero_documento"),
                                                 deb_cred = a.Field<string>("deb_cred"),
                                                 descricao = a.Field<string>("descricao"),
                                                 data = a.Field<DateTime>("data"),
                                                 historico = a.Field<string>("historico"),
                                                 nome_razao_social = a.Field<string>("nome_razao_social"),
                                                 divisao = a.Field<string>("divisao"),
                                                 job = a.Field<string>("job"),
                                                 valor = a.Field<Decimal>("valor"),
                                                 valor_moeda = 0,
                                                 terceiro = a.Field<string>("terceiro"),
                                                 modulo = a.Field<string>("modulo")
                                             };

                                tb = newDT.CopyToDataTable();
                            }
                            else
                            {
                                var moedaPadrao = tb2.AsEnumerable();
                                var moedaSelecionada = tb.AsEnumerable();

                                var novoDtLeft = from a in moedaPadrao.DefaultIfEmpty()
                                                 join c in moedaSelecionada.DefaultIfEmpty()
                                                 on new
                                                 {
                                                     cod_conta = a == null ? "" : a.Field<string>("cod_conta"),
                                                     lote = a == null ? 0 : a.Field<Decimal>("lote"),
                                                     seq_lote = a == null ? 0 : a.Field<Decimal>("seq_lote")
                                                 }
                                                 equals new
                                                 {
                                                     cod_conta = c == null ? "" : c.Field<string>("cod_conta"),
                                                     lote = c == null ? 0 : c.Field<Decimal>("lote"),
                                                     seq_lote = c == null ? 0 : c.Field<Decimal>("seq_lote")
                                                 }
                                                 into dtTemp
                                                 from c in dtTemp.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     cod_conta = a == null ? c.Field<string>("cod_conta") : a.Field<string>("cod_conta"),
                                                     lote_baixa = a == null ? c.Field<string>("lote_baixa") : a.Field<string>("lote_baixa"),
                                                     numero_documento = a == null ? c.Field<string>("numero_documento") : a.Field<string>("numero_documento"),
                                                     deb_cred = a == null ? c.Field<string>("deb_cred") : a.Field<string>("deb_cred"),
                                                     descricao = a == null ? c.Field<string>("descricao") : a.Field<string>("descricao"),
                                                     data = a == null ? c.Field<DateTime>("data") : a.Field<DateTime>("data"),
                                                     historico = a == null ? c.Field<string>("historico") : a.Field<string>("historico"),
                                                     nome_razao_social = a == null ? c.Field<string>("nome_razao_social") : a.Field<string>("nome_razao_social"),
                                                     divisao = a == null ? c.Field<string>("divisao") : a.Field<string>("divisao"),
                                                     job = a == null ? c.Field<string>("job") : a.Field<string>("job"),
                                                     valor = a == null ? c.Field<Decimal>("valor") : a.Field<Decimal>("valor"),
                                                     valor_moeda = c == null ? 0 : c.Field<Decimal>("valor"),
                                                     terceiro = a == null ? c.Field<string>("terceiro") : a.Field<string>("terceiro"),
                                                     modulo = a == null ? c.Field<string>("modulo") : a.Field<string>("modulo")
                                                 };

                                var novoDtRight = from a in moedaSelecionada.DefaultIfEmpty()
                                                  join c in moedaPadrao.DefaultIfEmpty()
                                                  on new
                                                  {
                                                      cod_conta = a.Field<string>("cod_conta"),
                                                      lote = a.Field<Decimal>("lote"),
                                                      seq_lote = a.Field<Decimal>("seq_lote")
                                                  }
                                                  equals new
                                                  {
                                                      cod_conta = c.Field<string>("cod_conta"),
                                                      lote = c.Field<Decimal>("lote"),
                                                      seq_lote = c.Field<Decimal>("seq_lote")
                                                  }
                                                  into dtTemp
                                                  from c in dtTemp.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      cod_conta = a == null ? c.Field<string>("cod_conta") : a.Field<string>("cod_conta"),
                                                      lote_baixa = a == null ? c.Field<string>("lote_baixa") : a.Field<string>("lote_baixa"),
                                                      numero_documento = a == null ? c.Field<string>("numero_documento") : a.Field<string>("numero_documento"),
                                                      deb_cred = a == null ? c.Field<string>("deb_cred") : a.Field<string>("deb_cred"),
                                                      descricao = a == null ? c.Field<string>("descricao") : a.Field<string>("descricao"),
                                                      data = a == null ? c.Field<DateTime>("data") : a.Field<DateTime>("data"),
                                                      historico = a == null ? c.Field<string>("historico") : a.Field<string>("historico"),
                                                      nome_razao_social = a == null ? c.Field<string>("nome_razao_social") : a.Field<string>("nome_razao_social"),
                                                      divisao = a == null ? c.Field<string>("divisao") : a.Field<string>("divisao"),
                                                      job = a == null ? c.Field<string>("job") : a.Field<string>("job"),
                                                      valor = c == null ? 0 : c.Field<Decimal>("valor"),
                                                      valor_moeda = a == null ? c.Field<Decimal>("valor") : a.Field<Decimal>("valor"),
                                                      terceiro = a == null ? c.Field<string>("terceiro") : a.Field<string>("terceiro"),
                                                      modulo = a == null ? c.Field<string>("modulo") : a.Field<string>("modulo")
                                                  };

                                DataTable tb3 = novoDtLeft.Union(novoDtRight).CopyToDataTable();
                                tb3.DefaultView.Sort = "cod_conta, data, lote_baixa";
                                tb = tb3.DefaultView.ToTable();
                                
                                //tb = novoDtLeft.Union(novoDtRight).CopyToDataTable();
                                //tb = novoDtLeft.Union(novoDtRight).OrderBy(r => (r.cod_conta + "-" + r.data.ToString("aaaaMMdd"))).CopyToDataTable();
                                //tb.DefaultView.Sort = "cod_conta, data, lote_baixa";
                                //tb = novoDtLeft.Union(novoDtRight).OrderBy(r => r.cod_conta).CopyToDataTable();
                            }   
                        }

                        if (erros.Count == 0)
                        {
                            Title += " - Razão Contábil";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Razão Contábil";
                            empresasDAO empr = new empresasDAO(_conn);
                            Master.literalNomeEmpresa.Text = empr.retornanomesempresas(empresas);
                            string detalhes = "";
                            detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

                            if (comboContaDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
                                if (comboContaAte.SelectedValue != "0")
                                    detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboDivisaoDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
                                if (comboDivisaoAte.SelectedValue != "0")
                                    detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboLinhaNegocioDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
                                if (comboLinhaNegocioAte.SelectedValue != "0")
                                    detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboClienteDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
                                if (comboClienteAte.SelectedValue != "0")
                                    detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            if (comboJobDe.SelectedValue != "0")
                            {
                                detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
                                if (comboJobAte.SelectedValue != "0")
                                    detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
                                else
                                    detalhes += "</p>";
                            }

                            Master.literalDetalhes.Text = detalhes;

                            DateTime inicio = Convert.ToDateTime(textPeriodoDe.Text);
                            DateTime termino = Convert.ToDateTime(textPeriodoAte.Text);


                            StringBuilder textFast = new StringBuilder();

                            textFast.Append("<table cellpadding=\"0\" id=\"razao\" cellspacing=\"1\">\n");
                            textFast.Append("<tr class=\"titulo\">\n");
                            textFast.Append("<td>Data</td>\n");
                            textFast.Append("<td>Lote/Baixa</td>\n");
                            textFast.Append("<td>Nº Doc.</td>\n");
                            textFast.Append("<td>Histórico</td>\n");
                            textFast.Append("<td>Divisão</td>\n");
                            textFast.Append("<td>Job</td>\n");
                            textFast.Append("<td>Cliente</td>\n");
                            textFast.Append("<td>Terceiro</td>\n");
                            textFast.Append("<td class=\"valores\">Débito</td>\n");
                            textFast.Append("<td class=\"valores\">Crédito</td>\n");
                            textFast.Append("<td class=\"valores\">Saldo</td>\n");
                            if (checkDuasMoedas.Checked) 
                            {
                                textFast.Append("<td class=\"valores\">Fator</td>\n");
                                textFast.Append("<td class=\"valores\">Débito</td>\n");
                                textFast.Append("<td class=\"valores\">Crédito</td>\n");
                                textFast.Append("<td class=\"valores\">Saldo</td>\n");
                                textFast.Append("</tr>\n");
                                textFast.Append("<tr class=\"titulo\">\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td></td>\n");
                                textFast.Append("<td class=\"valores\">Conversão</td>\n");
                                textFast.Append("<td class=\"valores moeda\">Moeda</td>\n");
                                textFast.Append("<td class=\"valores moeda\">Moeda</td>\n");
                                textFast.Append("<td class=\"valores moeda\">Moeda</td>\n");
                            }
                            textFast.Append("</tr>\n");

                            string codContaAnt = "";
                            string dataAnt = "";
                            string loteAnt = "";
                            bool primeiraConta = true;
                            bool foiSaldoInicial = false;
                            DataRow row = null;
                            DataRow rAnt = null;
                            decimal valor = 0;
                            decimal valorMoeda = 0;
                            decimal totalData = 0;
                            decimal totalDataMoeda = 0;
                            decimal totalLote = 0;
                            decimal totalLoteMoeda = 0;
                            DataRow r = null;
                            decimal valorSaldoInicial = 0;
                            decimal valorSaldoInicialMoeda = 0;
                            decimal valorOriginal = 0;
                            decimal valorOriginalMoeda = 0;
                            DateTime dataLancamento = new DateTime();
                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                r = tb.Rows[i];
                                valorOriginal = Convert.ToDecimal(r["valor"]);
                                if(checkDuasMoedas.Checked)
                                    valorOriginalMoeda = Convert.ToDecimal(r["valor_moeda"]);
                                dataLancamento = Convert.ToDateTime(r["data"]);

                                if (codContaAnt != r["cod_conta"].ToString())
                                {
                                    textFast.Append(finalizaLote(primeiraConta, rAnt, totalLote, totalLoteMoeda, checkDuasMoedas.Checked));
                                    textFast.Append(finalizaData(primeiraConta, rAnt, valor, valorMoeda, checkDuasMoedas.Checked,0));
                                
                                    if (!primeiraConta)
                                    {
                                        textFast.Append("<tr class=\"fim conta nivel0\">\n");
                                        textFast.Append("<td colspan=\"8\">Saldo Final da Conta : " + rAnt["cod_conta"].ToString() + " - " + rAnt["descricao"] + "</td>\n");

                                        if (valor > 0)
                                        {
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                            textFast.Append("<td class=\"valores\">-</td>");
                                        }
                                        else
                                        {
                                            textFast.Append("<td class=\"valores\">-</td>");
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                        }
                                        textFast.Append("<td class=\"valores\">-</td>");

                                        if (checkDuasMoedas.Checked)
                                        {
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00000}", (valor == 0 || valorMoeda == 0 ? 0 : (valor / valorMoeda))) + "</td>");
                                            if (valorMoeda > 0)
                                            {
                                                textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda) + "</td>\n");
                                                textFast.Append("<td class=\"valores\">-</td>");
                                            }
                                            else
                                            {
                                                textFast.Append("<td class=\"valores\">-</td>");
                                                textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda) + "</td>\n");
                                            }
                                            textFast.Append("<td class=\"valores\">-</td>");
                                        }
                                        textFast.Append("</tr>\n");

                                    }

                                    valor = 0;
                                    valorMoeda = 0;
                                    codContaAnt = r["cod_conta"].ToString();

                                    if (loteAnt != r["lote_baixa"].ToString())
                                    {
                                        totalLote = 0;
                                        totalLoteMoeda = 0;
                                    }
                                    if (dataAnt != r["data"].ToString())
                                    {
                                        //totalData = 0;
                                    }
                                    
                                    dataAnt = r["data"].ToString();
                                    loteAnt = r["lote_baixa"].ToString();
                                    

                                    textFast.Append("<tr class=\"inicio nivel0\">\n");
                                    textFast.Append("<td colspan=\"8\">Saldo Inicial da Conta: " + r["cod_conta"].ToString() + " - " + r["descricao"] + " </td>\n");
                                    
                                    valorSaldoInicial = 0;
                                    valorSaldoInicialMoeda = 0;

                                    //if (inicio <= dataLancamento || r["nome_razao_social"].ToString() != "Saldo Inicial")
                                    if (inicio <= dataLancamento)
                                    {
                                        valorSaldoInicial = 0;
                                    }
                                    else
                                    {
                                        valorSaldoInicial = valorOriginal;
                                    }
                                    if (primeiraConta)
                                    {
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                    }
                                    else
                                    {
                                        if (valorOriginal > 0)
                                        {
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                            textFast.Append("<td class=\"valores\">-</td>");
                                        }
                                        else
                                        {
                                            textFast.Append("<td class=\"valores\">-</td>");
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                        }
                                    }
                                    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>");

                                    if (checkDuasMoedas.Checked)
                                    {
                                        
                                        if (inicio < dataLancamento)
                                        {
                                            valorSaldoInicialMoeda = 0;
                                        }
                                        else
                                        {
                                            valorSaldoInicialMoeda = valorOriginalMoeda;
                                        }

                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00000}", (valorSaldoInicial == 0 || valorSaldoInicialMoeda == 0 ? 0 : (valorSaldoInicial / valorSaldoInicialMoeda))) + "</td>");

                                        if (primeiraConta)
                                        {
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicialMoeda) + "</td>\n");
                                            textFast.Append("<td class=\"valores\">-</td>");
                                        }
                                        else
                                        {
                                            if (valorOriginalMoeda > 0)
                                            {
                                                textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicialMoeda) + "</td>\n");
                                                textFast.Append("<td class=\"valores\">-</td>");
                                            }
                                            else
                                            {
                                                textFast.Append("<td class=\"valores\">-</td>");
                                                textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicialMoeda) + "</td>\n");
                                            }
                                        }
                                        textFast.Append("<td class=\"valores\">-</td>");
                                    }

                                    textFast.Append("</tr>\n");
                                    primeiraConta = false;
                                }

                                if (r["nome_razao_social"].ToString() != "Saldo Inicial")
                                {
                                    if (!foiSaldoInicial)
                                    {
                                        if (dataAnt != r["data"].ToString())
                                        {
                                            textFast.Append(finalizaLote(primeiraConta, rAnt, totalLote, totalLoteMoeda, checkDuasMoedas.Checked));
                                            loteAnt = r["lote_baixa"].ToString();
                                            totalLote = 0;
                                            totalLoteMoeda = 0;

                                            textFast.Append(finalizaData(primeiraConta, rAnt, valor, valorMoeda, checkDuasMoedas.Checked, 0));
                                            dataAnt = r["data"].ToString();
                                            totalData = 0;
                                            totalLoteMoeda = 0;
                                        }

                                        if (loteAnt != r["lote_baixa"].ToString())
                                        {
                                            textFast.Append(finalizaLote(primeiraConta, rAnt, totalLote, totalLoteMoeda, checkDuasMoedas.Checked));
                                            loteAnt = r["lote_baixa"].ToString();
                                            totalLote = 0;
                                            totalLoteMoeda = 0;
                                        }
                                    }

                                    textFast.Append("<tr class=\"linha nivel0\">\n");
                                    textFast.Append("<td>" + dataLancamento.ToString("dd/MM/yyyy") + "</td>\n");
                                    if (r["lote_baixa"].ToString().Contains("B"))
                                    {
                                        textFast.Append("<td><a href='FormDetalheBaixa.aspx?id=" + r["lote_baixa"].ToString().Substring(1, r["lote_baixa"].ToString().Length - 1) + "' target='_blank'>" + r["lote_baixa"] + "</a></td>\n");
                                    }else
                                        textFast.Append("<td><a href='FormGenericTitulos.aspx?modulo="+r["modulo"]+"&lote="+r["lote_baixa"].ToString().Substring(1,r["lote_baixa"].ToString().Length -1)+"' target='_blank'>" + r["lote_baixa"] + "</a></td>\n");
                                    textFast.Append("<td>" + r["numero_documento"] + "</td>\n");
                                    textFast.Append("<td>" + r["historico"] + "</td>\n");
                                    textFast.Append("<td>" + r["divisao"] + "</td>\n");
                                    textFast.Append("<td>" + r["job"] + "</td>\n");
                                    textFast.Append("<td>" + r["nome_razao_social"] + "</td>\n");
                                    textFast.Append("<td>" + r["terceiro"] + "</td>\n");

                                    if (valorOriginal > 0)
                                    {
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorOriginal) + "</td>\n");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                    }
                                    else
                                    {
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorOriginal) + "</td>\n");
                                    }
                                    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor + valorOriginal) + "</td>");

                                    if (checkDuasMoedas.Checked)
                                    {
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00000}", (valorOriginal == 0 || valorOriginalMoeda == 0 ? 0 : (valorOriginal / valorOriginalMoeda))) + "</td>");
                                        if (valorOriginalMoeda > 0)
                                        {
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorOriginalMoeda) + "</td>\n");
                                            textFast.Append("<td class=\"valores\">-</td>");
                                        }
                                        else
                                        {
                                            textFast.Append("<td class=\"valores\">-</td>");
                                            textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorOriginalMoeda) + "</td>\n");
                                        }
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda + valorOriginalMoeda) + "</td>");
                                    }
                                    textFast.Append("</tr>\n");

                                    totalLote += valorOriginal;
                                    totalLoteMoeda += valorOriginalMoeda;
                                    foiSaldoInicial = false;
                                }
                                else
                                {
                                    foiSaldoInicial = true;
                                }
                                valor += valorOriginal;
                                totalData += valorOriginal;

                                valorMoeda += valorOriginalMoeda;
                                totalDataMoeda += valorOriginalMoeda;

                                loteAnt = r["lote_baixa"].ToString();
                                dataAnt = r["data"].ToString();
                                rAnt = r;
                                
                            }

                            if (rAnt != null)
                            {
                                textFast.Append("<tr class=\"fim conta nivel0\">\n");
                                textFast.Append("<td colspan=\"8\">Saldo Final da Conta : " + rAnt["cod_conta"].ToString() + " - " + rAnt["descricao"] + "</td>\n");
                                if (valor > 0)
                                {
                                    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                    textFast.Append("<td class=\"valores\">-</td>");
                                }
                                else
                                {
                                    textFast.Append("<td class=\"valores\">-</td>");
                                    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                }
                                textFast.Append("<td class=\"valores\"></td>");

                                if(checkDuasMoedas.Checked)
                                {
                                    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", (valor == 0 || valorMoeda == 0 ? 0 : (valor / valorMoeda))) + "</td>");
                                    if (valor > 0)
                                    {
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda) + "</td>\n");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                    }
                                    else
                                    {
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda) + "</td>\n");
                                    }
                                    textFast.Append("<td class=\"valores\"></td>");
                                }

                                textFast.Append("</tr>\n");
                            }

                            textFast.Append("</table>\n");

                            literalRelatorio.Text = textFast.ToString();
                        }
                        break;
                    #endregion
                }
            }
        }
        else
        {
            Response.Redirect("FormRelatorios.aspx");
        }
    }

    private void inicializaConta_MovBancaria(ref string html, ref decimal saldoGrupo, ref decimal saldoTotal, ref string contaAnterior, ref string grupoAnterior, DataRow linha) 
    {
        if(contaAnterior != string.Empty)
		{
            html += "<tr class=\"inicio nivel0\", style=\"height:15px\">";
            html += "<td colspan=\"9\" bgcolor=\"#AAAAAA\"></td>";
            html += "</tr>";
        }

        html += "<tr class=\"inicio nivel0\">";
        html += "<td colspan='9'> Conta: " + Convert.ToString(linha["cod_conta"] + " - " + linha["descricao"]) + "</td>";
        html += "</tr>";

        saldoTotal = Convert.ToDecimal(linha["entradas"]) > 0 ? Convert.ToDecimal(linha["entradas"]) : -Convert.ToDecimal(linha["saidas"]);
        contaAnterior = Convert.ToString(linha["cod_conta"]);
        grupoAnterior = String.Empty;

        html += "<tr class=\"linha nivel0\">";
        html += "<td></td>";
        html += "<td>" + Convert.ToDateTime(linha["Data"]).ToString("dd/MM/yyyy") + "</td>";
        html += "<td>" + Convert.ToString(linha["historico"]) + "</td>";
        html += "<td>" + Convert.ToString(linha["numero_documento"]) + "</td>";
        html += "<td class=\"valores\">" + Convert.ToString(linha["terceiro"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", linha["entradas"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", linha["saidas"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoTotal) + "</td>";
        html += "</tr>";
    }

    private void finalizaGrupo_MovBancaria(ref string html, ref string grupoAnterior, ref decimal saldoGrupo, decimal saldoTotal, DateTime? dataAnterior, bool multiPeriodo)
	{
        if (grupoAnterior != "")
        {
            html += "<tr class = \"subtotal nivel0\">";
            html += "<td colspan=2></td>";
            html += "<td colspan=3> Total de " + grupoAnterior + (multiPeriodo ? " - " + dataAnterior.Value.ToString("dd/MM/yyyy") : string.Empty) + "</td>";
            if (saldoGrupo > 0)
            {
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoGrupo) + "</td>";
                html += "<td></td>";
            }
            else
            {
                html += "<td></td>";
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", Math.Abs(saldoGrupo)) + "</td>";
            }

            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoTotal) + "</td>";
            html += "</tr>";

            grupoAnterior = string.Empty;
            dataAnterior = null;

        }
        saldoGrupo = 0;
    }

    private void geraLinha_MovBancaria(ref string html, ref decimal saldoGrupo, ref decimal saldoTotal, ref string grupoAnterior, DataRow linha, ref DateTime? dataAnterior, bool inicializaGrupo = false)
	{

        if (Convert.ToDecimal(linha["entradas"]) != 0)
        {
            saldoGrupo += Convert.ToDecimal(linha["entradas"]);
            saldoTotal += Convert.ToDecimal(linha["entradas"]);
        }
        else
        {
            saldoGrupo -= Convert.ToDecimal(linha["saidas"]);
            saldoTotal -= Convert.ToDecimal(linha["saidas"]);
        }

        html += "<tr class = \"linha nivel0\">";
        html += "<td>" + (inicializaGrupo ? Convert.ToString(linha["Grupo"]) : string.Empty) + "</td>";
        html += "<td>" + Convert.ToDateTime(linha["Data"]).ToString("dd/MM/yyyy") + "</td>";
        html += "<td>" + Convert.ToString(linha["historico"]) + "</td>";
        html += "<td>" + Convert.ToString(linha["numero_documento"]) + "</td>";
        html += "<td>" + Convert.ToString(linha["terceiro"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", linha["entradas"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", linha["saidas"]) + "</td>";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoTotal) + "</td>";
        html += "</tr>";

        if (inicializaGrupo)
            grupoAnterior = Convert.ToString(linha["grupo"]);
        
        dataAnterior = Convert.ToDateTime(linha["Data"]);
    }

    private void movimentacao_financeira_layout2(DataTable tabelaMovimentacao, int maxDetalhe)
    {
        TextBox textPeriodoDe = PreviousPage.txtPeriodoDe;
        TextBox textPeriodoAte = PreviousPage.txtPeriodoAte;
        DropDownList comboContaDe;
        DropDownList comboContaAte;
        DropDownList comboDivisaoDe;
        DropDownList comboDivisaoAte;
        DropDownList comboLinhaNegocioDe;
        DropDownList comboLinhaNegocioAte;
        DropDownList comboClienteDe;
        DropDownList comboClienteAte;
        DropDownList comboJobDe;
        DropDownList comboJobAte;

        DropDownList comboDetalhamento1;
        DropDownList comboDetalhamento2;
        DropDownList comboDetalhamento3;
        DropDownList comboDetalhamento4;

        string contaDe = null;
        string contaAte = null;
        Nullable<int> divisaoDe = null;
        Nullable<int> divisaoAte = null;
        Nullable<int> linhaNegocioDe = null;
        Nullable<int> linhaNegocioAte = null;
        Nullable<int> clienteDe = null;
        Nullable<int> clienteAte = null;
        Nullable<int> jobDe = null;
        Nullable<int> jobAte = null;

        List<string> erros = new List<string>();



        textPeriodoDe = PreviousPage.txtPeriodoDe;
        textPeriodoAte = PreviousPage.txtPeriodoAte;

        comboContaDe = PreviousPage.cmbContaDe;
        comboContaAte = PreviousPage.cmbContaAte;
        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
        comboClienteDe = PreviousPage.cmbClienteDe;
        comboClienteAte = PreviousPage.cmbClienteAte;
        comboJobDe = PreviousPage.cmbJobDe;
        comboJobAte = PreviousPage.cmbJobAte;

        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;

        if (comboContaDe != null)
        {
            if (comboContaDe.SelectedValue == "0")
                contaDe = null;
            else
                contaDe = comboContaDe.SelectedValue;
        }

        if (comboContaAte != null)
        {
            if (comboContaAte.SelectedValue == "0")
                contaAte = null;
            else
                contaAte = comboContaAte.SelectedValue;
        }

        if (comboDivisaoDe != null)
        {
            if (comboDivisaoDe.SelectedValue == "0")
                divisaoDe = null;
            else
                divisaoDe = Convert.ToInt32(comboDivisaoDe.SelectedValue);
        }

        if (comboDivisaoAte != null)
        {
            if (comboDivisaoAte.SelectedValue == "0")
                divisaoAte = null;
            else
                divisaoAte = Convert.ToInt32(comboDivisaoAte.SelectedValue);
        }

        if (comboLinhaNegocioDe != null)
        {
            if (comboLinhaNegocioDe.SelectedValue == "0")
                linhaNegocioDe = null;
            else
                linhaNegocioDe = Convert.ToInt32(comboLinhaNegocioDe.SelectedValue);
        }

        if (comboLinhaNegocioAte != null)
        {
            if (comboLinhaNegocioAte.SelectedValue == "0")
                linhaNegocioAte = null;
            else
                linhaNegocioAte = Convert.ToInt32(comboLinhaNegocioAte.SelectedValue);
        }

        if (comboClienteDe != null)
        {
            if (comboClienteDe.SelectedValue == "0")
                clienteDe = null;
            else
                clienteDe = Convert.ToInt32(comboClienteDe.SelectedValue);
        }

        if (comboClienteAte != null)
        {
            if (comboClienteAte.SelectedValue == "0")
                clienteAte = null;
            else
                clienteAte = Convert.ToInt32(comboClienteAte.SelectedValue);
        }

        if (comboJobDe != null)
        {
            if (comboJobDe.SelectedValue == "0")
                jobDe = null;
            else
                jobDe = Convert.ToInt32(comboJobDe.SelectedValue);
        }

        if (comboJobAte != null)
        {
            if (comboJobAte.SelectedValue == "0")
                jobAte = null;
            else
                jobAte = Convert.ToInt32(comboJobAte.SelectedValue);
        }

        Title += " - Movimento Financeiro";
        empresa.codigo = Convert.ToInt32(Session["empresa"]);
        empresa.load();
        Master.literalNomeRelatorio.Text = "Movimentação Financeira";
        Master.literalNomeEmpresa.Text = empresa.nome;
        string detalhes = "";
        detalhes += "<p>Período: " + textPeriodoDe.Text + " até " + textPeriodoAte.Text + "</p>";

        if (comboContaDe.SelectedValue != "0")
        {
            detalhes += "<p>Conta: " + comboContaDe.SelectedItem.Text + "";
            if (comboContaAte.SelectedValue != "0")
                detalhes += " até " + comboContaAte.SelectedItem.Text + "</p>";
            else
                detalhes += "</p>";
        }

        if (comboDivisaoDe.SelectedValue != "0")
        {
            detalhes += "<p>Divisão: " + comboDivisaoDe.SelectedItem.Text + "";
            if (comboDivisaoAte.SelectedValue != "0")
                detalhes += " até " + comboDivisaoAte.SelectedItem.Text + "</p>";
            else
                detalhes += "</p>";
        }

        if (comboLinhaNegocioDe.SelectedValue != "0")
        {
            detalhes += "<p>Linha de Negócio: " + comboLinhaNegocioDe.SelectedItem.Text + "";
            if (comboLinhaNegocioAte.SelectedValue != "0")
                detalhes += " até " + comboLinhaNegocioAte.SelectedItem.Text + "</p>";
            else
                detalhes += "</p>";
        }

        if (comboClienteDe.SelectedValue != "0")
        {
            detalhes += "<p>Cliente: " + comboClienteDe.SelectedItem.Text + "";
            if (comboClienteAte.SelectedValue != "0")
                detalhes += " até " + comboClienteAte.SelectedItem.Text + "</p>";
            else
                detalhes += "</p>";
        }

        if (comboJobDe.SelectedValue != "0")
        {
            detalhes += "<p>Job: " + comboJobDe.SelectedItem.Text + "";
            if (comboJobAte.SelectedValue != "0")
                detalhes += " até " + comboJobAte.SelectedItem.Text + "</p>";
            else
                detalhes += "</p>";
        }

        Master.literalDetalhes.Text = detalhes;

        string html = "<table cellpadding=\"0\" id=\"mov_financeira\" cellspacing=\"0\">\n";
        html += "<thead class=\"titulo\">\n";
        html += "<tr>\n";
        html += "<th>Data</th>\n";
        html += "<th>Terceiro</th>\n";
        html += "<th>Job</th>\n";
        html += "<th>Histórico</th>\n";
        html += "<th class=\"valores\">Entrada</th>\n";
        html += "<th class=\"valores\">Saída</th>\n";
        html += "<th class=\"valores\">Líquido</th>\n";
        html += "</tr>\n";
        html += "</thead>\n";

        int nivelAtual = 0;
        //int codigoNivelAtual = 0;

        string contaAtual = null;
        string descContaAtual = "";

        string ultimaData = null;

        string terceiroAtual = null;

        string tipoContaAtual = "";

        decimal saldoInicialAtual = 0;
        decimal saldoInicialTotal = 0;

        decimal saldoEntradaAtual = 0;
        decimal saldoSaidaAtual = 0;

        decimal saldoMovimentacaoAtual = 0;
        decimal saldoMovimentacaoTotal = 0;

        decimal saldoTipoContaAtual = 0;
        decimal saldoTerceiroAtual = 0;

        int[] listaNiveis = new int[maxDetalhe + 1];

        foreach(DataRow linha in tabelaMovimentacao.Rows)
        {
            //Verifica Conta
            if (!linha["cod_conta"].Equals(contaAtual))
            {
                //Finaliza Saldo Conta
                if(contaAtual != null)
                {
                    if(nivelAtual != 0 || listaNiveis[0] != 0)
                    {
                        saldoMovimentacaoAtual += saldoTerceiroAtual;

                        finalizaTerceiroMovimentacaoFinanceira(ref html, saldoTerceiroAtual);
                        finalizaTipoContaMovimentacaoFinanceira(ref html, ultimaData, tipoContaAtual, saldoTipoContaAtual);
                    }

                    finalizaContaMovimentacaoFinanceira(ref html, ultimaData, descContaAtual, saldoInicialAtual + saldoMovimentacaoAtual, saldoEntradaAtual, saldoSaidaAtual);
                }

                //Inicia Conta
                nivelAtual = 0;
                listaNiveis = new int[maxDetalhe + 1];
                listaNiveis[0] = Convert.ToInt32(linha["cod_job"]);

                saldoInicialAtual = listaNiveis[0] != 0 ? 0 : Convert.ToDecimal(linha["valor"]);
                saldoInicialTotal += saldoInicialAtual;

                saldoMovimentacaoTotal += saldoMovimentacaoAtual;

                iniciaContaMovimentacaoFinanceira(ref html, (listaNiveis[0] != 0 ? Convert.ToDateTime(textPeriodoDe.Text) : Convert.ToDateTime(linha["data"])).ToString("dd/MM/yyyy"), Convert.ToString(linha["descricao"]), saldoInicialAtual);

                //Reseta Valores
                saldoMovimentacaoAtual = 0;
                saldoTerceiroAtual = 0;
                saldoTipoContaAtual = 0;
                saldoSaidaAtual = 0;
                saldoEntradaAtual = 0;

                contaAtual = Convert.ToString(linha["cod_conta"]);
                descContaAtual = Convert.ToString(linha["descricao"]);
               
                if (listaNiveis[0] == 0)
                {
                    tipoContaAtual = null;
                    terceiroAtual = null;
                    continue;
                }
                else
                {
                    tipoContaAtual = Convert.ToString(linha["tipo_conta"]);
                    terceiroAtual = Convert.ToString(linha["nome_razao_social"]);
                    //iniciaTipoContaMovimentacaoFinanceira(ref html, Convert.ToDateTime(linha["data"]).ToString("dd/MM/yyyy"), Convert.ToString(linha["tipo_conta"]));
                }

            }
            else if (!linha["tipo_conta"].Equals(tipoContaAtual))
            {
                //Finaliza Saldo Tipo
                if (tipoContaAtual != null)
                {
                    finalizaTerceiroMovimentacaoFinanceira(ref html, saldoTerceiroAtual);
                    finalizaTipoContaMovimentacaoFinanceira(ref html, ultimaData, tipoContaAtual, saldoTipoContaAtual);
                }

                //iniciaTipoContaMovimentacaoFinanceira(ref html, Convert.ToDateTime(linha["data"]).ToString("dd/MM/yyyy"), Convert.ToString(linha["tipo_conta"]));

                saldoMovimentacaoAtual += saldoTerceiroAtual;
                saldoTerceiroAtual = 0;
                saldoTipoContaAtual = 0;

                nivelAtual = 0;
                listaNiveis = new int[maxDetalhe + 1];
                listaNiveis[0] = Convert.ToInt32(linha["cod_job"]);

                tipoContaAtual = Convert.ToString(linha["tipo_conta"]);
                terceiroAtual = Convert.ToString(linha["nome_razao_social"]);

            }
            //Verifica Terceiro
            else if (!linha["nome_razao_social"].Equals(terceiroAtual))
            {
                //Finaliza Saldo Terceiro
                if(terceiroAtual != null)
                    finalizaTerceiroMovimentacaoFinanceira(ref html, saldoTerceiroAtual);

                //Inicia Terceiro
                saldoMovimentacaoAtual += saldoTerceiroAtual;

                //Reseta Valores
                saldoTerceiroAtual = 0;

                nivelAtual = 0;
                listaNiveis = new int[maxDetalhe + 1];
                listaNiveis[0] = Convert.ToInt32(linha["cod_job"]);

                terceiroAtual = Convert.ToString(linha["nome_razao_social"]);
            }
            //Verifica Nível
            //else if(nivelAtual == 0 || codigoNivelAtual == Convert.ToInt32(linha["codDetalhe" + nivelAtual]))
            //{
            //    if(nivelAtual < maxDetalhe && Convert.ToInt32(linha["codDetalhe" + (nivelAtual + 1)]) != 0)
            //        codigoNivelAtual = Convert.ToInt32(linha["codDetalhe" + (++nivelAtual)]);
            //}
            //else
            //{
            //    nivelAtual = 0;
            //    codigoNivelAtual = Convert.ToInt32(linha["cod_job"]);
            //}
            else
            {
                bool retrocedendoNiveis = false;
                while (nivelAtual <= maxDetalhe)
                {
                    if (nivelAtual == 0)
                    {
                        listaNiveis[0] = Convert.ToInt32(linha["cod_job"]);
                        if (retrocedendoNiveis) break;

                        nivelAtual++;
                    }
                    else if (Convert.ToInt32(linha["codDetalhe" + nivelAtual]) == 0)
                    {
                        listaNiveis[nivelAtual] = 0;
                        nivelAtual--;

                        retrocedendoNiveis = true;
                    }
                    else if (Convert.ToInt32(linha["codDetalhe" + nivelAtual]) == listaNiveis[nivelAtual])
                    {
                        if (retrocedendoNiveis) break;
                        nivelAtual++;
                    }
                    else
                    {
                        listaNiveis[nivelAtual] = Convert.ToInt32(linha["codDetalhe" + nivelAtual]);
                        break;
                    }
                }

                if (nivelAtual > maxDetalhe)
                    nivelAtual = maxDetalhe;
            }

            //Gera Linha
            ultimaData = Convert.ToDateTime(linha["data"]).ToString("dd/MM/yyyy");
            string descricaoJob = Convert.ToString(linha[nivelAtual == 0 ? "descr" : "descDetalhe" + nivelAtual]);
            string descricaoTerceiro = string.Empty;
            string historico = Convert.ToString(linha["historico"]);
            decimal valor = Convert.ToDecimal(linha["valor"]);
            int numTitulo = Convert.ToInt32(linha["lote_pai"]);
            string modulo = Convert.ToString(linha["modulo_pai"]);
            if (nivelAtual == 0)
            {
                if (valor > 0) saldoEntradaAtual += valor;
                else saldoSaidaAtual += Math.Abs(valor);

                descricaoTerceiro = Convert.ToString(linha["nome_razao_social"]);
                saldoTerceiroAtual += valor;
                saldoTipoContaAtual += valor;
            }


            geraLinhaMovimentacaoFinanceira(ref html, nivelAtual, ultimaData, descricaoJob, descricaoTerceiro, historico, numTitulo, modulo, valor);
        }

        if(nivelAtual != 0 || listaNiveis[0] != 0)
        {
            finalizaTerceiroMovimentacaoFinanceira(ref html, saldoTerceiroAtual);
            finalizaTipoContaMovimentacaoFinanceira(ref html, ultimaData, tipoContaAtual, saldoTipoContaAtual);
        }


        saldoMovimentacaoAtual += saldoTerceiroAtual;
        saldoMovimentacaoTotal += saldoMovimentacaoAtual;

        finalizaContaMovimentacaoFinanceira(ref html, ultimaData, descContaAtual, saldoInicialAtual + saldoMovimentacaoAtual, saldoEntradaAtual, saldoSaidaAtual, true);

        //Finaliza Relatorio
        finalizaRelatorioMovimentacaoFinanceira(ref html, saldoInicialTotal, saldoMovimentacaoTotal);

        literalRelatorio.Text = html;
    }

    private void finalizaRelatorioMovimentacaoFinanceira(ref string html, decimal saldoInicialTotal, decimal saldoMovimentacaoTotal)
    {
        html += "<tr class=\"resumo\">\n";
        html += "<td colspan=\"3\"></td>\n";
        html += "<td>Saldo Inicial Total</td>\n";
        html += "<td colspan=\"2\"></td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoInicialTotal) + "</td>\n";
        html += "</tr>\n";

        html += "<tr class=\"resumo\">\n";
        html += "<td colspan=\"3\"></td>\n";
        html += "<td>Movimento Total</td>\n";
        html += "<td colspan=\"2\"></td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoMovimentacaoTotal) + "</td>\n";
        html += "</tr>\n";

        html += "<tr class=\"resumo\">\n";
        html += "<td colspan=\"3\"></td>\n";
        html += "<td>Saldo Final Total</td>\n";
        html += "<td colspan=\"2\"></td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoInicialTotal + saldoMovimentacaoTotal) + "</td>\n";
        html += "</tr>\n";
        html += "</table>\n";
    }
    private void finalizaTerceiroMovimentacaoFinanceira(ref string html, decimal saldoTerceiroAtual)
    {
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoTerceiroAtual) + "</td>\n";
        html += "</tr>";
    }

    private void iniciaContaMovimentacaoFinanceira(ref string html, string data, string descConta, decimal saldoInicial)
    {
        html += "<tr class=\"inicio nivel0\">\n";
        html += "<td colspan=\"6\">" + data + " - " + descConta + " (Saldo Inicial)</td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoInicial) + "</td>\n";
        html += "</tr>\n";
    }
    private void finalizaContaMovimentacaoFinanceira(ref string html, string ultimaData, string descConta, decimal saldoConta, decimal saldoEntradaAtual, decimal saldoSaidaAtual, bool final = false)
    {
        html += "<tr class=\"fim nivel0\">\n";
        html += "<td colspan=\"4\">" + ultimaData + " - " + descConta + " (Saldo Final) </td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoEntradaAtual) + "</td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoSaidaAtual) + "</td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoConta) + "</td>\n";
        html += "</tr>\n";
        html += "<tr class=\"inicio nivel0\" style='height:15px;'>";
        html += "<td colspan=\"7\"" + (final ? "" : "bgcolor=\"#AAAAAA\"") + "></td>";
        html += "</tr>\n";
    }
    
    private void iniciaTipoContaMovimentacaoFinanceira(ref string html, string data, string tipoConta)
    {
        html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
        html += "<td></td>\n";
        html += "<td colspan=\"6\">" + tipoConta + " - " + data + " </td></tr>\n";
    }

    private void finalizaTipoContaMovimentacaoFinanceira(ref string html, string ultimaData, string tipoConta, decimal saldoTipoConta)
    {
        html += "<tr class=\"inicio nivel0\" bgcolor=\"#FFE7BA\" >\n";
        html += "<td></td>\n";
        html += "<td colspan=\"5\"> Subtotal de " + tipoConta + " </td>\n";
        html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", saldoTipoConta) + "</td></tr>\n";
    }

    private void geraLinhaMovimentacaoFinanceira(ref string html, int nivelAtual, string data, string descricaoJob, string descricaoTerceiro, string historico, int numTitulo, string modulo, decimal valor)
    {
        html += "<td class=\"valores\"></td>\n";

        html += "<tr class=\"linha nivel" + nivelAtual + "\">\n";
        html += "<td>" + data + "</td>\n";
        html += "<td>" + descricaoTerceiro + "</td>\n";
        html += "<td>" + descricaoJob.Replace(" ", "&nbsp;") + "</td>\n";
        html += "<td>" + historico + "</td>\n";

        string inicioLink = "";
        string terminoLink = "";
        if (numTitulo > 0)
        {
            inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + modulo + "&lote=" + numTitulo + "',900,600);\">";
            terminoLink = "</a>";
        }

        if (valor > 0)
        {

            html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", valor) + terminoLink + "</td>\n";
            html += "<td class=\"valores\"></td>\n";
        }
        else
        {
            html += "<td class=\"valores\"></td>\n";
            html += "<td class=\"valores\">" + inicioLink + String.Format("{0:0,0.00}", Math.Abs(valor)) + terminoLink + "</td>\n";
        }
    }

	private void verificaNivel(string relatorio, DataColumnCollection colunas, int nivel,
		DataRow row, string contaAtual, ref string html, ref decimal valor, ref decimal totalEntradaConta,
		ref decimal totalSaidaConta)
	{
		if (nivel <= 4)
		{
			if (tb.Columns.Contains("codDetalhe" + nivel + ""))
			{
				if (row["codDetalhe" + nivel + ""].ToString() != "0")
				{
					if (Convert.ToInt32(row["codDetalhe" + nivel + ""]) == detalheMovFinanc[nivel - 1])
					{
						verificaNivel(relatorio, colunas, nivel + 1, row, contaAtual, ref html, ref valor,
							ref totalEntradaConta, ref totalSaidaConta);

					}
					else
					{
						geraLinha(relatorio, nivel, row, ref html);
					}
				}
				else
				{
					geraLinha(relatorio, nivel - 1, row, ref html);
					if (nivel - 1 == 0)
					{
						valor += Convert.ToDecimal(row["valor"]);
						if (Convert.ToDecimal(row["valor"]) > 0)
							totalEntradaConta += Convert.ToDecimal(row["valor"]);
						else
							totalSaidaConta += Convert.ToDecimal(row["valor"]);
					}
				}

				detalheMovFinanc[nivel - 1] = Convert.ToInt32(row["codDetalhe" + nivel + ""]);
			}
			else
			{
				geraLinha(relatorio, nivel - 1, row, ref html);
				if (nivel - 1 == 0)
				{
					valor += Convert.ToDecimal(row["valor"]);
					if (Convert.ToDecimal(row["valor"]) > 0)
					{
						totalEntradaConta += Convert.ToDecimal(row["valor"]);

					}
					else
					{
						totalSaidaConta += Convert.ToDecimal(row["valor"]);
					}
				}
			}
		}
		else
		{
			geraLinha(relatorio, nivel, row, ref html);
		}
	}

	private void verificaNivelBeta(string relatorio, DataColumnCollection colunas, int nivel, 
        DataRow row, string contaAtual, ref string html, ref decimal valor, ref decimal totalEntradaConta, 
        ref decimal totalSaidaConta, ref decimal valorNivelAtual, ref int nivelAtual, int maxNivel, DataRow rowAnt)
    {

		if(nivel <= maxNivel)
		{
			//Verifica se CodDetalhe não é nulo
			if(row["codDetalhe" + nivel + ""].ToString() != "0")
			{
				//Verifica se CodDetalhe pertence a ordem atual
				if (Convert.ToInt32(row["codDetalhe" + nivel + ""]) == detalheMovFinanc[nivel - 1])
				{
					//Avança um nível para encontrar nivel Correto
                    verificaNivelBeta(relatorio, colunas, nivel + 1, row, contaAtual, ref html, ref valor, 
                        ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, rowAnt);
                    
                }
				//Se este for o seu nível
				else
				{
					
					//Se existe um nivelAtual
					if(nivelAtual != -1)
					{
						//Se o nível é o nivelAtual
						if (nivel == nivelAtual)
						{
							//Salve valores de row
							valorNivelAtual += Convert.ToDecimal(row["valor"]);
							//valor += Convert.ToDecimal(row["valor"]);
							return;
						}
						//Se não, finalize o nivel Atual
						else
						{
							rowAnt["valor"] = valorNivelAtual;
							geraLinhaBeta(relatorio, nivelAtual, rowAnt, ref html, maxNivel);

							if (nivelAtual == 0)
							{
								valor += Convert.ToDecimal(rowAnt["valor"]);
								if (Convert.ToDecimal(rowAnt["valor"]) > 0)
								{
									totalEntradaConta += Convert.ToDecimal(rowAnt["valor"]);
								}
								else
								{
									totalSaidaConta += Convert.ToDecimal(rowAnt["valor"]);
								}
							}

							nivelAtual = -1;
							valorNivelAtual = 0;

							//Continue a processar a linhaAtual
							verificaNivelBeta(relatorio, colunas, nivel, row, contaAtual, ref html, ref valor,
							ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, null);
							return;
						}
					}
					//Se o nível é igual ao MaxNivel
					else if (nivel == maxNivel)
					{
						//Gera Linha
						geraLinhaBeta(relatorio, nivel, row, ref html, maxNivel);

					}
					//Se não, atribua como nivelAtual
					else
					{
						//Atribua a linha atual como o nivelAtual
						nivelAtual = nivel;
						valorNivelAtual += Convert.ToDecimal(row["valor"]);
						//valor += Convert.ToDecimal(row["valor"]);
					}

				}
				detalheMovFinanc[nivel - 1] = Convert.ToInt32(row["codDetalhe" + nivel + ""]);
			}
			//Se for nulo, então a linha ulprapassou seu nivel
			else
			{
				if(nivelAtual == -1)
				{
					//Atribua a linha atual como o nivelAtual
					nivelAtual = nivel - 1;
					valorNivelAtual += Convert.ToDecimal(row["valor"]);
				}
				
				else if (nivel - 1 == nivelAtual)
				{
					//Salve valores de row
					valorNivelAtual += Convert.ToDecimal(row["valor"]);
					//valor += Convert.ToDecimal(row["valor"]);

					
				}
				else
				{
					//Gera Linha para o nivel processado
					rowAnt["valor"] = valorNivelAtual;
					geraLinhaBeta(relatorio, nivelAtual, rowAnt, ref html, maxNivel);

					if (nivelAtual == 0)
					{
						valor += Convert.ToDecimal(rowAnt["valor"]);
						if (Convert.ToDecimal(rowAnt["valor"]) > 0)
						{
							totalEntradaConta += Convert.ToDecimal(rowAnt["valor"]);
						}
						else
						{
							totalSaidaConta += Convert.ToDecimal(rowAnt["valor"]);
						}
					}

					nivelAtual = -1;
					valorNivelAtual = 0;

					//Continue a processar a linhaAtual
					verificaNivelBeta(relatorio, colunas, nivel, row, contaAtual, ref html, ref valor,
					ref totalEntradaConta, ref totalSaidaConta, ref valorNivelAtual, ref nivelAtual, maxNivel, null);
				}
			}
			
		}
		//Se o maxNivel for ultrapassado
		else
		{
			//Se há nivelAtual, finalize o nivel
			if(nivelAtual != -1)
			{
				rowAnt["valor"] = valorNivelAtual;

				if (nivelAtual == 0)
				{
					valor += Convert.ToDecimal(rowAnt["valor"]);
					if (Convert.ToDecimal(rowAnt["valor"]) > 0)
					{
						totalEntradaConta += Convert.ToDecimal(rowAnt["valor"]);
					}
					else
					{
						totalSaidaConta += Convert.ToDecimal(rowAnt["valor"]);
					}
				}

				geraLinhaBeta(relatorio, nivelAtual, rowAnt, ref html, maxNivel);
				nivelAtual = -1;
				valorNivelAtual = 0;

			}
			//Processe a linha atual
			geraLinhaBeta(relatorio, nivel - 1, row, ref html, maxNivel);

		}

  //      if (nivel <= 4)
  //      {
  //          if (tb.Columns.Contains("codDetalhe" + nivel + ""))
  //          {
  //              if (row["codDetalhe" + nivel + ""].ToString() != "0")
  //              {
  //                  if (Convert.ToInt32(row["codDetalhe" + nivel + ""]) == detalheMovFinanc[nivel - 1])
  //                  {
  //                      verificaNivel(relatorio, colunas, nivel + 1, row, contaAtual, ref html, ref valor, 
  //                          ref totalEntradaConta, ref totalSaidaConta);
                        
  //                  }
  //                  else
  //                  {
  //                      geraLinha(relatorio, nivel, row, ref html);
  //                  }
  //              }
  //              else
  //              {
  //                  geraLinha(relatorio, nivel - 1, row, ref html);
  //                  if (nivel - 1 == 0)
  //                  {
  //                      valor += Convert.ToDecimal(row["valor"]);
  //                      if (Convert.ToDecimal(row["valor"]) > 0)
  //                          totalEntradaConta += Convert.ToDecimal(row["valor"]);
  //                      else
  //                          totalSaidaConta += Convert.ToDecimal(row["valor"]);
  //                  }

  //              }

  //              detalheMovFinanc[nivel-1] = Convert.ToInt32(row["codDetalhe" + nivel + ""]);
  //          }
  //          else
  //          {
  //              geraLinha(relatorio, nivel - 1, row, ref html);
  //              if (nivel - 1 == 0)
  //              {
  //                  valor += Convert.ToDecimal(row["valor"]);
  //                  if (Convert.ToDecimal(row["valor"]) > 0)
  //                  {
  //                      totalEntradaConta += Convert.ToDecimal(row["valor"]);
                        
  //                  }
  //                  else
  //                  {
  //                      totalSaidaConta += Convert.ToDecimal(row["valor"]);
  //                  }
  //              }
  //          }
  //      }
  //      else
  //      {
		//	geraLinha(relatorio, nivel, row, ref html);
		//	//geraLinha(relatorio, nivel-1, row, ref html);
		//}
    }
	private void geraLinha(string relatorio, int nivel, DataRow row, ref string html)
	{
		switch (relatorio)
		{
			case "MOV_FINANC":
				html += "<tr class=\"linha nivel" + nivel + "\">\n";
				html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
				html += "<td>" + row["nome_razao_social"] + "</td>\n";
				if (nivel == 0)
				{
					html += "<td>" + row["descr"] + "</td>\n";
				}
				else
				{
					html += "<td>" + row["descDetalhe" + nivel + ""].ToString().Replace(" ", "&nbsp;") + "</td>\n";
				}
				html += "<td>" + row["historico"] + "</td>\n";

				string inicioLink = "";
				string terminoLink = "";
				if (Convert.ToInt32(row["lote_pai"]) > 0)
				{
					inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
					terminoLink = "</a>";
				}

				if (Convert.ToDecimal(row["valor"]) > 0)
				{

					html += "<td class=\"entrada valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
					html += "<td class=\"saida valores\"></td>\n";
				}
				else
				{
					html += "<td class=\"entrada valores\"></td>\n";
					html += "<td class=\"saida valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
				}

				break;
		}
	}

	private void geraLinhaBeta(string relatorio, int nivel, DataRow row, ref string html, int maxNivel)
    {
        switch (relatorio)
        {
			#region Beta
			case "MOV_FINANC_BETA":
                html += "<tr class=\"linha nivel"+nivel+"\">\n";
                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                if (nivel == 0)
                {
					//html += "<td>" + row["descr"] + "</td>\n";
					html += "<td> </td>";
					html += "<td> </td>";
				}
				else if (nivel == maxNivel)
				{
					html += "<td>" + row["descDetalhe" + nivel + ""].ToString().Replace(" ", "&nbsp;") + "</td>\n";
					html += "<td>" + row["historico"] + "</td>\n";
				}
				else
                {
                    html += "<td>" + row["descDetalhe"+nivel+""].ToString().Replace(" ", "&nbsp;") + "</td>\n";
					html += "<td> </td>";
				}

				//html += "<td>" + row["historico"] + "</td>\n";

				string inicioLink = "";
                string terminoLink = "";
                if (Convert.ToInt32(row["lote_pai"]) > 0)
                {
					inicioLink = "<a href=\"javascript:popup('FormGenericTitulos.aspx?modulo=" + row["modulo_pai"] + "&lote=" + row["lote_pai"] + "',900,600);\">";
					terminoLink = "</a>";
                }

                if (Convert.ToDecimal(row["valor"]) > 0)
                {

                    html += "<td class=\"entrada valores\">"+ inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
                    html += "<td class=\"saida valores\"></td>\n";
                }
                else
                {
                    html += "<td class=\"entrada valores\"></td>\n";
                    html += "<td class=\"saida valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink + "</td>\n";
                }

                break;
				#endregion
		}
	}

    private string finalizaLote(bool primeiraConta, DataRow r, decimal totalLote, decimal totalLoteMoeda, bool duasMoedas)
    {
        string html = "";
        if (!primeiraConta)
        {
            html += "<tr class=\"fim lote nivel0\">\n";
            html += "<td colspan=\"8\">Saldo do Lote: " + r["lote_baixa"].ToString() + "</td>\n";
            if (totalLote > 0)
            {
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalLote) + "</td>\n";
                html += "<td class=\"valores\">-</td>";
            }
            else
            {
                html += "<td class=\"valores\">-</td>";
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalLote) + "</td>\n";
            }
            html += "<td class=\"valores\">-</td>";

            if (duasMoedas)
            {
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00000}", (totalLote == 0 || totalLoteMoeda == 0 ? 0 : (totalLote / totalLoteMoeda))) + "</td>";
                if (totalLote > 0)
                {
                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalLoteMoeda) + "</td>\n";
                    html += "<td class=\"valores\">-</td>";
                }
                else
                {
                    html += "<td class=\"valores\">-</td>";
                    html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalLoteMoeda) + "</td>\n";
                }
                html += "<td class=\"valores\">-</td>";
            }

            html += "</tr>\n";
        }
        return html;
    }

    private string finalizaData(bool primeiraConta, DataRow r, decimal totalData, decimal valorMoeda, bool duasMoedas, decimal totalDataMoeda)
    {
        string html = "";
        if (!primeiraConta)
        {
            html += "<tr class=\"fim data nivel0\">\n";
            html += "<td colspan=\"8\">Saldo da Data: " + Convert.ToDateTime(r["data"]).ToString("dd/MM/yyyy") + "</td>\n";
            html += "<td class=\"valores\">-</td>";
            html += "<td class=\"valores\">-</td>\n";
            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", totalData) + "</td>";
            if(duasMoedas)
            {
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00000}", (totalData == 0 || valorMoeda == 0 ? 0 : (totalData / valorMoeda))) + "</td>\n";
                html += "<td class=\"valores\">-</td>\n";
                html += "<td class=\"valores\">-</td>\n";
                html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", valorMoeda) + "</td>\n";
            }
            html += "</tr>\n";
        }
        return html;
    }
}

public class ObjectShredder<T>
{
    private System.Reflection.FieldInfo[] _fi;
    private System.Reflection.PropertyInfo[] _pi;
    private System.Collections.Generic.Dictionary<string, int> _ordinalMap;
    private System.Type _type;

    // ObjectShredder constructor. 
    public ObjectShredder()
    {
        _type = typeof(T);
        _fi = _type.GetFields();
        _pi = _type.GetProperties();
        _ordinalMap = new Dictionary<string, int>();
    }

    /// <summary> 
    /// Loads a DataTable from a sequence of objects. 
    /// </summary> 
    /// <param name="source">The sequence of objects to load into the DataTable.</param>
    /// <param name="table">The input table. The schema of the table must match that 
    /// the type T.  If the table is null, a new table is created with a schema  
    /// created from the public properties and fields of the type T.</param> 
    /// <param name="options">Specifies how values from the source sequence will be applied to 
    /// existing rows in the table.</param> 
    /// <returns>A DataTable created from the source sequence.</returns> 
    public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        // Load the table from the scalar sequence if T is a primitive type. 
        if (typeof(T).IsPrimitive)
        {
            return ShredPrimitive(source, table, options);
        }

        // Create a new table if the input table is null. 
        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        // Initialize the ordinal map and extend the table schema based on type T.
        table = ExtendTable(table, typeof(T));

        // Enumerate the source sequence and load the object values into rows.
        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (options != null)
                {
                    table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(ShredObject(table, e.Current), true);
                }
            }
        }
        table.EndLoadData();

        // Return the table. 
        return table;
    }

    public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        // Create a new table if the input table is null. 
        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        if (!table.Columns.Contains("Value"))
        {
            table.Columns.Add("Value", typeof(T));
        }

        // Enumerate the source sequence and load the scalar values into rows.
        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            Object[] values = new object[table.Columns.Count];
            while (e.MoveNext())
            {
                values[table.Columns["Value"].Ordinal] = e.Current;

                if (options != null)
                {
                    table.LoadDataRow(values, (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(values, true);
                }
            }
        }
        table.EndLoadData();

        // Return the table. 
        return table;
    }

    public object[] ShredObject(DataTable table, T instance)
    {

        FieldInfo[] fi = _fi;
        PropertyInfo[] pi = _pi;

        if (instance.GetType() != typeof(T))
        {
            // If the instance is derived from T, extend the table schema 
            // and get the properties and fields.
            ExtendTable(table, instance.GetType());
            fi = instance.GetType().GetFields();
            pi = instance.GetType().GetProperties();
        }

        // Add the property and field values of the instance to an array.
        Object[] values = new object[table.Columns.Count];
        foreach (FieldInfo f in fi)
        {
            values[_ordinalMap[f.Name]] = f.GetValue(instance);
        }

        foreach (PropertyInfo p in pi)
        {
            values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
        }

        // Return the property and field values of the instance. 
        return values;
    }

    public DataTable ExtendTable(DataTable table, Type type)
    {
        // Extend the table schema if the input table was null or if the value  
        // in the sequence is derived from type T.             
        foreach (FieldInfo f in type.GetFields())
        {
            if (!_ordinalMap.ContainsKey(f.Name))
            {
                // Add the field as a column in the table if it doesn't exist 
                // already.
                DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                    : table.Columns.Add(f.Name, f.FieldType);

                // Add the field to the ordinal map.
                _ordinalMap.Add(f.Name, dc.Ordinal);
            }
        }
        foreach (PropertyInfo p in type.GetProperties())
        {
            if (!_ordinalMap.ContainsKey(p.Name))
            {
                // Add the property as a column in the table if it doesn't exist 
                // already.
                DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                    : table.Columns.Add(p.Name, p.PropertyType);

                // Add the property to the ordinal map.
                _ordinalMap.Add(p.Name, dc.Ordinal);
            }
        }

        // Return the table. 
        return table;
    }
}

public static class CustomLINQtoDataSetMethods
{
    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
    {
        return new ObjectShredder<T>().Shred(source, null, null);
    }

    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
                                                DataTable table, LoadOption? options)
    {
        return new ObjectShredder<T>().Shred(source, table, options);
    }

}
