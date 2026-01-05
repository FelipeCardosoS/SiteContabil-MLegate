using System;
using System.Data;
using System.Configuration;
using System.Web;

public class saldosContabDAO
{
    private Conexao _conn;

	public saldosContabDAO(Conexao c)
	{
        _conn = c;
	}

    public void gera_saldos(string periodoInicio, string periodoTermino, string periodoAnterior)
    {
        string sql = "INSERT INTO SALDOS_CONTAB "+
            " (DEB_CRED,DATA,COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE,VALOR,COD_TERCEIRO,COD_EMPRESA) "+
            " (SELECT 'D', '" + periodoTermino + "' AS DATA,COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE, SUM(case when DEB_CRED = 'D' then VALOR else -valor end) ,COD_TERCEIRO,COD_EMPRESA  " +
		    "  	FROM "+
			" 	(SELECT DEB_CRED, COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE,VALOR ,COD_TERCEIRO,COD_EMPRESA  "+
			" 	FROM SALDOS_CONTAB "+
            " 	WHERE DATA = '"+periodoAnterior+"' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " " +
			" 	UNION "+
			" 	SELECT DEB_CRED, COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE, SUM(VALOR) ,COD_TERCEIRO,COD_EMPRESA  "+
			" 	FROM LANCTOS_CONTAB "+
            " 	WHERE DATA >= '" + periodoInicio + "' AND DATA <= '" + periodoTermino + "' AND PENDENTE = 'FALSE' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " " +
            " 	GROUP BY DEB_CRED, COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE, COD_TERCEIRO,COD_EMPRESA ) X " +
			" 	GROUP BY COD_CONTA,COD_JOB,COD_LINHA_NEGOCIO,COD_DIVISAO,COD_CLIENTE, COD_TERCEIRO,COD_EMPRESA )";

        _conn.execute(sql);

        inverte_sinal();
    }

    public void inverte_sinal()
    {
        string sql = "update SALDOS_CONTAB set DEB_CRED = 'C', valor = -valor where valor < 0 and DEB_CRED = 'D' and COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete(string periodo)
    {
        string sql = "DELETE FROM SALDOS_CONTAB WHERE DATA = '"+periodo+"' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        _conn.execute(sql);
    }
}
