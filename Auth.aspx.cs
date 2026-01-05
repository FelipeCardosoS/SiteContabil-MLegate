using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Auth : System.Web.UI.Page
{
    private Usuario usuario;
    private Empresa empresa;
    private Conexao conn;
    private DataTable tbEmpresas = new DataTable("tbEmpresas");
    public string recaptcha; //Comentar esta linha para publicar na G5

    public Auth()
    {
        conn = new Conexao();
        usuario = new Usuario(conn);
        empresa = new Empresa(conn);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            recaptcha = ConfigurationManager.AppSettings["recaptcha"].ToString(); //Comentar esta linha para publicar na G5
            comboEmpresa.Items.Insert(0, new ListItem("Selecione..", "0"));
            comboEmpresa.Enabled = false;
        }
    }

    protected void botaoLogar_Click(object sender, EventArgs e)
    {
        usuario.email = textLogin.Text;
        usuario.senha = textSenha.Text;

        if (Convert.ToInt32(comboEmpresa.SelectedValue) == 0)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alerta", "alert('Selecione a empresa para efetuar o login.');", true);
        else
        {
            if (!usuario.auth(Convert.ToInt32(comboEmpresa.SelectedValue)))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerta", "alert('Login e senha informados estão incorretos.');", true);
            else
            {
                string urlRetorno = "";

                if (Session["retorno_url_admin"] != null)
                    urlRetorno = Session["retorno_url_admin"].ToString();
                else
                    urlRetorno = "Default.aspx";

                Session["retorno_url_admin"] = null;
                Response.Redirect(urlRetorno, true);
            }
        }
    }

    [WebMethod]
    public static string Autentica(string login, string senha, int empresa)
    {
        Conexao conn = new Conexao();
        Usuario usuario = new Usuario(conn);
        usuario.email = login;
        usuario.senha = senha;
        string msg = string.Empty;

        if (Convert.ToInt32(empresa) == 0)
        {
            msg = "Selecione a empresa para efetuar o login.";
        }
        else
        {
            if (!usuario.auth(Convert.ToInt32(empresa)))
            {
                msg = "Login e senha informados estão incorretos.";
            }
            else
            {
                string urlRetorno = "";
                if(HttpContext.Current.Session["retorno_url_admin"] != null)
                {
                    urlRetorno = HttpContext.Current.Session["retorno_url_admin"].ToString();
                }
                else
                {
                    urlRetorno = "Default.aspx";
                }
                HttpContext.Current.Session["retorno_url_admin"] = null;
                msg = "/" + urlRetorno;
            }
        }
        return msg;
    }

    protected void textLogin_TextChanged(object sender, EventArgs e)
    {
        comboEmpresa.DataSource = null;
        tbEmpresas.Clear();
        empresa.listaUsuarias(ref tbEmpresas, textLogin.Text);

        if (tbEmpresas.Rows.Count > 0)
        {
            comboEmpresa.DataSource = tbEmpresas;
            comboEmpresa.DataValueField = "COD_EMPRESA";
            comboEmpresa.DataTextField = "NOME_RAZAO_SOCIAL";
            comboEmpresa.DataBind();
            comboEmpresa.Items.Insert(0, new ListItem("Selecione..", "0"));
            comboEmpresa.Enabled = true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CHcomboFornecedor", "alert('O login informado não existe!')", true);
        }
    }
}
