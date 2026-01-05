using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de ConsultorJobDAO
/// </summary>
public class ConsultorJobDAO
{
	private Conexao _conn;
	public ConsultorJobDAO(Conexao c)
	{
		_conn = c;
	}

	public DataTable lista(int codJob)
	{
		string sql = "SELECT COD_CONSULTOR_JOB, COD_CONSULTOR, COD_JOB, COD_APROVADOR, COD_APROVADOR_RDV, TAXA_CONSULTOR, CUSTO_INTRADIVISAO, CONTATO_ALOCACAO, TELEFONE_CONTATO_ALOCACAO, EMAIL_CONTATO_ALOCACAO, ENVIAR_EMAIL_APROVADOR, DATA_INICIO, DATA_FIM, COD_EMPRESA FROM CONSULTOR_JOBS WHERE COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		return _conn.dataTable(sql, "CONSULTOR_JOBS");
	}

	public void salva(int codJob, List<ConsultorJob> listaConsultorJob)
	{
		if(listaConsultorJob.Count == 0)
			return;
		
		
		string sql = "";
		foreach (ConsultorJob consultorJob in listaConsultorJob)
		{
			if(consultorJob.CodConsultorJob == 0)
			{
				sql += "INSERT INTO CONSULTOR_JOBS (COD_CONSULTOR, COD_JOB, COD_APROVADOR, COD_APROVADOR_RDV, TAXA_CONSULTOR, ";
				sql += "CUSTO_INTRADIVISAO, CONTATO_ALOCACAO, TELEFONE_CONTATO_ALOCACAO, EMAIL_CONTATO_ALOCACAO, ";
				sql += "ENVIAR_EMAIL_APROVADOR, DATA_INICIO, DATA_FIM, COD_EMPRESA) VALUES (";
				sql += consultorJob.CodConsultor + ", " + codJob + ", " + consultorJob.CodAprovador + ", " + consultorJob.CodAprovadorRDV;
				sql += ", " + Convert.ToString(consultorJob.TaxaConsultor).Replace(",", ".") + ", " + Convert.ToString(consultorJob.CustoIntraDivisao).Replace(",", ".");
				sql += ", '" + consultorJob.ContatoAlocacao.Trim().Replace("'", "''").Replace("\"", "\"\"");
				sql += "', '" + consultorJob.TelefoneContato.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', '";
				sql += consultorJob.EmailContato.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', " + Convert.ToInt32(consultorJob.EnviarEmailAprovador) + ", '";
				sql += consultorJob.DataInicio.ToString("dd-MM-yyyy") + "', '" + consultorJob.DataFim.ToString("dd-MM-yyyy") + "', ";
				sql += HttpContext.Current.Session["empresa"] + ");";
			}
			else
			{
				sql += "UPDATE CONSULTOR_JOBS SET COD_CONSULTOR = " + consultorJob.CodConsultor + ", COD_APROVADOR = ";
				sql += consultorJob.CodAprovador + ", COD_APROVADOR_RDV = " + consultorJob.CodAprovadorRDV + ", TAXA_CONSULTOR = " + consultorJob.TaxaConsultor;
				sql += ", CUSTO_INTRADIVISAO = " + consultorJob.CustoIntraDivisao +", CONTATO_ALOCACAO = '";
				sql += consultorJob.ContatoAlocacao.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', TELEFONE_CONTATO_ALOCACAO = '";
				sql += consultorJob.TelefoneContato.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', EMAIL_CONTATO_ALOCACAO = '";
				sql += consultorJob.EmailContato.Trim().Replace("'", "''").Replace("\"", "\"\"") + "', ENVIAR_EMAIL_APROVADOR = ";
				sql += Convert.ToInt32(consultorJob.EnviarEmailAprovador) + ", DATA_INICIO = '" + consultorJob.DataInicio.ToString("dd-MM-yyyy");
				sql += "', DATA_FIM = '" + consultorJob.DataFim.ToString("dd-MM-yyyy") + "'";
				sql += " WHERE COD_CONSULTOR_JOB = " + consultorJob.CodConsultorJob + " AND COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
				sql += ";";
			}
		}

		_conn.execute(sql);
	}

	public void deleta(int codJob)
	{
		string sql = "DELETE FROM CONSULTOR_JOBS WHERE COD_JOB = " + codJob + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}

	public void deleta(int codJob, List<int> listaDeletar)
	{
		if (listaDeletar == null || listaDeletar.Count == 0)
			return;
		string sql = "DELETE FROM CONSULTOR_JOBS WHERE COD_JOB = " + codJob + " AND COD_CONSULTOR_JOB IN (" + String.Join(", ", listaDeletar) + ") AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

		_conn.execute(sql);
	}
}