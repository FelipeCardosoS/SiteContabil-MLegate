using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public class perfisDAO
{
    private Conexao _conn;

	public perfisDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(string cod_perfil, string descricao)
    {
        string sql = "INSERT INTO CAD_PERFIS(COD_PERFIL,DESCRICAO,COD_EMPRESA)VALUES('" + cod_perfil + "','" + descricao + "'," + HttpContext.Current.Session["empresa"] + ")";
        _conn.execute(sql);
    }

    public void insert(string cod_perfil, string descricao, int cod_empresa)
    {
        string sql = "INSERT INTO CAD_PERFIS(COD_PERFIL,DESCRICAO,COD_EMPRESA)VALUES('" + cod_perfil + "','" + descricao + "'," + cod_empresa + ")";
        _conn.execute(sql);
    }

    public void acessoTotal(string cod_perfil)
    {
        string sql = "INSERT INTO PERFIS_MODULOS(COD_MODULO,COD_PERFIL,COD_EMPRESA)";
        sql += "SELECT COD_MODULO,'" + cod_perfil + "'," + HttpContext.Current.Session["empresa"] + " FROM CAD_MODULOS";
        _conn.execute(sql);

        sql = "INSERT INTO PERFIS_MODULOS_TAREFAS(COD_MODULO,COD_PERFIL,COD_EMPRESA,COD_TAREFA)";
        sql += "SELECT COD_MODULO,'" + cod_perfil + "'," + HttpContext.Current.Session["empresa"] + ",COD_TAREFA FROM CAD_TAREFAS";
        _conn.execute(sql);
    }

    public void acessoTotal(string cod_perfil, int cod_empresa)
    {
        string sql = "INSERT INTO PERFIS_MODULOS(COD_MODULO,COD_PERFIL,COD_EMPRESA)";
        sql += "SELECT COD_MODULO,'" + cod_perfil + "'," + cod_empresa + " FROM CAD_MODULOS";
        _conn.execute(sql);

        sql = "INSERT INTO PERFIS_MODULOS_TAREFAS(COD_MODULO,COD_PERFIL,COD_EMPRESA,COD_TAREFA)";
        sql += "SELECT COD_MODULO,'" + cod_perfil + "'," + cod_empresa + ",COD_TAREFA FROM CAD_TAREFAS";
        _conn.execute(sql);
    }

    public bool getExigeModelo() 
    {
        string sql = "SELECT CAD_PERFIS.EXIGE_MODELO FROM CAD_USUARIOS, CAD_PERFIS WHERE CAD_USUARIOS.COD_USUARIO = " + HttpContext.Current.Session["usuario"] + " AND CAD_USUARIOS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CAD_USUARIOS.COD_PERFIL = CAD_PERFIS.COD_PERFIL AND CAD_USUARIOS.COD_EMPRESA = CAD_PERFIS.COD_EMPRESA";
        return Convert.ToBoolean(_conn.scalar(sql));
    }

    public void update(string cod_perfil, string descricao, bool exige_modelo)
    {
        string sql = "UPDATE CAD_PERFIS SET DESCRICAO='" + descricao + "', EXIGE_MODELO='" + exige_modelo + "' WHERE COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public void delete(string cod_perfil)
    {
        string sql = "DELETE FROM CAD_PERFIS WHERE COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }

    public DataTable load(string cod_perfil)
    {
        string sql = "SELECT * FROM CAD_PERFIS WHERE COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "perfil_acesso");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_PERFIS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_PERFIL";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " DESC)  ";
        sql += "AS Row, *  ";
        sql += "    FROM CAD_PERFIS WHERE 1=1 ";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '" + descricao + "%'";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += " ) as vw where 1=1 ";


        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string descricao)
    {
        string sql = "select count(COD_PERFIL) from CAD_PERFIS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '" + descricao + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
