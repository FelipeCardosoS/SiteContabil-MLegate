using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;

public class BaseForm : Permissao
{
    protected Literal dataHora;
    protected Literal empresalogin;
    protected Label labelUsuarioOn;
    protected HtmlContainerControl menu;
    protected ImageButton botaoLogout;
    protected HtmlGenericControl areaSubTitulo;
    protected Literal subTitulo;
    private modulosDAO moduloDAO;

	public BaseForm(string modulo)
        :base(modulo)
	{
        moduloDAO = new modulosDAO(_conn);
	}

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);
    }

    protected virtual void Page_Load(object sender, EventArgs e)
    {
        montaTela();
        montaMenu();
        verificaTarefas();

        if (Request.QueryString["pop"] == null)
        {
            if (modulo != "CAP_INCLUSAO_TITULO" && modulo != "CAR_INCLUSAO_TITULO" && modulo != "C_INCLUSAO_LANCTO" && modulo != "MODELO_LANCAMENTO" && modulo != "BAIXA_TITULO")
            {
                Session["ss_lancamentos_CAP_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_CAR_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_C_INCLUSAO_LANCTO"] = null;
                Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                Session["ss_lancamentos_BAIXA_TITULO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
                SessionView.NotaFiscalSession = null;
            }
            else if (modulo == "CAP_INCLUSAO_TITULO")
            {
                Session["ss_lancamentos_CAR_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_C_INCLUSAO_LANCTO"] = null;
                Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                Session["ss_lancamentos_BAIXA_TITULO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
            }
            else if (modulo == "CAR_INCLUSAO_TITULO")
            {
                Session["ss_lancamentos_CAP_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_C_INCLUSAO_LANCTO"] = null;
                Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                Session["ss_lancamentos_BAIXA_TITULO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
            }
            else if (modulo == "C_INCLUSAO_LANCTO")
            {
                Session["ss_lancamentos_CAR_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_CAP_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                Session["ss_lancamentos_BAIXA_TITULO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
            }
            else if (modulo == "MODELO_LANCAMENTO")
            {
                Session["ss_lancamentos_CAR_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_CAP_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_C_INCLUSAO_LANCTO"] = null;
                if (Request.QueryString["tipo"] == null && Request.QueryString["modelo"] == null)
                    Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                Session["ss_lancamentos_BAIXA_TITULO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
            }
            else if (modulo == "BAIXA_TITULO")
            {
                Session["ss_lancamentos_CAR_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_CAP_INCLUSAO_TITULO"] = null;
                Session["ss_lancamentos_MODELO_LANCAMENTO"] = null;
                HttpContext.Current.Session["ss_lote_gerado_" + modulo] = null;
            }
            else { }
        }
    }

    protected override void Page_UnLoad(object sender, EventArgs e)
    {
        base.Page_UnLoad(sender, e);

            if (modulo == "CAP_INCLUSAO_TITULO" && moduloAnterior == "CAR_INCLUSAO_TITULO")
            {
                if (Session["ss_lancamentos"] != null)
                {
                    Session["ss_lancamentos"] = null;
                }
            }
            else if (modulo == "CAR_INCLUSAO_TITULO" && moduloAnterior == "CAP_INCLUSAO_TITULO")
            {
                if (Session["ss_lancamentos"] != null)
                {
                    Session["ss_lancamentos"] = null;
                }
            }
            else { }
    }

    protected virtual void montaTela()
    {
        dataHora = (Literal)Master.FindControl("data_hora");
        empresalogin = (Literal)Master.FindControl("empresa_login");
        labelUsuarioOn = (Label)Master.FindControl("labelUsuarioOn");
        menu = (HtmlContainerControl)Master.FindControl("menu");
        botaoLogout = (ImageButton)Master.FindControl("botaoLogout");
        areaSubTitulo = (HtmlGenericControl)Master.FindControl("areaSubTitulo");
        subTitulo = (Literal)Master.FindControl("subTitulo");

        botaoLogout.Attributes.Add("formnovalidate", "");
        botaoLogout.Click += new ImageClickEventHandler(botaoLogout_Click);
        CultureInfo cultura = new CultureInfo("pt-BR");
        dataHora.Text = DateTime.Now.ToString("dd") + " de " + DateTime.Now.ToString("Y", cultura);
        empresalogin.Text = HttpContext.Current.Session["nome_empresa"].ToString();
        labelUsuarioOn.Text = "Olá, "+_usuario.nome;
    }

    protected void botaoLogout_Click(object sender, ImageClickEventArgs e)
    {
        logout();
    }

    private void montaMenu()
    {
        string html = "<ul id='ulMenu' class='menubar'>\n";
        int cont = 0;

        for (int i = 0; i < _modulos.Count; i++)
        {
            if (_modulos[i].codigoPai == "")
            {
                html += "<li class='submenu'>";
                if(_modulos[i].pagina == "")
                    html += "<a href='javascript:void(0);'>" + _modulos[i].descricao + "</a>";
                else
                    html += "<a href=\"" + _modulos[i].pagina + "\">" + _modulos[i].descricao + "</a>";
                
                bool possuiFilhos = false;
                for (int y = 0; y < _modulos.Count; y++)
                {
                    if (_modulos[y].codigoPai == _modulos[i].codigo)
                    {
                        possuiFilhos = true;
                        break;
                    }
                }
                if (possuiFilhos)
                    achaFilhos(_modulos[i].codigo, ref html);
                html += "</li>\n";
                cont++;
            }
        }
        html += "</ul>\n";

        menu.InnerHtml = html;
    }

    private void achaFilhos(string codigoModulo, ref string html)
    {
        html += "<ul  class='menu'>\n";
        for(int x=0;x<_modulos.Count;x++)
        {
            if (codigoModulo == _modulos[x].codigoPai)
            {
                bool possuiFilhos = false;
                for (int y = 0; y < _modulos.Count; y++)
                {
                    if (_modulos[y].codigoPai == _modulos[x].codigo)
                    {
                        possuiFilhos = true;
                        break;
                    }
                }

                    html += "<li class='submenu'>";


                if (_modulos[x].pagina == "")
                    html += "<a href='javascript:void(0);'>" + _modulos[x].descricao + "</a>";
                else
                    html += "<a href=\"" + _modulos[x].pagina + "\">" + _modulos[x].descricao + "</a>";
                
                if (possuiFilhos)
                    achaFilhos(_modulos[x].codigo, ref html);
                html += "</li>\n";
            }
        }

        html += "</ul>\n";
    }

    protected virtual void addSubTitulo(string link, string url)
    {
        Image seta = new Image();
        seta.ImageUrl = "Imagens/setaDupla.gif";
        HyperLink pgPai = new HyperLink();
        pgPai.Text = link;
        pgPai.NavigateUrl = url;
        areaSubTitulo.Controls.AddAt(0, seta);
        areaSubTitulo.Controls.AddAt(1, pgPai);

    }

    protected void listaModulos(ref DataTable tb)
    {
        moduloDAO.lista(ref tb);
    }

    protected Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
        {
            return root;
        }

        foreach (Control c in root.Controls)
        {
            Control t = FindControlRecursive(c, id);
            if (t != null)
            {
                return t;
            }
        }

        return null;
    }
}
