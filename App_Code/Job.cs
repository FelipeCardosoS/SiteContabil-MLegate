using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public class Job
{
    private int _codigo;
    private int _cliente;
    private string _clienteDesc;
    private int _linhaNegocio;
    private string _linhaNegocioDesc;
    private int _divisao;
    private string _divisaoDesc;
    private string _nome;
    private string _descricao;
    private string _obsNF;
    private char _status;
    private string _cod_referencia;
    private List<GestorJob> _listaGestores;
    private List<ConsultorJob> _listaConsultores;
    private List<DespesaJob> _listaDespesas;
    private int _codProjeto;
    private int _codFaturamento;

    private int _Medico;            //Medical Prime
    private int _Equipe;            //Medical Prime
    private int _Vendedor;          //Medical Prime
    private string _Paciente;       //Medical Prime
    private string _Convenio;       //Medical Prime

    public Conexao _c;
    private jobsDAO jobDAO;
    private empresasDAO empresaDAO;
    private linhasNegocioDAO linhaNegocioDAO;
    private divisoesDAO divisaoDAO;
    private List<string> erros;
    private GestorJob gestorJob;
    private ConsultorJob consultorJob;
    private DespesaJob despesaJob;

    public Job() { }

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public int cliente
    {
        get { return _cliente; }
        set { _cliente = value; }
    }

    public string clienteDesc
    {
        get { return _clienteDesc; }
        set { _clienteDesc = value; }
    }

    public int linhaNegocio
    {
        get { return _linhaNegocio; }
        set { _linhaNegocio = value; }
    }

    public string linhaNegocioDesc
    {
        get { return _linhaNegocioDesc; }
        set { _linhaNegocioDesc = value; }
    }

    public int divisao
    {
        get { return _divisao; }
        set { _divisao = value; }
    }

    public string divisaoDesc
    {
        get { return _divisaoDesc; }
        set { _divisaoDesc = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public string obsNF
    {
        get { return _obsNF; }
        set { _obsNF = value; }
    }

    public char status
    {
        get { return _status; }
        set { _status = value; }
    }

    public string cod_referencia
    {
        get { return _cod_referencia; }
        set { _cod_referencia = value; }
    }

    public int Medico //Medical Prime
    {
        get { return _Medico; }
        set { _Medico = value; }
    }

    public int Equipe //Medical Prime
    {
        get { return _Equipe; }
        set { _Equipe = value; }
    }

    public int Vendedor //Medical Prime
    {
        get { return _Vendedor; }
        set { _Vendedor = value; }
    }

    public string Paciente
    {
        get { return _Paciente; }
        set { _Paciente = value; }
    }

    public string Convenio //Medical Prime
    {
        get { return _Convenio; }
        set { _Convenio = value; }
    }

    public List<GestorJob> Gestores
	{
        get { return _listaGestores; }
        set { _listaGestores = value; }
    }

    public List<ConsultorJob> Consultores
    {
        get { return _listaConsultores; }
        set { _listaConsultores = value; }
    }

    public List<DespesaJob> Despesas
    {
        get { return _listaDespesas; }
        set { _listaDespesas = value; }
    }

    public int CodProjeto
    {
        get { return _codProjeto; }
        set { _codProjeto = value; }
    }

    public int CodFaturamento
    {
        get { return _codFaturamento; }
        set { _codFaturamento = value; }
    }

    public Job(Conexao c)
    {
        _c = c;
        jobDAO = new jobsDAO(c);
        empresaDAO = new empresasDAO(c);
        linhaNegocioDAO = new linhasNegocioDAO(c);
        divisaoDAO = new divisoesDAO(c);
        gestorJob = new GestorJob(c);
        consultorJob = new ConsultorJob(c);
        despesaJob = new DespesaJob(c);
        
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");

        if (_cliente == 0)
            erros.Add("Escolha um cliente.");

        if (_linhaNegocio == 0)
            erros.Add("Escolha uma linha de negócio.");

        if (_divisao == 0)
            erros.Add("Escolha uma divisão.");

        erros.AddRange(gestorJob.valida(_listaGestores));
        erros.AddRange(consultorJob.valida(_listaConsultores));
        erros.AddRange(despesaJob.valida(_listaDespesas));

        if (erros.Count == 0)
        {
            _codigo = jobDAO.insert(_cliente, _linhaNegocio, _divisao, _nome, _descricao, _obsNF, 'A', 0, DateTime.Now.ToString("yyyyMMdd"),
                DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"),
                'A', 0, _Medico, _Equipe, _Vendedor, _Paciente, _Convenio);

            gestorJob.salva(_codigo, _listaGestores);
            consultorJob.salva(_codigo, _listaConsultores);
            despesaJob.salva(_codigo, _listaDespesas);
        }

        return erros;
    }

    public List<string> novoList(List<Job> listJobs)
    {
        erros = new List<string>();

        foreach (Job _jobs in listJobs)
        {
            if (_jobs._nome == "" || _jobs._nome == null)
                erros.Add("Existe um Job da sua lista que está sem nome, por favor informe!");

            if (_jobs._cliente == 0)
                erros.Add("Escolha um cliente para o Job : " + _jobs._nome);

            if (_jobs._linhaNegocio == 0)
                erros.Add("Escolha uma linha de negócio para o Job : " + _jobs._nome);

            if (_jobs._divisao == 0)
                erros.Add("Escolha uma divisão para o Job : " + _jobs._nome);


            erros.AddRange(gestorJob.valida(_listaGestores));
            erros.AddRange(consultorJob.valida(_listaConsultores));
            erros.AddRange(despesaJob.valida(_listaDespesas));
        }


        if (erros.Count == 0)
        {
            foreach (Job _jobs in listJobs)
            {
               int codigo = jobDAO.insert(_jobs._cliente, _jobs._linhaNegocio, _jobs._divisao, _jobs._nome, _jobs._descricao, _jobs._obsNF, 'A', 0, DateTime.Now.ToString("yyyyMMdd"),
                    DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"),
                    'A', Convert.ToInt32(_jobs.cod_referencia), _Medico, _Equipe, _Vendedor, _Paciente, _Convenio);

                gestorJob.salva(codigo, _jobs.Gestores);
                consultorJob.salva(codigo, _jobs.Consultores);
                despesaJob.salva(codigo, _jobs.Despesas);
            }
        }

        return erros;
    }

    public List<string> alterar(List<int> listaGestoresDeletar = null, List<int> listaConsultoresDeletar = null, List<int> listaDespesasDeletar = null)
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");

        if (_cliente == 0)
            erros.Add("Escolha um cliente.");

        if (_linhaNegocio == 0)
            erros.Add("Escolha uma linha de negócio.");

        if (_divisao == 0)
            erros.Add("Escolha uma divisão.");

        erros.AddRange(gestorJob.valida(_listaGestores));
        erros.AddRange(consultorJob.valida(_listaConsultores));
        erros.AddRange(despesaJob.valida(_listaDespesas));

        if (erros.Count == 0)
        {
            jobDAO.update(_codigo, _cliente, _linhaNegocio, _divisao, _nome, _descricao, _obsNF, 'A', 0, DateTime.Now.ToString("yyyyMMdd"),
                DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"),
                'A', 0, false, _Medico, _Equipe, _Vendedor, _Paciente, _Convenio);

            gestorJob.salva(_codigo, _listaGestores, listaGestoresDeletar);
            consultorJob.salva(_codigo, _listaConsultores, listaConsultoresDeletar);
            despesaJob.salva(_codigo, _listaDespesas, listaDespesasDeletar);
        }

        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (erros.Count == 0)
        {
            gestorJob.deleta(_codigo);
            consultorJob.deleta(_codigo);
            despesaJob.deleta(_codigo);
            jobDAO.delete(_codigo);
        }

        return erros;
    }

    public void load()
    {
        DataTable linha = jobDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
			if (linha.Rows[0]["COD_CLIENTE"] != DBNull.Value)
				_cliente = Convert.ToInt32(linha.Rows[0]["COD_CLIENTE"]);

			if (linha.Rows[0]["DESCRICAO_CLIENTE"] != DBNull.Value)
				_clienteDesc = linha.Rows[0]["DESCRICAO_CLIENTE"].ToString();

			if (linha.Rows[0]["COD_LINHA_NEGOCIO"] != DBNull.Value)
				_linhaNegocio = Convert.ToInt32(linha.Rows[0]["COD_LINHA_NEGOCIO"]);

			if (linha.Rows[0]["DESCRICAO_LINHA_NEGOCIOS"] != DBNull.Value)
				_linhaNegocioDesc = linha.Rows[0]["DESCRICAO_LINHA_NEGOCIOS"].ToString();

			if (linha.Rows[0]["COD_DIVISAO"] != DBNull.Value)
				_divisao = Convert.ToInt32(linha.Rows[0]["COD_DIVISAO"]);

			if (linha.Rows[0]["DESCRICAO_DIVISAO"] != DBNull.Value)
				_divisaoDesc = linha.Rows[0]["DESCRICAO_DIVISAO"].ToString();

			if (linha.Rows[0]["NOME"] != DBNull.Value)
				_nome = linha.Rows[0]["NOME"].ToString();

			if (linha.Rows[0]["DESCRICAO"] != DBNull.Value)
				_descricao = linha.Rows[0]["DESCRICAO"].ToString();

            if (linha.Rows[0]["OBSERVACAO_NOTA_FISCAL"] != DBNull.Value)
                _obsNF = linha.Rows[0]["OBSERVACAO_NOTA_FISCAL"].ToString();

            if (linha.Rows[0]["STATUS"] != DBNull.Value)
				_status = Convert.ToChar(linha.Rows[0]["STATUS"]);

			if (linha.Rows[0]["cod_medico"] != DBNull.Value)
				_Medico = Convert.ToInt32(linha.Rows[0]["cod_medico"]);

			if (linha.Rows[0]["cod_equipe"] != DBNull.Value)
				_Equipe = Convert.ToInt32(linha.Rows[0]["cod_equipe"]);

			if (linha.Rows[0]["cod_vendedor"] != DBNull.Value)
				_Vendedor = Convert.ToInt32(linha.Rows[0]["cod_vendedor"]);

			if (linha.Rows[0]["paciente"] != DBNull.Value)
				_Paciente = linha.Rows[0]["paciente"].ToString();

			if (linha.Rows[0]["convenio"] != DBNull.Value)
				_Convenio = linha.Rows[0]["convenio"].ToString();

            _listaGestores = gestorJob.lista(_codigo);
            _listaConsultores = consultorJob.lista(_codigo);
            _listaDespesas = despesaJob.lista(_codigo);
		}
    }

    public List<Job> loadSincroniza() 
    {
        DataTable linha = jobDAO.sincronizaJobLinhaNegocioData();
        List<Job> list = new List<Job>();

        foreach (DataRow row in linha.Rows)
        {
            Job job = new Job(_c);
            job._cliente = Convert.ToInt32(row["COD_CLIENTE"]);
            job._linhaNegocio = Convert.ToInt32(row["COD_LINHA_NEGOCIO"]);
            job._divisao = Convert.ToInt32(row["COD_DIVISAO"]);
            job._nome = row["NOME"].ToString();
            job._descricao = row["DESCRICAO"].ToString();
            job._cod_referencia = row["codjob"].ToString();
            list.Add(job);
        }

        return list;
    }

    public void lista(ref DataTable tb)
    {
        jobDAO.lista(ref tb);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        jobDAO.lista(ref tb, colunaOrdenar);
    }

    public void lista(ref DataTable tb, char status)
    {
        jobDAO.lista(ref tb, status);
    }

    public void listaFiltroRelatorio(ref DataTable tb)
    {
        jobDAO.listaFiltroRelatorio(ref tb);
    }

    public void lista(ref DataTable tb, char status, string colunaOrdenar)
    {
        jobDAO.lista(ref tb, status, colunaOrdenar);
    }

    public List<Hashtable> lista()
    {
        return jobDAO.lista();
    }

    public DataTable listaDataTable()
    {
        return jobDAO.listaDataTable();
    }

    public List<Hashtable> lista(char status)
    {
        return jobDAO.lista(status);
    }

    public int totalRegistros(Nullable<int> cliente, string nome, string descricao)
    {
        return jobDAO.totalRegistros(cliente, nome, descricao);
    }

    public void listaPaginada(ref DataTable tb, Nullable<int> cliente, string nome, string descricao, int paginaAtual, string ordenacao)
    {
        jobDAO.lista(ref tb, cliente, nome, descricao, paginaAtual, ordenacao);
    }

    public int totalClassificacao()
    {
        return jobDAO.sincronizaJobLinhaNegocio();
    }

    public void dePara(int codigoTs)
    {
        DataTable depara = jobDAO.dePara(codigoTs);
        if (depara.Rows.Count > 0)
        {
            DataRow row = depara.Rows[0];
            _codigo = Convert.ToInt32(row["cod_job"]);
            _cliente = Convert.ToInt32(row["cod_cliente"]);
            _linhaNegocio = Convert.ToInt32(row["cod_linha_negocio"]);
            _divisao = Convert.ToInt32(row["cod_divisao"]);
            _descricao = row["descricao"].ToString();
        }
        else
        {
            throw new Exception("Job " + codigoTs + " não encontrado.");
        }
    }

    public List<string> sincroniza()
    {
        erros = new List<string>();
        DataSet dsTimesheet = new DataSet("dsTimesheet");
        DataTable tbLocal = new DataTable("tbLocal");
        int codigoDivisaoLocal = 0;
        try
        {

            jobDAO.listaTimesheet(ref dsTimesheet);
            jobDAO.listaSincronizacao(ref tbLocal);
            int count = tbLocal.Rows.Count;
            int count2 = dsTimesheet.Tables[0].Rows.Count;

     
            //var results = from myRow in tbLocal.AsEnumerable()
            //              where myRow.Field<int>("cod_referencia") == 1750
            //              select myRow;

         

            for (int i = 0; i < count2; i++)
            {

               //var results = from myRow in tbLocal.AsEnumerable()
               //               where myRow.Field<int>("cod_referencia") == 1750
               //               select myRow;

                bool existe = false;
                codigoDivisaoLocal = 0;


                //for (int x = 0; x < count; x++)
                //{
                //    if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"]))
                //    {
                //        existe = true;
                //        codigoDivisaoLocal = Convert.ToInt32(tbLocal.Rows[x]["COD_JOB"]);
                //        break;
                //    }
                //}

                Int32 cod_ref = Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]);
                Int32 inicio = 0;
                Int32 Fim = count -1;
                Int32 Posicao = Fim * cod_ref / Convert.ToInt32(tbLocal.Rows[Fim]["COD_REFERENCIA"]);
                if (Posicao > Fim)
                {
                    Posicao = Fim;
                }
                bool faz = true;
                while (faz)
                {
                    if (Convert.ToInt32(tbLocal.Rows[Fim]["COD_REFERENCIA"]) == cod_ref )
                    {
                        faz = false;
                        existe = true;
                    }
                    if (Convert.ToInt32(tbLocal.Rows[inicio]["COD_REFERENCIA"]) == cod_ref)
                    {
                        faz = false;
                        existe = true;
                    }
                    if (Convert.ToInt32(tbLocal.Rows[Posicao]["COD_REFERENCIA"]) == cod_ref)
                    {
                        faz = false;
                        existe = true;
                    }
                    if (faz)
                    {
                        if (inicio == Fim || inicio + 1 == Fim)
                        {
                            faz = false;
                        }
                        if (Convert.ToInt32(tbLocal.Rows[Posicao]["COD_REFERENCIA"]) < cod_ref)
                        {
                            inicio = Posicao;
                        }
                        else
                        {
                            Fim = Posicao;
                        }
                        if (Posicao == Fim)
                        {
                            Posicao = Fim * cod_ref / Convert.ToInt32(tbLocal.Rows[Fim]["COD_REFERENCIA"]);
                            if (Posicao >= Fim)
                            {
                                Posicao = inicio + (Fim - inicio) / 2;
                            }
                        }
                        else
                        {
                            Posicao = inicio + (Fim - inicio) / 2;
                        }
                    }
                }

                if (!existe && jobDAO.verificaDivisao(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"])))
                {
                    try
                    {
                        jobDAO.insert(empresaDAO.getCodigoClienteXTimeSheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodCliente"])),
                            linhaNegocioDAO.getCodigoXTimesheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodProjeto"])),
                            divisaoDAO.getCodigoXTimesheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"])),
                            dsTimesheet.Tables[0].Rows[i]["NomeJob"].ToString(), dsTimesheet.Tables[0].Rows[i]["DescJob"].ToString(), null,
                            Convert.ToChar(dsTimesheet.Tables[0].Rows[i]["StatusJob"]), Convert.ToDecimal(dsTimesheet.Tables[0].Rows[i]["ValorFaturamento"]),
                            Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataProposta"]).ToString("yyyyMMdd"), Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataResposta"]).ToString("yyyyMMdd"),
                            Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataInicio"]).ToString("yyyyMMdd"), Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataFim"]).ToString("yyyyMMdd"),
                            Convert.ToChar(dsTimesheet.Tables[0].Rows[i]["StatusResposta"]), Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]),
                            0,0,0,"","");
                    }
                    catch (Exception ex)
                    {
                        erros.Add("Erro na inserção do job:" + codigoDivisaoLocal + "  " + ex.Message);
                    }
                }
                else
                {
                    bool igual = false;
                    //erros.Add("Sincronizando Job:" + Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]).ToString());
                    //for (int x = 0; x < tbLocal.Rows.Count; x++)
                    //{

                    //    if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"])
                    //        && dsTimesheet.Tables[0].Rows[i]["NomeJob"].ToString() == tbLocal.Rows[x]["NOME"].ToString()
                    //        && dsTimesheet.Tables[0].Rows[i]["Descjob"].ToString() == tbLocal.Rows[x]["DESCRICAO"].ToString()
                    //        && dsTimesheet.Tables[0].Rows[i]["StatusJob"].ToString() == tbLocal.Rows[x]["STATUS"].ToString()
                    //        && dsTimesheet.Tables[0].Rows[i]["ValorFaturamento"].ToString() == tbLocal.Rows[x]["VALOR_FATURAMENTO"].ToString()
                    //        && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataProposta"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[x]["DATA_PROPOSTA"].ToString()).ToString("yyyyMMdd")
                    //        && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataResposta"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[x]["DATA_RESPOSTA"].ToString()).ToString("yyyyMMdd")
                    //        && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataInicio"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[x]["DATA_INICIO"].ToString()).ToString("yyyyMMdd")
                    //        && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataFim"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[x]["DATA_FIM"].ToString()).ToString("yyyyMMdd")
                    //        && dsTimesheet.Tables[0].Rows[i]["StatusResposta"].ToString() == tbLocal.Rows[x]["STATUS_RESPOSTA"].ToString())
                    //    {
                    //        igual = true;
                    //        break;
                    //    }
                    //}
                    if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]) == Convert.ToInt32(tbLocal.Rows[Posicao]["COD_REFERENCIA"]))
                    {
                        if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]) == Convert.ToInt32(tbLocal.Rows[Posicao]["COD_REFERENCIA"])
                            && dsTimesheet.Tables[0].Rows[i]["NomeJob"].ToString() == tbLocal.Rows[Posicao]["NOME"].ToString()
                            && dsTimesheet.Tables[0].Rows[i]["Descjob"].ToString() == tbLocal.Rows[Posicao]["DESCRICAO"].ToString()
                            && dsTimesheet.Tables[0].Rows[i]["StatusJob"].ToString() == tbLocal.Rows[Posicao]["STATUS"].ToString()
                            && dsTimesheet.Tables[0].Rows[i]["ValorFaturamento"].ToString() == tbLocal.Rows[Posicao]["VALOR_FATURAMENTO"].ToString()
                            && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataProposta"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[Posicao]["DATA_PROPOSTA"].ToString()).ToString("yyyyMMdd")
                            && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataResposta"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[Posicao]["DATA_RESPOSTA"].ToString()).ToString("yyyyMMdd")
                            && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataInicio"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[Posicao]["DATA_INICIO"].ToString()).ToString("yyyyMMdd")
                            && Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataFim"]).ToString("yyyyMMdd") == Convert.ToDateTime(tbLocal.Rows[Posicao]["DATA_FIM"].ToString()).ToString("yyyyMMdd")
                            && dsTimesheet.Tables[0].Rows[i]["StatusResposta"].ToString() == tbLocal.Rows[Posicao]["STATUS_RESPOSTA"].ToString())
                        {
                            igual = true;
                        }

                        if (!igual)
                        {
                            try
                            {
                                jobDAO.update(codigoDivisaoLocal, empresaDAO.getCodigoClienteXTimeSheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodCliente"])),
                                    linhaNegocioDAO.getCodigoXTimesheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodProjeto"])),
                                    divisaoDAO.getCodigoXTimesheet(Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"])),
                                    dsTimesheet.Tables[0].Rows[i]["NomeJob"].ToString(), dsTimesheet.Tables[0].Rows[i]["DescJob"].ToString(), null,
                                    Convert.ToChar(dsTimesheet.Tables[0].Rows[i]["StatusJob"]), Convert.ToDecimal(dsTimesheet.Tables[0].Rows[i]["ValorFaturamento"]),
                                    Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataProposta"].ToString()).ToString("yyyyMMdd"), Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataResposta"].ToString()).ToString("yyyyMMdd"),
                                    Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataInicio"].ToString()).ToString("yyyyMMdd"), Convert.ToDateTime(dsTimesheet.Tables[0].Rows[i]["DataFim"].ToString()).ToString("yyyyMMdd"),
                                    Convert.ToChar(dsTimesheet.Tables[0].Rows[i]["StatusResposta"]), Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodJob"]), true,
                                    0, 0, 0, "", "");
                            }
                            catch (Exception ex)
                            {
                                erros.Add("Erro na alteração do job:" + codigoDivisaoLocal + "  " + ex.Message);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            erros.Add("Erro na alteração do job:" + codigoDivisaoLocal + "  " + ex.Message);
        }

        return erros;
    }

    //public void lista_Jobs(ref DataTable tb)
    //{
    //    jobDAO.lista_Jobs(ref tb);
    //}

    public void lista_Jobs_Tomador(ref DataTable tb, int cod_tomador)
    {
        jobDAO.lista_Jobs_Tomador(ref tb, cod_tomador);
    }
}
