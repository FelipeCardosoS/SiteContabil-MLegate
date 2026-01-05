using System;
using System.Data;
using System.Web;

public class prestacaoServicosDAO
{
    private Conexao _conn;

	public prestacaoServicosDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, string descricao, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CPS.COD_PRESTACAO_SERVICO) FROM CAD_PRESTACAO_SERVICOS CPS";

        if (emitente == null)
        {
            sql += " WHERE 1=1";
        }
        else
        {
            sql += ", CAD_PRESTACAO_SERVICOS_EMITENTE CPSE WHERE CPS.COD_PRESTACAO_SERVICO = CPSE.COD_PRESTACAO_SERVICO";
            sql += " AND CPSE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CPS.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CPS.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += " AND CPS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, string descricao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CPS.COD_PRESTACAO_SERVICO DESC";

        string sql = "";

        sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CPS.*, dbo.Emitentes_Prestacao_Servicos(CPS.COD_PRESTACAO_SERVICO) AS EMITENTE FROM CAD_PRESTACAO_SERVICOS CPS";

        if (emitente == null)
        {
            sql += " WHERE 1=1";
        }
        else
        {
            sql += ", CAD_PRESTACAO_SERVICOS_EMITENTE CPSE WHERE CPS.COD_PRESTACAO_SERVICO = CPSE.COD_PRESTACAO_SERVICO AND CPSE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CPS.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CPS.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += " AND CPS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_prestacao_servico)
    {
        string sql = "SELECT NOME, DESCRICAO FROM CAD_PRESTACAO_SERVICOS WHERE COD_PRESTACAO_SERVICO = " + cod_prestacao_servico;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "prestacao_servico");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_prestacao_servico)
    {
        string sql = "SELECT CPSE.PADRAO, CPSE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_PRESTACAO_SERVICOS_EMITENTE CPSE, CAD_EMPRESAS CE";
        sql += " WHERE CPSE.COD_EMITENTE = CE.COD_EMPRESA AND CPSE.COD_PRESTACAO_SERVICO = " + cod_prestacao_servico + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string descricao)
    {
        string sql = "INSERT INTO CAD_PRESTACAO_SERVICOS(COD_EMPRESA, NOME, DESCRICAO) VALUES(";
        sql += HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', '" + descricao.Replace("'", "''") + "'); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_prestacao_servico, string nome, string descricao)
    {
        string sql = "UPDATE CAD_PRESTACAO_SERVICOS SET NOME = '" + nome.Replace("'", "''") + "', DESCRICAO = '" + descricao.Replace("'", "''");
        sql += "' WHERE COD_PRESTACAO_SERVICO = " + cod_prestacao_servico + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_prestacao_servico)
    {
        string sql = "DELETE FROM CAD_PRESTACAO_SERVICOS WHERE COD_PRESTACAO_SERVICO = " + cod_prestacao_servico + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_prestacao_servico, int cod_emitente, bool padrao)
    {
        string sql = "INSERT INTO CAD_PRESTACAO_SERVICOS_EMITENTE(COD_PRESTACAO_SERVICO, COD_EMITENTE, PADRAO) VALUES(" + cod_prestacao_servico + ", " + cod_emitente + ", '" + padrao + "')";
        _conn.execute(sql);
    }

    public void update_Emitentes_Padrao(int cod_prestacao_servico, int cod_emitente, bool padrao)
    {
        string sql = "UPDATE CAD_PRESTACAO_SERVICOS_EMITENTE SET PADRAO = '" + padrao + "' WHERE COD_PRESTACAO_SERVICO = " + cod_prestacao_servico + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_prestacao_servico, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_PRESTACAO_SERVICOS_EMITENTE WHERE COD_PRESTACAO_SERVICO = " + cod_prestacao_servico + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void lista_Prestacao_Servicos(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT CPS.COD_PRESTACAO_SERVICO, CPS.NOME, CPS.DESCRICAO, CPSE.PADRAO ";
        sql += "FROM CAD_PRESTACAO_SERVICOS CPS, CAD_PRESTACAO_SERVICOS_EMITENTE CPSE ";
        sql += "WHERE CPS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CPS.COD_PRESTACAO_SERVICO = CPSE.COD_PRESTACAO_SERVICO AND CPSE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CPS.NOME";
        _conn.fill(sql, ref tb);
    }
}