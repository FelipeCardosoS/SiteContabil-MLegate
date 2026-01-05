using System;
using System.Data;
using System.Web;

public class narrativasDAO
{
    private Conexao _conn;

	public narrativasDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, string descricao, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CN.COD_NARRATIVA) FROM CAD_NARRATIVAS CN";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
        {
            sql += ", CAD_NARRATIVAS_EMITENTE CNE WHERE CN.COD_NARRATIVA = CNE.COD_NARRATIVA";
            sql += " AND CNE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CN.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CN.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += " AND CN.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CN.COD_NARRATIVA DESC";

        string sql = "";

        sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CN.*, dbo.Emitentes_Narrativas(CN.COD_NARRATIVA) AS EMITENTE FROM CAD_NARRATIVAS CN";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
            sql += ", CAD_NARRATIVAS_EMITENTE CNE WHERE CN.COD_NARRATIVA = CNE.COD_NARRATIVA AND CNE.COD_EMITENTE = " + emitente.Value;

        if (nome != null)
            sql += " AND CN.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CN.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += " AND CN.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_narrativa)
    {
        string sql = "SELECT NOME, DESCRICAO FROM CAD_NARRATIVAS WHERE COD_NARRATIVA = " + cod_narrativa;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "narrativa");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_narrativa)
    {
        string sql = "SELECT CNE.PADRAO, CNE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_NARRATIVAS_EMITENTE CNE, CAD_EMPRESAS CE";
        sql += " WHERE CNE.COD_EMITENTE = CE.COD_EMPRESA AND CNE.COD_NARRATIVA = " + cod_narrativa + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string descricao)
    {
        string sql = "INSERT INTO CAD_NARRATIVAS(COD_EMPRESA, NOME, DESCRICAO) VALUES(";
        sql += HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', '" + descricao.Replace("'", "''") + "'); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_narrativa, string nome, string descricao)
    {
        string sql = "UPDATE CAD_NARRATIVAS SET NOME = '" + nome.Replace("'", "''") + "', DESCRICAO = '" + descricao.Replace("'", "''");
        sql += "' WHERE COD_NARRATIVA = " + cod_narrativa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_narrativa)
    {
        string sql = "DELETE FROM CAD_NARRATIVAS WHERE COD_NARRATIVA = " + cod_narrativa + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_narrativa, int cod_emitente, bool padrao)
    {
        string sql = "INSERT INTO CAD_NARRATIVAS_EMITENTE(COD_NARRATIVA, COD_EMITENTE, PADRAO) VALUES(" + cod_narrativa + ", " + cod_emitente + ", '" + padrao + "')";
        _conn.execute(sql);
    }

    public void update_Emitentes_Padrao(int cod_narrativa, int cod_emitente, bool padrao)
    {
        string sql = "UPDATE CAD_NARRATIVAS_EMITENTE SET PADRAO = '" + padrao + "' WHERE COD_NARRATIVA = " + cod_narrativa + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_narrativa, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_NARRATIVAS_EMITENTE WHERE COD_NARRATIVA = " + cod_narrativa + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void lista_Narrativas(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT CN.COD_NARRATIVA, CN.NOME, CN.DESCRICAO, CNE.PADRAO ";
        sql += "FROM CAD_NARRATIVAS CN, CAD_NARRATIVAS_EMITENTE CNE ";
        sql += "WHERE CN.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CN.COD_NARRATIVA = CNE.COD_NARRATIVA AND CNE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CN.NOME";
        _conn.fill(sql, ref tb);
    }
}