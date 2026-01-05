using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEstrturaDRE : BaseForm
{
    public FormEstrturaDRE() : base("CAD_ESTRUTURA_DRE") 
    {

    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Estrutura DRE";
        }

        if (!Page.IsPostBack)
        {
            
        }
    }

    [WebMethod]
    public static string Salva(EstruturaDRE estruturadre)
    {
        //verificar existe coddre //validar
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        _ContasDREDAO.insert(estruturadre);
        return string.Empty;
    }

    [WebMethod]
    public static string Editar(EstruturaDRE estruturadre)
    {
        //verificar existe coddre //validar
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        _ContasDREDAO.update(estruturadre);
        return string.Empty;
    }

    [WebMethod]
    public static string Deletar(string Cod_DRE)
    {        
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        _ContasDREDAO.delete(Cod_DRE);
        return string.Empty;
    }

    [WebMethod]
    public static List<EstruturaDRE> CarregaEstruturaDRE()
    {
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        return _ContasDREDAO.CarregaEstruturaDRE();
    }

    [WebMethod]
    public static string Up(string Cod_DRE)
    {
        //verificar existe coddre //validar
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        _ContasDREDAO.Up(Cod_DRE);
        return string.Empty;
    }

    [WebMethod]
    public static string Down(string Cod_DRE)
    {
        //verificar existe coddre //validar
        Conexao _conn = new Conexao();
        ContasDREDAO _ContasDREDAO = new ContasDREDAO(_conn);
        _ContasDREDAO.Down(Cod_DRE);
        return string.Empty;
    }
}