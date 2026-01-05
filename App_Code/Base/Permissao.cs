using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

public class Permissao : System.Web.UI.Page
{
    protected Usuario _usuario;
    protected List<SModulo> _modulos = new List<SModulo>();
    protected List<STarefa> _tarefas = new List<STarefa>();
    protected Conexao _conn;
    private string _modulo;
    private string _moduloAnterior;

    public string modulo
    {
        get { return _modulo; }
        set { _modulo = value; }
    }

    public string moduloAnterior
    {
        get { return _moduloAnterior; }
        set { _moduloAnterior = value; }
    }

	public Permissao(string modulo)
	{
        _modulo = modulo;
        _conn = new Conexao();
        _usuario = new Usuario(_conn);
	}
    
    protected virtual void Page_UnLoad(object sender, EventArgs e)
    {
        _moduloAnterior = modulo;
        _conn.close();
    }

    protected virtual void Page_PreLoad(object sender, EventArgs e)
    {
        verifica();
    }

    private bool analisaChave()
    {
        //return GeraChave();
        if (Session["empresa"] == null)
        {
            Session.Add("retorno_url_admin", HttpContext.Current.Request.RawUrl);
            Response.Redirect("Auth.aspx", true);
        }

        controleChaveDAO controleChaveDAO = new controleChaveDAO(_conn);
        DataTable chave = controleChaveDAO.buscaPeriodo(DateTime.Now.Month, DateTime.Now.Year);
        if (chave.Rows.Count > 0)
        {
            string strChave = chave.Rows[0]["chave"].ToString();

            Empresa empresa = new Empresa(_conn);
            empresa.codigo = Convert.ToInt32(Session["empresa"]);
            empresa.load();

            string cnpj = empresa.cnpjCpf;
            long oitoDigitos = long.Parse(cnpj.Substring(0, 8));
            long numeroPeriodo = long.Parse((DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()));

            long primeiroValor = numeroPeriodo * oitoDigitos;
            long restoDivisao = 0;
            Math.DivRem(primeiroValor, 11, out restoDivisao);
            if (restoDivisao == 0)
                restoDivisao = 1;
            long segundoValor = long.Parse((Math.Floor(Convert.ToDecimal(primeiroValor) / 11) * restoDivisao).ToString());
            long terceiroValor = 0;
            Math.DivRem(segundoValor, 97, out terceiroValor);
            string chaveNova = "";

            for (int i = 0; i < cnpj.Length-1; i++)
            {
                long numeroCnpj = long.Parse(cnpj.Substring(i, 1));
                long result = 0;
                Math.DivRem((terceiroValor * numeroCnpj) * i, 91, out result);
                if (result == 0)
                    result = 48 + i;

                if (result < 48)
                {
                    result += 42;
                }
                
                char ascii = (char)result;
                chaveNova += ascii.ToString();
            }

            return (chaveNova == strChave);

        }
        return false;
    }

    private bool GeraChave()
    {
        if (Session["empresa"] == null)
        {
            Session.Add("retorno_url_admin", HttpContext.Current.Request.RawUrl);
            Response.Redirect("Auth.aspx", true);
        }

        String Strsql = "select * from cad_empresas where cod_empresa_pai = 0 ";

        DataTable Empresas = _conn.dataTable(Strsql, "Empresas");
        foreach (DataRow row in Empresas.Rows)
        {
            string cod_Empresa = row["cod_empresa"].ToString();

            Empresa empresa = new Empresa(_conn);
            empresa.codigo = Convert.ToInt32(cod_Empresa);
            empresa.load();

            string cnpj = empresa.cnpjCpf;
            if (cnpj.Length < 9)
            {
                cnpj = cnpj.Trim() + "000000000000000000000000";
                cnpj = cnpj.Substring(0, 14);
            }
            for (int j = 1; j < 13; j++)
            {


                long oitoDigitos = long.Parse(cnpj.Substring(0, 8));
                long numeroPeriodo = long.Parse((j.ToString() + DateTime.Now.Year.ToString()));

                long primeiroValor = numeroPeriodo * oitoDigitos;
                long restoDivisao = 0;
                Math.DivRem(primeiroValor, 11, out restoDivisao);
                if (restoDivisao == 0)
                    restoDivisao = 1;
                long segundoValor = long.Parse((Math.Floor(Convert.ToDecimal(primeiroValor) / 11) * restoDivisao).ToString());
                long terceiroValor = 0;
                Math.DivRem(segundoValor, 97, out terceiroValor);
                string chaveNova = "";

                for (int i = 0; i < cnpj.Length - 1; i++)
                {
                    long numeroCnpj = long.Parse(cnpj.Substring(i, 1));
                    long result = 0;
                    Math.DivRem((terceiroValor * numeroCnpj) * i, 91, out result);
                    if (result == 0)
                        result = 48 + i;

                    if (result < 48)
                    {
                        result += 42;
                    }

                    char ascii = (char)result;
                    chaveNova += ascii.ToString();
                }


                Strsql = "delete from controle_chave where mes = " + j + " and ano = " + DateTime.Now.Year.ToString() + " and cod_Empresa = " + cod_Empresa + "";
                _conn.execute(Strsql);

                Strsql = "insert into controle_chave (mes, ano, chave, COD_EMPRESA) values (" + j + "," + DateTime.Now.Year.ToString() + ",'" + chaveNova + "'," + cod_Empresa + ")";
                _conn.execute(Strsql);
            }
        }
        return false;
    }

    public void verifica()
    {
        if (analisaChave())
        {
            if (Session["usuario"] != null)
            {
                if (this._modulo != "DEFAULT")
                {
                    _usuario.id = Convert.ToInt32(Session["usuario"]);
                    _usuario.load();

                    _modulos = _usuario.modulosPermitidos();
                    _tarefas = _usuario.tarefasPermitidas(this._modulo);

                    bool existe = false;

                    for (int i = 0; i < this._modulos.Count; i++)
                    {
                        if (this._modulo.Equals(_modulos[i].codigo))
                            existe = true;
                    }

                    if (!existe){
                        if(this._modulo.Equals("ALT_SENHA") || this._modulo.Equals("USUARIO"))
                            existe = true;
                    }

                    if (!existe)
                        Response.Redirect("AcessoNegado.aspx", true);
                }
                else
                {
                    string id = Session["usuario"].ToString();
                    this._usuario.id = Convert.ToInt32(Session["usuario"]);
                    this._usuario.load();
                    this._modulos = this._usuario.modulosPermitidos();
                    this._tarefas = this._usuario.tarefasPermitidas(this._modulo);
                }
            }
            else
            {
                Session.Add("retorno_url_admin", HttpContext.Current.Request.RawUrl);
                Response.Redirect("Auth.aspx", true);
            }
        }
        else
        {
            Response.Redirect("Licenca.aspx", true);
        }
    }

    protected virtual void verificaTarefas()
    {

    }

    protected virtual void logout()
    {
        Session.Abandon();
        Response.Redirect("Auth.aspx");
    }


    protected string limpaString(string texto)
    {
        return texto.Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("/", "");
    }

    protected void errosFormulario(List<string> erros)
    {
        string texto = "";

        for (int i = 0; i < erros.Count; i++)
            texto += erros[i].ToString() + "\\n";
        
        ScriptManager.RegisterStartupScript(this, GetType(), "alertaErro", "alert('" + texto + "');", true);
    }
}