using System;
using System.Data;
using System.Web;

public class naturezaOperacaoDAO
{
    private Conexao _conn;

	public naturezaOperacaoDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, string descricao, string natureza_operacao, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CNO.COD_NATUREZA_OPERACAO) FROM CAD_NATUREZA_OPERACAO CNO";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
        {
            sql += ", CAD_NATUREZA_OPERACAO_EMITENTE CNOE WHERE CNO.COD_NATUREZA_OPERACAO = CNOE.COD_NATUREZA_OPERACAO";
            sql += " AND CNOE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CNO.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CNO.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        if (natureza_operacao != null)
            sql += " AND CNO.NATUREZA_OPERACAO LIKE '%" + natureza_operacao.Replace("'", "''") + "%'";

        sql += " AND CNO.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, string natureza_operacao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CNO.COD_NATUREZA_OPERACAO DESC";

        string sql = "";

        sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CNO.*, dbo.Emitentes_Natureza_Operacao(CNO.COD_NATUREZA_OPERACAO) AS EMITENTE FROM CAD_NATUREZA_OPERACAO CNO";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
            sql += ", CAD_NATUREZA_OPERACAO_EMITENTE CNOE WHERE CNO.COD_NATUREZA_OPERACAO = CNOE.COD_NATUREZA_OPERACAO AND CNOE.COD_EMITENTE = " + emitente.Value;

        if (nome != null)
            sql += " AND CNO.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CNO.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        if (natureza_operacao != null)
            sql += " AND CNO.NATUREZA_OPERACAO LIKE '%" + natureza_operacao.Replace("'", "''") + "%'";

        sql += " AND CNO.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_natureza_operacao)
    {
        string sql = "SELECT NOME, DESCRICAO, NATUREZA_OPERACAO FROM CAD_NATUREZA_OPERACAO WHERE COD_NATUREZA_OPERACAO = " + cod_natureza_operacao;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "natureza_operacao");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_natureza_operacao)
    {
        string sql = "SELECT CNOE.PADRAO, CNOE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_NATUREZA_OPERACAO_EMITENTE CNOE, CAD_EMPRESAS CE";
        sql += " WHERE CNOE.COD_EMITENTE = CE.COD_EMPRESA AND CNOE.COD_NATUREZA_OPERACAO = " + cod_natureza_operacao + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string descricao, string natureza_operacao)
    {
        string sql = "INSERT INTO CAD_NATUREZA_OPERACAO(COD_EMPRESA, NOME, DESCRICAO, NATUREZA_OPERACAO) VALUES(";
        sql += HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', '" + descricao.Replace("'", "''") + "', '" + natureza_operacao.Replace("'", "''") + "'); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_natureza_operacao, string nome, string descricao, string natureza_operacao)
    {
        string sql = "UPDATE CAD_NATUREZA_OPERACAO SET NOME = '" + nome.Replace("'", "''") + "', DESCRICAO = '" + descricao.Replace("'", "''") + "', NATUREZA_OPERACAO = '" + natureza_operacao.Replace("'", "''");
        sql += "' WHERE COD_NATUREZA_OPERACAO = " + cod_natureza_operacao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_natureza_operacao)
    {
        string sql = "DELETE FROM CAD_NATUREZA_OPERACAO WHERE COD_NATUREZA_OPERACAO = " + cod_natureza_operacao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_natureza_operacao, int cod_emitente, bool padrao)
    {
        string sql = "INSERT INTO CAD_NATUREZA_OPERACAO_EMITENTE(COD_NATUREZA_OPERACAO, COD_EMITENTE, PADRAO) VALUES(" + cod_natureza_operacao + ", " + cod_emitente + ", '" + padrao + "')";
        _conn.execute(sql);
    }

    public void update_Emitentes_Padrao(int cod_natureza_operacao, int cod_emitente, bool padrao)
    {
        string sql = "UPDATE CAD_NATUREZA_OPERACAO_EMITENTE SET PADRAO = '" + padrao + "' WHERE COD_NATUREZA_OPERACAO = " + cod_natureza_operacao + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_natureza_operacao, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_NATUREZA_OPERACAO_EMITENTE WHERE COD_NATUREZA_OPERACAO = " + cod_natureza_operacao + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void lista_Natureza_Operacao(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT CNO.COD_NATUREZA_OPERACAO, CNO.NOME, CNO.DESCRICAO, CNO.NATUREZA_OPERACAO, CNOE.PADRAO ";
        sql += "FROM CAD_NATUREZA_OPERACAO CNO, CAD_NATUREZA_OPERACAO_EMITENTE CNOE ";
        sql += "WHERE CNO.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CNO.COD_NATUREZA_OPERACAO = CNOE.COD_NATUREZA_OPERACAO AND CNOE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CNO.NOME";
        _conn.fill(sql, ref tb);
    }
}