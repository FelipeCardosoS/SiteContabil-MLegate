using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Licenca : System.Web.UI.Page
{
    private Usuario usuario;
    private Empresa empresa;
    private Conexao conn;
    private DataTable tbEmpresas = new DataTable("tbEmpresas");

    public Licenca()
    {
        conn = new Conexao();
        usuario = new Usuario(conn);
        empresa = new Empresa(conn);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            tbEmpresas.Clear();
            empresa.listaUsuarias(ref tbEmpresas);
            comboEmpresa.DataSource = tbEmpresas;
            comboEmpresa.DataValueField = "COD_EMPRESA";
            comboEmpresa.DataTextField = "NOME_RAZAO_SOCIAL";
            comboEmpresa.DataBind();
        }
    }

    protected void botaoSalvar_Click(object sender, EventArgs e)
    {
        controleChaveDAO controleChaveDAO = new controleChaveDAO(conn);
        try
        {
            controleChaveDAO.delete(Convert.ToInt32(comboEmpresa.SelectedValue), Convert.ToInt32(textMes.Text),
                Convert.ToInt32(textAno.Text));
            controleChaveDAO.insert(Convert.ToInt32(comboEmpresa.SelectedValue), Convert.ToInt32(textMes.Text),
                Convert.ToInt32(textAno.Text), textChave.Text);
            Response.Redirect("Default.aspx");
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alerta", "alert('Erro no cadastro da chave de licença.');", true);
        }
    }
}
