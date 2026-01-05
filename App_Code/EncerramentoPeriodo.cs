using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for EncerramentoPeriodo
/// </summary>
public class EncerramentoPeriodo
{
    private controleEncerramentoDAO DAO;
    private lanctosContabDAO lanctoContabDAO;
    private Conexao conexao = new Conexao();

	public EncerramentoPeriodo(Conexao c)
	{
        conexao = c;
        DAO = new controleEncerramentoDAO(c);
        lanctoContabDAO = new lanctosContabDAO(c);
	}

    public void listaPaginada(ref DataTable tb, int paginaAtual, string ordenacao)
    {
        DAO.lista(ref tb, paginaAtual, ordenacao);
    }

    public int totalRegistros()
    {
        return DAO.totalRegistros();
    }

    public bool deletaEncerramento(int codigoEncerramento)
    {
        try
        {
            DAO.deleteLanctos(codigoEncerramento);
            DAO.delete(codigoEncerramento);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool encerraPeriodo(DateTime periodoInicio, DateTime periodoTermino,
        string contaApuracao, int codJobApuracao, int codLinhaNegocioApuracao, int codDivisaoApuracao,
        int codClienteApuracao, string encerramento)
    {

        DateTime inicioTemp = periodoInicio;
        FolhaLancamento folha = new FolhaLancamento(conexao);
        while (inicioTemp <= periodoTermino)
        {
            if (folha.verificaPeriodoFechamento(inicioTemp))
            {
                throw new ApplicationException("Existem meses que ainda estão abertos no período escolhido, por favor verifique.");
            }
            inicioTemp = inicioTemp.AddMonths(1);
        }

        DateTime periodoAnterior = new DateTime(periodoInicio.Year, periodoInicio.Month, 1);
        periodoAnterior = periodoAnterior.AddDays(-1);
        DateTime primeiraData = lanctoContabDAO.dataPrimeiroLancamento();
        primeiraData = Convert.ToDateTime("01/" + primeiraData.ToString("MM/yyyy"));

        //Evandro depois olhar aqui
        //if (periodoInicio < primeiraData)
        //    throw new ApplicationException("Não existem lançamentos nesse período.");

        if (periodoAnterior > primeiraData)
        {
            if (!DAO.verificaEncerramento(periodoAnterior))
                throw new ApplicationException("O perído "+primeiraData.ToString("dd/MM/yyyy")+" até "+periodoAnterior.ToString("dd/MM/yyyy") + " precisa estar encerrado, por favor verifique.");

        }

        List<SLancamento> folhaEncerramento = new List<SLancamento>();
        DataTable lanctosOriginais = lanctoContabDAO.listaLancamentosEncerramento(periodoInicio, periodoTermino);
        
        decimal total = 0;


        // Inicio do encerramento moeda Padrão
        int seqLote = 1;
        char db = 'D';
        for (int i = 0; i < lanctosOriginais.Rows.Count; i++)
        {
            DataRow row = lanctosOriginais.Rows[i];
            if (Convert.ToDecimal(row["valor"]) < 0)
            {
                db = 'D';
            }
            else
            {
                db = 'C';
            }

            SLancamento l = new SLancamento(0, seqLote, 1, 0, 0, 0, 0, 0, db, periodoTermino,
                row["cod_conta"].ToString(), row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]),
                row["desc_job"].ToString(), Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(),
                Convert.ToInt32(row["cod_divisao"]), row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]),
                row["desc_cliente"].ToString(), 1, (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"])),
                (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"])),
                encerramento, false, new List<SVencimento>(), 0, "", false, 0, "", "E", row["DESC_CONSULTOR"].ToString(), Convert.ToInt32(row["COD_CONSULTOR"]), 10, 0, 0);

            folhaEncerramento.Add(l);
            //Evandro depois olhar aqui
            //total += (db == 'D' ? Convert.ToDecimal(row["valor"]) : -Convert.ToDecimal(row["valor"]));
            total -= Convert.ToDecimal(row["valor"]);
            seqLote++;
        }

        if (total < 0)
        {
            db = 'D';
        }
        else
        {
            db = 'C';
        }

        ContaContabil conta = new ContaContabil(conexao);
        conta.codigo = contaApuracao;
        conta.load();

        Job job = new Job(conexao);
        job.codigo = codJobApuracao;
        job.load();

        LinhaNegocio linhaNegocio = new LinhaNegocio(conexao);
        linhaNegocio.codigo = codLinhaNegocioApuracao;
        linhaNegocio.load();

        Divisao divisao = new Divisao(conexao);
        divisao.codigo = codDivisaoApuracao;
        divisao.load();

        Cliente cliente = new Cliente(conexao);
        cliente.codigo = codClienteApuracao;
        cliente.load();

        SLancamento u = new SLancamento(0, seqLote, 1, 0, 0, 0, 0, 0, db, periodoTermino,
                contaApuracao, conta.descricao, codJobApuracao,
                job.descricao, codLinhaNegocioApuracao, linhaNegocio.descricao,
                codDivisaoApuracao, divisao.descricao, codClienteApuracao,
                cliente.nome, 1, (total < 0 ? total * -1 : total), (total < 0 ? total * -1 : total),
                encerramento, false, new List<SVencimento>(), 0, "", false, 0, "", "E", "", 0, 30, 0, 0);

        folhaEncerramento.Add(u);
        double lote = lanctoContabDAO.getNewNumeroLote();
        DAO.insert(periodoTermino, lote);
        for (int i = 0; i < folhaEncerramento.Count; i++)
        {
            SLancamento l = folhaEncerramento[i];
            lanctoContabDAO.insert(lote, l.seqLote, l.detLote.Value, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value,
                l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value, l.conta, l.descConta, l.job.Value,
                l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao, l.cliente.Value,
                l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro,
                l.pendente.Value, "C_INCLUSAO_LANCTO", l.modelo.Value, l.numeroDocumento, "E", "", 0, l.codPlanilha,0);
        }

        // Fim do encerramento moeda Padrão
        // Inicio do encerramento moeda outras moedas

        //int seqLote = 1;
        //Tratamento de Encerramento em outra Moeda
        //Variáveis de encerramento em outra moeda
        folhaEncerramento = new List<SLancamento>();
        int CodMoeda = 0;
        DataTable lanctosOriginaisMoeda = lanctoContabDAO.listaLancamentosEncerramentoMoeda(periodoInicio, periodoTermino);
        if (lanctosOriginaisMoeda.Rows.Count > 0)
        {
            CodMoeda = Convert.ToInt32(lanctosOriginaisMoeda.Rows[0]["Cod_moeda"]);
        }
        decimal TotalMoeda = 0;
        int SeqMoeda = 0;

        db = 'D';
        for (int i = 0; i < lanctosOriginaisMoeda.Rows.Count; i++)
        {
            DataRow row = lanctosOriginaisMoeda.Rows[i];
            if (Convert.ToDecimal(row["valor"]) < 0)
            {
                db = 'D';
            }
            else
            {
                db = 'C';
            }
            if (CodMoeda != Convert.ToInt32(row["cod_moeda"]))
            {
                ///adiciona dados quando trocar a moeda
                if (TotalMoeda > 0)
                {
                    db = 'D';
                }
                else
                {
                    db = 'C';
                }

                conta = new ContaContabil(conexao);
                conta.codigo = contaApuracao;
                conta.load();

                job = new Job(conexao);
                job.codigo = codJobApuracao;
                job.load();

                linhaNegocio = new LinhaNegocio(conexao);
                linhaNegocio.codigo = codLinhaNegocioApuracao;
                linhaNegocio.load();

                divisao = new Divisao(conexao);
                divisao.codigo = codDivisaoApuracao;
                divisao.load();

                cliente = new Cliente(conexao);
                cliente.codigo = codClienteApuracao;
                cliente.load();

                u = new SLancamento(0, seqLote, 1, 0, 0, 0, 0, 0, db, periodoTermino,
                        contaApuracao, conta.descricao, codJobApuracao,
                        job.descricao, codLinhaNegocioApuracao, linhaNegocio.descricao,
                        codDivisaoApuracao, divisao.descricao, codClienteApuracao,
                        cliente.nome, 1, (TotalMoeda < 0 ? TotalMoeda * -1 : TotalMoeda), (TotalMoeda < 0 ? TotalMoeda * -1 : TotalMoeda),
                        encerramento, false, new List<SVencimento>(), 0, "", false, 0, "", "E", "", 0, 30, 0, 0);

                folhaEncerramento.Add(u);

                for (int j = 0; j < folhaEncerramento.Count; j++)
                {
                    SLancamento w = folhaEncerramento[j];
                    lanctoContabDAO.insertMoeda(lote, w.seqLote, w.detLote.Value, CodMoeda, w.lotePai.Value, w.seqLotePai.Value, w.duplicata.Value,
                        w.seqBaixa.Value, w.detBaixa.Value, w.debCred.Value, w.dataLancamento.Value, w.conta, w.descConta, w.job.Value,
                        w.descJob, w.linhaNegocio.Value, w.descLinhaNegocio, w.divisao.Value, w.descDivisao, w.cliente.Value,
                        w.descCliente, w.qtd.Value, w.valorUnit.Value, w.valor.Value, w.historico, w.titulo.Value, w.terceiro.Value, w.descTerceiro,
                        w.pendente.Value, "C_INCLUSAO_LANCTO", w.modelo.Value, w.numeroDocumento, "", 0, 0, w.codPlanilha, 0, "E");
                }
                SeqMoeda = 0;
                CodMoeda = Convert.ToInt32(row["Cod_moeda"]);
                TotalMoeda = 0;
                folhaEncerramento = new List<SLancamento>();

                /// Fim da adição de dados

            }

            SeqMoeda = SeqMoeda + 1;
            TotalMoeda = TotalMoeda + Convert.ToDecimal(row["valor"]);

            SLancamento l = new SLancamento(0, SeqMoeda, 1, 0, 0, 0, 0, 0, db, periodoTermino,
                row["cod_conta"].ToString(), row["desc_conta"].ToString(), Convert.ToInt32(row["cod_job"]),
                row["desc_job"].ToString(), Convert.ToInt32(row["cod_linha_negocio"]), row["desc_linha_negocio"].ToString(),
                Convert.ToInt32(row["cod_divisao"]), row["desc_divisao"].ToString(), Convert.ToInt32(row["cod_cliente"]),
                row["desc_cliente"].ToString(), 1, (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"])),
                (Convert.ToDecimal(row["valor"]) < 0 ? Convert.ToDecimal(row["valor"]) * -1 : Convert.ToDecimal(row["valor"])),
                encerramento, false, new List<SVencimento>(), 0, "", false, 0, "", "E", row["DESC_CONSULTOR"].ToString(), Convert.ToInt32(row["COD_CONSULTOR"]), 10, 0, 0);

            folhaEncerramento.Add(l);
            //Evandro depois olhar aqui
            //total += (db == 'D' ? Convert.ToDecimal(row["valor"]) : -Convert.ToDecimal(row["valor"]));
        }

        if (TotalMoeda > 0)
        {
            db = 'D';
        }
        else
        {
            db = 'C';
        }

        conta = new ContaContabil(conexao);
        conta.codigo = contaApuracao;
        conta.load();

        job = new Job(conexao);
        job.codigo = codJobApuracao;
        job.load();

        linhaNegocio = new LinhaNegocio(conexao);
        linhaNegocio.codigo = codLinhaNegocioApuracao;
        linhaNegocio.load();

        divisao = new Divisao(conexao);
        divisao.codigo = codDivisaoApuracao;
        divisao.load();

        cliente = new Cliente(conexao);
        cliente.codigo = codClienteApuracao;
        cliente.load();
        SeqMoeda++;
        u = new SLancamento(0, SeqMoeda, 1, 0, 0, 0, 0, 0, db, periodoTermino,
                contaApuracao, conta.descricao, codJobApuracao,
                job.descricao, codLinhaNegocioApuracao, linhaNegocio.descricao,
                codDivisaoApuracao, divisao.descricao, codClienteApuracao,
                cliente.nome, 1, (TotalMoeda < 0 ? TotalMoeda * -1 : TotalMoeda), (TotalMoeda < 0 ? TotalMoeda * -1 : TotalMoeda),
                encerramento, false, new List<SVencimento>(), 0, "", false, 0, "", "E", "", 0, 30, 0, 0);

        folhaEncerramento.Add(u);

        //DAO.insert(periodoTermino, lote);
        for (int i = 0; i < folhaEncerramento.Count; i++)
        {
            SLancamento l = folhaEncerramento[i];
            lanctoContabDAO.insertMoeda(lote, l.seqLote, l.detLote.Value, CodMoeda, l.lotePai.Value, l.seqLotePai.Value, l.duplicata.Value,
                l.seqBaixa.Value, l.detBaixa.Value, l.debCred.Value, l.dataLancamento.Value, l.conta, l.descConta, l.job.Value,
                l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio, l.divisao.Value, l.descDivisao, l.cliente.Value,
                l.descCliente, l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.terceiro.Value, l.descTerceiro,
                l.pendente.Value, "C_INCLUSAO_LANCTO", l.modelo.Value, l.numeroDocumento,  "", 0, l.codPlanilha,0, 0, "E");
        }



        // Fim do encerramento moeda outras moedas
        return true;

    }
}
