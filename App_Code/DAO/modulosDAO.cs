using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public class modulosDAO
{
    private Conexao _conn;

	public modulosDAO(Conexao c)
	{
        _conn = c;
	}

    public void insertModuloPerfil(string cod_modulo, string cod_perfil)
    {
        string sql = "INSERT INTO PERFIS_MODULOS(COD_MODULO,COD_PERFIL,COD_EMPRESA)";
        sql += "VALUES";
        sql += "('" + cod_modulo + "','" + cod_perfil + "'," + HttpContext.Current.Session["empresa"] + ")";

        _conn.execute(sql);
    }

    public void deleteModulos(string cod_perfil)
    {
        string sql = "DELETE FROM PERFIS_MODULOS WHERE COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ";
        _conn.execute(sql);
    }

    public void deleteModuloPerfil(string cod_modulo, string cod_perfil)
    {
        string sql = "DELETE FROM PERFIS_MODULOS WHERE COD_PERFIL='" + cod_perfil + "' AND COD_MODULO='" + cod_modulo + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ";
        _conn.execute(sql);
    }

    public void insertModuloPerfilTarefa(string cod_modulo, string cod_perfil, int cod_tarefa)
    {
        string sql = "INSERT INTO PERFIS_MODULOS_TAREFAS(COD_MODULO,COD_PERFIL,COD_EMPRESA,COD_TAREFA)";
        sql += "VALUES";
        sql += "('" + cod_modulo + "','" + cod_perfil + "'," + HttpContext.Current.Session["empresa"] + ","+cod_tarefa+")";

        _conn.execute(sql);
    }

    public void deleteModuloPerfilTarefa(string cod_modulo, string cod_perfil, int cod_tarefa)
    {
        string sql = "DELETE FROM PERFIS_MODULOS_TAREFAS WHERE COD_MODULO='" + cod_modulo + "' AND COD_PERFIL='" + cod_perfil + "' AND COD_TAREFA=" + cod_tarefa + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ";
        _conn.execute(sql);
    }

    public void listaModulosPerfil(ref DataTable tb, string cod_perfil)
    {
        string sql = "SELECT * FROM PERFIS_MODULOS, CAD_MODULOS WHERE PERFIS_MODULOS.COD_MODULO = CAD_MODULOS.COD_MODULO AND COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY ORDEM,COD_MODULO_PAI ";
        _conn.fill(sql, ref tb);
    }

    public void listaTarefasDisponiveis(ref DataTable tb, string cod_modulo, string cod_perfil)
    {
        string sql = "SELECT * FROM CAD_TAREFAS WHERE " +
                    " COD_MODULO = '"+cod_modulo+"' " +
                    " AND COD_TAREFA NOT IN (SELECT COD_TAREFA FROM PERFIS_MODULOS_TAREFAS " +
                    "                    WHERE COD_MODULO = CAD_TAREFAS.COD_MODULO AND COD_PERFIL='"+cod_perfil+"' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + ")";

        _conn.fill(sql, ref tb);
    }

    public void listaTarefasSelecionadas(ref DataTable tb, string cod_modulo, string cod_perfil)
    {
        string sql = "SELECT * FROM CAD_TAREFAS WHERE " +
                    " COD_MODULO = '" + cod_modulo + "' " +
                    " AND COD_TAREFA IN (SELECT COD_TAREFA FROM PERFIS_MODULOS_TAREFAS " +
                    "                    WHERE COD_MODULO = CAD_TAREFAS.COD_MODULO AND COD_PERFIL='" + cod_perfil + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + ")";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_MODULOS ORDER BY ORDEM,COD_MODULO_PAI ";
        _conn.fill(sql, ref tb);
    }
}
