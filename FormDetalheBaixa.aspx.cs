using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class FormDetalheBaixa : BaseForm
{
    FolhaLancamento folha;

    public FormDetalheBaixa()
        : base("GRID_BAIXA_TITULO")
    {
        folha = new FolhaLancamento(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title += "Detalhamento da Baixa";

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                labelBaixa.Text = Request.QueryString["id"].ToString();
                List<SLancamento> lista = folha.carregaBaixa(Convert.ToDouble(Request.QueryString["id"]));
    
                string html = "<table id=\"gridLancamentos\" cellpadding=\"0\" cellspacing=\"0\" class=\"gridLancamentos\">";
                html += "<tr class=\"cab\"> ";
                html += "<td id=\"origem\">Origem</td> ";
                html += "<td id=\"linhaSeqLote\" style=\"display:none;\">Seq Lote</td> ";
                html += "<td class=\"debCred\">D/C</td> ";
                html += "<td class=\"dataLancto\">Data</td> ";
                html += "<td class=\"conta\">Conta</td> ";
                html += "<td class=\"job\">Job</td> ";
                html += "<td class=\"linhaNegocio\" style=\"display:none;\">Linha Negócio</td> ";
                html += "<td class=\"divisao\"  style=\"display:none;\">Divisão</td> ";
                html += "<td class=\"valor\">Valor($)</td> ";
                html += "<td class=\"historico\">Histórico</td> ";
                html += "<td class=\"cotacao\"></td> ";
                html += "<td class=\"fornecedor\">Terceiro</td> ";
                html += "<td class=\"titulo\">Título</td> ";
                html += "<td class=\"deletar\"></td> ";
                html += "<td class=\"parcelas\" style=\"display:none;\">N° Parc.</td> ";
                html += "</tr> ";
                
                for(int i=0;i<lista.Count;i++)
                {
                    if (lista[i].debCred == 'C' && lista[i].valor < 0)
                        lista[i].valor = -lista[i].valor;
                    else if (lista[i].debCred == 'C' && lista[i].valor > 0)
                        lista[i].debCred = 'D';
                    else if (lista[i].debCred == 'D' && lista[i].valor < 0)
                    {
                        lista[i].debCred = 'C';
                        lista[i].valor = -lista[i].valor;
                    }
                    html += "<tr class='linha'>";
                    html += "<td id='origem'><a href='FormGenericTitulos.aspx?modulo=" + lista[i].modulo + "&lote=" + lista[i].lotePai + "'>";
                    html += lista[i].lotePai;
                    html += "</a></td>";
                    html += "<td id='linhaSeqLote' style=\"display:none;\">";
                    html += lista[i].seqLote;
                    html += "</td>";
                    html += "<td style=\"display:none;\">FormDetalheBaixa.aspx</td>";
                    html += "<td class='debCred'>";
                    html += lista[i].debCred;
                    html += "</td>";
                    html += "<td class='dataLancto'>";
                    html += lista[i].dataLancamento.Value.ToString("dd/MM/yyyy");
                    html += "</td>";
                    html += "<td class='conta'>";
                    html += lista[i].descConta;
                    html += "</td>";
                    html += "<td class='job'>";
                    html += lista[i].descJob;
                    html += "</td>";
                    html += "<td class='linhaNegocio'  style=\"display:none;\">";
                    html += lista[i].descLinhaNegocio;
                    html += "</td>";
                    html += "<td class='divisao' style=\"display:none;\">";
                    html += lista[i].descDivisao;
                    html += "</td>";
                    html += "<td class='valor'>";
                    html += String.Format("{0:0,0.00}", lista[i].valor.Value);
                    html += "</td>";
                    html += "<td class='historico'>";
                    html += lista[i].historico;
                    html += "</td>";
                    html += "<td class='cotacao'>";
                    html += "<a onclick='loadCotacao(2," + lista[i].seqLote + ");' href='#'><img src='Imagens/dindin.png' width='24' height='18'></a>";
                    html += "</td>";
                    html += "<td class='fornecedor'>";
                    html += lista[i].descTerceiro;
                    html += "</td>";
                    html += "<td class='titulo'>";
                    if (lista[i].titulo.Value)
                        html += "<img src='Imagens/icones/tick.gif' alt='' />";
                    else
                        html += " - ";
                    html += "</td>";
                    html += "<td class='deletar'>";
                    
                    html += "";
                        
                    html += "</td>";
                    html += "<td class='parcelas' style=\"display:none;\">";
                    html += "</td>";
                    html += "</tr>"; 
                }

                html += "</table>";

                lGrid.Text = html;
            }
            else
            {
                Response.Redirect("FormGridBaixas.aspx", true);
            }
        }
    }

    protected override void montaTela()
    {
        base.montaTela();

        subTitulo.Text = "Detalhe";
        addSubTitulo("Baixas", "FormGridBaixas.aspx");
    }
}
