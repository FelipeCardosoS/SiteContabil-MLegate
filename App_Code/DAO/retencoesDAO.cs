using System;
using System.Data;
using System.Web;

public class retencoesDAO
{
    private Conexao _conn;

	public retencoesDAO(Conexao c)
	{
        _conn = c;
	}

    public int totalRegistros(string nome, Nullable<double> aliquota, string apresentacao, Nullable<int> emitente)
    {
        string sql = "SELECT COUNT(CR.COD_RETENCAO) FROM CAD_RETENCOES CR";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
        {
            sql += ", CAD_RETENCOES_EMITENTE CRE WHERE CR.COD_RETENCAO = CRE.COD_RETENCAO";
            sql += " AND CRE.COD_EMITENTE = " + emitente.Value;
        }

        if (nome != null)
            sql += " AND CR.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (aliquota != null)
            sql += " AND CR.ALIQUOTA LIKE '%" + aliquota.ToString().Replace(",", ".") + "%'";

        if (apresentacao != null)
            sql += " AND CR.APRESENTACAO LIKE '%" + apresentacao.Replace("'", "''") + "%'";

        sql += " AND CR.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaPaginada(ref DataTable tb, string nome, Nullable<double> aliquota, string apresentacao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";

        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CR.COD_RETENCAO DESC";

        string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS ROW, CR.*, dbo.Emitentes_Retencoes(CR.COD_RETENCAO) AS EMITENTE FROM CAD_RETENCOES CR";

        if (emitente == null)
            sql += " WHERE 1=1";
        else
            sql += ", CAD_RETENCOES_EMITENTE CRE WHERE CR.COD_RETENCAO = CRE.COD_RETENCAO AND CRE.COD_EMITENTE = " + emitente.Value;

        if (nome != null)
            sql += " AND CR.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (aliquota != null)
            sql += " AND CR.ALIQUOTA LIKE '%" + aliquota.ToString().Replace(",", ".") + "%'";

        if (apresentacao != null)
            sql += " AND CR.APRESENTACAO LIKE '%" + apresentacao.Replace("'", "''") + "%'";

        sql += " AND CR.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        //Paginação
        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }
    
    public void Load_Sys_Retencoes(ref DataTable tb)
    {
        string sql = "select * from System_Retencoes";

        _conn.fill(sql, ref tb);
    }

    public DataTable load(int cod_retencao)
    {
        string sql = "SELECT NOME, ALIQUOTA, APRESENTACAO, Cod_Retencoes_Sys FROM CAD_RETENCOES WHERE COD_RETENCAO = " + cod_retencao;
        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "retencao");
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb, int cod_retencao)
    {
        string sql = "SELECT CRE.COD_EMITENTE, CE.NOME_RAZAO_SOCIAL FROM CAD_RETENCOES_EMITENTE CRE, CAD_EMPRESAS CE";
        sql += " WHERE CRE.COD_EMITENTE = CE.COD_EMPRESA AND CRE.COD_RETENCAO = " + cod_retencao + " ORDER BY CE.NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string aliquota, string apresentacao, int Cod_Retencoes_Sys)
    {
        string sql = "INSERT INTO CAD_RETENCOES(COD_EMPRESA, NOME, ALIQUOTA, APRESENTACAO, Cod_Retencoes_Sys) VALUES("
            + HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".") + ", '" + apresentacao.Replace("'", "''") + "', "+ Cod_Retencoes_Sys
            + "); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_retencao, string nome, string aliquota, string apresentacao, int Cod_Retencoes_Sys)
    {
        string sql = "UPDATE CAD_RETENCOES SET NOME = '" + nome.Replace("'", "''") + "', ALIQUOTA = " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".") 
            + ", APRESENTACAO = '" + apresentacao.Replace("'", "''") + ", Cod_retencao_Sys = " + Cod_Retencoes_Sys
            + "' WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_retencao)
    {
        string sql = "DELETE FROM CAD_RETENCOES WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_retencao, int cod_emitente)
    {
        string sql = "INSERT INTO CAD_RETENCOES_EMITENTE(COD_RETENCAO, COD_EMITENTE) VALUES(" + cod_retencao + ", " + cod_emitente + ")";
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_retencao, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_RETENCOES_EMITENTE WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }

    public void lista_Retencoes(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT CR.COD_RETENCAO, CR.NOME, CR.ALIQUOTA, CR.APRESENTACAO ";
        sql += "FROM CAD_RETENCOES CR, CAD_RETENCOES_EMITENTE CRE ";
        sql += "WHERE CR.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CR.COD_RETENCAO = CRE.COD_RETENCAO AND CRE.COD_EMITENTE = " + cod_emitente;
        sql += " ORDER BY CR.NOME";
        _conn.fill(sql, ref tb);
    }
}