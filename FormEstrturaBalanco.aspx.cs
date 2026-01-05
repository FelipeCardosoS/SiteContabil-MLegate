using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEstrturaBalanco : BaseForm
{
    public FormEstrturaBalanco() : base("CAD_ESTRUTURA_BALANCO") 
    {

    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Estrutura Balanço";
        }

        if (!Page.IsPostBack)
        {

        }
    }

    [WebMethod]
    public static string Salva(EstruturaBalanco estruturabalanco)
    {
        //verificar existe codbalanco //validar
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        _ContasBalancoDAO.insert(estruturabalanco);
        return string.Empty;
    }

    [WebMethod]
    public static string Editar(EstruturaBalanco estruturabalanco)
    {
        //verificar existe codbalanco //validar
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        _ContasBalancoDAO.update(estruturabalanco);
        return string.Empty;
    }

    [WebMethod]
    public static string Deletar(string Cod_Balanco)
    {
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        _ContasBalancoDAO.delete(Cod_Balanco);
        return string.Empty;
    }

    [WebMethod]
    public static List<EstruturaBalanco> CarregaEstruturaBalanco()
    {
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        return _ContasBalancoDAO.CarregaEstruturaBalanco();
    }

    [WebMethod]
    public static string Up(string Cod_Balanco)
    {
        //verificar existe codbalanco //validar
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        _ContasBalancoDAO.Up(Cod_Balanco);
        return string.Empty;
    }

    [WebMethod]
    public static string Down(string Cod_Balanco)
    {
        //verificar existe codbalanco //validar
        Conexao _conn = new Conexao();
        ContasBalancoDAO _ContasBalancoDAO = new ContasBalancoDAO(_conn);
        _ContasBalancoDAO.Down(Cod_Balanco);
        return string.Empty;
    }
}