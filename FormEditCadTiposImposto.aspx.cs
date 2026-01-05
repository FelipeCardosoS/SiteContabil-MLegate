using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadTiposImposto : BaseEditCadForm
{
    tiposImpostoDAO tipoImpostoDAO;
    aliquotasTipoImpostoDAO aliquotaDAO;

    string tipoImposto = "";
    TipoImpostoClass tipoImpostoClass;
    public List<SAliquotaImposto> aliquotas
    {
        get
        {
            if (ViewState["aliquotas"] == null)
            {
                return new List<SAliquotaImposto>();
            }

            return (List<SAliquotaImposto>)ViewState["aliquotas"];
        }
        set
        {
            ViewState["aliquotas"] = value;
        }
    }

    

    public FormEditCadTiposImposto()
        : base("TIPO_IMPOSTO")
    {
        tipoImpostoDAO = new tiposImpostoDAO(_conn);
        aliquotaDAO = new aliquotasTipoImpostoDAO(_conn);
        tipoImpostoClass = new TipoImpostoClass(tipoImpostoDAO, aliquotaDAO);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!Page.IsPostBack)
        {
            Title += "Cadastro/Edição Tipos de Imposto";
            subTitulo.Text += "Cadastro/Edição Tipos de Imposto";
        }

        if (!_cadastro)
        {
            if (!Page.IsPostBack)
            {
                tipoImposto = Request.QueryString["id"];
                STipoImposto tipo = tipoImpostoDAO.load(tipoImposto, SessionView.EmpresaSession);
                if (tipo != null)
                {
                    textDescricao.Text = tipo.descricao;
                    textTipoImposto.Text = tipo.tipoImposto;
                    textTipoImposto.Enabled = false;

                    aliquotas = aliquotaDAO.list(tipo.tipoImposto, SessionView.EmpresaSession);
                }
                montaGrid();
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        if (_cadastro)
        {
            STipoImposto tipo = new STipoImposto();
            tipo.descricao = textDescricao.Text;
            tipo.tipoImposto = textTipoImposto.Text;
            tipo.codEmpresa = SessionView.EmpresaSession;
            tipoImpostoClass.novo(tipo, aliquotas);
        }
        else
        {
            STipoImposto tipo = new STipoImposto();
            tipo.descricao = textDescricao.Text;
            tipo.tipoImposto = textTipoImposto.Text;
            tipo.codEmpresa = SessionView.EmpresaSession;
            tipoImpostoClass.editar(tipo, aliquotas);
        }

        Response.Redirect("FormGridTiposImposto.aspx");
    }

    public void LimparForm()
    {
        checkCumulativo.Checked = false;
        textAliquota.Text = "";
        textAliquotaRetencao.Text = "";
    }

    public void montaGrid()
    {
        repeaterDados.DataSource = aliquotas;
        repeaterDados.DataBind();
    }

    protected void botaoInserir_Click(object sender, EventArgs e)
    {
        SAliquotaImposto aliq = new SAliquotaImposto();
        double aliquota = 0;
        double aliquotaRetencao = 0;

        double.TryParse(textAliquota.Text, out aliquota);
        double.TryParse(textAliquotaRetencao.Text, out aliquotaRetencao);
        bool cumulativo = checkCumulativo.Checked;

        aliq.tipoImposto = tipoImposto;
        aliq.cumulativo = cumulativo;
        aliq.aliquota = aliquota;
        aliq.aliquotaRetencao = aliquotaRetencao;
        aliq.codEmpresa = SessionView.EmpresaSession;
        List<SAliquotaImposto> aliquotasTemp = aliquotas;

        aliquotasTemp.Add(aliq);
        aliquotas = aliquotasTemp;

        montaGrid();
        LimparForm();
    }

    protected void botaoDeletar_Click(object sender, EventArgs e)
    {
        LinkButton link = (LinkButton)sender;
        string tipo = link.Attributes["tipoImposto"].ToString();
        bool cumulativo = Convert.ToBoolean(link.Attributes["cumulativo"]);
        List<SAliquotaImposto> aliquotasTemp = aliquotas;
        aliquotasTemp.Remove(aliquotasTemp.Where(a => a.tipoImposto == tipo && a.cumulativo == cumulativo).FirstOrDefault());
        if (!_cadastro)
        {
            aliquotaDAO.delete(tipo, cumulativo, SessionView.EmpresaSession);
        }
        montaGrid();
    }

    protected void botaoCancelar_Click(object sender, EventArgs e)
    {
        LimparForm();
    }
}