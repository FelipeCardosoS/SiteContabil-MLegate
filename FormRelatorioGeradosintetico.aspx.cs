using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;

public partial class FormRelatorioGerado : Permissao
{
    Relatorios relatorio;
    DataTable tb = new DataTable("tbRelatorio");
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
                CheckBox checkContasEncerramento;
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
                CheckBox checkContasEncerramentoDRE;
				CheckBox checkContasZeradas;


				DropDownList comboDetalhamento1;
                DropDownList comboDetalhamento2;
                DropDownList comboDetalhamento3;
                DropDownList comboDetalhamento4;
                DropDownList comboDetalhamento5;

                string contaDe = null;
                string contaAte = null;
                string tipo = null;
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
                Nullable<int> tipoMoeda = null;

                string detalhamento1 = null;
                string detalhamento2 = null;
                string detalhamento3 = null;
                string detalhamento4 = null;
                string detalhamento5 = null;
                List<string> erros = new List<string>();
                string filtros = "";

                switch (hiddenRelatorio.Value)
                {
                    #region Relatório Balancete
                    case "BALANCETE":

                        textPeriodo = PreviousPage.txtPeriodo;
                        checkContasEncerramento = PreviousPage.checkContasEncerr;
                        comboDivisaoDe = PreviousPage.cmbDivisaoDe;
                        comboDivisaoAte = PreviousPage.cmbDivisaoAte;
                        comboLinhaNegocioDe = PreviousPage.cmbLinhaNegocioDe;
                        comboLinhaNegocioAte = PreviousPage.cmbLinhaNegocioAte;
                        comboClienteDe = PreviousPage.cmbClienteDe;
                        comboClienteAte = PreviousPage.cmbClienteAte;
                        comboJobDe = PreviousPage.cmbJobDe;
                        comboJobAte = PreviousPage.cmbJobAte;
						checkContasZeradas = PreviousPage.checkContasZeradas;

                        comboDetalhamento1 = PreviousPage.cmbDetalhamento1;
                        comboDetalhamento2 = PreviousPage.cmbDetalhamento2;
                        comboDetalhamento3 = PreviousPage.cmbDetalhamento3;
                        comboDetalhamento4 = PreviousPage.cmbDetalhamento4;
                        comboDetalhamento5 = PreviousPage.cmbDetalhamento5;

                        empresas = PreviousPage.empresas_selecionadas;

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
                        //foreach (RepeaterItem ri in gvempresas.Items)
                        //{        
                        //    CheckBox box = (CheckBox)ri.FindControl("chkBox");
                        //    if (box.Checked)
                        //    {
                        //        ...
                        //    }
                        //}

                        Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        erros = relatorio.balancete(0, Convert.ToDateTime(textPeriodo.Text),
                            divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte,
                            jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb, checkContasEncerramento.Checked, empresas, "", checkContasZeradas.Checked);
                        if (erros.Count == 0)
                        {
                            Title += " - Balancete Contábil";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Balancete Contábil";
                            filtros = "";
                            Master.literalNomeEmpresa.Text = empresa.nome;
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

                            string html = "<table cellpadding=\"0\" cellspacing=\"0\">\n";
                            html += "<tr class=\"titulo\">\n";
                            html += "<td style=\"width:50px;\">Código</td>\n";
                            html += "<td style=\"width:160px;\">Conta</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Anterior</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Débito</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Crédito</td>\n";
                            html += "<td class=\"valores\" style=\"padding-right:4px; padding-left:4px;\">Saldo Atual</td>\n";
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

                                                                            html += "<td class=\"valores\"><a href=\"javascript:abreRelatorioRazao('FormRelatorioRazao.aspx?contaDe=" + row["cod_conta"] + "&contaAte=" + row["cod_conta"] + "&periodo=" + Convert.ToDateTime(textPeriodo.Text).ToString("MM/yyyy") + parametros + filtros + "');\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</a></td>\n";
                                                                        }
                                                                        else
                                                                        {
                                                                            html += "<td class=\"valores\">" + String.Format("{0:0,0.00}", (decimal.Parse(tb.Rows[i]["saldo_ini"].ToString()) + decimal.Parse(tb.Rows[i]["debito"].ToString())) - decimal.Parse(tb.Rows[i]["credito"].ToString())) + "</td>\n";
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

                        divisaoDe = null;
                        divisaoAte = null;
                        linhaNegocioDe = null;
                        linhaNegocioAte = null;
                        clienteDe = null;
                        clienteAte = null;
                        jobDe = null;
                        jobAte = null;
                        tipoMoeda = null;

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
                            Master.literalNomeEmpresa.Text = empresa.nome;
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
                            htmlx.Append("<td>Conta</td>\n");
                            htmlx.Append("<td class=\"valores\">Acumulado</td>");
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
                            DataRow row = null;
                            DataRow rowAnt = null;
                            decimal valor = 0;
                            decimal totalEntradaConta = 0;
                            decimal totalSaidaConta = 0;
                            decimal totalSaldoInicial = 0;
                            decimal totalSaldoFinal = 0;
                            decimal totalMovimentoEntrada = 0;
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
                            contaDe, contaAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, comboOrdenacao.SelectedValue, ref tb);

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
                                                html += "<td colspan=\"2\" align=\"right\">Total de " + rowAnt["desc_conta"] + "</td>\n";
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

                        erros = relatorio.razaoContabilsintetico(codMoeda, contaDe, contaAte, Convert.ToDateTime(textPeriodoDe.Text),
                            Convert.ToDateTime(textPeriodoAte.Text), divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte,
                            clienteDe, clienteAte, jobDe, jobAte, terDe, terAte, ref tb, empresas);

                        if (erros.Count == 0)
                        {
                            Title += " - Razão Contábil";
                            empresa.codigo = Convert.ToInt32(Session["empresa"]);
                            empresa.load();
                            Master.literalNomeRelatorio.Text = "Razão Contábil";

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

                            DateTime inicio = Convert.ToDateTime(textPeriodoDe.Text);
                            DateTime termino = Convert.ToDateTime(textPeriodoAte.Text);


                            StringBuilder textFast = new StringBuilder();

                            textFast.Append("<table cellpadding=\"0\" id=\"razao\" cellspacing=\"1\">\n");
                            textFast.Append("<thead>\n");
                            textFast.Append("<tr class=\"titulo\">\n");
                            textFast.Append("<td>Data</td>\n");
                            textFast.Append("<td>Lote/Baixa</td>\n");
                            textFast.Append("<td>Nº Doc.</td>\n");
                            textFast.Append("<td>Histórico</td>\n");
                            textFast.Append("<td class=\"valores\">Débito</td>\n");
                            textFast.Append("<td class=\"valores\">Crédito</td>\n");
                            textFast.Append("<td class=\"valores\">Saldo</td>\n");
                            textFast.Append("</tr>\n");
                            textFast.Append("</thead>");

                            string codContaAnt = "";
                            string dataAnt = "";
                            string loteAnt = "";
                            bool primeiraConta = true;
                            bool foiSaldoInicial = false;
                            DataRow row = null;
                            DataRow rAnt = null;
                            decimal valor = 0;
                            decimal totalData = 0;
                            decimal totalLote = 0;
                            DataRow r = null;
                            decimal valorSaldoInicial = 0;
                            decimal valorOriginal = 0;
                            DateTime dataLancamento = new DateTime();
                            for (int i = 0; i < tb.Rows.Count; i++)
                            {
                                r = tb.Rows[i];
                                valorOriginal = Convert.ToDecimal(r["valor"]);
                                dataLancamento = Convert.ToDateTime(r["data"]);

                                if (codContaAnt != r["cod_conta"].ToString())
                                {
                                    if (!primeiraConta)
                                    {
                                        textFast.Append("<tr class=\"fim conta nivel0\">\n");
                                        textFast.Append("<td colspan=\"4\">Saldo Final da Conta</td>\n");
                                        //textFast.Append("<td colspan=\"4\">Saldo Final da Conta : " + rAnt["cod_conta"].ToString() + " - " + rAnt["descricao"] + "</td>\n");

                                        //if (valor > 0)
                                        //{
                                        //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                        //    textFast.Append("<td class=\"valores\">-</td>");
                                        //}
                                        //else
                                        //{
                                        //    textFast.Append("<td class=\"valores\">-</td>");
                                        //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                        //}
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>");
                                        textFast.Append("</tr>\n");

                                    }

                                    valor = 0;
                                    codContaAnt = r["cod_conta"].ToString();

                                    if (loteAnt != r["lote_baixa"].ToString())
                                    {
                                        totalLote = 0;
                                    }
                                    if (dataAnt != r["data"].ToString())
                                    {
                                        //totalData = 0;
                                    }

                                    dataAnt = r["data"].ToString();
                                    loteAnt = r["lote_baixa"].ToString();

                                    //Verifico se é a primeira linha, caso seja não insiro a linha branca
                                    if (i != 0)
                                    {
                                        textFast.Append("<tr class=\"\">\n");
                                        textFast.Append("<td colspan=\"7\">&nbsp;</td>\n");
                                        textFast.Append("</tr>\n");
                                    }

                                    textFast.Append("<tr class=\"\">\n");
                                    textFast.Append("<td colspan=\"7\"><font size=\"2\" face=\"Tahoma\"><b>" + r["cod_conta"].ToString() + " - " + r["descricao"] + "</b></font></td>\n");
                                    textFast.Append("</tr>\n");

                                    textFast.Append("<tr class=\"inicio1 nivel00\">\n");
                                    textFast.Append("<td colspan=\"4\">Saldo Inicial da Conta</td>\n");
                                    //textFast.Append("<td colspan=\"4\">Saldo Inicial da Conta: " + r["cod_conta"].ToString() + " - " + r["descricao"] + " </td>\n");

                                    valorSaldoInicial = 0;

                                    if (inicio < dataLancamento)
                                    {
                                        valorSaldoInicial = 0;
                                    }
                                    else
                                    {
                                        valorSaldoInicial = valorOriginal;
                                    }

                                    if (primeiraConta)
                                    {
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                    }
                                    else
                                    {
                                        //if (valorOriginal > 0)
                                        //{
                                        //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                        //    textFast.Append("<td class=\"valores\">-</td>");
                                        //}
                                        //else
                                        //{
                                        //    textFast.Append("<td class=\"valores\">-</td>");
                                        //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>\n");
                                        //}
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">-</td>");
                                        textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valorSaldoInicial) + "</td>");
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

                                            loteAnt = r["lote_baixa"].ToString();
                                            totalLote = 0;


                                            dataAnt = r["data"].ToString();
                                            totalData = 0;
                                        }

                                        if (loteAnt != r["lote_baixa"].ToString())
                                        {

                                            loteAnt = r["lote_baixa"].ToString();
                                            totalLote = 0;
                                        }
                                    }


