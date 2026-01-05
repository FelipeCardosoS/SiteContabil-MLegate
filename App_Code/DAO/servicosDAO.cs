using System;
using System.Data;
using System.Web;

public class servicosDAO
{
    private Conexao _conn;

	public servicosDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, string cod_servico_prefeitura, Nullable<double> impostos, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CS.COD_SERVICO) FROM CAD_SERVICOS CS";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
        {
            sql += ", CAD_SERVICOS_EMITENTE CSE WHERE CS.COD_SERVICO = CSE.COD_SERVICO";
            sql += " AND CSE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CS.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (cod_servico_prefeitura != null)
            sql += " AND CS.COD_SERVICO_PREFEITURA LIKE '%" + cod_servico_prefeitura.Replace("'", "''") + "%'";

        if (impostos != null)
            sql += " AND CS.IMPOSTOS LIKE '%" + impostos.ToString().Replace(",", ".") + "%'";

        sql += " AND CS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, string cod_servico_prefeitura, Nullable<double> impostos, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CS.COD_SERVICO DESC";

        string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CS.*, dbo.Emitentes_Servicos(CS.COD_SERVICO) AS EMITENTE FROM CAD_SERVICOS CS";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
            sql += ", CAD_SERVICOS_EMITENTE CSE WHERE CS.COD_SERVICO = CSE.COD_SERVICO AND CSE.COD_EMITENTE = " + emitente.Value;

        if (nome != null)
            sql += " AND CS.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (cod_servico_prefeitura != null)
            sql += " AND CS.COD_SERVICO_PREFEITURA LIKE '%" + cod_servico_prefeitura.Replace("'", "''") + "%'";

        if (impostos != null)
            sql += " AND CS.IMPOSTOS LIKE '%" + impostos.ToString().Replace(",", ".") + "%'";

        sql += " AND CS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_servico)
    {
        string sql = "SELECT NOME, COD_SERVICO_PREFEITURA, IMPOSTOS FROM CAD_SERVICOS WHERE COD_SERVICO = " + cod_servico;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "servico");
    }

    public DataTable List_ServicoEmitente(int Cod_Tributo, int Cod_Emitente)
    {
        string sql = "SELECT COD_SERVICO FROM CAD_TRIBUTOS_EMITENTE WHERE Cod_Tributo = " + Cod_Tributo;
        sql += " AND Cod_Emitente = " + Cod_Emitente;

        return _conn.dataTable(sql, "servico");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_servico)
    {
        string sql = "SELECT CSE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_SERVICOS_EMITENTE CSE, CAD_EMPRESAS CE";
        sql += " WHERE CSE.COD_EMITENTE = CE.COD_EMPRESA AND CSE.COD_SERVICO = " + cod_servico + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string cod_servico_prefeitura, double impostos)
    {
        string sql = "INSERT INTO CAD_SERVICOS (COD_EMPRESA, NOME, COD_SERVICO_PREFEITURA, IMPOSTOS) VALUES (";
        sql += HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', '" + cod_servico_prefeitura.Replace("'", "''") + "', ";
        sql += impostos.ToString().Replace(",", ".") + "); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_servico, string nome, string cod_servico_prefeitura, double impostos)
    {
        string sql = "UPDATE CAD_SERVICOS SET NOME = '" + nome.Replace("'", "''") + "', COD_SERVICO_PREFEITURA = '" + cod_servico_prefeitura.Replace("'", "''") + "', ";
        sql += "IMPOSTOS = " + impostos.ToString().Replace(",", ".") + " WHERE COD_SERVICO = " + cod_servico + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_servico)
    {
        string sql = "DELETE FROM CAD_SERVICOS WHERE COD_SERVICO = " + cod_servico + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_servico, int cod_emitente)
    {
        string sql = "INSERT INTO CAD_SERVICOS_EMITENTE (COD_SERVICO, COD_EMITENTE) VALUES (" + cod_servico + ", " + cod_emitente + ")";
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_servico, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_SERVICOS_EMITENTE WHERE COD_SERVICO = " + cod_servico + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void lista_Servicos(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT CS.COD_SERVICO, CS.NOME ";
        sql += "FROM CAD_SERVICOS CS, CAD_SERVICOS_EMITENTE CSE ";
        sql += "WHERE CS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CS.COD_SERVICO = CSE.COD_SERVICO AND CSE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CS.NOME";
        _conn.fill(sql, ref tb);
    }

    public void Load_Servicos(ref DataTable tb)
    {
        string sql = "SELECT COD_SERVICO, NOME "
        + "FROM CAD_SERVICOS WHERE COD_EMPRESA = " 
        + HttpContext.Current.Session["empresa"] + " ORDER BY NOME";
        _conn.fill(sql, ref tb);
    }

    public void Load_Servicos(ref DataTable tb, int Cod_Emitente)
    {
        string sql = "select s.COD_SERVICO, nome from CAD_SERVICOS_emitente as se, CAD_SERVICOS as s where"
            +" s.COD_SERVICO = se.COD_SERVICO and cod_emitente = "+Cod_Emitente+" and s.COD_EMPRESA = "
            + HttpContext.Current.Session["empresa"] +" ORDER BY NOME";
        _conn.fill(sql, ref tb);
    }
}