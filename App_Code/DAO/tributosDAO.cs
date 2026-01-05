using System;
using System.Data;
using System.Web;

public class tributosDAO
{
    private Conexao _conn;

	public tributosDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, Nullable<double> aliquota, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CT.COD_TRIBUTO) FROM CAD_TRIBUTOS CT";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
        {
            sql += ", CAD_TRIBUTOS_EMITENTE CTE WHERE CT.COD_TRIBUTO = CTE.COD_TRIBUTO";
            sql += " AND CTE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CT.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (aliquota != null)
            sql += " AND CT.ALIQUOTA LIKE '%" + aliquota.ToString().Replace(",", ".") + "%'";

        sql += " AND CT.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, Nullable<double> aliquota, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CT.COD_TRIBUTO DESC";

        string sql = "";

        sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CT.*, dbo.Emitentes_Tributos(CT.COD_TRIBUTO) AS EMITENTE FROM CAD_TRIBUTOS CT";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
            sql += ", CAD_TRIBUTOS_EMITENTE CTE WHERE CT.COD_TRIBUTO = CTE.COD_TRIBUTO AND CTE.COD_EMITENTE = " + emitente.Value;

        if (nome != null)
            sql += " AND CT.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (aliquota != null)
            sql += " AND CT.ALIQUOTA LIKE '%" + aliquota.ToString().Replace(",", ".") + "%'";

        sql += " AND CT.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public void Load_Sys_Tributos(ref DataTable tb)
    {
        string sql = "select * from System_Tributos";

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_tributo)
    {
        string sql = "SELECT * FROM CAD_TRIBUTOS WHERE COD_TRIBUTO = " + cod_tributo;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "tributo");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_tributo)
    {
        string sql = "SELECT CTE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_TRIBUTOS_EMITENTE CTE, CAD_EMPRESAS CE";
        sql += " WHERE CTE.COD_EMITENTE = CE.COD_EMPRESA AND CTE.COD_TRIBUTO = " + cod_tributo + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string aliquota, int Cod_Tributos_Sys, bool Destacado)
    {
        string sql = "INSERT INTO CAD_TRIBUTOS(COD_EMPRESA, NOME, ALIQUOTA, Cod_Tributos_Sys, Destacado) VALUES("
            + HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".") + ", " 
            +Cod_Tributos_Sys +", '"+ Destacado +"'); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_tributo, string nome, string aliquota, int Cod_Tributos_Sys, bool Destacado)
    {
        string sql = "UPDATE CAD_TRIBUTOS SET NOME = '" + nome.Replace("'", "''") + "', ALIQUOTA = " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".")
            + ", Cod_Tributos_Sys = " + Cod_Tributos_Sys+ ", Destacado = '" + Destacado + "' "
            + " WHERE COD_TRIBUTO = " + cod_tributo + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_tributo)
    {
        string sql = "DELETE FROM CAD_TRIBUTOS WHERE COD_TRIBUTO = " + cod_tributo + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_tributo, int cod_emitente, int Cod_Servico)
    {
        string sql = "INSERT INTO CAD_TRIBUTOS_EMITENTE(COD_TRIBUTO, COD_EMITENTE, Cod_Servico) VALUES(" + cod_tributo + ", " + cod_emitente + ", "+ Cod_Servico + ")";
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_tributo, int cod_emitente)
    {
        string SQL = "DELETE FROM CAD_TRIBUTOS_EMITENTE WHERE COD_TRIBUTO = " + cod_tributo + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(SQL);
    }

    public DataTable lista_Tributos_EmissaoNF(int cod_emitente, string COD_SERVICO)
    {
        string sql = "SELECT DISTINCT CT.COD_TRIBUTO, CT.NOME, CT.ALIQUOTA, CTE.COD_SERVICO";
        sql += " FROM CAD_TRIBUTOS CT, CAD_TRIBUTOS_EMITENTE CTE";
        sql += " WHERE CT.COD_TRIBUTO = CTE.COD_TRIBUTO";
        sql += " AND CT.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        sql += " AND CTE.COD_EMITENTE = " + cod_emitente;
        sql += " AND CTE.COD_SERVICO IN (" + COD_SERVICO + ")";
        sql += " ORDER BY CT.COD_TRIBUTO";

        return _conn.dataTable(sql, "tributo");
    }

    public void lista_Tributos(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT DISTINCT CT.COD_TRIBUTO, CT.NOME, CT.ALIQUOTA ";
        sql += "FROM CAD_TRIBUTOS CT, CAD_TRIBUTOS_EMITENTE CTE ";
        sql += "WHERE CT.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CT.COD_TRIBUTO = CTE.COD_TRIBUTO AND CTE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CT.NOME";
        _conn.fill(sql, ref tb);
    }
}