using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public class usuariosDAO
{
    private Conexao _conn;

	public usuariosDAO(Conexao c)
	{
        _conn = c;
	}

    public DataTable auth(string LOGIN, int COD_EMPRESA)
    {
        return _conn.dataTable("select CAD_USUARIOS.*, CAD_EMPRESAS.*, CAD_PERFIS.EXIGE_MODELO as EXIGE_MODELO from CAD_USUARIOS, CAD_EMPRESAS, CAD_PERFIS WHERE CAD_USUARIOS.COD_PERFIL = CAD_PERFIS.COD_PERFIL AND CAD_USUARIOS.COD_EMPRESA = CAD_PERFIS.COD_EMPRESA and CAD_EMPRESAS.COD_EMPRESA = " + COD_EMPRESA + " and  CAD_USUARIOS.COD_EMPRESA=" + COD_EMPRESA + " and CAD_USUARIOS.LOGIN='" + LOGIN + "'", "USUARIO_AUTH");
    }

    public void delete(int COD_USUARIO)
    {
        _conn.execute("delete from CAD_USUARIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and COD_USUARIO=" + COD_USUARIO);
    }

    public bool existe(string LOGIN)
    {
        int result = Convert.ToInt32(_conn.scalar("select count(COD_USUARIO) from CAD_USUARIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and LOGIN='" + LOGIN + "'"));
        if (result == 0)
            return false;
        else
            return true;
    }

    public bool igual(int COD_USUARIO, string LOGIN)
    {
        int result = Convert.ToInt32(_conn.scalar("select count(COD_USUARIO) from CAD_USUARIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and LOGIN='" + LOGIN + "' and COD_USUARIO=" + COD_USUARIO));
        if (result == 0)
            return false;
        else
            return true;
    }

    public void insert(string nome, string LOGIN, string senha, string perfil)
    {
        _conn.execute("insert into CAD_USUARIOS(NOME_COMPLETO,LOGIN,SENHA,COD_PERFIL,COD_EMPRESA)values('" + nome + "','" + LOGIN + "','" + senha + "','" + perfil + "'," + HttpContext.Current.Session["empresa"] + ")");
    }

    public void insert(string nome, string LOGIN, string senha, string perfil, int cod_empresa)
    {
        _conn.execute("insert into CAD_USUARIOS(NOME_COMPLETO,LOGIN,SENHA,COD_PERFIL,COD_EMPRESA)values('" + nome + "','" + LOGIN + "','" + senha + "','" + perfil + "'," + cod_empresa + ")");
    }

    public DataTable load(int COD_USUARIO)
    {
        return _conn.dataTable("select * from CAD_USUARIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and COD_USUARIO=" + COD_USUARIO, "USUARIOS");
    }

    public DataTable modulosDisponiveis(string codigo_perfil)
    {
        return _conn.dataTable("SELECT        CAD_MODULOS.* " +
                            "FROM            perfis_modulos, CAD_MODULOS " +
                             "WHERE COD_EMPRESA='"+HttpContext.Current.Session["empresa"]+"'        and perfis_modulos.cod_modulo = CAD_MODULOS.cod_modulo AND (perfis_modulos.cod_perfil = '" + codigo_perfil + "') order by cad_modulos.ordem, cad_modulos.cod_modulo_pai","MODULOS_DISPONIVEIS");
    }

    public DataTable tarefasDisponiveis(string codigo_modulo, string codigo_perfil)
    {
        return _conn.dataTable("SELECT        CAD_TAREFAS.* " +
                            "FROM            CAD_TAREFAS, PERFIS_MODULOS_TAREFAS " +
                            "WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "        and CAD_TAREFAS.COD_tarefa = PERFIS_MODULOS_TAREFAS.COD_tarefa " +
                            "AND (PERFIS_MODULOS_TAREFAS.cod_modulo = '" + codigo_modulo + "') AND (PERFIS_MODULOS_TAREFAS.cod_perfil = '" + codigo_perfil + "')", "TAREFAS_DISPONIVEIS");
    }

    public void update(string nome, string LOGIN, string perfil, int COD_USUARIO)
    {
        _conn.execute("update CAD_USUARIOS set NOME_COMPLETO='" + nome + "', LOGIN='" + LOGIN + "', COD_PERFIL='" + perfil + "' WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and COD_USUARIO=" + COD_USUARIO);
    }

    public void update_senha(string senha, int COD_USUARIO)
    {
        _conn.execute("update CAD_USUARIOS set senha='" + senha + "' WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and COD_USUARIO=" + COD_USUARIO);
    }

    public bool verifica_senha(string senha, int COD_USUARIO)
    {
        int result = Convert.ToInt32(_conn.scalar("select count(COD_USUARIO) from CAD_USUARIOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and senha='" + senha + "' and COD_USUARIO=" + COD_USUARIO));
        if (result > 0)
            return true;
        else
            return false;
    }

    public DataTable lista()
    {
        return _conn.dataTable("select * from CAD_USUARIOS where COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and COD_USUARIO <> " + HttpContext.Current.Session["usuario"],"USUARIOS");
    }

    public void lista(ref DataTable tb, string nome, string perfil, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_USUARIO";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY CAD_USUARIOS." + tmpOrdenacao + " DESC)  ";
        sql += "AS Row, CAD_USUARIOS.*,CAD_PERFIS.DESCRICAO  ";
        sql += "    FROM CAD_USUARIOS, cad_perfis ";
        sql += "    WHERE CAD_USUARIOS.COD_PERFIL = CAD_PERFIS.COD_PERFIL AND CAD_PERFIS.COD_EMPRESA = CAD_USUARIOS.COD_EMPRESA";


        if (nome != null)
            sql += " and CAD_USUARIOS.NOME_COMPLETO like '" + nome + "%'";

        if (perfil != null)
            sql += " and CAD_USUARIOS.cOD_perfil = '" + perfil + "'";

        sql += " and CAD_USUARIOS.COD_USUARIO <> " + HttpContext.Current.Session["usuario"];
        sql += " and CAD_USUARIOS.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "'";

        sql += " ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);
 
        _conn.fill(sql, ref tb);
    }


    public int totalRegistros(string nome ,string perfil)
    {
        string sql = "select count(*) from CAD_USUARIOS where COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        if (nome != null)
            sql += " and CAD_USUARIOS.NOME_COMPLETO like '" + nome + "%'";

        if (perfil != null)
            sql += " and CAD_USUARIOS.cOD_perfil = '" + perfil + "'";

        sql += " and COD_USUARIO <> " + HttpContext.Current.Session["usuario"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

}