                                    textFast.Append("<tr class=\"linha nivel0\">\n");
                                    textFast.Append("<td>" + dataLancamento.ToString("dd/MM/yyyy") + "</td>\n");
                                    if (r["lote_baixa"].ToString().Contains("B"))
                                    {
                                        textFast.Append("<td><a href='FormDetalheBaixa.aspx?id=" + r["lote_baixa"].ToString().Substring(1, r["lote_baixa"].ToString().Length - 1) + "' target='_blank'>" + r["lote_baixa"] + "</a></td>\n");
                                    }
                                    else
                                        textFast.Append("<td><a href='FormGenericTitulos.aspx?modulo=" + r["modulo"] + "&lote=" + r["lote_baixa"].ToString().Substring(1, r["lote_baixa"].ToString().Length - 1) + "' target='_blank'>" + r["lote_baixa"] + "</a></td>\n");
                                    textFast.Append("<td>" + r["numero_documento"] + "</td>\n");
                                    textFast.Append("<td>" + r["historico"] + "</td>\n");


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
                                    textFast.Append("</tr>\n");





                                    totalLote += valorOriginal;
                                    foiSaldoInicial = false;
                                }
                                else
                                {
                                    foiSaldoInicial = true;
                                }
                                valor += valorOriginal;
                                totalData += valorOriginal;
                                loteAnt = r["lote_baixa"].ToString();
                                dataAnt = r["data"].ToString();
                                rAnt = r;

                            }

                            if (rAnt != null)
                            {
                                textFast.Append("<tr class=\"fim conta nivel0\">\n");
                                textFast.Append("<td colspan=\"4\">Saldo Final da Conta</td>\n");
                                //textFast.Append("<td colspan=\"4\">Saldo Final da Conta : " + rAnt["cod_conta"].ToString() + " - " + rAnt["descricao"] + "</td>\n");

                                //if (valor > 0)
                                //{
                                //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                //    textFast.Append("<td class=\"valores\">-</td>");
                                //}
                                //else
                                //{
                                //    textFast.Append("<td class=\"valores\">-</td>");
                                //    textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>\n");
                                //}
                                textFast.Append("<td class=\"valores\">-</td>");
                                textFast.Append("<td class=\"valores\">-</td>");
                                textFast.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", valor) + "</td>");
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

                detalheMovFinanc[nivel-1] = Convert.ToInt32(row["codDetalhe" + nivel + ""]);
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

    private void geraLinha(string relatorio, int nivel, DataRow row, ref string html)
    {
        switch (relatorio)
        {
            case "MOV_FINANC":
                html += "<tr class=\"linha nivel"+nivel+"\">\n";
                html += "<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n";
                html += "<td>" + row["nome_razao_social"] + "</td>\n";
                if (nivel == 0)
                {
                    html += "<td>" + row["descr"] + "</td>\n";
                }
                else
                {
                    html += "<td>" + row["descDetalhe"+nivel+""].ToString().Replace(" ", "&nbsp;") + "</td>\n";
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

                    html += "<td class=\"entrada valores\">"+ inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + terminoLink + "</td>\n";
                    html += "<td class=\"saida valores\"></td>\n";
                }
                else
                {
                    html += "<td class=\"entrada valores\"></td>\n";
                    html += "<td class=\"saida valores\">" + inicioLink + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"]) * -1) + terminoLink+ "</td>\n";
                }

                break;
        }
    }

   
}
