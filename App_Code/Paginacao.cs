using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public class Paginacao
{
    private int _pagina;
    private int _totalPaginas;
    private int _range;
    private int _inicio;
    private int _fim;

    public Paginacao() { }
    public Paginacao(int pagina, int totalPaginas, int range)
        : this()
    {
        _pagina = pagina;
        _totalPaginas = totalPaginas;
        _range = range;
    }

    public int pagina
    {
        set { _pagina = value; }
    }
    public int totalPaginas
    {
        set { _totalPaginas = value; }
    }
    public int range 
    {
        set { _range = value; }
    }
    public int inicio
    {
        get { return _inicio; }
    }
    public int fim
    {
        get { return _fim; }
    }

    public string monta()
    {
        string html = "";
        string url =  this.url();
        this.getInicioFim();

        html += "<div class=\"boxPaginacao\">";
        if (_pagina > 1)
            html += "<a class=\"botaoProAnt\" href=\"" + url + "pag=" + (_pagina - 1) + "\">< anterior</a>";
        for (int i = _inicio; i <= _fim; i++)
        {
            if (i == _pagina)
            {
                html += "<span class=\"paginaAtual\">" + i + "</span>";
            }
            else
            {
                html += "<a href=\""+url+"pag="+i+"\">"+i+"</a>";
            }
        }
        if (_pagina < _totalPaginas)
            html += "<a class=\"botaoProAnt\" href=\"" + url + "pag=" + (_pagina + 1) + "\">próxima ></a>";
        html += "</div>";

        return html;
    }

    private string url()
    {
        string url = "";
        string urlCompleta = HttpContext.Current.Request.Url.AbsoluteUri;
        string[] arrUrl = urlCompleta.Split('?');
        url = arrUrl[0];
        int nQuerys = HttpContext.Current.Request.QueryString.Keys.Count;

        url += "?";

        if (nQuerys > 0)
        {
            for (int i = 0; i < nQuerys; i++)
            {
                if (HttpContext.Current.Request.QueryString.Keys[i] != "pag")
                {
                    url += HttpContext.Current.Request.QueryString.Keys[i] + "=" + HttpContext.Current.Request.QueryString[i] + "&";
                }
            }
        }

        return url;
    }

    public void getInicioFim()
    {
        int pg = 0;
        int total = 0;
        int intervalo = 0;

        if (_pagina <= 0)
            pg = 1;
        else
            pg = _pagina;

        if (_totalPaginas <= 0)
            total = 1;
        else
            total = _totalPaginas;

        intervalo = 5;
        _inicio = pg - intervalo;
        _fim = pg + intervalo;
        
        if (_inicio < 1)
        {
            _inicio = 1;
            _fim = _range;
        }

        if (_fim > total)
        {
            _fim = total;
        }
    }
}

