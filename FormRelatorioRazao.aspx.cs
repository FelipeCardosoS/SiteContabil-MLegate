using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class FormRelatorioRazao : Permissao
{
    Relatorios relatorio;
    DataTable tb = new DataTable("tbRelatorio");
    Empresa empresa;

    public FormRelatorioRazao()
        : base("RELATORIO")
    {
        relatorio = new Relatorios(_conn);
        empresa = new Empresa(_conn);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["contaDe"] != null && Request.QueryString["contaAte"] != null 
            && Request.QueryString["periodo"] != null && Request.QueryString["tipo"] != null)
        {
            string contaDe = Request.QueryString["contaDe"].ToString();
            string contaAte = Request.QueryString["contaAte"].ToString();
            List<string> erros = new List<string>();
            DateTime periodo = Convert.ToDateTime(Request.QueryString["periodo"]);
            Nullable<int> moedaTipo = null;
            Nullable<int> divisaoDe = null;
            Nullable<int> divisaoAte = null;
            Nullable<int> linhaNegocioDe = null;
            Nullable<int> linhaNegocioAte = null;
            Nullable<int> clienteDe = null;
            Nullable<int> clienteAte = null;
            Nullable<int> jobDe = null;
            Nullable<int> jobAte = null;
            string detalhamento1Cod = null;
            string detalhamento2Cod = null;
            string detalhamento3Cod = null;
            string detalhamento4Cod = null;
            string detalhamento5Cod = null;
            string detalhamento1 = null;
            string detalhamento2 = null;
            string detalhamento3 = null;
            string detalhamento4 = null;
            string detalhamento5 = null;
            int tipomoeda = 0;

            if (Request.QueryString["divisaoDe"] != null && Request.QueryString["divisaoAte"] != null)
            {
                divisaoDe = Convert.ToInt32(Request.QueryString["divisaoDe"]);
                divisaoAte = Convert.ToInt32(Request.QueryString["divisaoAte"]);
            }
            if (Request.QueryString["linhaNegocioDe"] != null && Request.QueryString["linhaNegocioAte"] != null)
            {
                linhaNegocioDe = Convert.ToInt32(Request.QueryString["linhaNegocioDe"]);
                linhaNegocioAte = Convert.ToInt32(Request.QueryString["linhaNegocioAte"]);
            }
            if (Request.QueryString["clienteDe"] != null && Request.QueryString["clienteAte"] != null)
            {
                clienteDe = Convert.ToInt32(Request.QueryString["clienteDe"]);
                clienteAte = Convert.ToInt32(Request.QueryString["clienteAte"]);
            }
            if (Request.QueryString["jobDe"] != null && Request.QueryString["jobAte"] != null)
            {
                jobDe = Convert.ToInt32(Request.QueryString["jobDe"]);
                jobAte = Convert.ToInt32(Request.QueryString["jobAte"]);
            }
            if (Request.QueryString["moeda"] != null)
            {
                tipomoeda = Convert.ToInt32(Request.QueryString["moeda"]);                
            }

            if (Request.QueryString["detalhe1"] != null && Request.QueryString["detalhe1Cod"] != null)
            {
                detalhamento1 = Request.QueryString["detalhe1"].ToString();
                detalhamento1Cod = Request.QueryString["detalhe1Cod"].ToString();

                if (Request.QueryString["detalhe2"] != null && Request.QueryString["detalhe2Cod"] != null)
                {
                    detalhamento2 = Request.QueryString["detalhe2"].ToString();
                    detalhamento2Cod = Request.QueryString["detalhe2Cod"].ToString();

                    if (Request.QueryString["detalhe3"] != null && Request.QueryString["detalhe3Cod"] != null)
                    {
                        detalhamento3 = Request.QueryString["detalhe3"].ToString();
                        detalhamento3Cod = Request.QueryString["detalhe3Cod"].ToString();

                        if (Request.QueryString["detalhe4"] != null && Request.QueryString["detalhe4Cod"] != null)
                        {
                            detalhamento4 = Request.QueryString["detalhe4"].ToString();
                            detalhamento4Cod = Request.QueryString["detalhe4Cod"].ToString();

                            if (Request.QueryString["detalhe5"] != null && Request.QueryString["detalhe5Cod"] != null)
                            {
                                detalhamento5 = Request.QueryString["detalhe5"].ToString();
                                detalhamento5Cod = Request.QueryString["detalhe5Cod"].ToString();
                            }
                        }
                    }
                }
            }
            if (Request.QueryString["tipo"] == "DETALHADO")
            {
                erros = relatorio.razao(contaDe, contaAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte,
                    clienteDe, clienteAte, jobDe, jobAte, periodo, ref tb, detalhamento1, detalhamento1Cod, 
                    detalhamento2, detalhamento2Cod,
                    detalhamento3, detalhamento3Cod, detalhamento4, detalhamento4Cod, detalhamento5, detalhamento5Cod, tipomoeda);
            }
            else if (Request.QueryString["tipo"] == "CONTABIL")
            {
                erros = relatorio.razaoSemDrillFiltrado(contaDe, contaAte, periodo, periodo.AddMonths(1).AddDays(-1), divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte,
                    clienteDe, clienteAte, jobDe, jobAte, ref tb, detalhamento1, detalhamento1Cod, detalhamento2, detalhamento2Cod,
                    detalhamento3, detalhamento3Cod, detalhamento4, detalhamento4Cod, detalhamento5, detalhamento5Cod, tipomoeda);
            }
            if (erros.Count == 0)
            {
                Master.literalDataGeracao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Title += "Razão Detalhado";
                empresa.codigo = Convert.ToInt32(Session["empresa"]);
                empresa.load();
                Master.literalNomeRelatorio.Text = "Razão Detalhado";
                Master.literalNomeEmpresa.Text = empresa.nome;
                string detalhes = "";
                detalhes += "<p>Período: " + periodo.ToString("MM/yyyy") + "</p>";

                DateTime inicio = periodo;
                DateTime termino = periodo.AddMonths(1).AddDays(-1);


                if (contaDe != null)
                {
                    ContaContabil ccDe = new ContaContabil(_conn, contaDe);
                    ccDe.load();
                    detalhes += "<p>Conta: " + ccDe.codigo + " - " + ccDe.descricao + "";
                    if (contaAte != "0")
                    {
                        ContaContabil ccAte = new ContaContabil(_conn, contaAte);
                        ccAte.load();
                        detalhes += " até " + ccAte.codigo + " - " + ccAte.descricao + "</p>";
                    }
                    else
                        detalhes += "</p>";
                }

                Master.literalDetalhes.Text = detalhes;

                string codContaAnt = "";
                DataRow row = null;
                DataRow rowAnt = null;
                decimal valor = 0;
                int codDetalhe1Ant = 0;
                int codDetalhe2Ant = 0;
                int codDetalhe3Ant = 0;
                int codDetalhe4Ant = 0;

                if (Request.QueryString["tipo"] == "DETALHADO")
                {
                    #region Detalhado
                        StringBuilder htmlEx = new StringBuilder();

                        
                        htmlEx.Append("<table summary=\"\" cellpadding=\"0\" id=\"razao\"  cellspacing=\"1\">\n");
                        
                        
                        htmlEx.Append("<tr class=\"titulo\">\n");
                        htmlEx.Append("<td>Data</td>\n");
                        htmlEx.Append("<td>Histórico</td>\n");
                        htmlEx.Append("<td>Cliente</td>\n");
                        htmlEx.Append("<td>Divisão</td>\n");
                        htmlEx.Append("<td>Projeto/Job</td>\n");
                        htmlEx.Append("<td>Terceiro</td>\n");
                        htmlEx.Append("<td class=\"valores\">Débito</td>\n");
                        htmlEx.Append("<td class=\"valores\">Crédito</td>\n");
                        htmlEx.Append("</tr>\n");

                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            row = tb.Rows[i];
                            if (row["cod_conta"].ToString().Equals(codContaAnt))
                            {
                                htmlEx.Append("<tr class=\"linha nivel0\">\n");
                                htmlEx.Append("<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n");
                                htmlEx.Append("<td>" + row["historico"] + "</td>\n");
                                htmlEx.Append("<td>" + row["nome_razao_social"] + "</td>\n");
                                htmlEx.Append("<td>" + row["divisao"] + "</td>\n");
                                htmlEx.Append("<td>" + row["job"] + "</td>\n");
                                htmlEx.Append("<td>" + row["terceiro"] + "</td>\n");
                                string urlDrillDown = "";
                                if (Convert.ToDouble(row["seq_baixa"]) == 0)
                                {
                                    if (Convert.ToDouble(row["lote_pai"]) == 0)
                                    {
                                        urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo"].ToString() + "&lote=" + row["lote"].ToString();
                                    }
                                    else
                                    {
                                        urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo"].ToString() + "&lote=" + row["lote_pai"].ToString();
                                    }
                                }
                                else
                                {
                                    urlDrillDown = "FormDetalheBaixa.aspx?id=" + row["seq_baixa"].ToString();
                                }

                                if (Convert.ToDecimal(row["valor"]) > 0)
                                {
                                    htmlEx.Append("<td class=\"valores\"><a href=\"javascript:popupRazao('" + urlDrillDown + "',0,0);\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</a></td>\n");
                                    htmlEx.Append("<td class=\"valores\"></td>");
                                }
                                else
                                {
                                    htmlEx.Append("<td class=\"valores\"></td>");
                                    htmlEx.Append("<td class=\"valores\"><a href=\"javascript:popupRazao('" + urlDrillDown + "',0,0);\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</a></td>\n");
                                }
                                htmlEx.Append("</tr>\n");

                                valor += Convert.ToDecimal(row["valor"]);
                            }
                            else
                            {
                                if (rowAnt != null)
                                {
                                    htmlEx.Append("<tr class=\"fim nivel0\">\n");
                                    htmlEx.Append("<td>" + termino.ToString("dd/MM/yyyy") + "</td>\n");
                                    htmlEx.Append("<td>Saldo Final</td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    if (valor > 0)
                                    {
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                    }
                                    else
                                    {
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                    }
                                    htmlEx.Append("</tr>\n");

                                    valor = 0;

                                    htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                    htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                    htmlEx.Append("<td>Saldo Inicial</td>\n");
                                    htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    if (Convert.ToDecimal(row["valor"]) > 0)
                                    {
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                    }
                                    else
                                    {
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                    }
                                    htmlEx.Append("</tr>\n");
                                }
                                else
                                {
                                    if (row["nome_razao_social"].ToString() == "Saldo Inicial")
                                    {
                                        htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                        htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>Saldo Inicial</td>\n");
                                        htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        if (Convert.ToDecimal(row["valor"]) > 0)
                                        {
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                        }
                                        else
                                        {
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                        }
                                    }
                                    else
                                    {
                                        htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                        htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>Saldo Inicial</td>\n");
                                        htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        htmlEx.Append("<td></td>\n");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", 0) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");


                                        htmlEx.Append("<tr class=\"linha nivel0\">\n");
                                        htmlEx.Append("<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>" + row["historico"] + "</td>\n");
                                        htmlEx.Append("<td>" + row["nome_razao_social"] + "</td>\n");
                                        htmlEx.Append("<td>" + row["divisao"] + "</td>\n");
                                        htmlEx.Append("<td>" + row["job"] + "</td>\n");
                                        htmlEx.Append("<td>" + row["terceiro"] + "</td>\n");

                                        string urlDrillDown = "";
                                        if (Convert.ToDouble(row["seq_baixa"]) == 0)
                                        {
                                            if (Convert.ToDouble(row["lote_pai"]) == 0)
                                            {
                                                urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo"].ToString() + "&lote=" + row["lote"].ToString();
                                            }
                                            else
                                            {
                                                urlDrillDown = "FormGenericTitulos.aspx?modulo=" + row["modulo"].ToString() + "&lote=" + row["lote_pai"].ToString();
                                            }
                                        }
                                        else
                                        {
                                            urlDrillDown = "FormDetalheBaixa.aspx?id=" + row["seq_baixa"].ToString();
                                        }

                                        if (Convert.ToDecimal(row["valor"]) > 0)
                                        {
                                            htmlEx.Append("<td class=\"valores\"><a href=\"javascript:popupRazao('" + urlDrillDown + "',0,0);\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</a></td>\n");
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                        }
                                        else
                                        {
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                            htmlEx.Append("<td class=\"valores\"><a href=\"javascript:popupRazao('" + urlDrillDown + "',0,0);\">" + String.Format("{0:0.00}", Convert.ToDecimal(row["valor"])) + "</a></td>\n");
                                        }
                                        htmlEx.Append("</tr>\n");
                                    }
                                    htmlEx.Append("</tr>\n");
                                }

                                valor += Convert.ToDecimal(row["valor"]);
                            }

                            rowAnt = row;
                            codContaAnt = tb.Rows[i]["cod_conta"].ToString();
                        }

                        if (rowAnt != null)
                        {
                            htmlEx.Append("<tr class=\"fim nivel0\">\n");
                            htmlEx.Append("<td>" + termino.ToString("dd/MM/yyyy") + "</td>\n");
                            htmlEx.Append("<td>Saldo Final</td>\n");
                            htmlEx.Append("<td></td>\n");
                            htmlEx.Append("<td></td>\n");
                            htmlEx.Append("<td></td>\n");
                            htmlEx.Append("<td></td>\n");
                            if (valor > 0)
                            {
                                htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                htmlEx.Append("<td class=\"valores\"></td>");
                            }
                            else
                            {
                                htmlEx.Append("<td class=\"valores\"></td>");
                                htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                            }
                            htmlEx.Append("</tr>\n");
                            valor = 0;
                        }
                        else
                        {
                        }

                        htmlEx.Append("</table>\n");
                   

                        literalRelatorio.Text = htmlEx.ToString();
                    #endregion
                }
                else
                {
                    #region Contabil
                        StringBuilder htmlEx = new StringBuilder();
                        htmlEx.Append("<table cellpadding=\"0\" id=\"razao\" cellspacing=\"1\">\n");
                        htmlEx.Append("<tr class=\"titulo\">\n");
                        htmlEx.Append("<td>Data</td>\n");
                        htmlEx.Append("<td>Histórico</td>\n");
                        htmlEx.Append("<td></td>\n");
                        htmlEx.Append("<td class=\"valores\">Débito</td>\n");
                        htmlEx.Append("<td class=\"valores\">Crédito</td>\n");
                        htmlEx.Append("</tr>\n");

                        for (int i = 0; i < tb.Rows.Count; i++)
                        {
                            row = tb.Rows[i];
                            if (row["cod_conta"].ToString().Equals(codContaAnt))
                            {
                                htmlEx.Append("<tr class=\"linha nivel0\">\n");
                                htmlEx.Append("<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n");
                                htmlEx.Append("<td>" + row["historico"] + "</td>\n");
                                htmlEx.Append("<td>" + row["nome_razao_social"] + "</td>\n");

                                if (Convert.ToDecimal(row["valor"]) > 0)
                                {
                                    htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                    htmlEx.Append("<td class=\"valores\"></td>");
                                }
                                else
                                {
                                    htmlEx.Append("<td class=\"valores\"></td>");
                                    htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                }
                                htmlEx.Append("</tr>\n");

                                valor += Convert.ToDecimal(row["valor"]);
                            }
                            else
                            {
                                if (rowAnt != null)
                                {
                                    htmlEx.Append("<tr class=\"fim nivel0\">\n");
                                    htmlEx.Append("<td>" + termino.ToString("dd/MM/yyyy") + "</td>\n");
                                    htmlEx.Append("<td>Saldo Final</td>\n");
                                    htmlEx.Append("<td></td>\n");
                                    if (valor > 0)
                                    {
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                    }
                                    else
                                    {
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                    }
                                    htmlEx.Append("</tr>\n");

                                    valor = 0;

                                    htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                    htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                    htmlEx.Append("<td>Saldo Inicial</td>\n");
                                    htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                    if (Convert.ToDecimal(row["valor"]) > 0)
                                    {
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                    }
                                    else
                                    {
                                        htmlEx.Append("<td class=\"valores\"></td>");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                    }
                                    htmlEx.Append("</tr>\n");
                                }
                                else
                                {
                                    if (row["nome_razao_social"].ToString() == "Saldo Inicial")
                                    {
                                        htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                        htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>Saldo Inicial</td>\n");
                                        htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                        if (Convert.ToDecimal(row["valor"]) > 0)
                                        {
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                        }
                                        else
                                        {
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                        }
                                    }
                                    else
                                    {
                                        htmlEx.Append("<tr class=\"inicio nivel0\">\n");
                                        htmlEx.Append("<td>" + inicio.ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>Saldo Inicial</td>\n");
                                        htmlEx.Append("<td>" + row["descricao"] + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", 0) + "</td>\n");
                                        htmlEx.Append("<td class=\"valores\"></td>");


                                        htmlEx.Append("<tr class=\"linha nivel0\">\n");
                                        htmlEx.Append("<td>" + Convert.ToDateTime(row["data"]).ToString("dd/MM/yyyy") + "</td>\n");
                                        htmlEx.Append("<td>" + row["historico"] + "</td>\n");
                                        htmlEx.Append("<td>" + row["nome_razao_social"] + "</td>\n");

                                        if (Convert.ToDecimal(row["valor"]) > 0)
                                        {
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                        }
                                        else
                                        {
                                            htmlEx.Append("<td class=\"valores\"></td>");
                                            htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(row["valor"])) + "</td>\n");
                                        }
                                        htmlEx.Append("</tr>\n");
                                    }
                                    htmlEx.Append("</tr>\n");
                                }

                                valor += Convert.ToDecimal(row["valor"]);
                            }

                            rowAnt = row;
                            codContaAnt = tb.Rows[i]["cod_conta"].ToString();
                        }

                        if (rowAnt != null)
                        {
                            htmlEx.Append("<tr class=\"fim nivel0\">\n");
                            htmlEx.Append("<td>" + termino.ToString("dd/MM/yyyy") + "</td>\n");
                            htmlEx.Append("<td>Saldo Final</td>\n");
                            htmlEx.Append("<td></td>\n");
                            if (valor > 0)
                            {
                                htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                                htmlEx.Append("<td class=\"valores\"></td>");
                            }
                            else
                            {
                                htmlEx.Append("<td class=\"valores\"></td>");
                                htmlEx.Append("<td class=\"valores\">" + String.Format("{0:0,0.00}", Convert.ToDecimal(valor)) + "</td>\n");
                            }
                            htmlEx.Append("</tr>\n");
                            valor = 0;
                        }
                        else
                        {
                        }

                        htmlEx.Append("</table>\n");

                        literalRelatorio.Text = htmlEx.ToString();
                    #endregion
                }
            }
        }
        else
        {

        }
    }
}
