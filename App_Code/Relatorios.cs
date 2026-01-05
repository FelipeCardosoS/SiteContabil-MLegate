using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class Relatorios
{
    private List<string> erros;
    private relatoriosDAO relatorioDAO;

    public Relatorios(Conexao c)
    {
        relatorioDAO = new relatoriosDAO(c);
    }
    public List<string> titulosPendentes(DateTime periodo, int diasVencimento,string tipo,string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte,string ordenacao, ref DataTable tb)
    {
        erros = new List<string>();
        string ordenacaoNew = null;

        if (ordenacao == "DATA")
            ordenacaoNew = "data";
        else if (ordenacao == "TERCEIRO")
            ordenacaoNew = "cod_terceiro";
        else if (ordenacao == "CONTA")
            ordenacaoNew = "cod_conta";
        else
            ordenacaoNew = null;

        if (periodo == null)
            erros.Add("Período informado é inválido");

        if (erros.Count == 0)
        {
            relatorioDAO.titulosPendentes(periodo, diasVencimento,tipo,contaDe,contaAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe,
                clienteAte, jobDe, jobAte,ordenacaoNew, ref tb);
        }

        return erros;
    }

    public List<string> balancete(Nullable<int> tipoMoeda, DateTime data, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte,string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, string detalhamento5, ref DataTable tb, bool contasEncerramento, string empresas, string textAteEncerramento, bool contasZeradas)
    {
        erros = new List<string>();

        if (data == null)
            erros.Add("Data informada é inválida");

        if (!contasEncerramento && textAteEncerramento == "")
            erros.Add("Data do encerramento inválida!");

        if (erros.Count == 0)
        {
            relatorioDAO.balancete(tipoMoeda, data, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe,
                          clienteAte, jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb, contasEncerramento, empresas, textAteEncerramento, contasZeradas);
        }

        return erros;
    }

    public List<string> dre(Nullable<int> moedaTipo, DateTime periodoDe, DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, string detalhamento5, ref DataTable tb, bool encerramento, string empresas)
    {
        erros = new List<string>();

        if (periodoDe == null && periodoAte==null)
            erros.Add("Data informada é inválida");

        if (erros.Count == 0)
        {
            relatorioDAO.dre(moedaTipo, periodoDe, periodoAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe,
                          clienteAte, jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, detalhamento5, ref tb, encerramento, empresas);
        }

        return erros;
    }

	public List<string> movimentacaoFinanceiraBeta(DateTime periodoDe, DateTime periodoAte, string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
		Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
		Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
		string detalhamento2, string detalhamento3, string detalhamento4, ref DataTable tb)
	{
		erros = new List<string>();

		if (periodoDe == null && periodoAte == null)
			erros.Add("Período não informado ou é inválido.");

		if (erros.Count == 0)
		{
			DateTime periodoAnterior = periodoDe.AddMonths(-1);
			relatorioDAO.movimentacao_financeira_beta(periodoDe, periodoAte, periodoAnterior, contaDe, contaAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe,
						  clienteAte, jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, ref tb);
		}

		return erros;
	}

	public List<string> movimentacaoFinanceira(DateTime periodoDe, DateTime periodoAte,string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, ref DataTable tb, int tipoConta = 1)
    {
        erros = new List<string>();

        if (periodoDe == null && periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (erros.Count == 0)
        {
            DateTime periodoAnterior = periodoDe.AddMonths(-1);
            relatorioDAO.movimentacao_financeira(periodoDe, periodoAte,periodoAnterior,contaDe,contaAte, divisaoDe, divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe,
                          clienteAte, jobDe, jobAte, detalhamento1, detalhamento2, detalhamento3, detalhamento4, ref tb, tipoConta);
        }

        return erros;
    }


    public List<string> movimentacaoBancaria(DateTime periodoDe, DateTime periodoAte, string contaDe, string contaAte, string grupo, ref DataTable tb)
    {
        erros = new List<string>();

        if (periodoDe == null && periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (erros.Count == 0)
        {
            relatorioDAO.movimentacao_bancaria(periodoDe, periodoAte, contaDe, contaAte, grupo, ref tb);
        }

        return erros;
    }



    public List<string> razao(string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, DateTime periodo, ref DataTable tb, 
        string detalhamento1, string detalhamento1Cod,
        string detalhamento2, string detalhamento2Cod, string detalhamento3, string detalhamento3Cod,
        string detalhamento4, string detalhamento4Cod, string detalhamento5, string detalhamento5Cod,
        int cod_moeda
        )
    {
        erros = new List<string>();

        if (periodo == null)
            erros.Add("Período não informado ou é inválido.");

        if (contaDe == null)
            erros.Add("Informe apartir de qual conta deve ser gerado o relatório.");

        if (contaAte == null)
            erros.Add("Informe até qual conta deve ser gerado o relatório.");


        if (erros.Count == 0)
        {
            DateTime periodoAte = periodo.AddMonths(1).AddDays(-1);
            DateTime periodoAnterior = periodo.AddDays(-1);

            relatorioDAO.razao(contaDe, contaAte, periodoAnterior, periodo, periodoAte, divisaoDe, divisaoAte,
                linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, ref tb,detalhamento1, 
                detalhamento1Cod, detalhamento2, detalhamento2Cod, detalhamento3, detalhamento3Cod, detalhamento4, 
                detalhamento4Cod, detalhamento5, detalhamento5Cod, cod_moeda);
        }

        return erros;
    }

    public List<string> razaoSemDrill(string contaDe, string contaAte, DateTime periodoDe, DateTime periodoAte,
        Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte
        ,ref DataTable tb, string detalhamento1,
        string detalhamento2, string detalhamento3,string detalhamento4, string detalhamento5)
    {
        erros = new List<string>();

        if (periodoDe == null)
            erros.Add("Período não informado ou é inválido.");

        if (periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (contaDe == null)
            erros.Add("Informe apartir de qual conta deve ser gerado o relatório.");

        if (contaAte == null)
            erros.Add("Informe até qual conta deve ser gerado o relatório.");


        if (erros.Count == 0)
        {
            DateTime periodoAnterior = periodoDe.AddDays(-1);

            relatorioDAO.razaoSemDrill(contaDe, contaAte, periodoAnterior, periodoDe, periodoAte, divisaoDe,
                divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte,ref tb, 
                detalhamento1,
                                detalhamento2, detalhamento3, detalhamento4, detalhamento5);
        }

        return erros;
    }

    public List<string> razaoSemDrillFiltrado(string contaDe, string contaAte, DateTime periodoDe, DateTime periodoAte, 
        Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, ref DataTable tb,
        string detalhamento1, string detalhamento1Cod,
        string detalhamento2, string detalhamento2Cod, string detalhamento3, string detalhamento3Cod,
        string detalhamento4, string detalhamento4Cod, string detalhamento5, string detalhamento5Cod,
        int cod_moeda
        )
    {
        erros = new List<string>();

        if (periodoDe == null)
            erros.Add("Período não informado ou é inválido.");

        if (periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (contaDe == null)
            erros.Add("Informe apartir de qual conta deve ser gerado o relatório.");

        if (contaAte == null)
            erros.Add("Informe até qual conta deve ser gerado o relatório.");


        if (erros.Count == 0)
        {
            DateTime periodoAnterior = periodoDe.AddDays(-1);

            relatorioDAO.razaoSemDrillFiltrado(contaDe, contaAte, periodoAnterior, periodoDe, periodoAte, divisaoDe,
                divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, ref tb, detalhamento1, detalhamento1Cod,
                detalhamento2, detalhamento2Cod, detalhamento3, detalhamento3Cod, detalhamento4, detalhamento4Cod, detalhamento5, detalhamento5Cod, cod_moeda);
        }

        return erros;
    }

    public List<string> razaoContabil(int tipoMoeda, string contaDe, string contaAte, DateTime periodoDe, DateTime periodoAte,
        Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, Nullable<int> terDe, Nullable<int> terAte, ref DataTable tb, string empresas)
    {
        erros = new List<string>();

        if (periodoDe == null)
            erros.Add("Período não informado ou é inválido.");

        if (periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (contaDe == null)
            erros.Add("Informe apartir de qual conta deve ser gerado o relatório.");

        if (contaAte == null)
            erros.Add("Informe até qual conta deve ser gerado o relatório.");

        if (erros.Count == 0)
        {
            DateTime periodoAnterior = periodoDe.AddDays(-1);

            relatorioDAO.razaoContabil(tipoMoeda, contaDe, contaAte, periodoAnterior, periodoDe, periodoAte, divisaoDe,
                divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, terDe, terAte, ref tb, empresas);
        }

        return erros;
    }




    public List<string> razaoContabilsintetico(int codMoeda,string contaDe, string contaAte, DateTime periodoDe, DateTime periodoAte,
        Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, Nullable<int> terDe, Nullable<int> terAte, ref DataTable tb, string empresas)
    {
        erros = new List<string>();

        if (periodoDe == null)
            erros.Add("Período não informado ou é inválido.");

        if (periodoAte == null)
            erros.Add("Período não informado ou é inválido.");

        if (contaDe == null)
            erros.Add("Informe apartir de qual conta deve ser gerado o relatório.");

        if (contaAte == null)
            erros.Add("Informe até qual conta deve ser gerado o relatório.");

        if (erros.Count == 0)
        {
            DateTime periodoAnterior = periodoDe.AddDays(-1);

            relatorioDAO.razaoContabil(codMoeda, contaDe, contaAte, periodoAnterior, periodoDe, periodoAte, divisaoDe,
                divisaoAte, linhaNegocioDe, linhaNegocioAte, clienteDe, clienteAte, jobDe, jobAte, terDe, terAte, ref tb, empresas);
        }

        return erros;
    }
}
