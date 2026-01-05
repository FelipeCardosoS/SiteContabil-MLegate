using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class BaseGridForm : BaseForm
{
    protected int range = 50;
    protected double totalRegistros = 0;
    protected string ordenacao = "";

    protected HtmlContainerControl areaBotoesGrid;
    protected Button botaoFiltrar;
    protected Button botaoContabilizar;
    protected Button botaoGerarArquivo;
    protected Button botaoImportarArquivo;
    protected Button botaoDeletar;
    protected HyperLink botaoNovo;
    protected DropDownList comboOrdenar;

    protected DataSet dsDados = new DataSet("dsDados");

    public int paginaAtual
    {
        get
        {
            if (ViewState["paginaAtual"] == null || (int)ViewState["paginaAtual"] < 1)
                return 1;
            else
                return (int)ViewState["paginaAtual"];
        }
        set
        {
            if (ViewState["paginaAtual"] == null)
                ViewState.Add("paginaAtual", value);
            else
                ViewState["paginaAtual"] = value;
        }
    }

    public int totalPaginas
    {
        get
        {
            if (ViewState["totalPaginas"] == null || (int)ViewState["totalPaginas"] < 1)
                return 1;
            else
                return (int)ViewState["totalPaginas"];
        }
        set
        {
            if (ViewState["totalPaginas"] == null)
                ViewState.Add("totalPaginas", value);
            else
                ViewState["totalPaginas"] = value;
        }
    }


	public BaseGridForm(string modulo)
        :base(modulo)
	{
	
	}

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Page.IsPostBack)
            montaGrid();
    }

    protected override void montaTela()
    {
        base.montaTela();

        areaBotoesGrid = (HtmlContainerControl)Master.FindControl("areaBotoesGrid");
        botaoNovo = (HyperLink)Master.FindControl("botaoNovo");
        botaoFiltrar = (Button)Master.FindControl("botaoFiltrar");
        botaoContabilizar = (Button)Master.FindControl("botaoContabilizar");
        botaoGerarArquivo = (Button)Master.FindControl("botaoGerarArquivo");
        botaoImportarArquivo = (Button)Master.FindControl("botaoImportarArquivo");
        botaoDeletar = (Button)Master.FindControl("botaoDeletar");
        comboOrdenar = (DropDownList)Master.FindControl("comboOrdenar");

        botaoContabilizar.Attributes.Add("onclick", "return Verifica_Selecionados('botaoContabilizar');");
        botaoGerarArquivo.Attributes.Add("onclick", "return function_botaoGerarArquivo();");
        botaoDeletar.Attributes.Add("onclick", "return Verifica_Selecionados('botaoDeletar');");

        botaoFiltrar.Click += botaoFiltrar_Click;
        botaoContabilizar.Click += botaoContabilizar_Click;
        botaoGerarArquivo.Click += botaoGerarArquivo_Click;
        botaoImportarArquivo.Click += botaoImportarArquivo_Click;
        botaoDeletar.Click += botaoDeletar_Click;
        comboOrdenar.SelectedIndexChanged += comboOrdenar_SelectedIndexChanged;

        LinkButton linkAnterior = (LinkButton)Master.FindControl("linkPaginacaoAnterior");
        linkAnterior.Click += linkPaginacaoAnterior_Click;
        LinkButton linkProximo = (LinkButton)Master.FindControl("linkPaginacaoProximo");
        linkProximo.Click += linkPaginacaoProximo_Click;

        LinkButton linkPrimeira = (LinkButton)Master.FindControl("linkPrimeira");
        linkPrimeira.Click += linkPrimeira_Click;
        LinkButton linkUltima = (LinkButton)Master.FindControl("linkUltima");
        linkUltima.Click += linkUltima_Click;

        for (int i = 1; i <= 10; i++)
        {
            LinkButton link = (LinkButton)Master.FindControl("linkPaginacao" + i);
         
            if (link != null)
                link.Click += new EventHandler(linkPaginacao_Click);
        }

    }

    protected virtual void montaGrid()
    {
        Label lNenhumRegistro = (Label)Master.FindControl("labelNenhumRegistro");

        if (totalRegistros <= 0)
            lNenhumRegistro.Visible = true;
        else
            lNenhumRegistro.Visible = false;

        if (comboOrdenar.SelectedValue != "0")
            ordenacao = comboOrdenar.SelectedValue;

        montaPaginacao();
        verificaTarefas();

    }

    protected void montaPaginacao()
    {
        HtmlContainerControl boxPaginacao = (HtmlContainerControl)Master.FindControl("boxPaginacao");
        LinkButton linkPrimeira = (LinkButton)Master.FindControl("linkPrimeira");
        LinkButton linkUltima = (LinkButton)Master.FindControl("linkUltima");
        LinkButton linkAnterior = (LinkButton)Master.FindControl("linkPaginacaoAnterior");
        LinkButton linkProximo = (LinkButton)Master.FindControl("linkPaginacaoProximo");

        totalPaginas = Convert.ToInt32(Math.Ceiling(totalRegistros / range));
        
        if (totalRegistros > 0 && totalPaginas > 1)
        {
            boxPaginacao.Visible = true;
            linkPrimeira.Visible = true;
            linkUltima.Visible = true;
            linkAnterior.Visible = true;
            linkProximo.Visible = true;
            
            Paginacao paginacao = new Paginacao(paginaAtual, totalPaginas, range);
            paginacao.getInicioFim();

            if (paginaAtual <= 1)
            {
                linkPrimeira.Enabled = false;
                linkAnterior.Enabled = false;
            }
            else
            {
                linkPrimeira.Enabled = true;
                linkAnterior.Enabled = true;
            }

            if (paginaAtual >= totalPaginas)
            {
                linkUltima.Enabled = false;
                linkProximo.Enabled = false;
            }
            else
            {
                linkUltima.Enabled = true;
                linkProximo.Enabled = true;
            }

            for (int i = 1; i <= 10; i++)
            {
                LinkButton link = (LinkButton)Master.FindControl("linkPaginacao" + i);
                link.Visible = false;
            }

            int contPaginacao = 1;
            
            for (int i = paginacao.inicio; i <= paginacao.fim; i++)
            {
                LinkButton link = (LinkButton)Master.FindControl("linkPaginacao" + contPaginacao);
                
                if (link != null)
                {
                    if (paginaAtual == i)
                    {
                        link.CssClass = "atual";
                        link.Enabled = false;
                    }
                    else
                    {
                        link.CssClass = "";
                        link.Enabled = true;
                    }
                    link.Visible = true;
                    link.Click += new EventHandler(linkPaginacao_Click);
                    link.Text = i.ToString();
                }
                contPaginacao++;
            }
        }
        else
        {
            linkPrimeira.Visible = false;
            linkUltima.Visible = false;
            linkAnterior.Visible = false;
            linkProximo.Visible = false;

            for (int i = 1; i <= 10; i++)
            {
                LinkButton link = (LinkButton)Master.FindControl("linkPaginacao" + i);
                link.Visible = false;
            }
        }
    }

    protected virtual void botaoFiltrar_Click(object sender, EventArgs e)
    {
        paginaAtual = 1;
        montaGrid();
    }

    protected virtual void botaoContabilizar_Click(object sender, EventArgs e)
    {

    }

    protected virtual void botaoGerarArquivo_Click(object sender, EventArgs e)
    {

    }

    protected virtual void botaoImportarArquivo_Click(object sender, EventArgs e)
    {

    }

    protected virtual void botaoDeletar_Click(object sender, EventArgs e)
    {

    }

    protected void comboOrdenar_SelectedIndexChanged(object sender, EventArgs e)
    {
        montaGrid();
    }

    protected virtual void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected virtual void linkPaginacao_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;

        if (link != null)
            paginaAtual = Convert.ToInt32(link.Text);
        
        montaGrid();
    }

    protected virtual void linkPaginacaoAnterior_Click(object sender, EventArgs e)
    {
        paginaAtual--;
        montaGrid();
    }

    protected virtual void linkPaginacaoProximo_Click(object sender, EventArgs e)
    {
        paginaAtual++;
        montaGrid();
    }

    protected virtual void linkPrimeira_Click(object sender, EventArgs e)
    {
        paginaAtual = 1;
        montaGrid();
    }

    protected virtual void linkUltima_Click(object sender, EventArgs e)
    {
        paginaAtual = totalPaginas;
        montaGrid();
    }
}