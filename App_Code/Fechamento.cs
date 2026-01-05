using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

public class Fechamento
{
    private fechamentoDAO DAO;
    private saldosContabDAO saldosDAO;
    private List<string> erros;

	public Fechamento(Conexao c)
	{
        DAO = new fechamentoDAO(c);
        saldosDAO = new saldosContabDAO(c);
	}


    public List<string> novo(string periodo)
    {
        erros = new List<string>();

        if (periodo == "" || periodo == null)
            erros.Add("Informe o período para realizar o fechamento.");

        if (erros.Count == 0)
        {
            DAO.insert(periodo);
        }

        return erros;
    }

    public List<string> geraSaldos(string periodo)
    {
        erros = new List<string>();

        if (periodo == "" || periodo == null)
            erros.Add("Informe o período para realizar o fechamento.");

        if (erros.Count == 0)
        {
            string[] arrPeriodo = periodo.Split('/');
            DateTime p = new DateTime(Convert.ToInt32(arrPeriodo[1]), Convert.ToInt32(arrPeriodo[0]), 1);
            DateTime ultimoAnterior = new DateTime();
            DateTime primeiroPeriodo = new DateTime();
            DateTime primeiroProximoMes = new DateTime();
            DateTime ultimoPeriodo = new DateTime();


            ultimoAnterior = p.AddDays(-1);
            primeiroProximoMes = p.AddMonths(1);
            primeiroPeriodo = ultimoAnterior.AddDays(1);
            ultimoPeriodo = primeiroProximoMes.AddDays(-1);

            saldosDAO.delete(ultimoPeriodo.ToString("yyyyMMdd"));
            saldosDAO.gera_saldos(primeiroPeriodo.ToString("yyyyMMdd"), ultimoPeriodo.ToString("yyyyMMdd"),
                                    ultimoAnterior.ToString("yyyyMMdd"));
        }

        return erros;
    }

    public List<string> deletar(string periodo)
    {
        erros = new List<string>();

        if (periodo == "" || periodo == null)
            erros.Add("Informe o período para realizar o fechamento.");

        if (erros.Count == 0)
        {
            string[] arrPeriodo = periodo.Split('/');
            DateTime p = new DateTime(Convert.ToInt32(arrPeriodo[1]), Convert.ToInt32(arrPeriodo[0]), 1);
            DateTime primeiroProximoMes = new DateTime();
            primeiroProximoMes = p.AddMonths(1);
            DAO.delete(periodo);
        }

        return erros;
    }

    public void listaPaginada(ref DataTable tb, int paginaAtual, string ordenacao)
    {
        DAO.lista(ref tb, paginaAtual, ordenacao);
    }

    public int totalRegistros()
    {
        return DAO.totalRegistros();
    }

}
